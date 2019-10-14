using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("ShiftClickStorage")]
    static class Patch_GameScript_ShiftClickStorage
    {
        private static Item itemInSlot;

        [HarmonyPrefix]
        public static bool Prefix(GameScript __instance, ref bool ___shiftclicking, Item[] ___inventory, ref Item[] ___storage, int slot, int ___curStoragePage, ref IEnumerator __result)
        {
            int num = slot + ___curStoragePage * 30;
            itemInSlot = ___storage[num];
            if (!___shiftclicking && (itemInSlot.id < 300 || itemInSlot.id >= 2000))
            {
                bool flag1 = false, flag2 = false;
                int num2 = 0;
                int id = ___storage[num].id;
                for (int i = 0; i < 36; i++)
                {
                    if (___inventory[i].id == id && ___inventory[i].q < 9999)
                    {
                        if (___inventory[i].q + ___storage[num].q <= 9999)
                        {
                            flag1 = true;
                            num2 = i;
                            break;
                        }
                        else
                        {
                            if (!flag2) __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                            ___storage[num].q -= 9999 - ___inventory[i].q;
                            ___inventory[i].q = 9999;
                            typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { i });
                            flag2 = true;
                        }
                    }
                }
                if (flag1)
                {
                    __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                    ___inventory[num2].q += ___storage[num].q;
                    ___storage[num] = new Item(0, 0, 0, 0, 0, new int[3], new int[3]);
                    typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { num2 });
                }
                else
                {
                    for (int i = 0; i < 36; i++)
                    {
                        if (___inventory[i].id == 0)
                        {
                            if (!flag2) __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                            ___inventory[i] = ___storage[num];
                            ___storage[num] = new Item(0, 0, 0, 0, 0, new int[3], new int[3]);
                            typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { i });
                            break;
                        }
                        else if (___inventory[i].id == itemInSlot.id && ___inventory[i].q < 9999)
                        {
                            if (___inventory[i].q + ___storage[num].q <= 9999)
                            {
                                if (!flag2) __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                                ___inventory[i].q += ___storage[num].q;
                                ___storage[num] = new Item(0, 0, 0, 0, 0, new int[3], new int[3]);
                                typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { i });
                                break;
                            }
                            else
                            {
                                if (!flag2) __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                                ___storage[num].q -= 9999 - ___inventory[i].q;
                                ___inventory[i].q = 9999;
                                typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { i });
                            }
                            flag2 = true;
                        }
                    }
                }
                __instance.StartCoroutine((IEnumerator)typeof(GameScript).GetMethod("RefreshStoragePage", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { ___curStoragePage }));
                ___shiftclicking = false;
                __result = FakeRoutine();
                return false;
            }
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(GameScript __instance)
        {
            if (itemInSlot != null && itemInSlot.id >= 1000 && itemInSlot.id < 2000)
            {
                int[] gearBaseStats = (int[])typeof(GameScript).GetMethod("GetGearBaseStats", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { itemInSlot.id });
                int level = (int)typeof(GameScript).GetMethod("GetItemLevel", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { itemInSlot.exp });
                for (int j = 0; j < 6; j++)
                {
                    if (gearBaseStats[j] > 0)
                    {
                        GameScript.GEARSTAT[j] += itemInSlot.tier * 3 + gearBaseStats[j] * (level - 1);
                    }
                }
            }
        }

        public static IEnumerator FakeRoutine()
        {
            yield break;
        }
    }
}