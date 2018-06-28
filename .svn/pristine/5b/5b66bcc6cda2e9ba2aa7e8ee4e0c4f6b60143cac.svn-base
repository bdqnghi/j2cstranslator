// 
// J2CsMapping : runtime library for J2CsTranslator
// 
// Copyright (c) 2008-2010 Alexandre FAU.
// All rights reserved. This program and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html
// Contributors:
//   Alexandre FAU (IBM)
//

namespace ILOG.J2CsMapping.Collections.Generics
{
    using Collections = ILOG.J2CsMapping.Collections.Generics.Collections;
    using Hashtable = System.Collections.Hashtable;
    using ICollection = System.Collections.ICollection;
    using IIterator = ILOG.J2CsMapping.Collections.IIterator;
    using ILOG.J2CsMapping.Collections.Generics;
    using ILOG.J2CsMapping.Collections;
    using ILOG.J2CsMapping.IO;
    using ISet = ILOG.J2CsMapping.Collections.ISet;
    using System.Collections.Generic;
    using System.Collections;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml;
    using System;
    using System.Text;

    /// <summary>
    /// .NET Replacement for Java EnumMap
    /// </summary>
    public class EnumMap<K, V> : AbstractMap<K, V>, ICloneable
    {
        private Type keyType;

        Object[] keys;

        Object[] values;

        bool[] hasMapping;

        private int mappingsCount;

        int enumSize;

        private EnumMapEntrySet<K, V> entrySet = null;

        private class Entry<KT, VT> //: KeyValuePair<KT, VT>
        {
            private KT theKey = default(KT);
            private VT theValue = default(VT);

            private EnumMap<KT, VT> enumMap;

            private int ordinal;

            public Entry(KT theKey, VT theValue, EnumMap<KT, VT> em) //: base(theKey, theValue)
            {
                enumMap = em;
                ordinal = (int)(object)theKey;
                this.theKey = theKey;
                this.theValue = theValue;
            }


            public override bool Equals(Object obj)
            {
                if (!enumMap.hasMapping[ordinal])
                {
                    return false;
                }
                bool isEqual = false;
                if (obj is KeyValuePair<KT, VT>)
                {
                    KeyValuePair<KT, VT> entry = (KeyValuePair<KT, VT>)obj;
                    Object enumKey = entry.Key;
                    if (theKey.Equals(enumKey))
                    {
                        Object theValue = entry.Value;
                        isEqual = enumMap.values[ordinal] == null ? null == theValue
                                : enumMap.values[ordinal].Equals(theValue);
                    }
                }
                return isEqual;
            }

            public override int GetHashCode()
            {
                return (enumMap.keys[ordinal] == null ? 0 : enumMap.keys[ordinal]
                        .GetHashCode())
                        ^ (enumMap.values[ordinal] == null ? 0
                                : enumMap.values[ordinal].GetHashCode());
            }

            public KT GetKey()
            {
                checkEntryStatus();
                return (KT)enumMap.keys[ordinal];
            }

            public VT GetValue()
            {
                checkEntryStatus();
                return (VT)enumMap.values[ordinal];
            }

            public VT setValue(VT value)
            {
                checkEntryStatus();
                return enumMap.Put((KT)enumMap.keys[ordinal], value);
            }

            public String toString()
            {
                StringBuilder result = new StringBuilder(enumMap.keys[ordinal]
                        .ToString());
                result.Append("="); //$NON-NLS-1$
                result.Append(enumMap.values[ordinal].ToString());
                return result.ToString();
            }

            public KeyValuePair<KT, VT> GetKeyValue()
            {
                return new KeyValuePair<KT, VT>(GetKey(), GetValue());
            }

            private void checkEntryStatus()
            {
                if (!enumMap.hasMapping[ordinal])
                {
                    throw new Exception();
                }
            }
        }


        interface MapEntryType<RT, KT, VT>
        {
            RT Get(KeyValuePair<KT, VT> entry);
        }


        private class EnumMapIterator<E, KT, VT> : IIterator<E>
        {
            protected int position = 0;

            protected int prePosition = -1;

            protected EnumMap<KT, VT> enumMap;

            protected MapEntryType<E, KT, VT> type;

