using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;

namespace URP.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("HyperBeam")]
    static class Patch_PlayerScript_HyperBeam
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<Label> labels = new List<Label>();
            CodeInstruction[] codes = instructions.ToArray();
            for (int i = 0; i < codes.Length; i++)
            {
                if (codes[i] != null && codes[i].opcode == OpCodes.Ldstr && (string)codes[i].operand == "Shock2")
                {
                    codes[i].operand = "HyperBeam2";
                    return codes;
                }
            }
            throw new Exception("String \"Shock2\" not found in PlayerScript.HyperBeam. Patch was not applied.");
        }
    }
}