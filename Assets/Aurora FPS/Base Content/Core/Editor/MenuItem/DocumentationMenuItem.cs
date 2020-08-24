/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEditor;

namespace AuroraFPSEditor
{
    public class DocumentationMenuItem
    {
        [MenuItem(EditorPaths.MenuItem + "Documentation", false, 1)]
        public static void OpenDocumentation()
        {
            UserHelper.OpenDocumentation();
        }

         [MenuItem(EditorPaths.MenuItem + "API", false, 2)]
        public static void OpenAPI()
        {
            UserHelper.OpenAPI();
        }
    }
}