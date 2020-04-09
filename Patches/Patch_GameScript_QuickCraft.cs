using UnityEngine;
using HarmonyLib;
using System.Reflection;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("QuickCraft")]
    [HarmonyGadget("URP")]
    public static class Patch_GameScript_QuickCraft
    {
        [HarmonyPrefix]
        public static void Prefix(ref int q)
        {
            if (q < 1) q = 1;
        }
    }
}