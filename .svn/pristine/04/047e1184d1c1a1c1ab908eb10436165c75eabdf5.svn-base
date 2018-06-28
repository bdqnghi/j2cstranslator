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
    /// .NET Replacement for Java HashedSet
    /// </summary>
    public class HashedSet<T> : ExtendedCollectionBase<T>, ISet<T>
    {
        private Dictionary<T, T> dictionary;

        /// <summary>
        /// 
        /// </summary>
        public HashedSet()
            : this(0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public HashedSet(int capacity)
            : this(capacity, 1.0f)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="loadFactor"></param>
        public HashedSet(int capacity, float loadFactor)
        {
            dictionary = new Dictionary<T, T>(capacity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public HashedSet(ICollection<T> c)
            : this()
        {
            AddAll(c);
        }

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        protected override ICollection<T> InnerCollection
        {
            get
            {
                return dictionary.Keys;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsEmpty
        {
            get
            {
                return (dictionary.Count == 0);
            }
        }

        #region IExtendedCollection Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool Add(T e)
        {
            if (!dictionary.ContainsKey(e))
            {
                dictionary.Add(e, e);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            dictionary.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool Contains(T e)
        {
            return dictionary.ContainsKey(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool Remove(T e)
        {
            int count = dictionary.Count;
            dictionary.Remove(e);
            return count != dictionary.Count;
        }

        #endregion

         public override IEnumerator<T> GetEnumerator() {
            return dictionary.Values.GetEnumerator();
        }

    }
}