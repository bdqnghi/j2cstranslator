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

namespace ILOG.J2CsMapping.Collections.Generics {

    /// <summary>
    /// This interface allows ILOG.J2CsMapping.Collections.Collections utility methods to be
    /// called on a class that only implements java.util.Collection on the Java side.
    /// </summary>
    public interface IExtendedCollection<T> : ICollection<T> {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        bool Add(T e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool AddAll(ICollection<T> c);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool ContainsAll(ICollection<T> c);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool RemoveAll(ICollection<T> c);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool RetainAll(ICollection<T> c);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T[] ToArray();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        T[] ToArray(T[] arr);
    }
}
