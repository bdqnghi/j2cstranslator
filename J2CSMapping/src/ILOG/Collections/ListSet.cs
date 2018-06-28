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
    /// .NET Replacement for Java ListSet
    /// </summary>
    public class ListSet : ExtendedCollectionBase, ISet {

        private ArrayList list;

        /// <summary>
        /// 
        /// </summary>
        public ListSet() {
            list = new ArrayList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public ListSet(ICollection c) : this() {
            this.AddAll(c);
        }
        //

        /// <summary>
        /// 
        /// </summary>
        protected override System.Collections.ICollection InnerCollection {
            get {
                return list;
            }
        }

        #region IExtendedCollection Members

        public override bool Add(object e) {
            if (!list.Contains(e)) {
                list.Add(e);
                return true;
            }
            return false;
        }

        public void Clear() {
            list.Clear();
        }

        public override bool Contains(object e) {
            return list.Contains(e);
        }

        public override bool Remove(object e) {
            int count = list.Count;
            list.Remove(e);
            return count != list.Count;
        }

        #endregion

    }
}