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

namespace ILOG.J2CsMapping.Collections.Generics
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Utility methods for arrays
    /// </summary>
    public class Arrays
    {
        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static List<T> AsList<T>(params T[] array)
        {
            return new List<T>(array);
        }

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static bool AreEquals(Array array1, Array array2)
        {
            if (array1.Length != array2.Length)
                return false;
            for (int i1 = 0; i1 < array1.Length; i1++)
            {
                if (!array1.GetValue(i1).Equals(array2.GetValue(i1)))
                    return false;
            }
            return true;
        }
    }
}
