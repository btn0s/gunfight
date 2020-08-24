/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GrenadeItem), true)]
    public class GrenadeItemEditor : WeaponItemEditor
    {
        public override void DrawGroupField()
        {
            EditorGUI.BeginDisabledGroup(true);
            base.DrawGroupField();
            EditorGUI.EndDisabledGroup();
        }
    }
}