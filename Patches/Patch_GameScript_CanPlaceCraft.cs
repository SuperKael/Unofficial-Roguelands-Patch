using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("CanPlaceCraft")]
    static class Patch_GameScript_CanPlaceCraft
    {
        [HarmonyPrefix]
        public static bool Prefix(GameScript __instance, int a, ref bool __result, ref Item[] ___craft, int ___craftType)
        {
            if (___craftType == 3)
            {
                __result = (bool) typeof(GameScript).GetMethod("CanPlaceCraft2", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { a });
                return false;
            }
            return true;
        }
    }
}