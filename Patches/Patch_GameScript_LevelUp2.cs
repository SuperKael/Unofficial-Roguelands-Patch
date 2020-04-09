using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("LevelUp2")]
    [HarmonyGadget("URP")]
    public static class Patch_GameScript_LevelUp2
    {
        private static int[] oldStats;

        [HarmonyPrefix]
        public static void Prefix()
        {
            oldStats = GameScript.playerBaseStat;
        }

        [HarmonyPostfix]
        public static void Postfix(GameScript __instance)
        {
            if (Menuu.curAugment == 15)
            {
                GameScript.playerBaseStat[3] += 1;
                __instance.statUptxt[3].text = "TEC+" + (GameScript.playerBaseStat[3] - oldStats[3]);
                __instance.statUptxt[9].text = __instance.statUptxt[3].text;
                __instance.statUp[3].SendMessage("Play");
                __instance.UpdateEnergy();
                typeof(GameScript).GetMethod("RefreshStats", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { });
            }
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            List<CodeInstruction> newCodes = new List<CodeInstruction>();
            for (int i = 0; i < codes.Count; i++)
            {
                if (i > 0 && i < codes.Count - 1 && codes[i - 1] != null && codes[i] != null && codes[i + 1] != null && codes[i].opcode == OpCodes.Ldc_I4_3 && codes[i - 1].opcode == OpCodes.Ldloc_S && codes[i + 1].opcode == OpCodes.Ldelema)
                {
                    newCodes.Add(new CodeInstruction(OpCodes.Ldc_I4_2));
                }
                else
                {
                    newCodes.Add(codes[i]);
                }
            }
            return newCodes.Where(x => x != null);
        }
    }
}