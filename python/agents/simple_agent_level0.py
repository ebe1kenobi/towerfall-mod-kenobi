from .agent import Agent
from typing import Any, Mapping
import logging
import random
from typing import Any, Mapping
from common.logging_options import default_logging

from towerfall import Connection
import logging

default_logging()
_VERBOSE = 0
_TIMEOUT = 10

class SimpleAgentLevel0(Agent):
  
  def act(self, game_state: Mapping[str, Any]):
    
    if super().act(game_state):
      return True 

    enemy_state = None

    # Try to find an enemy archer.
    for state in self.players:
      if state['playerIndex'] == self.my_state['playerIndex']:
        continue
      
      if (self.state_scenario['mode'] != "Quest" and self.state_scenario['mode'] != "DarkWorld") \
        and ((state['team'] == 'neutral') or state['team'] != self.my_state['team']):
        enemy_state = state
        break

    # If no enemy archer is found, try to find another enemy.
    if not enemy_state:
      for state in self.state_update['entities']:
        if state['isEnemy']:
          enemy_state = state

    # If no enemy is found, means all are dead.
    if enemy_state == None:
      self.send_actions()
      return

    my_pos = self.my_state['pos']
    enemy_pos = enemy_state['pos']
    if enemy_pos['y'] >= my_pos['y'] and enemy_pos['y'] <= my_pos['y'] + 50:
      # Runs away if enemy is right above
      if my_pos['x'] < enemy_pos['x']:
        self.press('l')
      else:
        self.press('r')
    else:
      # Runs to enemy if they are below
      if my_pos['x'] < enemy_pos['x']:
        self.press('r')
      else:
        self.press('l')

      # If in the same line shoots,
      if abs(my_pos['y'] - enemy_pos['y']) < enemy_state['size']['y']:
        if random.randint(0, 1) == 0:
          self.press('s')

    # Presses dash in 1/10 of the frames.
    if random.randint(0, 9) == 0:
      self.press('z')

    # Presses jump in 1/20 of the frames.
    if random.randint(0, 19) == 0:
      self.press('j')

    # Respond the update frame with actions from this agent.
    # logging.info('SimpleAgentLevel0.send_actions')
    self.send_actions()

