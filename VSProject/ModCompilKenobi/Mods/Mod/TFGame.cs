using System;
using Microsoft.Xna.Framework;
using ModCompilKenobi;
using Patcher;
using TowerFall;
using TowerfallAi.Core;

namespace ModCompilKenobi
{
  [Patch]
  public class ModTFGame : TFGame {
    // This allows identifying that TowerFall.exe is patched.
    public const string AiModVersion = AiMod.ModAiVersion;

    Action originalInitialize;
    Action<GameTime> originalUpdate;

    [STAThread]
    public static void Main(string[] args) {
      try {
        ModCompilKenobi.InitLog();
        AiMod.ParseArgs(args);
        NAIMod.NAIMod.ParseArgs(args);
        TF8PlayerMod.TF8PlayerMod.ParseArgs(args);
        typeof(TFGame).GetMethod("$original_Main").Invoke(null, new object[] { args });
      } catch (Exception exception) {
        TFGame.Log(exception, false);
        TFGame.OpenLog();
      }
    }

    public ModTFGame(bool noIntro) : base(noIntro) {
      var ptr = typeof(TFGame).GetMethod("$original_Initialize").MethodHandle.GetFunctionPointer();
      originalInitialize = (Action)Activator.CreateInstance(typeof(Action), this, ptr);

      ptr = typeof(TFGame).GetMethod("$original_Update").MethodHandle.GetFunctionPointer();
      originalUpdate = (Action<GameTime>)Activator.CreateInstance(typeof(Action<GameTime>), this, ptr);

      for (var i = 0; i < TFGame.Players.Length; i++)
      {
        AiMod.nbPlayerType[i] = 0;
        AiMod.currentPlayerType[i] = PlayerType.None;
      }
      if (AiMod.ModAIEnabled) {
        this.InactiveSleepTime = new TimeSpan(0);

        if (AiMod.IsFastrun) {
          Monocle.Engine.Instance.Graphics.SynchronizeWithVerticalRetrace = false;
          this.IsFixedTimeStep = false;
        } else {
          this.IsFixedTimeStep = true;
        }
      }
    }

    public override void Initialize() {

      Logger.Info($"TowerfallAiMod version: {AiMod.ModAiVersion} Enabled: {AiMod.ModAIEnabled} Training: {AiMod.ModAITraining}");
      Logger.Info($"TF8PlayerMod version: {TF8PlayerMod.TF8PlayerMod.TF8PlayerModVersion}  Enabled: {TF8PlayerMod.TF8PlayerMod.Mod8PEnabled}");
      Logger.Info($"TowerfallModVariantSpeed version: {TowerfallModVariantSpeed.TowerfallModVariantSpeed.TowerfallModVariantSpeedVersion}");
      Logger.Info($"ModVariantControlGhost version: {ModVariantControlGhost.ModVariantControlGhost.ModVariantControlGhostVersion}");
      Logger.Info($"TowerfallModPlayTag version: {TowerfallModPlayTag.TowerfallModPlayTag.ModPlayTag}");
      Logger.Info($"NAIMod version: {NAIMod.NAIMod.ModNativeAiModVersion}  Enabled: {NAIMod.NAIMod.NAIModEnabled}");

      if (AiMod.ModAIEnabled)
      {
        AiMod.PreGameInitialize();
        originalInitialize();
        AiMod.PostGameInitialize();
        return;
      }

      originalInitialize();
    }

    public override void Update(GameTime gameTime) {

      if (NAIMod.NAIMod.NAIModEnabled || AiMod.ModAIEnabled) {
        //ModCompilKenobi.ModCompilKenobi.gameTime = gameTime;
        //ModCompilKenobi.ModCompilKenobi.Update(originalUpdate);

        ModCompilKenobi.gameTime = gameTime;
        ModCompilKenobi.Update(originalUpdate);
        return;
      }
      
/*
      if (NAIMod.NAIMod.NAIModEnabled)
      {
        NAIMod.NAIMod.gameTime = gameTime;
        NAIMod.NAIMod.Update(originalUpdate);
        return;
      }

      if (AiMod.ModAIEnabled)
      {
        AiMod.gameTime = gameTime;
        AiMod.Update(originalUpdate);
        return;
      }
*/
      originalUpdate(gameTime);

    }

    public override void Draw(GameTime gameTime) {
      if (!AiMod.ModAIEnabled) {
        base.Draw(gameTime);
        return;
      }

      if (AiMod.ModAITraining &&  (!AiMod.IsMatchRunning() || AiMod.NoGraphics)) {
        Monocle.Engine.Instance.GraphicsDevice.SetRenderTarget(null);
        return;
      }
      
      base.Draw(gameTime);

      // I don't know what this is for
      if (AiMod.ModAITraining) {
        Monocle.Draw.SpriteBatch.Begin();
        Agents.Draw(); 
        Monocle.Draw.SpriteBatch.End();
      }
    }
  }
}
