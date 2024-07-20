using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerFall;
using TowerfallAi.Core;

namespace TowerfallAi.Mod
{
    [Patch]
    public class MyMatchSettings : MatchSettings
    {

        public int GoalScore
        {
          get
          {
            int num;
        
            switch (this.Mode)
            {
              case Modes.LastManStanding:
                num = base.GoalScore;
                break;
              case Modes.HeadHunters:
              case Modes.Warlord:
                 num = base.GoalScore;
                break;
              case Modes.TeamDeathmatch:
                 num = base.GoalScore;
                break;
              case Modes.Trials:
              case Modes.LevelTest:
                  num = base.GoalScore;
                break;
              case Modes.PlayTag:
                 num = this.PlayerGoals(5, 4, 3);
                 break;
              default:
                throw new Exception("No Goal value defined for this mode!");
            }
            return (int)Math.Ceiling((double)num * (double)MatchSettings.GoalMultiplier[(int)this.MatchLength]);
          }
        }
    
        public MyMatchSettings(TowerFall.LevelSystem levelSystem, TowerFall.Modes mode, MatchLengths matchLength) : base(levelSystem, mode, matchLength)
        {
            if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
            {
              return;
            }
            this.LevelSystem = levelSystem;
            this.Mode = mode;
            this.MatchLength = matchLength;
            this.Teams = new MatchTeams(Allegiance.Neutral);
            this.Variants = new MatchVariants(false);
        }

        public override int GetMaxTeamSize()
        {
            if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
            {
              return base.GetMaxTeamSize();
            }
            if (!this.TeamMode)
            {
                return 1;
            }
            int[] numArray = new int[2];
            for (int i = 0; i < 8; i++)
            {
                if (TFGame.Players[i] && (this.Teams[i] != Allegiance.Neutral))
                {
                    numArray[(int)this.Teams[i]]++;
                }
            }
            return Math.Max(numArray[0], numArray[1]);
        }

        public override int PlayerGoals(int p2goal, int p3goal, int p4goal)
        {
            if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
            {
              return base.PlayerGoals(p2goal, p3goal, p4goal);
            }
            switch (TFGame.PlayerAmount)
            {
                case 2:
                    return p2goal;

                case 3:
                    return p3goal;

                case 4:
                    return p4goal;
            }
            return p4goal;
        }

        public override int PlayerLimit
        {
            get
            {
                if (!this.SoloMode)
                {
                    return TF8PlayerMod.TF8PlayerMod.Mod8PEnabled ? 8 : 4;
                }
                return 1;
            }
        }

    }
}
