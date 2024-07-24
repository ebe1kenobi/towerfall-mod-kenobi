using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModCompilKenobi;
using Monocle;
using NAIMod;
using Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerFall;
using TowerFall.Editor;
using TowerfallAi.Core;

namespace TowerfallAi.Mod
{
  [Patch]
  public class MyRollcallElement : RollcallElement
  {
    public Text playerName;
    public Image upArrow;
    public Image downArrow;

    public MyRollcallElement(int playerIndex) : base(playerIndex)
    {
      if (AiMod.ModAIEnabled || NAIMod.NAIMod.NAIModEnabled)
      {
        this.state.SetCallbacks(3, new Func<int>(this.ChangePlayerTypeUpdate), new Action(this.ChangePlayerTypeJoined), new Action(this.ChangePlayerTypeLeaved));
        if (NAIMod.NAIMod.savedPlayerInput[this.playerIndex] != null)
        {
          TFGame.PlayerInputs[this.playerIndex] = NAIMod.NAIMod.savedPlayerInput[this.playerIndex];
          input = TFGame.PlayerInputs[playerIndex];
        }
        else {
          // AUto set AI for human player with keyboard
          if (NAIMod.NAIMod.NAIModNoKeyboardEnabled
              && NAIMod.NAIMod.TowerfallKeyboardInputName.Equals(TFGame.PlayerInputs[this.playerIndex].GetType().ToString()))
          {
            NAIMod.NAIMod.savedPlayerInput[this.playerIndex] = TFGame.PlayerInputs[this.playerIndex];
          }
        }
      }

      Color color = ((state.State == 1) ? ArcherData.Archers[CharacterIndex].ColorB : ArcherData.Archers[CharacterIndex].ColorA);
      Vector2 positionText = new Vector2();
      if (TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      {
        positionText = new Vector2(-10, -60);
      }
      else
      {
        positionText = new Vector2(Position.X, 10);
      }

      if (NAIMod.NAIMod.IsThereOtherPlayerType(playerIndex))
      {
        upArrow = new Image(TFGame.Atlas["versus/playerIndicator"]);
        upArrow.FlipY = true;
        upArrow.Visible = true;
        upArrow.Color = color;
        this.Add((Component)upArrow);
        upArrow.X = -10;
        upArrow.Y = -70;

        downArrow = new Image(TFGame.Atlas["versus/playerIndicator"]);
        downArrow.Visible = true;
        this.Add((Component)downArrow);
        downArrow.X = -10;
        downArrow.Y = -50;
        downArrow.Color = color;
      }

      String name = "";
      playerName = new Text(TFGame.Font, name, positionText, color, Text.HorizontalAlign.Left, Text.VerticalAlign.Bottom);
      this.Add((Component)playerName);
    }

    public void SetPlayerName() {
      String name = "";

      if (NAIMod.NAIMod.IsAgentPlaying(this.playerIndex))
      {
        name = "AI " + (this.playerIndex + 1);
      }
      else
      {
        name = "P" + (this.playerIndex + 1);
      }
      ((Text)playerName).text = name;
    }
    public static Vector2 GetPosition(int playerIndex)
    {
      if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      {
        return new Vector2(52 + 72 * playerIndex, 100f);
      }

      return new Vector2((float)(0x12 + (0x29 * playerIndex)), 100f);
    }

    public void ChangePlayerTypeJoined()
    {

    }

    public void ChangePlayerTypeLeaved()
    {

    }

    private bool CanChangePlayerTypeSelection() 
    {
      return 
          NAIMod.NAIMod.NAIModEnabled
          &&
          TFGame.PlayerInputs[this.playerIndex] != NAIMod.NAIMod.AgentInputs[this.playerIndex]
        ;
    }

    private void SetAllPLayerInput() {
      for (var i = 0; i < NAIMod.NAIMod.savedPlayerInput.Length; i++) {
        if (NAIMod.NAIMod.savedPlayerInput[i] != null)
        {
          //replace the human control by the AI control before starting match
          TFGame.PlayerInputs[i] = NAIMod.NAIMod.AgentInputs[i];
        }
      }
    }
    public override void ForceStart()
    {
      // Assign real input
      SetAllPLayerInput();
      base.ForceStart();
    }
    public override void StartVersus()
    {
      // Assign real input
      SetAllPLayerInput();

      base.StartVersus();
    }

    public override void Render()
    {
      SetPlayerName();

      if (NAIMod.NAIMod.IsThereOtherPlayerType(playerIndex))
      {
        this.upArrow.Y = (float)(-68 + (double)this.arrowSine.Value * 3.0 + 6.0 * (this.rightArrowWiggle ? (double)this.arrowWiggle.Value : 0.0));
        this.downArrow.Y = (float)(-50.0 - (double)this.arrowSine.Value * 3.0 + 6.0 * (!this.rightArrowWiggle ? (double)this.arrowWiggle.Value : 0.0));
      }
      base.Render();
    }
    public int ChangePlayerTypeUpdate()
    {
      if (this.finishedJoined)
        return 2;

      NAIMod.NAIMod.lastPlayerInput[this.playerIndex] = TFGame.PlayerInputs[this.playerIndex];

      if (TFGame.PlayerInputs[this.playerIndex] != null)
      {
        if (NAIMod.NAIMod.savedPlayerInput[this.playerIndex] == null)
        {
          NAIMod.NAIMod.savedPlayerInput[this.playerIndex] = TFGame.PlayerInputs[this.playerIndex];
        }
        else
        {
          NAIMod.NAIMod.savedPlayerInput[this.playerIndex] = null;
        }
      }

      return 0;
    }

    public override int NotJoinedUpdate()
    {
      if (this.input == null)
        return 0;

      if ((this.input.MenuDown || this.input.MenuUp) && this.CanChangePlayerTypeSelection())
      {
        return 3;
      }

      if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      {
        return base.NotJoinedUpdate();
      }

      if (this.input != null)
      {
        if (this.input.MenuBack && !base.MainMenu.Transitioning)
        {
          for (int i = 0; i < 8; i++)
          {
            TFGame.Players[i] = false;
          }
          Sounds.ui_clickBack.Play(160f, 1f);
          if ((MainMenu.RollcallMode == MainMenu.RollcallModes.Versus) || (MainMenu.RollcallMode == MainMenu.RollcallModes.Trials))
          {
            base.MainMenu.State = MainMenu.MenuState.Main;
          }
          else
          {
            base.MainMenu.State = MainMenu.MenuState.CoOp;
          }
        }
        else if (this.input.MenuLeft && this.CanChangeSelection)
        {
          this.drawDarkWorldLock = false;
          this.ChangeSelectionLeft();
          Sounds.ui_move2.Play(160f, 1f);
          this.arrowWiggle.Start();
          this.rightArrowWiggle = false;
        }
        else if (this.input.MenuRight && this.CanChangeSelection)
        {
          this.drawDarkWorldLock = false;
          this.ChangeSelectionRight();
          Sounds.ui_move2.Play(160f, 1f);
          this.arrowWiggle.Start();
          this.rightArrowWiggle = true;
        }
        else if (this.input.MenuAlt && GameData.DarkWorldDLC)
        {
          this.drawDarkWorldLock = false;
          this.altWiggle.Start();
          Sounds.ui_altCostumeShift.Play(base.X, 1f);
          if (this.archerType == ArcherData.ArcherTypes.Normal)
          {
            this.archerType = ArcherData.ArcherTypes.Alt;
          }
          else
          {
            this.archerType = ArcherData.ArcherTypes.Normal;
          }
          this.portrait.SetCharacter(this.CharacterIndex, this.archerType, 1);
        }
        else if ((this.input.MenuConfirmOrStart && !TFGame.CharacterTaken(this.CharacterIndex)) && (TFGame.PlayerAmount < this.MaxPlayers))
        {
          if (ArcherData.Get(this.CharacterIndex, this.archerType).RequiresDarkWorldDLC && !GameData.DarkWorldDLC)
          {
            this.drawDarkWorldLock = true;
            if ((this.darkWorldLockEase < 1f) || !TFGame.OpenStoreDarkWorldDLC())
            {
              this.portrait.Shake();
              this.shakeTimer = 30f;
              Sounds.ui_invalid.Play(base.X, 1f);
              if (TFGame.PlayerInputs[this.playerIndex] != null)
              {
                TFGame.PlayerInputs[this.playerIndex].Rumble(1f, 20);
              }
            }
            return 0;
          }
          if ((this.input.MenuAlt2Check && (this.archerType == ArcherData.ArcherTypes.Normal)) && (ArcherData.SecretArchers[this.CharacterIndex] != null))
          {
            this.archerType = ArcherData.ArcherTypes.Secret;
            this.portrait.SetCharacter(this.CharacterIndex, this.archerType, 1);
          }
          this.portrait.Join(false);
          TFGame.Players[this.playerIndex] = true;
          TFGame.AltSelect[this.playerIndex] = this.archerType;
          if (TFGame.PlayerInputs[this.playerIndex] != null)
          {
            TFGame.PlayerInputs[this.playerIndex].Rumble(1f, 20);
          }
          this.shakeTimer = 20f;
          if (TFGame.PlayerAmount == this.MaxPlayers)
          {
            this.ForceStart();
          }
          return 1;
        }
      }
      return 0;
    }

    public int MaxPlayers
    {
      get
      {
        switch (MainMenu.RollcallMode)
        {
          case MainMenu.RollcallModes.Quest:
          case MainMenu.RollcallModes.DarkWorld:
          case MainMenu.RollcallModes.Trials:
              return base.MaxPlayers;
        }
        return (TF8PlayerMod.TF8PlayerMod.Mod8PEnabled) ? 8 : base.MaxPlayers;
      }
    }
  }
}
