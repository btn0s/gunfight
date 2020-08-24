/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.UI
{
    [System.Serializable]
    public struct CrosshairState
    {
        [SerializeField] private ControllerState state;
        [SerializeField] private CrosshairSpread crosshairSpread;

        public CrosshairState(ControllerState state, CrosshairSpread crosshairSpread)
        {
            this.state = state;
            this.crosshairSpread = crosshairSpread;
        }

        #region [Default Values]
        public readonly static CrosshairState none = new CrosshairState(ControllerState.None, CrosshairSpread.none);
        #endregion

        #region [Getter / Setter]
        public ControllerState GetState()
        {
            return state;
        }

        public void SetState(ControllerState value)
        {
            state = value;
        }

        public CrosshairSpread GetCrosshairSpread()
        {
            return crosshairSpread;
        }

        public void SetCrosshairSpread(CrosshairSpread value)
        {
            crosshairSpread = value;
        }
        #endregion
    }
}
