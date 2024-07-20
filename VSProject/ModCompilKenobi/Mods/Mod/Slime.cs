using Patcher;
using TowerFall;
using TowerfallAi.Api;
using ModCompilKenobi;

namespace TowerfallAi.Mod {
  [Patch("TowerFall.Slime")]
  public static class ModSlime {
    public static StateEntity GetState(this Slime ent) {
      var aiState = new StateSubType { type = "slime" };
      ExtEntity.SetAiState(ent, aiState);
      aiState.subType = ConversionTypes.SlimeTypes.GetB((Slime.SlimeColors)Util.GetPublicFieldValue("slimeColor", ent));
      return aiState;
    }
  }
}
