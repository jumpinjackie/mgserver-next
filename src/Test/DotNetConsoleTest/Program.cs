using Google.Protobuf;
using Grpc.Core;
using OSGeo.MapGuide;
using System;
using System.Diagnostics;
using System.IO;

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

            Console.WriteLine(nameof(MgResourceService));
            Console.WriteLine($"  Testing: {nameof(MgResourceService.MgResourceServiceClient.ResourceExists)}");
            {
                var sw = new Stopwatch();
                sw.Start();
                var response = resSvc.ResourceExists(new ResourceExistsRequest { Resource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource") });
                sw.Stop();
                if (response.Error != null)
                    Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message} ({sw.ElapsedMilliseconds}ms)");
                else
                    Console.WriteLine($"    Result: {response.Result} ({sw.ElapsedMilliseconds}ms)");
            }
            Console.WriteLine($"  Testing: {nameof(MgResourceService.MgResourceServiceClient.SetResource)}");
            {
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
            Console.WriteLine($"  Testing: {nameof(MgResourceService.MgResourceServiceClient.GetResourceContent)}");
            {
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

            Console.WriteLine(nameof(MgFeatureService));
            Console.WriteLine($"  Testing: {nameof(MgFeatureService.MgFeatureServiceClient.TestConnection)}");
            {
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
            Console.WriteLine($"  Testing: {nameof(MgFeatureService.MgFeatureServiceClient.GetSchemas)}");
            {
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
                    foreach (var name in response .Result.Items )
                    {
                        Console.WriteLine($"      - {name}");
                    }
                }
            }
            Console.WriteLine($"  Testing: {nameof(MgFeatureService.MgFeatureServiceClient.GetClasses)}");
            {
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

            channel.ShutdownAsync().Wait();
        }
    }
}
