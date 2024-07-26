using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using System;
using TowerFall;
using TowerfallAi.Core;
using NAIMod;
using Microsoft.Xna.Framework.Graphics;

namespace ModCompilKenobi
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
        // set the player with gamepad connected to human control even if AI player is the current player => the human can utilise the 
        // gamepad/keyboard to change the archer options
        if (AiMod.savedHumanPlayerInput[this.playerIndex] != null)
        {
          TFGame.PlayerInputs[this.playerIndex] = AiMod.savedHumanPlayerInput[this.playerIndex];
          input = TFGame.PlayerInputs[this.playerIndex];
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

      if (ModCompilKenobi.IsThereOtherPlayerType(playerIndex))
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
      ((Text)playerName).text = ModCompilKenobi.GetPlayerTypePlaying(this.playerIndex) + (this.playerIndex + 1);
    }

    public static Vector2 GetPosition(int playerIndex)
    {
      if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      {
        return new Vector2(52 + 72 * playerIndex, 100f);
      }

      return new Vector2((float)(0x12 + (0x29 * playerIndex)), 100f);
    }

    private void SetAllPLayerInput() {
      for (var i = 0; i < AiMod.currentPlayerType.Length; i++) {
        switch (AiMod.currentPlayerType[i]) {
          case PlayerType.Human:
            TFGame.PlayerInputs[i] = AiMod.savedHumanPlayerInput[i];
            break;
          case PlayerType.AiMod:
            TFGame.PlayerInputs[i] = AiMod.agents[i];
            break;
          case PlayerType.NAIMod:
            TFGame.PlayerInputs[i] = NAIMod.NAIMod.AgentInputs[i];
            break;
          case PlayerType.None:
            throw new Exception("Player Type not initialised for player " + i);
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
      //Logger.Info("StartVersus");
      //Logger.Info("avant SetAllPLayerInput");
      //AiMod.logGeneralStatus();
      SetAllPLayerInput();
      //Logger.Info("apres SetAllPLayerInput");
      //AiMod.logGeneralStatus();

      base.StartVersus();
    }

    //public bool CurrentPlayerIs(PlayerType type) {
    //   return AiMod.currentPlayerType[playerIndex] == type;
    //}

    public bool HumanControlExists()
    {
      return AiMod.savedHumanPlayerInput[this.playerIndex] != null;
    }
    public override void Render()
    {
      SetPlayerName();

      if (rightArrow.Visible && ModCompilKenobi.IsThereOtherPlayerType(playerIndex))
      {
        if (HumanControlExists()) {
          if (ModCompilKenobi.CurrentPlayerIs(PlayerType.Human, playerIndex)) {
            this.upArrow.Visible = false;
            this.downArrow.Visible = true;
          } 
          else if (ModCompilKenobi.CurrentPlayerIs(PlayerType.AiMod, playerIndex))
          {
            this.upArrow.Visible = true;
            if (NAIMod.NAIMod.NAIModEnabled)
              this.downArrow.Visible = true;  
            else
              this.downArrow.Visible = false;  
          }
          else if (ModCompilKenobi.CurrentPlayerIs(PlayerType.NAIMod, playerIndex))
          {
            this.upArrow.Visible = true;
            this.downArrow.Visible = false;
          }
        } else if (ModCompilKenobi.CurrentPlayerIs(PlayerType.AiMod, playerIndex)) {
          this.upArrow.Visible = false;
          this.downArrow.Visible = true;
        } else if (ModCompilKenobi.CurrentPlayerIs(PlayerType.NAIMod, playerIndex)) {
          this.upArrow.Visible = true;
          this.downArrow.Visible = false;
        }

        this.upArrow.Y = (float)(-68 + (double)this.arrowSine.Value * 3.0 + 6.0 * (this.rightArrowWiggle ? (double)this.arrowWiggle.Value : 0.0));
        this.downArrow.Y = (float)(-50.0 - (double)this.arrowSine.Value * 3.0 + 6.0 * (!this.rightArrowWiggle ? (double)this.arrowWiggle.Value : 0.0));
      }
      else
      {
        this.upArrow.Visible = false;
        this.downArrow.Visible = false;
      }

      base.Render();
    }
    public override int NotJoinedUpdate()
    {
      if (this.input == null)
        return 0;

      if (ModCompilKenobi.IsThereOtherPlayerType(playerIndex)) { //at leat 2 player type
        // Move up 
        if (this.input.MenuUp
            && HumanControlExists()
            && (int)AiMod.currentPlayerType[playerIndex] > (int)PlayerType.Human)
        {
          AiMod.currentPlayerType[playerIndex] = (PlayerType)(int)AiMod.currentPlayerType[playerIndex] - 1;
        }
        else if (this.input.MenuUp  
                && (int)AiMod.currentPlayerType[playerIndex] > (int)PlayerType.AiMod)
        {
          AiMod.currentPlayerType[playerIndex] = (PlayerType)(int)AiMod.currentPlayerType[playerIndex] - 1;
        }

        // Move down
        if (this.input.MenuDown 
            && NAIMod.NAIMod.NAIModEnabled && (int)AiMod.currentPlayerType[playerIndex] < (int)PlayerType.NAIMod)
        {
          AiMod.currentPlayerType[playerIndex] = (PlayerType)(int)AiMod.currentPlayerType[playerIndex] + 1;
        } else if (this.input.MenuDown  
                  && (int)AiMod.currentPlayerType[playerIndex] < (int)PlayerType.AiMod) {
          AiMod.currentPlayerType[playerIndex] = (PlayerType)(int)AiMod.currentPlayerType[playerIndex] + 1;
        }
      }

      if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
        return base.NotJoinedUpdate();

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
