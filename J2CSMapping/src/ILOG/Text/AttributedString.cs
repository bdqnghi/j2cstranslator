/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/1/10 3:36 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
namespace ILOG.J2CsMapping.Text
{

    using ILOG.J2CsMapping.Collections.Generics;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using ILOG.J2CsMapping.Collections;

    /// <summary>
    /// AttributedString
    /// </summary>
    ///
    public class AttributedString
    {

        internal String text;

        internal IDictionary<AttributedCharacterIterator_Constants.Attribute, IList<Range>> attributeMap;

        internal class Range
        {
            internal int start;

            internal int end;

            internal Object value_ren;

            internal Range(int s, int e, Object v)
            {
                start = s;
                end = e;
                value_ren = v;
            }
        }

        internal class AttributedIterator : AttributedCharacterIterator
        {

            private int begin, end, offset;

            private AttributedString attrString;

            private HashedSet<AttributedCharacterIterator_Constants.Attribute> attributesAllowed;

            internal AttributedIterator(AttributedString attrString_0)
            {
                this.attrString = attrString_0;
                begin = 0;
                end = attrString_0.text.Length;
                offset = 0;
            }

            internal AttributedIterator(AttributedString attrString_0,
                    AttributedCharacterIterator_Constants.Attribute[] attributes, int begin_1,
                    int end_2)
            {
                if (begin_1 < 0 || end_2 > attrString_0.text.Length || begin_1 > end_2)
                {
                    throw new ArgumentException();
                }
                this.begin = begin_1;
                this.end = end_2;
                offset = begin_1;
                this.attrString = attrString_0;
                if (attributes != null)
                {
                    HashedSet<AttributedCharacterIterator_Constants.Attribute> set = new HashedSet<AttributedCharacterIterator_Constants.Attribute>(
                            (attributes.Length * 4 / 3) + 1);
                    for (int i = attributes.Length; --i >= 0; )
                    {
                        ILOG.J2CsMapping.Collections.Generics.Collections.Add(set, attributes[i]);
                    }
                    attributesAllowed = set;
                }
            }

            /// <summary>
            /// Answers a new StringCharacterIterator with the same source String,
            /// begin, end, and current index as this StringCharacterIterator.
            /// </summary>
            ///
            /// <returns>a shallow copy of this StringCharacterIterator</returns>
            /// <seealso cref="T:System.ICloneable"/>
            public virtual Object Clone()
            {
                /*try {
                    AttributedString.AttributedIterator  clone = (AttributedString.AttributedIterator ) base.Clone();
                    if (attributesAllowed != null) {
                        clone.attributesAllowed = (HashedSet<AttributedCharacterIterator_Constants.Attribute>) attributesAllowed
                                .Clone();
                    }
                    return clone;
                } catch (Exception e) {
                    return null;
                }*/
                throw new NotImplementedException();
            }

            /// <summary>
            /// Answers the character at the current index in the source String.
            /// </summary>
            ///
            /// <returns>the current character, or DONE if the current index is past
            /// the end</returns>
            public virtual char Current()
            {
                if (offset == end)
                {
                    return ILOG.J2CsMapping.Text.CharacterIterator.Done;
                }
                return attrString.text[offset];
            }

            /// <summary>
            /// Sets the current position to the begin index and answers the
            /// character at the begin index.
            /// </summary>
            ///
            /// <returns>the character at the begin index</returns>
            public virtual char First()
            {
                if (begin == end)
                {
                    return ILOG.J2CsMapping.Text.CharacterIterator.Done;
                }
                offset = begin;
                return attrString.text[offset];
            }

            /// <summary>
            /// Answers the begin index in the source String.
            /// </summary>
            ///
            /// <returns>the index of the first character to iterate</returns>
            public virtual int GetBeginIndex()
            {
                return begin;
            }

            /// <summary>
            /// Answers the end index in the source String.
            /// </summary>
            ///
            /// <returns>the index one past the last character to iterate</returns>
            public virtual int GetEndIndex()
            {
                return end;
            }

            /// <summary>
            /// Answers the current index in the source String.
            /// </summary>
            ///
            /// <returns>the current index</returns>
            public virtual int GetIndex()
            {
                return offset;
            }

            public bool InRange(AttributedString.Range range)
            {
                if (!(range.value_ren is ILOG.J2CsMapping.Util.Annotation))
                {
                    return true;
                }
                return range.start >= begin && range.start < end
                        && range.end > begin && range.end <= end;
            }

