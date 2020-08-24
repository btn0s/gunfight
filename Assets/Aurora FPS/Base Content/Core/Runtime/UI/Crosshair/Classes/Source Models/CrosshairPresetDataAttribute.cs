/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;

namespace AuroraFPSRuntime.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CrosshairPresetDataAttribute : Attribute
    {
        private string name;
        private bool hidden;

        public CrosshairPresetDataAttribute(string name, bool hidden)
        {
            this.name = name;
            this.hidden = hidden;
        }

        #region [Getter / Setter]
        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }

        public bool Hidden()
        {
            return hidden;
        }

        public void Hidden(bool value)
        {
            hidden = value;
        }
        #endregion
    }
}