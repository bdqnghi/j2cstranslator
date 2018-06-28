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
using System.Collections;

namespace ILOG.J2CsMapping.Util
{
    /// <summary>
    /// .NET Replacement for java StringTokenizer
    /// 
    /// String tokenizer is used to break a string apart into tokens.
    /// 
    /// If returnDelimiters is false, successive calls to NextToken() return maximal
    /// blocks of characters that do not contain a delimiter.
    /// 
    /// If returnDelimiters is true, delimiters are considered to be tokens, and
    /// successive calls to NextToken() return either a one character delimiter, or a
    /// maximal block of text between delimiters.
    /// </summary>
    /// ToBeChecked
    public class StringTokenizer
    {
        private String str;

        private String delimiters;

        private bool returnDelimiters;

        private int position;

        /// <summary>
        /// Constructs a new StringTokenizer for string using whitespace as the
        /// delimiter, returnDelimiters is false.
        /// </summary>
        /// <param name="str">the string to be tokenized</param>
        public StringTokenizer(String str)
            : this(str, " \t\n\r\f", false)
        {
        }

        /// <summary>
        /// Constructs a new StringTokenizer for string using the specified
        /// delimiters, returnDelimiters is false.
        /// </summary>
        /// <param name="str">the string to be tokenized</param>
        /// <param name="delimiters">the delimiters to use</param>
        public StringTokenizer(String str, String delimiters)
            :
                this(str, delimiters, false)
        {
        }

        /// <summary>
        /// Constructs a new StringTokenizer for string using the specified
        /// delimiters and returning delimiters as tokens when specified.
        /// </summary>
        /// <param name="str">the string to be tokenized</param>
        /// <param name="delimiters">the delimiters to use</param>
        /// <param name="returnDelimiters">true to return each delimiter as a token</param>
        public StringTokenizer(String str, String delimiters,
                bool returnDelimiters)
        {
            if (str != null)
            {
                this.str = str;
                this.delimiters = delimiters;
                this.returnDelimiters = returnDelimiters;
                this.position = 0;
            }
            else
                throw new NullReferenceException();
        }

        /// <summary>
        /// Returns the number of unprocessed tokens remaining in the string.
        /// </summary>
        /// <returns>number of tokens that can be retreived before an exception will result</returns>
        public int CountTokens()
        {
            int count = 0;
            bool inToken = false;
            for (int i = position, length = str.Length; i < length; i++)
            {
                if (delimiters.IndexOf(str[i], 0) >= 0)
                {
                    if (returnDelimiters)
                        count++;
                    if (inToken)
                    {
                        count++;
                        inToken = false;
                    }
                }
                else
                {
                    inToken = true;
                }
            }
            if (inToken)
                count++;
            return count;
        }

        /// <summary>
        /// Returns true if unprocessed tokens remain.
        /// </summary>
        /// <returns>true if unprocessed tokens remain</returns>
        public bool HasMoreElements()
        {
            return HasMoreTokens();
        }

        /// <summary>
        /// Returns true if unprocessed tokens remain.
        /// </summary>
        /// <returns>true if unprocessed tokens remain</returns>
        public bool HasMoreTokens()
        {
            int length = str.Length;
            if (position < length)
            {
                if (returnDelimiters)
                    return true; // there is at least one character and even if
                // it is a delimiter it is a token

                // otherwise find a character which is not a delimiter
                for (int i = position; i < length; i++)
                    if (delimiters.IndexOf(str[i], 0) == -1)
                        return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the next token in the string as an Object.
        /// </summary>
        /// <returns>next token in the string as an Object</returns>
        public Object NextElement()
        {
            return NextToken();
        }

        /// <summary>
        /// Returns the next token in the string as a String.
        /// </summary>
        /// <returns>next token in the string as a String</returns>
        public String NextToken()
        {
            int i = position;
            int length = str.Length;

            if (i < length)
            {
                if (returnDelimiters)
                {
                    if (delimiters.IndexOf(str[position], 0) >= 0)
                        return "" + (str[position++]);
                    for (position++; position < length; position++)
                        if (delimiters.IndexOf(str[position], 0) >= 0)
                            return str.Substring(i, position - i);
                    return str.Substring(i);
                }

                while (i < length && delimiters.IndexOf(str[i], 0) >= 0)
                    i++;
                position = i;
                if (i < length)
                {
                    for (position++; position < length; position++)
                        if (delimiters.IndexOf(str[position], 0) >= 0)
                            return str.Substring(i, position -i);
                    return str.Substring(i);
                }
            }
            throw new Exception();
        }

        /// <summary>
        /// Returns the next token in the string as a String. The delimiters used are
        /// changed to the specified delimiters.
        /// </summary>
        /// <param name="delims">the new delimiters to use</param>
        /// <returns>next token in the string as a String</returns>
        public String NextToken(String delims)
        {
            this.delimiters = delims;
            return NextToken();
        }
    }
}