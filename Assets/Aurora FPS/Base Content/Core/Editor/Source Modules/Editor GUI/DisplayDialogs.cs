/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEditor;
using AuroraFPSRuntime;

namespace AuroraFPSEditor
{
    public static class DisplayDialogs
    {
        /// <summary>
        /// Aurora FPS dialog message.
        /// </summary>
        /// <param name="message"></param>
        public static bool Message(string title, string message, string ok = "Ok", string cancel = null)
        {
            return EditorUtility.DisplayDialog(AuroraFPSProduct.Name + ": " + title, message, ok, cancel);
        }

        /// <summary>
        /// Aurora FPS dialog complex message.
        /// </summary>
        /// <param name="message"></param>
        public static int MessageComplex(string title, string message, string ok = "Ok", string cancel = "Cancel", string alt = "Other")
        {
            return EditorUtility.DisplayDialogComplex(AuroraFPSProduct.Name + ": " + title, message, ok, cancel, alt);
        }

        /// <summary>
        /// Confirmation dialog message.
        /// </summary>
        /// <param name="message"></param>
        public static bool Confirmation(string message, string ok = "Yes", string cancel = "No")
        {
            return EditorUtility.DisplayDialog(AuroraFPSProduct.Name + ": Confirmation", message, ok, cancel);
        }

        /// <summary>
        /// Project not fully configured.
        /// </summary>
        /// <param name="itemName"></param>
        public static int ProjectNotConfigured()
        {
            return EditorUtility.DisplayDialogComplex(AuroraFPSProduct.Name + ": Project Verifier", "Configure of the project is not finished!\nConfigure the project via Setup assistant.", "Open assistant", "Ok", "Don't show again");
        }

        /// <summary>
        /// Error create some item from MenuItem menu.
        /// </summary>
        /// <param name="itemName"></param>
        public static bool ErrorCreateItemPropNull(string itemName)
        {
            return EditorUtility.DisplayDialog(AuroraFPSProduct.Name + ": Create " + itemName + " Error", "Message: " + itemName + " cannot be created...\n\n" +
                "Reason: Menu Items Properties asset not found.\n\n" +
                "Solution: Create Menu Items Properties asset from\n" + EditorPaths.MenuItemEditor + "Menu Items Properties in Resources/" + EditorResourcesHelper.PropertiesPath +" folder.", "Ok");
        }

        /// <summary>
        /// Error create some item from MenuItem menu.
        /// </summary>
        /// <param name="itemName"></param>
        public static bool ErrorCreateItemObjNull(string itemName)
        {
            return EditorUtility.DisplayDialog(AuroraFPSProduct.Name + ": Create " + itemName + " Error", "Message: " + itemName + " cannot be created...\n\n" +
                "Reason: " + itemName + " object not found.\n\n" +
                "Solution: Go to Aurora FPS > Manager\nand fill " + itemName + " GameObject.", "Ok");
        }
    }
}