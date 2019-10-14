using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("LightSworn")]
    static class Patch_GameScript_LightSworn
    {
        [HarmonyPrefix]
        public static bool Prefix(GameScript __instance)
        {
            PreviewLabs.PlayerPrefs.SetInt("lightsworn", 1);
            __instance.TD(100);
            __instance.TD(100);
            __instance.TD(100);
            return false;
        }
    }
}