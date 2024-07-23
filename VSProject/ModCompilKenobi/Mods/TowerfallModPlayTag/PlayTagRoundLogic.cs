using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace TowerfallAi.Mod
{
  public class PlayTagRoundLogic : RoundLogic
  {
    private RoundEndCounter roundEndCounter;
    private bool done;
    private static int[] playerOrder; 
    private static int currentPlayerOrderIndex;
    private static int lastNumberOfPlayer = 0;

    public PlayTagRoundLogic(Session session)
      : base(session, true)
    {
      this.roundEndCounter = new RoundEndCounter(session);
    }

    private void resetPlayerAlreadyTag() {
      currentPlayerOrderIndex = -1;
      lastNumberOfPlayer = 0;

      List<int> listPlayerIndex = new List<int>();
      foreach (Player player in this.Session.CurrentLevel.Players) {

        listPlayerIndex.Add(player.PlayerIndex);
        lastNumberOfPlayer++;
      }
      listPlayerIndex.Shuffle();
      playerOrder = listPlayerIndex.ToArray();
    }


    public override void OnLevelLoadFinish()
    {
      base.OnLevelLoadFinish();
      this.Session.CurrentLevel.Add<VersusStart>(new VersusStart(this.Session));
      this.Players = this.SpawnPlayersFFA();
    }

    // Match start
    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.SpawnTreasureChestsVersus();
      initPlayTag(getNextTagPlayerIndex()); 
    }

    public override void OnUpdate()
    {
      SessionStats.TimePlayed += Engine.DeltaTicks;
      base.OnUpdate();

      if (!this.RoundStarted || this.done || !this.Session.CurrentLevel.Ending || !this.Session.CurrentLevel.CanEnd)
        return;
      if (!this.roundEndCounter.Finished)
      {
        this.roundEndCounter.Update();
      }
      else
      {
        this.done = true;
        if (this.Session.CurrentLevel.Players.Count > 0)
        {
          //add score for all living player
          foreach (Player player in this.Session.CurrentLevel[GameTags.Player]) { 
            if (player.Dead) continue;  
            this.AddScore(player.PlayerIndex, 1);
          }
        }
        this.InsertCrownEvent();
        this.Session.EndRound(); 
      }
    }

    public override void OnPlayerDeath(
      Player player,
      PlayerCorpse corpse,
      int playerIndex,
      DeathCause deathType,
      Vector2 position,
      int killerIndex)
    {
      base.OnPlayerDeath(player, corpse, playerIndex, deathType, position, killerIndex);

      if (player.playTag)
      {
        this.Session.CurrentLevel.Ending = true; 
        return;
      }

      if (!player.playTagCountDownOn){
        this.Session.CurrentLevel.Ending = true;
        return;
      }

      if (this.Session.CurrentLevel.LivingPlayers == 1)
      {
        this.Session.CurrentLevel.Ending = true;
        return;
      }

      if (this.Session.CurrentLevel.LivingPlayers == 0)
      {
        this.Session.CurrentLevel.Ending = true;
        return;
      }
    }

    public bool OtherPlayerCouldWin(int playerIndex)
    {
      return false;
    }

    public void initPlayTag(int playerIndex)
    {
      Player.ShootLock = true;
      foreach (Player p in this.Session.CurrentLevel.Players) 
      {
        p.playTagCountDown = p.playTagDelayModePlayTag;
        p.playTagCountDownOn = true;
        p.playTag = false;
        p.creationTime = DateTime.Now;

        if (p.PlayerIndex == playerIndex) {
          p.playTag = true;
        }
      }
    }

    private int getNextTagPlayerIndex() {
      if (playerOrder == null || currentPlayerOrderIndex == playerOrder.Length - 1 || lastNumberOfPlayer != this.Session.CurrentLevel.Players.Count)
      {

        resetPlayerAlreadyTag();
      }
      currentPlayerOrderIndex++;

      return playerOrder[currentPlayerOrderIndex];
    }
  }
}
