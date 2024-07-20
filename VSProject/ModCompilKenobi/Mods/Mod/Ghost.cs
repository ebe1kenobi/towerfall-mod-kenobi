﻿using Patcher;
using TowerFall;
using TowerfallAi.Api;
using ModCompilKenobi;

namespace TowerfallAi.Mod {
  [Patch("TowerFall.Ghost")]
  public static class ModGhost {
    public static StateEntity GetState(this Ghost ent) {
      var aiState = new StateSubType { type = "ghost" };
      ExtEntity.SetAiState(ent, aiState);
      aiState.subType = ConversionTypes.GhostTypes.GetB((Ghost.GhostTypes)Util.GetPublicFieldValue("ghostType", ent));
      return aiState;
    }
  }
}
