#ifndef SERVER_FEATURE_SERVICE_H
#define SERVER_FEATURE_SERVICE_H

#include "DllExport.h"
#include "../../gRpc/cxx/FeatureService.grpc.pb.h"

using namespace OSGeo::MapGuide;

class ServerFeatureService final : public FeatureService::Service {
public:
	MG_SERVICES_API ServerFeatureService();

	MG_SERVICES_API virtual ~ServerFeatureService();
};

#endif