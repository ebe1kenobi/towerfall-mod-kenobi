using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;

namespace Patcher
{
  public static class MyVersusModeButton
  {
    public static void PatchModule(ModuleDefinition baseModule)
    {
      var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VersusModeButton");
      var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.VersusModeButton::Update()");
      var instructions = method.Body.Instructions;
      //insert a Ret after the base.Update(); -> 3rd or 4th Instruction .?
      //from
      //base.Update();
      //if (!this.Selected)
      //  return;
      //to
      //base.Update();
      //return; <<<<<<<<<< here
      //if (!this.Selected)
      //  return;

      instructions.Insert(2, Instruction.Create(OpCodes.Ret));
    }
  }
}
