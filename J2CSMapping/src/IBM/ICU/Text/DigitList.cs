//##header J2SE15
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /*
 *******************************************************************************
 * Copyright (C) 1996-2005, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
namespace IBM.ICU.Text {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	/// <summary>
	/// <c>DigitList</c> handles the transcoding between numeric values and
	/// strings of characters. It only represents non-negative numbers. The division
	/// of labor between <c>DigitList</c> and <c>DecimalFormat</c> is
	/// that <c>DigitList</c> handles the radix 10 representation issues and
	/// numeric conversion, including rounding; <c>DecimalFormat</c> handles
	/// the locale-specific issues such as positive and negative representation,
	/// digit grouping, decimal point, currency, and so on.
	/// <p>
	/// A <c>DigitList</c> is a representation of a finite numeric value.
	/// <c>DigitList</c> objects do not represent <c>NaN</c> or infinite
	/// values. A <c>DigitList</c> value can be converted to a
	/// <c>BigDecimal</c> without loss of precision. Conversion to other
	/// numeric formats may involve loss of precision, depending on the specific
	/// value.
	/// <p>
	/// The <c>DigitList</c> representation consists of a string of characters,
	/// which are the digits radix 10, from '0' to '9'. It also has a base 10
	/// exponent associated with it. The value represented by a
	/// <c>DigitList</c> object can be computed by mulitplying the fraction
	/// <em>f</em>, where 0 <= <em>f</em> < 1, derived by placing all the digits of
	/// the list to the right of the decimal point, by 10^exponent.
	/// </summary>
	///
	/// <seealso cref="T:System.Globalization.CultureInfo"/>
	/// <seealso cref="T:ILOG.J2CsMapping.Text.IlFormat"/>
	/// <seealso cref="T:IBM.ICU.Text.NumberFormat"/>
	/// <seealso cref="T:IBM.ICU.Text.DecimalFormat"/>
	/// <seealso cref="T:IBM.ICU.Text.ChoiceFormat"/>
	/// <seealso cref="T:ILOG.J2CsMapping.Text.MessageFormat"/>
	internal sealed class DigitList {
	    public DigitList() {
	        this.decimalAt = 0;
	        this.count = 0;
	        this.digits = new byte[MAX_LONG_DIGITS];
	    }
	
	    /// <summary>
	    /// The maximum number of significant digits in an IEEE 754 double, that is,
	    /// in a Java double. This must not be increased, or garbage digits will be
	    /// generated, and should not be decreased, or accuracy will be lost.
	    /// </summary>
	    ///
	    public const int MAX_LONG_DIGITS = 19; // ==
	                                                  // Long.toString(Long.MAX_VALUE).length()
	
	    public const int DBL_DIG = 17;
	
	    /// <summary>
	    /// These data members are intentionally public and can be set directly.
	    /// The value represented is given by placing the decimal point before
	    /// digits[decimalAt]. If decimalAt is < 0, then leading zeros between the
	    /// decimal point and the first nonzero digit are implied. If decimalAt is >
	    /// count, then trailing zeros between the digits[count-1] and the decimal
	    /// point are implied.
	    /// Equivalently, the represented value is given by f/// 10^decimalAt. Here f
	    /// is a value 0.1 <= f < 1 arrived at by placing the digits in Digits to the
	    /// right of the decimal.
	    /// DigitList is normalized, so if it is non-zero, figits[0] is non-zero. We
	    /// don't allow denormalized numbers because our exponent is effectively of
	    /// unlimited magnitude. The count value contains the number of significant
	    /// digits present in digits[].
	    /// Zero is represented by any DigitList with count == 0 or with each
	    /// digits[i] for all i <= count == '0'.
	    /// </summary>
	    ///
	    public int decimalAt;
	
	    public int count;
	
	    public byte[] digits;
	
	    private void EnsureCapacity(int digitCapacity, int digitsToCopy) {
	        if (digitCapacity > digits.Length) {
	            byte[] newDigits = new byte[digitCapacity * 2];
	            System.Array.Copy((Array)(digits),0,(Array)(newDigits),0,digitsToCopy);
	            digits = newDigits;
	        }
	    }
	
	    /// <summary>
	    /// Return true if the represented number is zero.
	    /// </summary>
	    ///
	    internal bool IsZero() {
	        for (int i = 0; i < count; ++i)
	            if (digits[i] != '0')
	                return false;
	        return true;
	    }
	
	    // Unused as of ICU 2.6 - alan
	    // /**
	    // * Clears out the digits.
	    // * Use before appending them.
	    // * Typically, you set a series of digits with append, then at the point
	    // * you hit the decimal point, you set myDigitList.decimalAt =
	    // myDigitList.count;
	    // * then go on appending digits.
	    // */
	    // public void clear () {
	    // decimalAt = 0;
	    // count = 0;
	    // }
	
	    /// <summary>
	    /// Appends digits to the list.
	    /// </summary>
	    ///
	    public void Append(int digit) {
	        EnsureCapacity(count + 1, count);
	        digits[count++] = (byte) digit;
	    }
	
	    /// <summary>
	    /// Utility routine to get the value of the digit list If (count == 0) this
	    /// throws a NumberFormatException, which mimics Long.parseLong().
	    /// </summary>
	    ///
	    public double GetDouble() {
	        if (count == 0)
	            return 0.0d;
	        StringBuilder temp = new StringBuilder(count);
	        temp.Append('.');
	        for (int i = 0; i < count; ++i)
	            temp.Append((char) (digits[i]));
	        temp.Append('E');
	        temp.Append(ILOG.J2CsMapping.Util.IlNumber.ToString(decimalAt));
	        return ((Double )Double.Parse(temp.ToString(),ILOG.J2CsMapping.Util.NumberFormatProvider.NumberFormat));
	        // long value = Long.parseLong(temp.toString());
	        // return (value * Math.pow(10, decimalAt - count));
	    }
	
	    /// <summary>
	    /// Utility routine to get the value of the digit list. If (count == 0) this
	    /// returns 0, unlike Long.parseLong().
	    /// </summary>
	    ///
	    public long GetLong() {
	        // for now, simple implementation; later, do proper IEEE native stuff
	
	        if (count == 0)
	            return 0;
	
	        // We have to check for this, because this is the one NEGATIVE value
	        // we represent. If we tried to just pass the digits off to parseLong,
	        // we'd get a parse failure.
	        if (IsLongMIN_VALUE())
	            return Int64.MinValue;
	
	        StringBuilder temp = new StringBuilder(count);
	        for (int i = 0; i < decimalAt; ++i) {
	            temp.Append((i < count) ? (char) (digits[i]) : '0');
	        }
	        return ((Int64 )Int64.Parse(temp.ToString(),System.Globalization.NumberStyles.Integer));
	    }
	
	    /// <summary>
	    /// Return a <c>BigInteger</c> representing the value stored in this
	    /// <c>DigitList</c>. This method assumes that this object contains an
	    /// integral value; if not, it will return an incorrect value. [bnf]
	    /// </summary>
	    ///
	    /// <param name="isPositive">determines the sign of the returned result</param>
	    /// <returns>the value of this object as a <c>BigInteger</c></returns>
	    public Int64 GetBigInteger(bool isPositive) {
	        if (IsZero())
	            return 0;
	        if (false) {
	            StringBuilder stringRep = new StringBuilder(count);
	            if (!isPositive) {
	                stringRep.Append('-');
	            }
	            for (int i = 0; i < count; ++i) {
	                stringRep.Append((char) digits[i]);
	            }
	            int d = decimalAt;
	            while (d-- > count) {
	                stringRep.Append('0');
	            }
	            return Int64.Parse(stringRep.ToString());
	        } else {
	            int len = (decimalAt > count) ? decimalAt : count;
	            if (!isPositive) {
	                len += 1;
	            }
	            char[] text = new char[len];
	            int n = 0;
	            if (!isPositive) {
	                text[0] = '-';
	                for (int i_0 = 0; i_0 < count; ++i_0) {
	                    text[i_0 + 1] = (char) digits[i_0];
	                }
	                n = count + 1;
	            } else {
	                for (int i_1 = 0; i_1 < count; ++i_1) {
	                    text[i_1] = (char) digits[i_1];
	                }
	                n = count;
	            }
	            for (int i_2 = n; i_2 < text.Length; ++i_2) {
	                text[i_2] = '0';
	            }
	            return Int64.Parse(ILOG.J2CsMapping.Util.StringUtil.NewString(text));
	        }
	    }
	
	    private String GetStringRep(bool isPositive) {
	        if (IsZero())
	            return "0";
	        StringBuilder stringRep = new StringBuilder(count + 1);
	        if (!isPositive) {
	            stringRep.Append('-');
	        }
	        int d = decimalAt;
	        if (d < 0) {
	            stringRep.Append('.');
	            while (d < 0) {
	                stringRep.Append('0');
	                ++d;
	            }
	            d = -1;
	        }
	        for (int i = 0; i < count; ++i) {
	            if (d == i) {
	                stringRep.Append('.');
	            }
	            stringRep.Append((char) digits[i]);
	        }
	        while (d-- > count) {
	            stringRep.Append('0');
	        }
	        return stringRep.ToString();
	    }
	
	    // #if defined(FOUNDATION10) || defined(J2SE13)
	    // #else
	    /// <summary>
	    /// Return a <c>BigDecimal</c> representing the value stored in this
	    /// <c>DigitList</c>. [bnf]
	    /// </summary>
	    ///
	    /// <param name="isPositive">determines the sign of the returned result</param>
	    /// <returns>the value of this object as a <c>BigDecimal</c></returns>
	    public Decimal GetBigDecimal(bool isPositive) {
	        if (IsZero())
	            return 0M;
	        return Decimal.Parse(GetStringRep(isPositive));
	    }
	
	    // #endif
	
	    /// <summary>
	    /// Return an <c>ICU BigDecimal</c> representing the value stored in
	    /// this <c>DigitList</c>. [bnf]
	    /// </summary>
	    ///
	    /// <param name="isPositive">determines the sign of the returned result</param>
	    /// <returns>the value of this object as a <c>BigDecimal</c></returns>
	    public IBM.ICU.Math.BigDecimal GetBigDecimalICU(bool isPositive) {
	        if (IsZero())
	            return IBM.ICU.Math.BigDecimal.ValueOf(0);
	        return new IBM.ICU.Math.BigDecimal(GetStringRep(isPositive));
	    }
	
	    /// <summary>
	    /// Return whether or not this objects represented value is an integer. [bnf]
	    /// </summary>
	    ///
	    /// <returns>true if the represented value of this object is an integer</returns>
	    internal bool IsIntegral() {
	        // Trim trailing zeros. This does not change the represented value.
	        while (count > 0 && digits[count - 1] == (byte) '0')
	            --count;
	        return count == 0 || decimalAt >= count;
	    }
	
	    // Unused as of ICU 2.6 - alan
	    // /**
	    // * Return true if the number represented by this object can fit into
	    // * a long.
	    // */
	    // boolean fitsIntoLong(boolean isPositive)
	    // {
	    // // Figure out if the result will fit in a long. We have to
	    // // first look for nonzero digits after the decimal point;
	    // // then check the size. If the digit count is 18 or less, then
	    // // the value can definitely be represented as a long. If it is 19
	    // // then it may be too large.
	    //
	    // // Trim trailing zeros. This does not change the represented value.
	    // while (count > 0 && digits[count - 1] == (byte)'0') --count;
	    //
	    // if (count == 0) {
	    // // Positive zero fits into a long, but negative zero can only
	    // // be represented as a double. - bug 4162852
	    // return isPositive;
	    // }
	    //
	    // if (decimalAt < count || decimalAt > MAX_LONG_DIGITS) return false;
	    //
	    // if (decimalAt < MAX_LONG_DIGITS) return true;
	    //
	    // // At this point we have decimalAt == count, and count ==
	    // MAX_LONG_DIGITS.
	    // // The number will overflow if it is larger than 9223372036854775807
	    // // or smaller than -9223372036854775808.
	    // for (int i=0; i<count; ++i)
	    // {
	    // byte dig = digits[i], max = LONG_MIN_REP[i];
	    // if (dig > max) return false;
	    // if (dig < max) return true;
	    // }
	    //
	    // // At this point the first count digits match. If decimalAt is less
	    // // than count, then the remaining digits are zero, and we return true.
	    // if (count < decimalAt) return true;
	    //
	    // // Now we have a representation of Long.MIN_VALUE, without the leading
	    // // negative sign. If this represents a positive value, then it does
	    // // not fit; otherwise it fits.
	    // return !isPositive;
	    // }
	
	    // Unused as of ICU 2.6 - alan
	    // /**
	    // * Set the digit list to a representation of the given double value.
	    // * This method supports fixed-point notation.
	    // * @param source Value to be converted; must not be Inf, -Inf, Nan,
	    // * or a value <= 0.
	    // * @param maximumFractionDigits The most fractional digits which should
	    // * be converted.
	    // */
	    // public final void set(double source, int maximumFractionDigits)
	    // {
	    // set(source, maximumFractionDigits, true);
	    // }
	
	    /// <summary>
	    /// Set the digit list to a representation of the given double value. This
	    /// method supports both fixed-point and exponential notation.
	    /// </summary>
	    ///
	    /// <param name="source">Value to be converted; must not be Inf, -Inf, Nan, or a value<= 0.</param>
	    /// <param name="maximumDigits">The most fractional or total digits which should be converted.</param>
	    /// <param name="fixedPoint">If true, then maximumDigits is the maximum fractional digitsto be converted. If false, total digits.</param>
	    internal void Set(double source, int maximumDigits, bool fixedPoint) {
	        if (source == 0)
	            source = 0;
	        // Generate a representation of the form DDDDD, DDDDD.DDDDD, or
	        // DDDDDE+/-DDDDD.
	        String rep = String.Concat(source);
	
	        Set(rep, MAX_LONG_DIGITS);
	
	        if (fixedPoint) {
	            // The negative of the exponent represents the number of leading
	            // zeros between the decimal and the first non-zero digit, for
	            // a value < 0.1 (e.g., for 0.00123, -decimalAt == 2). If this
	            // is more than the maximum fraction digits, then we have an
	            // underflow
	            // for the printed representation.
	            if (-decimalAt > maximumDigits) {
	                count = 0;
	                return;
	            } else if (-decimalAt == maximumDigits) {
	                if (ShouldRoundUp(0)) {
	                    count = 1;
	                    ++decimalAt;
	                    digits[0] = (byte) '1';
	                } else {
	                    count = 0;
	                }
	                return;
	            }
	            // else fall through
	        }
	
	        // Eliminate trailing zeros.
	        while (count > 1 && digits[count - 1] == '0')
	            --count;
	
	        // Eliminate digits beyond maximum digits to be displayed.
	        // Round up if appropriate.
	        Round((fixedPoint) ? (maximumDigits + decimalAt)
	                : (maximumDigits == 0) ? -1 : maximumDigits);
	    }
	
	    /// <summary>
	    /// Given a string representation of the form DDDDD, DDDDD.DDDDD, or
	    /// DDDDDE+/-DDDDD, set this object's value to it. Ignore any leading '-'.
	    /// </summary>
	    ///
	    private void Set(String rep, int maxCount) {
	        decimalAt = -1;
	        count = 0;
	        int exponent = 0;
	        // Number of zeros between decimal point and first non-zero digit after
	        // decimal point, for numbers < 1.
	        int leadingZerosAfterDecimal = 0;
	        bool nonZeroDigitSeen = false;
	        // Skip over leading '-'
	        int i = 0;
	        if (rep[i] == '-') {
	            ++i;
	        }
	        for (; i < rep.Length; ++i) {
	            char c = rep[i];
	            if (c == '.') {
	                decimalAt = count;
	            } else if (c == 'e' || c == 'E') {
	                ++i;
	                // Integer.parseInt doesn't handle leading '+' signs
	                if (rep[i] == '+') {
	                    ++i;
	                }
	                exponent = ((Int32 )Int32.Parse(rep.Substring(i)));
	                break;
	            } else if (count < maxCount) {
	                if (!nonZeroDigitSeen) {
	                    nonZeroDigitSeen = (c != '0');
	                    if (!nonZeroDigitSeen && decimalAt != -1) {
	                        ++leadingZerosAfterDecimal;
	                    }
	                }
	
	                if (nonZeroDigitSeen) {
	                    EnsureCapacity(count + 1, count);
	                    digits[count++] = (byte) c;
	                }
	            }
	        }
	        if (decimalAt == -1) {
	            decimalAt = count;
	        }
	        decimalAt += exponent - leadingZerosAfterDecimal;
	    }
	
	    /// <summary>
	    /// Return true if truncating the representation to the given number of
	    /// digits will result in an increment to the last digit. This method
	    /// implements half-even rounding, the default rounding mode. [bnf]
	    /// </summary>
	    ///
	    /// <param name="maximumDigits">the number of digits to keep, from 0 to <c>count-1</c>.If 0, then all digits are rounded away, and this methodreturns true if a one should be generated (e.g., formatting0.09 with "#.#").</param>
	    /// <returns>true if digit <c>maximumDigits-1</c> should be incremented</returns>
	    private bool ShouldRoundUp(int maximumDigits) {
	        // variable not used boolean increment = false;
	        // Implement IEEE half-even rounding
	        /*
	         * Bug 4243108 format(0.0) gives "0.1" if preceded by parse("99.99")
	         * [Richard/GCL]
	         */
	        if (maximumDigits < count) {
	            if (digits[maximumDigits] > '5') {
	                return true;
	            } else if (digits[maximumDigits] == '5') {
	                for (int i = maximumDigits + 1; i < count; ++i) {
	                    if (digits[i] != '0') {
	                        return true;
	                    }
	                }
	                return maximumDigits > 0
	                        && (digits[maximumDigits - 1] % 2 != 0);
	            }
	        }
	        return false;
	    }
	
	    /// <summary>
	    /// Round the representation to the given number of digits.
	    /// </summary>
	    ///
	    /// <param name="maximumDigits">The maximum number of digits to be shown. Upon return, countwill be less than or equal to maximumDigits. This now performsrounding when maximumDigits is 0, formerly it did not.</param>
	    public void Round(int maximumDigits) {
	        // Eliminate digits beyond maximum digits to be displayed.
	        // Round up if appropriate.
	        // [bnf] rewritten to fix 4179818
	        if (maximumDigits >= 0 && maximumDigits < count) {
	            if (ShouldRoundUp(maximumDigits)) {
	                // Rounding up involves incrementing digits from LSD to MSD.
	                // In most cases this is simple, but in a worst case situation
	                // (9999..99) we have to adjust the decimalAt value.
	                for (;;) {
	                    --maximumDigits;
	                    if (maximumDigits < 0) {
	                        // We have all 9's, so we increment to a single digit
	                        // of one and adjust the exponent.
	                        digits[0] = (byte) '1';
	                        ++decimalAt;
	                        maximumDigits = 0; // Adjust the count
	                        break;
	                    }
	
	                    ++digits[maximumDigits];
	                    if (digits[maximumDigits] <= '9')
	                        break;
	                    // digits[maximumDigits] = '0'; // Unnecessary since we'll
	                    // truncate this
	                }
	                ++maximumDigits; // Increment for use as count
	            }
	            count = maximumDigits;
	            /*
	             * Bug 4217661 DecimalFormat formats 1.001 to "1.00" instead of "1"
	             * Eliminate trailing zeros. [Richard/GCL]
	             */
	            while (count > 1 && digits[count - 1] == '0') {
	                --count;
	            } // [Richard/GCL]
	        }
	    }
	
	    /// <summary>
	    /// Utility routine to set the value of the digit list from a long
	    /// </summary>
	    ///
	    public void Set(long source) {
	        Set(source, 0);
	    }
	
	    /// <summary>
	    /// Set the digit list to a representation of the given long value.
	    /// </summary>
	    ///
	    /// <param name="source">Value to be converted; must be >= 0 or == Long.MIN_VALUE.</param>
	    /// <param name="maximumDigits">The most digits which should be converted. If maximumDigits islower than the number of significant digits in source, therepresentation will be rounded. Ignored if <= 0.</param>
	    public void Set(long source, int maximumDigits) {
	        // This method does not expect a negative number. However,
	        // "source" can be a Long.MIN_VALUE (-9223372036854775808),
	        // if the number being formatted is a Long.MIN_VALUE. In that
	        // case, it will be formatted as -Long.MIN_VALUE, a number
	        // which is outside the legal range of a long, but which can
	        // be represented by DigitList.
	        // [NEW] Faster implementation
	        if (source <= 0) {
	            if (source == Int64.MinValue) {
	                decimalAt = count = MAX_LONG_DIGITS;
	                System.Array.Copy((Array)(LONG_MIN_REP),0,(Array)(digits),0,count);
	            } else {
	                count = 0;
	                decimalAt = 0;
	            }
	        } else {
	            int left = MAX_LONG_DIGITS;
	            int right;
	            while (source > 0) {
	                digits[--left] = (byte) (((long) '0') + (source % 10));
	                source /= 10;
	            }
	            decimalAt = MAX_LONG_DIGITS - left;
	            // Don't copy trailing zeros
	            // we are guaranteed that there is at least one non-zero digit,
	            // so we don't have to check lower bounds
	            for (right = MAX_LONG_DIGITS - 1; digits[right] == (byte) '0'; --right) {
	            }
	            count = right - left + 1;
	            System.Array.Copy((Array)(digits),left,(Array)(digits),0,count);
	        }
	        if (maximumDigits > 0)
	            Round(maximumDigits);
	    }
	
	    /// <summary>
	    /// Set the digit list to a representation of the given BigInteger value.
	    /// [bnf]
	    /// </summary>
	    ///
	    /// <param name="source">Value to be converted</param>
	    /// <param name="maximumDigits">The most digits which should be converted. If maximumDigits islower than the number of significant digits in source, therepresentation will be rounded. Ignored if <= 0.</param>
	    public void Set2(Int64 source, int maximumDigits) {
	        String stringDigits = source.ToString();
	
	        count = decimalAt = stringDigits.Length;
	
	        // Don't copy trailing zeros
	        while (count > 1 && stringDigits[count - 1] == '0')
	            --count;
	
	        int offset = 0;
	        if (stringDigits[0] == '-') {
	            ++offset;
	            --count;
	            --decimalAt;
	        }
	
	        EnsureCapacity(count, 0);
	        for (int i = 0; i < count; ++i) {
	            digits[i] = (byte) stringDigits[i + offset];
	        }
	
	        if (maximumDigits > 0)
	            Round(maximumDigits);
	    }
	
	    /// <summary>
	    /// Internal method that sets this digit list to represent the given value.
	    /// The value is given as a String of the format returned by BigDecimal.
	    /// </summary>
	    ///
	    /// <param name="stringDigits">value to be represented with the following syntax, expressedas a regular expression: -?\d///.?\d/// Must not be an emptystring.</param>
	    /// <param name="maximumDigits">The most digits which should be converted. If maximumDigits islower than the number of significant digits in source, therepresentation will be rounded. Ignored if <= 0.</param>
	    /// <param name="fixedPoint">If true, then maximumDigits is the maximum fractional digitsto be converted. If false, total digits.</param>
	    private void SetBigDecimalDigits(String stringDigits, int maximumDigits,
	            bool fixedPoint) {
	        // | // Find the first non-zero digit, the decimal, and the last
	        // non-zero digit.
	        // | int first=-1, last=stringDigits.length()-1, decimal=-1;
	        // | for (int i=0; (first<0 || decimal<0) && i<=last; ++i) {
	        // | char c = stringDigits.charAt(i);
	        // | if (c == '.') {
	        // | decimal = i;
	        // | } else if (first < 0 && (c >= '1' && c <= '9')) {
	        // | first = i;
	        // | }
	        // | }
	        // |
	        // | if (first < 0) {
	        // | clear();
	        // | return;
	        // | }
	        // |
	        // | // At this point we know there is at least one non-zero digit, so
	        // the
	        // | // following loop is safe.
	        // | for (;;) {
	        // | char c = stringDigits.charAt(last);
	        // | if (c != '0' && c != '.') {
	        // | break;
	        // | }
	        // | --last;
	        // | }
	        // |
	        // | if (decimal < 0) {
	        // | decimal = stringDigits.length();
	        // | }
	        // |
	        // | count = last - first;
	        // | if (decimal < first || decimal > last) {
	        // | ++count;
	        // | }
	        // | decimalAt = decimal - first;
	        // | if (decimalAt < 0) {
	        // | ++decimalAt;
	        // | }
	        // |
	        // | ensureCapacity(count, 0);
	        // | for (int i = 0; i < count; ++i) {
	        // | digits[i] = (byte) stringDigits.charAt(first++);
	        // | if (first == decimal) {
	        // | ++first;
	        // | }
	        // | }
	
	        // The maxDigits here could also be Integer.MAX_VALUE
	        Set(stringDigits, stringDigits.Length);
	
	        // Eliminate digits beyond maximum digits to be displayed.
	        // Round up if appropriate.
	        // {dlf} Some callers depend on passing '0' to round to mean 'don't
	        // round', but
	        // rather than pass that information explicitly, we rely on some magic
	        // with maximumDigits
	        // and decimalAt. Unfortunately, this is no good, because there are
	        // cases where maximumDigits
	        // is zero and we do want to round, e.g. BigDecimal values -1 < x < 1.
	        // So since round
	        // changed to perform rounding when the argument is 0, we now force the
	        // argument
	        // to -1 in the situations where it matters.
	        Round((fixedPoint) ? (maximumDigits + decimalAt)
	                : (maximumDigits == 0) ? -1 : maximumDigits);
	    }
	
	    // #if defined(FOUNDATION10) || defined(J2SE13)
	    // #else
	    /// <summary>
	    /// Set the digit list to a representation of the given BigDecimal value.
	    /// [bnf]
	    /// </summary>
	    ///
	    /// <param name="source">Value to be converted</param>
	    /// <param name="maximumDigits">The most digits which should be converted. If maximumDigits islower than the number of significant digits in source, therepresentation will be rounded. Ignored if <= 0.</param>
	    /// <param name="fixedPoint">If true, then maximumDigits is the maximum fractional digitsto be converted. If false, total digits.</param>
	    public void Set(Decimal source, int maximumDigits,
	            bool fixedPoint) {
	        SetBigDecimalDigits(source.ToString(), maximumDigits, fixedPoint);
	    }
	
	    // #endif
	
	    /*
	     * Set the digit list to a representation of the given BigDecimal value.
	     * [bnf]
	     * 
	     * @param source Value to be converted
	     * 
	     * @param maximumDigits The most digits which should be converted. If
	     * maximumDigits is lower than the number of significant digits in source,
	     * the representation will be rounded. Ignored if <= 0.
	     * 
	     * @param fixedPoint If true, then maximumDigits is the maximum fractional
	     * digits to be converted. If false, total digits.
	     */
	    public void Set(IBM.ICU.Math.BigDecimal source,
	            int maximumDigits, bool fixedPoint) {
	        SetBigDecimalDigits(source.ToString(), maximumDigits, fixedPoint);
	    }
	
	    /// <summary>
	    /// Returns true if this DigitList represents Long.MIN_VALUE; false,
	    /// otherwise. This is required so that getLong() works.
	    /// </summary>
	    ///
	    private bool IsLongMIN_VALUE() {
	        if (decimalAt != count || count != MAX_LONG_DIGITS)
	            return false;
	
	        for (int i = 0; i < count; ++i) {
	            if (digits[i] != LONG_MIN_REP[i])
	                return false;
	        }
	
	        return true;
	    }
	
	    private static byte[] LONG_MIN_REP;
	
	    // (The following boilerplate methods are currently not called,
	    // and cannot be called by tests since this class is
	    // package-private. The methods may be useful in the future, so
	    // we do not delete them. 2003-06-11 ICU 2.6 Alan)
	    // /CLOVER:OFF
	    /// <summary>
	    /// equality test between two digit lists.
	    /// </summary>
	    ///
	    public override bool Equals(Object obj) {
	        if ((Object) this == obj) // quick check
	            return true;
	        if (!(obj  is  DigitList)) // (1) same object?
	            return false;
	        DigitList other = (DigitList) obj;
	        if (count != other.count || decimalAt != other.decimalAt)
	            return false;
	        for (int i = 0; i < count; i++)
	            if (digits[i] != other.digits[i])
	                return false;
	        return true;
	    }
	
	    /// <summary>
	    /// Generates the hash code for the digit list.
	    /// </summary>
	    ///
	    public override int GetHashCode() {
	        int hashcode = decimalAt;
	
	        for (int i = 0; i < count; i++)
	            hashcode = hashcode * 37 + digits[i];
	
	        return hashcode;
	    }
	
	    public override String ToString() {
	        if (IsZero())
	            return "0";
	        StringBuilder buf = new StringBuilder("0.");
	        for (int i = 0; i < count; ++i)
	            buf.Append((char) digits[i]);
	        buf.Append("x10^");
	        buf.Append(decimalAt);
	        return buf.ToString();
	    }
	    // /CLOVER:ON
	
	    static DigitList() {
	            String s = Int64.MinValue.ToString();
	            LONG_MIN_REP = new byte[MAX_LONG_DIGITS];
	            for (int i = 0; i < MAX_LONG_DIGITS; ++i) {
	                LONG_MIN_REP[i] = (byte) s[i + 1];
	            }
	        }
	}
}
