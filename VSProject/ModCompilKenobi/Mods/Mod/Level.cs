﻿using System;
using System.Xml;
using Monocle;
using Patcher;
using TowerFall;
using ModCompilKenobi;
using TowerfallAi.Core;

namespace TowerfallAi.Mod {
  [Patch]
  public class ModLevel : Level {
    Action originalUpdate;

    Action originalHandlePausing;

    private int nbUpdate = 0;

    public ModLevel(Session session, XmlElement xml) : base(session, xml) {
      if (TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      {
        this.controllerAttachedFlags = new bool[8];
        for (int i = 0; i < 8; i++)
        {
          this.controllerAttachedFlags[i] = true;
        }
      }

      if (NAIMod.NAIMod.NAIModEnabled)
      {
        NAIMod.NAIMod.SetAgentLevel(this);
      }

      var ptr = typeof(Level).GetMethod("$original_Update").MethodHandle.GetFunctionPointer();
      originalUpdate = (Action)Activator.CreateInstance(typeof(Action), this, ptr);

      ptr = typeof(Level).GetMethod("$original_HandlePausing").MethodHandle.GetFunctionPointer();
      originalHandlePausing = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
    }

    public override void HandlePausing() {
      // Avoid pausing when no human is playing and the screen goes out of focus.
      if (AiMod.ModAITraining && !AiMod.IsHumanPlaying()) {
        return;
      }

      originalHandlePausing();
    }

    public override void Update() {
      nbUpdate++;
      if (! (Ending)) {
        if (this.Session.CurrentLevel.LivingPlayers > 0 && ((Player)this.Session.CurrentLevel.Players[0]).playTagCountDownOn) //todo maybe crash here...
        TowerfallModPlayTag.TowerfallModPlayTag.Update();

        if (AiMod.ModAIEnabled) {
          Agents.RefreshInputFromAgents(this);

        }

        if (NAIMod.NAIMod.NAIModEnabled)
        {
          NAIMod.NAIMod.AgentUpdate(this);
        }
      } else {
      }

      originalUpdate();

      if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_1)
      {
        Engine.TimeRate = 1.1f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_2)
      {
        Engine.TimeRate = 1.2f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_3)
      {
        Engine.TimeRate = 1.3f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_4)
      {
        Engine.TimeRate = 1.4f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_5)
      {
        Engine.TimeRate = 1.5f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_6)
      {
        Engine.TimeRate = 1.6f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_7)
      {
        Engine.TimeRate = 1.7f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_8)
      {
        Engine.TimeRate = 1.8f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX1_9)
      {
        Engine.TimeRate = 1.9f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX2)
      {
        Engine.TimeRate = 2f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX2_5)
      {
        Engine.TimeRate = 2.5f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX3)
      {
        Engine.TimeRate = 3f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX3_5)
      {
        Engine.TimeRate = 3.5f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX4)
      {
        Engine.TimeRate = 4f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX4_5)
      {
        Engine.TimeRate = 4.5f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX5)
      {
        Engine.TimeRate = 5f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX6)
      {
        Engine.TimeRate = 6f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX7)
      {
        Engine.TimeRate = 7f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX8)
      {
        Engine.TimeRate = 8f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX9)
      {
        Engine.TimeRate = 9f;
      }
      else if ((bool)((MyMatchVariants)this.Session.MatchSettings.Variants).SpeedX10)
      {
        Engine.TimeRate = 10f;
      }
    }
  }
}
