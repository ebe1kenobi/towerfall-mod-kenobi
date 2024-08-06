using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Timers;

namespace Patcher
{
  public static class MyOptions
  {
    static MethodDefinition ctor;
    public static void PatchModule(ModuleDefinition baseModule)
    {
      var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.Options");

      initConstructor(baseModule, type);
      FieldDefinition EnablePlayTagChestTreasure = new FieldDefinition("EnablePlayTagChestTreasure", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(bool)));
      ModifyInstanceConstructorBool(baseModule, type, EnablePlayTagChestTreasure, true);
      type.Fields.Add(EnablePlayTagChestTreasure);

      FieldDefinition DelayGameTagPlayTagCountDown = new FieldDefinition("DelayGameTagPlayTagCountDown", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(int)));
      ModifyInstanceConstructorInt(baseModule, type, DelayGameTagPlayTagCountDown, 15);
      type.Fields.Add(DelayGameTagPlayTagCountDown);

      FieldDefinition DelayPickupPlayTagCountDown = new FieldDefinition("DelayPickupPlayTagCountDown", Mono.Cecil.FieldAttributes.Public, baseModule.ImportReference(typeof(int)));
      ModifyInstanceConstructorInt(baseModule, type, DelayPickupPlayTagCountDown, 10);
      type.Fields.Add(DelayPickupPlayTagCountDown);
    }

    private static void initConstructor(ModuleDefinition module, TypeDefinition type) {
      // Find or create an instance constructor (.ctor)
      if (ctor == null)
      {
        ctor = type.Methods.FirstOrDefault(m => m.Name == ".ctor" && m.IsConstructor);
        if (ctor == null)
        {
          // If no instance constructor exists, create one
          ctor = new MethodDefinition(
              ".ctor",
              MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
              module.TypeSystem.Void);

          type.Methods.Add(ctor);

          var il = ctor.Body.GetILProcessor();
          il.Append(il.Create(OpCodes.Ldarg_0));  // Load "this"
          il.Append(il.Create(OpCodes.Call, module.ImportReference(typeof(object).GetConstructor(new Type[0]))));  // Call base constructor
          il.Append(il.Create(OpCodes.Ret));
        }
      }
    }
    private static void ModifyInstanceConstructorInt(ModuleDefinition module, TypeDefinition type, FieldDefinition field, int value)
    {
      // Insert IL to initialize the field
      var ilProcessor = ctor.Body.GetILProcessor();
      var returnInstruction = ctor.Body.Instructions.Last(i => i.OpCode == OpCodes.Ret);

      // Load "this" onto the evaluation stack
      ilProcessor.InsertBefore(returnInstruction, ilProcessor.Create(OpCodes.Ldarg_0));

      // Load the initial value (e.g., 15) onto the evaluation stack
      ilProcessor.InsertBefore(returnInstruction, ilProcessor.Create(OpCodes.Ldc_I4, value));


      // Store the value in the instance field
      ilProcessor.InsertBefore(returnInstruction, ilProcessor.Create(OpCodes.Stfld, field));
    }

    private static void ModifyInstanceConstructorBool(ModuleDefinition module, TypeDefinition type, FieldDefinition field, bool value)
    {
      // Insert IL to initialize the field
      var ilProcessor = ctor.Body.GetILProcessor();
      var returnInstruction = ctor.Body.Instructions.Last(i => i.OpCode == OpCodes.Ret);

      // Load "this" onto the evaluation stack
      ilProcessor.InsertBefore(returnInstruction, ilProcessor.Create(OpCodes.Ldarg_0));

      // Load the initial value (e.g., 15) onto the evaluation stack
      ilProcessor.InsertBefore(returnInstruction, ilProcessor.Create(value == true ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0));


      // Store the value in the instance field
      ilProcessor.InsertBefore(returnInstruction, ilProcessor.Create(OpCodes.Stfld, field));
    }
  }
}
