using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using TowerFall;
using ModCompilKenobi;
using TowerfallAi.Core;
using System;

namespace TowerfallAi.Mod {
  [Patch]
  public class ModPauseMenu : PauseMenu {
  	
  	public readonly DateTime creationTime;
  	
    public ModPauseMenu(Level level, Vector2 position, MenuType menuType, int controllerDisconnected = -1) : base(level, position, menuType, controllerDisconnected) 
    {
   		creationTime = DateTime.Now; 
    }

	public override void Resume()
    {
      int pauseDuration = (int)(DateTime.Now - creationTime).TotalSeconds;

      for (var i = 0; i < TFGame.Players.Length; i++)
      {
        Player p = level.Session.CurrentLevel.GetPlayer(i);
        if (p != null)
        {
          p.pauseDuration += pauseDuration;
        }
      }
      base.Resume();
    }
    
    public override void VersusMatchSettingsAndSave() {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }
      
      Util.GetAction("$original_VersusMatchSettingsAndSave", this)();
    }

    public override void Quit() {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      Util.GetAction("$original_Quit", this)();
    }

    public override void VersusMatchSettings() {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      Util.GetAction("$original_VersusMatchSettings", this)();
    }

    public override void VersusArcherSelect() {
      if (TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      {
        Sounds.ui_clickBack.Play(160f, 1f);
        for (int i = 0; i < 8; i++)
        {
          TFGame.Players[i] = false;
        }
        Engine.Instance.Scene = new MainMenu(MainMenu.MenuState.Rollcall);
        this.level.Session.MatchSettings.LevelSystem.Dispose();
      }

      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      Util.GetAction("$original_VersusArcherSelect", this)();
    }

    public override void QuestMap() {
      if (AiMod.ModAITraining) {
        AiMod.EndSession();
        return;
      }

      Util.GetAction("$original_QuestMap", this)();
    }

    public override void VersusRematch() {
      if (AiMod.ModAITraining) {
        AiMod.Rematch();
        return;
      }

      Util.GetAction("$original_VersusRematch", this)();
    }

    public override void QuestRestart() {
      if (AiMod.ModAITraining) {
        AiMod.Rematch();
        return;
      }

      Util.GetAction("$original_QuestRestart", this)();
    }

    public override void QuestMapAndSave() {
      if (AiMod.ModAITraining)
      {
        AiMod.EndSession();
        return;
      }

      Util.GetAction("$original_QuestMapAndSave", this)();
    }

    public override void QuitAndSave() {
      if (AiMod.ModAITraining)
      {
        AiMod.EndSession();
        return;
      }

      Util.GetAction("$original_QuitAndSave", this)();
    }
  }
}
