using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MySession
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.Session");
            type.Fields.Add(new FieldDefinition("NbPlayTagPickupActivated", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(int))));
      
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.Session::.ctor(TowerFall.MatchSettings)");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.Session::EndRound()");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
        }

        public static void ChangeFoursToEights(Instruction i)
        {
            if (i.OpCode.Code == Code.Ldc_I4_4)
            {
                i.OpCode = OpCodes.Ldc_I4_8;
            }
        }
    }
}
