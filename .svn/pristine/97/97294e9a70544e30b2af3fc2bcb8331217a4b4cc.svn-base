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

namespace ILOG.J2CsMapping.Collections
{
    /// <summary>
    /// .NET Replacement for Java LinkedList
    /// </summary>
    public class LinkedList : ExtendedCollectionBase, System.Collections.IList, IExtendedCollection, ICloneable
    {
        private LinkedList<object> list;

        /// <summary>
        /// 
        /// </summary>
        public LinkedList()
        {
            list = new LinkedList<object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public LinkedList(System.Collections.ICollection c)
            : this()
        {
            foreach (object o in c)
            {
                list.AddLast(o);
            }
        }

        //
        //
        //

        /// <summary>
        /// 
        /// </summary>
        protected override System.Collections.ICollection InnerCollection
        {
            get
            {
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LinkedListNode<object> First
        {
            get
            {
                return list.First;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LinkedListNode<object> Last
        {
            get
            {
                return list.Last;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void AddFirst(object e)
        {
            list.AddFirst(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void AddLast(object e)
        {
            list.AddLast(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object RemoveFirst()
        {
            object result = list.First.Value;
            list.RemoveFirst();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object RemoveLast()
        {
            object result = list.Last.Value;
            list.RemoveLast();
            return result;
        }

        #region IExtendedCollection Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool Add(object e)
        {
            list.AddLast(e);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool Contains(object e)
        {
            return list.Contains(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool Remove(object e)
        {
            return list.Remove(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool RetainAll(System.Collections.ICollection c)
        {
            LinkedListNode<object> node = list.First;
            int count = list.Count;
            while (node != null)
            {
                if (!CollectionContains(c, node.Value))
                {
                    LinkedListNode<object> next = node.Next;
                    list.Remove(node);
                    node = next;
                }
                else
                {
                    node = node.Next;
                }
            }
            return list.Count != count;
        }

        private static bool CollectionContains(System.Collections.ICollection c, object v)
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

        #endregion

        #region IList Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int System.Collections.IList.Add(object value)
        {
            list.AddLast(value);
            return list.Count - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(object value)
        {
            int i = 0;
            foreach (object o in list)
            {
                if (object.Equals(o, value))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(int index, object value)
        {
            if (index == list.Count)
            {
                list.AddLast(value);
            }
            LinkedListNode<object> node = GetNodeAt(index);
            list.AddBefore(node, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        void System.Collections.IList.Remove(object value)
        {
            list.Remove(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            LinkedListNode<object> node = GetNodeAt(index);
            list.Remove(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                LinkedListNode<object> node = GetNodeAt(index);
                return node.Value;
            }
            set
            {
                LinkedListNode<object> node = GetNodeAt(index);
                list.AddAfter(node, value);
                list.Remove(node);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private LinkedListNode<object> GetNodeAt(int index)
        {
            if (index < 0 || index >= list.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            LinkedListNode<object> node;
            if (index < list.Count / 2)
            {
                node = list.First;
                for (int i = 0; i < index; i++)
                {
                    node = node.Next;
                }
            }
            else
            {
                node = list.Last;
                for (int i = list.Count - 1; i > index; --i)
                {
                    node = node.Previous;
                }
            }
            return node;
        }

        #region ICloneable Members

        public object Clone()
        {
            return new LinkedList(this);
        }

        #endregion
    }
}
