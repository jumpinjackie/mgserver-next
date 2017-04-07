project(MgStylization CXX)

include_directories(${MG_GRPC_CXX_DIR})
include_directories(${MG_COMMON_DIR}/Core)
include_directories(${PROJECT_NAME} PRIVATE ${FDO_INCLUDE})
include_directories(${PROJECT_NAME} PRIVATE ${FDO_EXPRENG_INCLUDE})

set(MgStylization_SOURCES
    ExpressionFunctionArgb.cpp
    ExpressionFunctionDecap.cpp
    ExpressionFunctionFeatureClass.cpp
    ExpressionFunctionFeatureId.cpp
    ExpressionFunctionFeatureSource.cpp
    ExpressionFunctionHtmlColor.cpp
    ExpressionFunctionIf.cpp
    ExpressionFunctionLayerDefinition.cpp
    ExpressionFunctionLayerId.cpp
    ExpressionFunctionLookup.cpp
    ExpressionFunctionMapCenterX.cpp
    ExpressionFunctionMapCenterY.cpp
    ExpressionFunctionMapName.cpp
    ExpressionFunctionMapScale.cpp
    ExpressionFunctionRange.cpp
    ExpressionFunctionSession.cpp
    ExpressionFunctionUrlEncode.cpp
    ExpressionHelper.cpp
    LineBuffer.cpp
    Matrix3D.cpp
    Point2D.cpp
    Point3D.cpp
    PolygonUtils.cpp
    Vector2D.cpp
    Vector3D.cpp
    Stylizer.cpp
    DefaultStylizer.cpp
    GeometryAdapter.cpp
    PointAdapter.cpp
    PolygonAdapter.cpp
    PolylineAdapter.cpp
    RasterAdapter.cpp
    SE_AreaPositioning.cpp
    SE_Bounds.cpp
    SE_BufferPool.cpp
    SE_ExpressionBase.cpp
    SE_LineBuffer.cpp
    SE_LineRenderer.cpp
    SE_Matrix.cpp
    SE_PositioningAlgorithms.cpp
    SE_Renderer.cpp
    SE_StyleVisitor.cpp
    SE_SymbolDefProxies.cpp
    SE_SymbolManager.cpp
    StylizationEngine.cpp
    atom_element_abandonment.cpp
    atom_element_environment.cpp
    atom_element_location.cpp
    atom_element_structure.cpp
    atom_element_style.cpp
    atom_element_textrun.cpp
    atom_element_transform.cpp
    atom_element_universe.cpp
    atom_impl.cpp
    BIDIConverter.cpp
    mtext_parser.cpp
    RichTextEngine.cpp
    RS_FontEngine.cpp
    RS_TextMetrics.cpp
    Band.cpp
    BandData.cpp
    Color.cpp
    GeometryAlgorithms.cpp
    GridApplyStatusReporter.cpp
    GridChannelBand.cpp
    GridColorBandHandler.cpp
    GridColorBandsHandler.cpp
    GridColorHandler.cpp
    GridColorNullHandler.cpp
    GridColorThemeHandler.cpp
    GridData.cpp
    GridStyleColorHandler.cpp
    GridStyleSurfaceColorHandler.cpp
    GridStyleSurfaceHandler.cpp
    GridStylizer.cpp
    GridTheme.cpp
    GridThemeParser.cpp
    KeyEncode.cpp
    LabelRenderer.cpp
    LabelRendererBase.cpp
    LabelRendererLocal.cpp
    LineStyleDef.cpp
    SimpleOverpost.cpp
    StylizationUtil.cpp
    TransformMesh.cpp
    ThemeParameters.cpp
)

link_libraries(${PROJECT_NAME} PRIVATE ${FDO_LIBRARY})
link_libraries(${PROJECT_NAME} PRIVATE ${FDO_EXPRENGINE_LIBRARY})
link_libraries(protobuf::libprotobuf)
link_libraries(MgProtoMessages)

set(CMAKE_CXX_FLAGS  "${CMAKE_CXX_FLAGS} /FIMessagesDllExport.h")
# Suppress protobuf-induced warning spam
add_definitions(/wd4267 /wd4251 /wd4275)

add_library(${PROJECT_NAME} SHARED ${MgStylization_SOURCES})