/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using System.Collections.Generic;

namespace AuroraFPSRuntime.AI
{
    public class DestinationMap : MonoBehaviour
    {
        // Base destination map properties
        [SerializeField] private List<Transform> destinations;

        #region [Getter / Setter]
        public List<Transform> GetDestinations()
        {
            return destinations;
        }

        public Transform GetDestination(int index)
        {
            return destinations[index];
        }

        public void SetDestinations(List<Transform> value)
        {
            destinations = value;
        }

        public void SetDestination(int index, Transform destination)
        {
            destinations[index] = destination;
        }

        public void SetDestination(int index, Vector3 position)
        {
            destinations[index].position = position;
        }

        public int GetCount()
        {
            return destinations?.Count ?? 0;
        }
        #endregion
    }
}