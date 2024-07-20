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
    timeout = 60000,
    config = dict(
      # option for training mode
      matchLengths = 'Quick', #Instant,Quick,Standard,Epic
      # randomLevel = True, # True False
      level=4, # mandatory if randomLevel != True
      # Minimum option for play game normally
      mode='LastManStanding', #Quest, DarkWorld, Trials, LastManStanding, HeadHunters, TeamDeathmatch, ( not supported = Warlord)
      fps=60,
      agentTimeout='01:00:00',
      agents=[
        dict(type='remote', archer='white'),
        dict(type='remote', archer='orange'),
        dict(type='remote', archer='blue'),
        dict(type='remote', archer='red'),
        dict(type='remote', archer='yellow'),
        dict(type='remote', archer='green'),
        dict(type='remote', archer='purple'),
        dict(type='remote', archer='pink'),
      ]
    )
  )

  towerfall.run()


if __name__ == '__main__':
  main()