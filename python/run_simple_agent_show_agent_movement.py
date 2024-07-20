from typing import Any, Mapping
from common.logging_options import default_logging
from towerfall import Towerfall
from agents import MovementAgent
import logging
import time

default_logging()

last_pos = dict(x=300, y=20)

def main():
  global last_pos
  # Creates or reuse a Towerfall game.
  towerfall = Towerfall(
    verbose = 0,
    config = dict(
      mode='sandbox',
      level=2,
      fps=60,
      # solids = [[0] * 32]*14 + [[1] * 32]*1+ [[0] * 32]*9,
      solids =  
      # + [[0] * 32]*23,
      
      # [[1] * 32]*1+
      [[1] * 1 + [0] * 30 + [1] * 1]*7
      # + [[0]*2+ [1]*2+ [0]*2+ [1]*2+ [0]*2+ [1]*2+ [0]*2+ [1]*2+ [0]*2+ [1]*2+ [0]*2+ [1]*2+ [0]*2+ [1]*2+ [0]*2+ [1]*2] 
      + [[0] * 32]*1 
      + [[1] * 1 + [0] * 30 + [1] * 1]*15
      + [[1] * 1 + [0] * 10 +  [1] * 20 + [1] * 1]*1
      ,
      agentTimeout='00:00:02',
      agents=[
        dict(type='remote', archer='white-alt', ai='MovementAgent')]
    )
  )

  connection = towerfall.join(timeout=20, verbose=0)

  send_reset(towerfall, last_pos)
  agent = MovementAgent(connection)

  movements = [
      # 'reset', dict(x=310, y=20)],
    'jump',
    'left',
    'right',
    'crouch',
    
    # ['reset', dict(x=310, y=170)],
    # the more we stay press on jump, the higher we go
    # the more we stay press on the direction left up, the farthest we go
    # jump up left, nb frame to press jump and up and left, then number of frame total to press left and up without jump
    ['jump_up_left', 0, 10,],
    # ['jump_up_left', 1, 10,],
    # ['jump_up_left', 2, 10,],
    # ['jump_up_left', 3, 10,],
    # ['jump_up_left', 4, 10,],
    # ['jump_up_left', 5, 10,],
    # ['jump_up_left', 6, 10,],
    # ['jump_up_left', 7, 10,],
    # ['jump_up_left', 8, 10,],
    # ['jump_up_left', 9, 10,],
    ['jump_up_left', 10, 10,],    
    # ['jump_up_left', 0, 20,],
    # ['jump_up_left', 1, 20,],
    # ['jump_up_left', 2, 20,],
    # ['jump_up_left', 3, 20,],
    # ['jump_up_left', 4, 20,],
    # ['jump_up_left', 5, 20,],
    # ['jump_up_left', 6, 20,],
    # ['jump_up_left', 7, 20,],
    # ['jump_up_left', 8, 20,],
    # ['jump_up_left', 9, 20,],
    ['jump_up_left', 10, 20,],
    ['jump_up_left', 20, 20,],
    ['jump_up_left', 30, 30,],
    ['jump_up_left', 40, 40,],
    ['jump_up_left', 50, 50,],
    ['jump_up_left', 60, 60,],
    ['jump_up_left', 70, 70,],
    ['jump_up_left', 80, 80,],

    'shoot_right',
    'shoot_up',
    'shoot_up_left',
    'jump_up_shoot_up',
    'jump_up_shoot_down',
    'jump_up_shoot_up_left',
    'jump_left_shoot_right',
    'jump_left_shoot_up_left',
    'jump_left_shoot_down_left',
    
    'super_jump_left',
    'hyper_jump_left_1',
    'hyper_jump_left_2',

    'dash_left',
    'super_dash_left',
    'hyper_dash_left',

    # 'jump_up_dash_left',
    # 'jump_koala',
    # 'jump_against_wall',
    # 'jump_to_ceiling',
    # 'shoot_left_and_catch_the_arrow',
  ]

  i = -1
  counter = 0
  while True:
    # Read the state of the game then replies with an action.
    game_state = connection.read_json()
    # check_for_end_condition(game_state, towerfall)
    if agent.act(game_state):
      # logging.info("agent.act(game_state) True")
      continue
    #wait before each action to end
    if counter > 0 and counter < 100:
      # logging.info("counter > 0 and counter < 100 True :counter = " + str(counter))
      counter += 1
      continue

    # time.sleep(5)
    send_reset(towerfall, last_pos)

    counter = 1

    i += 1 

    # loop
    if i >= len(movements):
      i = 0

    logging.info("New movement : " + str(movements[i]))
    logging.info("type(movements[i]) == " + str(type(movements[i])))

    if isinstance(movements[i], list):
      if movements[i][0] == "reset":
        logging.info("last_pos 1  " + str(last_pos))
        last_pos = movements[i][1]
        logging.info("last_pos 1  " + str(last_pos))
        send_reset(towerfall, last_pos)
    #     counter = 0
      else: #len(movements[i]) == 3:
        getattr(agent, movements[i][0])(movements[i][1], movements[i][2])
    else:
      getattr(agent, movements[i])()
    

def send_reset(towerfall: Towerfall, position):

  # if not isinstance(solids, list):
  # if last_pos == 0:
  #   logging.info("last_pos = 0")
  #   return

  # logging.info("towerfall.send_reset " + str(position))
  towerfall.send_reset([
      dict(type='archer', pos=position),
    ]
  )
  # else:
  #   logging.info("towerfall.send_config")
  #   towerfall.send_config(
  #     dict(
  #       mode='sandbox',
  #       level=2,
  #       fps=60,
  #       agentTimeout='00:00:02',
  #       solids=solids,
  #       agents=[
  #         dict(type='remote', archer='white-alt')]
  #     )
  #     )
  #   logging.info("towerfall.send_reset")
  #   towerfall.send_reset([
  #     dict(type='archer', pos=position),
  #   ] )          


def check_for_end_condition(game_state: Mapping[str, Any], towerfall: Towerfall):
  # The sandbox mode will not end the game automatically. We need to explicitly request a reset.
  if game_state['type'] != 'update':
    return

  is_player_alive = False
  is_enemy_alive = False
  for state in game_state['entities']:
    if state['type'] == 'archer':
      is_player_alive = True
    if state['isEnemy']:
      is_enemy_alive = True
  if not is_player_alive or not is_enemy_alive:
    send_reset(towerfall)


if __name__ == '__main__':
  main()