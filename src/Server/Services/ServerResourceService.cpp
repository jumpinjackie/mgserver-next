#include "ServerResourceService.h"

ServerResourceService::ServerResourceService() 
{

}

ServerResourceService::~ServerResourceService()
{

}

// Gets the content of the specified resource
::grpc::Status ServerResourceService::GetResourceContent(::grpc::ServerContext* context, const ::OSGeo::MapGuide::GetResourceContentRequest* request, ::OSGeo::MapGuide::GetResourceContentResponse* response)
{
	return ::grpc::Status::OK;
}

// Enumerates the resources in the specified repository
::grpc::Status ServerResourceService::EnumerateResources(::grpc::ServerContext* context, const ::OSGeo::MapGuide::EnumerateResourcesRequest* request, ::OSGeo::MapGuide::EnumerateResourcesResponse* response)
{
	auto err = response->mutable_error();
	err->set_name("MgNotImplementedException");
	err->set_message("Not implemented");
	return ::grpc::Status::OK;
}

// Checks to see if the specified resource exists
::grpc::Status ServerResourceService::ResourceExists(::grpc::ServerContext* context, const ::OSGeo::MapGuide::ResourceExistsRequest* request, ::OSGeo::MapGuide::ResourceExistsResponse* response)
{
	return ::grpc::Status::OK;
}

// Adds a new resource to a resource repository, or updates an existing resource
::grpc::Status ServerResourceService::SetResource(::grpc::ServerContext* context, const ::OSGeo::MapGuide::SetResourceRequest* request, ::OSGeo::MapGuide::BasicResponse* response)
{
	return ::grpc::Status::OK;
}