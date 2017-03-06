using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using OSGeo.FDO.Connections;
using OSGeo.FDO.ClientServices;
using OSGeo.FDO.Commands.Schema;
using OSGeo.FDO.Commands.Feature;
using OSGeo.FDO.Expression;
using System.Diagnostics;
using System.Threading;

namespace OSGeo.MapGuide.Services
{
    public class ServerFeatureService : FeatureService.FeatureServiceBase
    {
        private ResourceService.ResourceServiceClient _resSvc;
        readonly ResourcePathResolver _resolver;

        public ServerFeatureService(ResourcePathResolver resolver)
        {
            _resolver = resolver;
        }

        public void InitClientDependencies(Channel channel)
        {
            _resSvc = new ResourceService.ResourceServiceClient(channel);
        }

        const string DATA_PATH_TOKEN = "%MG_DATA_FILE_PATH%";

        private IConnection CreateFdoConnection(ResourceIdentifier resId, MdfModel.FeatureSource fs)
        {
            var connMgr = FeatureAccessManager.GetConnectionManager();
            var conn = connMgr.CreateConnection(fs.Provider);
            var parts = new List<string>();
            foreach (var p in fs.Parameters)
            {
                if (p.Value.Contains(DATA_PATH_TOKEN))
                {
                    var fullPath = _resolver.GetDataPath(resId);
                    parts.Add($"{p.Name}={p.Value.Replace(DATA_PATH_TOKEN, fullPath)}");
                }
                else
                {
                    parts.Add($"{p.Name}={p.Value}");
                }
            }
            conn.ConnectionString = string.Join(";", parts);
            return conn;
        }

        private async Task<MdfModel.FeatureSource> GetFeatureSourceAsync(ResourceIdentifier resId)
        {
            var req = new GetResourceContentRequest
            {
                Resource = resId
            };
            var resource = await _resSvc.GetResourceContentAsync(req);
            if (resource.Error != null)
            {
                throw new Exception(resource.Error.Message);
            }
            else
            {
                if (resource.Result.TypeCase == Resource.TypeOneofCase.FeatureSource)
                {
                    return resource.Result.FeatureSource;
                }
                throw new Exception($"Invalid resource type: {resource.Result.TypeCase}");
            }
        }

        public override async Task<TestConnectionResponse> TestConnection(TestConnectionRequest request, ServerCallContext context)
        {
            var response = new TestConnectionResponse();
            try
            {
                var fs = await GetFeatureSourceAsync(request.FeatureSource);
                var conn = CreateFdoConnection(request.FeatureSource, fs);
                if (conn.Open() == ConnectionState.ConnectionState_Open)
                {
                    conn.Close();
                    response.Result = true;
                }
                else
                {
                    response.Result = false;
                }
            }
            catch (Exception ex)
            {
                response.Error = ErrorUtil.ExceptionToError(ex);
            }
            return response;
        }

