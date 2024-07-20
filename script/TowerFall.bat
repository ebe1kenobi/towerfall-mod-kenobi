@echo off

echo =============================================
echo =       EXECUTE TOWERFALL WITH PYTHON       =
echo =============================================

< NUL call config.bat

cd %TOWERFALL_PYTHON_PATH%
%GAME_DRIVE%

python run_simple_agent.py
@REM pause