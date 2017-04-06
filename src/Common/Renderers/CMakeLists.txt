project(MgRenderers CXX)

include_directories(${MG_OEM_AGG_INCLUDE_DIR})
include_directories(${MG_OEM_AGG_DIR}/font_freetype)
include_directories(${FREETYPE_INCLUDE})
include_directories(${GD_INCLUDE})
include_directories(${MG_GRPC_CXX_DIR})
include_directories(${MG_COMMON_DIR}/Renderers)
include_directories(${MG_COMMON_DIR}/Stylization)

set(MgRenderers_SOURCES
    MapQuantization.cpp
    SymbolTrans.cpp
    #DWFRenderer.cpp
    #EPlotRenderer.cpp
    #W2DRewriter.cpp
    complex_polygon_gd.cpp
    GDFillPatterns.cpp
    GDRenderer.cpp
    GDUtils.cpp
    #GDW2DRewriter.cpp
    RS_ByteData.cpp
    ObservationMesh.cpp
    KmlContent.cpp
    KmlIconStyle.cpp
    KmlLineStyle.cpp
    KmlPolyStyle.cpp
    KmlRenderer.cpp
    FontManager.cpp
    ${MG_OEM_AGG_SOURCE_DIR}/agg_curves.cpp
    ${MG_OEM_AGG_DIR}/font_freetype/agg_font_freetype.cpp
    ${MG_OEM_AGG_SOURCE_DIR}/agg_line_aa_basics.cpp
    ${MG_OEM_AGG_SOURCE_DIR}/agg_line_profile_aa.cpp
    ${MG_OEM_AGG_SOURCE_DIR}/agg_sqrt_tables.cpp
    ${MG_OEM_AGG_SOURCE_DIR}/agg_trans_affine.cpp
    ${MG_OEM_AGG_SOURCE_DIR}/agg_vcgen_contour.cpp
    ${MG_OEM_AGG_SOURCE_DIR}/agg_vcgen_stroke.cpp
    AGGFillPatterns.cpp
    AGGImageIO.cpp
    AGGRenderer.cpp
    #AGGW2DRewriter.cpp
    CriticalSection.cpp
    RenderUtil.cpp
)

link_libraries(freetype)
link_libraries(${GD_LIBRARY})

add_library(${PROJECT_NAME} SHARED ${MgRenderers_SOURCES})