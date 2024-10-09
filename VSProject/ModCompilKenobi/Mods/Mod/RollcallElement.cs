using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using System;
using TowerFall;
using TowerfallAi.Core;
using NAIMod;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ModCompilKenobi
{
  [Patch]
  public class MyRollcallElement : RollcallElement
  {
    public Text playerName;
    public Image upArrow; 
    public Image downArrow;
    public Image upRightArrow;
    public Image upLeftArrow;
    static public Dictionary<String, int> difficultyLevel = new Dictionary<string, int>();
    public MyRollcallElement(int playerIndex) : base(playerIndex)
    {
      if (AiMod.ModAIEnabled || NAIMod.NAIMod.NAIModEnabled)
      {
        // set the player with gamepad connected to human control even if AI player is the current player => the human can utilise the 
        // gamepad/keyboard to change the archer options
        if (ModCompilKenobi.savedHumanPlayerInput[this.playerIndex] != null)
        {
          TFGame.PlayerInputs[this.playerIndex] = ModCompilKenobi.savedHumanPlayerInput[this.playerIndex];
          input = TFGame.PlayerInputs[this.playerIndex];
        }
      }

      Color color = ((state.State == 1) ? ArcherData.Archers[CharacterIndex].ColorB : ArcherData.Archers[CharacterIndex].ColorA);
      Vector2 positionText = new Vector2();
      if (TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
      {
        positionText = new Vector2(-10, -70);
      }
      else
      {
        positionText = new Vector2(Position.X, 10);
      }

      upArrow = new Image(TFGame.Atlas["versus/playerIndicator"]);
      upArrow.FlipY = true;
      upArrow.Visible = true;
      upArrow.Color = color;
      this.Add((Component)upArrow);
      upArrow.X = -10;
      upArrow.Y = -90;

      downArrow = new Image(TFGame.Atlas["versus/playerIndicator"]);
      downArrow.Visible = true;
      this.Add((Component)downArrow);
      downArrow.X = -10;
      downArrow.Y = -45;
      downArrow.Color = color;

      upRightArrow = new Image(TFGame.MenuAtlas["portraits/arrow"]);
      upRightArrow.Visible = true;
      upRightArrow.Color = color;
      this.Add((Component)upRightArrow);
      upRightArrow.X = 5;
      upRightArrow.Y = -70;

      upLeftArrow = new Image(TFGame.MenuAtlas["portraits/arrow"]);
      upLeftArrow.Visible = true;
      upLeftArrow.FlipX = true;
      upLeftArrow.Color = color;
      this.Add((Component)upLeftArrow);
      upLeftArrow.X = -20;
      upLeftArrow.Y = -70;

      String name = "";
      difficultyLevel["AI"] = 20;
      difficultyLevel["NAI"] = 20;
      playerName = new Text(TFGame.Font, name, positionText, color, Text.HorizontalAlign.Left, Text.VerticalAlign.Center);
      this.Add((Component)playerName);
    }

    public void SetPlayerName() {
      String type = ModCompilKenobi.GetPlayerTypePlaying(this.playerIndex);
      ((Text)playerName).text = type + (this.playerIndex + 1) + (type != "P" ? "\n\nLVL " + difficultyLevel[type] : "");
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
      for (var i = 0; i < ModCompilKenobi.currentPlayerType.Length; i++) {
        switch (ModCompilKenobi.currentPlayerType[i]) {
          case PlayerType.Human:
            TFGame.PlayerInputs[i] = ModCompilKenobi.savedHumanPlayerInput[i];
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
      SetAllPLayerInput();
      base.StartVersus();
    }

    public bool HumanControlExists()
    {
      return ModCompilKenobi.savedHumanPlayerInput[this.playerIndex] != null;
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
            this.upLeftArrow.Visible = false;
            this.upRightArrow.Visible = false;
          } 
          else if (ModCompilKenobi.CurrentPlayerIs(PlayerType.AiMod, playerIndex))
          {
            this.upArrow.Visible = true;
            if (playerIndex == 0) {
              String type = ModCompilKenobi.GetPlayerTypePlaying(this.playerIndex);

              //if (difficultyLevel[type] > 0) { 
                  this.upLeftArrow.Visible = true;
              //} else {
              //  this.upLeftArrow.Visible = false;
              //}
              //if (difficultyLevel[type] < 100) { 
                  this.upRightArrow.Visible = true;
              //} else {
              //  this.upRightArrow.Visible = false;
              //}
            }
            if (NAIMod.NAIMod.NAIModEnabled)
              this.downArrow.Visible = true;  
            else
              this.downArrow.Visible = false;  
          }
          else if (ModCompilKenobi.CurrentPlayerIs(PlayerType.NAIMod, playerIndex))
          {
            this.upArrow.Visible = true;
            this.downArrow.Visible = false;
            if (playerIndex == 0)
            {
              this.upLeftArrow.Visible = true;
              this.upRightArrow.Visible = true;
            }
          }
        } else if (ModCompilKenobi.CurrentPlayerIs(PlayerType.AiMod, playerIndex)) {
          this.upArrow.Visible = false;
          this.downArrow.Visible = true;
          this.upLeftArrow.Visible = false;
          this.upRightArrow.Visible = false;
        } else if (ModCompilKenobi.CurrentPlayerIs(PlayerType.NAIMod, playerIndex)) {
          this.upArrow.Visible = true;
          this.downArrow.Visible = false;
          this.upLeftArrow.Visible = false;
          this.upRightArrow.Visible = false;
        }

        this.upArrow.Y = (float)(-78 + (double)this.arrowSine.Value * 3.0 + 6.0 * (this.rightArrowWiggle ? (double)this.arrowWiggle.Value : 0.0));
        this.downArrow.Y = (float)(-50.0 - (double)this.arrowSine.Value * 3.0 + 6.0 * (!this.rightArrowWiggle ? (double)this.arrowWiggle.Value : 0.0));
        this.upRightArrow.X = (float)(7.0 - (double)this.arrowSine.Value * 3.0 + 6.0 * (!this.rightArrowWiggle ? (double)this.arrowWiggle.Value : 0.0));
        this.upLeftArrow.X = (float)(-24.0 + (double)this.arrowSine.Value * 3.0 + 6.0 * (!this.rightArrowWiggle ? (double)this.arrowWiggle.Value : 0.0));
      }
      else
      {
        this.upArrow.Visible = false;
        this.downArrow.Visible = false;
        this.upLeftArrow.Visible = false;
        this.upRightArrow.Visible = false;
      }

      base.Render();
    }
    public override int NotJoinedUpdate()
    {
      if (this.input == null)
        return 0;

      if ((int)ModCompilKenobi.currentPlayerType[playerIndex] > (int)PlayerType.Human) {
        String type = ModCompilKenobi.GetPlayerTypePlaying(this.playerIndex);

        if (this.input.MenuAlt)
          difficultyLevel[type] += 5;
          if (difficultyLevel[type] > 100) difficultyLevel[type] = 0;
        if (this.input.MenuAlt2)
          difficultyLevel[type] -= 5;
          if (difficultyLevel[type] < 1) difficultyLevel[type] = 100;
      }

      if (ModCompilKenobi.IsThereOtherPlayerType(playerIndex)) { //at leat 2 player type
        // Move up 
        if (this.input.MenuUp
            && HumanControlExists()
            && (int)ModCompilKenobi.currentPlayerType[playerIndex] > (int)PlayerType.Human)
        {
          ModCompilKenobi.currentPlayerType[playerIndex] = (PlayerType)(int)ModCompilKenobi.currentPlayerType[playerIndex] - 1;
        }
        else if (this.input.MenuUp  
                && (int)ModCompilKenobi.currentPlayerType[playerIndex] > (int)PlayerType.AiMod)
        {
          ModCompilKenobi.currentPlayerType[playerIndex] = (PlayerType)(int)ModCompilKenobi.currentPlayerType[playerIndex] - 1;
        }

        // Move down
        if (this.input.MenuDown 
            && NAIMod.NAIMod.NAIModEnabled && (int)ModCompilKenobi.currentPlayerType[playerIndex] < (int)PlayerType.NAIMod)
        {
          ModCompilKenobi.currentPlayerType[playerIndex] = (PlayerType)(int)ModCompilKenobi.currentPlayerType[playerIndex] + 1;
        } else if (this.input.MenuDown  
                  && (int)ModCompilKenobi.currentPlayerType[playerIndex] < (int)PlayerType.AiMod) {
          ModCompilKenobi.currentPlayerType[playerIndex] = (PlayerType)(int)ModCompilKenobi.currentPlayerType[playerIndex] + 1;
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
