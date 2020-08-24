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
    public partial class WeaponAnimationSystem
    {
        [System.Serializable]
        public class KickbackProperty
        {
            [SerializeField] private Vector3 kickbackPosition = new Vector3(0.01f, 0.01f, -0.01f);
            [SerializeField] private float positionSpeedUp = 10.0f;
            [SerializeField] private float positionSpeedOut = 15.0f;
            [SerializeField] private bool randomizePosition = true;

            [SerializeField] private Vector3 kickbackRotation = Vector3.zero;
            [SerializeField] private float rotationSpeedUp = 0.0f;
            [SerializeField] private float rotationSpeedOut = 0.0f;
            [SerializeField] private bool randomizeRotation = false;

            public Vector3 GetKickbackPosition()
            {
                if (randomizePosition)
                    return new Vector3(Random.Range(-kickbackPosition.x, kickbackPosition.x), Random.Range(-kickbackPosition.y, kickbackPosition.y), Random.Range(-kickbackPosition.z, kickbackPosition.z));
                return kickbackPosition;
            }

            public void SetKickbackPosition(Vector3 value)
            {
                kickbackPosition = value;
            }

            public Vector3 GetKickbackRotation()
            {
                if(randomizeRotation)
                    return new Vector3(Random.Range(-kickbackRotation.x, kickbackRotation.x), Random.Range(-kickbackRotation.y, kickbackRotation.y), Random.Range(-kickbackRotation.z, kickbackRotation.z));
                return kickbackRotation;
            }

            public void SetKickbackRotation(Vector3 value)
            {
                kickbackRotation = value;
            }

            #region [Getter / Setter]
            public float GetPositionSpeedUp()
            {
                return positionSpeedUp;
            }

            public void SetPositionSpeedUp(float value)
            {
                positionSpeedUp = value;
            }

            public float GetPositionSpeedOut()
            {
                return positionSpeedOut;
            }

            public void SetPositionSpeedOut(float value)
            {
                positionSpeedOut = value;
            }

            public bool GetRandomizePosition()
            {
                return randomizePosition;
            }

            public void SetRandomizePosition(bool value)
            {
                randomizePosition = value;
            }

            public float GetRotationSpeedUp()
            {
                return rotationSpeedUp;
            }

            public void SetRotationSpeedUp(float value)
            {
                rotationSpeedUp = value;
            }

            public float GetRotationSpeedOut()
            {
                return rotationSpeedOut;
            }

            public void SetRotationSpeedOut(float value)
            {
                rotationSpeedOut = value;
            }

            public bool GetRandomizeRotation()
            {
                return randomizeRotation;
            }

            public void SetRandomizeRotation(bool value)
            {
                randomizeRotation = value;
            }
            #endregion
        }
    }
}