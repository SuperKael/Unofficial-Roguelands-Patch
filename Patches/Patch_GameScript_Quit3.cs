using UnityEngine;
using HarmonyLib;
using System.Reflection;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("Quit3")]
    [HarmonyGadget("URP")]
    public static class Patch_GameScript_Quit3
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            GameScript.poison = 0;
            GameScript.frost = 0;
            GameScript.burn = 0;
        }
    }
}