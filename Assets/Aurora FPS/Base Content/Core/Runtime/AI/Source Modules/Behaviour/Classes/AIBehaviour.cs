/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AuroraFPSRuntime.AI
{
    [System.Serializable]
    public abstract class AIBehaviour : IAIBehaviourName, IAIBehaviourCore
    {
        [SerializeField] private string name;
        [SerializeField] private List<Transition> transitions;
        [SerializeField] private UnityEvent startEvent;

        protected AICore core;
        protected NavMeshAgent navMeshAgent;

        /// <summary>
        /// Initiailze is called when the script instance is being loaded.
        /// </summary>
        public virtual void Initialize(AICore core)
        {
            this.core = core;
            navMeshAgent = core.GetNavMeshAgent();
            InitiailzeTrasitionConditions(core);
        }

        /// <summary>
        /// Initiailze all trasition conditions of this AICore.
        /// </summary>
        protected void InitiailzeTrasitionConditions(AICore core)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                List<Condition> conditions = transitions[i].GetConditions();
                for (int j = 0; j < conditions.Count; j++)
                {
                    Condition condition = conditions[j];
                    condition?.Initialize(core);
                    conditions[j] = condition;
                }
                transitions[i].SetConditions(conditions);
            }
        }

        /// <summary>
        /// This function is called when the AIBehaviour becomes enabled.
        /// </summary>
        public virtual void Start()
        {
            EnableTransitionConditions();
            startEvent?.Invoke();
        }

        /// <summary>
        /// Update is called every frame, if the AIBehaviour is enabled.
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the AIBehaviour is enabled.
        /// </summary>
        public virtual void FixedUpdate()
        {

        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        public virtual void LateUpdate()
        {
            CheckTrasitionConditions();
        }

        /// <summary>
        /// This function is called when the AIBehaviour becomes disabled.
        /// </summary>
        public virtual void Stop()
        {
            DisableTransitionConditions();
        }

        /// <summary>
        /// Calling OnEnable method on each transition condition.
        /// </summary>
        protected void EnableTransitionConditions()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                transitions[i].EnableConditions();
            }
        }

        /// <summary>
        /// Calling OnDisable method on each transition condition.
        /// </summary>
        protected void DisableTransitionConditions()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                transitions[i].DisableConditions();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CheckTrasitionConditions()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                Transition transition = transitions[i];
                if (transition.MakeTransition())
                {
                    core.SwitchBehaviour(transition.GetNextBehaviour());
                }
            }
        }

        public void AddTransition(Transition transition)
        {
            transitions.Add(transition);
        }

        public bool RemoveTransition(Transition transition)
        {
            return transitions.Remove(transition);
        }

        public void RemoveTransition(int index)
        {
            transitions.RemoveAt(index);
        }

        public void RemoveAllTransitions()
        {
            transitions.Clear();
        }

        #region [Getter / Setter]
        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }

        public List<Transition> GetTransitions()
        {
            return transitions;
        }

        public void SetTransitions(List<Transition> value)
        {
            transitions = value;
        }

        public Transition GetTransition(int index)
        {
            return transitions[index];
        }

        public void SetTransition(int index, Transition value)
        {
            transitions[index] = value;
        }

        public int GetTransitionCount()
        {
            return transitions.Count;
        }

        public UnityEvent GetStartEvent()
        {
            return startEvent;
        }

        public void SetStartEvent(UnityEvent value)
        {
            startEvent = value;
        }

        public AICore GetCore()
        {
            return core;
        }

        protected void SetCore(AICore value)
        {
            core = value;
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