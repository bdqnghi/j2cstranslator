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

namespace ILOG.J2CsMapping.Collections
{
    /// <summary>
    /// .NET Replacement for Java ArrayListIterator
    /// </summary>
    public class ArrayListIterator : IListIterator
    {
        #region Fields

        private IList list;
        private int index;

        #endregion

        public ArrayListIterator(IList list)
            : this(list, -1)
        {
        }

        public ArrayListIterator(IList list, int index)
        {
            this.list = list;
            this.index = index;
        }

        #region IListIterator Members

        /// <summary></summary>
        public bool HasNext()
        {
            return index + 1< list.Count;
        }

        /// <summary></summary>
        public object Next()
        {
            if (!HasNext())
                throw new InvalidOperationException();
            return list[++index];
        }


        public void Add(object o)
        {
            list.Insert(++index, o);
            // throw new InvalidOperationException();
        }

        /// <summary></summary>
        public bool HasPrevious()
        {
            return index >= 0;
        }

        /// <summary></summary>
        public int NextIndex()
        {
            return index;
        }

        /// <summary></summary>
        public object Previous()
        {
            if (!HasPrevious())
                throw new InvalidOperationException();
            return list[index--];
        }

        /// <summary></summary>
        public int PreviousIndex()
        {
            return index - 1;
        }


        public void Remove()
        {
            throw new InvalidOperationException();
        }


        public void Set(object o)
        {
            throw new InvalidOperationException();
        }
     
        #endregion
    }        
}
