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

namespace ILOG.J2CsMapping.Collections.Generics
{
    /// <summary>
    /// .NET Replacement for Java AbstractQueue
    /// </summary>
    public abstract class AbstractQueue<E> : AbstractCollection<E>, IQueue<E>
    {
        protected AbstractQueue()
        {
        }

        public virtual bool Add(E o)
        {
            if (null == o)
            {
                throw new NullReferenceException();
            }
            if (Offer(o))
            {
                return true;
            }
            throw new Exception();
        }

        public override bool AddAll<T>(ICollection<T> c)
        {
            if (null == c)
            {
                throw new NullReferenceException();
            }
            if (this == c)
            {
                throw new NullReferenceException();
            }
            return base.AddAll(c);
        }

        public E Remove()
        {
            E o = Poll();
            if (null == o)
            {
                throw new NullReferenceException();
            }
            return o;
        }

        public E Element()
        {
            E o = Peek();
            if (null == o)
            {
                throw new NullReferenceException();
            }
            return o;
        }

        public override void Clear()
        {
            E o;
            do
            {
                o = Poll();
            } while (null != o);
        }

        #region IQueue<E> Members


        public virtual bool Offer(E e)
        {
            throw new NotImplementedException();
        }

        public virtual E Poll()
        {
            throw new NotImplementedException();
        }

        public virtual E Peek()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        public new System.Collections.IEnumerator GetEnumerator()
        {
            return new IEnumeratorAdapter<E>(Iterator());
        }

        #endregion
    }
}
