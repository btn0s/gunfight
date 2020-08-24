/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

namespace AuroraFPSRuntime
{
    public abstract class CameraSwitcher : ICameraSwitcher
    {
        // Stored required properties.
        protected HealthComponent healthComponent;

        /// <summary>
        /// Initialize death camera system instance.
        /// Called inside Awake() body method of health system component
        /// </summary>
        public virtual void Initialize(HealthComponent healthComponent)
        {
            this.healthComponent = healthComponent;
        }

        #region [ICameraSwitcher Implementation]
        /// <summary>
        /// Called when camera switching.
        /// </summary>
        public abstract void OnSwitch();
        #endregion

        #region [Getter / Setter]
        public HealthComponent GetHealthComponent()
        {
            return healthComponent;
        }

        public void SetHealthComponent(HealthComponent value)
        {
            healthComponent = value;
        }
        #endregion
    }
}

