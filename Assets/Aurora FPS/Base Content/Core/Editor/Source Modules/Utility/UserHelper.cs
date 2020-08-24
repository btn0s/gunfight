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
    // [CreateAssetMenu(fileName = "User Helper", menuName = AuroraEditorPaths.MenuItemEditor + "Internal/User Helper", order = 127)]
    public class UserHelper : ScriptableObject
    {
        public const string OfficialEmail = "infinitedawnts@gmail.com";
        public const string OnlineDocumentationURL = "https://aurorafpsdocumentation.github.io";
        public const string OfflineDocumentationURL = EditorPaths.BaseContent + "/Documentation/Aurora FPS Documentation.pdf";
        public const string OnlineAPI = "https://aurorafpsapi.github.io";
        public const string OfficialThreadURL = "https://forum.unity.com/threads/aurora-fps.858154/";
        public const string OfficialDiscordChannelURL = "https://discord.gg/9gZDbhb";
        public const string OfficialTwitterURL = "https://twitter.com/InfiniteDawnTS";

        public static void OpenDocumentation()
        {
            if (NetworkIsAvailable() && DisplayDialogs.Message("Documentation", "There is Internet access, you can open the online version of the documentation. We recommend using the online version as it is updated in real time.", "Online", "Offline"))
                OpenOnlineDocumentation();
            else
                OpenOfflineDocumenttation();
        }

        public static void OpenAPI()
        {
            if (NetworkIsAvailable() && DisplayDialogs.Message("API", "There is Internet access, you can open the online version of the API. We recommend using the online version as it is updated in real time.", "Online", "Offline"))
                OpenOnlineAPI();
            else
                OpenOfflineDocumenttation();
        }

        public static void OpenOfflineDocumenttation()
        {
            string localPath = "file:///" + Application.dataPath;
            localPath = localPath.Replace("/Assets", "/");
            localPath += OfflineDocumentationURL;
            localPath = System.Uri.EscapeUriString(localPath);
            Application.OpenURL(localPath);
        }

        public static void OpenOnlineDocumentation()
        {
            Application.OpenURL(OnlineDocumentationURL);
        }

        public static void OpenOnlineAPI()
        {
            Application.OpenURL(OnlineAPI);
        }

        public static void OpenOfficialThread()
        {
            Application.OpenURL(OfficialThreadURL);
        }

        public static void OpenDiscordChannel()
        {
            Application.OpenURL(OfficialDiscordChannelURL);
        }

        public static void OpenTwitter()
        {
            Application.OpenURL(OfficialTwitterURL);
        }

        public static bool NetworkIsAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}