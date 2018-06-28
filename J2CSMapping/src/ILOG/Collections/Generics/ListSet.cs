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
    /// .NET Replacement for Java ListSet
    /// </summary>
    public class ListSet<T> : ExtendedCollectionBase<T>, ISet<T> {

        private List<T> list;

        /// <summary>
        /// 
        /// </summary>
        public ListSet() {
            list = new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public ListSet(ICollection<T> c)
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
            get {
                return list;
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

        #region IExtendedCollection Members

        public override bool Add(T e) {
            if (!list.Contains(e)) {
                list.Add(e);
                return true;
            }
            return false;
        }

        public override void Clear() {
            list.Clear();
        }

        public override bool Contains(T e) {
            return list.Contains(e);
        }

        public override bool Remove(T e) {
            int count = list.Count;
            list.Remove(e);
            return count != list.Count;
        }

        #endregion

    }
}