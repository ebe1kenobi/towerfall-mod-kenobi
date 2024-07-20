from common.logging_options import default_logging
from towerfall import Towerfall

default_logging()

def main():
  # Creates or reuse a Towerfall game.
  towerfall = Towerfall(
    verbose = 0,
    config = dict(
      # option for training mode
      matchLengths = 'Quick', #Instant,Quick,Standard,Epic
      # randomLevel = True, # True False
      level=3, # mandatory if randomLevel != True
      # Minimum option for play game normally
      mode='HeadHunters', #Quest, DarkWorld, Trials, LastManStanding, HeadHunters, TeamDeathmatch, ( not supported = Warlord)
      fps=60,
      agentTimeout='01:00:00',
      agents=[
        dict(type='remote', archer='white'),
        dict(type='remote', archer='orange'),
        dict(type='remote', archer='blue'),
        
        ],
    )
  )

  towerfall.run()


if __name__ == '__main__':
  main()