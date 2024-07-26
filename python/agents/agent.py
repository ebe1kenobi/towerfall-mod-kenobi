import logging
import random
from typing import Any, Mapping
from common.logging_options import default_logging

from towerfall import Connection
import logging

default_logging()
_VERBOSE = 0
_TIMEOUT = 10

class Agent:
  def __init__(self, connection: Connection):
    # logging.info('agent.__init__')
    self.state_init: Mapping[str, Any] = {}
    self.state_scenario: Mapping[str, Any] = {}
    self.state_update: Mapping[str, Any] = {}
    self.pressed = set()
    self.connection = connection
    self.moves = []
    self.my_state = []
    self.players = []
    # self.attack_achers = attack_archers

  def act(self, game_state: Mapping[str, Any]):
    '''
    Handles a game message.
    '''
    # logging.info('agent.act')

    if game_state['type'] == 'notplaying':
      # logging.info('game_state.type = ' + str(game_state['type']))
      # 'notplaying' is sent every time a match series starts for an agents not selected to play
      # Acknowledge the init message.
      self.connection.send_json(dict(type='result', success=True, id = game_state['id']))
      return True    

    # There are three main types to handle, 'init', 'scenario' and 'update'.
    # Check 'type' to handle each accordingly.
    if game_state['type'] == 'init':
      # logging.info('game_state.type = ' + str(game_state['type']))

      # 'init' is sent every time a match series starts. It contains information about the players and teams.
      # The seed is based on the bot index so each bots acts differently.
      self.state_init = game_state
      random.seed(self.state_init['index'])
      # Acknowledge the init message.
      # logging.info('send_json : ' + str(dict(type='result', success=True)))
      self.connection.send_json(dict(type='result', success=True))
      return True

    # Add game mode # AiMod.Config.mode == GameModes.Quest
    if game_state['type'] == 'scenario': 
      # logging.info('game_state.type = ' + str(game_state['type']))
      # 'scenario' informs your bot about the current state of the ground. Store this information
      # to use in all subsequent loops. (This example bot doesn't use the shape of the scenario)
      self.state_scenario = game_state
      # logging.info(str(game_state))

      # Acknowledge the scenario message.
      # logging.info('send_json : ' + str(dict(type='result', success=True)))
      self.connection.send_json(dict(type='result', success=True))
      return True

    if game_state['type'] == 'update':
      # logging.info('game_state.type = ' + str(game_state['type']))
      # 'update' informs the state of entities in the map (players, arrows, enemies, etc).
      self.state_update = game_state

    self.my_state = None

    self.players = []

    for state in self.state_update['entities']:
      if state['type'] == 'archer':
        self.players.append(state)
        if state['playerIndex'] == self.state_init['index']:
          self.my_state = state

    # If the agent is not present, it means it is dead.
    if self.my_state == None:
      # You are required to reply with actions, or the agent will get disconnected.
      # logging.info('send_actions')
      self.send_actions()
      return True  

    return False    

  def press(self, b):
    # logging.info('agent.press')
    self.pressed.add(b)

  def send_actions(self):
    # logging.info('agent.send_actions ' + str(self.pressed))
    assert self.state_update
    self.connection.send_json(dict(
      type = 'actions',
      actions = ''.join(self.pressed),
      id = self.state_update['id']
    ))
    self.pressed.clear()

  def add_move(self, *moves):
    # logging.info('agent.add_move')
    self.moves.extend(moves)
  
  def has_move(self):
    # logging.info('agent.has_move')
    return True if len(self.moves) > 0 else False

  def shift_move(self):
    # logging.info('agent.shift_move')
    move = self.moves[0]
    self.moves.pop(0)
    return move    

  def has_arrow(self, my_state):
    # logging.info('agent.has_arrow')
    return len(my_state['arrows']) > 0

  def has_floor(self, my_state):
    # logging.info('agent.has_floor')
    return my_state['onGround']

  def is_playtag_on(self):
    return self.my_state['playTagOn']

  def is_tag(self):
    return self.my_state['playTag']      
