using Monocle;
using TowerFall;
using System;

namespace TowerfallModPlayTag
{
  public static class TowerfallModPlayTag
  {
    public const string ModPlayTag             = "v0.2";
    public static Counter pause = new Counter();
    public static String currentSong;
    public static Player currentPlayer;

    public static void StartPlayTagEffect(Player player)
    {
      currentPlayer = player;
      currentSong = Music.CurrentSong;
      Music.Stop();
      Sounds.boss_humanLaugh.Play(player.X);
      player.Level.LightingLayer.SetSpotlight((LevelEntity)player);
      //player.Level.Session.CurrentLevel.LightingLayer.SetSpotlight((LevelEntity)player);
      Engine.TimeRate = 0.1f;
      pause.Set(10);
    }
    public static void StopPlayTagEffect()
    {
      Music.Play(currentSong);
      currentPlayer.Level.LightingLayer.CancelSpotlight();
      //currentPlayer.Level.Session.CurrentLevel.LightingLayer.CancelSpotlight();
      Engine.TimeRate = 1f;
    }
    public static void Update()
    {
      if ((bool)pause)
      {
        pause.Update();
        if (!(bool)pause)
        {
          StopPlayTagEffect();
        }
      }
    }
  }
}
