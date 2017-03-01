using Grpc.Core;
using OSGeo.MapGuide;
using OSGeo.MapGuide.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var resolver = new ResourcePathResolver(contentRoot, dataFilesRoot);
            var serverFeatSvc = new MgServerFeatureService(resolver);
            var serverResSvc = new MgServerResourceService(resolver);
            var serverRenderSvc = new MgServerRenderingService();

            var credentials = ServerCredentials.Insecure;

            int port = 7000;
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
                    new ServerPort("localhost", port, credentials)
                }
            };
            server.Start();

            serverFeatSvc.InitClientDependencies("localhost", port);

            Console.WriteLine("MapGuide Server listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
            serverFeatSvc.Teardown();
        }
    }
}
