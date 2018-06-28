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

    /// <summary>
    /// .NET replacement for java enumset
    /// <p/>
    /// A specialized <see cref="T:ILOG.J2CsMapping.Collections.Generics.ISet"/> implementation for use with enum types.  All of
    /// the elements in an enum set must come from a single enum type that is
    /// specified, explicitly or implicitly, when the set is created.  Enum sets
    /// are represented internally as bit vectors.  This representation is
    /// extremely compact and efficient. The space and time performance of this
    /// class should be good enough to allow its use as a high-quality, typesafe
    /// alternative to traditional <tt>int</tt>-based "bit flags."  Even bulk
    /// operations (such as <tt>containsAll</tt> and <tt>retainAll</tt>) should
    /// run very quickly if their argument is also an enum set.
    /// <p>The iterator returned by the <tt>iterator</tt> method traverses the
    /// elements in their <i>natural order</i> (the order in which the enum
    /// constants are declared).  The returned iterator is <i>weakly
    /// consistent</i>: it will never throw <see cref="T:System.Exception"/>and it may or may not show the effects of any modifications to the set that
    /// occur while the iteration is in progress.
    /// <p>Null elements are not permitted.  Attempts to insert a null element
    /// will throw <see cref="T:System.NullReferenceException"/>.  Attempts to test for the
    /// presence of a null element or to remove one will, however, function
    /// properly.
    /// <p>Implementation note: All basic operations execute in constant time.
    /// They are likely (though not guaranteed) to be much faster than their<see cref="null"/> counterparts.  Even bulk operations execute in
    /// constant time if their argument is also an enum set.
    /// </summary>
    /// 
    /// <typeparam name="E">the type of elements maintained by this enumset</typeparam>
    public abstract class EnumSet<E> : ListSet<E>, ICloneable
    {

        public Type elementClass;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cls"></param>
        public EnumSet(Type cls)
        {
            elementClass = cls;
        }

        /// <summary>
        /// Creates an empty enum set. The permitted elements are of type Type.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static EnumSet<E> NoneOf<E>(Type elementType)
        {
            if (!elementType.IsEnum)
            {
                throw new Exception("ClassCastException");
            }
            if (Enum.GetValues(elementType).Length <= 64)
            {
                return new MiniEnumSet<E>(elementType);
            }
            return new HugeEnumSet<E>(elementType);
        }

        /// <summary>
        /// Creates an enum set. Element is contained in this enum set if and only if
        /// it is a member of the specified element type.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static EnumSet<E> AllOf<E>(Type elementType)
        {
            EnumSet<E> set = NoneOf<E>(elementType);
            set.Complement();
            return set;
        }

        /// <summary>
        /// Creates an enum set. All the contained elements are of type Class<E>,
        /// and the contained elements are the same as those contained in s.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static EnumSet<E> CopyOf<E>(EnumSet<E> s)
        {
            EnumSet<E> set = EnumSet<E>.NoneOf<E>(s.elementClass);
            set.AddAll(s);
            return set;
        }

        /// <summary>
        /// Creates an enum set. The contained elements are the same as those
        /// contained in collection c. If c is an enum set, invoking this method is
        /// the same as invoking {@link #copyOf(EnumSet)}.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="c"></param>
        /// <returns></returns>
        public static EnumSet<E> CopyOf<E>(ICollection<E> c)
        {
            if (c is EnumSet<E>)
            {
                return CopyOf((EnumSet<E>)c);
            }
            if (0 == c.Count)
            {
                throw new Exception("IllegalArgumentException");
            }
            IEnumerator<E> iterator = c.GetEnumerator();
            iterator.MoveNext();
            E element = iterator.Current;
            EnumSet<E> set = EnumSet<E>.NoneOf<E>((Type)((object)element).GetType());
            set.Add(element);
            while (iterator.MoveNext())
            {
                set.Add(iterator.Current);
            }
            return set;
        }

        /// <summary>
        /// Creates an enum set. All the contained elements complement those from the
        /// specified enum set.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static EnumSet<E> ComplementOf<E>(EnumSet<E> s)
        {
            EnumSet<E> set = EnumSet<E>.NoneOf<E>(s.elementClass);
            set.AddAll(s);
            set.Complement();
            return set;
        }

        public abstract void Complement();

        /// <summary>
        /// Creates a new enum set, containing only the specified element. There are
        /// six overloadings of the method. They accept from one to five elements
        /// respectively. The sixth one receives arbitrary number of elements, and
        /// runs slower than those only receive fixed number of elements.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static EnumSet<E> Of<E>(E e)
        {
            EnumSet<E> set = EnumSet<E>.NoneOf<E>(((object)e).GetType());
            set.Add(e);
            return set;
        }

        /// <summary>
        /// Creates a new enum set, containing only the specified elements. There are
        /// six overloadings of the method. They accept from one to five elements
        /// respectively. The sixth one receives arbitrary number of elements, and
        /// runs slower than those only receive fixed number of elements.
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static EnumSet<E> Of(E e1, E e2)
        {
            EnumSet<E> set = Of<E>(e1);
            set.Add(e2);
            return set;
        }

        /// <summary>
        /// Creates a new enum set, containing only the specified elements. There are
        /// six overloadings of the method. They accept from one to five elements
        /// respectively. The sixth one receives arbitrary number of elements, and
        /// runs slower than those only receive fixed number of elements.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="e3"></param>
        /// <returns></returns>
        public static EnumSet<E> Of<E>(E e1, E e2, E e3)
        {
            EnumSet<E> set = Of<E>(e1, e2);
            set.Add(e3);
            return set;
        }

        /// <summary>
        /// Creates a new enum set, containing only the specified elements. There are
        /// six overloadings of the method. They accept from one to five elements
        /// respectively. The sixth one receives arbitrary number of elements, and
        /// runs slower than those only receive fixed number of elements.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="e3"></param>
        /// <param name="e4"></param>
        /// <returns></returns>
        public static EnumSet<E> Of<E>(E e1, E e2, E e3, E e4)
        {
            EnumSet<E> set = Of<E>(e1, e2, e3);
            set.Add(e4);
            return set;
        }

        /// <summary>
        /// Creates a new enum set, containing only the specified elements. There are
        /// six overloadings of the method. They accept from one to five elements
        /// respectively. The sixth one receives arbitrary number of elements, and
        /// runs slower than those only receive fixed number of elements.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="e3"></param>
        /// <param name="e4"></param>
        /// <param name="e5"></param>
        /// <returns></returns>
        public static EnumSet<E> Of<E>(E e1, E e2, E e3, E e4, E e5)
        {
            EnumSet<E> set = Of<E>(e1, e2, e3, e4);
            set.Add(e5);
            return set;
        }

        /// <summary>
        /// Creates a new enum set, containing only the specified elements. It
        /// receives arbitrary number of elements, and runs slower than those only
        /// receive fixed number of elements.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="start"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static EnumSet<E> Of<E>(E start, params E[] others)
        {
            EnumSet<E> set = Of<E>(start);
            foreach (E e in others)
            {
                set.Add(e);
            }
            return set;
        }

        /// <summary>
        /// Creates an enum set containing all the elements within the range defined
        /// by start and end (inclusive). All the elements must be in order.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static EnumSet<E> Range<E>(E start, E end)
        {
            if ((int)(object)start - (int)(object)end > 0)
            {
                throw new Exception("IllegalArgumentException");
            }
            EnumSet<E> set = EnumSet<E>.NoneOf<E>((Type)((object)start).GetType());
            set.SetRange(start, end);
            return set;
        }

        public abstract void SetRange(E start, E end);

        /// <summary>
        /// Creates a new enum set with the same elements as those contained in this
        /// enum set.
        /// </summary>
        /// <returns></returns>
        public virtual EnumSet<E> Clone()
        {
            try
            {
                Object set = base.MemberwiseClone();
                return (EnumSet<E>)set;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        object ICloneable.Clone()
        {
            return ((EnumSet<E>)this).Clone();
        }

        public bool IsValidType(Type cls)
        {
            return cls == elementClass || cls.BaseType == elementClass;
        }

        public override E[] ToArray()
        {
            E[] result = new E[Count];
            int i = 0;
            foreach (E o in this)
            {
                result[i++] = o;
            }
            return result;
        }
    }
}
