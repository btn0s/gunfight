/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using System;
using IEnumerator = System.Collections.IEnumerator;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class QuickMeleeAttacker : MonoBehaviour
    {
        // Base quick melee attacker properties.
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float range;
        [SerializeField] private int damage;
        [SerializeField] private float timeToAttack;
        [SerializeField] private float attackRate;
        [SerializeField] private KeyCode attackKey;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private DecalMapping decalMapping;
        [SerializeField] private LayerMask attackLayer;
        [SerializeField] private string attackState;

        // Stored required components.
        private AudioSource audioSource;
        private Animator animator;

        // Stored required properties.
        private int attackStateHash;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            attackStateHash = Animator.StringToHash(attackState);
        }

        /// <summary>
        /// Quick attack processing.
        /// </summary>
        protected virtual IEnumerator AttackProcessing()
        {
            WaitForSeconds waitForAttackTime = new WaitForSeconds(timeToAttack);
            WaitForSeconds waitForRate = new WaitForSeconds(attackRate);
            while (true)
            {
                if (Input.GetKeyDown(attackKey))
                {
                    yield return waitForAttackTime;
                    Attack();
                    yield return waitForRate;
                }
                yield return null;
            }
        }

        public virtual void Attack()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hitInfo, range, attackLayer, QueryTriggerInteraction.Ignore))
            {
                Transform hitTransform = hitInfo.transform;
                PlayAttackAnimation();
                SendDamage(hitTransform);
                PlayAttackSound();
                Decal.Spawn(decalMapping, hitInfo);
                OnAttackHittedCallback?.Invoke(hitTransform);
            }
            OnAttackCallback?.Invoke();
        }

        /// <summary>
        /// Trying to send damage to other transform.
        /// </summary>
        public virtual void SendDamage(Transform other)
        {
            CharacterHealth health = other?.GetComponent<CharacterHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                OnAttackHealthHitCallback?.Invoke(other);
            }
        }

        /// <summary>
        /// Play attack sound.
        /// </summary>
        public void PlayAttackSound()
        {
            if (attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }

        public void PlayAttackAnimation()
        {
            animator.PlayInFixedTime(attackStateHash, 0, 0.1f);
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On attack callback event function.
        /// OnAttackCallback called when quick attack. 
        /// </summary>
        public event Action OnAttackCallback;

        /// <summary>
        /// On attack hitted callback event function.
        /// OnAttackHittedCallback called when quick attack hitted on any collider/rigidbody.
        /// </summary>
        /// <param name="Transform">Hitted transform instance.</param>
        public event Action<Transform> OnAttackHittedCallback;

        /// <summary>
        /// On attack hitted callback event function.
        /// OnAttackHealthHitCallback called when quick attack hitted on any object with component implemented from CharacterHealth abstract class.
        /// </summary>
        /// <param name="Transform">The Transform instance associated with health.</param>
        public event Action<Transform> OnAttackHealthHitCallback;

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

        public float GetRange()
        {
            return range;
        }

        public void SetRange(float value)
        {
            range = value;
        }

        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        public float GetTimeToAttack()
        {
            return timeToAttack;
        }

        public void SetTimeToAttack(float value)
        {
            timeToAttack = value;
        }

        public float GetAttackRate()
        {
            return attackRate;
        }

        public void SetAttackRate(float value)
        {
            attackRate = value;
        }

        public KeyCode GetAttackKey()
        {
            return attackKey;
        }

        public void SetAttackKey(KeyCode value)
        {
            attackKey = value;
        }

        public AudioClip GetAttackSound()
        {
            return attackSound;
        }

        public void SetAttackSound(AudioClip value)
        {
            attackSound = value;
        }

        public DecalMapping GetDecalMapping()
        {
            return decalMapping;
        }

        public void SetDecalMapping(DecalMapping value)
        {
            decalMapping = value;
        }

        public LayerMask GetAttackLayer()
        {
            return attackLayer;
        }

        public void SetAttackLayer(LayerMask value)
        {
            attackLayer = value;
        }

        public string GetAttackState()
        {
            return attackState;
        }

        public void SetAttackState(string value)
        {
            attackState = value;
            attackStateHash = Animator.StringToHash(attackState);
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        protected void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }

        public int GetAttackStateHash()
        {
            return attackStateHash;
        }

        protected void SetAttackStateHash(int value)
        {
            attackStateHash = value;
        }
        #endregion
    }
}