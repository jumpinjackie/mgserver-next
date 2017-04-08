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

# Protobuf
set(protobuf_MODULE_COMPATIBLE ON CACHE BOOL "")
find_package(Protobuf CONFIG REQUIRED)
message(STATUS "Using protobuf ${Protobuf_VERSION}")

# gRPC
find_package(gRPC CONFIG REQUIRED)
message(STATUS "Using gRPC ${gRPC_VERSION}")

# ACE
find_path(ACE_INCLUDE NAMES "ace/Init_ACE.h")
find_library(ACE_LIBRARY NAMES ACE)
if(NOT ACE_INCLUDE)
    message(FATAL_ERROR "ACE include path not found")
else(NOT ACE_INCLUDE)
    message(STATUS "Found ACE include path: ${ACE_INCLUDE}")
endif()
if(NOT ACE_LIBRARY)
    message(FATAL_ERROR "ACE library not found")
else(NOT ACE_LIBRARY)
    message(STATUS "Found ACE library: ${ACE_LIBRARY}")
endif()

# freetype
find_package(freetype CONFIG REQUIRED)
find_path(FREETYPE_INCLUDE NAMES "ft2build.h")

# libpng
find_package(png REQUIRED)
find_path(PNG_INCLUDE NAMES "png.h")

# libgd
find_path(GD_INCLUDE NAMES "gd.h")
find_library(GD_LIBRARY NAMES libgd)
if(NOT GD_INCLUDE)
    message(FATAL_ERROR "GD include path not found")
else(NOT GD_INCLUDE)
    message(STATUS "Found GD include path: ${GD_INCLUDE}")
endif()
if(NOT GD_LIBRARY)
    message(FATAL_ERROR "GD library not found")
else(NOT GD_LIBRARY)
    message(STATUS "Found GD library: ${GD_LIBRARY}")
endif()

# geos
find_path(GEOS_INCLUDE NAMES "geos/geos.h")
find_library(GEOS_LIBRARY NAMES GEOS)
if(NOT GEOS_INCLUDE)
    message(FATAL_ERROR "GEOS include path not found")
else(NOT GEOS_INCLUDE)
    message(STATUS "Found GEOS include path: ${GEOS_INCLUDE}")
endif()
if(NOT GEOS_LIBRARY)
    message(FATAL_ERROR "GEOS library not found")
else(NOT GEOS_LIBRARY)
    message(STATUS "Found GEOS library: ${GEOS_LIBRARY}")
endif()

# FDO common
find_path(FDO_COMMON_INCLUDE NAMES "FdoCommon.h"
    PATHS ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/Fdo/Inc
    NO_DEFAULT_PATH
    NO_CMAKE_ENVIRONMENT_PATH
    NO_CMAKE_PATH
    NO_SYSTEM_ENVIRONMENT_PATH
    NO_CMAKE_SYSTEM_PATH
    NO_CMAKE_FIND_ROOT_PATH)
if(NOT FDO_COMMON_INCLUDE)
    message(FATAL_ERROR "FDO common include path not found")
else(NOT FDO_COMMON_INCLUDE)
    message(STATUS "Found FDO common include path: ${FDO_COMMON_INCLUDE}")
endif()

find_library(FDO_COMMON_LIBRARY NAMES FDOCommon
    PATHS ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/Fdo/Lib
    NO_DEFAULT_PATH
    NO_CMAKE_ENVIRONMENT_PATH
    NO_CMAKE_PATH
    NO_SYSTEM_ENVIRONMENT_PATH
    NO_CMAKE_SYSTEM_PATH
    NO_CMAKE_FIND_ROOT_PATH)
if(NOT FDO_COMMON_LIBRARY)
    message(FATAL_ERROR "FDO common library path not found")
else(NOT FDO_COMMON_LIBRARY)
    message(STATUS "Found FDO common library path: ${FDO_COMMON_LIBRARY}")
endif()

# FDO geometry
find_path(FDO_GEOM_INCLUDE NAMES "FdoGeometry.h"
    PATHS ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/Fdo/Inc
    NO_DEFAULT_PATH
    NO_CMAKE_ENVIRONMENT_PATH
    NO_CMAKE_PATH
    NO_SYSTEM_ENVIRONMENT_PATH
    NO_CMAKE_SYSTEM_PATH
    NO_CMAKE_FIND_ROOT_PATH)
if(NOT FDO_GEOM_INCLUDE)
    message(FATAL_ERROR "FDO geometry include path not found")
else(NOT FDO_GEOM_INCLUDE)
    message(STATUS "Found FDO geometry include path: ${FDO_GEOM_INCLUDE}")
endif()

find_library(FDO_GEOM_LIBRARY NAMES FDOGeometry
    PATHS ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/Fdo/Lib
    NO_DEFAULT_PATH
    NO_CMAKE_ENVIRONMENT_PATH
    NO_CMAKE_PATH
    NO_SYSTEM_ENVIRONMENT_PATH
    NO_CMAKE_SYSTEM_PATH
    NO_CMAKE_FIND_ROOT_PATH)
if(NOT FDO_GEOM_LIBRARY)
    message(FATAL_ERROR "FDO geometry library path not found")
else(NOT FDO_GEOM_LIBRARY)
    message(STATUS "Found FDO geometry library path: ${FDO_GEOM_LIBRARY}")
endif()

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
    message(FATAL_ERROR "FDO include path not found")
else(NOT FDO_INCLUDE)
    message(STATUS "Found FDO include path: ${FDO_INCLUDE}")
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
    message(FATAL_ERROR "FDO library path not found")
else(NOT FDO_LIBRARY)
    message(STATUS "Found FDO library path: ${FDO_LIBRARY}")
endif()

# FDO Expression Engine
find_path(FDO_EXPRENG_INCLUDE NAMES "FdoExpressionEngine.h"
    PATHS ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/Fdo/Inc/ExpressionEngine
    NO_DEFAULT_PATH
    NO_CMAKE_ENVIRONMENT_PATH
    NO_CMAKE_PATH
    NO_SYSTEM_ENVIRONMENT_PATH
    NO_CMAKE_SYSTEM_PATH
    NO_CMAKE_FIND_ROOT_PATH)
if(NOT FDO_EXPRENG_INCLUDE)
    message(FATAL_ERROR, "FDO Expression Engine include path not found")
endif()

find_library(FDO_EXPRENG_LIBRARY NAMES ExpressionEngine
    PATHS ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/Fdo/Lib
    NO_DEFAULT_PATH
    NO_CMAKE_ENVIRONMENT_PATH
    NO_CMAKE_PATH
    NO_SYSTEM_ENVIRONMENT_PATH
    NO_CMAKE_SYSTEM_PATH
    NO_CMAKE_FIND_ROOT_PATH)
if(NOT FDO_EXPRENG_LIBRARY)
    message(FATAL_ERROR, "FDO Expression Engine library path not found")
endif()

add_subdirectory(src)