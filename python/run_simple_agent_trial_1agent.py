from common.logging_options import default_logging
from towerfall import Towerfall
import logging

default_logging()
_VERBOSE = 1
_TIMEOUT = 10

def main():
  # Creates or reuse a Towerfall game.
  towerfall = Towerfall(
    verbose = 0,
    config = dict(
      # option for training mode
      # randomLevel = True, # True False
      level=1, # mandatory if randomLevel != True
      subLevel=1, # mandatory for Trial only if randomLevel != True
      # Minimum option for play game normally
      mode='Trials', #Quest, DarkWorld, Trials, LastManStanding, HeadHunters, TeamDeathmatch, ( not supported = Warlord)
      fps=60,
      agentTimeout='01:00:00',
      agents=[
        dict(type='remote', archer='blue')
        ],
    )
  )

  towerfall.run()


if __name__ == '__main__':
  main()