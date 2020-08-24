/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace AuroraFPSEditor
{
    public static class EditorResourcesHelper
    {
        public const string PropertiesPath = "Editor/Properties/";
        public const string PropertiesIconsPath = "Editor/Icons/Properties/";
        public const string OtherIconsPath = "Editor/Icons/Other/";

        public static Texture2D GetPropertiesIcon(string iconName)
        {
            return (Texture2D) Resources.Load(PropertiesIconsPath + iconName) as Texture2D;
        }

         public static Texture2D GetOtherIcon(string iconName)
        {
            return (Texture2D) Resources.Load(OtherIconsPath + iconName) as Texture2D;
        }
    }
}