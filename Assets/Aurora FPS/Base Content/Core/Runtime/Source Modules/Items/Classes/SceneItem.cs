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
    public abstract class SceneItem : BaseItem, ISceneItem
    {
        // Base scene item properties.
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private Sprite sprite;
        [SerializeField] private DropObject dropObject;

        /// <summary>
        /// SceneItem constructor.
        /// </summary>
        public SceneItem()
        {
            this.displayName = "Item";
            this.description = "Description of item.";
            this.sprite = null;
            this.dropObject = DropObject.none;
        }

        #region [Getter / Setter]
        public string GetDisplayName()
        {
            return displayName;
        }

        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }

        public string GetDescription()
        {
            return description;
        }

        public void SetDescription(string description)
        {
            this.description = description;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public void SetSprite(Sprite value)
        {
            sprite = value;
        }

        public DropObject GetDropObject()
        {
            return dropObject;
        }

        public void SetDropObject(DropObject value)
        {
            dropObject = value;
        }
        #endregion
    }
}