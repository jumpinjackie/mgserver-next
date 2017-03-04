using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using OSGeo.FDO.Connections;
using OSGeo.FDO.ClientServices;
using OSGeo.FDO.Commands.Schema;

namespace OSGeo.MapGuide.Services
{
    public class MgServerFeatureService : MgFeatureService.MgFeatureServiceBase
    {
        private MgResourceService.MgResourceServiceClient _resSvc;
        readonly ResourcePathResolver _resolver;

        public MgServerFeatureService(ResourcePathResolver resolver)
        {
            _resolver = resolver;
        }

        public void InitClientDependencies(Channel channel)
        {
            _resSvc = new MgResourceService.MgResourceServiceClient(channel);
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
    }
}
