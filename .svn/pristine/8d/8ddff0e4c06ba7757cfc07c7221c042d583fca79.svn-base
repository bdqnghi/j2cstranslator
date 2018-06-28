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

    using ILOG.J2CsMapping.Collections;
    using ILOG.J2CsMapping.Collections.Generics;
    using Collections = ILOG.J2CsMapping.Collections.Generics.Collections;
    using HashedSet = ILOG.J2CsMapping.Collections.HashedSet;
    using IIterator = ILOG.J2CsMapping.Collections.IIterator;
    using ISet = ILOG.J2CsMapping.Collections.ISet;
    using ILOG.J2CsMapping.IO;
    using System;
    using System.Collections;
    using ConcurrentModificationException = System.Exception;
    using System.Collections.Generic;
    using ICollection = System.Collections.ICollection;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Type = System.Type;
    using System.Xml;
    using ILOG.J2CsMapping.Util;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public class HugeEnumSet<E> : EnumSet<E>
    {

        private E[] enums;

        private long[] bits;

        private int size;

        private static int BIT_IN_LONG = 64;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementType"></param>
        public HugeEnumSet(Type elementType)
            : base(elementType)
        {
            enums = (E[])Enum.GetValues(elementType);
            bits = new long[(enums.Length + BIT_IN_LONG - 1) / BIT_IN_LONG];
            ILOG.J2CsMapping.Collections.Arrays.Fill(bits, 0);
        }

        private class HugeEnumSetIterator : IIterator<E>
        {

            private long[] unProcessedBits;
            private HugeEnumSet<E> outer;
            private int bitsPosition = 0;

            /*
             * Mask for current element.
             */
            private long currentElementMask = 0;

            bool canProcess = true;

            public HugeEnumSetIterator(HugeEnumSet<E> outer)
            {
                this.outer = outer;
                unProcessedBits = new long[outer.bits.Length];
                Array.Copy(outer.bits, 0, unProcessedBits, 0, outer.bits.Length);
                bitsPosition = unProcessedBits.Length;
                FindNextNoneZeroPosition(0);
                if (bitsPosition == unProcessedBits.Length)
                {
                    canProcess = false;
                }
            }

            private void FindNextNoneZeroPosition(int start)
            {
                for (int i = start; i < unProcessedBits.Length; i++)
                {
                    if (0 != outer.bits[i])
                    {
                        bitsPosition = i;
                        break;
                    }
                }
            }

            public bool HasNext()
            {
                return canProcess;
            }

            public E Next()
            {
                if (!canProcess)
                {
                    throw new Exception("NoSuchElementException");
                }
                currentElementMask = unProcessedBits[bitsPosition]
                        & (-unProcessedBits[bitsPosition]);
                unProcessedBits[bitsPosition] -= currentElementMask;
                int index = Int64Helper.NumberOfTrailingZeros(currentElementMask)
                        + bitsPosition * BIT_IN_LONG;
                if (0 == unProcessedBits[bitsPosition])
                {
                    int oldBitsPosition = bitsPosition;
                    FindNextNoneZeroPosition(bitsPosition + 1);
                    if (bitsPosition == oldBitsPosition)
                    {
                        canProcess = false;
                    }
                }
                return outer.enums[index];
            }

            public void Remove()
            {
                if (currentElementMask == 0)
                {
                    throw new Exception("IllegalStateException");
                }
                outer.bits[bitsPosition] &= ~currentElementMask;
                outer.size--;
                currentElementMask = 0;
            }

           
            #region IIterator Members

            bool IIterator.HasNext()
            {
                throw new NotImplementedException();
            }

            object IIterator.Next()
            {
                throw new NotImplementedException();
            }

            void IIterator.Remove()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override bool Add(E element)
        {
            if (!IsValidType((Type)((object)element).GetType()))
            {
                throw new Exception("ClassCastException");
            }
            CalculateElementIndex(element);

            bits[bitsIndex] |= (1L << elementInBits);
            if (oldBits == bits[bitsIndex])
            {
                return false;
            }
            size++;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public override bool AddAll(ICollection<E> collection)
        {
            if (0 == collection.Count || this == collection)
            {
                return false;
            }
            if (collection is EnumSet<E>)
            {
                EnumSet<E> set = (EnumSet<E>)collection;
                if (!IsValidType(set.elementClass))
                {
                    throw new Exception("ClassCastException");
                }
                HugeEnumSet<E> hugeSet = (HugeEnumSet<E>)set;
                bool addSuccessful = false;
                for (int i = 0; i < bits.Length; i++)
                {
                    oldBits = bits[i];
                    bits[i] |= hugeSet.bits[i];
                    if (oldBits != bits[i])
                    {
                        addSuccessful = true;
                        size = size - Int64Helper.BitCount(oldBits)
                                + Int64Helper.BitCount(bits[i]);
                    }
                }
                return addSuccessful;
            }
            return base.AddAll(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Count
        {
            get { return size; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            ILOG.J2CsMapping.Collections.Arrays.Fill(bits, 0);
            size = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Complement()
        {
            if (0 != enums.Length)
            {
                bitsIndex = enums.Length / BIT_IN_LONG;

                size = 0;
                int bitCount = 0;
                for (int i = 0; i <= bitsIndex; i++)
                {
                    bits[i] = ~bits[i];
                    bitCount = Int64Helper.BitCount(bits[i]);
                    size += bitCount;
                }
                bits[bitsIndex] &= MathUtil.URS(-1L, (BIT_IN_LONG - enums.Length
                        % BIT_IN_LONG));
                size -= bitCount;
                bitCount = Int64Helper.BitCount(bits[bitsIndex]);
                size += bitCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Contains(E obj)
        {
            if (null == obj)
            {
                return false;
            }
            if (!IsValidType(obj.GetType()))
            {
                return false;
            }
            CalculateElementIndex((E)obj);
            return (bits[bitsIndex] & (1L << elementInBits)) != 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override EnumSet<E> Clone()
        {
            Object set = base.Clone();
            if (null != set)
            {
                ((HugeEnumSet<E>)set).bits = (long[])bits.Clone();
                return (HugeEnumSet<E>)set;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public override bool ContainsAll(ICollection<E> collection)
        {
            if (collection.Count == 0)
            {
                return true;
            }
            if (collection is HugeEnumSet<E>)
            {
                HugeEnumSet<E> set = (HugeEnumSet<E>)collection;
                if (IsValidType(set.elementClass))
                {
                    for (int i = 0; i < bits.Length; i++)
                    {
                        if ((bits[i] & set.bits[i]) != set.bits[i])
                        {
                            return false;
                        }

                    }
                    return true;
                }
            }
            return !(collection is EnumSet<E>) && base.ContainsAll(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (null == obj)
            {
                return false;
            }
            EnumSet<E> set = (EnumSet<E>)obj;
            if (!IsValidType(set.elementClass))
            {
                return base.Equals(obj);
            }
            return Arrays.AreEquals(bits, ((HugeEnumSet<E>)obj).bits);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IIterator<E> Iterator()
        {
            return new HugeEnumSetIterator(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<E> GetEnumerator()
        {
            return new IEnumeratorAdapter<E>(Iterator());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Remove(E obj)
        {
            if (!Contains(obj))
            {
                return false;
            }
            bits[bitsIndex] -= (1L << elementInBits);
            size--;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public override bool RemoveAll(ICollection<E> collection)
        {
            if (0 == collection.Count)
            {
                return false;
            }

            if (collection is EnumSet<E>)
            {
                EnumSet<E> set = (EnumSet<E>)collection;
                if (!IsValidType(set.elementClass))
                {
                    return false;
                }
                bool removeSuccessful = false;
                long mask = 0;
                for (int i = 0; i < bits.Length; i++)
                {
                    oldBits = bits[i];
                    mask = bits[i] & ((HugeEnumSet<E>)set).bits[i];
                    if (mask != 0)
                    {
                        bits[i] -= mask;
                        size = (size - Int64Helper.BitCount(oldBits) + Int64Helper.BitCount(bits[i]));
                        removeSuccessful = true;
                    }
                }
                return removeSuccessful;
            }
            return base.RemoveAll(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public override bool RetainAll(ICollection<E> collection)
        {
            if (collection is EnumSet<E>)
            {
                EnumSet<E> set = (EnumSet<E>)collection;
                if (!IsValidType(set.elementClass))
                {
                    Clear();
                    return true;
                }

                bool retainSuccessful = false;
                oldBits = 0;
                for (int i = 0; i < bits.Length; i++)
                {
                    oldBits = bits[i];
                    bits[i] &= ((HugeEnumSet<E>)set).bits[i];
                    if (oldBits != bits[i])
                    {
                        size = size - Int64Helper.BitCount(oldBits)
                                + Int64Helper.BitCount(bits[i]);
                        retainSuccessful = true;
                    }
                }
                return retainSuccessful;
            }
            return base.RetainAll(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public override void SetRange(E start, E end)
        {
            CalculateElementIndex(start);
            int startBitsIndex = bitsIndex;
            int startElementInBits = elementInBits;
            CalculateElementIndex(end);
            int endBitsIndex = bitsIndex;
            int endElementInBits = elementInBits;
            long range = 0;
            if (startBitsIndex == endBitsIndex)
            {
                range = MathUtil.URS(-1L, (BIT_IN_LONG - (endElementInBits - startElementInBits + 1))) << startElementInBits;
                size -= Int64Helper.BitCount(bits[bitsIndex]);
                bits[bitsIndex] |= range;
                size += Int64Helper.BitCount(bits[bitsIndex]);
            }
            else
            {
                range = MathUtil.URS(-1L, startElementInBits) << startElementInBits;
                size -= Int64Helper.BitCount(bits[startBitsIndex]);
                bits[startBitsIndex] |= range;
                size += Int64Helper.BitCount(bits[startBitsIndex]);

                // endElementInBits + 1 is the number of consecutive ones.
                // 63 - endElementInBits is the following zeros of the right most one.
                range = MathUtil.URS(-1L, (BIT_IN_LONG - (endElementInBits + 1)) << (63 - endElementInBits));
                size -= Int64Helper.BitCount(bits[endBitsIndex]);
                bits[endBitsIndex] |= range;
                size += Int64Helper.BitCount(bits[endBitsIndex]);
                for (int i = (startBitsIndex + 1); i <= (endBitsIndex - 1); i++)
                {
                    size -= Int64Helper.BitCount(bits[i]);
                    bits[i] = -1L;
                    size += Int64Helper.BitCount(bits[i]);
                }
            }
        }

        private void CalculateElementIndex(E element)
        {
            int elementOrdinal = (int)(object)element;
            bitsIndex = elementOrdinal / BIT_IN_LONG;
            elementInBits = elementOrdinal % BIT_IN_LONG;
            oldBits = bits[bitsIndex];
        }

        private int bitsIndex;

        private int elementInBits;

        private long oldBits;
    }
}
