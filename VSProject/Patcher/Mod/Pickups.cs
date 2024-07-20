using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
  public static class MyPickups
  {
    public static void PatchModule(ModuleDefinition baseModule)
    {
      var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.Pickups");
      FieldDefinition lastPlayTagArrows = new FieldDefinition("PlayTagArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 21 };
      type.Fields.Add(lastPlayTagArrows);
      FieldDefinition lastPlayTagBombArrows = new FieldDefinition("PlayTagBombArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 22 };
      type.Fields.Add(lastPlayTagBombArrows);
      FieldDefinition lastPlayTagSuperBombArrows = new FieldDefinition("PlayTagSuperBombArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 23 };
      type.Fields.Add(lastPlayTagSuperBombArrows);
      FieldDefinition lastPlayTagLaserArrows = new FieldDefinition("PlayTagLaserArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 24 };
      type.Fields.Add(lastPlayTagLaserArrows);
      FieldDefinition lastPlayTagBrambleArrows = new FieldDefinition("PlayTagBrambleArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 25 };
      type.Fields.Add(lastPlayTagBrambleArrows);
      FieldDefinition lastPlayTagDrillArrows = new FieldDefinition("PlayTagDrillArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 26 };
      type.Fields.Add(lastPlayTagDrillArrows);
      FieldDefinition lastPlayTagBoltArrows = new FieldDefinition("PlayTagBoltArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 27 };
      type.Fields.Add(lastPlayTagBoltArrows);
      FieldDefinition lastPlayTagFeatherArrows = new FieldDefinition("PlayTagFeatherArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 28 };
      type.Fields.Add(lastPlayTagFeatherArrows);
      FieldDefinition lastPlayTagTriggerArrows = new FieldDefinition("PlayTagTriggerArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 29 };
      type.Fields.Add(lastPlayTagTriggerArrows);
      FieldDefinition lastPlayTagPrismArrows = new FieldDefinition("PlayTagPrismArrows", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 30 };
      type.Fields.Add(lastPlayTagPrismArrows);
      FieldDefinition lastPlayTagShield = new FieldDefinition("PlayTagShield", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 31 };
      type.Fields.Add(lastPlayTagShield);
      FieldDefinition lastPlayTagWings = new FieldDefinition("PlayTagWings", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 32 };
      type.Fields.Add(lastPlayTagWings);
      FieldDefinition lastPlayTagSpeedBoots = new FieldDefinition("PlayTagSpeedBoots", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 33 };
      type.Fields.Add(lastPlayTagSpeedBoots);
      FieldDefinition lastPlayTagMirror = new FieldDefinition("PlayTagMirror", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 34 };
      type.Fields.Add(lastPlayTagMirror);
      FieldDefinition lastPlayTagTimeOrb = new FieldDefinition("PlayTagTimeOrb", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 35 };
      type.Fields.Add(lastPlayTagTimeOrb);
      FieldDefinition lastPlayTagDarkOrb = new FieldDefinition("PlayTagDarkOrb", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 36 };
      type.Fields.Add(lastPlayTagDarkOrb);
      FieldDefinition lastPlayTagLavaOrb = new FieldDefinition("PlayTagLavaOrb", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 37 };
      type.Fields.Add(lastPlayTagLavaOrb);
      FieldDefinition lastPlayTagSpaceOrb = new FieldDefinition("PlayTagSpaceOrb", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 38 };
      type.Fields.Add(lastPlayTagSpaceOrb);
      FieldDefinition lastPlayTagChaosOrb = new FieldDefinition("PlayTagChaosOrb", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 39 };
      type.Fields.Add(lastPlayTagChaosOrb);
      FieldDefinition lastPlayTagBomb = new FieldDefinition("PlayTagBomb", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 40 };
      type.Fields.Add(lastPlayTagBomb);
      FieldDefinition lastPlayTagGem = new FieldDefinition("PlayTagGem", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 41 };
      //type.Fields.Add(lastPlayTagGem);
    }
  }
}
