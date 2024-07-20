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

class SimpleAgentLevel1(Agent):

  def act(self, game_state: Mapping[str, Any]):
    
    if super().act(game_state):
      return True 

    enemy_state = None

    # Try to find an enemy archer.
    for state in self.players:
      if state['playerIndex'] == self.my_state['playerIndex']:
        continue
      #search the tag Player if not himself
      if self.is_playtag_on() and not self.is_tag():
        if state['playTag'] == True:
          enemy_state = state
          break        
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

    # If no enemy is found, means all are dead
    if enemy_state == None:
      self.send_actions()
      return

    my_pos = self.my_state['pos']
    enemy_pos = enemy_state['pos']
    
    # simple management of playtag mode pursue or flee
    if self.is_playtag_on():
      if self.is_tag(): # pursue
        if my_pos['x'] < enemy_pos['x']:
          self.press('r')
        else:
          self.press('l')
      else: # flee
        if my_pos['x'] < enemy_pos['x']:
          self.press('l')
        else:
          self.press('r')
        
    elif not self.has_move():
      # logging.info("not has_move")

      if enemy_pos['y'] >= my_pos['y'] and enemy_pos['y'] <= my_pos['y'] + 50:
        # Runs away if enemy is right above
        if my_pos['x'] < enemy_pos['x']:
          self.press('l')
        else:
          self.press('r')
      else:
        # If in the same vertical  line (+/- player width) above shoots,
        if self.enemy_is("up", self.my_state, enemy_state) \
           and self.can_shoot("up", self.my_state, enemy_state):
          # if random.randint(0, 1) == 0:
          self.add_move(['s', 'u'], ['u'])
          # logging.info("add_move su u : " + str(self.moves))

        # If in the same vertical line  (+/- player width) below shoots,
        elif self.enemy_is("down", self.my_state, enemy_state) \
            and self.can_shoot("down", self.my_state, enemy_state):
          # if random.randint(0, 1) == 0:
          self.add_move(['s', 'd'], ['d'])            
          # logging.info("add_move sd d : " + str(self.moves))

        # If in the same horizontal line  (+/- player height) shoots,
        elif self.enemy_is("right", self.my_state, enemy_state) \
          and self.can_shoot("right", self.my_state, enemy_state):
          # if random.randint(0, 1) == 0:
          self.add_move(['s', 'r'], ['r'])            
          # logging.info("add_move sr r : " + str(self.moves))
          # self.press('s')

        elif self.enemy_is("left", self.my_state, enemy_state) \
          and self.can_shoot("left", self.my_state, enemy_state):
          # if random.randint(0, 1) == 0:
          self.add_move(['s', 'l'], ['l'])            
          # logging.info("add_move sl l : " + str(self.moves))
          # self.press('s')

        else:
          # Runs to enemy if they are below
          if my_pos['x'] < enemy_pos['x']:
            self.press('r')
          else:
            self.press('l')

    if self.has_move():
      for move in self.shift_move():
        # logging.info("move in self.shift_move() : " + str(move))
        self.press(move)
    else:
      # Presses dash in 1/10 of the frames.
      if random.randint(0, 9) == 0:
        self.press('z')

      # Presses jump in 1/20 of the frames.
      if random.randint(0, 19) == 0:
        self.press('j')

    # Respond the update frame with actions from this agent.
    self.send_actions()


  def enemy_is(self, position, my_state, enemy_state):
    match position:
      case "up":
        return abs(my_state['pos']['x'] - enemy_state['pos']['x']) < enemy_state['size']['x'] \
               and my_state['pos']['y'] - enemy_state['pos']['y'] < 0
      
      case "down":
        return abs(my_state['pos']['x'] - enemy_state['pos']['x']) < enemy_state['size']['x'] \
               and my_state['pos']['y'] - enemy_state['pos']['y'] > 0

      case "right":
        return my_state['pos']['y'] - enemy_state['pos']['y'] < enemy_state['size']['y'] \
               and my_state['pos']['x'] - enemy_state['pos']['x'] < 0

      case "left":
        return my_state['pos']['y'] - enemy_state['pos']['y'] < enemy_state['size']['y'] \
               and my_state['pos']['x'] - enemy_state['pos']['x'] > 0

      case _:
        return False

  def can_shoot(self, direction, my_state, enemy_state):
    return self.has_arrow(my_state) \
           and not self.has_wall_between(direction, my_state, enemy_state) \
           and self.distance_to_shoot_ok(direction, my_state, enemy_state) \
           and (direction != "down" or not self.has_floor(my_state))



  def distance_to_shoot_ok(self, direction, my_state, enemy_state):
    my_cell = self.get_cell_from_position(my_state['pos'])
    enemy_cell = self.get_cell_from_position(enemy_state['pos'])
    if direction == "up":
      if abs(my_cell[1] - enemy_cell[1] <= 10):
        # logging.info("my_cell[1] - enemy_cell[1] <= 10 : " + str(my_cell[1]) + " - " + str(enemy_cell[1]))
        return True
    elif direction == "down":
      return True        
    else: # direction == "left" or direction == "right":
      if abs(my_cell[0] - enemy_cell[0] <= 10):
        # logging.info("my_cell[0] - enemy_cell[0] <= 10 : " + str(my_cell[0]) + " - " + str(enemy_cell[0]))
        return True      
    return False

  def has_wall_between(self, direction, my_state, enemy_state):
    # self.state_scenario
    # determine where the player are in the scenario table : the cell x,y
    my_cell = self.get_cell_from_position(my_state['pos'])
    enemy_cell = self.get_cell_from_position(enemy_state['pos'])
    # logging.info("self.state_scenario : " + str(self.state_scenario))
    # logging.info("my_state : " + str(my_state['pos']))
    # logging.info("enemy_state : " + str(enemy_state['pos']))
    # logging.info("my_cell : " + str(my_cell))
    # logging.info("enemy_cell : " + str(enemy_cell))
    
    if direction == "up":
      # logging.info("direction : " + direction)
      # search a wall from X,Y1 to X,Y2 because they are on the same vertical line, sort of...
      for i in range(my_cell[1], enemy_cell[1]):
        # logging.info("up-i : " + str(i) + " in range("+str(my_cell[1])+","+str(enemy_cell[1])+")")
        # logging.info("self.state_scenario['grid']"+str(my_cell[0])+"] : " + str(self.state_scenario['grid'][my_cell[0]]))
        if self.state_scenario['grid'][my_cell[0]][i] == 1:
          # logging.info("wall found in self.state_scenario['grid']"+str(my_cell[0])+"]["+str(i)+"]")
          return True
      return False  

    if direction == "down":
      # logging.info("direction : " + direction)
      for i in range(enemy_cell[1], my_cell[1]):
        # logging.info("down i : " + str(i) + " in range("+str(enemy_cell[1])+","+str(my_cell[1])+")")
        # logging.info("self.state_scenario['grid']["+str(my_cell[0])+"] : " + str(self.state_scenario['grid'][my_cell[0]]))
        if self.state_scenario['grid'][my_cell[0]][i] == 1:
          # logging.info("wall found in self.state_scenario['grid']["+str(my_cell[0])+"]["+str(i)+"]")
          return True
      return False 

    if direction == "right":
      # logging.info("direction : " + direction)
      for i in range(my_cell[0], enemy_cell[0]):
        # logging.info("right i : " + str(i) + " in range("+str(my_cell[0])+","+str(enemy_cell[0])+")")
        # logging.info("self.state_scenario['grid']["+str(i)+"] : " + str(self.state_scenario['grid'][i]))
        if self.state_scenario['grid'][i][my_cell[1]] == 1:
          # logging.info("wall found in self.state_scenario['grid']["+str(i)+"]["+str(my_cell[1])+"]")
          return True
      return False     

    if direction == "left":
      # logging.info("direction : " + direction)
      for i in range(enemy_cell[0], my_cell[0]):
        # logging.info("left i : " + str(i) + " in range("+str(enemy_cell[0])+","+str(my_cell[0])+")")
        # logging.info("self.state_scenario['grid']["+str(i)+"] : " + str(self.state_scenario['grid'][i]))
        if self.state_scenario['grid'][i][my_cell[1]] == 1:
          # logging.info("wall found in self.state_scenario['grid']["+str(i)+"]["+str(my_cell[1])+"]")
          return True
      return False                


  def get_cell_from_position(self, position):
    cell = [0,0]
    cell[0] = int(position['x']) // 10
    cell[1] = int(position['y']) // 10
    return cell
