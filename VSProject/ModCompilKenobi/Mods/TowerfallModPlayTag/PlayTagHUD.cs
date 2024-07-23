
using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;
using ModCompilKenobi;

namespace TowerfallAi.Mod
{
  public class PlayTagHUD : Component
  {
    public static readonly Color TriggerColorA = Calc.HexToColor("FF2E16");
    private Player player;
    private Color triggerColor;

    public PlayTagHUD()
      : base(true, false)
    {
      this.triggerColor = ArrowHUD.TriggerColorA;
    }

    public override void Added()
    {
      base.Added();
      this.player = this.Entity as Player;
    }

    public override void Removed()
    {
      base.Removed();
      this.player = (Player) null;
    }

    public void ShowAtStart()
    {
        return;
    }

    public override void Update()
    {
    }

    public override void Render() 
    {
      if (player.playTagCountDown <= 0)
      { // Yes I know, it's so bad to put that here ...
        foreach (Player p in player.Level.Session.CurrentLevel[GameTags.Player])
        {
          p.playTagCountDownOn = false;
        }
        Player.ShootLock = false;
        Explosion.SpawnSuper(player.Level, player.Position, player.PlayerIndex, true);
      }

      Draw.OutlineTextCentered(TFGame.Font, player.playTagCountDown.ToString(), Calc.Floor(player.Position + new Vector2(0f, -15f)), triggerColor, new Vector2(1f, 1f));
    }
  }
}
