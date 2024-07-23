using System;
using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using TowerFall;
using TowerfallAi.Core;

namespace TowerfallAi.Mod
{
  [Patch]
  public class MyLoader : Loader
  {
    public readonly DateTime creationTime;

    public MyLoader(bool showMessage) : base(showMessage) {
      creationTime = DateTime.Now;
    }

    public override void Render()
    {
      Message = "WAITING AI TO CONNECT ...";
      for (var i = 0; i < (int)(DateTime.Now - creationTime).TotalSeconds; i++) {
        Message += ".";
      }
      base.Render();
    }

  }
}
