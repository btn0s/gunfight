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
    public class HealthHitArea : HealthComponent
    {
        // Base hit area properties.
        [SerializeField] private int multiplier = 0;
        [SerializeField] private int protection = 25;

        // Stored required properties.
        private HealthComponent rootHealthComponent;

        /// <summary>
        /// Initialize hit area.
        /// </summary>
        /// <param name="rootHealthComponent">Root health component.</param>
        public virtual void Initialize(HealthComponent rootHealthComponent)
        {
            this.rootHealthComponent = rootHealthComponent;
        }

        #region [IHealth Implementation]
        /// <summary>
        /// Get health point of root health system.
        /// </summary>
        public override int GetHealth()
        {
            return rootHealthComponent.GetHealth();
        }

        /// <summary>
        /// Alive state of root health system.
        /// </summary>
        /// <returns>
        /// True if health > 0.
        /// Otherwise false.
        /// </returns>
        public override bool IsAlive()
        {
            return rootHealthComponent.IsAlive();
        }
        #endregion

        #region [IHealthDamageable Implementation]
        /// <summary>
        /// Take damage to the root health system.
        /// </summary>
        /// <param name="amount">Damage amount.</param>
        public override void TakeDamage(int amount)
        {
            if(rootHealthComponent == null)
            {
                return;
            }

            amount = Mathf.Abs(amount);

            if (protection > 0)
            {
                protection -= amount;
                if (protection < 0)
                {
                    rootHealthComponent.TakeDamage(Mathf.Abs(protection));
                    protection = 0;
                }
                else
                {
                    rootHealthComponent.TakeDamage(0);
                }
            }
            else if (protection == 0)
            {
                rootHealthComponent.TakeDamage(amount + multiplier);
            }
        }
        #endregion

        #region [Getter / Setter]
        public int GetMultiplier()
        {
            return multiplier;
        }

        public void SetMultiplier(int value)
        {
            multiplier = value;
        }

        public int GetProtection()
        {
            return protection;
        }

        public void SetProtection(int value)
        {
            protection = value;
        }

        public HealthComponent GetRootHealthComponent()
        {
            return rootHealthComponent;
        }

        protected void SetRootHealthComponent(HealthComponent value)
        {
            rootHealthComponent = value;
        }
        #endregion
    }
}