            public bool InRange(IList<Range> ranges)
            {
                IIterator<Range> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<ILOG.J2CsMapping.Text.AttributedString.Range>(ranges.GetEnumerator());
                while (it.HasNext())
                {
                    AttributedString.Range range = it.Next();
                    if (range.start >= begin && range.start < end)
                    {
                        return !(range.value_ren is ILOG.J2CsMapping.Util.Annotation)
                                || (range.end > begin && range.end <= end);
                    }
                    else if (range.end > begin && range.end <= end)
                    {
                        return !(range.value_ren is ILOG.J2CsMapping.Util.Annotation)
                                || (range.start >= begin && range.start < end);
                    }
                }
                return false;
            }

            public virtual ILOG.J2CsMapping.Collections.Generics.ISet<AttributedCharacterIterator_Constants.Attribute> GetAllAttributeKeys()
            {
                if (begin == 0 && end == attrString.text.Length
                        && attributesAllowed == null)
                {
                    return new ILOG.J2CsMapping.Collections.Generics.ListSet<ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute>(attrString.attributeMap.Keys);
                }

                ILOG.J2CsMapping.Collections.Generics.ISet<AttributedCharacterIterator_Constants.Attribute> result = new HashedSet<AttributedCharacterIterator_Constants.Attribute>(
                        (attrString.attributeMap.Count * 4 / 3) + 1);
                IIterator<KeyValuePair<AttributedCharacterIterator_Constants.Attribute, IList<Range>>> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<System.Collections.Generic.KeyValuePair<ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute, System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>>>(new ILOG.J2CsMapping.Collections.Generics.ListSet<KeyValuePair<ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute, System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>>>(attrString.attributeMap).GetEnumerator());
                while (it.HasNext())
                {
                    KeyValuePair<AttributedCharacterIterator_Constants.Attribute, IList<Range>> entry = it.Next();
                    if (attributesAllowed == null
                            || ILOG.J2CsMapping.Collections.Generics.Collections.Contains((ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)entry.Key, attributesAllowed))
                    {
                        IList<Range> ranges = entry.Value;
                        if (InRange(ranges))
                        {
                            ILOG.J2CsMapping.Collections.Generics.Collections.Add(result, entry.Key);
                        }
                    }
                }
                return result;
            }

            public Object CurrentValue(IList<Range> ranges)
            {
                IIterator<Range> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<ILOG.J2CsMapping.Text.AttributedString.Range>(ranges.GetEnumerator());
                while (it.HasNext())
                {
                    AttributedString.Range range = it.Next();
                    if (offset >= range.start && offset < range.end)
                    {
                        return (InRange(range)) ? range.value_ren : null;
                    }
                }
                return null;
            }

            public virtual Object GetAttribute(
                    AttributedCharacterIterator_Constants.Attribute attribute)
            {
                if (attributesAllowed != null
                        && !ILOG.J2CsMapping.Collections.Generics.Collections.Contains((ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)attribute, attributesAllowed))
                {
                    return null;
                }
                List<Range> ranges = (List<Range>)((System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>)ILOG.J2CsMapping.Collections.Generics.Collections.Get(attrString.attributeMap, attribute));
                if (ranges == null)
                {
                    return null;
                }
                return CurrentValue(ranges);
            }

            public virtual IDictionary<AttributedCharacterIterator_Constants.Attribute, Object> GetAttributes()
            {
                IDictionary<AttributedCharacterIterator_Constants.Attribute, Object> result = new Dictionary<AttributedCharacterIterator_Constants.Attribute, Object>(
                        (attrString.attributeMap.Count * 4 / 3) + 1);
                IIterator<KeyValuePair<AttributedCharacterIterator_Constants.Attribute, IList<Range>>> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<System.Collections.Generic.KeyValuePair<ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute, System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>>>(new ILOG.J2CsMapping.Collections.Generics.ListSet<KeyValuePair<ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute, System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>>>(attrString.attributeMap).GetEnumerator());
                while (it.HasNext())
                {
                    KeyValuePair<AttributedCharacterIterator_Constants.Attribute, IList<Range>> entry = it.Next();
                    if (attributesAllowed == null
                            || ILOG.J2CsMapping.Collections.Generics.Collections.Contains((ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)entry.Key, attributesAllowed))
                    {
                        Object value_ren = CurrentValue(entry.Value);
                        if (value_ren != null)
                        {
                            ILOG.J2CsMapping.Collections.Generics.Collections.Put(result, (ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)(entry.Key), (System.Object)(value_ren));
                        }
                    }
                }
                return result;
            }

