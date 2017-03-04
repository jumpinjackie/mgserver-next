@echo off

echo [clean]: .net Server/Client
if exist %CD%\src\gRpc\csharp\bin rd /S /Q %CD%\src\gRpc\csharp\bin
if exist %CD%\src\gRpc\csharp\obj rd /S /Q %CD%\src\gRpc\csharp\obj
if exist %CD%\src\gRpc\csharp\*.cs del %CD%\src\gRpc\csharp\*.cs
echo [clean]: PHP Client
if exist %CD%\src\gRpc\php\Osgeo rd /S /Q %CD%\src\gRpc\php\Osgeo
if exist %CD%\src\gRpc\php\GPBMetadata rd /S /Q %CD%\src\gRpc\php\GPBMetadata
echo [clean]: C++ Server/Client
if exist %CD%\src\gRpc\cxx\*.pb.cc del %CD%\src\gRpc\cxx\*.pb.cc
if exist %CD%\src\gRpc\cxx\*.pb.h del %CD%\src\gRpc\cxx\*.pb.h
echo [clean]: Java Client
if exist %CD%\src\gRpc\java\osgeo rd /S /Q %CD%\src\gRpc\java\osgeo