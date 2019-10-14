using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("PauseMenu")]
    static class Patch_GameScript_PauseMenu
    {
        private static bool pausing;

        [HarmonyPrefix]
        public static void Prefix(GameScript __instance)
        {
            pausing = GameScript.pausing;
        }

        [HarmonyPostfix]
        public static void Postfix(GameScript __instance)
        {
            if (!pausing)
            {
                __instance.txtQuit[0].text = "SAVE AND QUIT";
                __instance.txtQuit[1].text = __instance.txtQuit[0].text;
            }
        }
    }
}