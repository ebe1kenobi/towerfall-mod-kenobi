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
            Logger.Info("MyVersusRoundResults");
    }

    public override void Render()
    {
      if (this.Components == null)
        return;

      for (var i = 0; i < this.crowns.Length; i++)
      {
        this.crowns[i].Position.X = 100; // 126 origin
      }

      for (var i = 0; i < this.Components.Count; i++) 
      {
        if (this.Components[i].GetType().ToString() != "Monocle.Text") continue;
        Text text = (Text)this.Components[i];

        if (text.text.Length == 0) continue;
        if (! text.text[0].ToString().Equals("P")) continue;
        if (text.text[1].ToString().Equals(" ")) continue; //second pass for NAI 1 AI 1 P 1
        int playerIndex = int.Parse(text.text[1].ToString()) - 1;
        if (!ModCompilKenobi.CurrentPlayerIs(PlayerType.Human, playerIndex)) { 
          text.text = ModCompilKenobi.GetPlayerTypePlaying(playerIndex) + " " + (playerIndex + 1);
          text.Position.X -= 30;
        } else if (text.Position.X != 30) {
          text.text = ModCompilKenobi.GetPlayerTypePlaying(playerIndex) + " " + (playerIndex + 1);
          text.Position.X -= 30;
        }
      }
      base.Render();
    }
  }
}
