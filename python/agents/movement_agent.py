from .agent import Agent
from typing import Any, Mapping
from common.logging_options import default_logging
import logging

default_logging()
_VERBOSE = 0
_TIMEOUT = 10

class MovementAgent(Agent):

  def act(self, game_state: Mapping[str, Any]):
    # logging.info("act")

    if super().act(game_state):
      # logging.info("super().act True")
      return True

    if self.has_move():
      # logging.info("self.has_move() True")
      for move in self.shift_move():
        # logging.info("move in self.shift_move() : " + str(move))
        self.press(move)

    # logging.info("send_actions()")
    self.send_actions()

  def jump(self):
    # logging.info("jumpjumpjumpjumpjumpjumpjumpjumpjumpjumpjumpjumpjumpjump()")
    self.add_move(['j'])     

  def left(self):
    self.add_move(['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'])     
  
  def right(self):
    self.add_move(['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'],['r'])     
  
  def crouch(self):
    self.add_move(['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'],['d'])     

  def jump_up_left(self, jump_frame_number, frame_number):
    # logging.info("jump_up_left")

    for i in range(0, jump_frame_number):
      self.add_move(['jul'])
    for i in range(jump_frame_number, frame_number):
      self.add_move(['ul'])
    
    # logging.info("move in self.moves : " + str(self.moves))

  def shoot_right(self):
    self.add_move(['r'],     ['sr'],       ['r'])
    
  def shoot_up(self):
    self.add_move(['su'],      ['u'],     ['l'], ['l'], ['l'], ['l'], ['l'], ['l'], ['l'], ['l'], ['l'], ['l'], ['l'])
    
  def shoot_up_left(self):
    self.add_move(['sul'],         ['ul'])
    
  def jump_up_shoot_up(self):
    self.add_move(['uj'],['uj'],['uj'],['uj'],['uj'],['uj'],    ['suj'],    ['u'],     ['l'], ['l'], ['l'], ['l'], ['l'], ['l'], ['l'], ['l'])

  def jump_up_shoot_down(self):
    self.add_move(['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],       ['ds'],           ['d'])

  def jump_up_shoot_up_left(self):
    self.add_move(['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],['j'],     ['sl'],      ['l'])
  
  def jump_left_shoot_up(self):
    self.add_move(['ujs'],       ['uj'])
  
  def jump_left_shoot_down(self):
    self.add_move(['ujs'],    ['uj'])
  
  def jump_left_shoot_left(self):
    self.add_move(['jul'],          ['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],     ['ls'],    ['l'])

  def jump_left_shoot_right(self):
    self.add_move(['jul'],       ['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],     ['rs'],     ['r'])    
    
  
  def jump_left_shoot_up_left(self):
    self.add_move(['jul'],          ['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],     ['uls'],    ['ul'])

  def jump_left_shoot_down_left(self):
    self.add_move(['jul'],      ['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],['ul'],     ['dls'],       ['dl'])

  def super_jump_left(self):
    self.add_move(['l'],    ['dl'],['dl'],     ['dlz']    ,['lj'],    ['l'],    ['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'])

  def hyper_jump_left_1(self):
    self.add_move(['l'],    ['dl'],['dl'],     ['dlz'],    ['dl'],     ['dlz'],     ['lj'],      ['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'])

  def hyper_jump_left_2(self):
    self.add_move(['l'],    ['dl'],['dl'],    ['dlz'],    ['dl'],     ['dlz'],     ['zlj'],       ['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'],['zlj'])

  def dash_left(self):
    self.add_move(['zl'],    ['z'],    ['z'],['z'],['z'],['z'],['z'],['z'],['z'],['z'],['z'],['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'], ['z'])

  def super_dash_left(self):
    self.add_move(['l'],['l'],['l'],    ['lz'],    ['l'],    ['lz'],       ['lz'],['lz'],['lz'],['lz'],      ['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'],['l'])

  def hyper_dash_left(self):
    self.add_move(['l'],    ['dl'],['dl'],    ['dlz'],    ['dl'],     ['dlz'],       ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'], ['dlz'],['dlz'],['dlz'],['dlz'],      )


#   def jump_up_dash_left(self):
#   def jump_koala(self):
#   def jump_against_wall(self):
#   def jump_to_ceiling(self):
#   def shoot_left_and_catch_the_arrow(self):