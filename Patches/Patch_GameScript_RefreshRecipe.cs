using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("RefreshRecipe")]
    static class Patch_GameScript_RefreshRecipe
    {
        [HarmonyPostfix]
        public static void Postfix(GameScript __instance, int ___curRecipePage, int ___craftType)
        {
            if (___curRecipePage == 1 && ___craftType == 1)
            {
                __instance.menuRecipe.GetComponent<Renderer>().material = URP.r1t1Fixed;
            }
        }
    }
}