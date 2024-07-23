using Microsoft.Xna.Framework;
using Monocle;
using System;
using TowerFall;
using static TowerFall.OrbPickup;
using ModCompilKenobi;
using System.Collections.Generic;

namespace TowerfallAi.Mod
{
  public class PlayTag : Pickup
  {
    public static int countPlayTagPickup = 0;
    public Pickup realPickup;
    private GraphicsComponent graphic;
    private Sprite<int> sprite;
    private Sprite<int> mirror;
    private Image icon;
    private Image image;
    private Image border;
    private Pickups playTagType;
    public static List<Pickups> realPickupPossibleList = new List<Pickups> { 
                                  Pickups.Arrows,
                                  Pickups.BombArrows,
                                  Pickups.SuperBombArrows,
                                  Pickups.LaserArrows,
                                  Pickups.BrambleArrows,
                                  Pickups.DrillArrows,
                                  Pickups.BoltArrows,
                                  Pickups.FeatherArrows,
                                  Pickups.TriggerArrows,
                                  Pickups.PrismArrows,
                                  Pickups.Shield,
                                  Pickups.Wings,
                                  Pickups.SpeedBoots,
                                  Pickups.Mirror,
                                  Pickups.TimeOrb,
                                  Pickups.DarkOrb,
                                  Pickups.LavaOrb,
                                  Pickups.SpaceOrb,
                                };

    private static readonly Color[] BorderColors = new Color[4]
    {
      Calc.HexToColor("F8B8F8"),
      Calc.HexToColor("FFFFFF"),
      Calc.HexToColor("F8B800"),
      Calc.HexToColor("FFFFFF")
    };

