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

namespace ILOG.J2CsMapping.Util
{
    using System;

    /// <summary>
    /// .NET utility for java Number class
    /// </summary>
    public class IlNumber
    {

        /// <summary>
        /// 
        /// </summary>
        private IlNumber()
        {
        }

        //------------------------------------------------------------
        // Public APIs
        //------------------------------------------------------------

        /// <summary>
        /// The minimum possible radix used for conversions between Characters and
        /// integers.
        /// </summary>
        public static int CharacterMIN_RADIX = 2;

        /// <summary>
        /// The maximum possible radix used for conversions between Characters and
        /// integers.
        /// </summary>
        public static int CharacterMAX_RADIX = 36;

        /// <summary>
        /// Answers a string containing characters in the range 0..9, a..z (depending
        /// on the radix) which describe the representation of the argument in that
        /// radix.
        /// </summary>
        /// <param name="i">an int to get the representation of</param>
        /// <param name="radix">the base to use for conversion</param>
        /// <returns>the representation of the argument</returns>
        /// ToBeChecked
        public static String ToString(int i, int radix)
        {
            // http://java.sun.com/javase/6/docs/api/java/lang/Integer.html#toString(int, int)
            if (radix < CharacterMIN_RADIX || radix > CharacterMAX_RADIX)
            {
                radix = 10;
            }
            if (i == 0)
            {
                return "0";
            }

            int count = 2, j = i;
            if (i >= 0)
            {
                count = 1;
                j = -i;
            }
            while ((i /= radix) != 0)
            {
                count++;
            }

            char[] buffer = new char[count];
            if (i < 0)
            {
                buffer[0] = '-';
            }
            do
            {
                int ch = 0 - (j % radix);
                if (ch > 9)
                {
                    ch = ch - 10 + 'a';
                }
                else
                {
                    ch += '0';
                }
                buffer[--count] = (char)ch;
            } while ((j /= radix) != 0);

            return new String(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string ToString(int n)
        {
            return n.ToString();
        }
    }
}