            public virtual int GetRunLimit()
            {
                return GetRunLimit(GetAllAttributeKeys());
            }

            public int RunLimit(IList<Range> ranges)
            {
                int result = end;
                IListIterator<Range> it = new ILOG.J2CsMapping.Collections.Generics.ArrayListIterator<ILOG.J2CsMapping.Text.AttributedString.Range>(ranges, ranges.Count);
                while (it.HasPrevious())
                {
                    AttributedString.Range range = it.Previous();
                    if (range.end <= begin)
                    {
                        break;
                    }
                    if (offset >= range.start && offset < range.end)
                    {
                        return (InRange(range)) ? range.end : result;
                    }
                    else if (offset >= range.end)
                    {
                        break;
                    }
                    result = range.start;
                }
                return result;
            }

            public virtual int GetRunLimit(AttributedCharacterIterator_Constants.Attribute attribute)
            {
                if (attributesAllowed != null
                        && !ILOG.J2CsMapping.Collections.Generics.Collections.Contains((ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)attribute, attributesAllowed))
                {
                    return end;
                }
                List<Range> ranges = (List<Range>)((System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>)ILOG.J2CsMapping.Collections.Generics.Collections.Get(attrString.attributeMap, attribute));
                if (ranges == null)
                {
                    return end;
                }
                return RunLimit(ranges);
            }

            public virtual int GetRunLimit<T0>(ILOG.J2CsMapping.Collections.Generics.ISet<T0> attributes) where T0 : AttributedCharacterIterator_Constants.Attribute
            {
                int limit = end;
                IIterator<T0> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<T0>(attributes.GetEnumerator());
                while (it.HasNext())
                {
                    AttributedCharacterIterator_Constants.Attribute attribute = it.Next();
                    int newLimit = GetRunLimit(attribute);
                    if (newLimit < limit)
                    {
                        limit = newLimit;
                    }
                }
                return limit;
            }

            public virtual int GetRunStart()
            {
                return GetRunStart(GetAllAttributeKeys());
            }

            public int RunStart(IList<Range> ranges)
            {
                int result = begin;
                IIterator<Range> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<ILOG.J2CsMapping.Text.AttributedString.Range>(ranges.GetEnumerator());
                while (it.HasNext())
                {
                    AttributedString.Range range = it.Next();
                    if (range.start >= end)
                    {
                        break;
                    }
                    if (offset >= range.start && offset < range.end)
                    {
                        return (InRange(range)) ? range.start : result;
                    }
                    else if (offset < range.start)
                    {
                        break;
                    }
                    result = range.end;
                }
                return result;
            }

            public virtual int GetRunStart(AttributedCharacterIterator_Constants.Attribute attribute)
            {
                if (attributesAllowed != null
                        && !ILOG.J2CsMapping.Collections.Generics.Collections.Contains((ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)attribute, attributesAllowed))
                {
                    return begin;
                }
                List<Range> ranges = (List<Range>)((System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>)ILOG.J2CsMapping.Collections.Generics.Collections.Get(attrString.attributeMap, attribute));
                if (ranges == null)
                {
                    return begin;
                }
                return RunStart(ranges);
            }

            public virtual int GetRunStart<T0>(ILOG.J2CsMapping.Collections.Generics.ISet<T0> attributes) where T0 : AttributedCharacterIterator_Constants.Attribute
            {
                int start_0 = begin;
                IIterator<T0> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<T0>(attributes.GetEnumerator());
                while (it.HasNext())
                {
                    AttributedCharacterIterator_Constants.Attribute attribute = it.Next();
                    int newStart = GetRunStart(attribute);
                    if (newStart > start_0)
                    {
                        start_0 = newStart;
                    }
                }
                return start_0;
            }

            /// <summary>
            /// Sets the current position to the end index - 1 and answers the
            /// character at the current position.
            /// </summary>
            ///
            /// <returns>the character before the end index</returns>
            public virtual char Last()
            {
                if (begin == end)
                {
                    return ILOG.J2CsMapping.Text.CharacterIterator.Done;
                }
                offset = end - 1;
                return attrString.text[offset];
            }

