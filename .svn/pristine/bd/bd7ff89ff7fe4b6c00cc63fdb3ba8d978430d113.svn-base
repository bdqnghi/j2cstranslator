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

using System.Collections.Generic;
using System;

namespace ILOG.J2CsMapping.Collections.Generics
{
    /// <summary>
    /// This class is used to adapt Iterator (Java) behavior
    /// to Enumerator (C#) behavior.
    /// Concepts are closed BUT
    /// in Enumerator :
    ///    1/ MoveNext() do TWO things :
    ///      - go to the next element
    ///      - return a boolean to indicate ends of iterator
    ///    2/ Current : only return the current element
    /// in Iterator :
    ///    1/ HasNext() : to indicate ends
    ///    2/ Next() : to move to the next value and return it.
    ///    
    /// So i have to memories Enumerator state before every action ...
    /// 
    /// </summary>
    public class IteratorAdapter<T> : IIterator<T>
    {
        private IEnumerator<T> enumerator;
        private bool callMoveNext;
        private bool hasNext;

        /// <summary>
        /// Create an iterator adapator for an IEnumerator
        /// </summary>
        /// <param name="e">The IEnumerator to Adapt</param>
        public IteratorAdapter(IEnumerator<T> e)
        {
            enumerator = e;
            callMoveNext = true;
        }

        /// <summary>
        /// Move to the next item and update iterator values
        /// </summary>
        private void MoveNext()
        {
            if (callMoveNext) {
                hasNext = enumerator.MoveNext();
                callMoveNext = false;
            }
        }

        /// <summary>
        /// Tells if this iterator have more value or not.
        /// </summary>
        /// <returns>true if iterator has more value</returns>
        public bool HasNext()
        {
            MoveNext();
            return hasNext;
        }

        /// <summary>
        /// Return the next value of the iterator
        /// </summary>
        /// <returns>the current value</returns>
        public T Next()
        {
            MoveNext();
            callMoveNext = true;
            return enumerator.Current;
        }

        public void Remove()
        {
            throw new NotImplementedException("");            
        }

        #region IIterator Members

        bool IIterator.HasNext()
        {
            return HasNext();
        }

        object IIterator.Next()
        {
            return Next();
        }

        void IIterator.Remove()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
