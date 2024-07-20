using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerFall;
using TowerfallAi.Core;

namespace TowerfallAi.Mod
{
    [Patch]
    public class MyRollcallElement : RollcallElement
    {
        public MyRollcallElement(int playerIndex) : base(playerIndex)
        {
        }

        public static Vector2 GetPosition(int playerIndex)
        {
            if (!TF8PlayerMod.TF8PlayerMod.Mod8PEnabled)
            {
                return new Vector2(52 + 72 * playerIndex, 100f);
            }
            return new Vector2((float)(0x12 + (0x29 * playerIndex)), 100f);
        }

        public override int NotJoinedUpdate()
        {
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
