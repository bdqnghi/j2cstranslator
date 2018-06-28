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
    public class MiniEnumSet<E> : EnumSet<E> 
    {
        private static int MAX_ELEMENTS = 64;

        private int size;

        private E[] enums;

        private long bits;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementType"></param>
        public MiniEnumSet(Type elementType)
            : base(elementType)
        {
            enums = (E[]) Enum.GetValues(elementType);
        }

        private class MiniEnumSetIterator : IIterator<E>
        {

            private long unProcessedBits;

            private MiniEnumSet<E> outer;

            /*
             * Mask for current element.
             */
            private long currentElementMask;

            private bool canProcess = true;

            public MiniEnumSetIterator(MiniEnumSet<E> outer)
            {
                this.outer = outer;
                unProcessedBits = outer.bits;
                if (0 == unProcessedBits)
                {
                    canProcess = false;
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
                currentElementMask = unProcessedBits & (-unProcessedBits);
                unProcessedBits -= currentElementMask;
                if (0 == unProcessedBits)
                {
                    canProcess = false;
                }
                return outer.enums[Int64Helper.NumberOfTrailingZeros(currentElementMask)];
            }

            public void Remove()
            {
                if (currentElementMask == 0)
                {
                    throw new Exception("IllegalStateException");
                }
                outer.bits &= ~currentElementMask;
                outer.size = Int64Helper.BitCount(outer.bits);
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
        /// <returns></returns>
        public IIterator<E> Iterator()
        {
            return new MiniEnumSetIterator(this);
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
        public override int Count
        {
            get { return size; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            bits = 0;
            size = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override bool Add(E element)
        {
            if (!IsValidType((Type) ((object) element).GetType()))
            {
                throw new Exception("ClassCastException");
            }
            long mask = 1L << (int) (object) element;
            if ((bits & mask) == mask)
            {
                return false;
            }
            bits |= mask;

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
            if (0 == collection.Count)
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
                MiniEnumSet<E> miniSet = (MiniEnumSet<E>)set;
                long oldBits = bits;
                bits |= miniSet.bits;
                size = Int64Helper.BitCount(bits);
                return (oldBits != bits);
            }
            return base.AddAll(collection);
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
            Enum element = (Enum) (object) obj;
            int ordinal = (int)(object) element;
            return (bits & (1L << ordinal)) != 0;
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
            if (collection is MiniEnumSet<E>)
            {
                MiniEnumSet<E> set = (MiniEnumSet<E>)collection;
                return IsValidType(set.elementClass) && ((bits & set.bits) == set.bits);
            }
            return !(collection is EnumSet<E>) && base.ContainsAll(collection);
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
                bool removeSuccessful = false;
                if (IsValidType(set.elementClass))
                {
                    long mask = bits & ((MiniEnumSet<E>)set).bits;
                    if (mask != 0)
                    {
                        bits -= mask;
                        size = Int64Helper.BitCount(bits);
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
                long oldBits = bits;
                bits &= ((MiniEnumSet<E>)set).bits;
                if (oldBits != bits)
                {
                    size = Int64Helper.BitCount(bits);
                    retainSuccessful = true;
                }
                return retainSuccessful;
            }
            return base.RetainAll(collection);
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
            Enum element = (Enum) (object) obj;
            int ordinal = (int)(object)element;
            bits -= (1L << ordinal);
            size--;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (!(obj is EnumSet<E>))
            {
                return base.Equals(obj);
            }
            EnumSet<E> set = (EnumSet<E>)obj;
            if (!IsValidType(set.elementClass))
            {
                return size == 0 && set.Count == 0;
            }
            return bits == ((MiniEnumSet<E>)set).bits;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Complement()
        {
            if (0 != enums.Length)
            {
                bits = ~bits;
                bits &= MathUtil.URS(-1L, (MAX_ELEMENTS - enums.Length));
                size = enums.Length - size;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public override void SetRange(E start, E end)
        {
            int length = (int)(object)end - (int)(object)start + 1;
            long range = MathUtil.URS(-1L, (MAX_ELEMENTS - length)) << (int)(object)start;
            bits |= range;
            size = Int64Helper.BitCount(bits);
        }
    }
}

