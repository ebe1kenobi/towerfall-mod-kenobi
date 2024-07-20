using Microsoft.Xna.Framework;
using Monocle;
using Newtonsoft.Json.Linq;
using Patcher;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TowerFall;
using static TowerFall.UnlockData;

namespace TowerfallAi.Mod
{
  [Patch]
  public class MyMatchVariants : MatchVariants
  {
    [Header("SPEED")]
    [Description("Increase game speed")]
    public Variant SpeedX1_1;
    public Variant SpeedX1_2;
    public Variant SpeedX1_3;
    public Variant SpeedX1_4;
    public Variant SpeedX1_5;
    public Variant SpeedX1_6;
    public Variant SpeedX1_7;
    public Variant SpeedX1_8;
    public Variant SpeedX1_9;
    public Variant SpeedX2;
    public Variant SpeedX2_5;
    public Variant SpeedX3;
    public Variant SpeedX3_5;
    public Variant SpeedX4;
    public Variant SpeedX4_5;
    public Variant SpeedX5;
    public Variant SpeedX6;
    public Variant SpeedX7;
    public Variant SpeedX8;
    public Variant SpeedX9;
    public Variant SpeedX10;

    [Header("ARCHERS")]
    [PerPlayer]
    [Description("Use Left and Right Bumper (LB/LR and not LT/RT) to use Ghost mode in the select direction")]
    public Variant Ghost;
    [PerPlayer]
    [Description("Use Left and Right Bumper (LB/LR and not LT/RT) to use Ghost mode to a Random position")]
    public Variant GhostRandom;

    public MyMatchVariants(bool noPerPlayer = false) : base(noPerPlayer)
    {
      this.CreateLinks(Ghost, GhostRandom);
      this.CreateLinks(
        SpeedX1_1,
        SpeedX1_2,
        SpeedX1_3,
        SpeedX1_4,
        SpeedX1_5,
        SpeedX1_6,
        SpeedX1_7,
        SpeedX1_8,
        SpeedX1_9,
        SpeedX2,
        SpeedX2_5,
        SpeedX3,
        SpeedX3_5,
        SpeedX4,
        SpeedX4_5,
        SpeedX5,
        SpeedX6,
        SpeedX7,
        SpeedX8,
        SpeedX9,
        SpeedX10
      );
    }

    private static string GetVariantTitle(FieldInfo field)
    {
      string str = MatchVariants.GetVariantTitle(field);
      return str.Replace("_", " ");
    }
  }
}
