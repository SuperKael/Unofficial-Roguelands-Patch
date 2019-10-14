using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("RandomEvent")]
    static class Patch_GameScript_RandomEvent
    {
        private static GameScript instance;
        private static IEnumerator queuedEvent;

        [HarmonyPrefix]
        public static bool Prefix(GameScript __instance)
        {
            instance = __instance;
            if (UnityEngine.Random.Range(0, 10) == 0)
            {
                int num = UnityEngine.Random.Range(0, 3);
                if (num == 0)
                {
                    instance.StartCoroutine(queuedEvent = (IEnumerator)typeof(GameScript).GetMethod("SpacePirates", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(instance, new object[] { }));
                }
                else if (num == 1)
                {
                    instance.StartCoroutine(queuedEvent = (IEnumerator)typeof(GameScript).GetMethod("GlitterBug", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(instance, new object[] { }));
                }
                else if (num == 2)
                {
                    instance.StartCoroutine(queuedEvent = (IEnumerator)typeof(GameScript).GetMethod("Meteors", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(instance, new object[] { }));
                }
            }
            return false;
        }

        public static void CancelEvent()
        {
            if (queuedEvent != null)
            {
                instance.StopCoroutine(queuedEvent);
                queuedEvent = null;
            }
        }
    }
}