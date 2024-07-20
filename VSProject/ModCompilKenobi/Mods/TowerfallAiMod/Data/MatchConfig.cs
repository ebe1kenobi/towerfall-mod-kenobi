using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TowerfallAi.Api;

namespace TowerfallAi.Data {
  [DataContract]
  public class MatchConfig {

    [DataMember(EmitDefaultValue = false)]
    public int level;

    [DataMember(EmitDefaultValue = false)]
    public int subLevel;

    [DataMember(EmitDefaultValue = false)]
    public int skipWaves;

    [DataMember(EmitDefaultValue = false)]
    public List<AgentConfig> agents;

    [DataMember(EmitDefaultValue = false)]
    public string mode;

    [DataMember(EmitDefaultValue = false)]
    public string difficulty;

    [DataMember(EmitDefaultValue = false)]
    public string matchLengths;


    [DataMember(EmitDefaultValue = false)]
    public bool randomLevel;

    [DataMember(EmitDefaultValue = false)]
    public TimeSpan agentTimeout;

    [DataMember(EmitDefaultValue = false)]
    public int fps;

    [DataMember(EmitDefaultValue = false)]
    public int[,] solids;

    public override String ToString()
    {
      String s = "{level:" + level + ", mode:" + mode + ", agents:\n";
      foreach (var agent in agents) {
        s += agent.ToString() + "\n";
      }
      s += "}";
      return s;
    }
  }
}
