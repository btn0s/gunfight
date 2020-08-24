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

namespace AuroraFPSRuntime.AI
{
    [Serializable]
    public struct Transition : IEquatable<Transition>
    {
        [SerializeField] private string nextBehaviour;
        [SerializeReference] private List<Condition> conditions;
        [SerializeField] private bool mute;


        /// <summary>
        /// Transition constructor.
        /// </summary>
        /// <param name="nextBehaviour">Next behaviour to Transition.</param>
        /// <param name="conditions">Conditions to translate to the next behaviour.</param>
        public Transition(string nextBehaviour, List<Condition> conditions)
        {
            this.nextBehaviour = nextBehaviour;
            this.conditions = conditions;
            this.mute = false;
        }

         /// <summary>
        /// Transition constructor.
        /// </summary>
        /// <param name="nextBehaviour">Next behaviour to Transition.</param>
        /// <param name="conditions">Conditions to translate to the next behaviour.</param>
        /// <param name="mute">Transition is muted.</param>
        public Transition(string nextBehaviour, List<Condition> conditions, bool mute)
        {
            this.nextBehaviour = nextBehaviour;
            this.conditions = conditions;
            this.mute = mute;
        }
        
        /// <summary>
        /// Calling OnEnable method on each condition.
        /// </summary>
        public void EnableConditions()
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                conditions[i].OnEnable();
            }
        }

        /// <summary>
        /// Calling OnDisable method on each condition.
        /// </summary>
        public void DisableConditions()
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                conditions[i].OnDisable();
            }
        }
        
        public bool MakeTransition()
        {
            if(mute)
            {
                return false;
            }

            for (int i = 0; i < conditions.Count; i++)
            {
                Condition condition = conditions[i];

                if (condition.IsMuted())
                {
                    continue;
                }

                if (!conditions[i].IsExecuted())
                {
                    return false;
                }
            }
            return true;
        }

        public void AddCondition(Condition condition)
        {
            conditions.Add(condition);
        }

        public bool RemoveCondition(Condition condition)
        {
            return conditions.Remove(condition);
        }

        public void RemoveCondition(int index)
        {
            conditions.RemoveAt(index);
        }

        #region [Override type]
        public Condition this [int index]
        {
            get
            {
                return conditions[index];
            }
            set
            {
                conditions[index] = value;
            }
        }

        public static bool operator ==(Transition left, Transition right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Transition left, Transition right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is Transition metrics) && Equals(metrics);
        }

        public bool Equals(Transition other)
        {
            return (nextBehaviour, conditions) == (other.nextBehaviour, other.conditions);
        }

        public override int GetHashCode()
        {
            return (nextBehaviour, conditions).GetHashCode();
        }
        #endregion

        #region [Getter / Setter]
        public string GetNextBehaviour()
        {
            return nextBehaviour;
        }

        public void SetNextBehaviour(string value)
        {
            nextBehaviour = value;
        }

        public bool IsMuted()
        {
            return mute;
        }

        public void SetMute(bool value)
        {
            mute = value;
        }

        public List<Condition> GetConditions()
        {
            return conditions;
        }

        public void SetConditions(List<Condition> value)
        {
            conditions = value;
        }

        public Condition GetCondition(int index)
        {
            return conditions[index];
        }

        public void SetCondition(int index, Condition value)
        {
            conditions[index] = value;
        }

        public int GetConditionLength()
        {
            return conditions.Count;
        }
        #endregion
    }
}