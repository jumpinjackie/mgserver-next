using Grpc.Core;
using OSGeo.MapGuide;
using OSGeo.MapGuide.Services;
using System;
using System.IO;

namespace mgserver.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var thisDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dataRoot = Path.Combine(thisDir, "data");
            var contentRoot = Path.Combine(dataRoot, "content");
            var dataFilesRoot = Path.Combine(dataRoot, "datafiles");

            var hostName = "localhost";
            var port = 7000;
            var localChannel = new Channel(hostName, port, ChannelCredentials.Insecure);

            var resolver = new ResourcePathResolver(contentRoot, dataFilesRoot);
            var serverFeatSvc = new MgServerFeatureService(resolver);
            var serverResSvc = new MgServerResourceService(resolver);
            var serverRenderSvc = new MgServerRenderingService();

            var credentials = ServerCredentials.Insecure;
            var server = new Server
            {
                Services =
                {
                    MgFeatureService.BindService(serverFeatSvc),
                    MgResourceService.BindService(serverResSvc),
                    MgRenderingService.BindService(serverRenderSvc)
                },
                Ports =
                {
                    new ServerPort(hostName, port, credentials)
                }
            };
            server.Start();

            serverFeatSvc.InitClientDependencies(localChannel);

            Console.WriteLine("MapGuide Server listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            
            server.ShutdownAsync().Wait();
            localChannel.ShutdownAsync().Wait();
        }
    }
}
