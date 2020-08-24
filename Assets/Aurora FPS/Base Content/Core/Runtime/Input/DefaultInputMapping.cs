/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using AuroraFPSRuntime.Serialization.Collections;

namespace AuroraFPSRuntime
{
    [CreateAssetMenu(fileName = "Default Input Mapping", menuName = AuroraFPSProduct.Name + "/Mapping/Input/Default", order = 121)]
    public class DefaultInputMapping : ScriptableMapping
    {
        [SerializeField] private DictionaryStringToString axes = new DictionaryStringToString();
        [SerializeField] private DictionaryStringToKeyCode buttons = new DictionaryStringToKeyCode();

        public DefaultInputMapping()
        {
            axes = new DictionaryStringToString()
            {
                [INC.CharHorizontal] = "Horizontal",
                [INC.CharVertical] = "Vertical",
                [INC.CamHorizontal] = "Mouse X",
                [INC.CamVertical] = "Mouse Y",
                [INC.MouseWheel] = "Mouse ScrollWheel"
            };

            buttons = new DictionaryStringToKeyCode()
            {
                [INC.Sprint] = KeyCode.LeftShift,
                [INC.LightWalk] = KeyCode.LeftAlt,
                [INC.Jump] = KeyCode.Space,
                [INC.Crouch] = KeyCode.LeftControl,
                [INC.Attack] = KeyCode.Mouse0,
                [INC.Zoom] = KeyCode.Mouse1,
                [INC.ChangeFireMode] = KeyCode.V,
                [INC.Reload] = KeyCode.R,
                [INC.Grab] = KeyCode.F,
                [INC.Drop] = KeyCode.G,
                [INC.LeftTilt] = KeyCode.Q,
                [INC.RightTilt] = KeyCode.E
            };
        }

        public void AddAxis(string key, string value)
        {
            axes.Add(key, value);
        }

        public bool RemoveAxis(string key)
        {
            return axes.Remove(key);
        }

        public bool ContainAxis(string key)
        {
            return axes.ContainsKey(key);
        }

        public string GetAxis(string key)
        {
            return axes[key];
        }

        public bool TryGetAxis(string key, out string value)
        {
            return axes.TryGetValue(key, out value);
        }

        public DictionaryStringToString GetAxes()
        {
            return axes;
        }

        public void SetAxis(string key, string value)
        {
            axes[key] = value;
        }

        public void SetAxes(DictionaryStringToString value)
        {
            axes = value;
        }

        public void AddButtons(string key, KeyCode value)
        {
            buttons.Add(key, value);
        }

        public bool RemoveButton(string key)
        {
            return buttons.Remove(key);
        }

        public bool ContainButton(string key)
        {
            return buttons.ContainsKey(key);
        }

        public KeyCode GetButton(string key)
        {
            return buttons[key];
        }

        public bool TryGetButton(string key, out KeyCode value)
        {
            return buttons.TryGetValue(key, out value);
        }

        public DictionaryStringToKeyCode GetButtons()
        {
            return buttons;
        }

        public void SetButton(string key, KeyCode value)
        {
            buttons[key] = value;
        }

        public void SetButtons(DictionaryStringToKeyCode value)
        {
            buttons = value;
        }

        public override int GetMappingLength()
        {
            return axes?.Count ?? 0 + buttons?.Count ?? 0;
        }
    }
}