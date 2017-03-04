using Google.Protobuf;
using Grpc.Core;
using OSGeo.MapGuide;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace DotNetConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = new Channel("127.0.0.1:7000", ChannelCredentials.Insecure);
            var featureSvc = new MgFeatureService.MgFeatureServiceClient(channel);
            var resSvc = new MgResourceService.MgResourceServiceClient(channel);
            var renderSvc = new MgRenderingService.MgRenderingServiceClient(channel);

            Console.WriteLine($"=== {nameof(MgResourceService)} ===");
            ResourceService_ResourceExists(resSvc);
            ResourceService_SetResource(resSvc);
            ResourceService_GetResourceContent(resSvc);

#if DEBUG
            string config = "Debug";
#else
            string config = "Release";
#endif
            var dstPath = Path.GetFullPath($"../../../../Server/mgserver.console/bin/{config}/data/datafiles/Samples/Sheboygan/Data/Parcels.sdf");
            if (!File.Exists(dstPath))
            {
                var dstDir = Path.GetDirectoryName(dstPath);
                if (!Directory.Exists(dstDir))
                {
                    Directory.CreateDirectory(dstDir);
                }
                File.Copy($"../../../Data/Sheboygan_Parcels.sdf", dstPath);
            }

            Console.WriteLine($"=== {nameof(MgFeatureService)} ===");
            FeatureService_TestConnection(featureSvc);
            FeatureService_GetSchemas(featureSvc);
            FeatureService_GetClasses(featureSvc);
            FeatureService_SelectFeatures(featureSvc);
            FeatureService_SelectFeaturesFiltered(featureSvc);
            FeatureService_SelectAggregate(featureSvc);
            FeatureService_SelectAggregateFiltered(featureSvc);

            channel.ShutdownAsync().Wait();
            Console.WriteLine($"Press any key to continue");
            Console.Read();
        }

        static void ResourceService_ResourceExists(MgResourceService.MgResourceServiceClient resSvc)
        {
            Console.WriteLine($"  Testing: {nameof(ResourceService_ResourceExists)}");
            var sw = new Stopwatch();
            sw.Start();
            var response = resSvc.ResourceExists(new ResourceExistsRequest { Resource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource") });
            sw.Stop();
            if (response.Error != null)
                Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message} ({sw.ElapsedMilliseconds}ms)");
            else
                Console.WriteLine($"    Result: {response.Result} ({sw.ElapsedMilliseconds}ms)");
        }

        static void ResourceService_SetResource(MgResourceService.MgResourceServiceClient resSvc)
        {
            Console.WriteLine($"  Testing: {nameof(ResourceService_SetResource)}");
            var fs = new MdfModel.FeatureSource
            {
                Provider = "OSGeo.SDF"
            };
            fs.Parameters.Add(new NameValuePair { Name = "File", Value = "%MG_DATA_FILE_PATH%Parcels.sdf" });
            var req = new SetResourceRequest
            {
                Resource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource"),
                Content = new Resource
                {
                    FeatureSource = fs
                }
            };
            var sw = new Stopwatch();
            sw.Start();
            var response = resSvc.SetResource(req);
            sw.Stop();
            if (response.Error != null)
                Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message} ({sw.ElapsedMilliseconds}ms)");
            else
                Console.WriteLine($"    Result: OK ({sw.ElapsedMilliseconds}ms)");
        }

        static void ResourceService_GetResourceContent(MgResourceService.MgResourceServiceClient resSvc)
        {
            Console.WriteLine($"  Testing: {nameof(ResourceService_GetResourceContent)}");
            var req = new GetResourceContentRequest
            {
                Resource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource")
            };
            var sw = new Stopwatch();
            sw.Start();
            var response = resSvc.GetResourceContent(req);
            sw.Stop();
            if (response.Error != null)
            {
                Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message} ({sw.ElapsedMilliseconds}ms)");
            }
            else
            {
                var res = response.Result;
                Console.WriteLine($"    Type: {res.TypeCase} ({sw.ElapsedMilliseconds}ms)");
                switch (res.TypeCase)
                {
                    case Resource.TypeOneofCase.FeatureSource:
                        {
                            var fs = res.FeatureSource;
                            Console.WriteLine($"    FDO Provider: {fs.Provider}");
                            Console.WriteLine($"    Parameters: {fs.Parameters.Count}");
                            foreach (var p in fs.Parameters)
                            {
                                Console.WriteLine($"      {p.Name}: {p.Value}");
                            }
                        }
                        break;

                }
            }
        }

        static void FeatureService_TestConnection(MgFeatureService.MgFeatureServiceClient featureSvc)
        {
            Console.WriteLine($"  Testing: {nameof(FeatureService_TestConnection)}");
            var req = new TestConnectionRequest
            {
                FeatureSource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource")
            };
            var sw = new Stopwatch();
            sw.Start();
            var response = featureSvc.TestConnection(req);
            sw.Stop();
            if (response.Error != null)
                Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message} ({sw.ElapsedMilliseconds}ms)");
            else
                Console.WriteLine($"    Result: {response.Result} ({sw.ElapsedMilliseconds}ms)");
        }

        static void FeatureService_GetSchemas(MgFeatureService.MgFeatureServiceClient featureSvc)
        {
            Console.WriteLine($"  Testing: {nameof(FeatureService_GetSchemas)}");
            var req = new GetSchemasRequest
            {
                FeatureSource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource")
            };
            var sw = new Stopwatch();
            sw.Start();
            var response = featureSvc.GetSchemas(req);
            sw.Stop();
            if (response.Error != null)
            {
                Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message} ({sw.ElapsedMilliseconds}ms)");
            }
            else
            {
                Console.WriteLine($"    Result: {response.Result.Items.Count} ({sw.ElapsedMilliseconds}ms)");
                foreach (var name in response.Result.Items)
                {
                    Console.WriteLine($"      - {name}");
                }
            }
        }

        static void FeatureService_GetClasses(MgFeatureService.MgFeatureServiceClient featureSvc)
        {
            Console.WriteLine($"  Testing: {nameof(FeatureService_GetClasses)}");
            var req = new GetClassesRequest
            {
                FeatureSource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource"),
                SchemaName = "SHP_Schema"
            };
            var sw = new Stopwatch();
            sw.Start();
            var response = featureSvc.GetClasses(req);
            sw.Stop();
            if (response.Error != null)
            {
                Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message} ({sw.ElapsedMilliseconds}ms)");
            }
            else
            {
                Console.WriteLine($"    Result: {response.Result.Items.Count} ({sw.ElapsedMilliseconds}ms)");
                foreach (var name in response.Result.Items)
                {
                    Console.WriteLine($"      - {name}");
                }
            }
        }

        static void FeatureService_SelectFeatures(MgFeatureService.MgFeatureServiceClient featureSvc)
        {
            Console.WriteLine($"  Testing: {nameof(FeatureService_SelectFeatures)}");
            var req = new SelectFeaturesRequest
            {
                FeatureSource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource"),
                ClassName = "SHP_Schema:Parcels"
            };
            var sw = new Stopwatch();
            sw.Start();
            var response = featureSvc.SelectFeatures(req);
            var count = 0;
            try
            {
                var token = new CancellationToken();
                var fr = MgFeatureReader.Create(response, token).Result;
                var clsDef = fr.ClassDefinition;
                //Console.WriteLine($"{string.Join("|", clsDef.Properties.Select(p => p.Name))}");
                while (fr.ReadNextAsync().Result)
                {
                    var values = clsDef.Properties.Select(p =>
                    {
                        if (!fr.IsNull(p.Name))
                        {
                            switch (p.PropertyTypeCase)
                            {
                                case PropertyDefinition.PropertyTypeOneofCase.Data:
                                    {
                                        switch (p.Data.DataType)
                                        {
                                            case FdoDataType.Boolean:
                                                return $"{fr.GetBoolean(p.Name)}";
                                            case FdoDataType.Byte:
                                                return $"{fr.GetByte(p.Name)}";
                                            case FdoDataType.DateTime:
                                                return $"{fr.GetDateTime(p.Name)}";
                                            case FdoDataType.Decimal:
                                                return $"{fr.GetDecimal(p.Name)}";
                                            case FdoDataType.Double:
                                                return $"{fr.GetDouble(p.Name)}";
                                            case FdoDataType.Int16:
                                                return $"{fr.GetInt16(p.Name)}";
                                            case FdoDataType.Int32:
                                                return $"{fr.GetInt32(p.Name)}";
                                            case FdoDataType.Int64:
                                                return $"{fr.GetInt64(p.Name)}";
                                            case FdoDataType.Single:
                                                return $"{fr.GetSingle(p.Name)}";
                                            case FdoDataType.String:
                                                return $"{fr.GetString(p.Name)}";
                                        }
                                    }
                                    break;
                                case PropertyDefinition.PropertyTypeOneofCase.Geom:
                                case PropertyDefinition.PropertyTypeOneofCase.Raster:
                                    return "<null>";
                            }
                        }
                        return "<null>";
                    });
                    //Console.WriteLine(count);
                    count++;
                    /*
                    foreach (var p in clsDef.Properties)
                    {
                        Console.WriteLine($"{string.Join("|", values)}");
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"    {count} records read in {sw.ElapsedMilliseconds}ms");
            }
        }

        static void FeatureService_SelectFeaturesFiltered(MgFeatureService.MgFeatureServiceClient featureSvc)
        {
            Console.WriteLine($"  Testing: {nameof(FeatureService_SelectFeaturesFiltered)}");
            var req = new SelectFeaturesRequest
            {
                FeatureSource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource"),
                ClassName = "SHP_Schema:Parcels",
                Options = new FeatureQueryOptions
                {
                    Filter = "RNAME LIKE 'SCHMITT%'"
                }
            };
            var sw = new Stopwatch();
            sw.Start();
            var response = featureSvc.SelectFeatures(req);
            var count = 0;
            try
            {
                var token = new CancellationToken();
                var fr = MgFeatureReader.Create(response, token).Result;
                var clsDef = fr.ClassDefinition;
                //Console.WriteLine($"{string.Join("|", clsDef.Properties.Select(p => p.Name))}");
                while (fr.ReadNextAsync().Result)
                {
                    var values = clsDef.Properties.Select(p =>
                    {
                        if (!fr.IsNull(p.Name))
                        {
                            switch (p.PropertyTypeCase)
                            {
                                case PropertyDefinition.PropertyTypeOneofCase.Data:
                                    {
                                        switch (p.Data.DataType)
                                        {
                                            case FdoDataType.Boolean:
                                                return $"{fr.GetBoolean(p.Name)}";
                                            case FdoDataType.Byte:
                                                return $"{fr.GetByte(p.Name)}";
                                            case FdoDataType.DateTime:
                                                return $"{fr.GetDateTime(p.Name)}";
                                            case FdoDataType.Decimal:
                                                return $"{fr.GetDecimal(p.Name)}";
                                            case FdoDataType.Double:
                                                return $"{fr.GetDouble(p.Name)}";
                                            case FdoDataType.Int16:
                                                return $"{fr.GetInt16(p.Name)}";
                                            case FdoDataType.Int32:
                                                return $"{fr.GetInt32(p.Name)}";
                                            case FdoDataType.Int64:
                                                return $"{fr.GetInt64(p.Name)}";
                                            case FdoDataType.Single:
                                                return $"{fr.GetSingle(p.Name)}";
                                            case FdoDataType.String:
                                                return $"{fr.GetString(p.Name)}";
                                        }
                                    }
                                    break;
                                case PropertyDefinition.PropertyTypeOneofCase.Geom:
                                case PropertyDefinition.PropertyTypeOneofCase.Raster:
                                    return "<null>";
                            }
                        }
                        return "<null>";
                    });
                    //Console.WriteLine(count);
                    count++;
                    /*
                    foreach (var p in clsDef.Properties)
                    {
                        Console.WriteLine($"{string.Join("|", values)}");
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"    {count} records read in {sw.ElapsedMilliseconds}ms");
            }
        }

        static void FeatureService_SelectAggregate(MgFeatureService.MgFeatureServiceClient featureSvc)
        {
            Console.WriteLine($"  Testing: {nameof(FeatureService_SelectAggregate)}");
            var req = new SelectAggregateRequest
            {
                FeatureSource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource"),
                ClassName = "SHP_Schema:Parcels"
            };
            req.Options = new AggregateQueryOptions();
            req.Options.Properties.Add(new QueryProperty
            {
                Computed = new QueryPropertyComputed
                {
                    Alias = "Total",
                    Expression = "COUNT(Autogenerated_SDF_ID)"
                }
            });
            var sw = new Stopwatch();
            sw.Start();
            var response = featureSvc.SelectAggregate(req);
            var count = 0;
            try
            {
                var token = new CancellationToken();
                var fr = MgDataReader.Create(response, token).Result;
                var clsDef = fr.Header;
                Console.WriteLine();
                Console.WriteLine($"    {string.Join("|", clsDef.Properties.Select(p => p.Name))}");
                while (fr.ReadNextAsync().Result)
                {
                    var values = clsDef.Properties.Select(p =>
                    {
                        if (!fr.IsNull(p.Name))
                        {
                            switch (p.PropertyType)
                            {
                                case FdoLogicalPropertyType.DataBoolean:
                                    return $"{fr.GetBoolean(p.Name)}";
                                case FdoLogicalPropertyType.DataByte:
                                    return $"{fr.GetByte(p.Name)}";
                                case FdoLogicalPropertyType.DataDateTime:
                                    return $"{fr.GetDateTime(p.Name)}";
                                case FdoLogicalPropertyType.DataDecimal:
                                    return $"{fr.GetDecimal(p.Name)}";
                                case FdoLogicalPropertyType.DataDouble:
                                    return $"{fr.GetDouble(p.Name)}";
                                case FdoLogicalPropertyType.DataInt16:
                                    return $"{fr.GetInt16(p.Name)}";
                                case FdoLogicalPropertyType.DataInt32:
                                    return $"{fr.GetInt32(p.Name)}";
                                case FdoLogicalPropertyType.DataInt64:
                                    return $"{fr.GetInt64(p.Name)}";
                                case FdoLogicalPropertyType.DataSingle:
                                    return $"{fr.GetSingle(p.Name)}";
                                case FdoLogicalPropertyType.DataString:
                                    return $"{fr.GetString(p.Name)}";
                            }
                        }
                        return "<null>";
                    });
                    count++;
                    foreach (var p in clsDef.Properties)
                    {
                        Console.WriteLine($"    {string.Join("|", values)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }
            finally
            {
                sw.Stop();
                Console.WriteLine();
                Console.WriteLine($"    {count} records read in {sw.ElapsedMilliseconds}ms");
                Console.WriteLine();
            }
        }

        static void FeatureService_SelectAggregateFiltered(MgFeatureService.MgFeatureServiceClient featureSvc)
        {
            Console.WriteLine($"  Testing: {nameof(FeatureService_SelectAggregateFiltered)}");
            var req = new SelectAggregateRequest
            {
                FeatureSource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource"),
                ClassName = "SHP_Schema:Parcels"
            };
            req.Options = new AggregateQueryOptions
            {
                Filter = "RNAME LIKE 'SCHMITT%'"
            };
            req.Options.Properties.Add(new QueryProperty
            {
                Computed = new QueryPropertyComputed
                {
                    Alias = "Total",
                    Expression = "COUNT(Autogenerated_SDF_ID)"
                }
            });
            var sw = new Stopwatch();
            sw.Start();
            var response = featureSvc.SelectAggregate(req);
            var count = 0;
            try
            {
                var token = new CancellationToken();
                var fr = MgDataReader.Create(response, token).Result;
                var clsDef = fr.Header;
                Console.WriteLine();
                Console.WriteLine($"    {string.Join("|", clsDef.Properties.Select(p => p.Name))}");
                while (fr.ReadNextAsync().Result)
                {
                    var values = clsDef.Properties.Select(p =>
                    {
                        if (!fr.IsNull(p.Name))
                        {
                            switch (p.PropertyType)
                            {
                                case FdoLogicalPropertyType.DataBoolean:
                                    return $"{fr.GetBoolean(p.Name)}";
                                case FdoLogicalPropertyType.DataByte:
                                    return $"{fr.GetByte(p.Name)}";
                                case FdoLogicalPropertyType.DataDateTime:
                                    return $"{fr.GetDateTime(p.Name)}";
                                case FdoLogicalPropertyType.DataDecimal:
                                    return $"{fr.GetDecimal(p.Name)}";
                                case FdoLogicalPropertyType.DataDouble:
                                    return $"{fr.GetDouble(p.Name)}";
                                case FdoLogicalPropertyType.DataInt16:
                                    return $"{fr.GetInt16(p.Name)}";
                                case FdoLogicalPropertyType.DataInt32:
                                    return $"{fr.GetInt32(p.Name)}";
                                case FdoLogicalPropertyType.DataInt64:
                                    return $"{fr.GetInt64(p.Name)}";
                                case FdoLogicalPropertyType.DataSingle:
                                    return $"{fr.GetSingle(p.Name)}";
                                case FdoLogicalPropertyType.DataString:
                                    return $"{fr.GetString(p.Name)}";
                            }
                        }
                        return "<null>";
                    });
                    count++;
                    foreach (var p in clsDef.Properties)
                    {
                        Console.WriteLine($"    {string.Join("|", values)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }
            finally
            {
                sw.Stop();
                Console.WriteLine();
                Console.WriteLine($"    {count} records read in {sw.ElapsedMilliseconds}ms");
                Console.WriteLine();
            }
        }
    }
}
