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
using UnityEngine.Events;
using IEnumerator = System.Collections.IEnumerator;
using Random = UnityEngine.Random;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(WeaponAnimationSystem))]
    [RequireComponent(typeof(WeaponReloadSystem))]
    [RequireComponent(typeof(AudioSource))]
    public abstract partial class WeaponShootingSystem : MonoBehaviour
    {
        #region [Aurora Unity Events]
        [Serializable] public class AOnFireModeChangedEvent : UnityEvent<FireMode> { }
        #endregion

        /// <summary>
        /// Fire mode of the weapon.
        ///     Single - Single fire.
        ///     Fixed queue - Fire with fixed queue (for example 3 fire).
        ///     Free - Free fire without restrictions.
        /// </summary>
        public enum FireMode
        {
            Mute = 0,
            Single = 1,
            Queue = 2,
            Free = 3
        }

        // Base Shoothing properties.
        [SerializeField] private FireMode fireMode = FireMode.Mute;

        [SerializeField] private float singleFireRate = 0.09f;
        [SerializeField] private float freeFireRate = 0.09f;
        [SerializeField] private float queueFireRate = 0.05f;
        [SerializeField] private int queueCount = 3;

        [SerializeField] private Transform firePoint;

        [SerializeField] private RecoilMapping recoilMapping;

        [SerializeField] private FireSounds fireSounds;
        [SerializeField] private ParticleSystem[] fireEffects;

        [SerializeField] private AnimatorState fireState = "Fire";
        [SerializeField] private AnimatorState dryFireState = "Dry Fire";
        [SerializeField] private AnimatorState zoomFireState = "Zoom Fire";
        [SerializeField] private AnimatorState zoomDryFireState = "Zoom Dry Fire";

        [SerializeField] private AOnFireModeChangedEvent onFireModeChangedEvent;
        [SerializeField] private UnityEvent onFireEvent;
        [SerializeField] private UnityEvent onDryFireEvent;
        [SerializeField] private UnityEvent onStartShootingEvent;
        [SerializeField] private UnityEvent onStopShootingEvent;

        // Stored required components.
        private Animator animator;
        private FPController controller;
        private WeaponReloadSystem weaponReloadSystem;
        private WeaponAnimationSystem weaponAnimationSystem;
        private CameraControl cameraLook;
        private AudioSource audioSource;
        private CameraShake shakeCamera;

        // Stored required properties.
        private CoroutineObject fireModeCoroutine;
        private Vector3 originalFirePointRotation;
        private bool isShooting;
        private bool isShootingStarted;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            weaponReloadSystem = GetComponent<WeaponReloadSystem>();
            weaponAnimationSystem = GetComponent<WeaponAnimationSystem>();
            audioSource = GetComponent<AudioSource>();

            controller = transform.root.GetComponent<FPController>();
            cameraLook = controller.GetCameraControl();
            shakeCamera = CameraShake.Instance;

            fireModeCoroutine = new CoroutineObject(this);

            originalFirePointRotation = firePoint.localEulerAngles;

            if (firePoint == null)
            {
                firePoint = CreateFirePoint();
            }

            OnFireCallback += PlayFireAnimation;
            OnFireCallback += PlayFireEffects;
            OnFireCallback += PlayFireSound;
            OnFireCallback += () => isShooting = true;

            OnDryFireCallback += PlayDryFireSound;
            OnDryFireCallback += PlayDryFireAnimation;
            OnDryFireCallback += () => isShooting = true;

            OnFireModeChangedCallback += onFireModeChangedEvent.Invoke;
            OnFireCallback += onFireEvent.Invoke;
            OnDryFireCallback += onDryFireEvent.Invoke;
            OnStartShootingCallback += onStartShootingEvent.Invoke;
            OnStopShootingCallback += onStopShootingEvent.Invoke;

            OnStopShootingCallback += cameraLook.ReturnRecoil;
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            SetFireModeCoroutine(fireMode);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (AInput.GetButtonDown(INC.ChangeFireMode))
            {
                AutoChangeFireMode();
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            fireModeCoroutine.Stop();
        }

        /// <summary>
        /// Fire processing.
        /// </summary>
        protected abstract void FireProcessing();

        /// <summary>
        /// Weapon single fire mode processing.
        /// </summary>
        protected virtual IEnumerator SingleFireModeProcessing()
        {
            WaitForSeconds fireRate = new WaitForSeconds(singleFireRate);
            while (true)
            {
                if (!weaponReloadSystem.IsReloading() && AInput.GetButtonDown(INC.Attack))
                {
                    if (!weaponReloadSystem.AmmoIsEmpty())
                    {
                        if (!isShootingStarted)
                        {
                            OnStartShootingCallback.Invoke();
                            isShootingStarted = true;
                        }
                        ApplyRecoilPattern();
                        FireProcessing();
                        ResetRecoilChanges();
                        weaponReloadSystem.AmmoSubtraction();
                        OnFireCallback?.Invoke();
                    }
                    else
                    {
                        OnDryFireCallback?.Invoke();
                    }
                    yield return fireRate;
                }
                if (!isShooting && isShootingStarted)
                {
                    OnStopShootingCallback?.Invoke();
                    isShootingStarted = false;
                }
                isShooting = false;
                yield return null;
            }
        }

        /// <summary>
        /// Weapon queue fire mode processing.
        /// </summary>
        protected virtual IEnumerator QueueFireModeProcessing()
        {
            WaitForSeconds fireRate = new WaitForSeconds(queueFireRate);
            while (true)
            {
                if (!weaponReloadSystem.IsReloading() && AInput.GetButtonDown(INC.Attack))
                {
                    int storedQueueCount = queueCount;
                    while (storedQueueCount > 0)
                    {
                        if (!weaponReloadSystem.AmmoIsEmpty())
                        {
                            if (!isShootingStarted)
                            {
                                OnStartShootingCallback.Invoke();
                                isShootingStarted = true;
                            }
                            ApplyRecoilPattern();
                            FireProcessing();
                            ResetRecoilChanges();
                            weaponReloadSystem.AmmoSubtraction();
                            OnFireCallback?.Invoke();
                            storedQueueCount--;
                        }
                        else
                        {
                            OnDryFireCallback?.Invoke();
                            storedQueueCount = 0;
                        }
                        yield return fireRate;
                    }
                }
                if (!isShooting && isShootingStarted)
                {
                    OnStopShootingCallback?.Invoke();
                    isShootingStarted = false;
                }
                isShooting = false;
                yield return null;
            }
        }

        /// <summary>
        /// Weapon free fire mode processing.
        /// </summary>
        protected virtual IEnumerator FreeFireModeProcessing()
        {
            WaitForSeconds fireRate = new WaitForSeconds(freeFireRate);
            while (true)
            {
                if (!weaponReloadSystem.IsReloading())
                {
                    if (!weaponReloadSystem.AmmoIsEmpty() && AInput.GetButton(INC.Attack))
                    {
                        if (!isShootingStarted)
                        {
                            OnStartShootingCallback.Invoke();
                            isShootingStarted = true;
                        }
                        ApplyRecoilPattern();
                        FireProcessing();
                        ResetRecoilChanges();
                        weaponReloadSystem.AmmoSubtraction();
                        OnFireCallback?.Invoke();
                        yield return fireRate;
                    }
                    else if (weaponReloadSystem.AmmoIsEmpty() && AInput.GetButtonDown(INC.Attack))
                    {
                        OnDryFireCallback?.Invoke();
                    }
                }
                yield return null;
                if (!isShooting && isShootingStarted)
                {
                    OnStopShootingCallback?.Invoke();
                    isShootingStarted = false;
                }
                isShooting = false;
            }
        }

        protected virtual void SetFireModeCoroutine(FireMode fireMode)
        {
            switch (fireMode)
            {
                case FireMode.Single:
                    fireModeCoroutine.Start(SingleFireModeProcessing, true);
                    break;
                case FireMode.Queue:
                    fireModeCoroutine.Start(QueueFireModeProcessing, true);
                    break;
                case FireMode.Free:
                    fireModeCoroutine.Start(FreeFireModeProcessing, true);
                    break;
            }
        }

        /// <summary>
        /// Change currect fire mode.
        /// </summary>
        public void ChangeFireMode(FireMode fireMode)
        {
            if (this.fireMode != fireMode)
            {
                SetFireModeCoroutine(fireMode);
                this.fireMode = fireMode;
                OnFireModeChangedCallback?.Invoke(fireMode);
            }
        }

        /// <summary>
        /// Automatically change current mode to next mode from fire mode list.
        /// </summary>
        public FireMode AutoChangeFireMode()
        {
            FireMode nextFireMode = FireMode.Mute;
            switch (fireMode)
            {
                case FireMode.Mute:
                    nextFireMode = FireMode.Single;
                    break;
                case FireMode.Single:
                    nextFireMode = FireMode.Queue;
                    break;
                case FireMode.Queue:
                    nextFireMode = FireMode.Free;
                    break;
                case FireMode.Free:
                    nextFireMode = FireMode.Single;
                    break;
            }
            ChangeFireMode(nextFireMode);
            fireMode = nextFireMode;
            return nextFireMode;
        }

        /// <summary>
        /// Apply recoil pattern by current controller state.
        /// </summary>
        public virtual void ApplyRecoilPattern()
        {
            if (recoilMapping != null && controller != null)
            {
                if (recoilMapping.TryGetValue(controller.GetState(), out RecoilPattern recoilPattern))
                {
                    ApplyFirePointAccuracy(recoilPattern.GetVerticalBulletSpread(), recoilPattern.GetHorizontalBulletSpread());
                    cameraLook.AddRecoil(recoilPattern);
                    shakeCamera.AddShake(recoilPattern.GetShake());
                    weaponAnimationSystem.AddKickback(recoilPattern.GetWeaponKickbackProperty());
                }
            }
        }

        /// <summary>
        /// Reset recoil pattern changes.
        /// </summary>
        public void ResetRecoilChanges()
        {
            if (recoilMapping != null && controller != null)
            {
                ResetFirePointToOriginalAngle();
            }
        }

        public void ResetFirePointToOriginalAngle()
        {
            firePoint.localEulerAngles = originalFirePointRotation;
        }

        public void ApplyFirePointAccuracy(RangedFloat vertical, RangedFloat horizontal)
        {
            Vector3 accuracy = firePoint.localEulerAngles;
            accuracy.x += Random.Range(vertical.GetMin(), vertical.GetMax());
            accuracy.y += Random.Range(horizontal.GetMin(), horizontal.GetMax());
            firePoint.localEulerAngles = accuracy;
        }

        /// <summary>
        /// Play random fire sound.
        /// </summary>
        public void PlayFireSound()
        {
            AudioClip fireSound = fireSounds.GetRandomNormalFireSound();
            if (fireSound != null)
            {
                audioSource.PlayOneShot(fireSound);
            }
        }

        /// <summary>
        /// Play random dry fire sound.
        /// </summary>
        public void PlayDryFireSound()
        {
            AudioClip emptyFireSound = fireSounds.GetRandomDryFireSound();
            if (emptyFireSound != null)
            {
                audioSource.PlayOneShot(emptyFireSound);
            }
        }

        /// <summary>
        /// Play automatically both particle system effects (muzzle flash and cartridge ejection).
        /// </summary>
        public void PlayFireEffects()
        {
            if (fireEffects == null || fireEffects.Length == 0)
            {
                return;
            }

            for (int i = 0, length = fireEffects.Length; i < length; i++)
            {
                ParticleSystem effect = fireEffects[i];
                effect?.Play();
            }
        }

        public void PlayFireAnimation()
        {
            AnimatorState state = !cameraLook.IsZooming() ? fireState : zoomFireState;
            animator.CrossFadeInFixedTime(state);
        }

        public void PlayDryFireAnimation()
        {
            AnimatorState state = !cameraLook.IsZooming() ? dryFireState : zoomDryFireState;
            animator.CrossFadeInFixedTime(state);
        }

        /// <summary>
        /// Create new fire point for this weapon.
        /// </summary>
        public Transform CreateFirePoint()
        {
            Transform camera = Camera.main.transform;
            GameObject point = new GameObject(string.Format("Fire point [{0}]", name));
            point.transform.SetParent(transform);
            point.transform.localPosition = camera.localPosition;
            point.transform.localRotation = transform.localRotation;
            point.transform.localScale = Vector3.one;
            return point.transform;
        }

        #region [Delegates Callbakcs]
        /// <summary>
        ///Called when fire mode is changed.
        /// </summary>
        /// <param name="FireMode">New fire mode.</param>
        public event Action<FireMode> OnFireModeChangedCallback;

        /// <summary>
        /// Called when weapon fire (bullets count more then zero).
        /// </summary>
        public event Action OnFireCallback;

        /// <summary>
        /// Called when weapon dry fired (bullets count equal zero).
        /// </summary>
        public event Action OnDryFireCallback;

        /// <summary>
        /// Called when weapon started shooting.  
        /// </summary>
        public event Action OnStartShootingCallback;

        /// <summary>
        /// Called when weapon stopped shooting.
        /// </summary>
        public event Action OnStopShootingCallback;
        #endregion

        #region [Getter / Setter]
        public bool IsShooting()
        {
            return isShooting;
        }

        protected void IsShooting(bool value)
        {
            isShooting = value;
        }

        public FireMode GetCurrentFireMode()
        {
            return fireMode;
        }

        public float GetFreeFireRate()
        {
            return freeFireRate;
        }

        public void SetFreeFireRate(float value)
        {
            freeFireRate = value;
        }

        public float GetSingleFireRate()
        {
            return singleFireRate;
        }

        public void SetSingleFireRate(float value)
        {
            singleFireRate = value;
        }

        public float GetQueueFireRate()
        {
            return queueFireRate;
        }

        public void SetQueueFireRate(float value)
        {
            queueFireRate = value;
        }

        public int GetQueueCount()
        {
            return queueCount;
        }

        public void SetQueueCount(int value)
        {
            queueCount = value;
        }

        public Transform GetFirePoint()
        {
            return firePoint;
        }

        public void SetFirePoint(Transform value)
        {
            firePoint = value;
        }

        public RecoilMapping GetRecoilMapping()
        {
            return recoilMapping;
        }

        public void SetRecoilMapping(RecoilMapping value)
        {
            recoilMapping = value;
        }

        public FireSounds GetFireSounds()
        {
            return fireSounds;
        }

        public void SetFireSounds(FireSounds value)
        {
            fireSounds = value;
        }

        public ParticleSystem[] GetFireEffects()
        {
            return fireEffects;
        }

        public void SetFireEffects(ParticleSystem[] value)
        {
            fireEffects = value;
        }

        public AnimatorState GetFireState()
        {
            return fireState;
        }

        public void SetFireState(AnimatorState value)
        {
            fireState = value;
        }

        public AnimatorState GetDryFireState()
        {
            return dryFireState;
        }

        public void SetDryFireState(AnimatorState value)
        {
            dryFireState = value;
        }

        public AnimatorState GetZoomFireState()
        {
            return zoomFireState;
        }

        public void SetZoomFireState(AnimatorState value)
        {
            zoomFireState = value;
        }

        public AnimatorState GetZoomDryFireState()
        {
            return zoomDryFireState;
        }

        public void SetZoomDryFireState(AnimatorState value)
        {
            zoomDryFireState = value;
        }

        public WeaponReloadSystem GetWeaponReloading()
        {
            return weaponReloadSystem;
        }

        protected void SetWeaponReloading(WeaponReloadSystem value)
        {
            weaponReloadSystem = value;
        }

        public AOnFireModeChangedEvent GetOnFireModeChangedEvent()
        {
            return onFireModeChangedEvent;
        }

        public void SetOnFireModeChangedEvent(AOnFireModeChangedEvent value)
        {
            onFireModeChangedEvent = value;
        }

        public UnityEvent GetOnFireEvent()
        {
            return onFireEvent;
        }

        public void SetOnFireEvent(UnityEvent value)
        {
            onFireEvent = value;
        }

        public UnityEvent GetOnDryFireEvent()
        {
            return onDryFireEvent;
        }

        public void SetOnDryFireEvent(UnityEvent value)
        {
            onDryFireEvent = value;
        }

        public UnityEvent GetOnStartShootingEvent()
        {
            return onStartShootingEvent;
        }

        public void SetOnStartShootingEvent(UnityEvent value)
        {
            onStartShootingEvent = value;
        }

        public UnityEvent GetOnStopShootingEvent()
        {
            return onStopShootingEvent;
        }

        public void SetOnStopShootingEvent(UnityEvent value)
        {
            onStopShootingEvent = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        protected void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }
        #endregion
    }
}