
using Microsoft.Xna.Framework;
using System.Threading;
using System;
using TowerFall;
using TowerfallAi.Core;
using System.Diagnostics;
using TowerfallAiMod.Core;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace ModCompilKenobi
{
  /*
   * Set the order of player type selection
   */
  public enum PlayerType
  {
    None = 0,
    Human,
    AiMod,
    NAIMod,
  }

  public static class ModCompilKenobi {

    public const string ModCompilKenobiVersion = "v0.2.0";
    public const string BaseDirectory = "modcompilkenobi";

    public static GameTime gameTime;
    public static Stopwatch gameTimeWatch;
    private static readonly Stopwatch fpsWatch = new Stopwatch();

    public static PlayerInput[] savedHumanPlayerInput = new PlayerInput[TFGame.Players.Length];
    public static int[] nbPlayerType = new int[TFGame.Players.Length];
    public static PlayerType[] currentPlayerType = new PlayerType[TFGame.Players.Length];
    public static bool isHumanPlayerTypeSaved = false;

    public static void InitLog() {
      Util.CreateDirectory(ModCompilKenobi.BaseDirectory);
      Logger.Init(ModCompilKenobi.BaseDirectory);
    }

    public static bool CurrentPlayerIs(PlayerType type, int playerIndex)
    {
      return ModCompilKenobi.currentPlayerType[playerIndex] == type;
    }

    public static string GetPlayerTypePlaying(int playerIndex)
    {
      switch (ModCompilKenobi.currentPlayerType[playerIndex]) {
        case PlayerType.Human: return "P";
        case PlayerType.AiMod: return "AI";
        case PlayerType.NAIMod: return "NAI";
        default: throw new Exception("Current type Player not initialised :" + ModCompilKenobi.currentPlayerType[playerIndex]);   
      }
    }

    public static bool IsAgentPlaying(int playerIndex, Level level)
    {
      return level.GetPlayer(playerIndex) != null && (ModCompilKenobi.currentPlayerType[playerIndex] == PlayerType.AiMod || ModCompilKenobi.currentPlayerType[playerIndex] == PlayerType.NAIMod);
    }
    public static bool IsThereOtherPlayerType(int playerIndex)
    {
      return ModCompilKenobi.nbPlayerType[playerIndex] > 1;
    }

    public static void Update(Action<GameTime> originalUpdate)
    {
      if (TFGame.GameLoaded && !ModCompilKenobi.isHumanPlayerTypeSaved) {
        for (var i = 0; i < TFGame.PlayerInputs.Length; i++)
        {
          if (TFGame.PlayerInputs[i] == null) continue;
          ModCompilKenobi.nbPlayerType[i]++;
          ModCompilKenobi.currentPlayerType[i] = PlayerType.Human;
          ModCompilKenobi.savedHumanPlayerInput[i] = TFGame.PlayerInputs[i];
        }
        ModCompilKenobi.isHumanPlayerTypeSaved = true;
      }

      if (NAIMod.NAIMod.NAIModEnabled)
      {
        if (TFGame.GameLoaded && !NAIMod.NAIMod.isAgentReady && ModCompilKenobi.isHumanPlayerTypeSaved)
        {
          NAIMod.NAIMod.CreateAgent();
        }
      }

      int fps = 0;

      if (AiMod.ModAIEnabled && ModCompilKenobi.isHumanPlayerTypeSaved) {
        if (AiMod.Config?.fps > 0)
        {
          fps = AiMod.IsMatchRunning() ? AiMod.Config.fps : 10;
          fpsWatch.Stop();
          long ticks = 10000000L / fps;
          if (fpsWatch.ElapsedTicks < ticks)
          {
            Thread.Sleep((int)(ticks - fpsWatch.ElapsedTicks) / 10000);
          }
          fpsWatch.Reset();
          fpsWatch.Restart();
        }

        if (!AiMod.ConnectionDispatcher.IsRunning)
        {
          throw new Exception("ConnectionDispatcher stopped running");
        }

        if (!AiMod.loggedScreenSize)
        {
          Logger.Info("Screen: {0} x {1}, {2}".Format(
            TFGame.Instance.Screen.RenderTarget.Width,
            TFGame.Instance.Screen.RenderTarget.Height,
            TFGame.Instance.Screen.RenderTarget.Format));
          AiMod.loggedScreenSize = true;
        }
      }

      try
      {
        // We originalUpdate() if no AI or (if ai is enable but agent not yet connected we need to call original update
        // to display the Loader, else Preupdate will give problem (I forgot which one...)
        // introduce a problem with sandboxmode whcih wait for a reset call, which we had, but resetOperation will be null and nAgents.restet never called
        // because of the second preupdate (this one or the one in MainMenu.Update
        // => Ok corrected with (!AiMod.ModAITraining && !AiMod.AgentConnected)
        if (!AiMod.ModAIEnabled || ((!AiMod.ModAITraining && !AiMod.AgentConnected) || AiMod.PreUpdate()))
        {
          if (AiMod.ModAIEnabled && fps > 0)
          {
            originalUpdate(AiMod.GetGameTime());
          }
          else
          {
            originalUpdate(gameTime);
          }
        }
      }
      catch (AggregateException aggregateException)
      {
        foreach (var innerException in aggregateException.Flatten().InnerExceptions)
        {
          AiMod.HandleFailure(innerException);
        }
      }
      catch (Exception ex)
      {
        AiMod.HandleFailure(ex);
      }

      if (AiMod.ModAIEnabled && AiMod.gameTimeWatch.ElapsedMilliseconds > AiMod.logTimeInterval.TotalMilliseconds)
      {
        AiMod.LogGameTime();
        AiMod.gameTimeWatch.Restart();
      }
    }
  }
}
