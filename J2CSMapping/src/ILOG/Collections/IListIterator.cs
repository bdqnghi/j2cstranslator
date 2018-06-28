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

namespace ILOG.J2CsMapping.Collections
{
    /// <summary>
    /// .NET Replacement for java ListIterator
    /// </summary>
    public interface IListIterator : IRemoveableIterator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool HasPrevious();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object Previous();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int NextIndex();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int PreviousIndex();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        void Set(object x);
    }
}
