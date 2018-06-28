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

namespace ILOG.J2CsMapping.Collections.Generics {

    /// <summary>
    /// .NET Replacement for Java ReadOnlySet
    /// </summary>
    public class ReadOnlySet<T> : ISet<T>
    {
        private ISet<T> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        public ReadOnlySet(ISet<T> set) {
            this.set = set;
        }

        #region IExtendedCollection Members

        public bool Add(T e) {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool AddAll(ICollection<T> c)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear() {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(T e) {
            return set.Contains(e);
        }

        public bool ContainsAll(ICollection<T> c)
        {
            return set.ContainsAll(c);
        }

        public bool Remove(T e) {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool RemoveAll(ICollection<T> c)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool RetainAll(ICollection<T> c)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public T[] ToArray() {
            return set.ToArray();
        }

        public T[] ToArray(T[] arr) {
            return set.ToArray(arr);
        }

        #endregion

        #region ICollection Members

        public void CopyTo(T[] array, int index) {
            set.CopyTo(array, index);
        }

        public virtual int Count {
            get {
                return set.Count;
            }
        }

        public bool IsSynchronized {
            get {
                return false;
            }
        }

        public object SyncRoot {
            get {
                return false; // return set.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator<T> GetEnumerator()
        {
            return set.GetEnumerator();
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IExtendedCollection<T> Members

        bool IExtendedCollection<T>.Add(T e)
        {
            throw new NotImplementedException();
        }

        bool IExtendedCollection<T>.AddAll(ICollection<T> c)
        {
            throw new NotImplementedException();
        }

        /*void IExtendedCollection<T>.Clear()
        {
            throw new NotImplementedException();
        }*/

        /*bool IExtendedCollection<T>.Contains(T e)
        {
            throw new NotImplementedException();
        }*/

        bool IExtendedCollection<T>.ContainsAll(ICollection<T> c)
        {
            throw new NotImplementedException();
        }

       /* bool IExtendedCollection<T>.Remove(T e)
        {
            throw new NotImplementedException();
        }*/

        bool IExtendedCollection<T>.RemoveAll(ICollection<T> c)
        {
            throw new NotImplementedException();
        }

        bool IExtendedCollection<T>.RetainAll(ICollection<T> c)
        {
            throw new NotImplementedException();
        }

        T[] IExtendedCollection<T>.ToArray()
        {
            throw new NotImplementedException();
        }

        T[] IExtendedCollection<T>.ToArray(T[] arr)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection<T> Members


        void ICollection<T>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<T>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}