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
        public static bool Prefix(PlayerScript __instance, int id, Rigidbody ___r, ref bool ___canInteract, ref bool ___interacting, ref IEnumerator __result)
        {
            KylockeStand stand = null;
            if (id == 16 || id == 53 && (PlayerScript.curInteractObj == null || (stand = PlayerScript.curInteractObj.GetComponent<KylockeStand>()) == null))
            {
                ___r.velocity = new Vector3(0f, 0f, 0f);
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
                else if (id == 53)
                {
                    PlayerScript.curInteractObj.SendMessage("Request");
                    MonoBehaviour.print("sending request");
                }
                ___canInteract = false;
                __result = FakeRoutine();
                return false;
            }
            else if (stand != null)
            {
                ___r.velocity = new Vector3(0f, 0f, 0f);
                PlayerScript.curInteractObj.SendMessage("Request");
                MonoBehaviour.print("sending request");
                ___canInteract = true;
                ___interacting = false;
                __result = FakeRoutine();
                return false;
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