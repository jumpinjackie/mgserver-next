cmake_minimum_required(VERSION 3.0)

cmake_policy(SET CMP0054 OLD)

set(MG_VERSION_MAJOR 1)
set(MG_VERSION_MINOR 0)
set(MG_VERSION_PATCH 0)
set(MG_VERSION "${MG_VERSION_MAJOR}.${MG_VERSION_MINOR}.${MG_VERSION_PATCH}")
set(CMAKE_DEBUG_POSTFIX "d")

set(MG_GRPC_CXX_DIR "${CMAKE_CURRENT_SOURCE_DIR}/gRpc/cxx")
set(MG_COMMON_DIR "${CMAKE_CURRENT_SOURCE_DIR}/Common")
set(MG_SERVICES_DIR "${CMAKE_CURRENT_SOURCE_DIR}/Server/Services")

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

add_subdirectory(Server)