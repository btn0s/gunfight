/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraFPSRuntime
{
    public class FPInventory : MonoBehaviour, IInventory
    {
        // Base first person inventory.
        [Header("Base Properties")]
        [SerializeField] private Transform weaponsContainer;
        [SerializeField] private bool allowIdenticalWeapons = false;
        [SerializeField] private SwitchWeaponMode switchMode = SwitchWeaponMode.Both;
        [SerializeField] private float mouseWheelSensitivity = 0.75f;
        [SerializeField] private KeyCode startWeaponKey;

        [Header("Groups Properties")]
        [SerializeField] private List<InventoryGroup> groups;

        [SerializeField] private UnityEvent onSwitchUnityEvent;
        [SerializeField] private UnityEvent onHideUnityEvent;
        [SerializeField] private UnityEvent onDropUnityEvent;

        // Stored required properties.
        private KeyCode activeWeaponKey;
        private bool muteSelecting;
        private CoroutineObject<WeaponItem> inventoryCoroutine;
        private Dictionary<KeyCode, WeaponItem> storedWeaponItems;
        private Dictionary<WeaponItem, Transform> storedWeaponTransforms;
        private Dictionary<string, List<InventorySlot>> storedGroups;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeWeaponItemsHash(ref storedWeaponItems);
            InitializeWeaponTransformsHash(ref storedWeaponTransforms);
            InitializeWeaponGroups(ref storedGroups);

            inventoryCoroutine = new CoroutineObject<WeaponItem>(this);



            FPAdaptiveRagdoll adaptiveRagdoll = GetComponent<FPAdaptiveRagdoll>();
            if (adaptiveRagdoll != null)
            {
                adaptiveRagdoll.OnRagdollCallback += HideWeaponForce;
                adaptiveRagdoll.OnRagdollCallback += () => MuteSelecting(true);

                adaptiveRagdoll.OnReadyToGoCallback += () => ActivateWeapon(activeWeaponKey);
                adaptiveRagdoll.OnReadyToGoCallback += () => MuteSelecting(false);
            }

            CharacterHealth characterHealth = GetComponent<CharacterHealth>();
            if (characterHealth != null)
            {
                if (adaptiveRagdoll == null)
                {
                    characterHealth.OnWakeUpCallback += () => ActivateWeapon(activeWeaponKey);
                    characterHealth.OnWakeUpCallback += () => MuteSelecting(false);
                }

                characterHealth.OnDeadCallback += HideWeaponForce;
                characterHealth.OnDeadCallback += () => MuteSelecting(true);
            }

            FPController controller = GetComponent<FPController>();
            if(controller != null)
            {
                CameraControl cameraControl = controller.GetCameraControl();
                OnHideCallback += cameraControl.ZoomOut;
                OnDropCallback += (weapon) => cameraControl.ZoomOut();
            }

            OnSwitchCallback += _ => onSwitchUnityEvent?.Invoke();
            OnHideCallback += () => onHideUnityEvent?.Invoke();
            OnDropCallback += _ => onDropUnityEvent?.Invoke();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            // Activate default start weapon.
            ActivateWeapon(startWeaponKey);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (groups.Count == 0)
            {
                return;
            }

            if (!muteSelecting)
            {
                switch (switchMode)
                {
                    case SwitchWeaponMode.Keyboard:
                        SelectWeaponByKey();
                        break;
                    case SwitchWeaponMode.MouseWheel:
                        SelectWeaponByMouseWheel();
                        break;
                    case SwitchWeaponMode.Both:
                        SelectWeaponByKey();
                        SelectWeaponByMouseWheel();
                        break;
                }
            }

            if (AInput.GetButtonDown(INC.Drop))
            {
                Drop();
            }
        }

        /// <summary>
        /// Add new weapon in available slot in inventory.
        /// </summary>
        public virtual bool Add(WeaponItem weapon)
        {
            if (weapon == null || !storedGroups.ContainsKey(weapon.GetGroup()) || (allowIdenticalWeapons && storedWeaponItems.ContainsValue(weapon)))
                return false;

            List<InventorySlot> slots = storedGroups[weapon.GetGroup()];
            if (slots == null || slots.Count == 0)
                return false;

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeaponItem() == null)
                {
                    storedWeaponItems[slot.GetKey()] = weapon;
                    slot.SetWeaponItem(weapon);
                    storedGroups[weapon.GetGroup()][i] = slot;
                    return true;
                }
                else if(i == length - 1 && slot.GetWeaponItem() != null)
                {
                    return Replace(weapon);
                }
            }
            return false;
        }

        /// <summary>
        /// Remove weapon from inventory.
        /// </summary>
        public virtual bool Remove(WeaponItem weapon)
        {
            if (weapon == null || !storedGroups.ContainsKey(weapon.GetGroup()) || !storedWeaponItems.ContainsValue(weapon))
                return false;

            List<InventorySlot> slots = storedGroups[weapon.GetGroup()];
            if (slots == null || slots.Count == 0)
                return false;

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeaponItem() == weapon)
                {
                    storedWeaponItems[slot.GetKey()] = null;
                    slot.SetWeaponItem(null);
                    storedGroups[weapon.GetGroup()][i] = slot;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Replace current weapon on new.
        /// </summary>
        public virtual bool Replace(WeaponItem weapon)
        {
            if (weapon == null || activeWeaponKey == KeyCode.None || !storedGroups.ContainsKey(weapon.GetGroup()))
                return false;

            WeaponItem currentWeapon = GetActiveWeaponItem();
            if (currentWeapon.GetGroup() != weapon.GetGroup())
            {
                return false;
            }

            if (!inventoryCoroutine.Start(ReplaceWeaponProcessing, weapon))
            {
                return false;
            }

            List<InventorySlot> slots = storedGroups[weapon.GetGroup()];
            if (slots == null || slots.Count == 0)
                return false;

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeaponItem() == currentWeapon)
                {
                    storedWeaponItems[slot.GetKey()] = weapon;
                    slot.SetWeaponItem(weapon);
                    storedGroups[weapon.GetGroup()][i] = slot;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Replace inventory weapon on some new weapon.
        /// </summary>
        public virtual bool Replace(WeaponItem from, WeaponItem to)
        {
            if (from == null || to != null ||
                activeWeaponKey == KeyCode.None ||
                !storedGroups.ContainsKey(from.GetGroup()) || !storedWeaponItems.ContainsValue(from) ||
                !storedGroups.ContainsKey(to.GetGroup()) || !storedWeaponItems.ContainsValue(to))
                return false;

            if (from.GetGroup() != to.GetGroup())
            {
                return false;
            }

            WeaponItem currentWeapon = GetActiveWeaponItem();
            if (currentWeapon == from)
            {
                if (!inventoryCoroutine.Start(ReplaceWeaponProcessing, to))
                {
                    return false;
                }
            }

            List<InventorySlot> slots = storedGroups[from.GetGroup()];
            if (slots == null || slots.Count == 0)
                return false;

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeaponItem() == from)
                {
                    storedWeaponItems[slot.GetKey()] = to;
                    slot.SetWeaponItem(to);
                    storedGroups[to.GetGroup()][i] = slot;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add new group in inventory.
        /// </summary>
        public virtual bool AddGroup(string groupName, params InventorySlot[] slots)
        {
            if (string.IsNullOrEmpty(groupName))
                return false;

            List<InventorySlot> _slots = new List<InventorySlot>();
            if (slots != null && slots.Length > 0)
            {
                for (int i = 0, length = slots.Length; i < length; i++)
                {
                    InventorySlot slot = slots[i];
                    _slots.Add(slot);
                    storedWeaponItems.Add(slot.GetKey(), slot.GetWeaponItem());
                }
            }
            groups.Add(new InventoryGroup(groupName, _slots));
            storedGroups.Add(groupName, _slots);
            return true;
        }

        /// <summary>
        /// Remove group from inventory including weapons contained in this group.
        /// </summary>
        public virtual bool RemoveGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                return false;

            List<InventorySlot> _slots = storedGroups[groupName];
            if (_slots != null && _slots.Count > 0)
            {
                for (int i = 0, length = _slots.Count; i < length; i++)
                {
                    InventorySlot slot = _slots[i];
                    if (storedWeaponItems.ContainsKey(slot.GetKey()))
                    {
                        if (activeWeaponKey == slot.GetKey())
                        {
                            HideWeapon();
                            activeWeaponKey = KeyCode.None;
                        }
                        storedWeaponItems.Remove(slot.GetKey());
                    }
                }
            }

            for (int i = 0, length = groups.Count; i < length; i++)
            {
                InventoryGroup group = groups[i];
                if (group.GetName() == groupName)
                {
                    groups.RemoveAt(i);
                    break;
                }
            }

            if (storedGroups.ContainsKey(groupName))
                storedGroups.Remove(groupName);
            return true;
        }

        /// <summary>
        /// Add new slot in group.
        /// </summary>
        public virtual bool AddSlot(string groupName, InventorySlot slot)
        {
            if (string.IsNullOrEmpty(groupName) || slot == null || !storedGroups.ContainsKey(groupName))
                return false;

            for (int i = 0, length = groups.Count; i < length; i++)
            {
                if (groups[i].GetName() == groupName)
                {
                    groups[i].GetInventorySlots().Add(slot);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove slot from group including weapons contained in this slot.
        /// </summary>
        public virtual bool RemoveSlotFromGroup(string groupName, KeyCode key)
        {
            if (string.IsNullOrEmpty(groupName) || !storedGroups.ContainsKey(groupName))
                return false;

            for (int i = 0, length = groups.Count; i < length; i++)
            {
                InventoryGroup group = groups[i];
                if (group.GetName() == groupName)
                {
                    for (int j = 0, _length = group.GetInventorySlotsLength(); j < _length; j++)
                    {
                        InventorySlot slot = group.GetInventorySlot(j);
                        if (slot.GetKey() == key)
                        {
                            if (activeWeaponKey == slot.GetKey())
                            {
                                HideWeapon();
                                activeWeaponKey = KeyCode.None;
                            }
                            groups[i].GetInventorySlots().RemoveAt(j);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public virtual bool ContainsWeapon(WeaponItem weaponItem)
        {
            return storedWeaponItems.ContainsValue(weaponItem);
        }

        /// <summary>
        /// Activate weapon by WeaponItem.
        /// </summary>
        public virtual bool ActivateWeapon(WeaponItem weapon)
        {
            if (!storedWeaponItems.ContainsValue(weapon) || !inventoryCoroutine.Start(SwitchWeaponProcessing, weapon))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Activate weapon by key.
        /// </summary>
        public virtual bool ActivateWeapon(KeyCode key)
        {
            if (key == KeyCode.None || !storedWeaponItems.ContainsKey(key) || inventoryCoroutine.IsProcessing())
            {
                return false;
            }

            WeaponItem weapon = storedWeaponItems[key];
            inventoryCoroutine.Start(SwitchWeaponProcessing, weapon);

            return true;
        }

        public virtual bool ActivateHiddenWeapon()
        {
            return ActivateWeapon(activeWeaponKey);
        }

        /// <summary>
        /// Get active weapon transform.
        /// </summary>
        public virtual Transform GetActiveWeaponTransform()
        {
            if (activeWeaponKey == KeyCode.None || !storedWeaponItems.ContainsKey(activeWeaponKey))
                return null;

            WeaponItem weaponItem = storedWeaponItems[activeWeaponKey];
            if (weaponItem == null)
                return null;

            return storedWeaponTransforms[weaponItem];
        }

        /// <summary>
        /// Get active WeaponItem.
        /// </summary>
        public virtual WeaponItem GetActiveWeaponItem()
        {
            if (activeWeaponKey == KeyCode.None || !storedWeaponItems.ContainsKey(activeWeaponKey))
                return null;
            return storedWeaponItems[activeWeaponKey];
        }

        /// <summary>
        /// Get weapon transform by WeaponItem.
        /// </summary>
        public virtual Transform GetWeaponTransform(WeaponItem weaponItem)
        {
            if (weaponItem == null || !storedWeaponTransforms.ContainsKey(weaponItem))
                return null;

            return storedWeaponTransforms[weaponItem];
        }

        /// <summary>
        /// Hide active weapon.
        /// </summary>
        public virtual void HideWeapon()
        {
            if (HasActiveWeapon() && !inventoryCoroutine.IsProcessing() && storedWeaponItems.ContainsKey(activeWeaponKey))
            {
                WeaponItem weaponItem = storedWeaponItems[activeWeaponKey];
                inventoryCoroutine.Start(HideWeaponProcessing, weaponItem);
            }
        }

        public virtual void HideWeaponForce()
        {
            if (HasActiveWeapon())
            {
                GetActiveWeaponTransform().gameObject.SetActive(false);
                HideWeapon();
            }
        }

        /// <summary>
        /// Drop active weapon.
        /// </summary>
        public virtual void Drop()
        {
            if (HasActiveWeapon() && storedWeaponItems.ContainsKey(activeWeaponKey))
            {
                WeaponItem weaponItem = storedWeaponItems[activeWeaponKey];
                inventoryCoroutine.Start(DropWeaponProcessing, weaponItem);
            }
        }

        /// <summary>
        /// Weapon count in all groups.
        /// </summary>
        public virtual int WeaponCount()
        {
            int count = 0;
            for (int i = 0, length = groups.Count; i < length; i++)
            {
                InventoryGroup group = groups[i];
                for (int j = 0, _length = group.GetInventorySlotsLength(); j < length; j++)
                {
                    InventorySlot slot = group.GetInventorySlot(i);
                    if (slot.GetWeaponItem() != null)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Slot count of the group.
        /// </summary>
        public virtual int SlotCount(string groupName)
        {
            if (string.IsNullOrEmpty(groupName) || !storedGroups.ContainsKey(groupName) || storedGroups[groupName] == null)
                return 0;
            return storedGroups[groupName].Count;
        }

        /// <summary>
        /// Weapons group count.
        /// </summary>
        public virtual int GroupCount(string groupName)
        {
            return groups.Count;
        }

        /// <summary>
        /// Check that specific group is full
        /// </summary>
        public virtual bool IsFull(string group)
        {
            if (string.IsNullOrEmpty(group) || !storedGroups.ContainsKey(group))
            {
                return true;
            }

            List<InventorySlot> slots = storedGroups[group];
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeaponItem() == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Has any current active weapon.
        /// </summary>
        public bool HasActiveWeapon()
        {
            return activeWeaponKey != KeyCode.None;
        }

        /// <summary>
        /// Processing selecting weapon by keycode.
        /// </summary>
        protected virtual void SelectWeaponByKey()
        {
            if (inventoryCoroutine.IsProcessing())
                return;

            if (Input.GetKeyDown(activeWeaponKey))
            {
                WeaponItem weaponItem = GetActiveWeaponItem();
                if (weaponItem != null)
                {
                    GameObject weaponObject = GetActiveWeaponTransform()?.gameObject;
                    if (weaponObject != null && weaponObject.activeSelf)
                    {
                        return;
                    }
                }
            }

            for (int i = 0, length = groups.Count; i < length; i++)
            {
                InventoryGroup group = groups[i];
                for (int j = 0, _length = group.GetInventorySlotsLength(); j < _length; j++)
                {
                    InventorySlot slot = group.GetInventorySlot(j);
                    if (Input.GetKeyDown(slot.GetKey()))
                    {
                        if (slot.GetWeaponItem() == null)
                        {
                            return;
                        }
                        ActivateWeapon(slot.GetWeaponItem());
                    }
                }
            }
        }

        /// <summary>
        /// Processing selecting weapom by mouse wheel;
        /// </summary>
        protected virtual void SelectWeaponByMouseWheel()
        {
            if (inventoryCoroutine.IsProcessing())
            {
                return;
            }

            float mouseWheelValue = AInput.GetAxisRaw(INC.MouseWheel);
            if (mouseWheelValue == 0 || (1 - Mathf.Abs(mouseWheelValue)) >= mouseWheelSensitivity)
            {
                return;
            }

            int weaponIndex = 0;
            int weaponGroup = 0;

            if (HasActiveWeapon() && storedWeaponItems.ContainsKey(activeWeaponKey))
            {
                string currentGroup = storedWeaponItems[activeWeaponKey].GetGroup();
                if (!storedGroups.ContainsKey(currentGroup))
                {
                    return;
                }
                List<InventorySlot> slots = storedGroups[currentGroup];

                for (int i = 0, length = groups.Count; i < length; i++)
                {
                    InventoryGroup group = groups[i];
                    if (group.GetName() == currentGroup)
                    {
                        weaponGroup = i;
                        slots = group.GetInventorySlots();
                        break;
                    }
                }

                if (slots != null)
                {
                    int s_length = slots.Count;
                    for (int i = 0; i < s_length; i++)
                    {
                        InventorySlot slot = slots[i];
                        if (slot.GetKey() == activeWeaponKey)
                        {
                            weaponIndex = i;
                        }
                    }
                    if (mouseWheelValue > 0)
                    {
                        if (weaponIndex - 1 >= 0)
                        {
                            KeyCode acivateKey = slots[weaponIndex - 1].GetKey();
                            if (acivateKey != activeWeaponKey)
                            {
                                ActivateWeapon(acivateKey);
                            }
                            return;
                        }
                        else if (weaponIndex - 1 < 0 && groups.Count == 1)
                        {
                            KeyCode acivateKey = slots[slots.Count - 1].GetKey();
                            if (acivateKey != activeWeaponKey)
                            {
                                ActivateWeapon(acivateKey);
                            }
                            return;
                        }
                        else if (weaponIndex - 1 < 0 && weaponGroup - 1 < 0)
                        {
                            List<InventorySlot> _slots = groups[groups.Count - 1].GetInventorySlots();
                            for (int i = 0, length = _slots.Count; i < length; i++)
                            {
                                InventorySlot slot = _slots[i];
                                if (slot.GetWeaponItem() != null)
                                {
                                    ActivateWeapon(slot.GetKey());
                                    return;
                                }
                            }
                        }
                        else if (weaponIndex - 1 < 0 && weaponGroup - 1 >= 0)
                        {
                            List<InventorySlot> _slots = groups[weaponGroup - 1].GetInventorySlots();
                            for (int i = 0, length = _slots.Count; i < length; i++)
                            {
                                InventorySlot slot = _slots[i];
                                if (slot.GetWeaponItem() != null)
                                {
                                    ActivateWeapon(slot.GetKey());
                                    return;
                                }
                            }
                        }
                    }
                    else if (mouseWheelValue < 0)
                    {
                        if (weaponIndex + 1 <= slots.Count - 1)
                        {
                            KeyCode acivateKey = slots[weaponIndex + 1].GetKey();
                            if (acivateKey != activeWeaponKey)
                            {
                                ActivateWeapon(acivateKey);
                            }
                            return;
                        }
                        else if (weaponIndex + 1 > (slots.Count - 1) && groups.Count == 1)
                        {
                            KeyCode acivateKey = slots[0].GetKey();
                            if (acivateKey != activeWeaponKey)
                            {
                                ActivateWeapon(acivateKey);
                            }
                            return;
                        }
                        else if (weaponIndex + 1 > (slots.Count - 1) && weaponGroup + 1 < (groups.Count - 1))
                        {
                            List<InventorySlot> _slots = groups[0].GetInventorySlots();
                            for (int i = 0, length = _slots.Count; i < length; i++)
                            {
                                InventorySlot slot = _slots[i];
                                if (slot.GetWeaponItem() != null)
                                {
                                    ActivateWeapon(slot.GetKey());
                                    return;
                                }
                            }
                        }
                        else if (weaponIndex + 1 > (slots.Count - 1) && weaponGroup + 1 <= (groups.Count - 1))
                        {
                            List<InventorySlot> _slots = groups[weaponGroup + 1].GetInventorySlots();
                            for (int i = 0, length = _slots.Count; i < length; i++)
                            {
                                InventorySlot slot = _slots[i];
                                if (slot.GetWeaponItem() != null)
                                {
                                    ActivateWeapon(slot.GetKey());
                                    return;
                                }
                            }
                        }
                    }
                }

            }
            else if (activeWeaponKey == KeyCode.None)
            {
                List<InventorySlot> _slots = groups[0].GetInventorySlots();
                for (int i = 0, _s_length = _slots.Count; i < _s_length; i++)
                {
                    InventorySlot _slot = _slots[i];
                    if (_slot.GetWeaponItem() != null)
                    {
                        ActivateWeapon(_slot.GetKey());
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Initialize weapon items dictionary hash list.
        /// </summary>
        protected virtual void InitializeWeaponItemsHash(ref Dictionary<KeyCode, WeaponItem> storedWeaponItems)
        {
            storedWeaponItems = new Dictionary<KeyCode, WeaponItem>();
            for (int i = 0, length = groups.Count; i < length; i++)
            {
                for (int j = 0, s_Length = groups[i].GetInventorySlotsLength(); j < s_Length; j++)
                {
                    InventorySlot slot = groups[i].GetInventorySlot(j);
                    storedWeaponItems.Add(slot.GetKey(), slot.GetWeaponItem());
                }
            }
        }

        /// <summary>
        /// Initialize weapon transforms dictionary hash list.
        /// </summary>
        protected virtual void InitializeWeaponTransformsHash(ref Dictionary<WeaponItem, Transform> storedWeaponTransforms)
        {
            storedWeaponTransforms = new Dictionary<WeaponItem, Transform>();
            for (int i = 0, length = weaponsContainer.childCount; i < length; i++)
            {
                Transform weapon = weaponsContainer.GetChild(i);
                WeaponIdentifier weaponIdentifier = weapon.GetComponent<WeaponIdentifier>();
                if (weaponIdentifier == null || weaponIdentifier.GetWeaponItem() == null)
                    continue;
                
                if(!storedWeaponTransforms.ContainsKey(weaponIdentifier.GetWeaponItem()))
                {
                    storedWeaponTransforms.Add(weaponIdentifier.GetWeaponItem(), weapon);
                }
                #if UNITY_EDITOR
                else
                {
                    Debug.Log(string.Format("Weapon object ({0}) with weapon item ({1}) was ignored, becouse weapon with same weapon item already contains.", weapon.transform.name, weaponIdentifier.GetWeaponItem().name));
                }
                #endif
            }

        }

        /// <summary>
        /// Initialize groups dictionary hash list.
        /// </summary>
        protected void InitializeWeaponGroups(ref Dictionary<string, List<InventorySlot>> storedGroups)
        {
            storedGroups = new Dictionary<string, List<InventorySlot>>();
            for (int i = 0, length = groups.Count; i < length; i++)
            {
                InventoryGroup group = groups[i];
                storedGroups.Add(group.GetName(), group.GetInventorySlots());
            }
        }

        /// <summary>
        /// Switch weapon processsing coroutine.
        /// </summary>
        protected virtual IEnumerator SwitchWeaponProcessing(WeaponItem weaponItem)
        {
            if (HasActiveWeapon())
            {
                WeaponItem activeWeaponItem = storedWeaponItems[activeWeaponKey];
                Transform currentTransform = storedWeaponTransforms[activeWeaponItem];
                IWeaponAnimationSystem currrentAnimator = null;
                if (currentTransform != null)
                {
                    currrentAnimator = currentTransform.GetComponent<IWeaponAnimationSystem>();
                }
                else
                {
                    inventoryCoroutine.Stop();
                    yield break;
                }

                if (currrentAnimator != null)
                {
                    currrentAnimator.PlayTakeOutAnimation();
                    yield return new WaitForSeconds(currrentAnimator.GetTakeOutTime());
                }

                currentTransform.gameObject.SetActive(false);
                activeWeaponKey = KeyCode.None;
            }

            if (storedWeaponTransforms.ContainsKey(weaponItem))
            {
                Transform targetTransform = storedWeaponTransforms[weaponItem];
                IWeaponAnimationSystem targetAnimator = targetTransform.GetComponent<IWeaponAnimationSystem>();
                targetTransform.gameObject.SetActive(true);

                if (targetAnimator != null)
                {
                    yield return new WaitForSeconds(targetAnimator.GetTakeInTime());
                }

#if UNITY_EDITOR
                if (string.IsNullOrEmpty(weaponItem.GetGroup()))
                {
                    Debug.LogError(string.Format("({0}), not have a group!", weaponItem.name));
                }
                else if (!storedGroups.ContainsKey(weaponItem.GetGroup()))
                {
                    Debug.LogError(string.Format("Inventory not contains ({0}) group of {1} weapon.", weaponItem.GetGroup(), weaponItem.name));
                }
#endif
                if (storedGroups.ContainsKey(weaponItem.GetGroup()))
                {
                    List<InventorySlot> slots = storedGroups[weaponItem.GetGroup()];
                    for (int i = 0, length = slots.Count; i < length; i++)
                    {
                        InventorySlot slot = slots[i];
                        if (slot.GetWeaponItem() == weaponItem)
                        {
                            activeWeaponKey = slot.GetKey();
                            break;
                        }
                    }
                }

                OnSwitchCallback?.Invoke(targetTransform);
                inventoryCoroutine.Stop();
                yield break;

            }
            else
            {
                inventoryCoroutine.Stop();
                yield break;
            }
        }

        /// <summary>
        /// Replace weapon processsing coroutine.
        /// </summary>
        protected virtual IEnumerator ReplaceWeaponProcessing(WeaponItem weaponItem)
        {
            if (HasActiveWeapon())
            {
                WeaponItem activeWeaponItem = storedWeaponItems[activeWeaponKey];
                Transform currentTransform = storedWeaponTransforms[activeWeaponItem];
                IWeaponAnimationSystem currrentAnimator = null;
                if (currentTransform != null)
                {
                    currrentAnimator = currentTransform.GetComponent<IWeaponAnimationSystem>();
                }
                else
                {
                    inventoryCoroutine.Stop();
                    yield break;
                }

                if (currrentAnimator != null)
                {
                    currrentAnimator.PlayTakeOutAnimation();
                    yield return new WaitForSeconds(currrentAnimator.GetTakeOutTime());
                }

                DropObject dropObject = activeWeaponItem.GetDropObject();
                if (dropObject != DropObject.none)
                {
                    dropObject.InstantiateAndThrow(weaponsContainer.position, weaponsContainer.forward);
                }

                currentTransform.gameObject.SetActive(false);
                activeWeaponKey = KeyCode.None;
            }

            Transform targetTransform = storedWeaponTransforms[weaponItem];
            IWeaponAnimationSystem targetAnimator = null;
            if (targetTransform != null)
            {
                targetAnimator = targetTransform.GetComponent<IWeaponAnimationSystem>();
            }
            else
            {
                inventoryCoroutine.Stop();
                yield break;
            }

            targetTransform.gameObject.SetActive(true);

            if (targetAnimator != null)
                yield return new WaitForSeconds(targetAnimator.GetTakeInTime());

            List<InventorySlot> slots = storedGroups[weaponItem.GetGroup()];
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeaponItem() == weaponItem)
                {
                    activeWeaponKey = slot.GetKey();
                    break;
                }
            }
            inventoryCoroutine.Stop();
            yield break;
        }

        /// <summary>
        /// Hide weapon processsing coroutine.
        /// </summary>
        protected virtual IEnumerator HideWeaponProcessing(WeaponItem weaponItem)
        {
            Transform activeWeaponTransform = storedWeaponTransforms[weaponItem];
            IWeaponAnimationSystem activeWeaponAnimator = null;
            if (activeWeaponTransform != null)
            {
                activeWeaponAnimator = activeWeaponTransform.GetComponent<IWeaponAnimationSystem>();
            }
            else
            {
                inventoryCoroutine.Stop();
                yield break;
            }

            if (activeWeaponAnimator != null)
            {
                activeWeaponAnimator.PlayTakeOutAnimation();
                yield return new WaitForSeconds(activeWeaponAnimator.GetTakeOutTime());
            }

            activeWeaponTransform.gameObject.SetActive(false);
            OnHideCallback?.Invoke();
            inventoryCoroutine.Stop();
            yield break;
        }

        /// <summary>
        /// Drop weapon processsing coroutine.
        /// </summary>
        protected virtual IEnumerator DropWeaponProcessing(WeaponItem weaponItem)
        {
            Transform activeWeaponTransform = storedWeaponTransforms[weaponItem];
            IWeaponAnimationSystem activeWeaponAnimator = null;
            if (activeWeaponTransform != null)
                activeWeaponAnimator = activeWeaponTransform.GetComponent<IWeaponAnimationSystem>();

            if (activeWeaponAnimator != null)
            {
                activeWeaponAnimator.PlayTakeOutAnimation();
                yield return new WaitForSeconds(activeWeaponAnimator.GetTakeOutTime());
            }

            activeWeaponTransform.gameObject.SetActive(false);
            Remove(weaponItem);
            OnDropCallback?.Invoke(activeWeaponTransform);

            DropObject dropObject = weaponItem.GetDropObject();
            if (dropObject != DropObject.none)
            {
                dropObject.InstantiateAndThrow(weaponsContainer.position, weaponsContainer.forward);
            }

            activeWeaponKey = KeyCode.None;
            inventoryCoroutine.Stop();
            yield break;
        }

        public List<InventorySlot> GetGroupItems(string groupName)
        {
            if (storedGroups.ContainsKey(groupName))
            {
                return storedGroups[groupName];
            }
            return null;
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On switch callback event function.
        /// OnSwitchCallback called when weapon is switched.
        /// </summary>
        /// <param name="Transform">New switched weapon.</param>
        public event Action<Transform> OnSwitchCallback;

        /// <summary>
        /// On hide callback event function.
        /// OnHideCallback called when weapon is hided.
        /// </summary>
        public event Action OnHideCallback;

        /// <summary>
        /// On drop callback event function.
        /// OnDropCallback called when dropping current weapon.
        /// </summary>
        /// <param name="Transform">Current droped weapon.</param>
        public event Action<Transform> OnDropCallback;

        #endregion

        #region [Getter / Setter]
        public Transform GetWeaponsContainer()
        {
            return weaponsContainer;
        }

        public void SetWeaponsContainer(Transform value)
        {
            weaponsContainer = value;
        }

        public List<InventoryGroup> GetGroups()
        {
            return groups;
        }

        public void SetGroups(List<InventoryGroup> value)
        {
            groups = value;
        }

        public InventoryGroup GetGroup(int index)
        {
            return groups[index];
        }

        public void SetGroup(int index, InventoryGroup value)
        {
            groups[index] = value;
        }

        public bool AllowIdenticalWeapons()
        {
            return allowIdenticalWeapons;
        }

        public void AllowIdenticalWeapons(bool value)
        {
            allowIdenticalWeapons = value;
        }

        public SwitchWeaponMode GetSwitchMode()
        {
            return switchMode;
        }

        public void SetSwitchMode(SwitchWeaponMode value)
        {
            switchMode = value;
        }

        public float GetMouseWheelSensitivity()
        {
            return mouseWheelSensitivity;
        }

        public void SetMouseWheelSensitivity(float value)
        {
            mouseWheelSensitivity = value;
        }

        public KeyCode GetStartWeaponKey()
        {
            return startWeaponKey;
        }

        public void SetStartWeaponKey(KeyCode value)
        {
            startWeaponKey = value;
        }

        public KeyCode GetActiveWeaponKey()
        {
            return activeWeaponKey;
        }

        protected void SetCurrentWeaponKey(KeyCode value)
        {
            activeWeaponKey = value;
        }

        public bool MuteSelecting()
        {
            return muteSelecting;
        }

        public void MuteSelecting(bool value)
        {
            muteSelecting = value;
        }

        protected UnityEvent GetOnSwitchUnityEvent()
        {
            return onSwitchUnityEvent;
        }

        protected void SetOnSwitchUnityEvent(UnityEvent value)
        {
            onSwitchUnityEvent = value;
        }

        protected UnityEvent GetOnHideUnityEvent()
        {
            return onHideUnityEvent;
        }

        protected void SetOnHideUnityEvent(UnityEvent value)
        {
            onHideUnityEvent = value;
        }

        protected UnityEvent GetOnDropUnityEvent()
        {
            return onDropUnityEvent;
        }

        protected void SetOnDropUnityEvent(UnityEvent value)
        {
            onDropUnityEvent = value;
        }

        public CoroutineObject<WeaponItem> GetInventoryCoroutine()
        {
            return inventoryCoroutine;
        }

        protected void SetInventoryCoroutine(CoroutineObject<WeaponItem> value)
        {
            inventoryCoroutine = value;
        }

        public Dictionary<KeyCode, WeaponItem> GetStoredWeaponItems()
        {
            return storedWeaponItems;
        }

        protected void SetStoredWeaponItems(Dictionary<KeyCode, WeaponItem> value)
        {
            storedWeaponItems = value;
        }

        public Dictionary<WeaponItem, Transform> GetStoredWeaponTransforms()
        {
            return storedWeaponTransforms;
        }

        protected void SetStoredWeaponTransforms(Dictionary<WeaponItem, Transform> value)
        {
            storedWeaponTransforms = value;
        }

        public Dictionary<string, List<InventorySlot>> GetStoredGroups()
        {
            return storedGroups;
        }

        protected void SetStoredGroups(Dictionary<string, List<InventorySlot>> value)
        {
            storedGroups = value;
        }
        #endregion
    }
}