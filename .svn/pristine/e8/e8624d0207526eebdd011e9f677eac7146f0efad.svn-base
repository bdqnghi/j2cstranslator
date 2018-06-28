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

using System.Collections;
using System;

namespace ILOG.J2CsMapping.Collections
{
	/// <summary>
	/// This class is used to adapt IEnumerator (C#) behavior
	/// to Iterator (Java) behavior.
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
	public class IEnumeratorAdapter : IEnumerator {
		private IIterator enume = null;
	    private object current = null;

		/// <summary>
		/// Create an iterator adapator for an IEnumerator
		/// </summary>
		/// <param name="enume">The IEnumerator to Adapt</param>
        public IEnumeratorAdapter(IIterator enume)
        {			
			if (enume != null) {
				this.enume = enume;
                if (enume.HasNext())
                    current = enume.Next();			
			}
		}

        #region IEnumerator Members

        public object Current
        {
            get { return current; }
        }

        public bool MoveNext()
        {
            bool hasNext = enume.HasNext();
            if (hasNext)
                current = enume.Next();
            return hasNext;
        }

        public void Reset()
        {
            // Don't knwo how to do that ...
        }

        #endregion
    }
}
