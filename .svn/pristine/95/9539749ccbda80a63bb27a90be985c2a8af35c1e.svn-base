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

using System;
using System.Collections.Generic;
using System.Text;
using ILOG.J2CsMapping.Collections.Generics;

namespace ILOG.J2CsMapping.Collections.Generics
{
    /// <summary>
    /// .NET Replacement for Java AbstractCollection
    /// </summary>
    public abstract class AbstractCollection<E> : ICollection<E>
    {
        protected AbstractCollection()
        {
        }

        //

        public virtual void Add(E obj)
        {
            throw new NotSupportedException();
        }

        public virtual bool AddAll<T>(ICollection<T> c) where T : E
        {
            foreach (T current in c)
            {
                Add(current);
            }
            return true;
        }


        public virtual void Clear()
        {
            IIterator<E> it = Iterator();
            while (it.HasNext())
            {
                it.Next();
                it.Remove();
            }
        }

        public virtual bool Contains(E obj)
        {
            IIterator<E> it = Iterator();
            if (obj != null)
            {
                while (it.HasNext())
                {
                    if (obj.Equals(it.Next()))
                    {
                        return true;
                    }
                }
            }
            else
            {
                while (it.HasNext())
                {
                    if (it.Next() == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public virtual bool ContainsAll(ICollection<E> collection)
        {
            foreach (E current in collection)
            {
                if (!Contains(current))
                {
                    return false;
                }
            }
            return true;
        }

        public virtual bool IsEmpty()
        {
            return Count == 0;
        }

        public abstract IIterator<E> Iterator();

        public virtual bool Remove(Object obj)
        {
            IIterator<E> it = Iterator();
            if (obj != null)
            {
                while (it.HasNext())
                {
                    if (obj.Equals(it.Next()))
                    {
                        it.Remove();
                        return true;
                    }
                }
            }
            else
            {
                while (it.HasNext())
                {
                    if (it.Next() == null)
                    {
                        it.Remove();
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool RemoveAll(ICollection<E> collection)
        {
            bool result = false;
            IIterator<E> it = Iterator();
            while (it.HasNext())
            {
                if (collection.Contains(it.Next()))
                {
                    it.Remove();
                    result = true;
                }
            }
            return result;
        }

        public virtual bool RetainAll(ICollection<E> collection)
        {
            bool result = false;
            IIterator<E> it = Iterator();
            while (it.HasNext())
            {
                if (!collection.Contains(it.Next()))
                {
                    it.Remove();
                    result = true;
                }
            }
            return result;
        }

        public abstract int Count { get; }

        public virtual E[] ToArray()
        {
            int size = Count, index = 0;
            IIterator<E> it = Iterator();
            E[] array = new E[size];
            while (index < size)
            {
                array[index++] = it.Next();
            }
            return array;
        }

        public virtual T[] ToArray<T>(T[] contents) where T : E
        {
            int size = Count, index = 0;
            if (size > contents.Length)
            {
                Type eType = contents.GetType().GetElementType();
                contents = (T[])Array.CreateInstance(eType, size);
            }
            foreach (E entry in this)
            {
                contents[index++] = (T)entry;
            }
            if (index < contents.Length)
            {
                contents[index] = default(T);
            }
            return contents;
        }

        public override String ToString()
        {
            if (IsEmpty())
            {
                return "[]";
            }

            StringBuilder buffer = new StringBuilder(Count * 16);
            buffer.Append('[');
            IIterator<E> it = Iterator();
            while (it.HasNext())
            {
                Object next = it.Next();
                if (next != this)
                {
                    buffer.Append(next);
                }
                else
                {
                    buffer.Append("(this Collection)");
                }
                if (it.HasNext())
                {
                    buffer.Append(", ");
                }
            }
            buffer.Append(']');
            return buffer.ToString();
        }

        #region ICollection<E> Members


        public virtual void CopyTo(E[] array, int arrayIndex)
        {
            E[] res = ToArray();
            for (int i = 0; i < Count; i++)
            {
                array[i] = res[i];
            }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool Remove(E item)
        {
            return this.Remove((object)item);
        }

        #endregion

        #region IEnumerable<E> Members

        public IEnumerator<E> GetEnumerator()
        {
            return new IEnumeratorAdapter<E>(Iterator());
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new IEnumeratorAdapter<E>(Iterator());
        }

        #endregion
    }
}
