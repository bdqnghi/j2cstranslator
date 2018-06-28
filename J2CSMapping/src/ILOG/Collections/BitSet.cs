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

namespace ILOG.J2CsMapping.Collections
{
    using ILOG.J2CsMapping.IO;
    using IOException = System.IO.IOException;
    using IlObjectInputStream = ILOG.J2CsMapping.IO.IlObjectInputStream;
    using IlObjectOutputStream = ILOG.J2CsMapping.IO.IlObjectOutputStream;
    using System.Collections.Generic;
    using System.Collections;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;
    using System;
    using ILOG.J2CsMapping.Util;

    /// <summary>
    /// .NET Replacement for Java BitSet
    /// </summary>
    /// ToBeChecked
    public class BitSet
    {
        private static int OFFSET = 6;

        private static int ELM_SIZE = 1 << OFFSET;

        private static int RIGHT_BITS = ELM_SIZE - 1;

        private static ulong[] TWO_N_ARRAY = new ulong[] { 0x1L, 0x2L, 0x4L,
            0x8L, 0x10L, 0x20L, 0x40L, 0x80L, 0x100L, 0x200L, 0x400L, 0x800L,
            0x1000L, 0x2000L, 0x4000L, 0x8000L, 0x10000L, 0x20000L, 0x40000L,
            0x80000L, 0x100000L, 0x200000L, 0x400000L, 0x800000L, 0x1000000L,
            0x2000000L, 0x4000000L, 0x8000000L, 0x10000000L, 0x20000000L,
            0x40000000L, 0x80000000L, 0x100000000L, 0x200000000L, 0x400000000L,
            0x800000000L, 0x1000000000L, 0x2000000000L, 0x4000000000L,
            0x8000000000L, 0x10000000000L, 0x20000000000L, 0x40000000000L,
            0x80000000000L, 0x100000000000L, 0x200000000000L, 0x400000000000L,
            0x800000000000L, 0x1000000000000L, 0x2000000000000L,
            0x4000000000000L, 0x8000000000000L, 0x10000000000000L,
            0x20000000000000L, 0x40000000000000L, 0x80000000000000L,
            0x100000000000000L, 0x200000000000000L, 0x400000000000000L,
            0x800000000000000L, 0x1000000000000000L, 0x2000000000000000L,
            0x4000000000000000L, 0x8000000000000000L };

        private ulong[] bits;

        private bool needClear;

        private int actualArrayLength;

        private bool isLengthActual;

        /// <summary>
        /// Create a new BitSet with size equal to 64 bits
        /// </summary>
        public BitSet()
        {
            bits = new ulong[1];
            actualArrayLength = 0;
            isLengthActual = true;
        }

        /// <summary>
        /// Create a new BitSet with size equal to nbits. If nbits is not a multiple
        /// of 64, then create a BitSet with size nbits rounded to the next closest
        /// multiple of 64.
        /// </summary>
        /// <param name="nbits">the size of the bit set</param>
        public BitSet(int nbits)
        {
            if (nbits < 0)
            {
                throw new Exception("Negative Array Size");
            }
            bits = new ulong[(nbits >> OFFSET) + ((nbits & RIGHT_BITS) > 0 ? 1 : 0)];
            actualArrayLength = 0;
            isLengthActual = true;
        }

        /// <summary>
        /// Private constructor called from get(int, int) method
        /// </summary>
        /// <param name="bits">the size of the bit set</param>
        /// <param name="needClear"></param>
        /// <param name="actualArrayLength"></param>
        /// <param name="isLengthActual"></param>
        private BitSet(ulong[] bits, bool needClear, int actualArrayLength,
                bool isLengthActual)
        {
            this.bits = bits;
            this.needClear = needClear;
            this.actualArrayLength = actualArrayLength;
            this.isLengthActual = isLengthActual;
        }

