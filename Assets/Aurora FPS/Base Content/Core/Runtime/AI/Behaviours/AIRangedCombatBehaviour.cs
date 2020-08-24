/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AIFieldOfView))]
    [RequireComponent(typeof(AudioSource))]
    [AIBehaviourOptions(path = "Attack/Ranged Combat", priority = 4)]
    public class AIRangedCombatBehaviour : AIBehaviour
    {
        public enum Tactic
        {
            InPlace,
            Spin,
            GetAroundFront,
            GetAroundBack,
            GetAroundLeft,
            GetAroundRight
        }

        // Base ranged combat properties.
        [Header("Combat Properties")]
        [SerializeField] private BulletItem bulletItem;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 0.9f;
        [SerializeField] private float fireRange = 500.0f;
        [SerializeField] private float accuracy = 0.75f;
        [SerializeField] private float accuracyLimit = 0.25f;
        [SerializeField] private float targetOffset = 0.75f;
        [SerializeField] private bool randomAccuracy = true;
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AnimatorValue fireState = "Fire";
        [SerializeField] private ParticleSystem[] fireEffects;
        [SerializeField] private LayerMask cullingLayer = Physics.AllLayers;

        // Base reload properties.
        [Header("Reload Properties")]
        [SerializeField] private int bulletCount = 30;
        [SerializeField] private float reloadTime = 1.5f;
        [SerializeField] private AudioClip reloadSound;
        [SerializeField] private AnimatorValue reloadState = "Reload";

        // Base behaviour properties.
        [Header("Behaviour Properties")]
        [SerializeField] private Tactic tactic = Tactic.Spin;
        [SerializeField] private bool randomTactic = true;
        [SerializeField] private float distance = 12.5f;
        [SerializeField] private float rotateSpeed = 7.0f;

        // Stored required components.
        private Transform transform;
        private Animator animator;
        private AIFieldOfView fieldOfView;
        private AudioSource audioSource;

        // Stored required properties.
        private Transform target;
        private int storedBulletCount;
        private CoroutineObject fireCoroutine;

        /// <summary>
        /// Initiailze is called when the script instance is being loaded.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void Initialize(AICore core)
        {
            base.Initialize(core);
            transform = core.transform;
            animator = core.GetComponent<Animator>();
            fieldOfView = core.GetComponent<AIFieldOfView>();
            audioSource = core.GetComponent<AudioSource>();
            fireCoroutine = new CoroutineObject(core);

            storedBulletCount = bulletCount;

            if (randomAccuracy)
            {
                accuracy = AMath.AllocatePart(Random.Range(accuracyLimit, accuracy), 100);
            }

            if (randomTactic)
            {
                tactic = (Tactic)Random.Range(0, Enum.GetNames(typeof(Tactic)).Length);
            }
        }

        /// <summary>
        /// Update is called every frame, if the AIBehaviour is enabled.
        /// </summary>
        public override void Update()
        {
            base.Update();
            target = fieldOfView.GetVisibleTargetCount() > 0 ? fieldOfView.GetVisibleTarget(0) : null;
            if (target != null)
            {
                fireCoroutine.Start(ShootingProcessing);
                transform.LookAt(target, rotateSpeed);
                transform.FreezeRotation(AuroraExtension.Axis.X, 0);
                // float currentDistance = Vector3.Distance(transform.position, target.position);
                switch (tactic)
                {
                    case Tactic.InPlace:
                        navMeshAgent.isStopped = true;
                        break;
                    case Tactic.Spin:
                        Vector3 desiredDestination = (target.position - transform.position) + transform.right * distance;
                        navMeshAgent.SetDestination(desiredDestination);
                        break;
                    case Tactic.GetAroundFront:
                        navMeshAgent.SetDestination(target.position + (Vector3.forward * distance));
                        break;
                    case Tactic.GetAroundBack:
                        navMeshAgent.SetDestination(target.position - (Vector3.forward * distance));
                        break;
                    case Tactic.GetAroundLeft:
                        navMeshAgent.SetDestination(target.position + (-Vector3.right * distance));
                        break;
                    case Tactic.GetAroundRight:
                        navMeshAgent.SetDestination(target.position + (Vector3.right * distance));
                        break;
                }
            }
        }

        /// <summary>
        /// This function is called when the AIBehaviour becomes disabled.
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            fireCoroutine.Stop();
        }

        /// <summary>
        /// Fire processing coroutine.
        /// 
        /// Started when the behaviour becomes enabled.
        /// Stopped when the behaviour becomes disabled.
        /// </summary>
        protected virtual IEnumerator ShootingProcessing()
        {
            WaitForSeconds waitForFire = new WaitForSeconds(fireRate);
            WaitForSeconds waitForReload = new WaitForSeconds(reloadTime);
            while (true)
            {
                if (target != null && bulletCount > 0)
                {
                    MakeShoot();
                    OnFireCallback?.Invoke();
                    yield return waitForFire;
                }
                else if (bulletCount <= 0)
                {
                    PlayReloadAnimation();
                    PlayReloadSound();
                    BeforeReloadCallback?.Invoke();
                    yield return waitForReload;
                    MakeReload();
                    AfterReloadCallback?.Invoke();
                }
                else
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// Make shoot from weapon to the target.
        /// </summary>
        protected virtual void MakeShoot()
        {
            RaycastHit hitInfo;
            Vector3 direction = CalculateDirectionWithAccuracy(accuracy, firePoint.position, target.position);
            if (Physics.Raycast(firePoint.position, direction, out hitInfo, fireRange, cullingLayer, QueryTriggerInteraction.Ignore))
            {
                Transform hitTransform = hitInfo.transform;
                Decal.Spawn(bulletItem.GetDecalMapping(), hitInfo);
                TrySendDamage(hitTransform, direction);
                TrySendImpulse(hitTransform);
            }
            bulletCount--;
            PlayFireAnimation();
            PlayFireEffects();
            PlayFireSounds();
        }

        /// <summary>
        /// Make weapon reload.
        /// </summary>
        protected virtual void MakeReload()
        {
            bulletCount = storedBulletCount;
        }

        /// <summary>
        /// Trying send damage to transform.
        /// Successfully if transform have class implemented from CharacterHealth.
        /// </summary>
        /// <param name="value">Target to send damage.</param>
        protected void TrySendDamage(Transform value, Vector3 direction)
        {
            if (value != null)
            {
                CharacterHealth health = value.GetComponent<CharacterHealth>();
                health?.TakeDamage(bulletItem.GetDamage());
            }
        }

        /// <summary>
        /// Trying send physics impulse to transform.
        /// Successfully if transform have Rigidbody component.
        /// </summary>
        /// <param name="value">Target to send physics impulse.</param>
        protected void TrySendImpulse(Transform value)
        {
            if (value != null)
            {
                Rigidbody rigidbody = value.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.AddForce(firePoint.forward * bulletItem.GetImpulse(), ForceMode.Impulse);
                }
            }
        }

        /// <summary>
        /// Calculate direction from start to end point with specific accuracy.
        /// </summary>
        /// <param name="accuracy">Accuracy value [0...1]</param>
        /// <param name="origin">Start point.</param>
        /// <param name="direction">End point.</param>
        /// <returns>Direction from start to end point with specific accuracy.</returns>
        private Vector3 CalculateDirectionWithAccuracy(float accuracy, Vector3 origin, Vector3 direction)
        {
            direction += Vector3.up * targetOffset;
            direction = direction - origin;
            float actualAccuracy = (1 - accuracy) * 2;
            direction.x = Random.Range(direction.x - actualAccuracy, direction.x + actualAccuracy);
            direction.y = Random.Range(direction.y - actualAccuracy, direction.y + actualAccuracy);
            return direction;
        }

        /// <summary>
        /// Play fire animation from animator controller.
        /// 
        /// Default parameters: State hash: [Fire state hash], Layer: [1], Fixed time: [0.1f].
        /// </summary>
        protected virtual void PlayFireAnimation()
        {
            animator.PlayInFixedTime(fireState.GetNameHash(), 1, 0.1f);
        }

        /// <summary>
        /// Play reload animation from animator controller.
        /// 
        /// Default parameters: State hash: [Reload state hash], Layer: [1], Fixed time: [0.1f].
        /// </summary>
        protected virtual void PlayReloadAnimation()
        {
            animator.PlayInFixedTime(reloadState.GetNameHash(), 1, 0.1f);
        }

        /// <summary>
        /// Play all fire sounds.
        /// </summary>
        protected void PlayFireSounds()
        {
            if (fireSound != null)
            {
                audioSource.clip = fireSound;
                audioSource.Play();
            }
        }

        /// <summary>
        /// Play reload sound.
        /// </summary>
        protected void PlayReloadSound()
        {
            if (reloadSound != null)
            {
                audioSource.clip = reloadSound;
                audioSource.Play();
            }
        }

        /// <summary>
        /// Play all fire effects.
        /// </summary>
        protected void PlayFireEffects()
        {
            if (fireEffects != null && fireEffects.Length != 0)
            {
                for (int i = 0; i < fireEffects.Length; i++)
                {
                    ParticleSystem effect = fireEffects[i];
                    effect?.Play();
                }
            }
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On fire callback event function.
        /// Called when AI making shoot.
        /// </summary>
        public event Action OnFireCallback;

        /// <summary>
        /// Before reload callback event function.
        /// Called before reloading.
        /// </summary>
        public event Action BeforeReloadCallback;

        /// <summary>
        /// After reload callback event function.
        /// Called after reload.
        /// </summary>
        public event Action AfterReloadCallback;

        #endregion

        #region [Getter / Setter]
        public BulletItem GetBulletItem()
        {
            return bulletItem;
        }

        public void SetBulletItem(BulletItem value)
        {
            bulletItem = value;
        }

        public Transform GetFirePoint()
        {
            return firePoint;
        }

        public void SetFirePoint(Transform value)
        {
            firePoint = value;
        }

        public float GetFireRate()
        {
            return fireRate;
        }

        public void SetFireRate(float value)
        {
            fireRate = value;
        }

        public float GetFireRange()
        {
            return fireRange;
        }

        public void SetFireRange(float value)
        {
            fireRange = value;
        }

        public float GetAccuracy()
        {
            return accuracy;
        }

        public void SetAccuracy(float value)
        {
            accuracy = value;
        }

        public float GetAccuracyLimit()
        {
            return accuracyLimit;
        }

        public void SetAccuracyLimit(float value)
        {
            accuracyLimit = value;
        }

        public float GetTargetOffset()
        {
            return targetOffset;
        }

        public void SetTargetOffset(float value)
        {
            targetOffset = value;
        }

        public bool GetRandomAccuracy()
        {
            return randomAccuracy;
        }

        public void SetRandomAccuracy(bool value)
        {
            randomAccuracy = value;
        }

        public ParticleSystem[] GetFireEffects()
        {
            return fireEffects;
        }

        public void SetFireEffects(ParticleSystem[] value)
        {
            fireEffects = value;
        }

        public AudioClip GetFireSound()
        {
            return fireSound;
        }

        public void SetFireSound(AudioClip value)
        {
            fireSound = value;
        }

        public AnimatorValue GetFireState()
        {
            return fireState;
        }

        public void SetFireState(AnimatorValue value)
        {
            fireState = value;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public void SetCullingLayer(LayerMask value)
        {
            cullingLayer = value;
        }

        public int GetBulletCount()
        {
            return bulletCount;
        }

        public void SetBulletCount(int value)
        {
            bulletCount = value;
        }

        public int GetStoredBulletCount()
        {
            return storedBulletCount;
        }

        public void SetStoredBulletCount(int value)
        {
            storedBulletCount = value;
        }

        public float GetReloadTime()
        {
            return reloadTime;
        }

        public void SetReloadTime(float value)
        {
            reloadTime = value;
        }

        public AudioClip GetReloadSound()
        {
            return reloadSound;
        }

        public void SetReloadSound(AudioClip value)
        {
            reloadSound = value;
        }

        public AnimatorValue GetReloadState()
        {
            return reloadState;
        }

        public void SetReloadState(AnimatorValue value)
        {
            reloadState = value;
        }

        public Tactic GetTactic()
        {
            return tactic;
        }

        public void SetTactic(Tactic value)
        {
            tactic = value;
        }

        public bool RandomTactic()
        {
            return randomTactic;
        }

        public void RandomTactic(bool value)
        {
            randomTactic = value;
        }

        public float GetDistance()
        {
            return distance;
        }

        public void SetDistance(float value)
        {
            distance = value;
        }

        public float GetRotateSpeed()
        {
            return rotateSpeed;
        }

        public void SetRotateSpeed(float value)
        {
            rotateSpeed = value;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        protected void SetTransform(Transform value)
        {
            transform = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }

        public AIFieldOfView GetFieldOfView()
        {
            return fieldOfView;
        }

        protected void SetFieldOfView(AIFieldOfView value)
        {
            fieldOfView = value;
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