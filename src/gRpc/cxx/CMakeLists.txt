project(MgGrpcCommon CXX)
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
	#set(CMAKE_CXX_FLAGS_DEBUG "/MTd /Zi /Ob0 /Od /RTC1 /D_ITERATOR_DEBUG_LEVEL=2")
	#set(CMAKE_CXX_FLAGS_RELEASE "/MT /O2 /Ob2 /DNDEBUG")
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

link_libraries(protobuf::libprotobuf)
link_libraries(gRPC::grpc++_unsecure)

file(GLOB SOURCES "*.cc")
file(GLOB HEADERS "*.h")

add_library("${PROJECT_NAME}" SHARED "${SOURCES}")