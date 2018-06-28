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
using System.Globalization;

namespace ILOG.J2CsMapping.Util
{

    /// <summary>
    /// Utility class for the mapping of System.Int32 with java.lang.Integer.
    /// </summary>
    public class Int32Helper
    {

        //
        // CompareTo
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static int CompareTo(Int32? current, Int32? other)
        {
            return current.Value.CompareTo(other.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static int CompareTo(Int32 current, Int32 other)
        {
            return current.CompareTo(other);
        }

        //
        // NumberOfTrailingZeros
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// ToBeChecked
        public static int NumberOfTrailingZeros(int i)
        {
            return BitCount((i & -i) - 1);
        }

        //
        // BitCount
        //

        /// <summary>
        /// <p>Counts the number of 1 bits in the <code>int</code>
        /// value passed; this is sometimes referred to as a
        /// population count.</p>
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// ToBeChecked
        public static int BitCount(int i)
        {
            // Algo from : http://aggregate.ee.engr.uky.edu/MAGIC/#Population%20Count%20(ones%20Count)   
            i -= ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            i = (((i >> 4) + i) & 0x0F0F0F0F);
            i += (i >> 8);
            i += (i >> 16);
            return (i & 0x0000003F);
        }

        //
        // Decode
        //

        private static int Parse(String str, int offset, int radix,
           bool negative)
        {
            int max = Int32.MinValue / radix;
            int result = 0, length = str.Length;
            while (offset < length)
            {
                int digit = Character.Digit(str[offset++], radix);
                if (digit == -1)
                {
                    throw new ArgumentException(str);
                }
                if (max > result)
                {
                    throw new ArgumentException(str);
                }
                int next = result * radix - digit;
                if (next > result)
                {
                    throw new ArgumentException(str);
                }
                result = next;
            }
            if (!negative)
            {
                result = -result;
                if (result < 0)
                {
                    throw new ArgumentException(str);
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the string argument as if it was an int value and returns the
        /// result. Throws NumberFormatException if the string does not represent an
        /// int quantity. The string may be a hexadecimal ("0x..."), octal ("0..."),
        /// or decimal ("...") representation of an integer
        /// </summary>
        /// <param name="nm">a string representation of an int quantity.</param>
        /// <returns>the value represented by the argument</returns>
        /// ToBeChecked
        public static int Decode(String nm)
        {
            int length = nm.Length;
            int i = 0;
            if (length == 0)
            {
                throw new ArgumentException();
            }
            char firstDigit = nm[i];
            bool negative = firstDigit == '-';
            if (negative)
            {
                if (length == 1)
                {
                    throw new ArgumentException(nm);
                }
                firstDigit = nm[++i];
            }

            int bas = 10;
            if (firstDigit == '0')
            {
                if (++i == length)
                {
                    return 0;
                }
                if ((firstDigit = nm[i]) == 'x' || firstDigit == 'X')
                {
                    if (++i == length)
                    {
                        throw new ArgumentException(nm);
                    }
                    bas = 16;
                }
                else
                {
                    bas = 8;
                }
            }
            else if (firstDigit == '#')
            {
                if (++i == length)
                {
                    throw new ArgumentException(nm);
                }
                bas = 16;
            }

            int result = Parse(nm, i, bas, negative);
            return result;           
        }
    }
}
