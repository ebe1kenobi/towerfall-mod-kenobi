from common.logging_options import default_logging
from towerfall import Towerfall

default_logging()

def main():
  # Creates or reuse a Towerfall game.
  towerfall = Towerfall(
    verbose = 0,
    config = dict(
      # option for training mode
      difficulty='Normal', # Normal, Hardcore, Legendary
      # randomLevel = True, # True False
      level=3, # mandatory if randomLevel != True
      # Minimum option for play game normally
      mode='DarkWorld', #Quest, DarkWorld, Trials, LastManStanding, HeadHunters, TeamDeathmatch, ( not supported = Warlord)
      fps=60,
      agentTimeout='01:00:00',
      agents=[
        dict(type='human', archer='yellow'),
        dict(type='remote', archer='red')],
    )
  )

  towerfall.run()


if __name__ == '__main__':
  main()