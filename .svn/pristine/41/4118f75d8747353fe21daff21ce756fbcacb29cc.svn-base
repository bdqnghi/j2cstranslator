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

namespace ILOG.J2CsMapping.Collections {

    /// <summary>
    /// This interface allows ILOG.J2CsMapping.Collections.Collections utility methods to be
    /// called on a class that only implements java.util.Collection on the Java side.
    /// </summary>
    public interface IExtendedCollection : ICollection {
        bool Add(object e);
        bool AddAll(ICollection c);
        void Clear();
        bool Contains(object e);
        bool ContainsAll(ICollection c);
        bool Remove(object e);
        bool RemoveAll(ICollection c);
        bool RetainAll(ICollection c);
        object[] ToArray();
        object[] ToArray(object[] arr);
    }
}
