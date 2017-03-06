#ifndef SERVER_RESOURCE_SERVICE_H
#define SERVER_RESOURCE_SERVICE_H

#include "DllExport.h"
#include "../../gRpc/cxx/ResourceService.grpc.pb.h"

using namespace OSGeo::MapGuide;

class ServerResourceService final : public ResourceService::Service {
public:
	MG_SERVICES_API ServerResourceService();

	MG_SERVICES_API virtual ~ServerResourceService();

public:
	// Gets the content of the specified resource
	virtual ::grpc::Status GetResourceContent(::grpc::ServerContext* context, const ::OSGeo::MapGuide::GetResourceContentRequest* request, ::OSGeo::MapGuide::GetResourceContentResponse* response);
	// Enumerates the resources in the specified repository
	virtual ::grpc::Status EnumerateResources(::grpc::ServerContext* context, const ::OSGeo::MapGuide::EnumerateResourcesRequest* request, ::OSGeo::MapGuide::EnumerateResourcesResponse* response);
	// Checks to see if the specified resource exists
	virtual ::grpc::Status ResourceExists(::grpc::ServerContext* context, const ::OSGeo::MapGuide::ResourceExistsRequest* request, ::OSGeo::MapGuide::ResourceExistsResponse* response);
	// Adds a new resource to a resource repository, or updates an existing resource
	virtual ::grpc::Status SetResource(::grpc::ServerContext* context, const ::OSGeo::MapGuide::SetResourceRequest* request, ::OSGeo::MapGuide::BasicResponse* response);
};

#endif