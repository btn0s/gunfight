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
    [CustomEditor(typeof(WeaponGrenadeSystem))]
    public class WeaponGrenadeSystemEditor : AuroraEditor<WeaponGrenadeSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent Grenade = new GUIContent("Grenade", "Grenade prefab/gameobject instance.");
            public readonly static GUIContent ThrowPoint = new GUIContent("Throw Point", "Throw point transform.");
            public readonly static GUIContent Force = new GUIContent("Force", "Throw force.");
            public readonly static GUIContent ThrowRate = new GUIContent("Rate", "Number of throws per second.");

            public readonly static GUIContent AnimationProperties = new GUIContent("Animation Properties");
            public readonly static GUIContent TimeToThrow = new GUIContent("Time To Throw", "Time from animation when grenade have to throw.");
            public readonly static GUIContent ThrowState = new GUIContent("State", "Throw animation state from animatior controller.");
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetGrenade(AEditorGUILayout.ObjectField(ContentProperties.Grenade, instance.GetGrenade(), true));
            instance.SetThrowPoint(AEditorGUILayout.ObjectField(ContentProperties.ThrowPoint, instance.GetThrowPoint(), true));
            instance.SetForce(AEditorGUILayout.FixedFloatField(ContentProperties.Force, instance.GetForce(), 0));
            instance.SetThrowRate(AEditorGUILayout.FixedFloatField(ContentProperties.ThrowRate, instance.GetThrowRate(), instance.GetTimeToThrow()));
            EndGroup();

            BeginGroup(ContentProperties.AnimationProperties);
            instance.SetTimeToThrow(AEditorGUILayout.FixedFloatField(ContentProperties.TimeToThrow, instance.GetTimeToThrow(), 0));

            AnimatorValue throwState = instance.GetThrowState();
            throwState.SetName(EditorGUILayout.TextField(ContentProperties.ThrowState, throwState.GetName()));
            instance.SetThrowState(throwState);
            EndGroup();
        }
    }
}