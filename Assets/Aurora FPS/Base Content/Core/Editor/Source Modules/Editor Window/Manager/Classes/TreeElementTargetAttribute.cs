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
    public class TreeElementTargetAttribute : Attribute
    {
        private Type target;

        /// <summary>
        /// TreeElementTargetAttribute constructor.
        /// </summary>
        /// <param name="target">Target manager window type.</param>
        public TreeElementTargetAttribute(Type target)
        {
            this.target = target;
        }

        #region [Getter / Setter]
        /// <summary>
        /// Target manager window type.
        /// </summary>
        public Type GetTarget()
        {
            return target;
        }
        #endregion
    }
}