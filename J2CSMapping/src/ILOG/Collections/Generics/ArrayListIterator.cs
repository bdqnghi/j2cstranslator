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

namespace ILOG.J2CsMapping.Collections.Generics
{
    /// <summary>
    /// .NET Replacement for Java ArrayListIterator
    /// </summary>
    public class ArrayListIterator<T> : IListIterator<T>
    {

        private IList<T> list;
        private int index;

        public ArrayListIterator(T[] list)
            : this(list, -1)
        {
        }

        public ArrayListIterator(T[] list, int index)
            : this((IList<T>)list, index)
        {
        }

        public ArrayListIterator(IList<T> list)
            : this(list, -1)
        {
        }

        public ArrayListIterator(IList<T> list, int index)
        {
            this.list = list;
            this.index = index;
        }

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasNext()
        {
            return index + 1 < list.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException();
            }
            return list[++index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public void Add(T o)
        {
            list.Insert(++index, o);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasPrevious()
        {
            return index >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int NextIndex()
        {
            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Previous()
        {
            if (!HasPrevious())
                throw new InvalidOperationException();
            return list[index--];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int PreviousIndex()
        {
            return index - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            //throw new InvalidOperationException();
            list.Remove(list[index]);
            index = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public void Set(T o)
        {
            if (index < 0 || index >= list.Count)
            {
                throw new InvalidOperationException();
            }
            list[index] = o;
        }

        
        #region IIterator Members

        bool IIterator.HasNext()
        {
            throw new NotImplementedException();
        }

        object IIterator.Next()
        {
            return Next();
        }

        void IIterator.Remove()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
