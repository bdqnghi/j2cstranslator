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

namespace ILOG.J2CsMapping.Collections.Generics
{
    /// <summary>
    /// Utility methods for Collections
    /// </summary>
    public class Collections : ILOG.J2CsMapping.Collections.Collections
    {

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="other"></param>
        /// <returns></returns>
        static public IList<T> NewList<T, U>(ICollection<U> other) where U : T
        {
            IList<T> list = new List<T>();
            foreach (U obj in other)
            {
                list.Add(obj);
            }
            return list;
        }

        static public IList<T> NewList<T>(IEnumerable other)
        {
            IList<T> list = new List<T>();
            foreach (Object obj in other)
            {
                list.Add((T) obj);
            }
            return list;
        }

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        static public IDictionary<T, U> SynchronizedMap<T, U>(IDictionary<T, U> item)
        {
            return item; // Dictionary<T, U>.Synchronized(item);
        }

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T RemoveFirst<T>(LinkedList<T> list)
        {
            T result = list.First.Value;
            list.RemoveFirst();
            return result;
        }

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IList<T> SubList<T>(IList<T> list, int start, int length)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < length - start; i++)
            {
                result.Add(list[i + start]);
            }
            return result;
        }

        //
        // Sort
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static ICollection<T> Sort<T>(LinkedList<T> list, IComparer<T> comp)
        {
            List<T> alist = new List<T>(list);
            alist.Sort(comp);
            return alist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> Sort<T>(IList<T> list)
        {
            ((List<T>)list).Sort();
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="list"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static IList<U> Sort<T, U>(IList<U> list, IComparer<T> comp) where U : T
        {
            ((List<U>)list).Sort(Adapt<T, U>(comp));
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        public class ComparerAdapter<T, U> : IComparer<U> where U : T
        {
            IComparer<T> comp;

            public ComparerAdapter(IComparer<T> comp)
            {
                this.comp = comp;
            }

            #region IComparer<U> Members

            public int Compare(U x, U y)
            {
                return comp.Compare(x, y);
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static IComparer<U> Adapt<T, U>(IComparer<T> comp) where U : T
        {
            return new ComparerAdapter<T, U>(comp);
        }

        //
        // Put
        //

        public static Object NULL = new Object();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <param name="newvalue"></param>
        /// <returns></returns>
        static public object Put<U, V, A, B>(IDictionary<U, V> table, A key, B newvalue)
            where A : U
            where B : V
        {
           /* if (key == null)
                return null; // hum hum
            object oldvalue = null;
            if (table.ContainsKey(key))
                oldvalue = table[key];
            table[key] = newvalue;
            return oldvalue;*/

            //
            if (key == null)
                return null;
            V oldValue;
            table.TryGetValue((U)key, out oldValue);
            table[key] = newvalue;
            return oldValue;
        }

        //
        // Get
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static public object Get<U, V, A>(IDictionary<U, V> table, A key)
            where A : U
        {
            /*if (key == null)
                return null;
            object oldvalue = null;
            V res = default(V);
            try
            {
                bool success = table.TryGetValue(key, out res);
            }
            catch (Exception)
            {
            }
            if (table.ContainsKey((U)key))
                oldvalue = table[key];
            return oldvalue;*/
            //
            if (key == null)
                return null;
            V oldValue;
            table.TryGetValue(key, out oldValue);         
            return oldValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static public object Get<V>(IDictionary<Int32?, V> table, int key)
        {
            /*object oldvalue = null;
            if (table.ContainsKey(key))
                oldvalue = table[key];
            return oldvalue;*/
            //
            V oldValue;
            table.TryGetValue(key, out oldValue);
            return oldValue;
        }

        // 
        // EmptyList<T>
        // 

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public IList<T> EmptyList<T>()
        {
            return new List<T>();
        }

        //
        // UnmodifiableList<T>
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        static public IList<T> UnmodifiableList<T>(IList<T> list)
        {
            // TODO : the collection is not readonly
            return new List<T>(list);
        }

        //
        // UnmodifiableSet<T>
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        static public ISet<T> UnmodifiableSet<T>(ISet<T> set)
        {
            // TODO : the collection is not readonly
            return new HashedSet<T>(set);
        }

        //
        // SingletonList
        // 

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        static public List<T> SingletonList<T>(T item)
        {
            List<T> list = new List<T>();
            list.Add(item);
            return list;
        }

        //
        // Singleton 
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        static public ISet<T> Singleton<T>(T item)
        {
            HashedSet<T> list = new HashedSet<T>();
            list.Add(item);
            return list;
        }

        //
        // PutAll
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        public static void PutAll<T, U>(IDictionary<T, U> dest, IDictionary<T, U> source)
        {
            foreach (T key in source.Keys)
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
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <param name="rank"></param>
        /// <param name="tobeCopied"></param>
        public static void AddAll<T>(ICollection<T> dest, int rank, ICollection<T> tobeCopied)
        {
            if (dest is List<T>)
            {
                ((List<T>)dest).InsertRange(rank, tobeCopied);
                return;
            }
            throw new NotImplementedException("AddAll<T>(ICollection<T> dest, int rank, ICollection<T> tobeCopied)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddAll<T, U>(ICollection<T> source, ICollection<U> dest) where T : U
        {
            foreach (T elem in source)
            {
                dest.Add(elem);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddAll<T, U>(ICollection<T> source, Stack<U> dest) where T : U
        {
            foreach (T elem in source)
            {
                dest.Push(elem);
            }
            return true;
        }

        // TODO : not generic !
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
                foreach (object elem in source)
                {
                    ((IList) dest).Add(elem);
                }
                return true;
                // return AddAll(source, (IList)dest);
            }
            throw new NotImplementedException("AddAll(ICollection source, ICollection dest)");
        }

        // TODO : not generic !
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddAll(ICollection source, ISet dest)
        {
            foreach (object elem in source)
            {
                dest.Add(elem);
            }
            return true;
        }

        //
        // ContainsAll
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool ContainsAll<T, U>(ICollection<T> source, ICollection<U> dest)
        {
            foreach (object elem in dest)
            {
                if (!source.Contains((T)elem))
                    return false;
            }
            return true;
        }

        //
        // AddRange
        //

        // TODO : not generic !
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool AddRange(IDictionary source, IDictionary dest)
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
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static bool Contains<U, T>(U o, ICollection<T> coll) where U : T
        {
            return coll.Contains(o);
        }

        //
        // ToArray
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="coll"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static U[] ToArray<T, U>(ICollection<T> coll, U[] array)
        {
            IList l = coll as IList;
            if (array.Length < coll.Count)
                array = (U[])Array.CreateInstance(array.GetType().GetElementType(), coll.Count);
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
            ISet<T> s0 = coll as ISet<T>;
            try
            {
                if (s0 != null)
                {
                    int i = 0;
                    foreach (object o in s0)
                    {
                        array[i++] = (U)o;
                    }
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

            int j = 0;
            foreach (T elem in coll)
            {
                object o = elem;
                array[j++] = (U)o;
            }
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static object[] ToArray<T>(ICollection<T> coll)
        {
            object[] result = new object[coll.Count];
            int i = 0;
            foreach (object o in coll)
            {
                result[i++] = o;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static T[] ToArrayGeneric<T>(ICollection<T> coll)
        {
            T[] result = new T[coll.Count];
            int i = 0;
            foreach (T o in coll)
            {
                result[i++] = o;
            }
            return result;
        }

        //
        // clear
        //

        #region Clear

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        public static void Clear<T>(Stack<T> coll)
        {
            coll.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        public static void Clear<T>(ICollection<T> coll)
        {
            coll.Clear();
        }

        #endregion

        //
        // Add
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Add<T, U>(System.Collections.Generic.ICollection<T> coll, U obj) where U : T
        {
            int count = coll.Count;
            coll.Add(obj);
            return count != coll.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Add<T>(System.Collections.Generic.ICollection<T> coll, object obj)
        {
            int count = coll.Count;
            coll.Add((T)obj);
            return count != coll.Count;
        }

        //
        // AddItem
        //

        // TODO : not generic
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool AddItem(IList coll, object obj)
        {
            /*if (coll.Contains(obj))
                return false;
            coll.Add(obj);
            return true;*/
            //
            int count = coll.Count;
            coll.Add(obj);
            return count != coll.Count;
        }

        //
        // Remove
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static T RemoveFirst<T>(IList<T> coll)
        {
            T current = coll[0];

            Remove(coll, current);

            return current;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static T RemoveFirst<T>(ICollection<T> coll)
        {
            IEnumerator<T> et = coll.GetEnumerator();
            et.Reset();
            et.MoveNext();
            T current = et.Current;

            Remove(coll, current);

            return current;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove<T>(IList<T> coll, T obj)
        {
           /* if (!coll.Contains(obj))
                return false;
            coll.Remove(obj);
            return true;*/
            //
            return coll.Remove(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove<T>(ICollection<T> coll, T obj)
        {
            return coll.Remove(obj);
            /*
            if (!coll.Contains(obj))
                return false;
            coll.Remove(obj);
            return true;*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove<T>(IList<T> coll, object obj)
        {
            return coll.Remove((T)obj);
            /*
            if (!coll.Contains((T)obj))
                return false;
            coll.Remove((T)obj);
            return true;*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool RemoveAll<T>(ICollection<T> coll, ICollection<T> obj)
        {
            bool res = true;
            foreach (T elem in obj)
            {
                res |= coll.Remove(elem);
            }
            return res;
        }

        //
        // RetainAll
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool RetainAll<T>(ICollection<T> c1, ICollection<T> c2)
        {
            IList<T> list = new List<T>(c1);
            foreach (T o in list)
            {
                if (!CollectionContains(c2, o))
                {
                    c1.Remove(o);
                }
            }
            return c1.Count != list.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private static bool CollectionContains<T>(ICollection<T> c, T v)
        {
            foreach (T o in c)
            {
                if (object.Equals(o, v))
                {
                    return true;
                }
            }
            return false;
        }

        //
        // Remove
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static U Remove<T, U>(IDictionary<T, U> coll, T obj)
        {
            U val = default(U);
            if (coll.TryGetValue(obj, out val))
                coll.Remove(obj);
            return val;
            /*
            if (coll.ContainsKey(obj))
            {
                U val = coll[obj];
                coll.Remove(obj);
                return val;
            }
            return default(U);*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Remove<T>(ISet<T> coll, T obj)
        {
            if (!coll.Contains(obj))
                return false;
            coll.Remove(obj);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T RemoveAt<T>(IList<T> coll, int index)
        {
            T obj = coll[index];
            coll.RemoveAt(index);
            return obj;
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
        // UnmodifiableMap<T1, T2>
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static IDictionary<T1, T2> UnmodifiableMap<T1, T2>(IDictionary<T1, T2> tasks)
        {
            return new Dictionary<T1, T2>(tasks);
        }

        //
        // EmptySet<T1>
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public static ISet<T1> EmptySet<T1>()
        {
            return new ListSet<T1>();
        }

        //
        // EmptyMap<T, K>
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public static IDictionary<T, K> EmptyMap<T, K>()
        {
            return new Dictionary<T, K>();
        }

        //
        // BinarySearch
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public int BinarySearch<T>(IList<T> list, T name)
        {
            if (list is List<T>)
            {
                List<T> tmp = (List<T>)list;
                int i = tmp.BinarySearch(name);
                return i;
            }
            throw new NotImplementedException("BinarySearch<T>(IList<T> list, T name)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variables"></param>
        /// <param name="name"></param>
        /// <param name="comparator"></param>
        /// <returns></returns>
        public static int BinarySearch<T>(IList<T> variables, T name, IComparer<T> comparator)
        {
            if (variables is List<T>)
            {
                List<T> tmp = (List<T>)variables;
                int i = tmp.BinarySearch(name, comparator);
                return i;
            }
            throw new NotImplementedException("BinarySearch<T>(IList<T> variables, T name, IComparer<T> comparator)");
        }

        //
        // UnmodifiableCollection
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static ICollection<T> UnmodifiableCollection<T>(ICollection<T> errors)
        {
            return errors;
        }

        //
        // SubMap
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="map"></param>
        /// <param name="fromKey"></param>
        /// <param name="toKey"></param>
        /// <returns></returns>
        public static SortedList<T, V> SubMap<T, V>(SortedList<T, V> map, T fromKey, T toKey)
        {
            IDictionary<T, V> subDictionary = new Dictionary<T, V>();
            foreach (T key in map.Keys)
            {
                if (((IComparable<T>)key).CompareTo(fromKey) <= 0 && ((IComparable<T>)key).CompareTo(toKey) >= 0)
                {
                    subDictionary.Add(key, map[key]);
                }
            }
            return new SortedList<T, V>(subDictionary);
        }

        //
        // SingletonMap
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDictionary<T1, T2> SingletonMap<T1, T2>(T1 key, T2 value)
        {
            IDictionary<T1, T2> res = new Dictionary<T1, T2>();
            res[key] = value;
            return res;
        }

        public static IComparer<T> ReverseOrder<T>()
        {
            return new ReverseComparator<T>();
        }

        private class ReverseComparator<T> : IComparer<T>
        {

            public int Compare(T o1, T o2)
            {
                IComparable<T> c2 = (IComparable<T>)o2;
                return c2.CompareTo(o1);
            }
        }
    }
}
