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

namespace ILOG.J2CsMapping.Collections.Generics
{
    /// <summary>
    /// .NET replacement for java Queue
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public interface IQueue<E> : ICollection<E>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        bool Add(E e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        bool Offer(E e);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        E Remove();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        E Poll();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        E Element();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        E Peek();
    }
}
