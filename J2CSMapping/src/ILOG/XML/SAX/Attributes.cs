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

namespace ILOG.J2CsMapping.XML.Sax
{

    public class Attributes : IAttributes
    {
        internal int length;
        internal string[] data;

        public Attributes()
            : base()
        {
            length = 0;
            data = null;
        }

        public Attributes(IAttributes attributes)
            : base()
        {
            this.SetAttributes(attributes);
        }

        public int GetLength()
        {
            return length;
        }

        public string GetURI(int i)
        {
            if (i >= 0 && i < length)
                return data[i * 5];
            return null;
        }

        public string GetLocalName(int i)
        {
            if (i >= 0 && i < length)
                return data[i * 5 + 1];
            return null;
        }

        public string GetQName(int i)
        {
            if (i >= 0 && i < length)
                return data[i * 5 + 2];
            return null;
        }

        public string GetType(int i)
        {
            if (i >= 0 && i < length)
                return data[i * 5 + 3];
            return null;
        }

        public string GetValue(int i)
        {
            if (i >= 0 && i < length)
                return data[i * 5 + 4];
            return null;
        }

        public int GetIndex(string str, string arg)
        {
            int i = length * 5;
            for (int i0 = 0; i0 < i; i0 += 5)
            {
                if (data[i0].Equals(str) && data[i0 + 1].Equals(arg))
                    return i0 / 5;
            }
            return -1;
        }

        public int GetIndex(string str)
        {
            int i = length * 5;
            for (int i0 = 0; i0 < i; i0 += 5)
            {
                if (data[i0 + 2].Equals(str))
                    return i0 / 5;
            }
            return -1;
        }

        public string GetType(string str, string arg)
        {
            int i = length * 5;
            for (int i0 = 0; i0 < i; i0 += 5)
            {
                if (data[i0].Equals(str) && data[i0 + 1].Equals(arg))
                    return data[i0 + 3];
            }
            return null;
        }

        public string GetType(string str)
        {
            int i = length * 5;
            for (int i0 = 0; i0 < i; i0 += 5)
            {
                if (data[i0 + 2].Equals(str))
                    return data[i0 + 3];
            }
            return null;
        }

        public string GetValue(string str, string arg)
        {
            int i = length * 5;
            for (int i0 = 0; i0 < i; i0 += 5)
            {
                if (data[i0].Equals(str) && data[i0 + 1].Equals(arg))
                    return data[i0 + 4];
            }
            return null;
        }

        public string GetValue(string str)
        {
            int i = length * 5;
            for (int i0 = 0; i0 < i; i0 += 5)
            {
                if (data[i0 + 2].Equals(str))
                    return data[i0 + 4];
            }
            return null;
        }

        public void Clear()
        {
            length = 0;
        }

        public void SetAttributes(IAttributes attributes)
        {
            this.Clear();
            length = attributes.GetLength();
            if (length > 0)
            {
                data = new string[length * 5];
                for (int i = 0; i < length; i++)
                {
                    data[i * 5] = attributes.GetURI(i);
                    data[i * 5 + 1] = attributes.GetLocalName(i);
                    data[i * 5 + 2] = attributes.GetQName(i);
                    data[i * 5 + 3] = attributes.GetType(i);
                    data[i * 5 + 4] = attributes.GetValue(i);
                }
            }
        }

        public void AddAttribute(string str, string arg, string str0, string str1,
                     string str2)
        {
            EnsureCapacity(length + 1);
            data[length * 5] = str;
            data[length * 5 + 1] = arg;
            data[length * 5 + 2] = str0;
            data[length * 5 + 3] = str1;
            data[length * 5 + 4] = str2;
            length++;
        }

        public void SetAttribute(int i, string str, string arg, string str0,
                     string str1, string str2)
        {
            if (i >= 0 && i < length)
            {
                data[i * 5] = str;
                data[i * 5 + 1] = arg;
                data[i * 5 + 2] = str0;
                data[i * 5 + 3] = str1;
                data[i * 5 + 4] = str2;
            }
            else
                BadIndex(i);
        }

        public void RemoveAttribute(int i)
        {
            if (i >= 0 && i < length)
            {
                data[i * 5] = null;
                data[i * 5 + 1] = null;
                data[i * 5 + 2] = null;
                data[i * 5 + 3] = null;
                data[i * 5 + 4] = null;
                if (i < length - 1)
                    System.Array.Copy(data, (i + 1) * 5, data, i * 5
                      , (length - i - 1) * 5);
                length--;
            }
            else
                BadIndex(i);
        }

        public void SetURI(int i, string str)
        {
            if (i >= 0 && i < length)
                data[i * 5] = str;
            else
                BadIndex(i);
        }

        public void SetLocalName(int i, string str)
        {
            if (i >= 0 && i < length)
                data[i * 5 + 1] = str;
            else
                BadIndex(i);
        }

        public void SetQName(int i, string str)
        {
            if (i >= 0 && i < length)
                data[i * 5 + 2] = str;
            else
                BadIndex(i);
        }

        public void SetType(int i, string str)
        {
            if (i >= 0 && i < length)
                data[i * 5 + 3] = str;
            else
                BadIndex(i);
        }

        public void SetValue(int i, string str)
        {
            if (i >= 0 && i < length)
                data[i * 5 + 4] = str;
            else
                BadIndex(i);
        }

        private void EnsureCapacity(int i)
        {
            if (i > 0)
            {
                int i0;
                if (data == null || data.Length == 0)
                    i0 = 25;
                else
                {
                    if (data.Length >= i * 5)
                        return;
                    i0 = data.Length;
                }
                for (; i0 < i * 5; i0 *= 2) ;
                string[] strings = new string[i0];
                if (length > 0)
                    System.Array.Copy(data, 0, strings, 0, length * 5);
                data = strings;
            }
        }

        private void BadIndex(int i)
        {
            string str = "Attempt to modify attribute at illegal index: " + i;
            throw new IndexOutOfRangeException(str.ToString());
        }

    }
}
