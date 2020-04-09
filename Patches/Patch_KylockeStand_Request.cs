using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using GadgetCore.API;

namespace URP.Patches
{
    [HarmonyPatch(typeof(KylockeStand))]
    [HarmonyPatch("Request")]
    [HarmonyGadget("URP")]
    public static class Patch_KylockeStand_Request
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            List<CodeInstruction> newCodes = new List<CodeInstruction>();
            CodeInstruction lastInsn = null;
            int index = -1;
            for (int i = 0;i < codes.Count;i++)
            {
                if (lastInsn != null && codes[i].opcode == OpCodes.Ldc_I4_S && ((sbyte)codes[i].operand) == 10 && lastInsn.opcode == OpCodes.Ldfld)
                {
                    index = i;
                    break;
                }
                else
                {
                    newCodes.Add(lastInsn);
                    lastInsn = codes[i];
                }
            }
            if (index > 0)
            {
                newCodes.Add(new CodeInstruction(OpCodes.Call, typeof(Patch_KylockeStand_Request).GetMethod("ReportKylockeError", BindingFlags.Public | BindingFlags.Static)));
                for (int i = 0; i < 7; i++) newCodes.Add(new CodeInstruction(OpCodes.Nop));
            }
            for (int i = index + 2;i < codes.Count;i++)
            {
                newCodes.Add(codes[i]);
            }
            return newCodes.Where(x => x != null);
        }

        public static void ReportKylockeError(KylockeStand stand)
        {
            if (stand.isCredits)
            {
                if (stand.isTrophies)
                {
                    GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("txtError"), MenuScript.player.transform.position, Quaternion.identity);
                    gameObject.SendMessage("InitError", "Insufficient Wealth Trophies!");
                }
                else
                {
                    GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("txtError"), MenuScript.player.transform.position, Quaternion.identity);
                    gameObject.SendMessage("InitError", "Insufficient Credits!");
                }
            }
            else
            {
                GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("txtError"), MenuScript.player.transform.position, Quaternion.identity);
                gameObject.SendMessage("InitError", "Insufficient Scrap Metal!");
            }
        }
    }
}