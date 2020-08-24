/* ==================================================================
   ---------------------------------------------------
   Project   :    #0001
   Publisher :    #0002
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEditor;

namespace AuroraFPSEditor
{
    public static class HelpBoxMessages
    {
        public static void Message(string message, MessageType messageType = MessageType.Info, bool fixIndentLevel = false)
        {
            if (fixIndentLevel)
                EditorGUI.indentLevel--;
            EditorGUILayout.HelpBox(message, messageType);
            if (fixIndentLevel)
                EditorGUI.indentLevel++;
        }

        public static void Error(string message, string solution, bool fixIndentLevel = false)
        {
            if (fixIndentLevel)
                EditorGUI.indentLevel--;
            EditorGUILayout.HelpBox("Message: " + message + "\nSolution: " + solution, MessageType.Error);
            if (fixIndentLevel)
                EditorGUI.indentLevel++;
        }

        public static void Tip(string message, string tip, bool fixIndentLevel = false)
        {
            if (fixIndentLevel)
                EditorGUI.indentLevel--;
            EditorGUILayout.HelpBox("Message: " + message + "\nTip: " + tip, MessageType.Warning);
            if (fixIndentLevel)
                EditorGUI.indentLevel++;
        }

        public static void PlayerError(bool fixIndentLevel = false)
        {
            Error("Player is empty, add Player transform!", "Hierarchy window [Player] or try to use the automatic search button.", fixIndentLevel);
        }

        public static void CameraError(bool fixIndentLevel = false)
        {
            Error("Player Camera is empty, add Camera component!", "Hierarchy window [Player > FPCamera] or try to use the automatic search button.", fixIndentLevel);
        }
    }
}