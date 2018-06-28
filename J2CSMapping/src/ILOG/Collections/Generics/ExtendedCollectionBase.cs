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

namespace ILOG.J2CsMapping.Collections.Generics {

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExtendedCollectionBase<T> : IExtendedCollection<T> {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static ISet<T> ReadOnly(ISet<T> set) {
            return new ReadOnlySet<T>(set);
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract ICollection<T> InnerCollection {
            get;
        }

        #region IExtendedCollection Members

        public abstract bool Add(T e);

        public abstract bool IsEmpty
        {
            get;
        }

        public virtual bool AddAll(ICollection<T> c)
        {
            bool result = false;
            foreach (T o in c) {
                result |= this.Add(o);
            }
            return result;
        }

        public abstract void Clear();

        public abstract bool Contains(T e);

        public virtual bool ContainsAll(ICollection<T> c)
        {
            foreach (T o in c) {
                if (!this.Contains(o)) {
                    return false;
                }
            }
            return true;
        }

        public abstract bool Remove(T e);

        public virtual bool RemoveAll(ICollection<T> c)
        {
            bool result = false;
            foreach (T o in c) {
                result |= Remove(o);
            }
            return result;
        }

        public virtual bool RetainAll(ICollection<T> c)
        {
            List<T> list = new List<T>(InnerCollection);
            foreach (T o in list) {
                if (!CollectionContains(c, o)) {
                    Remove(o);
                }
            }
            return this.Count != list.Count;
        }

        private static bool CollectionContains(ICollection<T> c, T v)
        {
            foreach (T o in c) {
                if (object.Equals(o, v)) {
                    return true;
                }
            }
            return false;
        }

        public virtual T[] ToArray() {
            T[] result = new T[InnerCollection.Count];
            int i = 0;
            foreach (T o in InnerCollection) {
                result[i++] = o;
            }
            return result;
        }

        public T[] ToArray(T[] arr) {
            int count = this.Count;
            if (arr.Length < count) {
                arr = (T[])Array.CreateInstance(arr.GetType().GetElementType(), count);
            }
            this.CopyTo(arr, 0);
            if (arr.Length > count) {
                arr[count] = default(T);
            }
            return arr;
        }

        #endregion

        #region ICollection Members

        public void CopyTo(T[] array, int index) {
            InnerCollection.CopyTo(array, index);
        }

        public virtual int Count {
            get {
                return InnerCollection.Count;
            }
        }

        public virtual bool IsSynchronized
        {
            get {
                return false; // InnerCollection.IsSynchronized;
            }
        }

        public virtual object SyncRoot
        {
            get {
                return null; // InnerCollection.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            return InnerCollection.GetEnumerator();
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
