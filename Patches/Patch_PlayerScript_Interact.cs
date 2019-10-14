using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections;

namespace URP.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("Interact")]
    static class Patch_PlayerScript_Interact
    {
        [HarmonyPrefix]
        public static bool Prefix(PlayerScript __instance, int id, GameScript ___gameScript, Rigidbody ___r, ref bool ___canInteract, ref bool ___interacting, ref IEnumerator __result)
        {
            if (id == 16 || id == 53)
            {
                ___r.velocity = new Vector3(0f, 0f, 0f);
                if (GameScript.combatMode)
                {
                    if (id >= 20 && id < 30)
                    {
                        ___gameScript.ExitCM2();
                    }
                    else if (id == 17)
                    {
                        ___gameScript.ExitCM3();
                    }
                    else
                    {
                        ___gameScript.ExitCM();
                    }
                }
                if (GameScript.buildMode)
                {
                    ___gameScript.ExitBuildMode();
                }
                if (id != 6)
                {
                    __instance.w.SetActive(false);
                }
                KylockeStand stand;
                if (id == 16 || PlayerScript.curInteractObj == null || (stand = PlayerScript.curInteractObj.GetComponent<KylockeStand>()) == null)
                {
                    if (id == 16)
                    {
                        if (Network.isServer)
                        {
                            PlayerScript.curInteractObj.SendMessage("Open");
                        }
                        else
                        {
                            PlayerScript.curInteractObj.GetComponent<NetworkView>().RPC("Open", RPCMode.Server, new object[0]);
                        }
                    }
                    else
                    {
                        PlayerScript.curInteractObj.SendMessage("Request");
                        MonoBehaviour.print("sending request");
                    }
                    __instance.W(1);
                    __result = FakeRoutine();
                    ___interacting = false;
                    return false;
                }
                else
                {
                    PlayerScript.curInteractObj.SendMessage("Request");
                    MonoBehaviour.print("sending request");
                    typeof(KylockeStand).GetField("purchased", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(stand, false);
                    __instance.W(0);
                    __result = FakeRoutine();
                    ___interacting = false;
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static IEnumerator FakeRoutine()
        {
            yield break;
        }
    }
}