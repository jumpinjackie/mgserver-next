@echo off
SET GRPC_TOOLS=D:\Workspace\vcpkg\installed\x64-windows\tools

echo [grpc]: .net Server/Client
%GRPC_TOOLS%\protoc.exe -Isrc/proto --csharp_opt=file_extension=.g.cs --csharp_out src/Common/gRpc/csharp --grpc_out src/Common/gRpc/csharp --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_csharp_plugin.exe src\proto\*.proto
echo [grpc]: PHP Client
%GRPC_TOOLS%\protoc.exe -Isrc/proto --php_out src/Common/gRpc/php --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_php_plugin.exe src\proto\*.proto
echo [grpc]: C++ Server/Client
%GRPC_TOOLS%\protoc.exe -Isrc/proto --csharp_opt=file_extension=.g.cs --cpp_out src/Common/gRpc/cxx --grpc_out src/Common/gRpc/cxx --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_cpp_plugin.exe src\proto\*.proto
echo [grpc]: Java Client
%GRPC_TOOLS%\protoc.exe -Isrc/proto --java_out src/Common/gRpc/java src\proto\*.proto
