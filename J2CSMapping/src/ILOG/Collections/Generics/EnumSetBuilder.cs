using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG.J2CsMapping.Collections.Generics
{
    public class EnumSetBuilder
    {
        public static EnumSet<Z> Of<Z>(Z a) 
        {
            return EnumSet<Z>.Of(a);
        }

        public static EnumSet<Z> Of<Z>(Z a, params Z[] b) 
        {
            return EnumSet<Z>.Of(a, b);
        }

        public static EnumSet<Z> NoneOf<Z>(Type a) 
        {
            return EnumSet<Z>.NoneOf<Z>(a);
        }

        public static EnumSet<U> Range<U>(U a, U b) 
        {
            return EnumSet<U>.Range<U>(a, b); ;
        }

        public static EnumSet<U> CopyOf<U>(IList<U> semModifiers) 
        {
            return EnumSet<U>.CopyOf<U>(semModifiers);
        }

        public static EnumSet<U> CopyOf<U>(ISet<U> semModifiers)
        {
            return EnumSet<U>.CopyOf<U>(semModifiers);
        }

        public static EnumSet<U> ComplementOf<U>(EnumSet<U> enumSet) 
        {
            return EnumSet<U>.ComplementOf<U>(enumSet);
        }

        public static EnumSet<U> AllOf<U>(System.Type a)
        {
            return EnumSet<U>.AllOf<U>(a);
        }
    }

    /*
    public abstract class EnumSet2<T> : IList<T>, IEnumerable<T>
    {

        public IEnumerator<T> GetEnumerator()
        {
            return null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return null;
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, T item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public T this[int index]
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(T item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(T item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        public EnumSet2<T> Clone()
        {
            // TODO: throw new Exception("The method or operation is not implemented.");
            return this;
        }

        public void RetainAll(EnumSet<T> enumSet)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }*/
}
