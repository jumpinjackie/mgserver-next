project(mgserver CXX)

include_directories(${MG_GRPC_CXX_DIR})
include_directories(${MG_SERVICES_DIR})
include_directories(${MG_SERVICES_DIR}/gRpc)

link_libraries(protobuf::libprotobuf)
link_libraries(gRPC::grpc++_unsecure)
link_libraries(MgServices)

# Suppress protobuf-induced warning spam
add_definitions(/wd4267 /wd4251 /wd4275)

set(CMAKE_CXX_FLAGS  "${CMAKE_CXX_FLAGS} /FIServicesDllExport.h /FIMessagesDllExport.h")

file(GLOB SOURCES "*.cpp")
file(GLOB HEADERS "*.h")

add_executable("${PROJECT_NAME}" "${SOURCES}")