            /// <summary>
            /// Increments the current index and returns the character at the new
            /// index.
            /// </summary>
            ///
            /// <returns>the character at the next index, or DONE if the next index is
            /// past the end</returns>
            public virtual char Next()
            {
                if (offset >= (end - 1))
                {
                    offset = end;
                    return ILOG.J2CsMapping.Text.CharacterIterator.Done;
                }
                return attrString.text[++offset];
            }

            /// <summary>
            /// Decrements the current index and returns the character at the new
            /// index.
            /// </summary>
            ///
            /// <returns>the character at the previous index, or DONE if the previous
            /// index is past the beginning</returns>
            public virtual char Previous()
            {
                if (offset == begin)
                {
                    return ILOG.J2CsMapping.Text.CharacterIterator.Done;
                }
                return attrString.text[--offset];
            }

            /// <summary>
            /// Sets the current index in the source String.
            /// </summary>
            ///
            /// <returns>the character at the new index, or DONE if the index is past
            /// the end</returns>
            /// <exception cref="IllegalArgumentException">when the new index is less than the begin index orgreater than the end index</exception>
            public virtual char SetIndex(int location)
            {
                if (location < begin || location > end)
                {
                    throw new ArgumentException();
                }
                offset = location;
                if (offset == end)
                {
                    return ILOG.J2CsMapping.Text.CharacterIterator.Done;
                }
                return attrString.text[offset];
            }
        }

        public AttributedString(AttributedCharacterIterator iterator)
        {
            if (iterator.GetBeginIndex() > iterator.GetEndIndex())
            {
                // text.0A=Invalid substring range
                throw new ArgumentException("text.0A"); //$NON-NLS-1$
            }
            StringBuilder buffer = new StringBuilder();
            for (int i = iterator.GetBeginIndex(); i < iterator.GetEndIndex(); i++)
            {
                buffer.Append(iterator.Current());
                iterator.Next();
            }
            text = buffer.ToString();
            ILOG.J2CsMapping.Collections.Generics.ISet<AttributedCharacterIterator_Constants.Attribute> attributes = iterator
                    .GetAllAttributeKeys();
            if (attributes == null)
            {
                return;
            }
            attributeMap = new Dictionary<AttributedCharacterIterator_Constants.Attribute, IList<Range>>(
                    (attributes.Count * 4 / 3) + 1);

            IIterator<AttributedCharacterIterator_Constants.Attribute> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute>(attributes.GetEnumerator());
            while (it.HasNext())
            {
                AttributedCharacterIterator_Constants.Attribute attribute = it.Next();
                iterator.SetIndex(0);
                while (iterator.Current() != ILOG.J2CsMapping.Text.CharacterIterator.Done)
                {
                    int start_0 = iterator.GetRunStart(attribute);
                    int limit = iterator.GetRunLimit(attribute);
                    Object value_ren = iterator.GetAttribute(attribute);
                    if (value_ren != null)
                    {
                        AddAttribute(attribute, value_ren, start_0, limit);
                    }
                    iterator.SetIndex(limit);
                }
            }
        }

        private AttributedString(AttributedCharacterIterator iterator, int start_0,
                int end_1, ILOG.J2CsMapping.Collections.Generics.ISet<AttributedCharacterIterator_Constants.Attribute> attributes)
        {
            if (start_0 < iterator.GetBeginIndex() || end_1 > iterator.GetEndIndex()
                    || start_0 > end_1)
            {
                throw new ArgumentException();
            }

            if (attributes == null)
            {
                return;
            }

            StringBuilder buffer = new StringBuilder();
            iterator.SetIndex(start_0);
            while (iterator.GetIndex() < end_1)
            {
                buffer.Append(iterator.Current());
                iterator.Next();
            }
            text = buffer.ToString();
            attributeMap = new Dictionary<AttributedCharacterIterator_Constants.Attribute, IList<Range>>(
                    (attributes.Count * 4 / 3) + 1);

            IIterator<AttributedCharacterIterator_Constants.Attribute> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute>(attributes.GetEnumerator());
            while (it.HasNext())
            {
                AttributedCharacterIterator_Constants.Attribute attribute = it.Next();
                iterator.SetIndex(start_0);
                while (iterator.GetIndex() < end_1)
                {
                    Object value_ren = iterator.GetAttribute(attribute);
                    int runStart = iterator.GetRunStart(attribute);
                    int limit = iterator.GetRunLimit(attribute);
                    if ((value_ren is ILOG.J2CsMapping.Util.Annotation && runStart >= start_0 && limit <= end_1)
                            || (value_ren != null && !(value_ren is ILOG.J2CsMapping.Util.Annotation)))
                    {
                        AddAttribute(attribute, value_ren, ((runStart < start_0) ? start_0
                                : runStart) - start_0, ((limit > end_1) ? end_1 : limit)
                                - start_0);
                    }
                    iterator.SetIndex(limit);
                }
            }
        }

