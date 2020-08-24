/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;

namespace AuroraFPSRuntime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CameraSwitcherSettingsAttribute : Attribute
    {
        private string path;
        private int priority;
        private bool hidden;


        public CameraSwitcherSettingsAttribute(string path)
        {
            this.path = path;
            this.priority = 0;
            this.hidden = false;
        }

        public CameraSwitcherSettingsAttribute(string path, int priority)
        {
            this.path = path;
            this.priority = priority;
            this.hidden = false;
        }

        public CameraSwitcherSettingsAttribute(string path, int priority, bool hidden)
        {
            this.path = path;
            this.priority = priority;
            this.hidden = hidden;
        }

        #region [Getter / Setter]
        public string GetPath()
        {
            return path;
        }

        public void SetPath(string value)
        {
            path = value;
        }

        public int GetPriority()
        {
            return priority;
        }

        public void SetPriority(int value)
        {
            priority = value;
        }

        public bool GetHidden()
        {
            return hidden;
        }

        public void SetHidden(bool value)
        {
            hidden = value;
        }
        #endregion
    }
}
