project(MgFoundation CXX)

include_directories(${ACE_INCLUDE})
include_directories(${MG_COMMON_DIR}/MdfModel) # For Base64.h
include_directories(${MG_COMMON_DIR}/Foundation)

set(MgFoundation_SOURCES
    FoundationBuild.cpp)

add_definitions(-DFOUNDATION_EXPORTS)

link_libraries(${ACE_LIBRARY})

add_library(${PROJECT_NAME} SHARED ${MgFoundation_SOURCES})