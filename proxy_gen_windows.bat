@echo off
SET GRPC_TOOLS=D:\Workspace\vcpkg\installed\x64-windows\tools

echo [grpc]: .net Server/Client
%GRPC_TOOLS%\protoc.exe -Isrc/proto --csharp_opt=file_extension=.g.cs --csharp_out src/Common/gRpc/csharp --grpc_out src/Common/gRpc/csharp --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_csharp_plugin.exe src\proto\*.proto
rem %GRPC_TOOLS%\protoc.exe -Isrc/Common/proto --php_out src/Common/gRpcClient/php --grpc_out src/Server/gRpcServer src\Common\proto\FeatureService.proto src\Common\proto\ResourceService.proto src\Common\proto\RenderingService.proto --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_csharp_plugin.exe
rem %GRPC_TOOLS%\protoc.exe -Isrc/Common/proto --csharp_out src/Common/gRpcClient/csharp --grpc_out src/Server/gRpcServer src\Common\proto\FeatureService.proto src\Common\proto\ResourceService.proto src\Common\proto\RenderingService.proto --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_csharp_plugin.exe
