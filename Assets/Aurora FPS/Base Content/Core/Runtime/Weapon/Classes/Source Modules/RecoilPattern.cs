/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using Shake = AuroraFPSRuntime.CameraShake.Shake;
using KickbackProperty = AuroraFPSRuntime.WeaponAnimationSystem.KickbackProperty;

namespace AuroraFPSRuntime
{
    [Serializable]
    public class RecoilPattern
    {
        [SerializeField] private RangedFloat verticalBulletSpread = new RangedFloat(-1.0f, 1.0f, -10.0f, 10.0f);
        [SerializeField] private RangedFloat horizontalBulletSpread = new RangedFloat(-1.0f, 1.0f, -10.0f, 10.0f);
        [SerializeField] private RangedFloat verticalRecoilImpulse = new RangedFloat(-1.5f, 1.5f, -10.0f, 10.0f);
        [SerializeField] private RangedFloat horizontalRecoilImpulse = new RangedFloat(-0.0f, 0.0f, -10.0f, 10.0f);
        [SerializeField] private RangedFloat clampVerticalRecoil = new RangedFloat(-5.0f, 5.0f, -180.0f, 180.0f);
        [SerializeField] private RangedFloat clampHorizontalRecoil = new RangedFloat(-2.0f, 2.0f, -180.0f, 180.0f);
        [SerializeField] private Vector2 impulseInputCompensate = new Vector2(1.0f, 1.0f);
        [SerializeField] private Vector2 speedApplyImpulse = new Vector2(1.0f, 1.0f);
        [SerializeField] private Vector2 speedReturnImpulse = new Vector2(1.0f, 1.0f);
        [SerializeField] private bool autoReturn = true;
        [SerializeField] private bool randomVerticalCameraRecoil = false;
        [SerializeField] private bool randomHorizontalCameraRecoil = false;
        [SerializeField] private KickbackProperty weaponKickbackProperty;
        [SerializeField] private Shake shake = new Shake(Shake.Target.Both, new CameraShake.ShakeProperty(0.25f, 7.5f, 0.15f, AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f)), new CameraShake.ShakeProperty(2f, 17.5f, 0.15f, AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f)));

        #region [Getter / Setter]
        public RangedFloat GetVerticalBulletSpread()
        {
            return verticalBulletSpread;
        }

        public void SetVerticalBulletSpread(RangedFloat value)
        {
            verticalBulletSpread = value;
        }

        public RangedFloat GetHorizontalBulletSpread()
        {
            return horizontalBulletSpread;
        }

        public void SetHorizontalBulletSpread(RangedFloat value)
        {
            horizontalBulletSpread = value;
        }

        public RangedFloat GetVerticalRecoilImpulse()
        {
            return verticalRecoilImpulse;
        }

        public void SetVerticalRecoilImpulse(RangedFloat value)
        {
            verticalRecoilImpulse = value;
        }

        public RangedFloat GetHorizontalRecoilImpulse()
        {
            return horizontalRecoilImpulse;
        }

        public void SetHorizontalRecoilImpulse(RangedFloat value)
        {
            horizontalRecoilImpulse = value;
        }

        public RangedFloat GetClampVerticalRecoil()
        {
            return clampVerticalRecoil;
        }

        public void SetClampVerticalRecoil(RangedFloat value)
        {
            clampVerticalRecoil = value;
        }

        public RangedFloat GetClampHorizontalRecoil()
        {
            return clampHorizontalRecoil;
        }

        public void SetClampHorizontalRecoil(RangedFloat value)
        {
            clampHorizontalRecoil = value;
        }

        public Vector2 GetImpulseInputCompensate()
        {
            return impulseInputCompensate;
        }

        public void SetImpulseInputCompensate(Vector2 value)
        {
            impulseInputCompensate = value;
        }

        public Vector2 GetApplyImpulseSpeed()
        {
            return speedApplyImpulse;
        }

        public void SetApplyImpulseSpeed(Vector2 value)
        {
            speedApplyImpulse = value;
        }

        public Vector2 GetReturnImpulseSpeed()
        {
            return speedReturnImpulse;
        }

        public void SetReturnImpulseSpeed(Vector2 value)
        {
            speedReturnImpulse = value;
        }

        public bool AutoReturn()
        {
            return autoReturn;
        }

        public void AutoReturn(bool value)
        {
            autoReturn = value;
        }

        public bool RandomVerticalCameraRecoil()
        {
            return randomVerticalCameraRecoil;
        }

        public void RandomVerticalCameraRecoil(bool value)
        {
            randomVerticalCameraRecoil = value;
        }

        public bool RandomHorizontalCameraRecoil()
        {
            return randomHorizontalCameraRecoil;
        }

        public void RandomHorizontalCameraRecoil(bool value)
        {
            randomHorizontalCameraRecoil = value;
        }

        public KickbackProperty GetWeaponKickbackProperty()
        {
            return weaponKickbackProperty;
        }

        public void SetWeaponKickbackProperty(KickbackProperty value)
        {
            weaponKickbackProperty = value;
        }

        public Shake GetShake()
        {
            return shake;
        }

        public void SetShake(Shake value)
        {
            shake = value;
        }
        #endregion
    }
}