        public override async Task<GetSchemasResponse> GetSchemas(GetSchemasRequest request, ServerCallContext context)
        {
            var response = new GetSchemasResponse();
            try
            {
                var fs = await GetFeatureSourceAsync(request.FeatureSource);
                var conn = CreateFdoConnection(request.FeatureSource, fs);
                if (conn.Open() == ConnectionState.ConnectionState_Open)
                {
                    try
                    {
                        IDescribeSchema cmd = (IDescribeSchema)conn.CreateCommand(FDO.Commands.CommandType.CommandType_DescribeSchema);
                        using (var schemas = cmd.Execute())
                        {
                            var result = new StringCollection();
                            result.Items.AddRange(schemas.Cast<OSGeo.FDO.Schema.FeatureSchema>().Select(sc => sc.Name));
                            response.Result = result;
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    throw new Exception($"Failed to open FDO connection");
                }
            }
            catch (Exception ex)
            {
                response.Error = ErrorUtil.ExceptionToError(ex);
            }
            return response;
        }

        public override async Task<GetClassesResponse> GetClasses(GetClassesRequest request, ServerCallContext context)
        {
            var response = new GetClassesResponse();
            try
            {
                var fs = await GetFeatureSourceAsync(request.FeatureSource);
                var conn = CreateFdoConnection(request.FeatureSource, fs);
                if (conn.Open() == ConnectionState.ConnectionState_Open)
                {
                    try
                    {
                        IDescribeSchema cmd = (IDescribeSchema)conn.CreateCommand(FDO.Commands.CommandType.CommandType_DescribeSchema);
                        using (var schemas = cmd.Execute())
                        {
                            var schema = schemas.Cast<OSGeo.FDO.Schema.FeatureSchema>().FirstOrDefault(sc => sc.Name == request.SchemaName);
                            var result = new StringCollection();
                            if (schema != null)
                            {
                                var classes = schema.Classes;
                                result.Items.AddRange(classes.Cast<OSGeo.FDO.Schema.ClassDefinition>().Select(cls => cls.Name));
                            }
                            response.Result = result;
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    throw new Exception($"Failed to open FDO connection");
                }
            }
            catch (Exception ex)
            {
                response.Error = ErrorUtil.ExceptionToError(ex);
            }
            return response;
        }

        public override async Task SelectFeatures(SelectFeaturesRequest request, IServerStreamWriter<FeatureRecord> responseStream, ServerCallContext context)
        {
            try
            {
                var fs = await GetFeatureSourceAsync(request.FeatureSource);
                var conn = CreateFdoConnection(request.FeatureSource, fs);
                if (conn.Open() == ConnectionState.ConnectionState_Open)
                {
                    try
                    {
                        ISelect cmd = (ISelect)conn.CreateCommand(FDO.Commands.CommandType.CommandType_Select);
                        if (!string.IsNullOrEmpty(request.ClassName))
                            cmd.SetFeatureClassName(request.ClassName);
                        if (request.Options != null)
                        {
                            if (!string.IsNullOrEmpty(request.Options.Filter))
                                cmd.SetFilter(request.Options.Filter);

                            if (request.Options.FetchSize > 0)
                                cmd.FetchSize = request.Options.FetchSize;

                            if (request.Options.Properties.Count > 0)
                            {
                                var props = cmd.PropertyNames;
                                foreach (var p in request.Options.Properties)
                                {
                                    switch (p.TypeCase)
                                    {
                                        case QueryProperty.TypeOneofCase.Computed:
                                            var expr = Expression.Parse(p.Computed.Expression);
                                            var comp = new ComputedIdentifier(p.Computed.Alias, expr);
                                            props.Add(comp);
                                            break;
                                        case QueryProperty.TypeOneofCase.Identifier:
                                            var ident = new Identifier(p.Identifier.Name);
                                            props.Add(ident);
                                            break;
                                    }
                                }
                            }

                            if (request.Options.Ordering != null)
                            {
                                var ordering = cmd.Ordering;
                                foreach (var prop in request.Options.Ordering.OrderBy.Items)
                                {
                                    var ident = new Identifier(prop);
                                    ordering.Add(ident);
                                }
                                switch (request.Options.Ordering.Direction)
                                {
                                    case OrderingDirection.Ascending:
                                        cmd.OrderingOption = FDO.Commands.OrderingOption.OrderingOption_Ascending;
                                        break;
                                    case OrderingDirection.Descending:
                                        cmd.OrderingOption = FDO.Commands.OrderingOption.OrderingOption_Descending;
                                        break;
                                }
                            }
                        }

                        using (var reader = cmd.Execute())
                        {
                            var clsDef = reader.GetClassDefinition();
                            var klass = FdoUtils.ConvertFdoClass(clsDef);
                            await responseStream.WriteAsync(new FeatureRecord //First record is either class def or error
                            {
                                Header = klass
                            });
                            var sw = new Stopwatch();
                            sw.Start();
                            try
                            {
                                while (reader.ReadNext())
                                {
                                    var rec = FdoUtils.ConvertFeatureRecord(reader, klass);
                                    await responseStream.WriteAsync(rec);
                                }
                            }
                            finally
                            {
                                sw.Stop();
                                reader.Close();
                                Console.WriteLine($"({Thread.CurrentThread.ManagedThreadId}) - SelectFeatures request processed in {sw.ElapsedMilliseconds}ms");
                            }
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    throw new Exception($"Failed to open FDO connection");
                }
            }
            catch (Exception ex)
            {
                var record = new FeatureRecord //First record is either class def or error
                {
                    Error = ErrorUtil.ExceptionToError(ex)
                };
                await responseStream.WriteAsync(record);
            }
        }

        public override async Task SelectAggregate(SelectAggregateRequest request, IServerStreamWriter<DataRecord> responseStream, ServerCallContext context)
        {
            try
            {
                var fs = await GetFeatureSourceAsync(request.FeatureSource);
                var conn = CreateFdoConnection(request.FeatureSource, fs);
                if (conn.Open() == ConnectionState.ConnectionState_Open)
                {
                    try
                    {
                        ISelectAggregates cmd = (ISelectAggregates)conn.CreateCommand(FDO.Commands.CommandType.CommandType_SelectAggregates);
                        if (!string.IsNullOrEmpty(request.ClassName))
                            cmd.SetFeatureClassName(request.ClassName);
                        if (request.Options != null)
                        {
                            if (!string.IsNullOrEmpty(request.Options.Filter))
                                cmd.SetFilter(request.Options.Filter);

                            if (request.Options.FetchSize > 0)
                                cmd.FetchSize = request.Options.FetchSize;

                            if (request.Options.Properties.Count > 0)
                            {
                                var props = cmd.PropertyNames;
                                foreach (var p in request.Options.Properties)
                                {
                                    switch (p.TypeCase)
                                    {
                                        case QueryProperty.TypeOneofCase.Computed:
                                            var expr = Expression.Parse(p.Computed.Expression);
                                            var comp = new ComputedIdentifier(p.Computed.Alias, expr);
                                            props.Add(comp);
                                            break;
                                        case QueryProperty.TypeOneofCase.Identifier:
                                            var ident = new Identifier(p.Identifier.Name);
                                            props.Add(ident);
                                            break;
                                    }
                                }
                            }

                            if (request.Options.Ordering != null)
                            {
                                var ordering = cmd.Ordering;
                                foreach (var prop in request.Options.Ordering.OrderBy.Items)
                                {
                                    var ident = new Identifier(prop);
                                    ordering.Add(ident);
                                }
                                switch (request.Options.Ordering.Direction)
                                {
                                    case OrderingDirection.Ascending:
                                        cmd.OrderingOption = FDO.Commands.OrderingOption.OrderingOption_Ascending;
                                        break;
                                    case OrderingDirection.Descending:
                                        cmd.OrderingOption = FDO.Commands.OrderingOption.OrderingOption_Descending;
                                        break;
                                }
                            }
                        }

                        using (var reader = cmd.Execute())
                        {
                            var header = FdoUtils.ConvertHeader(reader);
                            await responseStream.WriteAsync(new DataRecord //First record is either class def or error
                            {
                                Header = header
                            });
                            var sw = new Stopwatch();
                            sw.Start();
                            try
                            {
                                while (reader.ReadNext())
                                {
                                    var rec = FdoUtils.ConvertDataRecord(reader, header);
                                    await responseStream.WriteAsync(rec);
                                }
                            }
                            finally
                            {
                                sw.Stop();
                                reader.Close();
                                Console.WriteLine($"({Thread.CurrentThread.ManagedThreadId}) - SelectAggregate request processed in {sw.ElapsedMilliseconds}ms");
                            }
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    throw new Exception($"Failed to open FDO connection");
                }
            }
            catch (Exception ex)
            {
                var record = new DataRecord //First record is either class def or error
                {
                    Error = ErrorUtil.ExceptionToError(ex)
                };
                await responseStream.WriteAsync(record);
            }
        }
    }
}
