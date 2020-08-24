/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AuroraFPSRuntime.UI
{
    /// <summary>
    /// Player UI HUD Manager.
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        public enum HealthDisplayMode
        {
            Value,
            Persent
        }

        public struct GrenadeUIProperty
        {
            public WeaponGrenadeSystem system;
            public Text text;
        }

        [SerializeField] private Transform player;
        [SerializeField] private HealthDisplayMode healthValue;
        [SerializeField] private string grenadeGroupName;
        [SerializeField] private HUDElements hudElements = new HUDElements();

        private CharacterHealth health;
        private WeaponItem weaponItem;
        private WeaponShootingSystem weaponShootingSystem;
        private WeaponReloadSystem weaponReloadSystem;
        private GrenadeUIProperty[] grenadeUIProperties;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            if (player == null)
            {
                return;
            }

            health = player.GetComponent<CharacterHealth>();

            FPInventory inventory = player.GetComponent<FPInventory>();
            inventory.OnSwitchCallback += weapon =>
            {
                weaponItem = weapon.GetComponent<WeaponIdentifier>()?.GetWeaponItem();
                weaponShootingSystem = weapon.GetComponent<WeaponShootingSystem>();
                weaponReloadSystem = weapon.GetComponent<WeaponReloadSystem>();
            };

            inventory.OnHideCallback += () =>
            {
                weaponItem = null;
                weaponShootingSystem = null;
                weaponReloadSystem = null;
                hudElements.UpdateWeaponElements("Empty");
                hudElements.UpdateAmmoElements(0, 0);
                hudElements.UpdateFireMode("None");
            };

            inventory.OnDropCallback += _ =>
            {
                weaponItem = null;
                weaponShootingSystem = null;
                weaponReloadSystem = null;
                hudElements.UpdateWeaponElements("Empty");
                hudElements.UpdateAmmoElements(0, 0);
                hudElements.UpdateFireMode("None");
            };

            if (!string.IsNullOrEmpty(grenadeGroupName))
            {
                InitializeGrenadesUI(inventory);
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (health != null)
            {
                switch (healthValue)
                {
                    case HealthDisplayMode.Value:
                        hudElements.UpdateHealthElements(health.GetHealth(), health.GetHealth(), health.GetMaxHealth());
                        break;
                    case HealthDisplayMode.Persent:
                        hudElements.UpdateHealthElements(AMath.GetPersent(health.GetHealth(), health.GetMaxHealth()), health.GetHealth(), health.GetMaxHealth());
                        break;
                }
            }

            if (weaponItem != null)
            {
                hudElements.UpdateWeaponElements(weaponItem.GetDisplayName(), weaponItem.GetSprite());
            }

            if (weaponReloadSystem != null)
            {
                hudElements.UpdateAmmoElements(weaponReloadSystem.GetAmmoCount(), weaponReloadSystem.GetClipCount());
            }

            if (weaponShootingSystem != null)
            {
                hudElements.UpdateFireMode(weaponShootingSystem.GetCurrentFireMode().ToString());
            }

            if (!string.IsNullOrEmpty(grenadeGroupName) && grenadeUIProperties != null && grenadeUIProperties.Length > 0)
            {
                UpdateGrenadeUI();
            }
        }

        protected void InitializeGrenadesUI(FPInventory inventory)
        {
            List<InventorySlot> grenadeGroup = inventory.GetGroupItems(grenadeGroupName);
            if (grenadeGroup != null && grenadeGroup.Count > 0)
            {
                for (int i = 0; i < grenadeGroup.Count; i++)
                {
                    WeaponItem grenadeItem = grenadeGroup[i].GetWeaponItem();
                    if (grenadeItem != null)
                    {
                        Transform grenadeTransform = inventory.GetWeaponTransform(grenadeItem);
                        grenadeTransform.gameObject.SetActive(true);
                        WeaponGrenadeSystem grenadeSystem = grenadeTransform.GetComponent<WeaponGrenadeSystem>();
                        if (grenadeSystem != null && grenadeSystem.GetWeaponAmmoSystem() != null)
                        {
                            if (grenadeUIProperties == null)
                            {
                                grenadeUIProperties = new GrenadeUIProperty[grenadeGroup.Count];
                            }

                            GrenadeUIProperty grenadeUIProperty = grenadeUIProperties[i];
                            grenadeUIProperty.system = grenadeSystem;
                            grenadeUIProperty.text = hudElements.CreateGrenadeElement(grenadeItem.GetDisplayName(), grenadeSystem.GetWeaponAmmoSystem().GetAmmoCount(), grenadeItem.GetSprite());
                            grenadeUIProperties[i] = grenadeUIProperty;
                        }
                        grenadeTransform.gameObject.SetActive(false);
                    }

                }
            }
        }

        protected void UpdateGrenadeUI()
        {
            for (int i = 0; i < grenadeUIProperties.Length; i++)
            {
                GrenadeUIProperty grenadeUIProperty = grenadeUIProperties[i];
                grenadeUIProperty.text.text = grenadeUIProperty.system.GetWeaponAmmoSystem().GetAmmoCount().ToString();
            }
        }

        #region [Getter / Setter]
        public Transform GetPlayer()
        {
            return player;
        }

        public void SetPlayer(Transform value)
        {
            player = value;
        }

        public HealthDisplayMode GetHealthValue()
        {
            return healthValue;
        }

        public void SetHealthValue(HealthDisplayMode value)
        {
            healthValue = value;
        }

        public HUDElements GetElements()
        {
            return hudElements;
        }

        protected void SetHUDElements(HUDElements value)
        {
            hudElements = value;
        }

        public string GetGrenadeGroupName()
        {
            return grenadeGroupName;
        }

        public void SetGrenadeGroupName(string value)
        {
            grenadeGroupName = value;
        }

        public GrenadeUIProperty[] GetGrenadeUIProperties()
        {
            return grenadeUIProperties;
        }

        public void SetGrenadeUIProperties(GrenadeUIProperty[] value)
        {
            grenadeUIProperties = value;
        }

        public CharacterHealth GetHealth()
        {
            return health;
        }

        public void SetHealth(CharacterHealth value)
        {
            health = value;
        }
        #endregion
    }
}