            public EnumMapIterator(MapEntryType<E, KT, VT> value, EnumMap<KT, VT> em)
            {
                enumMap = em;
                type = value;
            }

            public virtual bool HasNext()
            {
                int length = enumMap.enumSize;
                for (; position < length; position++)
                {
                    if (enumMap.hasMapping[position])
                    {
                        break;
                    }
                }
                return position != length;
            }

            public virtual E Next()
            {
                if (!HasNext())
                {
                    throw new Exception("NoSuchElementException");
                }
                prePosition = position++;
                return type.Get(new KeyValuePair<KT, VT>((KT)enumMap.keys[prePosition],
                        (VT)enumMap.values[prePosition]));
            }

            public void Remove()
            {
                CheckStatus();
                if (enumMap.hasMapping[prePosition])
                {
                    enumMap.Remove((KT)enumMap.keys[prePosition]);
                }
                prePosition = -1;
            }

            public override String ToString()
            {
                if (-1 == prePosition)
                {
                    return base.ToString();
                }
                return type.Get(
                        new KeyValuePair<KT, VT>((KT)enumMap.keys[prePosition],
                                (VT)enumMap.values[prePosition])).ToString();
            }

            private void CheckStatus()
            {
                if (-1 == prePosition)
                {
                    throw new Exception("IllegalStateException");
                }
            }


            #region IIterator Members

            bool IIterator.HasNext()
            {
                throw new NotImplementedException();
            }

            object IIterator.Next()
            {
                throw new NotImplementedException();
            }

            void IIterator.Remove()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        private class EnumMapKeySet<KT, VT> :
                AbstractSet<KT>
        {
            private EnumMap<KT, VT> enumMap;

            public EnumMapKeySet(EnumMap<KT, VT> em)
            {
                enumMap = em;
            }

            public override void Clear()
            {
                enumMap.Clear();
            }

            public bool Contains(Object obj)
            {
                return enumMap.ContainsKey((KT)obj);
            }

            public override IIterator<KT> Iterator()
            {
                return new EnumMapIterator<KT, KT, VT>(
                        new IteratorAnonClass<KT, VT>(), enumMap);
                //throw new NotImplementedException("");
            }

            class IteratorAnonClass<KT2, VT2> : MapEntryType<KT2, KT2, VT2>
            {
                public KT2 Get(KeyValuePair<KT2, VT2> entry)
                {
                    return entry.Key;
                }
            }

            public override bool Remove(Object obj)
            {
                if (Contains(obj))
                {
                    enumMap.Remove((KT)obj);
                    return true;
                }
                return false;
            }

            public override int Count
            {
                get { return enumMap.Count; }
            }

            public override KT[] ToArray(KT[] arr)
            {
                throw new NotImplementedException();
            }
        }

        protected class EnumMapValueCollection<KT, VT> : AbstractCollection<VT>
        {
            private EnumMap<KT, VT> enumMap;

            public EnumMapValueCollection(EnumMap<KT, VT> em)
            {
                enumMap = em;
            }

            public override void Clear()
            {
                enumMap.Clear();
            }

            public bool Contains(Object obj)
            {
                return enumMap.ContainsValue((VT)obj);
            }

            public override IIterator<VT> Iterator()
            {
                return new EnumMapIterator<VT, KT, VT>(
                         new IteratorAnonClass<KT, VT>(), enumMap);
            }


            class IteratorAnonClass<KT2, VT2> : MapEntryType<VT2, KT2, VT2>
            {
                public VT2 Get(KeyValuePair<KT2, VT2> entry)
                {
                    return entry.Value;
                }
            }

            public override bool Remove(Object obj)
            {
                if (null == obj)
                {
                    for (int i = 0; i < enumMap.enumSize; i++)
                    {
                        if (enumMap.hasMapping[i] && null == enumMap.values[i])
                        {
                            enumMap.Remove((KT)enumMap.keys[i]);
                            return true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < enumMap.enumSize; i++)
                    {
                        if (enumMap.hasMapping[i]
                                && obj.Equals(enumMap.values[i]))
                        {
                            enumMap.Remove((KT)enumMap.keys[i]);
                            return true;
                        }
                    }
                }
                return false;
            }

            public override int Count
            {
                get { return enumMap.Count; }
            }
        }

