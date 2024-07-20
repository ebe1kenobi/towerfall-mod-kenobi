@echo OFF
< NUL call config.bat

cd %REPO_SCRIPT_PATH%
%REPO_DRIVE%
< NUL call release.bat

cd %REPO_SCRIPT_PATH%
%REPO_DRIVE%
< NUL call deploy.bat

cd %REPO_SCRIPT_PATH%
%REPO_DRIVE%
< NUL call testMod.bat
@REM < NUL call TowerFallDev.bat

pause
