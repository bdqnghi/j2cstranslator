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
using System.Collections;
using System.Text;

namespace ILOG.J2CsMapping.Collections
{

    /// <summary>
    /// 
    /// </summary>
    public class ReadOnlySet : ISet
    {

        private ISet set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        public ReadOnlySet(ISet set)
        {
            this.set = set;
        }

        #region IExtendedCollection Members

        public bool Add(object e)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public bool AddAll(ICollection c)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public void Clear()
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public bool Contains(object e)
        {
            return set.Contains(e);
        }

        public bool ContainsAll(ICollection c)
        {
            return set.ContainsAll(c);
        }

        public bool Remove(object e)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public bool RemoveAll(ICollection c)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public bool RetainAll(ICollection c)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public object[] ToArray()
        {
            return set.ToArray();
        }

        public object[] ToArray(object[] arr)
        {
            return set.ToArray(arr);
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            set.CopyTo(array, index);
        }

        public int Count
        {
            get
            {
                return set.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                return set.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return set.GetEnumerator();
        }

        #endregion
    }
}