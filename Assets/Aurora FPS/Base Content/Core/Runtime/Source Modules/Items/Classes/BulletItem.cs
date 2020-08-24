/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [CreateAssetMenu(fileName = "Bullet Item", menuName = AuroraFPSProduct.Name + "/Items/Bullet", order = 125)]
    public class BulletItem : SceneItem
    {
        [SerializeField] private int damage = 5;
        [SerializeField] private float impulse = 1.75f;
        [SerializeField] private float ballsVariance = 0;
        [SerializeField] private int ballsNumber = 1;
        [SerializeField] private DecalMapping decalMapping;

        public Vector3 GetRandomVarianceDirection(Vector3 localEulerAngles)
        {
            if (ballsVariance > 0)
            {
                localEulerAngles.x = Random.Range(localEulerAngles.x - ballsVariance, localEulerAngles.x + ballsVariance);
                localEulerAngles.y = Random.Range(localEulerAngles.y - ballsVariance, localEulerAngles.y + ballsVariance);
            }
            return localEulerAngles;
        }

        #region [Getter / Setter]
        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        public float GetImpulse()
        {
            return impulse;
        }

        public void SetImpulse(float value)
        {
            impulse = value;
        }

        public float GetBallsVariance()
        {
            return ballsVariance;
        }

        public void SetBallsVariance(float value)
        {
            ballsVariance = value;
        }

        public int GetBallsNumber()
        {
            return ballsNumber;
        }

        public void SetBallsNumber(int value)
        {
            ballsNumber = value;
        }

        public DecalMapping GetDecalMapping()
        {
            return decalMapping;
        }

        public void SetDecalMapping(DecalMapping value)
        {
            decalMapping = value;
        }
        #endregion
    }
}