using Microsoft.Xna.Framework;
using ModCompilKenobi;
using Monocle;
using Newtonsoft.Json.Linq;
using Patcher;
using System;
using System.Collections.Generic;
using TowerFall;

namespace TowerfallAi.Mod
{
  [Patch]
  public class MyTreasureSpawner : TreasureSpawner
  {
    private const float BIG_CHEST_CHANCE = 0.03f;
    public static readonly float[][] ChestChances = new float[5][];
    public static readonly bool[] DarkWorldTreasures;
    public const float DEFAULT_ARROW_CHANCE = 0.6f;
    public static readonly float[] DefaultTreasureChances = new float[20]; 
    public static readonly int[] FullTreasureMask = new int[20];  
    //public bool IsPlayTagSpawn = false;
    List<Pickups> listPickupArrow = new List<Pickups>();
    List<Pickups> listPickupReplacement = new List<Pickups>();

    static MyTreasureSpawner()
    {

      float[] numArray = new float[] { 0.9f, 0.9f, 0.2f, 0.1f };
      float[] numArray2 = new float[] { 0.9f, 0.9f, 0.8f, 0.2f, 0.1f };
      float[] numArray3 = new float[] { 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
      float[] numArray4 = new float[] { 0.9f, 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
      float[] numArray5 = new float[] { 0.9f, 0.9f, 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
      ChestChances[0] = numArray;
      ChestChances[1] = numArray2;
      ChestChances[2] = numArray3;
      ChestChances[3] = numArray4;
      ChestChances[4] = numArray5;
      DefaultTreasureChances = new float[] {
                0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f,
                  0.5f, 0.5f, 0.5f, 0.25f, 0.15f, 0.15f,
                0.15f, 0.15f, 0.001f, 0.1f,
              };
      FullTreasureMask = new int[] {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                1, 1, 1, 1, 1, 1, 1,
                1, 1, 1,
            };
    bool[] flagArray = new bool[20];
      flagArray[8] = true;
      flagArray[9] = true;
      DarkWorldTreasures = flagArray;
    }

    public MyTreasureSpawner(Session session, int[] mask, float arrowChance, bool arrowShuffle) : base(session, mask, arrowChance, arrowShuffle)
    {
      listPickupArrow.Add(Pickups.Arrows);
      listPickupArrow.Add(Pickups.BombArrows);
      listPickupArrow.Add(Pickups.SuperBombArrows);
      listPickupArrow.Add(Pickups.LaserArrows);
      listPickupArrow.Add(Pickups.BrambleArrows);
      listPickupArrow.Add(Pickups.DrillArrows);
      listPickupArrow.Add(Pickups.BoltArrows);
      listPickupArrow.Add(Pickups.FeatherArrows);
      listPickupArrow.Add(Pickups.TriggerArrows);
      listPickupArrow.Add(Pickups.PrismArrows);

      listPickupReplacement.Add(Pickups.Shield);
      listPickupReplacement.Add(Pickups.Wings);
      listPickupReplacement.Add(Pickups.SpeedBoots);
      listPickupReplacement.Add(Pickups.Mirror);
      listPickupReplacement.Add(Pickups.TimeOrb);
      listPickupReplacement.Add(Pickups.DarkOrb);
      listPickupReplacement.Add(Pickups.LavaOrb);
      listPickupReplacement.Add(Pickups.SpaceOrb);
      listPickupReplacement.Add(Pickups.ChaosOrb);
      listPickupReplacement.Add(Pickups.Bomb);
    }

    public Pickups getPlayTagPickupFromRealPickup(Pickups pickup)
    {
      switch (pickup)
      {
        case Pickups.Arrows: return Pickups.PlayTagArrows;
        case Pickups.BombArrows: return Pickups.PlayTagBombArrows;
        case Pickups.SuperBombArrows: return Pickups.PlayTagSuperBombArrows;
        case Pickups.LaserArrows: return Pickups.PlayTagLaserArrows;
        case Pickups.BrambleArrows: return Pickups.PlayTagBrambleArrows;
        case Pickups.DrillArrows: return Pickups.PlayTagDrillArrows;
        case Pickups.BoltArrows: return Pickups.PlayTagBoltArrows;
        case Pickups.FeatherArrows: return Pickups.PlayTagFeatherArrows;
        case Pickups.TriggerArrows: return Pickups.PlayTagTriggerArrows;
        case Pickups.PrismArrows: return Pickups.PlayTagPrismArrows;
        case Pickups.Shield: return Pickups.PlayTagShield;
        case Pickups.Wings: return Pickups.PlayTagWings;
        case Pickups.SpeedBoots: return Pickups.PlayTagSpeedBoots;
        case Pickups.Mirror: return Pickups.PlayTagMirror;
        case Pickups.TimeOrb: return Pickups.PlayTagTimeOrb;
        case Pickups.DarkOrb: return Pickups.PlayTagDarkOrb;
        case Pickups.LavaOrb: return Pickups.PlayTagLavaOrb;
        case Pickups.SpaceOrb: return Pickups.PlayTagSpaceOrb;
        default: throw new Exception("Pickup type not authorized!");
      }
    }

    public override List<TreasureChest> GetChestSpawnsForLevel(
      List<Vector2> chestPositions,
      List<Vector2> bigChestPositions)
    {

      List<TreasureChest> chestSpawnsForLevel = base.GetChestSpawnsForLevel(chestPositions, bigChestPositions);

      if (SaveData.Instance.Options.EnablePlayTagChestTreasure 
          && this.Session.NbPlayTagPickupActivated == 0
          && chestSpawnsForLevel.Count > 0 && this.Session.MatchSettings.Mode != Modes.PlayTag)
      {
        Random rnd = new Random();
        int draw = rnd.Next(0, 3);
        for (var i = 0; draw == 1 && i < chestSpawnsForLevel.Count; i++)
        {
          if (!PlayTag.realPickupPossibleList.Contains(chestSpawnsForLevel[i].pickups[0]))
          {
            continue;
          }
          chestSpawnsForLevel[i].pickups[0] = getPlayTagPickupFromRealPickup(chestSpawnsForLevel[i].pickups[0]);
          break;
        }
      } 
      else if (this.Session.MatchSettings.Mode == Modes.PlayTag) {
        Random rnd = new Random();

        for (var i = 0; i < chestSpawnsForLevel.Count; i++)
        {
          if (!listPickupArrow.Contains(chestSpawnsForLevel[i].pickups[0]))
          {
            continue;
          }
          // No need for Arrow in this mode
          chestSpawnsForLevel[i].pickups[0] = listPickupReplacement[rnd.Next(0, listPickupReplacement.Count - 1)];
        }
      }
      return chestSpawnsForLevel;
    }

    public override bool CanSpawnAnotherChest(int alreadySpawnedAmount)
    {
      if (alreadySpawnedAmount >= ChestChances[Math.Min((TFGame.PlayerAmount - 2), 4)].Length)
      {
        return false;
      }
      if (this.Session.MatchSettings.Variants.MaxTreasure == null)
      {
        return this.Random.Chance(ChestChances[Math.Min((TFGame.PlayerAmount - 2), 4)][alreadySpawnedAmount]);
      }
      return true;
    }

  }
}
