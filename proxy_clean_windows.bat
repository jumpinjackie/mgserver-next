@echo off

echo [clean]: .net Server/Client
if exist %CD%\src\gRpc\csharp\bin rd /S /Q %CD%\src\gRpc\csharp\bin
if exist %CD%\src\gRpc\csharp\obj rd /S /Q %CD%\src\gRpc\csharp\obj
if exist %CD%\src\gRpc\csharp\*.cs del %CD%\src\gRpc\csharp\*.cs
echo [clean]: PHP Client
if exist %CD%\src\gRpc\php\OSGeo rd /S /Q %CD%\src\gRpc\php\OSGeo
if exist %CD%\src\gRpc\php\GPBMetadata rd /S /Q %CD%\src\gRpc\php\GPBMetadata
echo [clean]: C++ Server/Client
if exist %CD%\src\gRpc\cxx\*.pb.cc del %CD%\src\gRpc\cxx\*.pb.cc
if exist %CD%\src\gRpc\cxx\*.pb.h del %CD%\src\gRpc\cxx\*.pb.h
if exist %CD%\src\Server\Services\gRpc\*.pb.cc del %CD%\src\Server\Services\gRpc\*.pb.cc
if exist %CD%\src\Server\Services\gRpc\*.pb.h del %CD%\src\Server\Services\gRpc\*.pb.h
echo [clean]: Java Client
if exist %CD%\src\gRpc\java\org rd /S /Q %CD%\src\gRpc\java\org