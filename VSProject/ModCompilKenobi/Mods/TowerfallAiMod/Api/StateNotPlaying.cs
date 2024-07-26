using TowerfallAi.Core;

namespace TowerfallAi.Api {
  public class StateNotPlaying : State {
    public StateNotPlaying() {
      type = "notplaying";
      version = AiMod.ModAiVersion;
    }

    // The version of the mod.
    public string version;

    // The index of the player
    public int index;
    public int id;
  }
}