        /// <summary>
        /// Create a copy of this BitSet
        /// </summary>
        /// <returns>A copy of this BitSet.</returns>
        public Object Clone()
        {
            try
            {
                BitSet clone = (BitSet)base.MemberwiseClone();
                clone.bits = (ulong[])bits.Clone();
                return clone;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Compares the argument to this BitSet and answer if they are equal. The
        /// object must be an instance of BitSet with the same bits set.
        /// </summary>
        /// <param name="obj">the <code>BitSet</code> object to compare</param>
        /// <returns>A boolean indicating whether or not this BitSet and obj are equal</returns>
        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj is BitSet)
            {
                ulong[] bsBits = ((BitSet)obj).bits;
                int length1 = this.actualArrayLength, length2 = ((BitSet)obj).actualArrayLength;
                if (this.isLengthActual && ((BitSet)obj).isLengthActual
                        && length1 != length2)
                {
                    return false;
                }
                // If one of the BitSets is larger than the other, check to see if
                // any of its extra bits are set. If so return false.
                if (length1 <= length2)
                {
                    for (int i = 0; i < length1; i++)
                    {
                        if (bits[i] != bsBits[i])
                        {
                            return false;
                        }
                    }
                    for (int i = length1; i < length2; i++)
                    {
                        if (bsBits[i] != 0)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < length2; i++)
                    {
                        if (bits[i] != bsBits[i])
                        {
                            return false;
                        }
                    }
                    for (int i = length2; i < length1; i++)
                    {
                        if (bits[i] != 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Increase the size of the internal array to accommodate pos bits. The new
        /// array max index will be a multiple of 64
        /// </summary>
        /// <param name="len">the index the new array needs to be able to access</param>
        private void GrowLength(int len)
        {
            ulong[] tempBits = new ulong[Math.Max(len, bits.Length * 2)];
            System.Array.Copy(bits, 0, tempBits, 0, this.actualArrayLength);
            bits = tempBits;
        }

        /// <summary>
        /// Computes the hash code for this BitSet.
        /// </summary>
        /// <returns>The <code>int</code> representing the hash code for this bitset.</returns>
        public override int GetHashCode()
        {
            ulong x = 1234;
            for (int i = 0, length = actualArrayLength; i < length; i++)
            {
                x ^= bits[i] * (ulong)(i + 1);
            }
            return (int)((x >> 32) ^ x);
        }

        /// <summary>
        /// Retrieve the bit at index pos. Grows the BitSet if pos > size.
        /// </summary>
        /// <param name="pos">the index of the bit to be retrieved</param>
        /// <returns><code>true</code> if the bit at <code>pos</code> is set,<code>false</code> otherwise</returns>
        public bool Get(int pos)
        {
            if (pos < 0)
            {
                // Negative index specified
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            int arrayPos = pos >> OFFSET;
            if (arrayPos < actualArrayLength)
            {
                return (bits[arrayPos] & TWO_N_ARRAY[pos & RIGHT_BITS]) != 0;
            }
            return false;
        }

        /// <summary>
        /// Retrieves the bits starting from pos1 to pos2 and answers back a new
        /// bitset made of these bits. Grows the BitSet if pos2 > size.
        /// </summary>
        /// <param name="pos1">beginning position</param>
        /// <param name="pos2">ending position</param>
        /// <returns>new bitset</returns>
        public BitSet Get(int pos1, int pos2)
        {
            if (pos1 < 0 || pos2 < 0 || pos2 < pos1)
            {
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            int last = actualArrayLength << OFFSET;
            if (pos1 >= last || pos1 == pos2)
            {
                return new BitSet(0);
            }
            if (pos2 > last)
            {
                pos2 = last;
            }

            int idx1 = pos1 >> OFFSET;
            int idx2 = (pos2 - 1) >> OFFSET;
            long factor1 = (~0L) << (pos1 & RIGHT_BITS);
            long factor2 = MathUtil.URS((~0L), (ELM_SIZE - (pos2 & RIGHT_BITS)));

            if (idx1 == idx2)
            {
                ulong result = MathUtil.URS((bits[idx1] & (ulong)(factor1 & factor2)), (pos1 % ELM_SIZE));
                if (result == 0)
                {
                    return new BitSet(0);
                }
                return new BitSet(new ulong[] { result }, needClear, 1, true);
            }
            ulong[] newbits = new ulong[idx2 - idx1 + 1];
            // first fill in the first and last indexes in the new bitset
            newbits[0] = bits[idx1] & (ulong)factor1;
            newbits[newbits.Length - 1] = bits[idx2] & (ulong)factor2;

            // fill in the in between elements of the new bitset
            for (int i = 1; i < idx2 - idx1; i++)
            {
                newbits[i] = bits[idx1 + i];
            }

            // shift all the elements in the new bitset to the right by pos1
            // % ELM_SIZE
            int numBitsToShift = pos1 & RIGHT_BITS;
            int actualLen = newbits.Length;
            if (numBitsToShift != 0)
            {
                for (int i = 0; i < newbits.Length; i++)
                {
                    // shift the current element to the right regardless of
                    // sign
                    newbits[i] = MathUtil.URS(newbits[i], (numBitsToShift));

                    // apply the last x bits of newbits[i+1] to the current
                    // element
                    if (i != newbits.Length - 1)
                    {
                        newbits[i] |= newbits[i + 1] << (ELM_SIZE - (numBitsToShift));
                    }
                    if (newbits[i] != 0)
                    {
                        actualLen = i + 1;
                    }
                }
            }
            return new BitSet(newbits, needClear, actualLen,
                    newbits[actualLen - 1] != 0);
        }
                   
        /// <summary>
        /// Sets the bit at index pos to 1. Grows the BitSet if pos > size.
        /// </summary>
        /// <param name="pos">the index of the bit to set</param>
        public void Set(int pos)
        {
            if (pos < 0)
            {
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            int len = (pos >> OFFSET) + 1;
            if (len > bits.Length)
            {
                GrowLength(len);
            }
            bits[len - 1] |= TWO_N_ARRAY[pos & RIGHT_BITS];
            if (len > actualArrayLength)
            {
                actualArrayLength = len;
                isLengthActual = true;
            }
            NeedClear();
        }

        /// <summary>
        /// Sets the bit at index pos to the value. Grows the BitSet if pos > size.
        /// </summary>
        /// <param name="pos">the index of the bit to set</param>
        /// <param name="val">value to set the bit</param>
        public void Set(int pos, bool val)
        {
            if (val)
            {
                Set(pos);
            }
            else
            {
                Clear(pos);
            }
        }

        /// <summary>
        /// Sets the bits starting from pos1 to pos2. Grows the BitSet if pos2 >
        /// size.
        /// </summary>
        /// <param name="pos1">beginning position</param>
        /// <param name="pos2">ending position</param>
        public void Set(int pos1, int pos2)
        {
            if (pos1 < 0 || pos2 < 0 || pos2 < pos1)
            {
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            if (pos1 == pos2)
            {
                return;
            }
            int len2 = ((pos2 - 1) >> OFFSET) + 1;
            if (len2 > bits.Length)
            {
                GrowLength(len2);
            }

            int idx1 = pos1 >> OFFSET;
            int idx2 = (pos2 - 1) >> OFFSET;
            ulong factor1 = (~0UL) << (pos1 & RIGHT_BITS);
            ulong factor2 = MathUtil.URS((~0UL), (ELM_SIZE - (pos2 & RIGHT_BITS)));

            if (idx1 == idx2)
            {
                bits[idx1] |= (factor1 & factor2);
            }
            else
            {
                bits[idx1] |= factor1;
                bits[idx2] |= factor2;
                for (int i = idx1 + 1; i < idx2; i++)
                {
                    bits[i] |= (~0UL);
                }
            }
            if (idx2 + 1 > actualArrayLength)
            {
                actualArrayLength = idx2 + 1;
                isLengthActual = true;
            }
            NeedClear();
        }

        private void NeedClear()
        {
            this.needClear = true;
        }

        /// <summary>
        /// Sets the bits starting from pos1 to pos2 to the given boolean value.
        /// Grows the BitSet if pos2 > size.
        /// </summary>
        /// <param name="pos1">beginning position</param>
        /// <param name="pos2">ending position</param>
        /// <param name="val">value to set these bits</param>
        public void Set(int pos1, int pos2, bool val)
        {
            if (val)
            {
                Set(pos1, pos2);
            }
            else
            {
                Clear(pos1, pos2);
            }
        }

        /// <summary>
        /// Clears all the bits in this bitset.
        /// </summary>
        public void Clear()
        {
            if (needClear)
            {
                for (int i = 0; i < bits.Length; i++)
                {
                    bits[i] = 0L;
                }
                actualArrayLength = 0;
                isLengthActual = true;
                needClear = false;
            }
        }

        /// <summary>
        /// Clears the bit at index pos. Grows the BitSet if pos > size.
        /// </summary>
        /// <param name="pos">the index of the bit to clear</param>
        public void Clear(int pos)
        {
            if (pos < 0)
            {
                // Negative index specified
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            if (!needClear)
            {
                return;
            }
            int arrayPos = pos >> OFFSET;
            if (arrayPos < actualArrayLength)
            {
                bits[arrayPos] &= ~(TWO_N_ARRAY[pos & RIGHT_BITS]);
                if (bits[actualArrayLength - 1] == 0)
                {
                    isLengthActual = false;
                }
            }
        }

        /// <summary>
        /// Clears the bits starting from pos1 to pos2. Grows the BitSet if pos2 >
        /// size.
        /// </summary>
        /// <param name="pos1">beginning position</param>
        /// <param name="pos2">ending position</param>
        public void Clear(int pos1, int pos2)
        {
            if (pos1 < 0 || pos2 < 0 || pos2 < pos1)
            {
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            if (!needClear)
            {
                return;
            }
            int last = (actualArrayLength << OFFSET);
            if (pos1 >= last || pos1 == pos2)
            {
                return;
            }
            if (pos2 > last)
            {
                pos2 = last;
            }

            int idx1 = pos1 >> OFFSET;
            int idx2 = (pos2 - 1) >> OFFSET;
            ulong factor1 = (~0UL) << (pos1 & RIGHT_BITS);
            ulong factor2 = MathUtil.URS((~0UL), (ELM_SIZE - (pos2 & RIGHT_BITS)));

            if (idx1 == idx2)
            {
                bits[idx1] &= ~(factor1 & factor2);
            }
            else
            {
                bits[idx1] &= ~factor1;
                bits[idx2] &= ~factor2;
                for (int i = idx1 + 1; i < idx2; i++)
                {
                    bits[i] = 0L;
                }
            }
            if ((actualArrayLength > 0) && (bits[actualArrayLength - 1] == 0))
            {
                isLengthActual = false;
            }
        }

        /// <summary>
        /// Flips the bit at index pos. Grows the BitSet if pos > size.
        /// </summary>
        /// <param name="pos">the index of the bit to flip</param>
        public void Flip(int pos)
        {
            if (pos < 0)
            {
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            int len = (pos >> OFFSET) + 1;
            if (len > bits.Length)
            {
                GrowLength(len);
            }
            bits[len - 1] ^= TWO_N_ARRAY[pos & RIGHT_BITS];
            if (len > actualArrayLength)
            {
                actualArrayLength = len;
            }
            isLengthActual = !((actualArrayLength > 0) && (bits[actualArrayLength - 1] == 0));
            NeedClear();
        }

        /// <summary>
        /// Flips the bits starting from pos1 to pos2. Grows the BitSet if pos2 >
        /// size.
        /// </summary>
        /// <param name="pos1">beginning position</param>
        /// <param name="pos2">ending position</param>
        public void Flip(int pos1, int pos2)
        {
            if (pos1 < 0 || pos2 < 0 || pos2 < pos1)
            {
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            if (pos1 == pos2)
            {
                return;
            }
            int len2 = ((pos2 - 1) >> OFFSET) + 1;
            if (len2 > bits.Length)
            {
                GrowLength(len2);
            }

            int idx1 = pos1 >> OFFSET;
            int idx2 = (pos2 - 1) >> OFFSET;
            ulong factor1 = (~0UL) << (pos1 & RIGHT_BITS);
            ulong factor2 = MathUtil.URS((~0UL), (ELM_SIZE - (pos2 & RIGHT_BITS)));

            if (idx1 == idx2)
            {
                bits[idx1] ^= (factor1 & factor2);
            }
            else
            {
                bits[idx1] ^= factor1;
                bits[idx2] ^= factor2;
                for (int i = idx1 + 1; i < idx2; i++)
                {
                    bits[i] ^= (~0UL);
                }
            }
            if (len2 > actualArrayLength)
            {
                actualArrayLength = len2;
            }
            isLengthActual = !((actualArrayLength > 0) && (bits[actualArrayLength - 1] == 0));
            NeedClear();
        }

        /// <summary>
        /// Checks if these two bitsets have at least one bit set to true in the same
        /// position.
        /// </summary>
        /// <param name="bs">BitSet used to calculate intersect</param>
        /// <returns><code>true</code> if bs intersects with this BitSet,<code>false</code> otherwise</returns>
        public bool Intersects(BitSet bs)
        {
            ulong[] bsBits = bs.bits;
            int length1 = actualArrayLength, length2 = bs.actualArrayLength;

            if (length1 <= length2)
            {
                for (int i = 0; i < length1; i++)
                {
                    if ((bits[i] & bsBits[i]) != 0L)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < length2; i++)
                {
                    if ((bits[i] & bsBits[i]) != 0L)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Performs the logical AND of this BitSet with another BitSet.
        /// </summary>
        /// <param name="bs">BitSet to AND with</param>
        public void And(BitSet bs)
        {
            ulong[] bsBits = bs.bits;
            if (!needClear)
            {
                return;
            }
            int length1 = actualArrayLength, length2 = bs.actualArrayLength;
            if (length1 <= length2)
            {
                for (int i = 0; i < length1; i++)
                {
                    bits[i] &= bsBits[i];
                }
            }
            else
            {
                for (int i = 0; i < length2; i++)
                {
                    bits[i] &= bsBits[i];
                }
                for (int i = length2; i < length1; i++)
                {
                    bits[i] = 0;
                }
                actualArrayLength = length2;
            }
            isLengthActual = !((actualArrayLength > 0) && (bits[actualArrayLength - 1] == 0));
        }

        /// <summary>
        /// Clears all bits in the receiver which are also set in the parameter
        /// BitSet.
        /// </summary>
        /// <param name="bs">BitSet to ANDNOT with</param>
        public void AndNot(BitSet bs)
        {
            ulong[] bsBits = bs.bits;
            if (!needClear)
            {
                return;
            }
            int range = actualArrayLength < bs.actualArrayLength ? actualArrayLength
                    : bs.actualArrayLength;
            for (int i = 0; i < range; i++)
            {
                bits[i] &= ~bsBits[i];
            }

            if (actualArrayLength < range)
            {
                actualArrayLength = range;
            }
            isLengthActual = !((actualArrayLength > 0) && (bits[actualArrayLength - 1] == 0));
        }

        /// <summary>
        /// Performs the logical OR of this BitSet with another BitSet.
        /// </summary>
        /// <param name="bs">BitSet to OR with</param>
        public void Or(BitSet bs)
        {
            int bsActualLen = bs.GetActualArrayLength();
            if (bsActualLen > bits.Length)
            {
                ulong[] tempBits = new ulong[bsActualLen];
                System.Array.Copy(bs.bits, 0, tempBits, 0, bs.actualArrayLength);
                for (int i = 0; i < actualArrayLength; i++)
                {
                    tempBits[i] |= bits[i];
                }
                bits = tempBits;
                actualArrayLength = bsActualLen;
                isLengthActual = true;
            }
            else
            {
                ulong[] bsBits = bs.bits;
                for (int i = 0; i < bsActualLen; i++)
                {
                    bits[i] |= bsBits[i];
                }
                if (bsActualLen > actualArrayLength)
                {
                    actualArrayLength = bsActualLen;
                    isLengthActual = true;
                }
            }
            NeedClear();
        }

        /// <summary>
        /// Performs the logical XOR of this BitSet with another BitSet.
        /// </summary>
        /// <param name="bs">BitSet to XOR with</param>
        public void Xor(BitSet bs)
        {
            int bsActualLen = bs.GetActualArrayLength();
            if (bsActualLen > bits.Length)
            {
                ulong[] tempBits = new ulong[bsActualLen];
                System.Array.Copy(bs.bits, 0, tempBits, 0, bs.actualArrayLength);
                for (int i = 0; i < actualArrayLength; i++)
                {
                    tempBits[i] ^= bits[i];
                }
                bits = tempBits;
                actualArrayLength = bsActualLen;
                isLengthActual = !((actualArrayLength > 0) && (bits[actualArrayLength - 1] == 0));
            }
            else
            {
                ulong[] bsBits = bs.bits;
                for (int i = 0; i < bsActualLen; i++)
                {
                    bits[i] ^= bsBits[i];
                }
                if (bsActualLen > actualArrayLength)
                {
                    actualArrayLength = bsActualLen;
                    isLengthActual = true;
                }
            }
            NeedClear();
        }

        /// <summary>
        /// Answers the number of bits this bitset has.
        /// </summary>
        /// <returns>The number of bits contained in this BitSet.</returns>
        public int Size()
        {
            return bits.Length << OFFSET;
        }

        /// <summary>
        /// Returns the number of bits up to and including the highest bit set.
        /// </summary>
        /// <returns>the length of the BitSet</returns>
        public int Length()
        {
            int idx = actualArrayLength - 1;
            while (idx >= 0 && bits[idx] == 0)
            {
                --idx;
            }
            actualArrayLength = idx + 1;
            if (idx == -1)
            {
                return 0;
            }
            int i = ELM_SIZE - 1;
            ulong val = bits[idx];
            while ((val & (TWO_N_ARRAY[i])) == 0 && i > 0)
            {
                i--;
            }
            return (idx << OFFSET) + i + 1;
        }

        private int GetActualArrayLength()
        {
            if (isLengthActual)
            {
                return actualArrayLength;
            }
            int idx = actualArrayLength - 1;
            while (idx >= 0 && bits[idx] == 0)
            {
                --idx;
            }
            actualArrayLength = idx + 1;
            isLengthActual = true;
            return actualArrayLength;
        }

        /// <summary>
        /// Answers a string containing a concise, human-readable description of the
        /// receiver.
        /// </summary>
        /// <returns>A comma delimited list of the indices of all bits that are set.</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder(bits.Length / 2);
            int bitCount = 0;
            sb.Append('{');
            bool comma = false;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == 0)
                {
                    bitCount += ELM_SIZE;
                    continue;
                }
                for (int j = 0; j < ELM_SIZE; j++)
                {
                    if (((bits[i] & (TWO_N_ARRAY[j])) != 0))
                    {
                        if (comma)
                        {
                            sb.Append(", "); //$NON-NLS-1$
                        }
                        sb.Append(bitCount);
                        comma = true;
                    }
                    bitCount++;
                }
            }
            sb.Append('}');
            return sb.ToString();
        }

        /// <summary>
        /// Answers the position of the first bit that is true on or after pos
        /// </summary>
        /// <param name="pos">the starting position (inclusive)</param>
        /// <returns>-1 if there is no bits that are set to true on or after pos.</returns>
        public int NextSetBit(int pos)
        {
            if (pos < 0)
            {
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            if (pos >= actualArrayLength << OFFSET)
            {
                return -1;
            }

            int idx = pos >> OFFSET;
            // first check in the same bit set element
            if (bits[idx] != 0L)
            {
                for (int j = pos & RIGHT_BITS; j < ELM_SIZE; j++)
                {
                    if (((bits[idx] & (TWO_N_ARRAY[j])) != 0))
                    {
                        return (idx << OFFSET) + j;
                    }
                }

            }
            idx++;
            while (idx < actualArrayLength && bits[idx] == 0L)
            {
                idx++;
            }
            if (idx == actualArrayLength)
            {
                return -1;
            }

            // we know for sure there is a bit set to true in this element
            // since the bitset value is not 0L
            for (int j = 0; j < ELM_SIZE; j++)
            {
                if (((bits[idx] & (TWO_N_ARRAY[j])) != 0))
                {
                    return (idx << OFFSET) + j;
                }
            }

            return -1;
        }

        /// <summary>
        /// Answers the position of the first bit that is false on or after pos
        /// </summary>
        /// <param name="pos">the starting position (inclusive)</param>
        /// <returns>the position of the next bit set to false, even if it is further than this bitset's size.</returns>
        public int NextClearBit(int pos)
        {
            if (pos < 0)
            {
                throw new Exception("IndexOutOfBoundsException"); //$NON-NLS-1$
            }

            int length = actualArrayLength;
            int bssize = length << OFFSET;
            if (pos >= bssize)
            {
                return pos;
            }

            int idx = pos >> OFFSET;
            // first check in the same bit set element
            if (bits[idx] != (~0UL))
            {
                for (int j = pos % ELM_SIZE; j < ELM_SIZE; j++)
                {
                    if (((bits[idx] & (TWO_N_ARRAY[j])) == 0))
                    {
                        return idx * ELM_SIZE + j;
                    }
                }
            }
            idx++;
            while (idx < length && bits[idx] == (~0UL))
            {
                idx++;
            }
            if (idx == length)
            {
                return bssize;
            }

            // we know for sure there is a bit set to true in this element
            // since the bitset value is not 0L
            for (int j = 0; j < ELM_SIZE; j++)
            {
                if (((bits[idx] & (TWO_N_ARRAY[j])) == 0))
                {
                    return (idx << OFFSET) + j;
                }
            }

            return bssize;
        }

        /// <summary>
        /// Answers true if all the bits in this bitset are set to false.
        /// </summary>
        /// <returns><code>true</code> if the BitSet is empty, <code>false</code> otherwise</returns>
        public bool IsEmpty()
        {
            if (!needClear)
            {
                return true;
            }
            int length = bits.Length;
            for (int idx = 0; idx < length; idx++)
            {
                if (bits[idx] != 0L)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Answers the number of bits that are true in this bitset.
        /// </summary>
        /// <returns>the number of true bits in the set</returns>
        public int Cardinality()
        {
            if (!needClear)
            {
                return 0;
            }
            int count = 0;
            int length = bits.Length;
            // FIXME: need to test performance, if still not satisfied, change it to
            // 256-bits table based
            for (int idx = 0; idx < length; idx++)
            {
                count += Pop(bits[idx] & 0xffffffffL);
                count += Pop(MathUtil.URS(bits[idx], 32));
            }
            return count;
        }

        private int Pop(ulong x)
        {
            x = x - ((MathUtil.URS(x, 1)) & 0x55555555);
            x = (x & 0x33333333) + ((MathUtil.URS(x, 2)) & 0x33333333);
            x = (x + (MathUtil.URS(x, 4))) & 0x0f0f0f0f;
            x = x + (MathUtil.URS(x, 8));
            x = x + (MathUtil.URS(x, 16));
            return (int)x & 0x0000003f;
        }
    }
}

