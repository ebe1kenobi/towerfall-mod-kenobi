import logging
import random
from typing import Any, Mapping
from common.logging_options import default_logging

from towerfall import Connection
import logging

default_logging()
_VERBOSE = 0
_TIMEOUT = 10

class NoMoveAgent:
  '''
  params connection: A connection to a Towerfall game.
  params attack_archers: If True, the agent will attack other neutral archers.
  '''
  # def __init__(self, connection: Connection, attack_archers: bool = False):
  def __init__(self, connection: Connection):
    self.state_init: Mapping[str, Any] = {}
    self.state_scenario: Mapping[str, Any] = {}
    self.state_update: Mapping[str, Any] = {}
    self.pressed = set()
    self.connection = connection
    # self.attack_archers = attack_archers

  def act(self, game_state: Mapping[str, Any]):
    '''
    Handles a game message.
    '''

    # There are three main types to handle, 'init', 'scenario' and 'update'.
    # Check 'type' to handle each accordingly.
    if game_state['type'] == 'init':
      # 'init' is sent every time a match series starts. It contains information about the players and teams.
      # The seed is based on the bot index so each bots acts differently.
      self.state_init = game_state
      random.seed(self.state_init['index'])
      # Acknowledge the init message.
      self.connection.send_json(dict(type='result', success=True))
      return True

    # Add game mode # AiMod.Config.mode == GameModes.Quest
    if game_state['type'] == 'scenario': 
      # 'scenario' informs your bot about the current state of the ground. Store this information
      # to use in all subsequent loops. (This example bot doesn't use the shape of the scenario)
      self.state_scenario = game_state
      # logging.info(str(game_state))

      # Acknowledge the scenario message.
      self.connection.send_json(dict(type='result', success=True))
      return

    if game_state['type'] == 'update':
      # 'update' informs the state of entities in the map (players, arrows, enemies, etc).
      self.state_update = game_state

      #NOMOVE

    self.send_actions()

  def press(self, b):
    self.pressed.add(b)

  def send_actions(self):
    assert self.state_update
    self.connection.send_json(dict(
      type = 'actions',
      actions = ''.join(self.pressed),
      id = self.state_update['id']
    ))
    self.pressed.clear()
