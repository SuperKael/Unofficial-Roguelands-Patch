using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("QuickCraft")]
    static class Patch_GameScript_QuickCraft
    {
        [HarmonyPrefix]
        public static void Prefix(GameScript __instance, ref int q)
        {
            if (q < 1) q = 1;
        }
    }
}