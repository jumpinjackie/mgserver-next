project(MgServices CXX)

include_directories(${MG_GRPC_CXX_DIR})

find_package(gRPC REQUIRED)
find_package(protobuf REQUIRED)

link_libraries(protobuf::libprotobuf)
link_libraries(gRPC::grpc++_unsecure)

file(GLOB SOURCES "*.cpp")
file(GLOB HEADERS "*.h")

add_library("${PROJECT_NAME}" SHARED "${SOURCES}")