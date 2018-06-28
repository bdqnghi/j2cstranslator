/* 
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/1/10 3:36 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
namespace ILOG.J2CsMapping.Formatting
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Text;
    using ILOG.J2CsMapping.Text;

    /// <summary>
    /// Format is the abstract superclass of classes which format and parse objects
    /// according to Locale specific rules.
    /// </summary>
    ///
    [Serializable]
    public abstract class Format : ICloneable
    {

        private const long serialVersionUID = -299282585814624189L;

        /// <summary>
        /// Constructs a new instance of Format.
        /// </summary>
        ///
        public Format()
        {
        }

        /// <summary>
        /// Answers a copy of this Format.
        /// </summary>
        ///
        /// <returns>a shallow copy of this Format</returns>
        /// <seealso cref="T:System.ICloneable"/>
        public virtual Object Clone()
        {
            try
            {
                return base.MemberwiseClone();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        internal String ConvertPattern(String template, String fromChars, String toChars,
                bool check)
        {
            if (!check && fromChars.Equals(toChars))
            {
                return template;
            }
            bool quote = false;
            StringBuilder output = new StringBuilder();
            int length = template.Length;
            for (int i = 0; i < length; i++)
            {
                int index;
                char next = template[i];
                if (next == '\'')
                {
                    quote = !quote;
                }
                if (!quote && (index = fromChars.IndexOf(next)) != -1)
                {
                    output.Append(toChars[index]);
                }
                else if (check
                        && !quote
                        && ((next >= 'a' && next <= 'z') || (next >= 'A' && next <= 'Z')))
                {
                    // text.05=Invalid pattern char {0} in {1}
                    throw new ArgumentException(
                            "text.05" + next.ToString() + template); //$NON-NLS-1$
                }
                else
                {
                    output.Append(next);
                }
            }
            if (quote)
            {
                // text.04=Unterminated quote
                throw new ArgumentException("text.04"); //$NON-NLS-1$
            }
            return output.ToString();
        }

        /// <summary>
        /// Formats the specified object using the rules of this Format.
        /// </summary>
        ///
        /// <param name="object">the object to format</param>
        /// <returns>the formatted String</returns>
        /// <exception cref="IllegalArgumentException">when the object cannot be formatted by this Format</exception>
        public String FormatObject(Object obj0)
        {
            return FormatObject(obj0, new StringBuilder(), new FieldPosition(0))
                    .ToString();
        }

        /// <summary>
        /// Formats the specified object into the specified StringBuffer using the
        /// rules of this Format. If the field specified by the FieldPosition is
        /// formatted, set the begin and end index of the formatted field in the
        /// FieldPosition.
        /// </summary>
        ///
        /// <param name="object">the object to format</param>
        /// <param name="buffer">the StringBuffer</param>
        /// <param name="field">the FieldPosition</param>
        /// <returns>the StringBuffer parameter <c>buffer</c></returns>
        /// <exception cref="IllegalArgumentException">when the object cannot be formatted by this Format</exception>
        public abstract StringBuilder FormatObject(Object obj0, StringBuilder buffer,
                FieldPosition field);

        /// <summary>
        /// Formats the specified object using the rules of this format and returns
        /// an AttributedCharacterIterator with the formatted String and no
        /// attributes.
        /// <p>
        /// Subclasses should return an AttributedCharacterIterator with the
        /// appropriate attributes.
        /// </summary>
        ///
        /// <param name="object">the object to format</param>
        /// <returns>an AttributedCharacterIterator with the formatted object and
        /// attributes</returns>
        /// <exception cref="IllegalArgumentException">when the object cannot be formatted by this Format</exception>
        public virtual ILOG.J2CsMapping.Text.AttributedCharacterIterator FormatToCharacterIterator(Object obj0)
        {
            return new ILOG.J2CsMapping.Text.AttributedString(FormatObject(obj0)).GetIterator();
        }

        /// <summary>
        /// Parse the specified String using the rules of this Format.
        /// </summary>
        ///
        /// <param name="string">the String to parse</param>
        /// <returns>the object resulting from the parse</returns>
        /// <exception cref="ParseException">when an error occurs during parsing</exception>
        public Object ParseObject(String str0)
        {
            ParsePosition position = new ParsePosition(0);
            Object result = ParseObject(str0, position);
            if (position.GetErrorIndex() != -1 || position.GetIndex() == 0)
            {
                throw new ILOG.J2CsMapping.Util.ParseException(null, position.GetErrorIndex());
            }
            return result;
        }

        /// <summary>
        /// Parse the specified String starting at the index specified by the
        /// ParsePosition. If the string is successfully parsed, the index of the
        /// ParsePosition is updated to the index following the parsed text.
        /// </summary>
        ///
        /// <param name="string">the String to parse</param>
        /// <param name="position">the ParsePosition, updated on return with the index followingthe parsed text, or on error the index is unchanged and theerror index is set to the index where the error occurred</param>
        /// <returns>the object resulting from the parse, or null if there is an error</returns>
        public abstract Object ParseObject(String str0, ParsePosition position);

        /*
         * Gets private field value by reflection.
         * 
         * @param fieldName the field name to be set @param target the object which
         * field to be gotten
         */
        static internal Object GetInternalField(String fieldName, Object target)
        {
            Object value_ren = new Format.Anonymous_C0(fieldName, target).Run();
            return value_ren;
        }

        static internal bool UpTo(String str0, ParsePosition position,
                StringBuilder buffer, char stop)
        {
            int index = position.GetIndex(), length = str0.Length;
            bool lastQuote = false, quote = false;
            while (index < length)
            {
                char ch = str0[index++];
                if (ch == '\'')
                {
                    if (lastQuote)
                    {
                        buffer.Append('\'');
                    }
                    quote = !quote;
                    lastQuote = true;
                }
                else if (ch == stop && !quote)
                {
                    position.SetIndex(index);
                    return true;
                }
                else
                {
                    lastQuote = false;
                    buffer.Append(ch);
                }
            }
            position.SetIndex(index);
            return false;
        }

        static internal bool UpToWithQuotes(String str0, ParsePosition position,
                StringBuilder buffer, char stop, char start)
        {
            int index = position.GetIndex(), length = str0.Length, count = 1;
            bool quote = false;
            while (index < length)
            {
                char ch = str0[index++];
                if (ch == '\'')
                {
                    quote = !quote;
                }
                if (!quote)
                {
                    if (ch == stop)
                    {
                        count--;
                    }
                    if (count == 0)
                    {
                        position.SetIndex(index);
                        return true;
                    }
                    if (ch == start)
                    {
                        count++;
                    }
                }
                buffer.Append(ch);
            }
            // text.07=Unmatched braces in the pattern
            throw new ArgumentException("text.07"); //$NON-NLS-1$
        }

        public sealed class Anonymous_C0 : Object
        {
            private readonly String fieldName;
            private readonly Object target;

            public Anonymous_C0(String fieldName_0, Object target_1)
            {
                this.fieldName = fieldName_0;
                this.target = target_1;
            }

            public Object Run()
            {
                Object result = null;
                FieldInfo field = null;
                try
                {
                    field = target.GetType().GetField(
                            fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    ILOG.J2CsMapping.Reflect.AccessibleObject.SetAccessible(field, true);
                    result = field.GetValue(target);
                }
                catch (Exception e1)
                {
                    return null;
                }
                return result;
            }
        }

        /// <summary>
        /// This inner class is used to represent Format attributes in the
        /// AttributedCharacterIterator that formatToCharacterIterator() method
        /// returns in the Format subclasses.
        /// </summary>
        ///
        public class Field : ILOG.J2CsMapping.Text.AttributedCharacterIterator_Constants.Attribute
        {

            private const long serialVersionUID = 276966692217360283L;

            /// <summary>
            /// Constructs a new instance of Field with the given fieldName.
            /// </summary>
            ///
            protected internal Field(String fieldName_0)
                : base(fieldName_0)
            {
            }
        }
    }
}
