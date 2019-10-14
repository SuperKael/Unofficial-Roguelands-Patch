using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;

namespace URP.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("TrashItem")]
    static class Patch_GameScript_TrashItem
    {
#pragma warning disable CS0618 // Type or member is obsolete
        [HarmonyPrefix]
        public static bool Prefix(GameScript __instance, int slot, ref Item[] ___inventory, ref bool ___trashAgain, ref bool ___trashing)
        {
            __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/emblem"), Menuu.soundLevel / 10f);
            int q = ___inventory[slot].q;
            int value = (int)typeof(GameScript).GetMethod("GetItemWorth", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { ___inventory[slot].id });
            if (value * q < 10000)
            {
                ___inventory[slot] = new Item(52, q * value, 0, 0, 0, new int[3], new int[3]);
                typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { slot });
            }
            else
            {
                int sellsPerStack = 9999 / value;
                int leftoverValue = 9999 - (sellsPerStack * value);
                int extraValue = 0;
                List<int> availableSlots = new List<int>();
                int slotIndex = 0;
                int availableValueSpace = 9999;
                for (int i = slot == 35 ? 0 : slot + 1;i != slot;i = i == 35 ? 0 : i + 1)
                {
                    if (___inventory[i].id == 0)
                    {
                        availableSlots.Add(i);
                        availableValueSpace += 9999;
                    }
                    else if (___inventory[i].id == 52 && ___inventory[i].q < 9999)
                    {
                        availableSlots.Add(i);
                        availableValueSpace += 9999 - ___inventory[i].q;
                    }
                    if (availableValueSpace >= q * value) break;
                }
                int qToRemain = 0;
                if (availableValueSpace < q * value)
                {
                    availableValueSpace -= 9999;
                    if (availableValueSpace < value)
                    {
                        ErrorInvSpace();
                        ___trashing = false;
                        __instance.bbTrasher.GetComponent<Animation>().Stop();
                        __instance.cursorTrasher.SetActive(false);
                        Cursor.visible = true;
                        __instance.loopBox.SendMessage("Cease");
                        return false;
                    }
                    else
                    {
                        qToRemain = q - (availableValueSpace / value);
                        ___inventory[slot].q = qToRemain;
                        slot = availableSlots[slotIndex];
                        slotIndex++;
                        typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { slot });
                    }
                }
                else
                {
                    ___inventory[slot] = new Item(0, 0, 0, 0, 0, new int[3], new int[3]);
                }
                while (q > qToRemain || extraValue > 0)
                {
                    int desiredSells = extraValue + ___inventory[slot].q > leftoverValue ? sellsPerStack : (sellsPerStack + 1);
                    if (desiredSells <= q - qToRemain)
                    {
                        extraValue = extraValue + ___inventory[slot].q + (desiredSells * value) - 9999;
                        q -= desiredSells;
                        ___inventory[slot] = new Item(52, 9999, 0, 0, 0, new int[3], new int[3]);
                    }
                    else
                    {
                        extraValue = extraValue + ___inventory[slot].q + (q * value) - 9999;
                        q = 0;
                        if (extraValue < 0)
                        {
                            ___inventory[slot] = new Item(52, 9999 + extraValue, 0, 0, 0, new int[3], new int[3]);
                            extraValue = 0;
                        }
                        else
                        {
                            ___inventory[slot] = new Item(52, 9999, 0, 0, 0, new int[3], new int[3]);
                        }
                    }
                    typeof(GameScript).GetMethod("RefreshSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { slot });
                    slot = availableSlots[slotIndex];
                    slotIndex++;
                }
            }
            ___trashAgain = true;
            __instance.bbTrasher.GetComponent<Animation>().Play();
            __instance.cursorTrasher.SetActive(true);
            Cursor.visible = false;
            __instance.loopBox.gameObject.SendMessage("PlayLoops", 1);
            typeof(GameScript).GetMethod("RefreshHoldingSlot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] {});
            return false;
        }
#pragma warning restore CS0618

        public static void ErrorInvSpace()
        {
            Vector3 position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("txtError"), position, Quaternion.identity);
            gameObject.SendMessage("InitError", "Not enough inventory space!");
        }
    }
}