        private class EnumMapEntryIterator<E, KT, VT>
                : EnumMapIterator<E, KT, VT>
        {
            public EnumMapEntryIterator(MapEntryType<E, KT, VT> value, EnumMap<KT, VT> em)
                : base(value, em)
            {
            }

            public override E Next()
            {
                if (!HasNext())
                {
                    throw new Exception("NoSuchElementException");
                }
                prePosition = position++;
                return type.Get(new Entry<KT, VT>((KT)enumMap.keys[prePosition],
                        (VT)enumMap.values[prePosition], enumMap).GetKeyValue());
            }
        }

        private class EnumMapEntrySet<KT, VT> :
                AbstractSet<KeyValuePair<KT, VT>>
        {
            private EnumMap<KT, VT> enumMap;

            public EnumMapEntrySet(EnumMap<KT, VT> em)
            {
                enumMap = em;
            }

            public override void Clear()
            {
                enumMap.Clear();
            }

            public override bool Contains(KeyValuePair<KT, VT> obj)
            {
                bool isEqual = false;
                if (obj is KeyValuePair<K, V>)
                {
                    Object enumKey = ((KeyValuePair<KT, VT>)obj).Key;
                    Object enumValue = ((KeyValuePair<KT, VT>)obj).Value;
                    if (enumMap.ContainsKey((KT)enumKey))
                    {
                        VT value = enumMap.Get(enumKey);
                        isEqual = (value == null ? null == enumValue : value
                                .Equals(enumValue));
                    }
                }
                return isEqual;
            }

            public override IIterator<KeyValuePair<KT, VT>> Iterator()
            {
                return new EnumMapEntryIterator<KeyValuePair<KT, VT>, KT, VT>(
                        new IteratorAnonClass<KT, VT>(), enumMap);
            }

            class IteratorAnonClass<KT2, VT2> : MapEntryType<KeyValuePair<KT2, VT2>, KT2, VT2>
            {
                public KeyValuePair<KT2, VT2> Get(KeyValuePair<KT2, VT2> entry)
                {
                    return entry;
                }
            }

            public override bool Remove(KeyValuePair<KT, VT> obj)
            {
                if (Contains(obj))
                {
                    enumMap.Remove(((KeyValuePair<KT, VT>)obj).Key);
                    return true;
                }
                return false;
            }

            public override int Count
            {
                get
                {
                    return enumMap.Count;
                }
            }

            public virtual Object[] ToArray()
            {
                Object[] entryArray = new Object[enumMap.Count];
                return ToArray(entryArray);
            }

            public Object[] ToArray(Object[] array)
            {
                int size = enumMap.Count;
                int index = 0;
                Object[] entryArray = array;
                if (size > array.Length)
                {
                    Type clazz = array.GetType().GetElementType();
                    entryArray = (Object[])ILOG.J2CsMapping.Collections.Arrays.NewInstance(clazz, size);
                }
                IIterator<KeyValuePair<KT, VT>> iter = Iterator();
                for (; index < size; index++)
                {
                    KeyValuePair<KT, VT> entry = iter.Next();
                    entryArray[index] = new KeyValuePair<KT, VT>(entry.Key, entry
                            .Value);
                }
                if (index < array.Length)
                {
                    entryArray[index] = null;
                }
                return entryArray;
            }
        }

        /// <summary>
        /// Constructs an empty EnumMap.
        /// </summary>
        /// <param name="keyType">the Class that is to be used for the key type for this map</param>
        public EnumMap(Type keyType)
        {
            Initialization(keyType);
        }

        /// <summary>
        /// Constructs an EnumMap using the same key type and contents as the given
        /// EnumMap.
        /// </summary>
        /// <param name="map">the EnumMap from which the initial contents of this EnumMap are copied</param>
        public EnumMap(EnumMap<K, V> map) /*where T : V*/ {
            Initialization(map);
        }

