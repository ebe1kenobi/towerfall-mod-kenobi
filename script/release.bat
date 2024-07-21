@echo off

echo ================================
echo =       BUILD RELEASE          =
echo ================================

< NUL call config.bat

echo %REPO_RELEASE_PATH%

rmdir /S /Q %REPO_RELEASE_PATH%
mkdir %REPO_RELEASE_PATH%

copy "%REPO_BUILD_PATH%%PDB_MOD_FILE_NAME%" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%CommandLine.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%Microsoft.Threading.Tasks.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%Microsoft.Threading.Tasks.Extensions.Desktop.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%Microsoft.Threading.Tasks.Extensions.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%Mono.Cecil.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%Newtonsoft.Json.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%%DLL_PATCHERLIB_NAME%" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%System.IO.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%System.Runtime.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%System.Threading.Tasks.dll" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%%DLL_MOD_FILE_NAME%" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%%EXE_PATCHER_NAME%" %REPO_RELEASE_PATH%
copy "%REPO_BUILD_PATH%Mono.Cecil.dll" %REPO_RELEASE_PATH%

xcopy /E /I %REPO_PYTHON_SCRIPT_PATH%  %REPO_RELEASE_PATH%\python

xcopy /E /I %REPO_PATH%modFile\* %REPO_RELEASE_PATH%
