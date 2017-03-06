project(mgserver CXX)
cmake_minimum_required(VERSION 3.0)

cmake_policy(SET CMP0054 OLD)

set(VERSION_MAJOR 1)
set(VERSION_MINOR 0)
set(VERSION_PATCH 0)
set(VERSION "${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_PATCH}")
set(CMAKE_DEBUG_POSTFIX "d")

message(STATUS "Build type is: ${CMAKE_BUILD_TYPE}")
if(WIN32)
	add_definitions(-D_WIN32_WINNT=0x600)
	add_definitions(-DUNICODE -D_UNICODE)
	if(MSVC)
		if(NOT (MSVC14))
			message(FATAL_ERROR, "Only MSVC14 (Visual Studio 2015) is supported compiler on Windows")
		endif(NOT (MSVC14))
	else(MSVC)
		message(FATAL_ERROR, "Only MSVC is the supported compiler on Windows")
	endif(MSVC)
endif(WIN32)

find_package(gRPC REQUIRED)
find_package(protobuf REQUIRED)

include_directories("../Services")

link_libraries(protobuf::libprotobuf)
link_libraries(gRPC::grpc++_unsecure)
link_libraries(MgServices)

file(GLOB SOURCES "*.cpp")
file(GLOB HEADERS "*.h")

add_executable("${PROJECT_NAME}" "${SOURCES}")