using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections;
using System;

namespace URP.Patches
{
    [HarmonyPatch(typeof(ScarabScript))]
    [HarmonyPatch("Boost")]
    static class Patch_ScarabScript_Boost
    {
        [HarmonyPrefix]
        public static bool Prefix(ScarabScript __instance, ref IEnumerator __result)
        {
            if (__instance.isFell)
            {
                Type type = typeof(ScarabScript);
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                __result = new BoostCoroutine(__instance, type.GetField("orbs", flags), type.GetField("egg", flags), type.GetField("speed", flags), type.GetField("target", flags));
                return false;
            }
            return true;
        }
    }

    public sealed class BoostCoroutine : CoroutineOverrideTemplate<ScarabScript>
    {
        public BoostCoroutine(ScarabScript instance, params FieldInfo[] parameters) : base(instance, parameters) { }

        public override YieldInstruction Next(int PC, ScarabScript instance, ref object[] parameters)
        {
            switch (PC % 3)
            {
                case 0:
                    return new WaitForSeconds(2f);
                case 1:
                    if (parameters[3] == null) return new WaitForSeconds(3f);
                    if (UnityEngine.Random.Range(0, 3) == 0)
                    {
                        if (((int)parameters[0]) < 12)
                        {
                            instance.GetComponent<NetworkView>().RPC("Au2", RPCMode.All, new object[0]);
                            ((GameObject[])parameters[1])[(int)parameters[0]] = (GameObject)Network.Instantiate(Resources.Load("haz/fellOrb"), instance.transform.position, Quaternion.identity, 0);
                            parameters[0] = ((int)parameters[0]) + 1;
                        }
                        else
                        {
                            instance.GetComponent<NetworkView>().RPC("Au3", RPCMode.All, new object[0]);
                            GameObject gameObject = (GameObject)Network.Instantiate(Resources.Load("proj/fellProj"), instance.transform.position, Quaternion.identity, 0);
                            gameObject.SendMessage("EnemySet", ((Transform)parameters[3]).transform.position, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else
                    {
                        instance.GetComponent<NetworkView>().RPC("Au3", RPCMode.All, new object[0]);
                        GameObject gameObject2 = (GameObject)Network.Instantiate(Resources.Load("proj/fellProj"), instance.transform.position, Quaternion.identity, 0);
                        gameObject2.SendMessage("EnemySet", ((Transform)parameters[3]).transform.position, SendMessageOptions.DontRequireReceiver);
                    }
                    parameters[2] = 20f;
                    return new WaitForSeconds(3f);
                case 2:
                    if (parameters[3] == null) return new WaitForSeconds(3f);
                    if (UnityEngine.Random.Range(0, 3) == 0)
                    {
                        if (((int)parameters[0]) < 12)
                        {
                            instance.GetComponent<NetworkView>().RPC("Au2", RPCMode.All, new object[0]);
                            ((GameObject[])parameters[1])[(int)parameters[0]] = (GameObject)Network.Instantiate(Resources.Load("haz/fellOrb"), instance.transform.position, Quaternion.identity, 0);
                            parameters[0] = ((int)parameters[0]) + 1;
                        }
                        else
                        {
                            instance.GetComponent<NetworkView>().RPC("Au3", RPCMode.All, new object[0]);
                            Network.Instantiate(Resources.Load("proj/fellProj"), instance.transform.position, Quaternion.identity, 0);
                        }
                    }
                    else
                    {
                        instance.GetComponent<NetworkView>().RPC("Au2", RPCMode.All, new object[0]);
                        GameObject gameObject4 = (GameObject)Network.Instantiate(Resources.Load("e/fellSlime0"), instance.transform.position, Quaternion.identity, 0);
                        gameObject4.SendMessage("EnemySet", ((Transform)parameters[3]).transform.position, SendMessageOptions.DontRequireReceiver);
                    }
                    parameters[2] = 15f;
                    return new WaitForEndOfFrame();
            }
            return null;
        }
    }
}