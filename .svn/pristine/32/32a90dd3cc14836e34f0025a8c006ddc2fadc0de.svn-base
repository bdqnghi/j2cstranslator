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

namespace ILOG.J2CsMapping.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class MathUtil
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long URS(long a, int b)
        {
            long res = 0;
            if (a < 0)
            {
                ulong c = (ulong)a >> b;
                res = (long)c;
            }
            else
            {
                ulong c = ((ulong)a) >> b;
                res = Convert.ToInt64(c);

            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ulong URS(ulong a, int b)
        {
            ulong res = 0;
            if (a < 0)
            {
                ulong c = (ulong)a >> b;
                res = (ulong)c;
            }
            else
            {
                ulong c = ((ulong)a) >> b;
                res = c;

            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int URS2(int a, int b)
        {
            int res = 0;
            if (a < 0)
            {
                uint c = (uint)a >> b;
                res = (int)c;
            }
            else
            {
                uint c = ((uint)a) >> b;
                res = Convert.ToInt32(c);

            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int URS(int a, int b)
        {
            uint c = ((uint)a) >> b;
            int res = (int)c; // Convert.ToInt32(c);            
            return res;
        }
    }
}
