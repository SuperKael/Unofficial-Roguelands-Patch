using UnityEngine;
using HarmonyLib;
using System.Reflection;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(Millipede))]
    [HarmonyPatch("DropLocal")]
    [HarmonyGadget("URP")]
    public static class Patch_Millipede_DropLocal
    {
        [HarmonyPostfix]
        public static void Postfix(Millipede __instance, ref Transform ___t)
        {
            if (__instance.isLava)
            {
                if (__instance.isMykonogre)
                {
                    if (UnityEngine.Random.Range(0, 20) == 0)
                    {
                        __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/glitter"), Menuu.soundLevel / 10f);
                        int[] array8 = new int[11];
                        array8[0] = 790;
                        array8[1] = 1;
                        array8[3] = (int)typeof(Millipede).GetMethod("GetRandomTier", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { });
                        GameObject gameObject8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("i3"), ___t.position, Quaternion.identity);
                        gameObject8.SendMessage("InitL", array8);
                        __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/glitter"), Menuu.soundLevel / 10f);
                        int[] array9 = new int[11];
                        array9[0] = 890;
                        array9[1] = 1;
                        array9[3] = (int)typeof(Millipede).GetMethod("GetRandomTier", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { });
                        GameObject gameObject9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("i3"), ___t.position, Quaternion.identity);
                        gameObject9.SendMessage("InitL", array9);
                    }
                }
                else if (__instance.isGlaedria)
                {
                    if (UnityEngine.Random.Range(0, 20) == 0)
                    {
                        __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/glitter"), Menuu.soundLevel / 10f);
                        int[] array8 = new int[11];
                        array8[0] = 796;
                        array8[1] = 1;
                        array8[3] = (int)typeof(Millipede).GetMethod("GetRandomTier", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { });
                        GameObject gameObject8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("i3"), ___t.position, Quaternion.identity);
                        gameObject8.SendMessage("InitL", array8);
                        __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/glitter"), Menuu.soundLevel / 10f);
                        int[] array9 = new int[11];
                        array9[0] = 896;
                        array9[1] = 1;
                        array9[3] = (int)typeof(Millipede).GetMethod("GetRandomTier", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { });
                        GameObject gameObject9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("i3"), ___t.position, Quaternion.identity);
                        gameObject9.SendMessage("InitL", array9);
                    }
                }
            }
        }
    }
}