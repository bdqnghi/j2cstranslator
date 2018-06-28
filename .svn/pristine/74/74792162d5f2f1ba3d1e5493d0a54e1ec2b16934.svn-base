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
    /// .NET Replacement for Java AbstractSet
    /// </summary>
    public abstract class AbstractSet<E> : AbstractCollection<E>, ISet<E>
    {

        protected AbstractSet()
        {
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj is ISet<E>)
            {
                ISet<E> s = (ISet<E>)obj;

                try
                {
                    return Count == s.Count && ContainsAll(s);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }


        public override int GetHashCode()
        {
            int result = 0;
            IIterator<E> it = Iterator();
            while (it.HasNext())
            {
                Object next = it.Next();
                result += next == null ? 0 : next.GetHashCode();
            }
            return result;
        }


        public override bool RemoveAll(ICollection<E> collection)
        {
            bool result = false;
            if (Count <= collection.Count)
            {
                IIterator<E> it = Iterator();
                while (it.HasNext())
                {
                    if (collection.Contains(it.Next()))
                    {
                        it.Remove();
                        result = true;
                    }
                }
            }
            else
            {
                foreach (E e in collection)
                {
                    result = Remove(e) || result;
                }
            }
            return result;
        }

        #region IExtendedCollection<E> Members

        public virtual new bool Add(E e)
        {
            throw new NotImplementedException();
        }

        public bool AddAll(ICollection<E> c)
        {
            throw new NotImplementedException();
        }

        /*public override E[] ToArray(E[] arr) //where T : E
        {
            return base.ToArray(arr);
        }*/

        #endregion

        #region IEnumerable Members

        public virtual new IEnumerator<E> GetEnumerator()
        {
            //throw new NotImplementedException();
            return new IEnumeratorAdapter<E>(Iterator());
        }

        #endregion

        public override IIterator<E> Iterator()
        {
            throw new NotImplementedException();
        }

        public override int Count
        {
            get { throw new NotImplementedException(); }
        }

        #region IExtendedCollection<E> Members

        public virtual E[] ToArray(E[] arr)
        {
            return base.ToArray<E>(arr);
        }

        #endregion
    }
}
