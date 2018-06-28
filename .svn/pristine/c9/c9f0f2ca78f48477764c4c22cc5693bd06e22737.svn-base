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
    /// .NET Replacement for Java SortedSet
    /// </summary>
    public class SortedSet<T> : ExtendedCollectionBase<T>, ISet<T>
    {
        private SortedList<T, T> list;

        /// <summary>
        /// 
        /// </summary>
        public SortedSet()
        {
            list = new SortedList<T, T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public SortedSet(IComparer<T> c)
        {
            list = new SortedList<T, T>(c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public SortedSet(ICollection<T> c)
            : this()
        {
            this.AddAll(c);
        }

        //

        /// <summary>
        /// 
        /// </summary>
        protected override ICollection<T> InnerCollection
        {
            get
            {
                return list.Keys;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsEmpty
        {
            get
            {
                return (list.Count == 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IIterator<T> Iterator()
        {
            return new IteratorAdapter<T>(GetEnumerator());
        }

        #region IExtendedCollection Members

        public override bool Add(T e)
        {
            if (!list.ContainsKey(e))
            {
                list.Add(e, e);
                return true;
            }
            return false;
        }

        public override void Clear()
        {
            list.Clear();
        }

        public override bool Contains(T e)
        {
            return list.ContainsKey(e);
        }

        public override bool Remove(T e)
        {
            int count = list.Count;
            list.Remove(e);
            return count != list.Count;
        }

        #endregion
    }
}