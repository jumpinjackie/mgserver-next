project(mgserver CXX)

find_package(gRPC REQUIRED)
find_package(protobuf REQUIRED)

include_directories(${MG_SERVICES_DIR})

link_libraries(protobuf::libprotobuf)
link_libraries(gRPC::grpc++_unsecure)
link_libraries(MgServices)

file(GLOB SOURCES "*.cpp")
file(GLOB HEADERS "*.h")

add_executable("${PROJECT_NAME}" "${SOURCES}")