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

namespace AuroraFPSRuntime
{
    /// <summary>
    /// Base coroutine object implementation.
    /// </summary>
    public sealed class CoroutineObject : CoroutineObjectBase
    {
        // Coroutine object properties.
        private Func<IEnumerator> routine;

        public CoroutineObject(MonoBehaviour owner) : base(owner) { }

        /// <summary>
        /// Coroutine process implementation.
        /// </summary>
        private IEnumerator Process()
        {
            yield return routine.Invoke();
            coroutine = null;
        }

        /// <summary>
        /// Start coroutine processing.
        /// </summary>
        public bool Start(Func<IEnumerator> routine, bool force = false)
        {
            if (IsProcessing() && !force)
            {
                return false;
            }
            Stop();
            this.routine = routine;
            coroutine = owner.StartCoroutine(Process());
            return true;
        }

        /// <summary>
        /// Stop coroutine processing.
        /// </summary>
        public bool Stop()
        {
            if (IsProcessing())
            {
                owner.StopCoroutine(coroutine);
                coroutine = null;
                return true;
            }
            return false;
        }

        #region [Getter / Setter]
        public Func<IEnumerator> GetRoutine()
        {
            return routine;
        }

        private void SetRoutine(Func<IEnumerator> value)
        {
            routine = value;
        }
        #endregion
    }

    /// <summary>
    /// Coroutine object with generic parameter implementation.
    /// </summary>
    public sealed class CoroutineObject<T> : CoroutineObjectBase
    {
        // Coroutine object properties.
        private Func<T, IEnumerator> routine;

        /// <summary>
        /// Constructor of CoroutineObject.
        /// </summary>
        public CoroutineObject(MonoBehaviour owner) : base(owner) { }

        /// <summary>
        /// Coroutine process implementation.
        /// </summary>
        private IEnumerator Process(T arg)
        {
            yield return routine.Invoke(arg);
            coroutine = null;
        }

        /// <summary>
        /// Start coroutine processing.
        /// </summary>
        public bool Start(Func<T, IEnumerator> routine, T arg, bool force = false)
        {
            if (IsProcessing() && !force)
            {
                return false;
            }
            Stop();
            this.routine = routine;
            coroutine = owner.StartCoroutine(Process(arg));
            return true;
        }

        /// <summary>
        /// Stop coroutine processing.
        /// </summary>
        public bool Stop()
        {
            if (IsProcessing())
            {
                owner.StopCoroutine(coroutine);
                coroutine = null;
                return true;
            }
            return false;
        }

        #region [Getter / Setter]
        public Func<T, IEnumerator> GetRoutine()
        {
            return routine;
        }

        private void SetRoutine(Func<T, IEnumerator> value)
        {
            routine = value;
        }
        #endregion
    }

    /// <summary>
    /// Coroutine object with two generic parameter implementation.
    /// </summary>
    public sealed class CoroutineObject<T1, T2> : CoroutineObjectBase
    {
        // Coroutine object properties.
        private Func<T1, T2, IEnumerator> routine;

        /// <summary>
        /// Constructor of CoroutineObject.
        /// </summary>
        public CoroutineObject(MonoBehaviour owner) : base(owner) { }

        /// <summary>
        /// Coroutine process implementation.
        /// </summary>
        private IEnumerator Process(T1 arg1, T2 arg2)
        {
            yield return routine.Invoke(arg1, arg2);
            coroutine = null;
        }

        /// <summary>
        /// Start coroutine processing.
        /// </summary>
        public bool Start(Func<T1,T2,  IEnumerator> routine, T1 arg1, T2 arg2,  bool force = false)
        {
            if (IsProcessing() && !force)
            {
                return false;
            }
            Stop();
            this.routine = routine;
            coroutine = owner.StartCoroutine(Process(arg1, arg2));
            return true;
        }

        /// <summary>
        /// Stop coroutine processing.
        /// </summary>
        public bool Stop()
        {
            if (IsProcessing())
            {
                owner.StopCoroutine(coroutine);
                coroutine = null;
                return true;
            }
            return false;
        }

        #region [Getter / Setter]
        public Func<T1, T2, IEnumerator> GetRoutine()
        {
            return routine;
        }

        private void SetRoutine(Func<T1, T2, IEnumerator> value)
        {
            routine = value;
        }
        #endregion
    }
}