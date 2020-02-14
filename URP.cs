using UnityEngine;
using UModFramework.API;
using UnityEngine.SceneManagement;
using System.IO;
using System.Reflection;
using UModFramework;
using System.Collections;
using System;
using Ionic.Zip;
using System.Linq;
using System.Collections.Generic;

namespace URP
{
    class URP
    {
        public static Texture2D recipesStaminaMiscFixed;
        public static Material r1t1Fixed;

        internal static Dictionary<string, Texture2D> cachedTexes = new Dictionary<string, Texture2D>();
        internal static object GadgetCoreLib;

        internal static void Log(string text, bool clean = false)
        {
            using (UMFLog log = new UMFLog()) log.Log(text, clean);
        }

        [UMFConfig]
        public static void LoadConfig()
        {
            URPConfig.Load();
        }

		[UMFHarmony(22)] //Set this to the number of harmony patches in your mod.
        public static void Start()
		{
			Log("Unofficial Roguelands Patch v" + UMFMod.GetModVersion().ToString(), true);
            if (File.Exists(Path.Combine(UMFData.LibrariesPath, "GadgetCoreLib.dll")))
            {
                try
                {
                    GadgetCoreLib = Activator.CreateInstance(Assembly.LoadFile(Path.Combine(UMFData.LibrariesPath, "GadgetCoreLib.dll")).GetType("GadgetCore.Lib.GadgetCoreLib"));
                }
                catch
                {
                    GadgetCoreLib = null;
                }
            }
            Texture2D recipesSTAMINAMISC_FixedTex = LoadTexture2D("recipesSTAMINAMISC_Fixed.png");
            if (recipesSTAMINAMISC_FixedTex != null)
            {
                recipesStaminaMiscFixed = LoadTexture2D("recipesSTAMINAMISC_Fixed.png");
                recipesStaminaMiscFixed.filterMode = FilterMode.Point;
                r1t1Fixed = Resources.Load<Material>("mat/r1t1");
                r1t1Fixed.mainTexture = recipesStaminaMiscFixed;
            }
            else
            {
                if (GadgetCoreLib == null)
                {
                    Log("Nuldmg Quick-Craft Texture Fix FAILED: GadgetCoreLib Required!");
                }
                else
                {
                    Log("Nuldmg Quick-Craft Texture Fix FAILED");
                }
            }
        }

        internal static Texture2D LoadTexture2D(string file, bool shared = false)
        {
            string modName = shared ? "Shared" : Assembly.GetCallingAssembly().GetName().Name;
            string filePath = Path.Combine(Path.Combine(UMFData.AssetsPath, modName), file);
            if (cachedTexes.ContainsKey(filePath))
            {
                return cachedTexes[filePath];
            }
            string assetPath = Path.Combine(Path.Combine("Assets", modName), file);
            if (cachedTexes.ContainsKey(Path.Combine(UMFData.TempPath, assetPath)))
            {
                return cachedTexes[Path.Combine(UMFData.TempPath, assetPath)];
            }
            if (!File.Exists(filePath) && GadgetCoreLib != null)
            {
                filePath = Path.Combine(UMFData.TempPath, assetPath);
                string[] umfmods = Directory.GetFiles(UMFData.ModsPath, (shared ? "" : modName) + "*.umfmod");
                string[] zipmods = Directory.GetFiles(UMFData.ModsPath, (shared ? "" : modName) + "*.zip");
                string[] mods = new string[umfmods.Length + zipmods.Length];
                Array.Copy(umfmods, mods, umfmods.Length);
                Array.Copy(zipmods, 0, mods, umfmods.Length, zipmods.Length);
                foreach (string mod in mods)
                {
                    using (ZipFile modZip = new ZipFile(mod))
                    {
                        if (mod.EndsWith(".umfmod")) GadgetCoreLib.GetType().GetMethod("DecryptUMFModFile", BindingFlags.Public | BindingFlags.Instance).Invoke(GadgetCoreLib, new object[] { modZip });
                        if (modZip.ContainsEntry(assetPath))
                        {
                            modZip[assetPath].Extract(UMFData.TempPath, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                }
            }
            if (File.Exists(filePath))
            {
                byte[] fileData;
                fileData = File.ReadAllBytes(filePath);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
                tex.filterMode = FilterMode.Point;
                cachedTexes.Add(filePath, tex);
                if (filePath.StartsWith(UMFData.TempPath))
                {
                    File.Delete(filePath);
                    RecursivelyDeleteDirectory(UMFData.TempPath);
                }
                return tex;
            }
            else
            {
                return File.Exists(Path.Combine(UMFData.ManagedPath, "System.Drawing.dll")) ? UMFAsset.LoadTexture2D(file, shared) : null;
            }
        }

        internal static void RecursivelyDeleteDirectory(string path, bool deleteFiles = false)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                RecursivelyDeleteDirectory(directory);
            }

            if (deleteFiles || (Directory.GetFiles(path).Length == 0 && Directory.GetDirectories(path).Length == 0))
            {
                try
                {
                    Directory.Delete(path, deleteFiles);
                }
                catch (IOException)
                {
                    Directory.Delete(path, deleteFiles);
                }
                catch (UnauthorizedAccessException)
                {
                    Directory.Delete(path, deleteFiles);
                }
            }
        }
    }
}