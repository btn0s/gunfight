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
    public class RespawnLogicMenuAttribute : Attribute
    {
        private string path;

        public RespawnLogicMenuAttribute(string path)
        {
            this.path = path;
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
        #endregion
    }
}