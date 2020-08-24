/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AICore : MonoBehaviour
    {
        [SerializeField] private string defaultBehaviour;
        [SerializeReference] private List<AIBehaviour> behaviours;

        // Stored required components.
        protected NavMeshAgent navMeshAgent;

        // Stored required properties.
        private AIBehaviour activeBehaviour;

        /// <summary>
        /// /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            InitializeBehaviours();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            activeBehaviour?.Start();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            SetDefaultBehaviour();

            OnEnabledCallback += (enabled) => navMeshAgent.isStopped = !enabled;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            activeBehaviour?.Update();
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            activeBehaviour?.FixedUpdate();
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            activeBehaviour?.LateUpdate();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            activeBehaviour?.Stop();
        }

        /// <summary>
        /// Set default AI state behaviour. 
        /// </summary>
        public void SetDefaultBehaviour()
        {
            SwitchBehaviour(defaultBehaviour);
        }

        /// <summary>
        /// Switch to the new AIBehaviour.
        /// </summary>
        public void SwitchBehaviour(string name)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                AIBehaviour behaviour = behaviours[i];
                if (behaviour.GetName() == name)
                {
                    activeBehaviour?.Stop();
                    activeBehaviour = behaviour;
                    activeBehaviour?.Start();
                    OnSwitchBehaviourCallback?.Invoke(activeBehaviour);
                }
            }
        }

        /// <summary>
        /// Add new behaviour instance in AICore.
        /// </summary>
        /// <param name="key">The name for the new behaviour to add.</param>
        /// <param name="value">The behaviour instance to add.</param>
        /// <param name="Switch">Auto switch on the new behaviour after add.</param>
        public void AddBehaviour(AIBehaviour behaviour, bool autoSwitch = false)
        {
            if (!ContainsBehaviour(behaviour.GetName()))
            {
                behaviours.Add(behaviour);
                if (autoSwitch)
                {
                    SwitchBehaviour(behaviour.GetName());
                }
            }
        }

        /// <summary>
        /// Removes the behaviour instance from AICore.
        /// </summary>
        /// <param name="key">Behaviour name to remove.</param>
        /// <returns>
        /// True if the element is successfully found and removed.
        /// Otherwise, false.
        /// </returns>
        public bool RemoveBehaviour(string behaviourName, string nextBehaviour)
        {
            if (behaviourName != nextBehaviour &&
                ContainsBehaviour(behaviourName) &&
                ContainsBehaviour(nextBehaviour))
            {
                SwitchBehaviour(nextBehaviour);
                return behaviours.Remove(GetBehaviour(behaviourName));
            }
            return false;
        }

        /// <summary>
        /// Check that behaviour with specific name contains in AICore.
        /// </summary>
        /// <param name="name">Behaviour name.</param>
        public bool ContainsBehaviour(string name)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i].GetName() == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check that behaviour with specific name contains in AICore.
        /// </summary>
        /// <param name="name">Behaviour name.</param>
        public bool ContainsBehaviour(AIBehaviour behaviour)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i].GetName() == behaviour.GetName())
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveAllBehaviours()
        {
            behaviours.Clear();
        }

        /// <summary>
        /// Initialize all binded AI behaviours.
        /// </summary>
        protected virtual void InitializeBehaviours()
        {
            for (int i = 0; i < this.behaviours.Count; i++)
            {
                this.behaviours[i].Initialize(this);

            }
        }

        /// <summary>
        /// Get AI velocity.
        /// </summary>
        public Vector3 GetVelocity()
        {
            return navMeshAgent.velocity;
        }

        /// <summary>
        /// AI is enabled.
        /// </summary>
        public bool Enabled()
        {
            return enabled;
        }

        /// <summary>
        /// Set AI enabled.
        /// </summary>
        public virtual void Enabled(bool enabled)
        {
            this.enabled = enabled;
            OnEnabledCallback?.Invoke(enabled);
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On enabled callback function.
        /// OnEnabledCallback called when AI controller enabled state changed.
        /// </summary>
        /// <param name="bool">New enabled state.</param>
        public event Action<bool> OnEnabledCallback;

        /// <summary>
        /// On switch behaviour callback function.
        /// OnSwitchStateCallback called when AI controller behaviour being changed.
        /// </summary>
        /// <param name="AIState">New AI behaviour name.</param>
        public event Action<AIBehaviour> OnSwitchBehaviourCallback;
        #endregion

        #region [Getter / Setter]
        public List<AIBehaviour> GetBehaviours()
        {
            return behaviours;
        }

        public void SetBehaviours(List<AIBehaviour> value)
        {
            behaviours = value;
        }

        public AIBehaviour GetBehaviour(int index)
        {
            return behaviours[index];
        }

        public void SetBehaviour(int index, AIBehaviour value)
        {
            behaviours[index] = value;
        }

        public AIBehaviour GetBehaviour(string name)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                AIBehaviour behaviour = behaviours[i];
                if (behaviour.GetName() == name)
                {
                    return behaviour;
                }
            }
            return null;
        }

        public void SetBehaviour(string name, AIBehaviour value)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i].GetName() == name)
                {
                    behaviours[i] = value;
                }
            }
        }

        public int GetBehaviourCount()
        {
            return behaviours.Count;
        }

        public string GetDefaultBehaviour()
        {
            return defaultBehaviour;
        }

        public void SetDefaultBehaviour(string name)
        {
            if (ContainsBehaviour(name))
            {
                defaultBehaviour = name;
            }
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return navMeshAgent;
        }

        protected void SetNavMeshAgent(NavMeshAgent value)
        {
            navMeshAgent = value;
        }
        #endregion
    }
}