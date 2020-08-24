/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;
using InvokeTime = AuroraFPSRuntime.AI.DestinationEvent.InvokeTime;

namespace AuroraFPSRuntime.AI
{
    [AIBehaviourOptions(path = "Movement/Walking", priority = 1)]
    public class AIWalkingBehaviour : AIBehaviour
    {
        public enum WalkingType
        {
            /// <summary>
            /// Constantly selected at random from destination info array a new destination point.
            /// </summary>
            Random,

            /// <summary>
            /// Walking through all the destination points in turn circle after circle.
            /// </summary>
            Sequential,

            /// <summary>
            /// Walking through all the destination points and stop on the last.
            /// </summary>
            Finite
        }

        // Base AI walking movement properties. 
        [SerializeField] private WalkingType walkingType;
        [SerializeField] private DestinationMap destinationMap;
        [SerializeField] private DestinationEvent[] destinationEvents;

        // Stored required properties.
        private int index;
        private CoroutineObject walkingProcessing;

        /// <summary>
        /// Initiailze is called when the script instance is being loaded.
        /// </summary>
        public override void Initialize(AICore core)
        {
            base.Initialize(core);
            walkingProcessing = new CoroutineObject(core);
        }

        /// <summary>
        /// This function is called when the AIWalkingBehaviour becomes enabled.
        /// </summary>
        public override void Start()
        {
            base.Start();
            navMeshAgent.isStopped = false;
            walkingProcessing.Start(WalkingProcessing);
        }

        /// <summary>
        /// AI walking processing coroutine.
        /// 
        /// Automatically started when the behaviour becomes enabled.
        /// Automatically stopped when the behaviour becomes disabled.
        /// </summary>
        public virtual IEnumerator WalkingProcessing()
        {
            WaitForReach waitForReach = new WaitForReach(navMeshAgent);
            waitForReach.OnStartedCallback += _ => navMeshAgent.SetDestination(GetDestination());
            waitForReach.OnDoneCallback += _ => navMeshAgent.SetDestination(GetDestination());

            while (true)
            {
                if (walkingType == WalkingType.Finite && index == destinationMap.GetCount() - 1)
                    yield break;
                else
                    yield return waitForReach;

                DestinationEvent destinationEvent = GetDestinationEvent(index);
                if (destinationEvent != null)
                {
                    destinationEvent.InvokeEvent(InvokeTime.OnReadched);

                    if (destinationEvent.GetDelay() > 0)
                        yield return new WaitForSeconds(destinationEvent.GetDelay());

                    destinationEvent.InvokeEvent(InvokeTime.OnComplete);
                }
            }
        }

        /// <summary>
        /// This function is called when the AIWalkingBehaviour becomes disabled.
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            walkingProcessing.Stop();
        }

        /// <summary>
        /// Get destination position by walking type.
        /// </summary>
        /// <returns>Destination position by walking type.</returns>
        private Vector3 GetDestination()
        {
            switch (walkingType)
            {
                case WalkingType.Random:
                    index = Random.Range(0, destinationMap.GetCount() - 1);
                    break;
                case WalkingType.Sequential:
                    index = index < destinationMap.GetCount() - 1 ? index + 1 : 0;
                    break;
                case WalkingType.Finite:
                    index = index < destinationMap.GetCount() - 1 ? index + 1 : 0;
                    break;
            }
            return destinationMap.GetDestination(index).position;
        }

        /// <summary>
        /// Get destination position by index and walking type.
        /// </summary>
        /// <param name="index">Current index reference.</param>
        /// <returns>Destination position by index and walking type.</returns>
        private Vector3 GetLastDestination()
        {
            return destinationMap.GetDestination(index).position;
        }

        /// <summary>
        /// Get destination event by overrided index.
        /// </summary>
        /// <param name="index">Detination index.</param>
        /// <returns>
        /// If destination overrided return event.
        /// Otherwise null.
        /// </returns>
        private DestinationEvent GetDestinationEvent(int index)
        {
            for (int i = 0, length = destinationEvents.Length; i < length; i++)
            {
                DestinationEvent destinationEvent = destinationEvents[i];
                if (destinationEvent.GetIndex() == index)
                {
                    return destinationEvent;
                }
            }
            return null;
        }

        #region [Getter / Setter]
        public WalkingType GetWalkingType()
        {
            return walkingType;
        }

        public void SetWalkingType(WalkingType value)
        {
            walkingType = value;
        }

        public DestinationMap GetDestinationMap()
        {
            return destinationMap;
        }

        public void SetDestinationMap(DestinationMap value)
        {
            destinationMap = value;
        }

        public DestinationEvent[] GetDestinationEvents()
        {
            return destinationEvents;
        }

        public void SetDestinationEvents(DestinationEvent[] value)
        {
            destinationEvents = value;
        }
        #endregion
    }
}