        public AttributedString(AttributedCharacterIterator iterator, int start_0,
                int end_1)
            : this(iterator, start_0, end_1, iterator.GetAllAttributeKeys())
        {
        }

        public AttributedString(AttributedCharacterIterator iterator, int start_0,
                int end_1, AttributedCharacterIterator_Constants.Attribute[] attributes)
            : this(iterator, start_0, end_1, new HashedSet<AttributedCharacterIterator_Constants.Attribute>(ILOG.J2CsMapping.Collections.Generics.Arrays.AsList(attributes)))
        {
        }

        public AttributedString(String value_ren)
        {
            if (value_ren == null)
            {
                throw new NullReferenceException();
            }
            text = value_ren;
            attributeMap = new Dictionary<AttributedCharacterIterator_Constants.Attribute, IList<Range>>(11);
        }

        public AttributedString(String value_ren,
                IDictionary<object, Object> attributes)
        {
            if (value_ren == null)
            {
                throw new NullReferenceException();
            }
            if (value_ren.Length == 0 && !(attributes.Count == 0))
            {
                // text.0B=Cannot add attributes to empty string
                throw new ArgumentException("text.0B"); //$NON-NLS-1$
            }
            text = value_ren;
            attributeMap = new Dictionary<AttributedCharacterIterator_Constants.Attribute, IList<Range>>(
                    (attributes.Count * 4 / 3) + 1);

            foreach (KeyValuePair<Object, Object> entry in attributes)
            {
                List<Range> ranges = new List<Range>(1);
                ILOG.J2CsMapping.Collections.Generics.Collections.Add(ranges, new AttributedString.Range(0, text.Length, entry.Value));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(attributeMap, (ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)((AttributedCharacterIterator_Constants.Attribute)entry.Key), (System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>)(ranges));

            }
            /*IIterator it = new ILOG.J2CsMapping.Collections.IteratorAdapter(new ILOG.J2CsMapping.Collections.ListSet(attributes).GetEnumerator());
            while (it.HasNext()) {
                KeyValuePair<object, object> entry = (KeyValuePair<object, object>) it.Next();
                List<Range> ranges = new List<Range>(1);
                ILOG.J2CsMapping.Collections.Generics.Collections.Add(ranges,new AttributedString.Range (0, text.Length, entry.Value));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(attributeMap,(ILOG.J2CsMapping.Text2.AttributedCharacterIterator_Constants.Attribute)((AttributedCharacterIterator_Constants.Attribute) entry.Key),(System.Collections.Generic.IList<ILOG.J2CsMapping.Text2.AttributedString.Range>)(ranges));
            }*/
        }

        public void AddAttribute(AttributedCharacterIterator_Constants.Attribute attribute,
                Object value_ren)
        {
            if (null == attribute)
            {
                throw new NullReferenceException();
            }
            if (text.Length == 0)
            {
                throw new ArgumentException();
            }

            IList<Range> ranges = ((System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>)ILOG.J2CsMapping.Collections.Generics.Collections.Get(attributeMap, attribute));
            if (ranges == null)
            {
                ranges = new List<Range>(1);
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(attributeMap, (ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)(attribute), (System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>)(ranges));
            }
            else
            {
                ILOG.J2CsMapping.Collections.Generics.Collections.Clear(ranges);
            }
            ILOG.J2CsMapping.Collections.Generics.Collections.Add(ranges, new AttributedString.Range(0, text.Length, value_ren));
        }

