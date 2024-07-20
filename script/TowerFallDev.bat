echo ==============================================================
echo =       EXECUTE TOWERFALL WITH PYTHON IN REPO DIRECTORY      =
echo ==============================================================

< NUL call config.bat
cd %REPO_PYTHON_SCRIPT_PATH%
%REPO_DRIVE%
python run_simple_agent.py
@REM pause