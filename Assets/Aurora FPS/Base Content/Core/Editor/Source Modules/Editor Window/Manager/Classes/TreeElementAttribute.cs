/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System;

namespace AuroraFPSEditor.Window.Manager
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TreeElementAttribute : Attribute
    {
        private string name;
        private string section;
        private int priority;

        /// <summary>
        /// TreeElementAttribute constructor.
        /// </summary>
        /// <param name="name">Element display name.</param>
        /// <param name="section">Element root section.</param>
        public TreeElementAttribute(string name, string section)
        {
            this.name = name;
            this.section = section;
            this.priority = 999;
        }

        /// <summary>
        /// TreeElementAttribute constructor.
        /// </summary>
        /// <param name="name">Element display name.</param>
        /// <param name="section">Element root section.</param>
        /// <param name="priority">Element display priority.</param>
        public TreeElementAttribute(string name, string section, int priority)
        {
            this.name = name;
            this.section = section;
            this.priority = priority;
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

        public string GetSection()
        {
            return section;
        }

        public void SetSection(string value)
        {
            section = value;
        }

        public int GetPriority()
        {
            return priority;
        }

        public void SetPriority(int value)
        {
            priority = value;
        }
        #endregion
    }
}