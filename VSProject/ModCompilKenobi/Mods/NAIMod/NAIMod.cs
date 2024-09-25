using Microsoft.Xna.Framework;
using System;
using TowerFall;
using ModCompilKenobi;
using TowerfallAi.Core;
using System.Collections.ObjectModel;

namespace NAIMod
{
  public static class NAIMod
  {

    public const string ModNativeAiModVersion = "v0.1";
    public const string InputName = "NAIMod.Input";
    public const string TowerfallKeyboardInputName = "TowerFall.KeyboardInput";
    public static GameTime gameTime;
    public static bool isAgentReady = false;
    private static Agent[] agents = new Agent[TFGame.Players.Length];
    public static PlayerInput[] AgentInputs = new PlayerInput[TFGame.Players.Length];

    public static bool NAIModEnabled { get; private set;}
    //public static bool NAIModNoKeyboardEnabled { get; private set;}
    
    public static void ParseArgs(string[] args)
    {
      NAIModEnabled = true;
      //NAIModNoKeyboardEnabled = true;
      for (int i = 0; i < args.Length; i++)
      {
        //Always enabled in compil
        //if (args[i] == "--nonativeaimod")
        //{
        //  NAIModEnabled = false;
        //}
        //if (args[i] == "--nonativeaimodkeyboard")
        //{
        //  NAIModNoKeyboardEnabled = false;
        //}
      }
    }

    public static void Update(Action<GameTime> originalUpdate)
    {
      if (TFGame.GameLoaded && !isAgentReady) {
        CreateAgent();
      }
      try
      {
        originalUpdate(gameTime);
      }
      catch (AggregateException aggregateException)
      {
        foreach (var innerException in aggregateException.Flatten().InnerExceptions)
        {
          HandleFailure(innerException);
        }
      }
    }

    public static void CreateAgent()
    {
      //detect first player slot free
      for (int i = 0; i < TF8PlayerMod.TF8PlayerMod.GetPlayerCount(); i++) //todo use everywhere
      {
        // create an agent for each player
        AgentInputs[i] = new Input(i);
        agents[i] = new Agent(i, AgentInputs[i]);
        ModCompilKenobi.ModCompilKenobi.nbPlayerType[i]++;
        Logger.Info("Agent " + i + " Created");
        if (null != TFGame.PlayerInputs[i]) continue;

        TFGame.PlayerInputs[i] = AgentInputs[i];
        ModCompilKenobi.ModCompilKenobi.currentPlayerType[i] = PlayerType.NAIMod;
      }

      isAgentReady = true;
    }

    public static void SetAgentLevel(Level level)
    {
      for (var i = 0; i < TFGame.Players.Length; i++)
      {
        if (!TFGame.Players[i]) continue;
        if (null == TFGame.PlayerInputs[i]) continue;
        if (! InputName.Equals(TFGame.PlayerInputs[i].GetType().ToString())) continue;
        //set level reference once, at Level creation
        agents[i].SetLevel(level);
      }
    }

    public static void AgentUpdate(Level level) {
      if (level.LivingPlayers == 0) return;

      for (int i = 0; i < TFGame.PlayerInputs.Length; i++)
      {
        if (!(ModCompilKenobi.ModCompilKenobi.CurrentPlayerIs(PlayerType.NAIMod, i)
            && ModCompilKenobi.ModCompilKenobi.IsAgentPlaying(i, level)))
          continue;
        agents[i].Play();
      }
    }

    public static void HandleFailure(Exception ex)
    {
      Logger.Info($"Unhandled exception.\n  {ex}");
      throw ex;
    }
  }
}
