project(MgServices CXX)

include_directories(${MG_GRPC_CXX_DIR})
include_directories(${MG_SERVICES_DIR})
include_directories(${MG_SERVICES_DIR}/gRpc)

set(MgServices_SOURCES
    gRpc/Common.grpc.pb.cc
    gRpc/Fdo.grpc.pb.cc
    gRpc/FeatureService_Messages.grpc.pb.cc
    gRpc/FeatureService.grpc.pb.cc
    gRpc/RenderingService_Messages.grpc.pb.cc
    gRpc/RenderingService.grpc.pb.cc
    gRpc/ResourceService_Messages.grpc.pb.cc
    gRpc/ResourceService.grpc.pb.cc
    gRpc/MdfModel/FeatureSource.grpc.pb.cc
    gRpc/MdfModel/LayerDefinition.grpc.pb.cc
    gRpc/MdfModel/MapDefinition.grpc.pb.cc
    gRpc/MdfModel/MdfCommon.grpc.pb.cc
    gRpc/MdfModel/SymbolDefinition.grpc.pb.cc
    gRpc/MdfModel/WatermarkDefinition.grpc.pb.cc
    ServerFeatureService.cpp
    ServerRenderingService.cpp
    ServerResourceService.cpp
)

link_libraries(protobuf::libprotobuf)
link_libraries(gRPC::grpc++_unsecure)
link_libraries(MgProtoMessages)

set(CMAKE_CXX_FLAGS  "${CMAKE_CXX_FLAGS} /FIServicesDllExport.h /FIMessagesDllExport.h")
add_definitions(-DSERVICES_EXPORTS)

add_library(${PROJECT_NAME} SHARED ${MgServices_SOURCES})