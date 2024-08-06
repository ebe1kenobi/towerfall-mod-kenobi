< NUL call config.bat
cd %REPO_PYTHON_SCRIPT_PATH%
%REPO_DRIVE%

@REM --aimod --aimodtraining --8pmod -nointro -loadlog -nogamepads -noquit -nogamepadupdates

@REM ------------------------------------------------------
@REM --8pmod -nointro
@REM ok
@REM python run_simple_agent.py
@REM pause


@REM ------------------------------------------------------
@REM --aimod -nointro
@REM ok
@REM python run_simple_agent.py
@REM pause

@REM ------------------------------------------------------
@REM --aimod --8pmod -nointro
@REM ok
@REM python run_simple_agent.py
@REM pause

@REM ------------------------------------------------------
@REM --aimodtraining
@REM ok ko
@REM python end_to_end_test.py
@REM ok ko
@REM python reset_test.py
@REM ok
@REM python run_simple_agent_darkworld_1agent.py
@REM ok
@REM python run_simple_agent_darkworld_2agent.py
@REM ok
@REM python run_simple_agent_darkworld_random_2agent.py
@REM ok
@REM python run_simple_agent_darkworld_1agent_1human.py
@REM ok
@REM python run_simple_agent_headhunter_1agent_vs_1agent.py
@REM ok
@REM python run_simple_agent_headhunter_1human_vs_1agent.py
@REM ok
@REM python run_simple_agent_headhunter_3agent.py
@REM ok
@REM python run_simple_agent_headhunter_4agent.py
@REM ok
@REM python run_simple_agent_headhunter_random_4agent.py
@REM ok
@REM python run_simple_agent_lastmanstanding_2agent_vs_2agent.py
@REM ok
@REM python run_simple_agent_quest_1agent.py
@REM ok
@REM python run_simple_agent_quest_1agent_1human.py
@REM ok
@REM python run_simple_agent_quest_random_2agent.py
@REM ok
@REM python run_simple_agent_sandbox.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_1human_and_1agent_vs_1human_and_1agent.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_2agent_vs_1human_and_1agent.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_2agent_vs_2agent.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_2agent_vs_2human.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_random_2agent_vs_2agent.py
@REM agent don't move, don't see enemy  TODO
@REM python run_simple_agent_trial_1agent.py
@REM agent don't move, don't see enemy  TODO
@REM python run_simple_agent_trial_random_1agent.py

@REM ------------------------------------------------------
@REM --aimodtraining --8pmod

@REM ok
@REM python run_simple_agent_darkworld_1agent.py
@REM ok
@REM python run_simple_agent_darkworld_1agent_1human.py
@REM ok
@REM python run_simple_agent_darkworld_2agent.py
@REM ok
@REM python run_simple_agent_darkworld_3agent.py
@REM ok
@REM python run_simple_agent_darkworld_4agent.py
@REM ok
@REM python run_simple_agent_darkworld_random_2agent.py
@REM ok
@REM python run_simple_agent_darkworld_random_4agent.py

@REM ok
@REM python run_simple_agent_headhunter_1agent_vs_1agent.py
@REM ok
@REM python run_simple_agent_headhunter_1human_vs_1agent.py
@REM ok
@REM python run_simple_agent_headhunter_3agent.py
@REM ok
@REM python run_simple_agent_headhunter_4agent.py
@REM ok
@REM python run_simple_agent_headhunter_5agent.py
@REM ok
@REM python run_simple_agent_headhunter_6agent.py
@REM ok
@REM python run_simple_agent_headhunter_7agent.py
@REM ok
@REM python run_simple_agent_headhunter_8agent.py
@REM ok
@REM python run_simple_agent_headhunter_random_4agent.py
@REM ok
@REM python run_simple_agent_headhunter_random_8agent.py

@REM ok
@REM python run_simple_agent_lastmanstanding_4agent.py
@REM ok
@REM python run_simple_agent_lastmanstanding_8agent.py

@REM ok
@REM python run_simple_agent_quest_1agent.py
@REM ok
@REM python run_simple_agent_quest_1agent_1human.py
@REM ok
@REM python run_simple_agent_quest_random_2agent.py

@REM ok
@REM python run_simple_agent_sandbox.py

@REM ok
@REM python run_simple_agent_teamdeathmatch_1human_and_1agent_vs_1human_and_1agent.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_1human_and_2agent_vs_3agent.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_2agent_vs_1human_and_1agent.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_2agent_vs_2agent.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_2agent_vs_2human.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_3agent_vs_3agent.py
@REM not possible : normal, documented!
@REM python run_simple_agent_teamdeathmatch_3agent_vs_4agent.py
@REM not possible : normal, documented!
@REM python run_simple_agent_teamdeathmatch_4agent_vs_4agent.py
@REM ok
@REM python run_simple_agent_teamdeathmatch_random_2agent_vs_2agent.py

@REM ok
@REM python run_simple_agent_trial_1agent.py
@REM ok
@REM python run_simple_agent_trial_random_1agent.py

pause