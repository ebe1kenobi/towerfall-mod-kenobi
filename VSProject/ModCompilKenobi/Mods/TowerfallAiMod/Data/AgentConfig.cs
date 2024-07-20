using System;
using System.Runtime.Serialization;
using TowerfallAi.Core;

namespace TowerfallAi.Data {
  [DataContract]
  public class AgentConfig {
    public static class Type {
      public static string Human = "human";
      public static string Remote = "remote";
    }

    [DataMember(EmitDefaultValue = true)]
    public string type;

    [DataMember(EmitDefaultValue = false)]
    public string team;

    [DataMember(EmitDefaultValue = false)]
    public string archer;

    public override String ToString()
    {
      return "{type:" + type + ", team:" + team + ", archer:" + archer + "}";
    }
  }
}
