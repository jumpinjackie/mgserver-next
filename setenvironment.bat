@echo off
SET VCPKG_ROOT=D:\Workspace\vcpkg
SET PATH=%PATH%;%VCPKG_ROOT%\downloads\cmake-3.8.0-rc1-win32-x86\bin
SET GRPC_TOOLS=%VCPKG_ROOT%\installed\x64-windows\tools

call proxy_gen_windows.bat