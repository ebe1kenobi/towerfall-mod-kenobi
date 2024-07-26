
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

    //use in archer selection screen to change between human or AI
    //public static PlayerInput[] savedHumanPlayerInput = new PlayerInput[TFGame.Players.Length];
    //public static int[] nbPlayerType = new int[TFGame.Players.Length];
    //public static PlayerType[] currentPlayerType = new PlayerType[TFGame.Players.Length];
    //public static bool isHumanPlayerTypeSaved = false;
    //public static PlayerInput[] lastPlayerInput = new PlayerInput[TFGame.Players.Length];

    public static GameTime gameTime;
    public static Stopwatch gameTimeWatch;
    private static readonly Stopwatch fpsWatch = new Stopwatch();
    //private static TimeSpan totalGameTime = new TimeSpan();
    //private static long totalFrame = 0;
    //private static readonly TimeSpan ellapsedGameTime = new TimeSpan(10000000 / 60);

    public static void InitLog() {
      Util.CreateDirectory(ModCompilKenobi.BaseDirectory);
      Logger.Init(ModCompilKenobi.BaseDirectory);
    }

    //public static GameTime GetGameTime()
    //{
    //  return new GameTime(totalGameTime, ellapsedGameTime);
    //}

    public static bool CurrentPlayerIs(PlayerType type, int playerIndex)
    {
      //Logger.Info("CurrentPlayerIs" + playerIndex);
      return AiMod.currentPlayerType[playerIndex] == type;
    }

    public static string GetPlayerTypePlaying(int playerIndex)
    {
      switch (AiMod.currentPlayerType[playerIndex]) {
        case PlayerType.Human: return "P";
        case PlayerType.AiMod: return "AI";
        case PlayerType.NAIMod: return "NAI";
        default: throw new Exception("Current type Player not initialised :" + AiMod.currentPlayerType[playerIndex]);   
      }
    }

    public static bool IsAgentPlaying(int playerIndex, Level level)
    {
      //Logger.Info("IsAgentPlaying" + playerIndex);
      return level.GetPlayer(playerIndex) != null && (AiMod.currentPlayerType[playerIndex] == PlayerType.AiMod || AiMod.currentPlayerType[playerIndex] == PlayerType.NAIMod);
      //return ModCompilKenobi.savedHumanPlayerInput[playerIndex] != null
      //        || NAIMod.NAIMod.InputName.Equals(TFGame.PlayerInputs[playerIndex].GetType().ToString());
    }
    public static bool IsThereOtherPlayerType(int playerIndex)
    {
      //Logger.Info("IsThereOtherPlayerType => nbPlayerType[" + playerIndex + "] = " + AiMod.nbPlayerType[playerIndex]);

      return AiMod.nbPlayerType[playerIndex] > 1;
    }

    public static void Update(Action<GameTime> originalUpdate)
    {
      if (TFGame.GameLoaded && !AiMod.isHumanPlayerTypeSaved) {
        for (var i = 0; i < TFGame.PlayerInputs.Length; i++)
        {
          if (TFGame.PlayerInputs[i] == null) continue;
          AiMod.nbPlayerType[i]++;
          AiMod.currentPlayerType[i] = PlayerType.Human;
          AiMod.savedHumanPlayerInput[i] = TFGame.PlayerInputs[i];

          Logger.Info("AiMod.nbPlayerType["+ i + "] = "  + AiMod.nbPlayerType[i]);
          Logger.Info("AiMod.currentPlayerType[" + i + "] = " + AiMod.currentPlayerType[i]);
        }
        AiMod.isHumanPlayerTypeSaved = true;
      }

      if (NAIMod.NAIMod.NAIModEnabled)
      {
        if (TFGame.GameLoaded && !NAIMod.NAIMod.isAgentReady && AiMod.isHumanPlayerTypeSaved)
        {
          NAIMod.NAIMod.CreateAgent();
        }
      }

      int fps = 0;

      if (AiMod.ModAIEnabled && AiMod.isHumanPlayerTypeSaved) {
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
        if (!AiMod.ModAIEnabled || (!AiMod.AgentConnected || AiMod.PreUpdate()))
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
