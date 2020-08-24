/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.Events;

namespace AuroraFPSRuntime.UI
{
    public partial class Crosshair : MonoBehaviour
    {
        public enum SpreadUpdateFunction
        {
            Static,
            SmoothStep,
            Lerp,
            MoveTowerds
        }

        // Base crosshair properties.
        [Header("Base Properties")]
        [SerializeReference] private CrosshairPreset crosshairPreset;
        [SerializeField] private CrosshairState[] crosshairStates;

        [Header("Fire Settings")]
        [SerializeField] private CrosshairSpread fireSpread;

        [Header("Hit Settings")]
        [SerializeReference] private CrosshairPreset hitPreset;
        [SerializeField] private CrosshairSpread hitSpread;
        [SerializeField] private float hitHideValue;

        [Header("Kill Settings")]
        [SerializeReference] private CrosshairPreset killPreset;
        [SerializeField] private CrosshairSpread killSpread;
        [SerializeField] private float killHideValue;

        [Header("Other Settings")]
        [SerializeField] private SpreadUpdateFunction spreadUpdateFunction = SpreadUpdateFunction.SmoothStep;
        [SerializeField] private float spread;
        [SerializeField] private bool visible = true;

        // Stored required properties.
        private FPController controller;

        // Stored required properties.
        private float storedHitSpread;
        private float storedKillSpread;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            controller = transform.root.GetComponent<FPCController>();
            crosshairPreset?.Initialize(controller);
            hitPreset?.Initialize(controller);
            killPreset?.Initialize(controller);
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        protected virtual void OnGUI()
        {
            if (visible)
            {
                OnBasePresetGUI();
                OnHitPresetGUI();
                OnKillPresetGUI();
            }
        }

        /// <summary>
        /// Main crosshair GUI processing.
        /// </summary>
        protected virtual void OnBasePresetGUI()
        {
            crosshairPreset.DrawElementsLayout(GetSpreadValue());
        }

        protected virtual void OnHitPresetGUI()
        {
            if(storedHitSpread >= hitHideValue)
            {
                storedHitSpread = Mathf.SmoothStep(storedHitSpread, 0, hitSpread.GetSpeed() * Time.deltaTime);
                hitPreset.DrawElementsLayout(storedHitSpread);
            }
        }

        protected virtual void OnKillPresetGUI()
        {
            if (storedKillSpread >= killHideValue)
            {
                storedKillSpread = Mathf.SmoothStep(storedKillSpread, 0, killSpread.GetSpeed() * Time.deltaTime);
                killPreset.DrawElementsLayout(storedKillSpread);
            }
        }

        /// <summary>
        /// Processing crosshair spread value relative controller states.
        /// </summary>
        protected float GetSpreadValue()
        {
            if (crosshairStates != null && crosshairStates.Length > 0 && controller != null)
            {
                for (int i = 0, length = crosshairStates.Length; i < length; i++)
                {
                    CrosshairState crosshairState = crosshairStates[i];
                    if (controller.CompareState(crosshairState.GetState()))
                    {
                        CrosshairSpread crosshairSpread = crosshairState.GetCrosshairSpread();
                        switch (spreadUpdateFunction)
                        {
                            case SpreadUpdateFunction.SmoothStep:
                                spread = Mathf.SmoothStep(spread, crosshairSpread.GetValue(), crosshairSpread.GetSpeed() * Time.deltaTime);
                                break;
                            case SpreadUpdateFunction.Lerp:
                                spread = Mathf.Lerp(spread, crosshairSpread.GetValue(), Time.deltaTime * crosshairSpread.GetSpeed());
                                break;
                            case SpreadUpdateFunction.MoveTowerds:
                                spread = Mathf.MoveTowards(spread, crosshairSpread.GetValue(), Time.deltaTime * crosshairSpread.GetSpeed());
                                break;
                        }
                        break;
                    }
                }
            }
            return spread;
        }

        /// <summary>
        /// Override current crosshair spread and apply fire spread value.
        /// </summary>
        public void ApplyFireSpread()
        {
            switch (spreadUpdateFunction)
            {
                case SpreadUpdateFunction.SmoothStep:
                    spread = Mathf.SmoothStep(spread, fireSpread.GetValue(), fireSpread.GetSpeed());
                    break;
                case SpreadUpdateFunction.Lerp:
                    spread = Mathf.Lerp(spread, fireSpread.GetValue(), Time.deltaTime * fireSpread.GetSpeed());
                    break;
                case SpreadUpdateFunction.MoveTowerds:
                    spread = Mathf.MoveTowards(spread, fireSpread.GetValue(), Time.deltaTime * fireSpread.GetSpeed());
                    break;
            }
        }

        /// <summary>
        /// Show crosshair hit effect.
        /// </summary>
        public void ShowHitEffect()
        {
            storedHitSpread = hitSpread.GetValue();
        }

        /// <summary>
        /// Show crosshair kill effect.
        /// </summary>
        public void ShowKillEffect()
        {
            storedHitSpread = -1;
            storedKillSpread = killSpread.GetValue();
        }

        #region [Getter / Setter]
        public FPController GetController()
        {
            return controller;
        }

        protected void SetController(FPController value)
        {
            controller = value;
        }

        public CrosshairPreset GetCrosshairPreset()
        {
            return crosshairPreset;
        }

        public void SetCrosshairPreset(CrosshairPreset value)
        {
            crosshairPreset = value;
        }

        public CrosshairState[] GetCrosshairStates()
        {
            return crosshairStates;
        }

        public CrosshairState GetCrosshairState(int index)
        {
            return crosshairStates[index];
        }

        public void SetCrosshairStates(CrosshairState[] value)
        {
            crosshairStates = value;
        }

        public void SetCrosshairState(int index, CrosshairState state)
        {
            crosshairStates[index] = state;
        }

        public int GetCrosshairStatesLength()
        {
            return crosshairStates?.Length ?? 0;
        }

        public float GetSpread()
        {
            return spread;
        }

        public void SetSpread(float value)
        {
            spread = value;
        }

        public SpreadUpdateFunction GetSpreadUpdateFunction()
        {
            return spreadUpdateFunction;
        }

        public void SetSpreadUpdateFunction(SpreadUpdateFunction value)
        {
            spreadUpdateFunction = value;
        }

        public CrosshairSpread GetFireSpread()
        {
            return fireSpread;
        }

        public void SetFireSpread(CrosshairSpread value)
        {
            fireSpread = value;
        }

        public CrosshairPreset GetHitPreset()
        {
            return hitPreset;
        }

        public void SetHitPreset(CrosshairPreset value)
        {
            hitPreset = value;
        }

        public CrosshairSpread GetHitSpread()
        {
            return hitSpread;
        }

        public void SetHitSpread(CrosshairSpread value)
        {
            hitSpread = value;
        }

        public float GetHitHideValue()
        {
            return hitHideValue;
        }

        public void SetHitHideValue(float value)
        {
            hitHideValue = value;
        }

        public CrosshairPreset GetKillPreset()
        {
            return killPreset;
        }

        public void SetKillPreset(CrosshairPreset value)
        {
            killPreset = value;
        }

        public CrosshairSpread GetKillSpread()
        {
            return killSpread;
        }

        public void SetKillSpread(CrosshairSpread value)
        {
            killSpread = value;
        }

        public float GetKillHideValue()
        {
            return killHideValue;
        }

        public void SetKillHideValue(float value)
        {
            killHideValue = value;
        }

        public bool Visible()
        {
            return visible;
        }

        public void Visible(bool value)
        {
            visible = value;
        }
        #endregion
    }
}