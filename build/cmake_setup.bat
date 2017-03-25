@echo off
SET PLAT=
SET CONF=
SET CMAKE_GENERATOR=

if "%1" == "" goto error
if "%1" == "Win32" (
    set PLAT=%1
    set CMAKE_GENERATOR="Visual Studio 15 2017"
    goto setup_config
)
if "%1" == "Win64" (
    set PLAT=%
    set CMAKE_GENERATOR="Visual Studio 15 2017 Win64"
    goto setup_config
)
goto error

:setup_config
if "%2" == "" goto error
if "%2" == "Debug" (
    set CONF=%1
    goto ready
)
if "%2" == "Release" (
    set CONF=%1
    goto ready
)

:ready
set BUILD_DIR=%PLAT%_%CONF%
cmake -G %CMAKE_GENERATOR% -B%BUILD_DIR% -DCMAKE_BUILD_TYPE=%CONF% -DCMAKE_TOOLCHAIN_FILE=%VCPKG_ROOT%\scripts\buildsystems\vcpkg.cmake ..
goto done

:error
echo Usage:
echo cmake_setup.bat [Win32 -OR- Win64] [Debug -OR- Release]

:done