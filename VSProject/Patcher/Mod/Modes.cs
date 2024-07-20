using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
  public static class MyModes
  {
    public static void PatchModule(ModuleDefinition baseModule)
    {
      var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.Modes");
      FieldDefinition PlayTag = new FieldDefinition("PlayTag", FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.Public | FieldAttributes.HasDefault, type) { Constant = 10 };
      type.Fields.Add(PlayTag);
    }
  }
}
