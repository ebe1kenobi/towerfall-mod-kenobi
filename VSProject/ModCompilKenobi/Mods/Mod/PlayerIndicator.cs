using Microsoft.Xna.Framework;
using Patcher;
using TowerFall;

namespace ModCompilKenobi
{
  [Patch]
  public class ModPlayerIndicator : PlayerIndicator
  {
    public ModPlayerIndicator(Vector2 offset, int playerIndex, bool crown)
      : base(offset, playerIndex, crown)
    {
      if (! ModCompilKenobi.CurrentPlayerIs(PlayerType.Human, playerIndex))
      //if (ModCompilKenobi.IsAgentPlaying(playerIndex))
      {
        base.text = ModCompilKenobi.GetPlayerTypePlaying(playerIndex) + " " + (playerIndex + 1).ToString();
      }
    }
  }
}
