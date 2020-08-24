/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using AuroraFPSRuntime.AI;
using UnityEngine;

namespace AuroraFPSRuntime.Serialization.Collections
{
    #region [Represents Of Serialized Dictionary Classes]
    /// <summary>
    /// Represents base class for serialized Dictionary(string, string).
    /// </summary>
    [System.Serializable]
    public class DictionaryStringToString : SerializableDictionary<string, string>
    {
        [SerializeField] private string[] keys;
        [SerializeField] private string[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(string, string) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryStringToString() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, string) class that contains elements copied from the specified IDictionary(string, string) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(string, string) whose elements are copied to the new Dictionary(string, string).</param>
        public DictionaryStringToString(IDictionary<string, string> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, string) class that contains elements copied from the specified IDictionary(string, string) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToString(IEqualityComparer<string> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, string) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, string) can contain.</param>
        public DictionaryStringToString(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, string) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(string, string) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToString(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, string) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, string) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToString(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override string[] GetKeys()
        {
            return keys;
        }

        protected override string[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(string[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(string[] values)
        {
            this.values = values;
        }
        #endregion
    }

    /// <summary>
    /// Represents base class for serialized Dictionary(string, KeyCode).
    /// </summary>
    [System.Serializable]
    public class DictionaryStringToKeyCode : SerializableDictionary<string, KeyCode>
    {
        [SerializeField] private string[] keys;
        [SerializeField] private KeyCode[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(string, KeyCode) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryStringToKeyCode() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, KeyCode) class that contains elements copied from the specified IDictionary(string, KeyCode) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(string, KeyCode) whose elements are copied to the new Dictionary(string, KeyCode).</param>
        public DictionaryStringToKeyCode(IDictionary<string, KeyCode> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, KeyCode) class that contains elements copied from the specified IDictionary(string, KeyCode) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToKeyCode(IEqualityComparer<string> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, KeyCode) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, KeyCode) can contain.</param>
        public DictionaryStringToKeyCode(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, KeyCode) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(string, KeyCode) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToKeyCode(IDictionary<string, KeyCode> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, KeyCode) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, KeyCode) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToKeyCode(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override string[] GetKeys()
        {
            return keys;
        }

        protected override KeyCode[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(string[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(KeyCode[] values)
        {
            this.values = values;
        }
        #endregion
    }

    /// <summary>
    /// Represents base class for serialized Dictionary(Object, AudioClipArrayStorage).
    /// </summary>
    [System.Serializable]
    public class DictionaryObjectToArrayAudioClip : SerializableDictionary<Object, AudioClipStorage>
    {
        [SerializeField] private Object[] keys;
        [SerializeField] private AudioClipStorage[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, ArrayAudioClip) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryObjectToArrayAudioClip() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, ArrayAudioClip) class that contains elements copied from the specified IDictionary(Object, ArrayAudioClip) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(Object, ArrayAudioClip) whose elements are copied to the new Dictionary(Object, ArrayAudioClip).</param>
        public DictionaryObjectToArrayAudioClip(IDictionary<Object, AudioClipStorage> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, ArrayAudioClip) class that contains elements copied from the specified IDictionary(Object, ArrayAudioClip) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToArrayAudioClip(IEqualityComparer<Object> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, ArrayAudioClip) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(Object, ArrayAudioClip) can contain.</param>
        public DictionaryObjectToArrayAudioClip(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, ArrayAudioClip) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(Object, ArrayAudioClip) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToArrayAudioClip(IDictionary<Object, AudioClipStorage> dictionary, IEqualityComparer<Object> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, ArrayAudioClip) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(Object, ArrayAudioClip) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToArrayAudioClip(int capacity, IEqualityComparer<Object> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override Object[] GetKeys()
        {
            return keys;
        }

        protected override AudioClipStorage[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(Object[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(AudioClipStorage[] values)
        {
            this.values = values;
        }
        #endregion
    }

    /// <summary>
    /// Represents base class for serialized Dictionary(Object, FootstepProperty).
    /// </summary>
    [System.Serializable]
    public class DictionaryObjectToFootstepProperty : SerializableDictionary<Object, FootstepSounds>
    {
        [SerializeField] private Object[] keys;
        [SerializeField] private FootstepSounds[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, FootstepProperty) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryObjectToFootstepProperty() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, FootstepProperty) class that contains elements copied from the specified IDictionary(Object, FootstepProperty) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(Object, FootstepProperty) whose elements are copied to the new Dictionary(Object, FootstepProperty).</param>
        public DictionaryObjectToFootstepProperty(IDictionary<Object, FootstepSounds> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, FootstepProperty) class that contains elements copied from the specified IDictionary(Object, FootstepProperty) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToFootstepProperty(IEqualityComparer<Object> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, FootstepProperty) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(Object, FootstepProperty) can contain.</param>
        public DictionaryObjectToFootstepProperty(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, FootstepProperty) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(Object, FootstepProperty) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToFootstepProperty(IDictionary<Object, FootstepSounds> dictionary, IEqualityComparer<Object> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, FootstepProperty) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(Object, FootstepProperty) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToFootstepProperty(int capacity, IEqualityComparer<Object> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override Object[] GetKeys()
        {
            return keys;
        }

        protected override FootstepSounds[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(Object[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(FootstepSounds[] values)
        {
            this.values = values;
        }
        #endregion
    }

    /// <summary>
    /// Represents base class for serialized Dictionary(string, PoolContainer).
    /// </summary>
    [System.Serializable]
    public class DictionaryStringToPoolContainer : SerializableDictionary<string, PoolContainer>
    {
        [SerializeField] private string[] keys;
        [SerializeField] private PoolContainer[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(string, PoolContainer) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryStringToPoolContainer() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, PoolContainer) class that contains elements copied from the specified IDictionary(string, PoolContainer) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(string, PoolContainer) whose elements are copied to the new Dictionary(string, PoolContainer).</param>
        public DictionaryStringToPoolContainer(IDictionary<string, PoolContainer> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, PoolContainer) class that contains elements copied from the specified IDictionary(string, PoolContainer) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToPoolContainer(IEqualityComparer<string> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, PoolContainer) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, PoolContainer) can contain.</param>
        public DictionaryStringToPoolContainer(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, PoolContainer) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(string, PoolContainer) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToPoolContainer(IDictionary<string, PoolContainer> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, PoolContainer) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, PoolContainer) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToPoolContainer(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override string[] GetKeys()
        {
            return keys;
        }

        protected override PoolContainer[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(string[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(PoolContainer[] values)
        {
            this.values = values;
        }
        #endregion
    }

    /// <summary>
    /// Represents base class for serialized Dictionary(Object, PoolObjectStorage).
    /// </summary>
    [System.Serializable]
    public class DictionaryObjectToPoolObjectStorage : SerializableDictionary<Object, PoolObjectStorage>
    {
        [SerializeField] private Object[] keys;
        [SerializeField] private PoolObjectStorage[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, PoolObjectStorage) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryObjectToPoolObjectStorage() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, PoolObjectStorage) class that contains elements copied from the specified IDictionary(Object, PoolObjectStorage) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(Object, PoolObjectStorage) whose elements are copied to the new Dictionary(Object, PoolObjectStorage).</param>
        public DictionaryObjectToPoolObjectStorage(IDictionary<Object, PoolObjectStorage> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, PoolObjectStorage) class that contains elements copied from the specified IDictionary(Object, PoolObjectStorage) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToPoolObjectStorage(IEqualityComparer<Object> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, PoolObjectStorage) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(Object, PoolObjectStorage) can contain.</param>
        public DictionaryObjectToPoolObjectStorage(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, PoolObjectStorage) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(Object, PoolObjectStorage) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToPoolObjectStorage(IDictionary<Object, PoolObjectStorage> dictionary, IEqualityComparer<Object> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(Object, PoolObjectStorage) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(Object, PoolObjectStorage) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryObjectToPoolObjectStorage(int capacity, IEqualityComparer<Object> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override Object[] GetKeys()
        {
            return keys;
        }

        protected override PoolObjectStorage[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(Object[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(PoolObjectStorage[] values)
        {
            this.values = values;
        }
        #endregion
    }

    /// <summary>
    /// Represents base class for serialized Dictionary(string, ShakeCamera.ShakeProperties).
    /// </summary>
    [System.Serializable]
    public class DictionaryStringToShake : SerializableDictionary<string, CameraShake.Shake>
    {
        [SerializeField] private string[] keys;
        [SerializeField] private CameraShake.Shake[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(string, ShakeCamera.ShakeProperties) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryStringToShake() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, ShakeCamera.ShakeProperties) class that contains elements copied from the specified IDictionary(string, ShakeCamera.ShakeProperties) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(string, ShakeCamera.ShakeProperties) whose elements are copied to the new Dictionary(string, ShakeCamera.ShakeProperties).</param>
        public DictionaryStringToShake(IDictionary<string, CameraShake.Shake> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, ShakeCamera.ShakeProperties) class that contains elements copied from the specified IDictionary(string, ShakeCamera.ShakeProperties) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToShake(IEqualityComparer<string> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, ShakeCamera.ShakeProperties) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, ShakeCamera.ShakeProperties) can contain.</param>
        public DictionaryStringToShake(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, ShakeCamera.ShakeProperties) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(string, ShakeCamera.ShakeProperties) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToShake(IDictionary<string, CameraShake.Shake> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, ShakeCamera.ShakeProperties) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, ShakeCamera.ShakeProperties) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToShake(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override string[] GetKeys()
        {
            return keys;
        }

        protected override CameraShake.Shake[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(string[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(CameraShake.Shake[] values)
        {
            this.values = values;
        }
        #endregion
    }

    /// <summary>
    /// Represents base class for serialized Dictionary(string, AIBehaviour).
    /// </summary>
    [System.Serializable]
    public class DictionaryStringToAIBehaviour : SerializableDictionary<string, AIBehaviour>
    {
        [SerializeField] private string[] keys;
        [SerializeReference] private AIBehaviour[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(string, AIBehaviour) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryStringToAIBehaviour() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, AIBehaviour) class that contains elements copied from the specified IDictionary(string, AIBehaviour) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(string, AIBehaviour) whose elements are copied to the new Dictionary(string, AIBehaviour).</param>
        public DictionaryStringToAIBehaviour(IDictionary<string, AIBehaviour> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, AIBehaviour) class that contains elements copied from the specified IDictionary(string, AIBehaviour) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToAIBehaviour(IEqualityComparer<string> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, AIBehaviour) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, AIBehaviour) can contain.</param>
        public DictionaryStringToAIBehaviour(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, AIBehaviour) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(string, AIBehaviour) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToAIBehaviour(IDictionary<string, AIBehaviour> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(string, AIBehaviour) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(string, AIBehaviour) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryStringToAIBehaviour(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override string[] GetKeys()
        {
            return keys;
        }

        protected override AIBehaviour[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(string[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(AIBehaviour[] values)
        {
            this.values = values;
        }
        #endregion
    }

    /// <summary>
    /// Represents base class for serialized Dictionary(ControllerState, RecoilPattern).
    /// </summary>
    [System.Serializable]
    public class DictionaryControllerStateToRecolPattern : SerializableDictionary<ControllerState, RecoilPattern>
    {
        [SerializeField] private ControllerState[] keys;
        [SerializeField] private RecoilPattern[] values;

        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Dictionary(ControllerState, RecoilPattern) class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryControllerStateToRecolPattern() : base() { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(ControllerState, RecoilPattern) class that contains elements copied from the specified IDictionary(ControllerState, RecoilPattern) and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary(ControllerState, RecoilPattern) whose elements are copied to the new Dictionary(ControllerState, RecoilPattern).</param>
        public DictionaryControllerStateToRecolPattern(IDictionary<ControllerState, RecoilPattern> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(ControllerState, RecoilPattern) class that contains elements copied from the specified IDictionary(ControllerState, RecoilPattern) and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryControllerStateToRecolPattern(IEqualityComparer<ControllerState> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(ControllerState, RecoilPattern) class that is empty, has the default initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(ControllerState, RecoilPattern) can contain.</param>
        public DictionaryControllerStateToRecolPattern(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(ControllerState, RecoilPattern) class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The initial number of elements that the Dictionary(ControllerState, RecoilPattern) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryControllerStateToRecolPattern(IDictionary<ControllerState, RecoilPattern> dictionary, IEqualityComparer<ControllerState> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the Dictionary(ControllerState, RecoilPattern) class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer(T).
        /// </summary>
        /// <param name="capacity">The initial number of elements that the Dictionary(ControllerState, RecoilPattern) can contain.</param>
        /// <param name="comparer">The IEqualityComparer(T) implementation to use when comparing keys, or null to use the default EqualityComparer(T) for the type of the key.</param>
        public DictionaryControllerStateToRecolPattern(int capacity, IEqualityComparer<ControllerState> comparer) : base(capacity, comparer) { }
        #endregion

        #region [SerializableDictionary Abstract Implementation]
        protected override ControllerState[] GetKeys()
        {
            return keys;
        }

        protected override RecoilPattern[] GetValues()
        {
            return values;
        }

        protected override void SetKeys(ControllerState[] keys)
        {
            this.keys = keys;
        }

        protected override void SetValues(RecoilPattern[] values)
        {
            this.values = values;
        }
        #endregion
    }
    #endregion

    #region [Represents Of Serialized Storage Classes]
    /// <summary>
    /// Represents base class for serialized array storage AudioClip.
    /// </summary>
    [System.Serializable]
    public class AudioClipStorage : Storage<AudioClip[]> { }

    /// <summary>
    /// Represents base class for serialized array storage for PoolObject.
    /// </summary>
    [System.Serializable]
    public class PoolObjectStorage : Storage<PoolObject[]> { }
    #endregion
}