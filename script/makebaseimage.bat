@echo off

echo =================================
echo =       MAKE UNSEALED EXE       =
echo =================================

< NUL call config.bat

cd %REPO_BUILD_PATH%
%REPO_DRIVE%
%EXE_PATCHER_NAME% makebaseimage -t %EXE_TOWERFALL_NAME%
pause