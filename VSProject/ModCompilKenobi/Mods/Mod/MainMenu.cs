using Monocle;
using Patcher;
using TowerFall;
using TowerfallAi.Core;
using System.Collections.Generic;
using System;
using ModCompilKenobi;
using System.Collections;

namespace TowerfallAi.Mod
{
  [Patch]
  public class MyMainMenu : MainMenu
  {
    public static OptionsButton enablePlayTagChestTreasure;
    public static OptionsButton delayGameTagPlayTagCountDown;
    public static OptionsButton delayPickupPlayTagCountDown;
    public MyMainMenu(MenuState state) : base(state) {}

    public override void Update()
    {
      if (AiMod.ModAIEnabled) {
        if (!AiMod.PreUpdate()) {
          TFGame.GameLoaded = false;
          AiMod.AgentConnected = false;
        } else {
          TFGame.GameLoaded = true;
          AiMod.AgentConnected = true;
        }
      }
      base.Update();
    }

    public static string MyBoolToString(bool value)
    {
      if (!value)
      {
        return "OFF";
      }

      return "ON";
    }

    public static void SetPropertiesEnablePlayTagChestTreasure()
    {
      enablePlayTagChestTreasure.State = MyBoolToString(SaveData.Instance.Options.EnablePlayTagChestTreasure);
    }

    public static bool OnConfirmEnablePlayTagChestTreasure()
    {
      SaveData.Instance.Options.EnablePlayTagChestTreasure = !SaveData.Instance.Options.EnablePlayTagChestTreasure;
      return SaveData.Instance.Options.EnablePlayTagChestTreasure;
    }

    public static void SetPropertiesDelayGameTagPlayTagCountDown()
    {
      delayGameTagPlayTagCountDown.State = SaveData.Instance.Options.DelayGameTagPlayTagCountDown.ToString();
      delayGameTagPlayTagCountDown.CanLeft = SaveData.Instance.Options.DelayGameTagPlayTagCountDown > 0;
      delayGameTagPlayTagCountDown.CanRight = SaveData.Instance.Options.DelayGameTagPlayTagCountDown < 60;
    }

    public static void OnLeftDelayGameTagPlayTagCountDown()
    {
      SaveData.Instance.Options.DelayGameTagPlayTagCountDown--;
    }

    public static void OnRightDelayGameTagPlayTagCountDown()
    {
      SaveData.Instance.Options.DelayGameTagPlayTagCountDown++;
    }

    public static void SetPropertiesDelayPickupPlayTagCountDown()
    {
      delayPickupPlayTagCountDown.State = SaveData.Instance.Options.DelayPickupPlayTagCountDown.ToString();
      delayPickupPlayTagCountDown.CanLeft = SaveData.Instance.Options.DelayPickupPlayTagCountDown > 0;
      delayPickupPlayTagCountDown.CanRight = SaveData.Instance.Options.DelayPickupPlayTagCountDown < 60;
    }

    public static void OnLeftDelayPickupPlayTagCountDown()
    {
      SaveData.Instance.Options.DelayPickupPlayTagCountDown--;
    }

    public static void OnRightDelayPickupPlayTagCountDown()
    {
      SaveData.Instance.Options.DelayPickupPlayTagCountDown++;
    }


    public override void InitOptions(List<OptionsButton> buttons)
    {
      enablePlayTagChestTreasure = new OptionsButton("ENABLE PLAY TAG TREASURE");
      enablePlayTagChestTreasure.SetCallbacks(SetPropertiesEnablePlayTagChestTreasure, null, null, OnConfirmEnablePlayTagChestTreasure);
      buttons.Add(enablePlayTagChestTreasure);

      delayGameTagPlayTagCountDown = new OptionsButton("GAME PLAYTAG COUNTDOWN");
      delayGameTagPlayTagCountDown.SetCallbacks(SetPropertiesDelayGameTagPlayTagCountDown, OnLeftDelayGameTagPlayTagCountDown, OnRightDelayGameTagPlayTagCountDown, null);
      buttons.Add(delayGameTagPlayTagCountDown);

      delayPickupPlayTagCountDown = new OptionsButton("GAME PLAYTAG PICKUP COUNTDOWN");
      delayPickupPlayTagCountDown.SetCallbacks(SetPropertiesDelayPickupPlayTagCountDown, OnLeftDelayPickupPlayTagCountDown, OnRightDelayPickupPlayTagCountDown, null);
      buttons.Add(delayPickupPlayTagCountDown);

      base.InitOptions(buttons);
    }
  }
}
