project(MgProtoMessages CXX)

include_directories(${MG_GRPC_CXX_DIR})

set(MgProtoMessages_SOURCES
    UnicodeString.cpp
    Common.pb.cc
    Fdo.pb.cc
    FeatureService_Messages.pb.cc
    FeatureService.pb.cc
    RenderingService_Messages.pb.cc
    RenderingService.pb.cc
    ResourceService_Messages.pb.cc
    ResourceService.pb.cc
    MdfModel/FeatureSource.pb.cc
    MdfModel/LayerDefinition.pb.cc
    MdfModel/MapDefinition.pb.cc
    MdfModel/MdfCommon.pb.cc
    MdfModel/SymbolDefinition.pb.cc
    MdfModel/WatermarkDefinition.pb.cc
)

set(CMAKE_CXX_FLAGS  "${CMAKE_CXX_FLAGS} /FIMessagesDllExport.h")
add_definitions(-DPROTO_MESSAGES_EXPORTS)
# Suppress protobuf-induced warning spam
add_definitions(/wd4267 /wd4251 /wd4275)
link_libraries(protobuf::libprotobuf)

add_library(${PROJECT_NAME} SHARED ${MgProtoMessages_SOURCES})