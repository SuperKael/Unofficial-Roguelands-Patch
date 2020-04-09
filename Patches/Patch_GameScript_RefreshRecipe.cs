using UnityEngine;
using HarmonyLib;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("RefreshRecipe")]
    [HarmonyGadget("URP")]
    public static class Patch_GameScript_RefreshRecipe
    {
        [HarmonyPostfix]
        public static void Postfix(GameScript __instance, int ___curRecipePage, int ___craftType)
        {
            if (___curRecipePage == 1 && ___craftType == 1 && URP.r1t1Fixed != null)
            {
                __instance.menuRecipe.GetComponent<Renderer>().material = URP.r1t1Fixed;
            }
        }
    }
}