        /// <summary>
        /// Constructs an EnumMap with the same contents as the given Map. If the Map
        /// is an EnumMap, this is equivalent to calling
        /// {@link EnumMap#EnumMap(EnumMap)}}. Otherwise, the given map cannot be
        /// empty so that the key type of this EnumMap can be inferred.
        /// </summary>
        /// <param name="map">the Map from which the initial contents of this EnumMap are copied</param>
        public EnumMap(IDictionary<K, V> map)
        {
            if (map is EnumMap<K, V>)
            {
                Initialization((EnumMap<K, V>)map);
            }
            else
            {
                if (0 == map.Count)
                {
                    throw new ArgumentException("");
                }
                IEnumerator<K> iter = map.Keys.GetEnumerator();
                K enumKey = iter.Current;
                Type clazz = enumKey.GetType();
                if (clazz.IsEnum)
                {
                    Initialization(clazz);
                }
                else
                {
                    Initialization(clazz.BaseType);
                }
                PutAllImpl(map);
            }
        }

        /// <summary>
        /// Clears this map.
        /// </summary>
        public override void Clear()
        {
            ILOG.J2CsMapping.Collections.Arrays.Fill(values, null);
            ILOG.J2CsMapping.Collections.Arrays.Fill(hasMapping, false);
            mappingsCount = 0;
        }

