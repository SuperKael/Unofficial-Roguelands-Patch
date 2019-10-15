using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;

namespace URP.Patches
{
    [HarmonyPatch(typeof(EnemyScript))]
    [HarmonyPatch("DropLocal")]
    static class Patch_EnemyScript_DropLocal
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction[] codes = instructions.ToArray();
            for (int i = 38;i < codes.Length;i++)
            {
                if (codes[i - 38] != null && codes[i - 38].opcode == OpCodes.Ldloc_S
                 && codes[i - 37] != null && codes[i - 37].opcode == OpCodes.Callvirt
                 && codes[i - 36] != null && codes[i - 36].opcode == OpCodes.Ldarg_0
                 && codes[i - 35] != null && codes[i - 35].opcode == OpCodes.Call
                 && codes[i - 34] != null && codes[i - 34].opcode == OpCodes.Ldstr
                 && codes[i - 33] != null && codes[i - 33].opcode == OpCodes.Call
                 && codes[i - 32] != null && codes[i - 32].opcode == OpCodes.Castclass
                 && codes[i - 31] != null && codes[i - 31].opcode == OpCodes.Ldsfld
                 && codes[i - 30] != null && codes[i - 30].opcode == OpCodes.Ldc_R4
                 && codes[i - 29] != null && codes[i - 29].opcode == OpCodes.Div
                 && codes[i - 28] != null && codes[i - 28].opcode == OpCodes.Callvirt
                 && codes[i - 27] != null && codes[i - 27].opcode == OpCodes.Ldc_I4_S
                 && codes[i - 26] != null && codes[i - 26].opcode == OpCodes.Newarr
                 && codes[i - 25] != null && codes[i - 25].opcode == OpCodes.Dup
                 && codes[i - 24] != null && codes[i - 24].opcode == OpCodes.Ldc_I4_0
                 && codes[i - 23] != null && codes[i - 23].opcode == OpCodes.Ldc_I4
                 && codes[i - 22] != null && codes[i - 22].opcode == OpCodes.Stelem_I4
                 && codes[i - 21] != null && codes[i - 21].opcode == OpCodes.Dup
                 && codes[i - 20] != null && codes[i - 20].opcode == OpCodes.Ldc_I4_1
                 && codes[i - 19] != null && codes[i - 19].opcode == OpCodes.Ldc_I4_1
                 && codes[i - 18] != null && codes[i - 18].opcode == OpCodes.Stelem_I4
                 && codes[i - 17] != null && codes[i - 17].opcode == OpCodes.Dup
                 && codes[i - 16] != null && codes[i - 16].opcode == OpCodes.Ldc_I4_3
                 && codes[i - 15] != null && codes[i - 15].opcode == OpCodes.Ldarg_0
                 && codes[i - 14] != null && codes[i - 14].opcode == OpCodes.Call
                 && codes[i - 13] != null && codes[i - 13].opcode == OpCodes.Stelem_I4
                 && codes[i - 12] != null && codes[i - 12].opcode == OpCodes.Stloc_S
                 && codes[i - 11] != null && codes[i - 11].opcode == OpCodes.Ldstr
                 && codes[i - 10] != null && codes[i - 10].opcode == OpCodes.Call
                 && codes[i - 9] != null && codes[i - 9].opcode == OpCodes.Ldarg_0
                 && codes[i - 8] != null && codes[i - 8].opcode == OpCodes.Ldfld
                 && codes[i - 7] != null && codes[i - 7].opcode == OpCodes.Callvirt
                 && codes[i - 6] != null && codes[i - 6].opcode == OpCodes.Call
                 && codes[i - 5] != null && codes[i - 5].opcode == OpCodes.Call
                 && codes[i - 4] != null && codes[i - 4].opcode == OpCodes.Castclass
                 && codes[i - 3] != null && codes[i - 3].opcode == OpCodes.Stloc_S
                 && codes[i - 2] != null && codes[i - 2].opcode == OpCodes.Ldloc_S
                 && codes[i - 1] != null && codes[i - 1].opcode == OpCodes.Ldstr
                 && codes[i] != null && codes[i].opcode == OpCodes.Ldloc_S)
                {
                    codes[i].operand = codes[i - 12].operand;
                }
            }
            return codes.Where(x => x != null);
        }
    }
}