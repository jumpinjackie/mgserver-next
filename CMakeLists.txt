cmake_minimum_required(VERSION 3.0)

if(COMMAND cmake_policy)
    cmake_policy(SET CMP0003 NEW)
    cmake_policy(SET CMP0054 OLD)
endif(COMMAND cmake_policy)

set(MG_VERSION_MAJOR 1)
set(MG_VERSION_MINOR 0)
set(MG_VERSION_PATCH 0)
set(MG_VERSION "${MG_VERSION_MAJOR}.${MG_VERSION_MINOR}.${MG_VERSION_PATCH}")
set(CMAKE_DEBUG_POSTFIX "d")

set(MG_GRPC_CXX_DIR "${CMAKE_CURRENT_SOURCE_DIR}/src/gRpc/cxx")
set(MG_COMMON_DIR "${CMAKE_CURRENT_SOURCE_DIR}/src/Common")
set(MG_SERVICES_DIR "${CMAKE_CURRENT_SOURCE_DIR}/src/Server/Services")
set(MG_OEM_AGG_DIR "${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/agg-2.4")
set(MG_OEM_AGG_INCLUDE_DIR "${MG_OEM_AGG_DIR}/include")
set(MG_OEM_AGG_SOURCE_DIR "${MG_OEM_AGG_DIR}/src")

message(STATUS "Build type is: ${CMAKE_BUILD_TYPE}")

if(WIN32)
    add_definitions(-D_WIN32_WINNT=0x600)
    add_definitions(-DUNICODE -D_UNICODE)
    add_definitions(/MP)
    if(MSVC)
        if(NOT (MSVC14))
            message(FATAL_ERROR, "Only MSVC14 (Visual Studio 2015) is supported compiler on Windows")
        endif(NOT (MSVC14))
    else(MSVC)
        message(FATAL_ERROR, "Only MSVC is the supported compiler on Windows")
    endif(MSVC)
endif(WIN32)

# FDO
find_path(FDO_INCLUDE NAMES "Fdo.h"
    PATHS ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/Fdo/Inc
    NO_DEFAULT_PATH
    NO_CMAKE_ENVIRONMENT_PATH
    NO_CMAKE_PATH
    NO_SYSTEM_ENVIRONMENT_PATH
    NO_CMAKE_SYSTEM_PATH
    NO_CMAKE_FIND_ROOT_PATH)
if(NOT FDO_INCLUDE)
    message(FATAL_ERROR, "FDO include path not found")
endif()

find_library(FDO_LIBRARY NAMES FDO
    PATHS ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/Fdo/Lib
    NO_DEFAULT_PATH
    NO_CMAKE_ENVIRONMENT_PATH
    NO_CMAKE_PATH
    NO_SYSTEM_ENVIRONMENT_PATH
    NO_CMAKE_SYSTEM_PATH
    NO_CMAKE_FIND_ROOT_PATH)
if(NOT FDO_LIBRARY)
    message(FATAL_ERROR, "FDO library path not found")
endif()

# Protobuf
set(protobuf_MODULE_COMPATIBLE ON CACHE BOOL "")
find_package(Protobuf CONFIG REQUIRED)
message(STATUS "Using protobuf ${Protobuf_VERSION}")

# gRPC
find_package(gRPC CONFIG REQUIRED)
message(STATUS "Using gRPC ${gRPC_VERSION}")

#freetype
find_package(freetype CONFIG REQUIRED)
find_path(FREETYPE_INCLUDE NAMES "ft2build.h")

#libgd
find_path(GD_INCLUDE NAMES "gd.h")
find_library(GD_LIBRARY NAMES libgd)

add_subdirectory(src)