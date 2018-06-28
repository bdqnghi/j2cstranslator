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

namespace ILOG.J2CsMapping.Text
{
    /// <summary>
    /// .NET replacement for StringCharacterIterator
    /// </summary>
    public class StringCharacterIterator : CharacterIterator, ICharacterIterator
    {
        private string text;
        private int begin;
        private int end;
        // invariant: begin <= pos <= end
        private int pos;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public StringCharacterIterator(string text)
            : this(text, 0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        public StringCharacterIterator(string text, int pos)
            : this(text, 0, text.Length, pos)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="pos"></param>
        public StringCharacterIterator(string text, int begin, int end, int pos)
        {
            if (text == null)
                throw new NullReferenceException();
            this.text = text;

            if (begin < 0 || begin > end || end > text.Length)
                throw new ArgumentException("Invalid substring range");

            if (pos < begin || pos > end)
                throw new ArgumentException("Invalid position");

            this.begin = begin;
            this.end = end;
            this.pos = pos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void SetText(String text)
        {
            if (text == null)
                throw new NullReferenceException();
            this.text = text;
            this.begin = 0;
            this.end = text.Length;
            this.pos = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public char First()
        {
            pos = begin;
            return Current();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public char Last()
        {
            if (end != begin)
            {
                pos = end - 1;
            }
            else
            {
                pos = end;
            }
            return Current();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public char SetIndex(int p)
        {
            if (p < begin || p > end)
                throw new ArgumentException("Invalid index");
            pos = p;
            return Current();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public char Current()
        {
            if (pos >= begin && pos < end)
            {
                return text[pos];
            }
            else
            {
                return Done;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public char Next()
        {
            if (pos < end - 1)
            {
                pos++;
                return text[pos];
            }
            else
            {
                pos = end;
                return Done;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public char Previous()
        {
            if (pos > begin)
            {
                pos--;
                return text[pos];
            }
            else
            {
                return Done;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetBeginIndex()
        {
            return begin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetEndIndex()
        {
            return end;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetIndex()
        {
            return pos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(obj is StringCharacterIterator))
                return false;

            StringCharacterIterator that = (StringCharacterIterator)obj;

            if (GetHashCode() != that.GetHashCode())
                return false;
            if (!text.Equals(that.text))
                return false;
            if (pos != that.pos || begin != that.begin || end != that.end)
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return text.GetHashCode() ^ pos ^ begin ^ end;
        }
    }
}
