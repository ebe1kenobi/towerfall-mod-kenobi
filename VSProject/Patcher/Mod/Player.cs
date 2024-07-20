using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Timers;

namespace Patcher
{
  public static class MyPlayer
  {
    public static void PatchModule(ModuleDefinition baseModule)
    {
      var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.Player");
      type.Fields.Add(new FieldDefinition("playTag", Mono.Cecil.FieldAttributes.Public , baseModule.ImportReference(typeof(Boolean))));
      type.Fields.Add(new FieldDefinition("playTagCountDown", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(int))));
      type.Fields.Add(new FieldDefinition("playTagCountDownOn", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(Boolean))));
      type.Fields.Add(new FieldDefinition("playTagDelay", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(int))));
      type.Fields.Add(new FieldDefinition("playTagDelayModePlayTag", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(int))));
      type.Fields.Add(new FieldDefinition("creationTime", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(DateTime))));
    }
  }
}
