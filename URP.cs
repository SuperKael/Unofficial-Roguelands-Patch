using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using GadgetCore.API;

namespace URP
{
    [Gadget("URP")]
    public class URP : Gadget
    {
        public static Texture2D recipesStaminaMiscFixed;
        public static Material r1t1Fixed;

        internal static Dictionary<string, Texture2D> cachedTexes = new Dictionary<string, Texture2D>();

        protected override void Initialize()
		{
			Logger.Log("Unofficial Roguelands Patch v" + Info.Mod.Version);
            recipesStaminaMiscFixed = GadgetCoreAPI.LoadTexture2D("recipesSTAMINAMISC_Fixed.png");
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