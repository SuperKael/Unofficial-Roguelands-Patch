using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("HyperBeam")]
    [HarmonyGadget("URP")]
    public static class Patch_PlayerScript_HyperBeam
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction[] codes = instructions.ToArray();
            for (int i = 0; i < codes.Length; i++)
            {
                if (codes[i] != null && codes[i].opcode == OpCodes.Ldstr && (string)codes[i].operand == "Shock2")
                {
                    codes[i].operand = "HyperBeam2";
                }
            }
            return codes;
        }
    }
}