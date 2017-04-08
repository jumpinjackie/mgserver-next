project(MgGeometry CXX)

include_directories(${GEOS_INCLUDE})
include_directories(${MG_COMMON_DIR}/Foundation)
include_directories(${MG_COMMON_DIR}/Geometry)

set(MgGeometry_SOURCES
    GeometryBuild.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/ArbitraryCoordsys.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysDictionaryBase.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSys.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysCatalog.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysCategory.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysCategoryDictionary.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysDatum.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysDatumDictionary.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysDictionary.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysEllipsoid.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysEllipsoidDictionary.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysEnum.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysEnumCategory.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysEnumCoordinateSystemInCategory.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysEnumDatum.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysEnumEllipsoid.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysFormatConverter.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticAnalyticalTransformDefParams.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticInterpolationTransformDefParams.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticMultipleRegressionTransformDefParams.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticStandaloneTransformDefParams.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticPath.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticPathDictionary.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticPathElement.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticTransformation.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticTransformDef.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticTransformDefDictionary.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticTransformDefParams.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGeodeticTransformGridFile.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysMeasure.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysTransform.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysDictionaryUtility.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysEnumInteger32.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysMathComparator.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysProjectionInformation.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysUnitInformation.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysUtil.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/MentorDictionary.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/MentorUtil.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysWktFailureCache.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGridGeneric.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysGrids.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysMgrsZone.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysMgrs.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysMgrsMajorRegion.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CoordSysOneGrid.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/namestruct.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/CriticalSection.cpp
    ${MG_COMMON_DIR}/CoordinateSystem/SmartCriticalClass.cpp)

add_definitions(-DGEOMETRY_EXPORTS)

link_libraries(${GEOS_LIBRARY})

add_library(${PROJECT_NAME} SHARED ${MgGeometry_SOURCES})