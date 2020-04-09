using UnityEngine;
using HarmonyLib;
using System.Reflection;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("PauseMenu")]
    [HarmonyGadget("URP")]
    public static class Patch_GameScript_PauseMenu
    {
        private static bool pausing;

        [HarmonyPrefix]
        public static void Prefix()
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