        /// <summary>
        /// Clones this map to create a shallow copy.
        /// </summary>
        /// <returns>a shallow copy of this map</returns>
        public new Object Clone()
        {
            try
            {
                EnumMap<K, V> enumMap = (EnumMap<K, V>)base.MemberwiseClone();
                enumMap.Initialization(this);
                return enumMap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true if the given object is present as a key in this map.
        /// </summary>
        /// <param name="key">the key to look for</param>
        /// <returns>true if this map contains the key</returns>
        public override bool ContainsKey(K key)
        {
            if (IsValidKeyType(key))
            {
                int keyOrdinal = (int)(object)key;
                return hasMapping[keyOrdinal];
            }
            return false;
        }

        /// <summary>
        /// Returns true if the given object is present as a value in this map.
        /// </summary>
        /// <param name="value"> the value to look for</param>
        /// <returns>true if this map contains the value.</returns>
        public override bool ContainsValue(V value)
        {
            if (null == value)
            {
                for (int i = 0; i < enumSize; i++)
                {
                    if (hasMapping[i] && null == values[i])
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < enumSize; i++)
                {
                    if (hasMapping[i] && value.Equals(values[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a Set of <code>Map.Entry</code>s that represent the entries in
        /// this EnumMap. Making changes to this Set will change the original EnumMap
        /// and vice-versa. Entries can be removed from the Set, or their values can
        /// be changed, but new entries cannot be added.
        /// </summary>
        /// <returns>a Set of <code>Map.Entry</code>s representing the entries in this EnumMap</returns>
        public override ISet<KeyValuePair<K, V>> EntrySet()
        {
            if (null == entrySet)
            {
                entrySet = new EnumMapEntrySet<K, V>(this);
            }
            return entrySet;
        }

        /// <summary>
        /// Returns true if this EnumMap is equal to the given object.
        /// </summary>
        /// <param name="obj">the object</param>
        /// <returns>true if this EnumMap is equal to the given object.</returns>
        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is EnumMap<K, V>))
            {
                return base.Equals(obj);
            }
            EnumMap<K, V> enumMap = (EnumMap<K, V>)obj;
            if (keyType != enumMap.keyType || Count != enumMap.Count)
            {
                return false;
            }
            return Arrays.AreEquals(hasMapping, enumMap.hasMapping)
                    && Arrays.AreEquals(values, enumMap.values);
        }

        /// <summary>
        /// Returns the value stored in this map for the given key in this map, or null
        /// if this map has no entry for that key.
        /// </summary>
        /// <param name="key">the key to get the value for.</param>
        /// <returns>the value for the given key.</returns>
        public override V Get(Object key)
        {
            if (!IsValidKeyType(key))
            {
                return default(V);
            }
            int keyOrdinal = (int)(object)((Enum)key);
            Object oldValue = values[keyOrdinal];
            if (oldValue == null)
                return default(V);
            return (V)values[keyOrdinal];
        }

        /// <summary>
        /// Returns a Set containing the keys for this EnumMap. Making changes to
        /// this Set will change the original EnumMap and vice-versa. Entries can be
        /// removed from the Set, but new entries cannot be added.
        /// The order of the Set will be the order that the Enum keys were declared
        /// in.
        /// </summary>
        /// <returns>a Set containing the keys for this EnumMap.</returns>
        public override ISet<K> KeySet()
        {
            if (null == keySet)
            {
                keySet = new EnumMapKeySet<K, V>(this);
            }
            return keySet;
        }

        /// <summary>
        /// Stores a value in this map for the given key. If the map already has an
        /// entry for this key the current value will be overwritten.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value">the value to store for the given key</param>
        /// <returns>the value stored for the given key, or null if this map has no entry for the key</returns>
        public override V Put(K key, V value)
        {
            return PutImpl(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public override ICollection<K> Keys
        {
            get
            {
                return KeySet();
            }
        }

        /// <summary>
        /// Add all the entries in the given map to this map
        /// </summary>
        /// <param name="map">the map whose entries to copy</param>
        public override void PutAll(IDictionary<K, V> map)
        {
            PutAllImpl(map);
        }

        /// <summary>
        /// Removes the entry for the given key from this map, if there is one.
        /// </summary>
        /// <param name="key">the key to remove</param>
        /// <returns>the value that had been stored for the key, or null if there was not one.</returns>
        public new V Remove(K key)
        {
            if (!IsValidKeyType(key))
            {
                return default(V);
            }
            int keyOrdinal = (int)(object)key;
            if (hasMapping[keyOrdinal])
            {
                hasMapping[keyOrdinal] = false;
                mappingsCount--;
            }
            Object oldValue = values[keyOrdinal];

            values[keyOrdinal] = null;
            if (oldValue != null)
            {
                return (V)oldValue;
            }
            else
                return default(V);
        }

        /// <summary>
        /// Returns the size of this map
        /// </summary>
        public override int Count
        {
            get { return mappingsCount; }
        }

        /// <summary>
        /// Returns a Collection containing the values for this EnumMap. Making
        /// changes to this Collection will change the original EnumMap and
        /// vice-versa. Values can be removed from the Collection, but new entries
        /// cannot be added.
        /// 
        /// The order of the values in the Collection will be the order that their
        /// corresponding Enum keys were declared in.
        /// </summary>
        /// <returns>a Collection containing the values for this EnumMap</returns>
        public new ICollection<V> Values()
        {
            if (null == valuesCollection)
            {
                valuesCollection = new EnumMapValueCollection<K, V>(this);
            }
            return valuesCollection;
        }


        private bool IsValidKeyType(Object key)
        {
            if (null != key && keyType.IsInstanceOfType(key))
            {
                return true;
            }
            return false;
        }

        private void Initialization(EnumMap<K, V> enumMap)
        {
            keyType = enumMap.keyType;
            keys = enumMap.keys;
            enumSize = enumMap.enumSize;
            values = (object[])enumMap.values.Clone();
            hasMapping = (bool[])enumMap.hasMapping.Clone();
            mappingsCount = enumMap.mappingsCount;
        }

        private void Initialization(Type type)
        {
            keyType = type;
            Array srcArray = Enum.GetValues(keyType); //.GetEnumConstants();
            keys = new Object[srcArray.Length];
            Array.Copy(srcArray, keys, srcArray.Length);
            enumSize = keys.Length;
            values = new Object[enumSize];
            ILOG.J2CsMapping.Collections.Arrays.Fill(values, 0, values.Length, default(V));
            hasMapping = new bool[enumSize];
        }

        private void PutAllImpl(IDictionary<K, V> map)
        {
            IEnumerator<KeyValuePair<K, V>> iter = map.GetEnumerator();
            while (iter.MoveNext())
            {
                KeyValuePair<K, V> entry = (KeyValuePair<K, V>)iter.Current;
                PutImpl((K)entry.Key, (V)entry.Value);
            }
        }

        private V PutImpl(K key, V value)
        {
            if (null == key)
            {
                throw new NullReferenceException();
            }
            if (!IsValidKeyType(key))
            {
                throw new Exception("");
            }
            int keyOrdinal = (int)(object)key;
            if (!hasMapping[keyOrdinal])
            {
                hasMapping[keyOrdinal] = true;
                mappingsCount++;
            }
            Object oldValue = values[keyOrdinal];

            values[keyOrdinal] = value;
            if (oldValue != null)
            {
                return (V)oldValue;
            }
            else
                return default(V);
        }
    }
}