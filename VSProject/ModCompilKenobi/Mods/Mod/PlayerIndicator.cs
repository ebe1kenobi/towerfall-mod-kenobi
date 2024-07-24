using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using TowerFall;
using TowerfallAi.Api;

namespace ModCompilKenobi
{
  [Patch]
  public class ModPlayerIndicator : PlayerIndicator
  {
    public ModPlayerIndicator(Vector2 offset, int playerIndex, bool crown)
      : base(offset, playerIndex, crown)
    {
      if (NAIMod.NAIMod.IsAgentPlaying(playerIndex))
      {
        base.text = "AI " + (playerIndex + 1).ToString();
      }
    }
  }
}
