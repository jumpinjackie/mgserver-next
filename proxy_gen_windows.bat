@echo off
echo [grpc]: .net Server/Client
%GRPC_TOOLS%\protoc.exe -Isrc/proto --csharp_opt=file_extension=.g.cs --csharp_out src/gRpc/csharp --grpc_out src/gRpc/csharp --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_csharp_plugin.exe src\proto\*.proto src\proto\MdfModel\*.proto
echo [grpc]: PHP Client
%GRPC_TOOLS%\protoc.exe -Isrc/proto --php_out src/gRpc/php --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_php_plugin.exe src\proto\*.proto src\proto\MdfModel\*.proto
echo [grpc]: C++ Messages
%GRPC_TOOLS%\protoc.exe -Isrc/proto --cpp_out=dllexport_decl=MG_PROTO_MESSAGES_API:src/gRpc/cxx --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_cpp_plugin.exe src\proto\*.proto src\proto\MdfModel\*.proto
echo [grpc]: C++ Server
%GRPC_TOOLS%\protoc.exe -Isrc/proto --grpc_out src/Server/Services/gRpc --plugin=protoc-gen-grpc=%GRPC_TOOLS%\grpc_cpp_plugin.exe src\proto\*.proto src\proto\MdfModel\*.proto
echo [grpc]: Java Client
%GRPC_TOOLS%\protoc.exe -Isrc/proto --java_out src/gRpc/java src\proto\*.proto src\proto\MdfModel\*.proto