    public PlayTag(Vector2 position, Vector2 targetPosition, Pickups pickupType)
      : base(position, targetPosition)
    {
      playTagType = pickupType;
      PlayTag.countPlayTagPickup++;
      switch (playTagType)
      {
        case Pickups.PlayTagArrows:
        case Pickups.PlayTagBombArrows:
        case Pickups.PlayTagSuperBombArrows:
        case Pickups.PlayTagLaserArrows:
        case Pickups.PlayTagBrambleArrows:
        case Pickups.PlayTagDrillArrows:
        case Pickups.PlayTagBoltArrows:
        case Pickups.PlayTagFeatherArrows:
        case Pickups.PlayTagTriggerArrows:
        case Pickups.PlayTagPrismArrows:
          this.Collider = (Collider)new Hitbox(16f, 16f, -8f, -8f);
          this.Tag(GameTags.PlayerCollectible);
          switch (playTagType)
          {
            case Pickups.PlayTagArrows:
              this.graphic = (GraphicsComponent)new Image(TFGame.Atlas["pickups/arrowPickup"]);
              this.graphic.CenterOrigin();
              this.Add((Component)this.graphic);
              break;
            case Pickups.PlayTagBombArrows:
              Sprite<int> sprite1 = new Sprite<int>(TFGame.Atlas["pickups/bombArrows"], 12, 12);
              sprite1.Add(0, 0.3f, 0, 1);
              sprite1.Play(0);
              sprite1.CenterOrigin();
              this.Add((Component)sprite1);
              this.graphic = (GraphicsComponent)sprite1;
              break;
            case Pickups.PlayTagSuperBombArrows:
              Sprite<int> sprite2 = new Sprite<int>(TFGame.Atlas["pickups/superBombArrows"], 12, 12);
              sprite2.Add(0, 0.3f, 0, 1);
              sprite2.Play(0);
              sprite2.CenterOrigin();
              this.Add((Component)sprite2);
              this.graphic = (GraphicsComponent)sprite2;
              break;
            case Pickups.PlayTagLaserArrows:
              Sprite<int> sprite3 = new Sprite<int>(TFGame.Atlas["pickups/laserArrows"], 12, 12);
              sprite3.Add(0, 0.3f, 0, 1);
              sprite3.Play(0);
              sprite3.CenterOrigin();
              this.Add((Component)sprite3);
              this.graphic = (GraphicsComponent)sprite3;
              break;
            case Pickups.PlayTagBrambleArrows:
              this.graphic = (GraphicsComponent)new Image(TFGame.Atlas["pickups/brambleArrows"]);
              this.graphic.CenterOrigin();
              this.Add((Component)this.graphic);
              break;
            case Pickups.PlayTagDrillArrows:
              this.graphic = (GraphicsComponent)new Image(TFGame.Atlas["pickups/drillArrows"]);
              this.graphic.CenterOrigin();
              this.Add((Component)this.graphic);
              break;
            case Pickups.PlayTagBoltArrows:
              Sprite<int> sprite4 = new Sprite<int>(TFGame.Atlas["pickups/boltArrows"], 12, 12);
              sprite4.Add(0, 0.05f, 0, 1, 2);
              sprite4.Play(0);
              sprite4.CenterOrigin();
              this.Add((Component)sprite4);
              this.graphic = (GraphicsComponent)sprite4;
              break;
            case Pickups.PlayTagFeatherArrows:
              this.graphic = (GraphicsComponent)new Image(TFGame.Atlas["pickups/featherArrows"]);
              this.graphic.CenterOrigin();
              this.Add((Component)this.graphic);
              break;
            case Pickups.PlayTagTriggerArrows:
              Sprite<int> spriteInt1 = TFGame.SpriteData.GetSpriteInt("TriggerArrowsPickup");
              spriteInt1.Play(0);
              this.Add((Component)spriteInt1);
              this.graphic = (GraphicsComponent)spriteInt1;
              break;
            case Pickups.PlayTagPrismArrows:
              Sprite<int> spriteInt2 = TFGame.SpriteData.GetSpriteInt("PrismArrowsPickup");
              spriteInt2.Play(0);
              this.Add((Component)spriteInt2);
              this.graphic = (GraphicsComponent)spriteInt2;
              break;
          }
          break;
        case Pickups.PlayTagShield:
          this.Collider = (Collider)new Hitbox(16f, 16f, -8f, -8f);
          this.Tag(GameTags.PlayerCollectible);
          this.sprite = TFGame.SpriteData.GetSpriteInt("Shield");
          this.sprite.CenterOrigin();
          this.sprite.Play(0);
          this.Add((Component)this.sprite);
          this.icon = new Image(TFGame.Atlas["pickups/shieldIcon"]);
          this.icon.CenterOrigin();
          this.icon.Visible = false;
          this.Add((Component)this.icon);
          break;
        case Pickups.PlayTagWings:
          this.Collider = (Collider)new Hitbox(16f, 16f, -8f, -8f);
          this.Tag(GameTags.PlayerCollectible);
          this.image = new Image(TFGame.Atlas["pickups/wings"]);
          this.image.CenterOrigin();
          this.Add((Component)this.image);
          break;
        case Pickups.PlayTagSpeedBoots:
          this.Collider = (Collider)new Hitbox(16f, 16f, -8f, -8f);
          this.Tag(GameTags.PlayerCollectible);
          this.image = new Image(TFGame.Atlas["pickups/speedBoots"]);
          this.image.CenterOrigin();
          this.Add((Component)this.image);
          break;
        case Pickups.PlayTagMirror:
          this.Collider = (Collider)new Hitbox(16f, 16f, -8f, -8f);
          this.Tag(GameTags.PlayerCollectible);
          this.mirror = TFGame.SpriteData.GetSpriteInt("Mirror");
          this.mirror.Play(0);
          this.Add((Component)this.mirror);
          break;
        case Pickups.PlayTagTimeOrb:
        case Pickups.PlayTagDarkOrb:
        case Pickups.PlayTagLavaOrb:
        case Pickups.PlayTagSpaceOrb:
          int index = 0;
          this.Collider = (Collider)new Hitbox(16f, 16f, -8f, -8f);
          this.Tag(GameTags.PlayerCollectible);
          this.border = new Image(TFGame.Atlas["pickups/orbBorder"]);
          this.border.CenterOrigin();
          this.Add((Component)this.border);
          switch (playTagType)
          {
            case Pickups.PlayTagDarkOrb:
              this.sprite = TFGame.SpriteData.GetSpriteInt("DarkOrb");
              index = (int)OrbPickup.OrbTypes.Dark;
              break;
            case Pickups.PlayTagTimeOrb:
              index = (int)OrbPickup.OrbTypes.Time;
              this.sprite = TFGame.SpriteData.GetSpriteInt("TimeOrb");
              break;
            case Pickups.PlayTagLavaOrb:
              index = (int)OrbPickup.OrbTypes.Lava;
              this.sprite = TFGame.SpriteData.GetSpriteInt("FireOrb");
              break;
            case Pickups.PlayTagSpaceOrb:
              index = (int)OrbPickup.OrbTypes.Space;
              this.sprite = TFGame.SpriteData.GetSpriteInt("SpaceOrb");
              break;
          }
          this.sprite.Play(0);
          this.Add((Component)this.sprite);
          this.LightColor = OrbPickup.BorderColors[index].Invert();
          this.border.Color = OrbPickup.BorderColors[index];
          break;
        default: 
          throw new Exception("PlayTag Pickup type not authorized!");
      }
    }

