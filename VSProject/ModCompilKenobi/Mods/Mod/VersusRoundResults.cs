using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using System.Collections.Generic;
using TowerFall;

namespace ModCompilKenobi
{
  [Patch]
  public class MyVersusRoundResults : VersusRoundResults
  {
    public MyVersusRoundResults(Session session, List<EventLog> events) : base(session, events)
    {
    }

    public override void Render()
    {
      if (this.Components == null)
        return;
      for (var i = 0; i < this.Components.Count; i++) 
      {
        if (this.Components[i].GetType().ToString() != "Monocle.Text") continue;
        Text text = (Text)this.Components[i];

        if (text.text.Length == 0) continue;
        if (! text.text[0].ToString().Equals("P")) continue;
        int playerIndex = int.Parse(text.text[1].ToString()) - 1;
        if (NAIMod.NAIMod.IsAgentPlaying(playerIndex)) {
          text.text = "AI " + (playerIndex + 1);
          text.Position.X -= 7;
        }
      }
      base.Render();
    }
  }
}
