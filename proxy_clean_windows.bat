@echo off

echo [clean]: .net Server/Client
if exist %CD%\src\Common\gRpc\csharp\bin rd /S /Q %CD%\src\Common\gRpc\csharp\bin
if exist %CD%\src\Common\gRpc\csharp\obj rd /S /Q %CD%\src\Common\gRpc\csharp\obj
if exist %CD%\src\Common\gRpc\csharp\*.cs del %CD%\src\Common\gRpc\csharp\*.cs
echo [clean]: PHP Client
if exist %CD%\src\Common\gRpc\php\Osgeo rd /S /Q %CD%\src\Common\gRpc\php\Osgeo
if exist %CD%\src\Common\gRpc\php\GPBMetadata rd /S /Q %CD%\src\Common\gRpc\php\GPBMetadata
echo [clean]: C++ Server/Client
if exist %CD%\src\Common\gRpc\cxx\*.pb.cc del %CD%\src\Common\gRpc\cxx\*.pb.cc
if exist %CD%\src\Common\gRpc\cxx\*.pb.h del %CD%\src\Common\gRpc\cxx\*.pb.h
echo [clean]: Java Client
if exist %CD%\src\Common\gRpc\java\osgeo rd /S /Q %CD%\src\Common\gRpc\java\osgeo