from towerfall import Towerfall

def main():
  # Creates or reuse a Towerfall game.
  towerfall = Towerfall(
    verbose = 0,
    config = dict(
      mode='HeadHunters', #Quest, DarkWorld, Trials, LastManStanding, HeadHunters, TeamDeathmatch, ( not supported = Warlord)
      matchLengths = 'Quick', #Instant,Quick,Standard,Epic
      level=2,
      fps=60,
      agentTimeout='24:00:00',      
      agents=[
        dict(type='human', archer='white'),
        dict(type='human', archer='pink'),
        dict(type='remote', archer='pink', ai='NoMoveAgent'),
        ],
    )
  )

  towerfall.run()

if __name__ == '__main__':
  main()