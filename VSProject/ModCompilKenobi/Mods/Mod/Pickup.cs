using Microsoft.Xna.Framework;
using System;
using Patcher;
using TowerFall;
using ModCompilKenobi;

namespace TowerfallAi.Mod
{
  [Patch]
  public class MyPickup : Pickup
  {
    public static Pickup CreatePickup(
      Vector2 position,
      Vector2 targetPosition,
      Pickups type,
      int playerIndex)
    {
      Pickup pickup;
      try
      {
        pickup = Pickup.CreatePickup(position, targetPosition, type, playerIndex);
      } catch (Exception e) {
        switch (type)
        {
          case Pickups.PlayTagSpaceOrb:
          case Pickups.PlayTagLavaOrb:
          case Pickups.PlayTagTimeOrb:
          case Pickups.PlayTagDarkOrb:
          case Pickups.PlayTagShield:
          case Pickups.PlayTagWings:
          case Pickups.PlayTagSpeedBoots:
          case Pickups.PlayTagMirror:
          case Pickups.PlayTagArrows:
          case Pickups.PlayTagBombArrows:
          case Pickups.PlayTagSuperBombArrows:
          case Pickups.PlayTagLaserArrows:
          case Pickups.PlayTagBrambleArrows:
          case Pickups.PlayTagDrillArrows:
          case Pickups.PlayTagBoltArrows:
          case Pickups.PlayTagFeatherArrows:
          case Pickups.PlayTagTriggerArrows:
          case Pickups.PlayTagPrismArrows:
            try
            {
              pickup = (Pickup) new PlayTag(position, targetPosition, type);
            }
            catch (Exception ExceptionPlayTag)
            {
              throw ExceptionPlayTag;
            }
            break;
          default:
            throw e;
        }
        pickup.PickupType = type;
      }

      return pickup;
    }

    public MyPickup(Vector2 position, Vector2 targetPosition)
      : base(position, targetPosition)
    {
    }
  }
}
