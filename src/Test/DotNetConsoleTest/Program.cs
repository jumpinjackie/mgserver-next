using Grpc.Core;
using OSGeo.MapGuide;
using System;

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
                var response = resSvc.ResourceExists(new ResourceExistsRequest { Resource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource") });
                if (response.Error != null)
                    Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message}");
                else
                    Console.WriteLine($"    Result: {response.Result}");
            }
            Console.WriteLine($"  Testing: {nameof(MgResourceService.MgResourceServiceClient.SetResource)}");
            {
                var fs = new FeatureSource
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
                var response = resSvc.SetResource(req);
                if (response.Error != null)
                    Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message}");
                else
                    Console.WriteLine($"    Result: OK");
            }
            Console.WriteLine($"  Testing: {nameof(MgResourceService.MgResourceServiceClient.GetResourceContent)}");
            {
                var req = new GetResourceContentRequest
                {
                    Resource = ResourceIdentifier.Parse("Library://Samples/Sheboygan/Data/Parcels.FeatureSource")
                };
                var response = resSvc.GetResourceContent(req);
                if (response.Error != null)
                {
                    Console.WriteLine($"    Error: {response.Error.Name} - {response.Error.Message}");
                }
                else
                {
                    var res = response.Result;
                    Console.WriteLine($"    Type: {res.TypeCase}");
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

            channel.ShutdownAsync().Wait();
        }
    }
}
