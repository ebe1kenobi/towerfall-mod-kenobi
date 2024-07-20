using System;
using Patcher;
using TowerFall;
using ModCompilKenobi;
using TowerfallAi.Core;

namespace TowerfallAi.Mod {
  [Patch]
  public class ModSession : Session {
    Action originalOnLevelLoadFinish;

    //Copy parent...
    public bool IsInOvertime
    {
      get
      {
        if (this.RoundIndex >= 0 && (this.MatchSettings.Mode == Modes.HeadHunters || this.MatchSettings.Mode == Modes.PlayTag))
        {
          for (int index = 0; index < this.Scores.Length; ++index)
          {
            if (this.Scores[index] >= this.MatchSettings.GoalScore)
              return true;
          }
        }
        return false;
      }
    }

    public ModSession(MatchSettings matchSettings) : base(matchSettings) { 
      originalOnLevelLoadFinish = Util.GetAction("$original_OnLevelLoadFinish", typeof(Session), this);
    }

    public override void LevelLoadStart(Level level)
    {
      try {
        base.LevelLoadStart(level);
      } catch (Exception e) {
        if (e.Message == "No defined Round Logic for that mode!" && this.MatchSettings.Mode == Modes.PlayTag) {
          this.RoundLogic = new PlayTagRoundLogic(this);
        } else {
          throw e;
        }
      }
    }

    public override void OnLevelLoadFinish() {
      originalOnLevelLoadFinish();

      if (AiMod.ModAIEnabled) {
        Agents.NotifyLevelLoad(this.CurrentLevel);
      }
    }
  }
}
