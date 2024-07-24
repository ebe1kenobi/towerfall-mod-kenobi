using Microsoft.Xna.Framework;
using System;
using TowerFall;
using ModCompilKenobi;
using TowerfallAi.Core;

namespace NAIMod
{
  public static class NAIMod
  {

    public const string ModNativeAiModVersion = "v0.1";
    public const string InputName = "NAIMod.Input";
    public const string TowerfallKeyboardInputName = "TowerFall.KeyboardInput";
    public static GameTime gameTime;
    private static bool isAgentReady = false;
    private static Agent[] agents = new Agent[TFGame.Players.Length];
    public static PlayerInput[] AgentInputs = new PlayerInput[TFGame.Players.Length];
    public static PlayerInput[] savedPlayerInput = new PlayerInput[TFGame.Players.Length];
    public static PlayerInput[] lastPlayerInput = new PlayerInput[TFGame.Players.Length];
    public static bool NAIModEnabled { get; private set;}
    public static bool NAIModNoKeyboardEnabled { get; private set;}
    
    public static void ParseArgs(string[] args)
    {
      NAIModEnabled = true; 
      NAIModNoKeyboardEnabled = true;
      for (int i = 0; i < args.Length; i++)
      {
        if (args[i] == "--nonativeaimod")
        {
          NAIModEnabled = false;
        }
        if (args[i] == "--nonativeaimodkeyboard")
        {
          NAIModNoKeyboardEnabled = false;
        }
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

    public static bool IsAgentPlaying(int playerIndex) {
      return NAIMod.savedPlayerInput[playerIndex] != null 
              || NAIMod.InputName.Equals(TFGame.PlayerInputs[playerIndex].GetType().ToString());

    }
    public static bool IsThereOtherPlayerType(int playerIndex)
    {
      return ! (NAIMod.savedPlayerInput[playerIndex] == null
              && NAIMod.InputName.Equals(TFGame.PlayerInputs[playerIndex].GetType().ToString()));
    }

    public static void CreateAgent()
    {
      Logger.Info("NativeAiMod.CreateAgent");
      Logger.Info("NativeAiMod.TFGame.Players.Length = " + TFGame.Players.Length);
      Logger.Info("NativeAiMod.agents.Length = " + agents.Length);
      //detect first player slot free
      for (int i = 0; i < TF8PlayerMod.TF8PlayerMod.GetPlayerCount(); i++)
      {
        Logger.Info("NativeAiMod.createAgent i = " + i);
        // create an agent for each player
        AgentInputs[i] = new Input(i);
        agents[i] = new Agent(i, AgentInputs[i]);
        Logger.Info("Agent " + i + " Created");
        //if (null != TFGame.PlayerInputs[i] && NAIModNoKeyboardEnabled && TowerfallKeyboardInputName.Equals(TFGame.PlayerInputs[i].GetType().ToString()))
        //{
        //  Logger.Info("NativeAiMod.createAgent i = " + i + " : " + TFGame.PlayerInputs[i].GetType());
        //  Logger.Info("destroy keyboard input object");
        //  TFGame.PlayerInputs[i] = null;
        //}
        if (null != TFGame.PlayerInputs[i]) continue;

        
        //Logger.Info("NativeAiMod.createAgent i = " + i + " is null or KeyboardInput");
        // Automatically set an agent for free input slot
        // add a controller to PlayerInputs
        //TFGame.PlayerInputs[i] = new Input(i);
        //agents[i] = new Agent(i, TFGame.PlayerInputs[i]);
        TFGame.PlayerInputs[i] = AgentInputs[i];
        Logger.Info("Agent " + i + " set to free input");
      }

      isAgentReady = true;
    }

    public static void SetAgentLevel(Level level)
    {
      for (var i = 0; i < TFGame.Players.Length; i++)
      {
        if (!TFGame.Players[i]) continue;
        if (! InputName.Equals(TFGame.PlayerInputs[i].GetType().ToString())) continue;
        //set level reference once, at Level creation
        agents[i].SetLevel(level);
      }
    }

    public static void AgentUpdate(Level level) {
      if (level.LivingPlayers == 0) return;

      for (int i = 0; i < TFGame.PlayerInputs.Length; i++)
      {
        if (level.GetPlayer(i) == null) continue;
        if (!NAIMod.InputName.Equals(TFGame.PlayerInputs[i].GetType().ToString())) continue;
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
