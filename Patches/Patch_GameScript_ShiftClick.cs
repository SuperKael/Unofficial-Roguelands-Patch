using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("ShiftClick")]
    static class Patch_GameScript_ShiftClick
    {
        private static Item itemInSlot;

        [HarmonyPrefix]
        public static bool Prefix(GameScript __instance, ref bool ___shiftclicking, Item[] ___inventory, ref Item[] ___storage, int slot, int ___curStoragePage, ref IEnumerator __result)
        {
            itemInSlot = ___inventory[slot];
            if (!___shiftclicking && (itemInSlot.id < 300 || itemInSlot.id >= 2000))
            {
                bool flag1 = false, flag2 = false;
                int num = 0;
                int num2 = ___curStoragePage * 30;
                int num3 = num2 + 30;
                for (int i = num2; i < num3; i++)
                {
                    if (___storage[i].id == itemInSlot.id && ___storage[i].q < 9999)
                    {
                        if (___storage[i].q + ___inventory[slot].q <= 9999)
                        {
                            flag1 = true;
                            num = i;
                        }
                        else
                        {
                            if (!flag2) __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                            ___inventory[slot].q -= 9999 - ___storage[i].q;
                            ___storage[i].q = 9999;
                            flag2 = true;
                        }
                        break;
                    }
                }
                if (flag1)
                {
                    __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                    ___storage[num].q += ___inventory[slot].q;
                    ___inventory[slot] = new Item(0, 0, 0, 0, 0, new int[3], new int[3]);
                }
                else
                {
                    for (int i = num2; i < num3; i++)
                    {
                        if (___storage[i].id == 0)
                        {
                            if (!flag2) __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                            ___storage[i] = ___inventory[slot];
                            ___inventory[slot] = new Item(0, 0, 0, 0, 0, new int[3], new int[3]);
                            break;
                        }
                        else if (___storage[i].id == itemInSlot.id && ___storage[i].q < 9999)
                        {
                            if (___storage[i].q + ___inventory[slot].q <= 9999)
                            {
                                if (!flag2) __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                                ___storage[i].q += ___inventory[slot].q;
                                ___inventory[slot] = new Item(0, 0, 0, 0, 0, new int[3], new int[3]);
                                break;
                            }
                            else
                            {
                                if (!flag2) __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/CLICK3"), Menuu.soundLevel / 10f);
                                ___inventory[slot].q -= 9999 - ___storage[i].q;
                                ___storage[i].q = 9999;
                            }
                            flag2 = true;
                        }
                    }
                }
                typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { slot });
                __instance.StartCoroutine((IEnumerator)typeof(GameScript).GetMethod("RefreshStoragePage", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { ___curStoragePage }));
                ___shiftclicking = false;
                __result = FakeRoutine();
                return false;
            }
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(GameScript __instance, int slot)
        {
            if (itemInSlot != null && itemInSlot.id >= 1000 && itemInSlot.id < 2000 && slot > 41)
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