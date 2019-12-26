using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace URP.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("ReloadLevel")]
    static class Patch_PlayerScript_ReloadLevel
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            Patch_GameScript_RandomEvent.CancelEvent();
        }
    }
}