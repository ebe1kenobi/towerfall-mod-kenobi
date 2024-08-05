using System.Reflection;
using Patcher;
using TowerFall;
using TowerfallAi.Api;
using ModCompilKenobi;

namespace TowerfallAi.Mod {
  [Patch("TowerFall.QuestSpawnPortal")]
  public static class ModQuestSpawnPortal {
    public static StateEntity GetState(this QuestSpawnPortal ent) {
      if (!(bool)Util.GetFieldValue("appeared", typeof(QuestSpawnPortal), ent, BindingFlags.Instance | BindingFlags.Public)) {
        return null;
      }

      var aiState = new StateEntity {
        type = "portal",
      };

      ExtEntity.SetAiState(ent, aiState);
      return aiState;
    }
  }
}
