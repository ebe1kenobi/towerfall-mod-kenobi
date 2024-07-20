using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using System;
using TowerFall;

namespace TowerfallAi.Mod
{
  [Patch]
  public class ModVersusModeButton : VersusModeButton
  {
    public static string GetModeName(Modes mode)
    {
      switch (mode)
      {
        case Modes.LastManStanding:
          return "LAST MAN STANDING";
        case Modes.HeadHunters:
          return "HEADHUNTERS";
        case Modes.TeamDeathmatch:
          return "TEAM DEATHMATCH";
        case Modes.Warlord:
          return "WARLORD";
        case Modes.PlayTag:
          return "PLAY TAG";
        default:
          throw new Exception("Cannot get name for mode! This should only be used for Versus modes");
      }
    }

    public static Subtexture GetModeIcon(Modes mode)
    {
      switch (mode)
      {
        case Modes.LastManStanding:
          return TFGame.MenuAtlas["gameModes/lastManStanding"];
        case Modes.HeadHunters:
          return TFGame.MenuAtlas["gameModes/headhunters"];
        case Modes.TeamDeathmatch:
          return TFGame.MenuAtlas["gameModes/teamDeathmatch"];
        case Modes.Warlord:
          return TFGame.MenuAtlas["gameModes/warlord"];
        case Modes.PlayTag:
          return TFGame.MenuAtlas["gameModes/warlord"];
        default:
          throw new Exception("Cannot get icon for mode! This should only be used for Versus modes");
      }
    }

    public ModVersusModeButton(Vector2 position, Vector2 tweenFrom)
      : base(position, tweenFrom) {}

    public override void Update()
    {
      base.Update();
      if (!this.Selected)
        return;
      if (MenuInput.Right)
      {
        int mode = (int)MainMenu.VersusMatchSettings.Mode;
        if (mode == 10)
          return;

        int num;
        if (mode == 5)
        {
          num = 10;
        }
        else
        {
          num = mode + 1;
        }
        MainMenu.VersusMatchSettings.Mode = (Modes)num;
        Sounds.ui_move2.Play();
        this.iconWiggler.Start();
        this.OnConfirm();
        this.UpdateSides();
      }
      else
      {
        if (!MenuInput.Left)
          return;
        int mode = (int)MainMenu.VersusMatchSettings.Mode;
        if (mode == 3)
          return;

        int num;
        if (mode == 10)
        {
          num = 5;
        }
        else
        {
          num = mode - 1;
        }
        MainMenu.VersusMatchSettings.Mode = (Modes)num;
        Sounds.ui_move2.Play();
        this.iconWiggler.Start();
        this.OnConfirm();
        this.UpdateSides();
      }
    }

    public override void OnConfirm()
    {
      this.scale = 0.9f;
      this.sine.Restart();
    }

    public override void UpdateSides()
    {
      DrawRight = MainMenu.VersusMatchSettings.Mode < Modes.PlayTag;
      DrawLeft = MainMenu.VersusMatchSettings.Mode > Modes.LastManStanding;
    }
  }
}
