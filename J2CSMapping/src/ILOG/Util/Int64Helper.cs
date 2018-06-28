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
    /// Utility class for the mapping of System.Int64 with java.lang.Double.
    /// </summary>
    public class Int64Helper
    {
        //
        // NumberOfTrailingZeros
        //

        /// <summary>
        /// Determines the number of trailing zeros in the <code>long</code> passed
        /// after the long lowest one bit}.
        /// </summary>
        /// <param name="i">The <code>long</code> to process.</param>
        /// <returns>The number of trailing zeros.</returns>
        /// ToBeChecked
        public static int NumberOfTrailingZeros(long lng)
        {
            return BitCount((lng & -lng) - 1);
        }

        //
        // NumberOfLeadingZeros
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// ToBeChecked
        public static int NumberOfLeadingZeros(long lng)
        {
            lng |= lng >> 1;
            lng |= lng >> 2;
            lng |= lng >> 4;
            lng |= lng >> 8;
            lng |= lng >> 16;
            lng |= lng >> 32;
            return BitCount(~lng);
        }

        //
        // BitCount
        //

        /// <summary>
        /// <p>Counts the number of 1 bits in the <code>int</code>
        /// value passed; this is sometimes referred to as a
        /// population count.</p>
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        /// ToBeChecked
        public static int BitCount(long lng)
        {
            lng = (lng & 0x5555555555555555L) + ((lng >> 1) & 0x5555555555555555L);
            lng = (lng & 0x3333333333333333L) + ((lng >> 2) & 0x3333333333333333L);
            // adjust for 64-bit integer
            int i = (int)(ILOG.J2CsMapping.Util.MathUtil.URS(lng, 32) + lng);
            i = (i & 0x0F0F0F0F) + ((i >> 4) & 0x0F0F0F0F);
            i = (i & 0x00FF00FF) + ((i >> 8) & 0x00FF00FF);
            i = (i & 0x0000FFFF) + ((i >> 16) & 0x0000FFFF);
            return i;
        }

        //
        // Decode
        //

        private static long Parse(String str, int offset, int radix,
           bool negative)
        {
            long max = Int64.MinValue / radix;
            long result = 0, length = str.Length;
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
                long next = result * radix - digit;
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
        /// 
        /// </summary>
        /// <param name="nm"></param>
        /// <returns></returns>
        /// ToBeChecked
        public static long Decode(String str)
        {
            int length = str.Length, i = 0;
            if (length == 0)
            {
                throw new ArgumentException();
            }
            char firstDigit = str[i];
            bool negative = firstDigit == '-';
            if (negative)
            {
                if (length == 1)
                {
                    throw new ArgumentException(str);
                }
                firstDigit = str[++i];
            }

            int bas = 10;
            if (firstDigit == '0')
            {
                if (++i == length)
                {
                    return 0L;
                }
                if ((firstDigit = str[i]) == 'x' || firstDigit == 'X')
                {
                    if (i == length)
                    {
                        throw new ArgumentException(str);
                    }
                    i++;
                    bas = 16;
                }
                else
                {
                    bas = 8;
                }
            }
            else if (firstDigit == '#')
            {
                if (i == length)
                {
                    throw new ArgumentException(str);
                }
                i++;
                bas = 16;
            }

            long result = Parse(str, i, bas, negative);
            return result;
        }

        //
        //
        //

        /// <summary>
        /// <p>
        /// Determines the lowest (rightmost) bit that is 1 and returns the value
        /// that is the bit mask for that bit. This is sometimes referred to as the
        /// Least Significant 1 Bit.
        /// </p>
        /// </summary>
        /// <param name="lng">The <c>long</c> to interrogate.</param>
        /// <returns>The bit mask indicating the lowest 1 bit.</returns>
        public static long LowestOneBit(long lng)
        {
            return (lng & (-lng));
        }

        /// <summary>
        ///  <p>
        /// Determines the highest (leftmost) bit that is 1 and returns the value
        /// that is the bit mask for that bit. This is sometimes referred to as the
        /// Most Significant 1 Bit.
        /// </p>
        /// </summary>
        /// <param name="lng">The <c>long</c> to interrogate.</param>
        /// <returns>The bit mask indicating the highest 1 bit.</returns>
        public static long HighestOneBit(long lng)
        {
            lng |= (lng >> 1);
            lng |= (lng >> 2);
            lng |= (lng >> 4);
            lng |= (lng >> 8);
            lng |= (lng >> 16);
            lng |= (lng >> 32);
            return (lng & ~MathUtil.URS(lng, 1));
        }

        public static String ToString(long l, int radix)
        {
            if (radix < Character.MIN_RADIX || radix > Character.MAX_RADIX)
            {
                radix = 10;
            }
            if (l == 0)
            {
                return "0"; //$NON-NLS-1$
            }

            int count = 2;
            long j = l;
            bool negative = l < 0;
            if (!negative)
            {
                count = 1;
                j = -l;
            }
            while ((l /= radix) != 0)
            {
                count++;
            }

            char[] buffer = new char[count];
            do
            {
                int ch = 0 - (int)(j % radix);
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
            if (negative)
            {
                buffer[0] = '-';
            }
            return new String(buffer, 0, buffer.Length);
        }
    }
}
