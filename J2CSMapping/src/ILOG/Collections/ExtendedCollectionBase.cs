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

namespace ILOG.J2CsMapping.Collections {

    /// <summary>
    /// 
    /// </summary>
    public abstract class ExtendedCollectionBase {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static ISet ReadOnly(ISet set) {
            return new ReadOnlySet(set);
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract ICollection InnerCollection {
            get;
        }

        #region IExtendedCollection Members

        public abstract bool Add(object e);

        public bool AddAll(ICollection c) {
            bool result = false;
            foreach (object o in c) {
                result |= this.Add(o);
            }
            return result;
        }

        public abstract bool Contains(object e);

        public virtual bool ContainsAll(ICollection c)
        {
            foreach (object o in c) {
                if (!this.Contains(o)) {
                    return false;
                }
            }
            return true;
        }

        public abstract bool Remove(object e);

        public virtual bool RemoveAll(ICollection c)
        {
            bool result = false;
            foreach (object o in c) {
                result |= Remove(o);
            }
            return result;
        }

        public virtual bool RetainAll(ICollection c) {
            ArrayList list = new ArrayList(InnerCollection);
            foreach (object o in list) {
                if (!CollectionContains(c, o)) {
                    Remove(o);
                }
            }
            return this.Count != list.Count;
        }

        private static bool CollectionContains(System.Collections.ICollection c, object v) {
            foreach (object o in c) {
                if (object.Equals(o, v)) {
                    return true;
                }
            }
            return false;
        }

        public virtual object[] ToArray()
        {
            object[] result = new object[InnerCollection.Count];
            int i = 0;
            foreach (object o in InnerCollection) {
                result[i++] = o;
            }
            return result;
        }

        public virtual object[] ToArray(object[] arr)
        {
            int count = this.Count;
            if (arr.Length < count) {
                arr = (object[])Array.CreateInstance(arr.GetType().GetElementType(), count);
            }
            this.CopyTo(arr, 0);
            if (arr.Length > count) {
                arr[count] = null;
            }
            return arr;
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index) {
            InnerCollection.CopyTo(array, index);
        }

        public int Count {
            get {
                return InnerCollection.Count;
            }
        }

        public bool IsSynchronized {
            get {
                return InnerCollection.IsSynchronized;
            }
        }

        public object SyncRoot {
            get {
                return InnerCollection.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public virtual IEnumerator GetEnumerator() {
            return InnerCollection.GetEnumerator();
        }

        #endregion
    }
}
