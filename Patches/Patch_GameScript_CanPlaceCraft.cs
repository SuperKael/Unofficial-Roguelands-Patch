using UnityEngine;
using HarmonyLib;
using System.Reflection;
using UModFramework.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("CanPlaceCraft")]
    static class Patch_GameScript_CanPlaceCraft
    {
        [HarmonyPrefix]
        public static bool Prefix(GameScript __instance, int a, ref bool __result, int ___craftType)
        {
            if (UMFMod.GetMod("GadgetCore") != null) return true;
            if (___craftType == 3)
            {
                __result = (bool) typeof(GameScript).GetMethod("CanPlaceCraft2", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { a });
                return false;
            }
            return true;
        }
    }
}