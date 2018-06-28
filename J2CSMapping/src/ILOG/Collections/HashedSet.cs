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
using System.Collections;

namespace ILOG.J2CsMapping.Collections {

    /// <summary>
    /// .NET Replacement for Java HashedSet
    /// </summary>
    public class HashedSet: ExtendedCollectionBase, ISet {

        private Dictionary<object, object> dictionary;

        //
        //
        //

        public HashedSet() : this(0) {
        }

        public HashedSet(int capacity) : this (capacity, 1.0f) {
        }

        public HashedSet(int capacity, float loadFactor) {
            dictionary = new Dictionary<object, object>(capacity);
        }

        public HashedSet(System.Collections.ICollection c) : this() {
            AddAll(c);
        }

        //
        //
        //

        public override IEnumerator GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }        

        protected override System.Collections.ICollection InnerCollection {
            get {
                return dictionary.Keys;
            }
        }

        #region IExtendedCollection Members

        public override bool Add(object e) {
            if (!dictionary.ContainsKey(e)) {
                dictionary.Add(e, e);
                return true;
            }
            return false;
        }

        public void Clear() {
            dictionary.Clear();
        }

        public override bool Contains(object e) {
            return dictionary.ContainsKey(e);
        }

        public override bool Remove(object e) {
            int count = dictionary.Count;
            dictionary.Remove(e);
            return count != dictionary.Count;
        }

        #endregion

   }
}