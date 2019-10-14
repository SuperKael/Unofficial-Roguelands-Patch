using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;

namespace URP.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("Update")]
    static class Patch_PlayerScript_Update
    {
        private static FieldInfo energyField = typeof(GameScript).GetField("energy", BindingFlags.Public | BindingFlags.Static);

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            List<CodeInstruction> newCodes = new List<CodeInstruction>();
            for (int i = 1;i < codes.Count;i++)
            {
                if (i < codes.Count - 2 && codes[i - 1] != null && codes[i] != null && codes[i + 1] != null && codes[i].opcode == OpCodes.Ldc_I4_0 && codes[i - 1].opcode == OpCodes.Ldsfld && codes[i - 1].operand == energyField && codes[i + 1].opcode == OpCodes.Ble)
                {
                    newCodes.Add(new CodeInstruction(OpCodes.Call, typeof(Patch_PlayerScript_Update).GetMethod("NeedsEnergy", BindingFlags.Public | BindingFlags.Static)));
                    newCodes.Add(new CodeInstruction(OpCodes.Nop));
                    newCodes.Add(new CodeInstruction(OpCodes.Brtrue, codes[i + 1].operand));
                    i += 2;
                }
                else
                {
                    newCodes.Add(codes[i - 1]);
                }
            }
            newCodes.Add(codes[codes.Count - 1]);
            return newCodes.Where(x => x != null);
        }

        public static bool NeedsEnergy()
        {
            return GameScript.energy <= 0 && Menuu.curAugment != 2;
        }
    }
}