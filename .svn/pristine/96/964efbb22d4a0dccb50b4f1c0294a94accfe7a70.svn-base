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
using System.Collections;
using System.Collections.Generic;

namespace ILOG.J2CsMapping.Collections
{
    /// <summary>
    /// Utility methods for Collections.
    /// </summary>
    public class Collections
    {
        static public IList EMPTY_LIST = new ArrayList();
        static public ISet EMPTY_SET = new HashedSet();
        static public IDictionary EMPTY_MAP = new Hashtable();


        #region IExtendedCollection utility methods

        //
        // Add
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool Add(IExtendedCollection c, object e)
        {
            return c.Add(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool AddAll(ICollection c1, IExtendedCollection c2)
        {
            return c2.AddAll(c1);
        }

        //
        // Clear
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public static void Clear(IExtendedCollection c)
        {
            c.Clear();
        }

        //
        // Contains
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool Contains(object e, IExtendedCollection c)
        {
            return c.Contains(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool ContainsAll(IExtendedCollection c1, ICollection c2)
        {
            return c1.ContainsAll(c2);
        }

        //
        // Remove
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool Remove(IExtendedCollection c, object e)
        {
            if (e == null || !c.Contains(e))
            {
                return false;
            }
            return c.Remove(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool RemoveAll(IExtendedCollection c1, ICollection c2)
        {
            return c1.RemoveAll(c2);
        }

        //
        // Retain
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool RetainAll(IExtendedCollection c1, ICollection c2)
        {
            return c1.RetainAll(c2);
        }

        public static bool RetainAll(IList c1, ICollection c2)
        {
            IList list = new ArrayList(c1);
            foreach (Object o in list)
            {
                if (!CollectionContains(c2, o))
                {
                    c1.Remove(o);
                }
            }
            return c1.Count != list.Count;
        }

        private static bool CollectionContains(ICollection c, object v)
        {
            foreach (object o in c)
            {
                if (object.Equals(o, v))
                {
                    return true;
                }
            }
            return false;
        }

        //
        // ToArray
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static object[] ToArray(IExtendedCollection c)
        {
            return c.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static object[] ToArray(IExtendedCollection c, object[] arr)
        {
            return c.ToArray(arr);
        }

        #endregion

        //
        // Equals
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool Equals(IList list1, IList list2)
        {
            if (list2 == null)
            {
                return false;
            }
            if (list1 == list2)
            {
                return true;
            }
            IEnumerator e1 = list1.GetEnumerator();
            IEnumerator e2 = list2.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext())
            {
                if (!object.Equals(e1.Current, e2.Current))
                {
                    return false;
                }
            }
            return list1.Count == list2.Count;
        }

        //
        // SubList
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="idx1"></param>
        /// <param name="idx2"></param>
        /// <returns></returns>
        public static IList SubList(IList list, int idx1, int idx2)
        {
            ArrayList alist = list as ArrayList;
            if (alist != null)
            {
                return alist.GetRange(idx1, idx2 - idx1);
            }
            else
            {
                throw new NotImplementedException("SubList(IList list, int idx1, int idx2)");
            }
        }

        //
        // Sort
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public static void Sort(IList list)
        {
            if (list is ArrayList)
                ((ArrayList)list).Sort();
            else
                throw new NotImplementedException("Sort(IList list)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="comp"></param>
        public static void Sort(IList list, IComparer comp)
        {
            if (list is ArrayList)
                ((ArrayList)list).Sort(comp);
            else
                throw new NotImplementedException("Sort(IList list, IComparer comp)");
        }

        //
        // Put
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <param name="newvalue"></param>
        /// <returns></returns>
        static public object Put(IDictionary table, object key, object newvalue)
        {
            object oldvalue = table[key];
            table[key] = newvalue;
            return oldvalue;
        }

        //
        // Get
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static public object Get(IDictionary table, object key)
        {
            return key == null ? null : table[key];
        }

        //
        // EmptyList
        //

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public IList EmptyList()
        {
            return EMPTY_LIST;
        }

        //
        // Unmodifiable 
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static public IList UnmodifiableList(IList list)
        {
            return ArrayList.ReadOnly(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        static public ISet UnmodifiableSet(ISet set)
        {
            return HashedSet.ReadOnly(set);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static IDictionary UnmodifiableMap(IDictionary tasks)
        {
            return tasks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static ICollection UnmodifiableCollection(ICollection coll)
        {
            return coll;
        }

        //
        // Singleton
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        static public IList SingletonList(object item)
        {
            ArrayList list = new ArrayList();
            list.Add(item);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        static public ISet Singleton(object item)
        {
            ISet list = new HashedSet();
            list.Add(item);
            return list;
        }

        //
        // Synchronized
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        static public Hashtable SynchronizedMap(Hashtable item)
        {
            return Hashtable.Synchronized(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        static public Dictionary<String, String> SynchronizedMap(Dictionary<String, String> item)
        {
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        static public Dictionary<object, object> SynchronizedMap(Dictionary<object, object> item)
        {
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        static public Dictionary<String, ICollection<String>> SynchronizedMap(Dictionary<String, ICollection<String>> item)
        {
            return item;
        }

        //
        // BinarySearch
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public int BinarySearch(IList list, Object name)
        {
            if (list is ArrayList)
            {
                ArrayList tmp = (ArrayList)list;
                int i = tmp.BinarySearch(name);
                return i;
            }
            throw new NotImplementedException("BinarySearch(IList list, Object name)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="name"></param>
        /// <param name="VARIABLE_COMPARATOR"></param>
        /// <returns></returns>
        public static int BinarySearch(ICollection variables, Object name, IComparer VARIABLE_COMPARATOR)
        {
            ArrayList arrayList = variables as ArrayList;
            if (arrayList != null)
            {
                return arrayList.BinarySearch(name, VARIABLE_COMPARATOR);
            }
            throw new NotImplementedException("BinarySearch(ICollection variables, Object name, IComparer VARIABLE_COMPARATOR)");
        }

        //
        // PutAll
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        public static void PutAll(IDictionary dest, IDictionary source)
        {
            foreach (object key in source.Keys)
            {
                dest[key] = source[key];
            }
        }

        //
        // AddAll
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="rank"></param>
        /// <param name="tobeCopied"></param>
        public static void AddAll(ICollection dest, int rank, ICollection tobeCopied)
        {
            if (dest is ArrayList)
            {
                ((ArrayList)dest).InsertRange(rank, tobeCopied);
                return;
            }
            throw new NotImplementedException("AddAll(ICollection dest, int rank, ICollection tobeCopied)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddAll(ICollection source, ICollection dest)
        {
            if (dest is IList)
            {
                return AddAll(source, (IList)dest);
            }
            throw new NotImplementedException("AddAll(ICollection source, ICollection dest)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddAll(ICollection source, IList dest)
        {
            foreach (object elem in source)
            {
                dest.Add(elem);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddAll(ICollection source, LinkedList dest)
        {
            return dest.AddAll(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddAll(ICollection source, HashedSet dest)
        {
            return dest.AddAll(source);
        }

        //
        // ContainsAll
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool ContainsAll(ArrayList source, ICollection dest)
        {
            foreach (object elem in dest)
            {
                if (!source.Contains(elem))
                    return false;
            }
            return true;
        }

        //
        // RemoveAll
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool RemoveAll(IList source, ICollection dest)
        {
            foreach (object elem in dest)
            {
                source.Remove(elem);
            }
            return true;
        }

        //
        // AddRange
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddRange(Hashtable source, Hashtable dest)
        {
            foreach (string elem in dest.Keys)
            {
                source[elem] = dest[elem];
            }
            return true;
        }

        //
        // Contains
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static bool Contains(object o, ICollection coll)
        {
            Array array = coll as Array;
            if (array != null)
            {
                foreach (object i in coll)
                {
                    if (i != null)
                    {
                        if (i is ValueType && o is ValueType)
                        {

                        }
                        else
                        {
                            if (i.Equals(o))
                                return true;
                        }
                    }
                    else if (o == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            IList al = coll as IList;
            if (al != null)
            {
                return al.Contains(o);
            }
            Hashtable ha = coll as Hashtable;
            if (ha != null)
            {
                return ha.Contains(o);
            }
            IDictionary d = coll as IDictionary;
            if (d != null)
            {
                return d.Contains(o);
            }
            Console.WriteLine(coll);
            //
            throw new NotImplementedException("Contains");
           // return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ContainsAll(ICollection a, ICollection b)
        {
            for (IEnumerator e = b.GetEnumerator(); e.MoveNext(); )
            {
                if (!Contains(e.Current, a))
                {
                    return false;
                }
            }
            return true;
        }

        //
        // ToArray
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static object[] ToArray(ICollection coll, object[] array)
        {
            IList l = coll as IList;
            if (array.Length < coll.Count)
                array = (object[])Array.CreateInstance(array.GetType().GetElementType(), coll.Count);
            try
            {
                if (l != null)
                {
                    l.CopyTo(array, 0);
                    return array;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            ISet s = coll as ISet;
            try
            {
                if (s != null)
                {
                    s.CopyTo(array, 0);
                    return array;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            ICollection dict = coll as ICollection;
            if (dict != null)
            {
                dict.CopyTo(array, 0);
                return array;
            }
            throw new NotImplementedException("ToArray(ICollection coll, object[] array)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static object[] ToArray(ICollection coll)
        {
            IList l = coll as IList;
            if (l != null)
            {
                object[] array = new object[l.Count];
                l.CopyTo(array, 0);
                return array;
            }
            throw new NotImplementedException("ToArray(ICollection coll)");
        }

        //
        // Clear
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        public static void Clear(ICollection coll)
        {
            if (coll is IList)
            {
                ((IList)coll).Clear();
                return;
            }
            if (coll is ISet)
            {
                ((ISet)coll).Clear();
                return;
            }
            if (coll is IDictionary)
            {
                ((IDictionary)coll).Clear();
                return;
            }
            if (coll is Stack)
            {
                ((Stack)coll).Clear();
                return;
            }
            throw new NotImplementedException("Clear(ICollection coll) not implemented for type " + coll.GetType().FullName);
        }

        //
        // Add
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Add(ICollection coll, object obj)
        {
            if (coll is IList)
            {
                ((IList)coll).Add(obj);
                return true;
            }
            if (coll is ISet)
            {
                return ((ISet)coll).Add(obj);
            }
            throw new NotImplementedException("Add(ICollection coll, object obj)");
        }

        // ArrayList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool AddItem(ArrayList coll, object obj)
        {
            if (coll.Contains(obj))
                return false;
            coll.Add(obj);
            return true;
        }

        //
        // Remove
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove(ICollection coll, object obj)
        {
            IList list = coll as IList;
            if (list != null)
                return Remove(list, obj);
            throw new NotImplementedException("Remove(ICollection coll, object obj)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove(IList coll, object obj)
        {
            if (!coll.Contains(obj))
                return false;
            coll.Remove(obj);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Remove(IDictionary coll, object obj)
        {
            object val = coll[obj];
            coll.Remove(obj);
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove(ISet coll, object obj)
        {
            if (obj == null || !coll.Contains(obj))
                return false;
            coll.Remove(obj);
            return true;
        }

        //
        // RemoveAt
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object RemoveAt(ArrayList coll, int index)
        {
            object obj = coll[index];
            coll.RemoveAt(index);
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object RemoveAt(IList coll, int index)
        {
            object result = coll[index];
            coll.RemoveAt(index);
            return result;
        }

        //
        // BitArray
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        public static void Or(BitArray b1, BitArray b2)
        {
            if (b1.Length > b2.Length)
            {
                b2.Length = b1.Length;
            }
            else if (b2.Length > b1.Length)
            {
                b1.Length = b2.Length;
            }
            b1.Or(b2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static BitArray And(BitArray b1, BitArray b2)
        {
            if (b1.Length > b2.Length)
            {
                b2.Length = b1.Length;
            }
            else if (b2.Length > b1.Length)
            {
                b1.Length = b2.Length;
            }
            b1.And(b2);
            return b1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static BitArray AndNot(BitArray b1, BitArray b2)
        {
            if (b1.Length > b2.Length)
            {
                b2.Length = b1.Length;
            }
            else if (b2.Length > b1.Length)
            {
                b1.Length = b2.Length;
            }
            b1.And(b2.Not());
            return b1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool Get(BitArray b, int pos)
        {
            if (b.Length <= pos)
                return false;
            else
                return b.Get(pos);
        }

        //
        // HeadMap
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public static SortedList HeadMap(SortedList map, Object from)
        {
            SortedList res = new SortedList();
            int index = map.IndexOfKey(from);
            for (int i = 0; i < index; i++)
            {
                object key = map.GetKey(i);
                object val = map[key];
                res[key] = val;
            }
            return res;
        }

        //
        // EmptySet
        //

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ISet EmptySet()
        {
            throw new Exception("EmptySet()");
        }

        //
        // Reverse
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        public static void Reverse(IList l)
        {
            int idx1 = 0;
            int idx2 = l.Count - 1;
            while (idx1 < idx2)
            {
                object item = l[idx1];
                l[idx1] = l[idx2];
                l[idx2] = item;
                idx1++;
                --idx2;
            }
        }

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindings"></param>
        /// <returns></returns>
        internal static IIterator Enumeration(ICollection bindings)
        {
            return new IteratorAdapter(bindings.GetEnumerator());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static ICollection List(object p)
        {
            throw new Exception("List(object p)");
        }
    }
}
