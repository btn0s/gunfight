/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    public abstract class BaseItem : ScriptableObject, IBaseItem
    {
        // Base item properties.
        [SerializeField] private string id;

        /// <summary>
        /// BaseItem constructor.
        /// </summary>
        public BaseItem()
        {
            id = AuroraExtension.GenerateID(32);
        }

        #region [Getter / Setter]
        public string GetID()
        {
            return id;
        }

        public void SetID(string id)
        {
            this.id = id;
        }
        #endregion
    }
}