    public void Update()
    {
      base.Update();
      switch (playTagType)
      {
        case Pickups.PlayTagArrows:
          this.graphic.Position = this.DrawOffset;
          break;
        case Pickups.PlayTagShield:
          if (this.Collidable)
          {
            this.sprite.Scale.X = (float)(0.60000002384185791 + 0.10000000149011612 * (double)this.sine.ValueOverTwo);
            this.sprite.Scale.Y = (float)(0.60000002384185791 + 0.10000000149011612 * (double)this.sine.Value);
            this.icon.Scale.X = (float)(1.0 + 0.05000000074505806 * (double)this.sine.ValueOverTwo);
            this.icon.Scale.Y = (float)(1.0 + 0.05000000074505806 * (double)this.sine.Value);
          }
          this.icon.Position = this.sprite.Position = this.DrawOffset;
          break;
        case Pickups.PlayTagWings:
          if (this.Collidable)
          {
            this.image.Scale.X = (float)(1.0 + 0.05000000074505806 * (double)this.sine.ValueOverTwo);
            this.image.Scale.Y = (float)(1.0 + 0.05000000074505806 * (double)this.sine.Value);
          }
          this.image.Position = this.DrawOffset;
          break;
        case Pickups.PlayTagSpeedBoots:
          if (this.Collidable)
          {
            this.image.Scale.X = (float)(1.0 + 0.05000000074505806 * (double)this.sine.ValueOverTwo);
            this.image.Scale.Y = (float)(1.0 + 0.05000000074505806 * (double)this.sine.Value);
          }
          this.image.Position = this.DrawOffset;
          break;
        case Pickups.PlayTagMirror:
          base.Update();
          this.mirror.Position = this.DrawOffset;
          break;
        case Pickups.PlayTagTimeOrb:
        case Pickups.PlayTagDarkOrb:
        case Pickups.PlayTagLavaOrb:
        case Pickups.PlayTagSpaceOrb:
          if (this.Collidable)
          {
            this.sprite.Scale.X = (float)(1.0 + 0.05000000074505806 * (double)this.sine.ValueOverTwo);
            this.sprite.Scale.Y = (float)(1.0 + 0.05000000074505806 * (double)this.sine.Value);
          }
          this.sprite.Position = this.DrawOffset;
          this.border.Scale = this.sprite.Scale;
          this.border.Position = this.sprite.Position;
          if (this.Level.OnInterval(5))
            this.border.Visible = !this.border.Visible;
          break;
      }
    }

    public override void Render()
    {
      switch (playTagType)
      {
        case Pickups.PlayTagArrows:
          this.DrawGlow();
          this.graphic.DrawOutline();
          base.Render();
          break;
        case Pickups.PlayTagShield:
          this.DrawGlow();
          base.Render();
          this.icon.DrawOutline();
          this.icon.Render();
          break;
        case Pickups.PlayTagWings:
          this.DrawGlow();
          this.image.DrawOutline();
          base.Render();
          break;
        case Pickups.PlayTagSpeedBoots:
          this.DrawGlow();
          this.image.DrawOutline();
          base.Render();
          break;
        case Pickups.PlayTagMirror:
          this.DrawGlow();
          this.mirror.DrawOutline();
          base.Render();
          break;
        case Pickups.PlayTagTimeOrb:
        case Pickups.PlayTagDarkOrb:
        case Pickups.PlayTagLavaOrb:
        case Pickups.PlayTagSpaceOrb:
          this.DrawGlow();
          if (this.border.Visible)
            this.border.DrawOutline();
          else
            this.sprite.DrawOutline();
          base.Render();
          break;
      }
    }

    public override void OnPlayerCollide(Player player)
    {
      if (player.playTag)
        return;
      startPlayTag(player);
      Sounds.boss_humanLaugh.Play(player.X);
      this.RemoveSelf();
    }

    public override void TweenUpdate(float t)
    {
      switch (playTagType)
      {
        case Pickups.PlayTagArrows:
          base.TweenUpdate(t);
          this.graphic.Scale = Vector2.One * t;
          break;
        case Pickups.PlayTagShield:
          base.TweenUpdate(t);
          this.sprite.Scale = Vector2.One * t * 0.6f;
          this.icon.Scale = Vector2.One * t;
          break;
        case Pickups.PlayTagWings:
          base.TweenUpdate(t);
          this.image.Scale = Vector2.One * t;
          break;
        case Pickups.PlayTagSpeedBoots:
          base.TweenUpdate(t);
          this.image.Scale = Vector2.One * t;
          break;
        case Pickups.PlayTagMirror:
          base.TweenUpdate(t);
          this.mirror.Scale = Vector2.One * t;
          break;
        case Pickups.PlayTagTimeOrb:
        case Pickups.PlayTagDarkOrb:
        case Pickups.PlayTagLavaOrb:
        case Pickups.PlayTagSpaceOrb:
          base.TweenUpdate(t);
          this.border.Scale = this.sprite.Scale = Vector2.One * t;
          break;
      }
    }

    public void startPlayTag(Player player) {
      if (player.playTag)
        return;
      if (player.playTagCountDownOn)
      {
        return;
      }
      Player.ShootLock = true;
      player.playTagCountDown = player.playTagDelay;
      player.playTag = true;
      player.creationTime = DateTime.Now;
      player.pauseDuration = 0;

      for (var i = 0; i < TFGame.Players.Length; i++)
      {
        Player p = player.Level.Session.CurrentLevel.GetPlayer(i);
        if (p != null) {
          p.playTagCountDownOn = true;
        }
      }
    }

    public override void DoPlayerCollect(Player player)
    {
      startPlayTag(player);
    }
  }
}
