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
    [AIBehaviourOptions(path = "Attack/Melee Combat", priority = 5)]
    public class AIMeleeCombatBehaviour : AIBehaviour
    {
        // Base ranged combat properties.
        [Header("Combat Properties")]
        [SerializeField] private Transform attackPoint;
        [SerializeField] private int attackDamage = 5;
        [SerializeField] private float attackImpulse = 25.0f;
        [SerializeField] private float attackSpeed = 0.9f;
        [SerializeField] private float attackRange = 500.0f;
        [SerializeField] private float accuracy = 0.75f;
        [SerializeField] private float accuracyLimit = 0.25f;
        [SerializeField] private bool randomAccuracy = true;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AnimatorValue attackState = "Fire";
        [SerializeField] private DecalMapping decalMapping;
        [SerializeField] private LayerMask cullingLayer = Physics.AllLayers;

        // Base behaviour properties.
        [Header("Behaviour Properties")]
        [SerializeField] private float attackDistance = 12.5f;
        [SerializeField] private float rotateSpeed = 7.0f;

        // Stored required components.
        private Transform transform;
        private Animator animator;
        private AIFieldOfView fieldOfView;
        private AudioSource audioSource;

        // Stored required properties.
        private float storedTime;
        private Transform target;

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

            if (randomAccuracy)
            {
                accuracy = AMath.AllocatePart(Random.Range(accuracyLimit, accuracy), 100);
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
                transform.LookAt(target, rotateSpeed);
                transform.FreezeRotation(AuroraExtension.Axis.X, 0);
                float currentDistance = Vector3.Distance(transform.position, target.position);
                if (currentDistance <= attackDistance)
                {
                    if (storedTime == 0)
                    {
                        storedTime = Time.time;
                    }
                    if (storedTime > 0 && (Time.time - storedTime >= attackSpeed))
                    {
                        MakeAttack();
                        OnAttackCallback?.Invoke();
                        storedTime = 0;
                    }
                }
                else
                {
                    navMeshAgent.SetDestination(target.position);
                }
            }
        }

        /// <summary>
        /// Make shoot from weapon to the target.
        /// </summary>
        protected virtual void MakeAttack()
        {
            Vector3 direction = CalculateDirectionWithAccuracy(accuracy, attackPoint.position, target.position);
            if (Physics.Raycast(attackPoint.position, direction, out RaycastHit hitInfo, attackRange, cullingLayer, QueryTriggerInteraction.Ignore))
            {
                Transform hitTransform = hitInfo.transform;
                Decal.Spawn(decalMapping, hitInfo);
                TrySendDamage(hitTransform, direction);
                TrySendImpulse(hitTransform);
            }
            PlayAttackAnimation();
            PlayAttackSounds();
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
                health?.TakeDamage(attackDamage);
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
                    rigidbody.AddForce(attackPoint.forward * attackImpulse, ForceMode.Impulse);
                }
            }
        }

        /// <summary>
        /// Calculate direction from start to end point with specific accuracy.
        /// </summary>
        /// <param name="accuracy">Accuracy value [0...1]</param>
        /// <param name="start">Start point.</param>
        /// <param name="end">End point.</param>
        /// <returns>Direction from start to end point with specific accuracy.</returns>
        private Vector3 CalculateDirectionWithAccuracy(float accuracy, Vector3 start, Vector3 end)
        {
            Vector3 direction = end - start;
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
        protected virtual void PlayAttackAnimation()
        {
            animator.PlayInFixedTime(attackState.GetNameHash(), 1, 0.1f);
        }

        /// <summary>
        /// Play all fire sounds.
        /// </summary>
        protected void PlayAttackSounds()
        {
            if (attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }

        /// <summary>
        /// Initialize OnFindTargetsCallbacks that's will be added when behaviour start 
        /// and will be removed when behaviour stopped.
        /// </summary>
        protected virtual void OnFindTargetsCallback()
        {
            target = fieldOfView.GetVisibleTarget(0);
        }

        #region [Event Callback Functions]
        /// <summary>
        /// Called when AI making attack.
        /// </summary>
        public event Action OnAttackCallback;
        #endregion

        #region [Getter / Setter]
        public Transform GetAttackPoint()
        {
            return attackPoint;
        }

        public void SetAttackPoint(Transform value)
        {
            attackPoint = value;
        }

        public int GetAttackDamage()
        {
            return attackDamage;
        }

        public void SetAttackDamage(int value)
        {
            attackDamage = value;
        }

        public float GetAttackImpulse()
        {
            return attackImpulse;
        }

        public void SetAttackImpulse(float value)
        {
            attackImpulse = value;
        }

        public float GetAttackSpeed()
        {
            return attackSpeed;
        }

        public void SetAttackSpeed(float value)
        {
            attackSpeed = value;
        }

        public float GetAttackRange()
        {
            return attackRange;
        }

        public void SetAttackRange(float value)
        {
            attackRange = value;
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

        public bool GetRandomAccuracy()
        {
            return randomAccuracy;
        }

        public void SetRandomAccuracy(bool value)
        {
            randomAccuracy = value;
        }

        public AudioClip GetAttackSound()
        {
            return attackSound;
        }

        public void SetAttackSound(AudioClip value)
        {
            attackSound = value;
        }

        public AnimatorValue GetAttackState()
        {
            return attackState;
        }

        public void SetAttackState(AnimatorValue value)
        {
            attackState = value;
        }

        public DecalMapping GetDecalMapping()
        {
            return decalMapping;
        }

        public void SetDecalMapping(DecalMapping value)
        {
            decalMapping = value;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public void SetCullingLayer(LayerMask value)
        {
            cullingLayer = value;
        }

        public float GetAttackDistance()
        {
            return attackDistance;
        }

        public void SetAttackDistance(float value)
        {
            attackDistance = value;
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