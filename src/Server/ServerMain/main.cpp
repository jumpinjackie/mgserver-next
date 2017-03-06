#include <string>

#include "ServerRenderingService.h"
#include "ServerFeatureService.h"
#include "ServerResourceService.h"

#include <grpc/grpc.h>
#include <grpc++/grpc++.h>
#include <grpc++/channel.h>
#include <grpc++/server.h>
#include <grpc++/server_builder.h>
#include <grpc++/server_context.h>
#include <grpc++/security/server_credentials.h>

int main (int argc, char** argv)
{
    std::string server_address("0.0.0.0:7000");

    std::shared_ptr<grpc::Channel> localChannel(grpc::CreateChannel(server_address, grpc::InsecureChannelCredentials()));

    ServerRenderingService svRenderSvc;
    ServerResourceService svResourceSvc;
    ServerFeatureService svFeatureSvc;

    grpc::ServerBuilder builder;
    builder.AddListeningPort(server_address, grpc::InsecureServerCredentials());
    builder.RegisterService(&svResourceSvc);
    builder.RegisterService(&svFeatureSvc);
    builder.RegisterService(&svRenderSvc);

    std::unique_ptr<grpc::Server> server(builder.BuildAndStart());
    std::cout << "Server listening on " << server_address << std::endl;
    server->Wait();

    return 0;
}