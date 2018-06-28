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
    using ICollection = System.Collections.ICollection;
    using IIterator = ILOG.J2CsMapping.Collections.IIterator;
    using ILOG.J2CsMapping.Collections.Generics;
    using ILOG.J2CsMapping.Collections;
    using ILOG.J2CsMapping.IO;
    using ISet = ILOG.J2CsMapping.Collections.ISet;
    using System.Collections.Generic;
    using System.Collections;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;
    using System;

    /// <summary>
    /// .NET Replacement for Java AbstractMap
    /// </summary>
    public abstract class AbstractMap<K, V> : IDictionary<K, V>
    {

        protected ISet<K> keySet;

        protected ICollection<V> valuesCollection;

        protected AbstractMap()
        {
        }

        public virtual void Clear()
        {
            EntrySet().Clear();
        }


        public virtual bool ContainsKey(K key)
        {
            if (key != null)
            {
                foreach (KeyValuePair<K, V> kvp in EntrySet())
                {
                    if (key.Equals(kvp.Key))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<K, V> kvp in EntrySet())
                {
                    if (kvp.Key == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool ContainsValue(V value)
        {
            if (value != null)
            {
                foreach (KeyValuePair<K, V> kvp in EntrySet())
                {
                    if (value.Equals(kvp.Value))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<K, V> kvp in EntrySet())
                {
                    if (kvp.Value == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public abstract ISet<KeyValuePair<K, V>> EntrySet();

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj is IDictionary<K, V>)
            {
                IDictionary<K, V> map = (IDictionary<K, V>)obj;
                if (Count != map.Count)
                {
                    return false;
                }

                try
                {
                    foreach (KeyValuePair<K, V> entry in EntrySet())
                    {
                        K key = entry.Key;
                        V value = entry.Value;
                        V obj0 = map[key];
                        if ((null != obj0) && (!obj0.Equals(value)) || (null == obj0) /*&& (obj0 != value)*/)
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public virtual V Get(Object key)
        {
            IEnumerator<KeyValuePair<K, V>> it = EntrySet().GetEnumerator();
            if (key != null)
            {
                foreach (KeyValuePair<K, V> entry in EntrySet())
                {
                    if (key.Equals(entry.Key))
                    {
                        return entry.Value;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<K, V> entry in EntrySet())
                {
                    if (entry.Key == null)
                    {
                        return entry.Value;
                    }
                }
            }
            return default(V);
        }

        public override int GetHashCode()
        {
            int result = 0;
            foreach (KeyValuePair<K, V> entry in EntrySet())
            {
                result += entry.GetHashCode();
            }
            return result;
        }

        public virtual bool IsEmpty()
        {
            return Count == 0;
        }

        public virtual ISet<K> KeySet()
        {
            if (keySet == null)
            {
                keySet = new AnonymousSet(this);
            }
            return keySet;
        }

        public class AnonymousSet : AbstractSet<K>
        {

            AbstractMap<K, V> enclosing = null;

            public AnonymousSet(AbstractMap<K, V> enclosing)
            {
                this.enclosing = enclosing;
            }

            public bool Contains(Object obj)
            {
                return enclosing.ContainsKey((K)obj);
            }

            public override int Count
            {
                get { return enclosing.Count; }
            }

            public override IIterator<K> Iterator()
            {
                return new AnonymousIterator(enclosing);
            }

            public class AnonymousIterator : IIterator<K>
            {
                IIterator<KeyValuePair<K, V>> setIterator = null;

                public AnonymousIterator(AbstractMap<K, V> enclosing)
                {
                    setIterator = new IteratorAdapter<KeyValuePair<K, V>>(enclosing.EntrySet().GetEnumerator());
                }

                public bool HasNext()
                {
                    return setIterator.HasNext();
                }

                public K Next()
                {
                    return setIterator.Next().Key;
                }

                public void Remove()
                {
                    setIterator.Remove();
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
        }

        public virtual V Put(K key, V value)
        {
            throw new NotSupportedException();
        }

        public virtual void PutAll(IDictionary<K, V> map)
        {
            foreach (KeyValuePair<K, V> entry in map)
            {
                this[entry.Key] = entry.Value;
            }
        }

        public virtual bool Remove(K key)
        {
            if (key != null)
            {
                foreach (KeyValuePair<K, V> entry in EntrySet())
                {
                    if (key.Equals(entry.Key))
                    {
                        // it.Remove();
                        // return entry.Value;
                        throw new NotImplementedException("");
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<K, V> entry in EntrySet())
                {
                    if (entry.Key == null)
                    {
                        //it.Remove();
                        //return entry.Value;
                        throw new NotImplementedException("");
                    }
                }
            }
            throw new NotImplementedException("");
        }

        public virtual int Count
        {
            get { return EntrySet().Count; }
        }


        public override String ToString()
        {
            if (IsEmpty())
            {
                return "{}"; 
            }

            StringBuilder buffer = new StringBuilder(Count * 28);
            buffer.Append('{');
            foreach (KeyValuePair<K, V> entry in EntrySet())
            {
                Object key = entry.Key;
                if (key != this)
                {
                    buffer.Append(key);
                }
                else
                {
                    buffer.Append("(this Map)"); 
                }
                buffer.Append('=');
                Object value = entry.Value;
                if (value != this)
                {
                    buffer.Append(value);
                }
                else
                {
                    buffer.Append("(this Map)"); 
                }
                /*if (it.HasNext())
                {
                    buffer.Append(", "); //$NON-NLS-1$
                }*/
            }
            buffer.Append('}');
            return buffer.ToString();
        }


        public virtual ICollection<V> Values()
        {
            if (valuesCollection == null)
            {
                /*valuesCollection = new AbstractCollection<V>() {
                    @Override
                    public int size() {
                        return AbstractMap.this.size();
                    }

                    @Override
                    public boolean contains(Object object) {
                        return containsValue(object);
                    }

                    @Override
                    public Iterator<V> iterator() {
                        return new Iterator<V>() {
                            Iterator<Map.Entry<K, V>> setIterator = entrySet()
                                    .iterator();

                            public boolean hasNext() {
                                return setIterator.hasNext();
                            }

                            public V next() {
                                return setIterator.next().getValue();
                            }

                            public void remove() {
                                setIterator.remove();
                            }
                        };
                    }
                };*/
            }
            return valuesCollection;
        }

        protected Object Clone()
        {
            throw new NotImplementedException("");
        }


        #region IDictionary<K,V> Members

        public virtual void Add(K key, V value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual ICollection<K> Keys
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }



        public virtual bool TryGetValue(K key, out V value)
        {
            try
            {
                value = this[key];
                return true;
            }
            catch (Exception)
            {
                value = default(V);
                return false;
            }
        }

        ICollection<V> IDictionary<K, V>.Values
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual V this[K key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Put(key, value);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<K,V>> Members

        public void Add(KeyValuePair<K, V> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual bool Remove(KeyValuePair<K, V> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<KeyValuePair<K,V>> Members

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return EntrySet().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return EntrySet().GetEnumerator();
        }

        #endregion

    }
}