        public void AddAttribute(AttributedCharacterIterator_Constants.Attribute attribute,
                Object value_ren, int start_0, int end_1)
        {
            if (null == attribute)
            {
                throw new NullReferenceException();
            }
            if (start_0 < 0 || end_1 > text.Length || start_0 >= end_1)
            {
                throw new ArgumentException();
            }

            if (value_ren == null)
            {
                return;
            }

            IList<Range> ranges = ((System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>)ILOG.J2CsMapping.Collections.Generics.Collections.Get(attributeMap, attribute));
            if (ranges == null)
            {
                ranges = new List<Range>(1);
                ILOG.J2CsMapping.Collections.Generics.Collections.Add(ranges, new AttributedString.Range(start_0, end_1, value_ren));
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(attributeMap, (ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute)(attribute), (System.Collections.Generic.IList<ILOG.J2CsMapping.Text.AttributedString.Range>)(ranges));
                return;
            }
            IListIterator<Range> it = new ArrayListIterator<Range>(ranges);
            while (it.HasNext())
            {
                // foreach(Range range in ranges) {
                AttributedString.Range range = it.Next();
                if (end_1 <= range.start)
                {
                    it.Previous();
                    break;
                }
                else if (start_0 < range.end
                        || (start_0 == range.end && value_ren.Equals(range.value_ren)))
                {
                    AttributedString.Range r1 = null, r3;
                    it.Remove();
                    r1 = new AttributedString.Range(range.start, start_0, range.value_ren);
                    r3 = new AttributedString.Range(end_1, range.end, range.value_ren);

                    while (end_1 > range.end && it.HasNext())
                    {
                        range = it.Next();
                        if (end_1 <= range.end)
                        {
                            if (end_1 > range.start
                                    || (end_1 == range.start && value_ren
                                            .Equals(range.value_ren)))
                            {
                                it.Remove();
                                r3 = new AttributedString.Range(end_1, range.end, range.value_ren);
                                break;
                            }
                        }
                        else
                        {
                            it.Remove();
                        }
                    }

                    if (value_ren.Equals(r1.value_ren))
                    {
                        if (value_ren.Equals(r3.value_ren))
                        {
                            it.Add(new AttributedString.Range((r1.start < start_0) ? r1.start : start_0,
                                    (r3.end > end_1) ? r3.end : end_1, r1.value_ren));
                        }
                        else
                        {
                            it.Add(new AttributedString.Range((r1.start < start_0) ? r1.start : start_0,
                                    end_1, r1.value_ren));
                            if (r3.start < r3.end)
                            {
                                it.Add(r3);
                            }
                        }
                    }
                    else
                    {
                        if (value_ren.Equals(r3.value_ren))
                        {
                            if (r1.start < r1.end)
                            {
                                it.Add(r1);
                            }
                            it.Add(new AttributedString.Range(start_0, (r3.end > end_1) ? r3.end : end_1,
                                    r3.value_ren));
                        }
                        else
                        {
                            if (r1.start < r1.end)
                            {
                                it.Add(r1);
                            }
                            it.Add(new AttributedString.Range(start_0, end_1, value_ren));
                            if (r3.start < r3.end)
                            {
                                it.Add(r3);
                            }
                        }
                    }
                    return;
                }
            }
            it.Add(new AttributedString.Range(start_0, end_1, value_ren));
        }

        public void AddAttributes<T0, T1>(
                IDictionary<T0, T1> attributes,
                int start_0, int end_1) where T0 : AttributedCharacterIterator_Constants.Attribute
        {
            foreach (KeyValuePair<T0, T1> entry in attributes)
            {
                AddAttribute(
                (AttributedCharacterIterator_Constants.Attribute)entry.Key,
                entry.Value, start_0, end_1);
            }
            /*IIterator<object> it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<System.Collections.Generic.KeyValuePair<object>>(new ILOG.J2CsMapping.Collections.Generics.ListSet<KeyValuePair<object>>(attributes).GetEnumerator());
            while (it.HasNext()) {
                KeyValuePair<object, object> entry = (KeyValuePair<object, object>) it.Next();
                AddAttribute(
                        (AttributedCharacterIterator_Constants.Attribute) entry.Key,
                        entry.Value, start_0, end_1);
            }*/
        }

        public AttributedCharacterIterator GetIterator()
        {
            return new AttributedString.AttributedIterator(this);
        }

        public AttributedCharacterIterator GetIterator(
                AttributedCharacterIterator_Constants.Attribute[] attributes)
        {
            return new AttributedString.AttributedIterator(this, attributes, 0, text.Length);
        }

        public AttributedCharacterIterator GetIterator(
                AttributedCharacterIterator_Constants.Attribute[] attributes, int start_0,
                int end_1)
        {
            return new AttributedString.AttributedIterator(this, attributes, start_0, end_1);
        }
    }
}
