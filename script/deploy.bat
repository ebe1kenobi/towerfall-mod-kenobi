@echo off

echo =================================
echo =       DEPLOY RELEASE          =
echo =================================

< NUL call config.bat

xcopy /E /S /Y "%REPO_RELEASE_PATH%*" %TOWERFALL_PATH%

cd %TOWERFALL_PATH%
%TOWERFALL_DRIVE%

taskkill /IM %EXE_TOWERFALL_NAME%
@REM timeout 5
del "%EXE_TOWERFALL_NAME%"
copy %EXE_TOWERFALL_ORIGINAL_NAME% %EXE_TOWERFALL_NAME%
rmdir /S /Q "Patcher"

@REM create EXE_PATCHED_NAME file
%EXE_PATCHER_NAME%

del "%EXE_TOWERFALL_NAME%"
copy %EXE_PATCHED_NAME% %EXE_TOWERFALL_NAME%
del %EXE_PATCHED_NAME%

@REM pause
