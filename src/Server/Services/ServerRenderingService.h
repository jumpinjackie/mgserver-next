#ifndef SERVER_RENDERING_SERVICE_H
#define SERVER_RENDERING_SERVICE_H

#include "gRpc/RenderingService.grpc.pb.h"

using namespace OSGeo::MapGuide;

class ServerRenderingService final : public RenderingService::Service {
public:
	MG_SERVICES_API ServerRenderingService();

	MG_SERVICES_API virtual ~ServerRenderingService();
};

#endif