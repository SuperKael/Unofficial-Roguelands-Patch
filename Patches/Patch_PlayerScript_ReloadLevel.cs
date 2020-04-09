using UnityEngine;
using HarmonyLib;
using System.Reflection;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("ReloadLevel")]
    [HarmonyGadget("URP")]
    public static class Patch_PlayerScript_ReloadLevel
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            Patch_GameScript_RandomEvent.CancelEvent();
        }
    }
}