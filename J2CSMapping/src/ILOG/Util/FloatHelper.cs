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
    public class FloatHelper
    {
        /**
     * Returns a representation of the specified floating-point value
     * according to the IEEE 754 floating-point "single format" bit
     * layout, preserving Not-a-Number (NaN) values.
     * <p>
     * Bit 31 (the bit that is selected by the mask 
     * <code>0x80000000</code>) represents the sign of the floating-point 
     * number. 
     * Bits 30-23 (the bits that are selected by the mask 
     * <code>0x7f800000</code>) represent the exponent. 
     * Bits 22-0 (the bits that are selected by the mask 
     * <code>0x007fffff</code>) represent the significand (sometimes called 
     * the mantissa) of the floating-point number. 
     * <p>If the argument is positive infinity, the result is 
     * <code>0x7f800000</code>. 
     * <p>If the argument is negative infinity, the result is 
     * <code>0xff800000</code>.
     * <p>
     * If the argument is NaN, the result is the integer representing
     * the actual NaN value.  Unlike the <code>floatToIntBits</code>
     * method, <code>floatToRawIntBits</code> does not collapse all the
     * bit patterns encoding a NaN to a single &quot;canonical&quot;
     * NaN value.
     * <p>
     * In all cases, the result is an integer that, when given to the
     * {@link #intBitsToFloat(int)} method, will produce a
     * floating-point value the same as the argument to
     * <code>floatToRawIntBits</code>.
     * @param   value   a floating-point number.
     * @return the bits that represent the floating-point number.
     * @since 1.3
     */
        public static int FloatToRawIntBits(float value)
        {
            int num2 = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            return num2;
        }
    }
}
