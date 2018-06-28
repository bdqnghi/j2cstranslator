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

namespace ILOG.J2CsMapping.Util
{
	/// <summary>
	/// Utility class for the mapping of System.Boolean with java.lang.Boolean.
	/// </summary>
    public class BooleanUtil {

        public const Boolean TRUE = true;
        public const Boolean FALSE = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ValueOf(string s) {
            if (s == null) {
                return false;
            } else {
                return bool.Parse(s);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToString(bool b) {
            return b ? "true" : "false";
        }
    }
}
