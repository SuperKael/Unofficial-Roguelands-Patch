using HarmonyLib;
using GadgetCore.API;
using System.Collections;
using UnityEngine;
using System.Reflection;
using System;

namespace SchegsFix.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("ShootProjectile")]
    [HarmonyGadget("URP")]
    public static class Patch_PlayerScript_ShootProjectile
    {
        [HarmonyPrefix]
        public static bool Prefix(int id, Vector3 targ, int dmg, PlayerScript __instance)
        {
            if (id < 492 || id > 494)
                return true;
            else
            {
                string shotName;
                if (id == 492) // shot 0
                    shotName = "schegF";
                else if (id == 493) // shot 1
                    shotName = "schegT";
                else // shot 2
                    shotName = "schegI";
                GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("proj/" + shotName), __instance.shot.transform.position, Quaternion.identity);
                Package2 value5 = new Package2(targ, dmg, id, GameScript.MODS[10]); // note: client's mods -- vanilla bug
                gameObject.SendMessage("Set", value5);
                return false;
            }
        }
    }
}