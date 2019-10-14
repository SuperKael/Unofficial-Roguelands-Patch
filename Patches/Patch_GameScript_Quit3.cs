using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("Quit3")]
    static class Patch_GameScript_Quit3
    {
        [HarmonyPrefix]
        public static void Prefix(GameScript __instance)
        {
            GameScript.poison = 0;
            GameScript.frost = 0;
            GameScript.burn = 0;
        }
    }
}