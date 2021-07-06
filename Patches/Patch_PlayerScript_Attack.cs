using HarmonyLib;
using GadgetCore.API;
using System.Collections;
using UnityEngine;
using System.Reflection;
using System;

namespace SchegsFix.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("Attack")]
    [HarmonyGadget("URP")]
    public static class Patch_PlayerScript_Attack
    {
        static FieldInfo canAttackField = typeof(PlayerScript).GetField("canAttack", BindingFlags.NonPublic | BindingFlags.Instance);
        static FieldInfo attackingField = typeof(PlayerScript).GetField("attacking", BindingFlags.NonPublic | BindingFlags.Instance);
        static FieldInfo hyperField = typeof(PlayerScript).GetField("hyper", BindingFlags.NonPublic | BindingFlags.Instance);
        static FieldInfo curShotField = typeof(PlayerScript).GetField("curShot", BindingFlags.NonPublic | BindingFlags.Instance);

        [HarmonyPrefix]
        public static bool Prefix(PlayerScript __instance)
        {
            if ((bool)attackingField.GetValue(__instance) || !(bool)canAttackField.GetValue(__instance) || InstanceTracker.GameScript.combatSwitching || PlayerScript.beaming)
            {
                return false;
            }

            if (GameScript.equippedIDs[0] == 495) // Scheg's Bow
            {
                __instance.StartCoroutine(SchegsCoroutine(__instance));
                return false;
            }
            else
            {
                return true;
            }
        }

        private static IEnumerator SchegsCoroutine(PlayerScript instance)
        {
            int wepID = GameScript.equippedIDs[0];
            canAttackField.SetValue(instance, false);
            attackingField.SetValue(instance, true);
            instance.StartCoroutine(instance.ATKSOUND());
            instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/shoot"), Menuu.soundLevel / 10f);
            instance.Animate(4);
            yield return new WaitForSeconds(0.3f);
            int dmg = InstanceTracker.GameScript.GetFinalStat(2) + InstanceTracker.GameScript.GetFinalStat(3);
            if((bool)hyperField.GetValue(instance))
            {
                hyperField.SetValue(instance, false);
                instance.HyperBeam();
            }
            int bonusCritChance = 0;
            if (Menuu.curUniform == 1)
            {
                bonusCritChance += 5;
            }
            if (UnityEngine.Random.Range(0, 100) + GameScript.MODS[11] * 0.9f + bonusCritChance >= 95f)
            {
                instance.GetComponent<AudioSource>().PlayOneShot(instance.critSound, Menuu.soundLevel / 10f);
                float tmpdmg = (float)dmg;
                GameObject.Instantiate(instance.crit, instance.transform.position, Quaternion.identity);
                tmpdmg *= 1.5f + (float)GameScript.MODS[12] * 0.05f;
                dmg = (int)tmpdmg;
            }
            Rigidbody r = instance.GetComponent<Rigidbody>();
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > instance.transform.position.x)
            {
                r.velocity = new Vector3(-10f, r.velocity.y + 5f, 0f);
            }
            else
            {
                r.velocity = new Vector3(10f, r.velocity.y + 5f, 0f);
            }
            Vector3 targ = Camera.main.ScreenToWorldPoint(Input.mousePosition) - instance.transform.position;
            float dmgdmg = (float)(dmg / 2);

            int curShot = (int)curShotField.GetValue(instance);
            if (curShot == 0)
            {
                dmg += InstanceTracker.GameScript.GetFinalStat(4);
                Package2 package11 = new Package2(targ, (float)dmg, GameScript.equippedIDs[0], (float)GameScript.MODS[10]);
                GameObject gameObject8 = (GameObject)GameObject.Instantiate(Resources.Load("proj/schegF"), instance.shot.transform.position, Quaternion.identity);
                gameObject8.SendMessage("Set", package11);
                instance.GetComponent<NetworkView>().RPC("ShootProjectile", RPCMode.Others, new object[]
                {
                        492, // an unused id
                        targ,
                        dmg
                });
                instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/schegF"), Menuu.soundLevel / 10f);
            }
            else if (curShot == 1)
            {
                Package2 package12 = new Package2(targ, (float)dmg, GameScript.equippedIDs[0], (float)GameScript.MODS[10]);
                GameObject gameObject9 = (GameObject)GameObject.Instantiate(Resources.Load("proj/schegT"), instance.shot.transform.position, Quaternion.identity);
                gameObject9.SendMessage("Set", package12);
                instance.GetComponent<NetworkView>().RPC("ShootProjectile", RPCMode.Others, new object[]
                {
                        493, // an unused id
                        targ,
                        dmg
                });
                instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/schegT"), Menuu.soundLevel / 10f);
            }
            else if (curShot == 2)
            {
                instance.StartCoroutine(SchegI(dmg, instance));
            }

            if(curShot < 2)
                curShotField.SetValue(instance, curShot + 1);
            else
                curShotField.SetValue(instance, 0);

            yield return new WaitForSeconds(0.3f);


            attackingField.SetValue(instance, false);
            yield return new WaitForSeconds(0.1f);
            canAttackField.SetValue(instance, true);

            yield break;
        }

        private static IEnumerator SchegI(int dmg, PlayerScript instance)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 targ = Camera.main.ScreenToWorldPoint(Input.mousePosition) - instance.transform.position;
                Package2 pack = new Package2(targ, (float)dmg, GameScript.equippedIDs[0], (float)GameScript.MODS[10]);
                GameObject proj = (GameObject)GameObject.Instantiate(Resources.Load("proj/schegI"), instance.shot.transform.position, Quaternion.identity);
                proj.SendMessage("Set", pack);
                instance.GetComponent<NetworkView>().RPC("ShootProjectile", RPCMode.Others, new object[]
                {
                    494, // an unused id
                    targ,
                    dmg
                });
                instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/schegI"), Menuu.soundLevel / 10f);
                if(i < 2) // only wait between shots, not after the last one (doesn't really matter since it's the end of the coroutine anyway)
                    yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }
    }
}