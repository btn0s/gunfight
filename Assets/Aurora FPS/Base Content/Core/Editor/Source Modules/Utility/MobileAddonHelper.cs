/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace AuroraFPSEditor
{
    public static class MobileAddonHelper
    {
        public const string ResourcesName = "Resources.unitypackage";
        public const string MobileNamespace = "AuroraFPSRuntime.Mobile";

        public static bool AddonIsInstalled()
        {
            bool mobileDirectoryExist = Directory.Exists(EditorPaths.MobileContent);
            return mobileDirectoryExist;
        }

        public static bool ResourcesIsAvailable()
        {
            string path = Path.Combine(EditorPaths.MobileAddon, ResourcesName);
            bool exist = File.Exists(path);
            if (!exist)
            {
                return false;
            }
            return true;
        }

        public static bool InstallResources()
        {
            string path = Path.Combine(EditorPaths.MobileAddon, ResourcesName);
            bool exist = File.Exists(path);
            if (!exist)
            {
                return false;
            }
            AssetDatabase.ImportPackage(path, false);
            AssetDatabase.Refresh();
            return true;
        }

        public static bool InstallVersionByName(string version)
        {
            string path = Path.Combine(EditorPaths.MobileAddon, version + ".unitypackage");
            bool exist = File.Exists(path);
            if (!exist)
            {
                return false;
            }
            AssetDatabase.ImportPackage(path, false);
            AssetDatabase.Refresh();
            return true;
        }

        public static bool InstallVersionByPath(string path)
        {
            bool exist = File.Exists(path);
            if (!exist)
            {
                return false;
            }
            AssetDatabase.ImportPackage(path, false);
            AssetDatabase.Refresh();
            return true;
        }

        public static bool RemoveCurrentVersion()
        {
            bool exist = Directory.Exists(EditorPaths.MobileContent);
            if (!exist)
            {
                return false;
            }
            Directory.Delete(EditorPaths.MobileContent, true);
            AssetDatabase.Refresh();
            return true;
        }

        public static string GetInstalledVersion()
        {
            string path = Path.Combine(EditorPaths.MobileContent, "Core", "Version.txt");
            bool exist = File.Exists(path);
            bool directoryExist = Directory.Exists(EditorPaths.MobileContent);
            if (!directoryExist)
            {
                return "No installed version.";
            }
            string version = exist ? File.ReadAllText(path) : "";
            if (string.IsNullOrEmpty(version))
            {
                return "Unknown.";
            }
            return version;
        }

        public static string[] GetAllVersionPaths()
        {
            List<string> packages = new List<string>();
            string[] files = Directory.GetFiles(EditorPaths.MobileAddon);
            for (int i = 0, length = files.Length; i < length; i++)
            {
                string file = files[i];
                string fileName = Path.GetFileName(file);
                if (Path.GetExtension(file) == ".unitypackage" && fileName[0] == 'v')
                {
                    packages.Add(file);
                }
            }
            return packages.ToArray();
        }

        public static string[] GetAllVersions()
        {
            List<string> packages = new List<string>();
            if (!Directory.Exists(EditorPaths.MobileAddon))
            {
                return new string[1] { "None" };
            }

            string[] files = Directory.GetFiles(EditorPaths.MobileAddon);
            for (int i = 0, length = files.Length; i < length; i++)
            {
                string file = files[i];
                string fileName = Path.GetFileName(file);
                if (Path.GetExtension(file) == ".unitypackage" && fileName[0] == 'v')
                {
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                    packages.Add(fileName);
                }
            }
            return packages.ToArray();
        }

        public static bool IsUpdateCapability(float updateVersion)
        {
            string[] installedVersion = Regex.Split(GetInstalledVersion(), @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").ToArray();
            if (installedVersion.Length == 0)
            {
                return false;
            }
            float installedVersionFloat = float.Parse(installedVersion[0], CultureInfo.InvariantCulture.NumberFormat);
            return installedVersionFloat < updateVersion;
        }

        public static bool IsUpdateCapability(string version)
        {
            string[] installedVersion = Regex.Split(GetInstalledVersion(), @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").ToArray();
            string[] updateVersion = Regex.Split(version, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").ToArray();
            if (installedVersion.Length == 0 || updateVersion.Length == 0)
            {
                return false;
            }
            float installedVersionFloat = float.Parse(installedVersion[0], CultureInfo.InvariantCulture.NumberFormat);
            float updateVersionFloat = float.Parse(updateVersion[0], CultureInfo.InvariantCulture.NumberFormat);
            return installedVersionFloat < updateVersionFloat;
        }
    }
}