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

namespace ILOG.J2CsMapping.Collections {

    /// <summary>
    /// .NET Replacement for Java SortedSet
    /// </summary>
    public class SortedSet : ExtendedCollectionBase, ISet {
        private SortedList list;

        /// <summary>
        /// 
        /// </summary>
        public SortedSet() {
            list = new SortedList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public SortedSet(IComparer c) {
            list = new SortedList(c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public SortedSet(ICollection c) : this() {
            this.AddAll(c);
        }

        //

        /// <summary>
        /// 
        /// </summary>
        protected override System.Collections.ICollection InnerCollection {
            get {
                return list.Keys;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IIterator Iterator() {
            return new IteratorAdapter(GetEnumerator());
        }

        #region IExtendedCollection Members

        public override bool Add(object e) {
            if (!list.ContainsKey(e)) {
                list.Add(e, e);
                return true;
            }
            return false;
        }

        public void Clear() {
            list.Clear();
        }

        public override bool Contains(object e) {
            return list.ContainsKey(e);
        }

        public override bool Remove(object e) {
            int count = list.Count;
            list.Remove(e);
            return count != list.Count;
        }

        #endregion
    }
}