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
using System.Text.RegularExpressions;
using ILOG.J2CsMapping.Text;

namespace ILOG.J2CsMapping.Util
{

    /// <summary>
    /// Utility class for the mapping of System.String with java.lang.String.
    /// </summary>
    public class StringUtil
    {
        public static Comparison<String> CASE_INSENSITIVE_ORDER;

        private static int CaseInsensitiveComparator(String s1, String s2)
            {
                int n1 = s1.Length, n2 = s2.Length;
                for (int i1 = 0, i2 = 0; i1 < n1 && i2 < n2; i1++, i2++)
                {
                    char c1 = s1[i1];
                    char c2 = s2[i2];
                    if (c1 != c2)
                    {
                        c1 = Character.ToUpperCase(c1);
                        c2 = Character.ToUpperCase(c2);
                        if (c1 != c2)
                        {
                            c1 = Character.ToLowerCase(c1);
                            c2 = Character.ToLowerCase(c2);
                            if (c1 != c2)
                            {
                                return c1 - c2;
                            }
                        }
                    }
                }
                return n1 - n2;
        }
        

        //
        // new string
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static string NewString(byte[] p, string p_2)
        {
            // TODO: Decode using p encoding...
            String result = "";
            for (int i = 0; i < p.Length; i++)
                result += (char)p[i];

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string NewString(byte[] p)
        {
            String result = "";
            for (int i = 0; i < p.Length; i++)
                result += (char)p[i];

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string NewString(char[] p)
        {
            return new String(p);
        }

        //
        // Matches
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexp"></param>
        /// <returns></returns>
        public static bool Matches(string input, string regex)
        {
            //return ILOG.J2CsMapping.RegEx.Pattern.Matches(regex, input);
            
            Regex regex_c = new Regex(regex);

            return regex_c.Match(input).Success;
        }

        //
        // GetBytes
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stingMessage"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string stingMessage, string p)
        {
            // TODO: Encode using p encoding...
            byte[] res = new byte[stingMessage.Length];
            for (int i = 0; i < stingMessage.Length; i++)
            {
                res[i] = (byte)stingMessage[i];
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stingMessage"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string stingMessage)
        {
            byte[] res = new byte[stingMessage.Length];
            for (int i = 0; i < stingMessage.Length; i++)
            {
                res[i] = (byte)stingMessage[i];
            }
            return res;
        }

        //
        // GetChars
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stingMessage"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static char[] GetChars(string stingMessage, string p)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stingMessage"></param>
        /// <returns></returns>
        public static char[] GetChars(string stingMessage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //
        // CopyValueOf
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static String CopyValueOf(char[] a)
        {
            return new String(a);
        }

        //
        // Capitalize
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Capitalize(string name)
        {
            if (name.Length == 0)
                return name;
            String first = name.Substring(0, 1).ToUpper();
            String rest = name.Substring(1);
            return first + rest;
        }

        //
        // Substring
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static String Substring(String str, int start, int end)
        {
            return str.Substring(start, end - start);
        }

        //
        // ReplaceFirst
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qualifiedName_0"></param>
        /// <param name="p"></param>
        /// <param name="p_3"></param>
        /// <returns></returns>
        public static string ReplaceFirst(string qualifiedName_0, string p, string p_3)
        {
            int startIndex = qualifiedName_0.IndexOf(p);
            if (startIndex >= 0)
            {
                int endIndex = startIndex + p.Length;
                return qualifiedName_0.Substring(0, startIndex) + p_3 + qualifiedName_0.Substring(endIndex);
            }
            return qualifiedName_0;
        }

        //
        // IndexOf
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static int IndexOf(string s1, string s2, int idx)
        {
            if (idx == 0)
                return s1.IndexOf(s2, idx);
            if (idx == -1)
            {
                idx = 0;
            }
            else if (idx >= s1.Length)
            {
                idx = s1.Length - 1;
            }
            return s1.IndexOf(s2, idx);
        }

        //
        // StartsWith
        //

        /// <summary>
        /// Compares the specified string to this String, starting at the specified
        /// offset, to determine if the specified string is a prefix.
        /// </summary>
        /// <param name="value">The string to search for</param>
        /// <param name="prefix">the string to look for</param>
        /// <param name="toffset">the starting offset</param>
        /// <returns></returns>
        /// ToBeChecked
        public static bool StartsWith(String value, String prefix, int toffset)
        {
            return RegionMatches(value, toffset, prefix, 0, prefix.Length);
        }

        public static bool RegionMatches(String thisString, int thisStart, String str, int start, int length)
        {
            if (str == null)
            {
                throw new NullReferenceException();
            }
            if (start < 0 || str.Length - start < length)
            {
                return false;
            }
            if (thisStart < 0 || thisString.Length - thisStart < length)
            {
                return false;
            }
            if (length <= 0)
            {
                return true;
            }
            int o1 = thisStart, o2 = start;
            for (int i = 0; i < length; ++i)
            {
                if (thisString[o1 + i] != str[o2 + i])
                {
                    return false;
                }
            }
            return true;
        }

    /**
     * Compares the specified string to this String and compares the specified
     * range of characters to determine if they are the same. When ignoreCase is
     * true, the case of the characters is ignored during the comparison.
     * 
     * @param ignoreCase
     *            specifies if case should be ignored
     * @param thisStart
     *            the starting offset in this String
     * @param string
     *            the string to compare
     * @param start
     *            the starting offset in string
     * @param length
     *            the number of characters to compare
     * @return true if the ranges of characters is equal, false otherwise
     * 
     * @throws NullPointerException
     *             when string is null
     */
        public static bool RegionMatches(String thisString, bool ignoreCase, int thisStart,
                String str, int start, int length)
        {
            if (!ignoreCase)
            {
                return RegionMatches(thisString, thisStart, str, start, length);
            }

            if (str != null)
            {
                if (thisStart < 0 || length > thisString.Length - thisStart)
                {
                    return false;
                }
                if (start < 0 || length > str.Length - start)
                {
                    return false;
                }

                thisStart += 0; // thisString.offset;
                start += 0; // str.offset;
                int end = thisStart + length;
                char c1, c2;
                char[] target = str.ToCharArray();
                while (thisStart < end)
                {
                    if ((c1 = thisString.ToCharArray()[thisStart++]) != (c2 = target[start++])
                            && Character.ToUpperCase(c1) != Character
                                    .ToUpperCase(c2)
                        // Required for unicode that we test both cases
                            && Character.ToLowerCase(c1) != Character
                                    .ToLowerCase(c2))
                    {
                        return false;
                    }
                }
                return true;
            }
            throw new NullReferenceException();
        }

        //
        // Format
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static String Format(String format, params Object[] args)
        {
            String mappedFormat = MapFormat(format);
            return System.String.Format(mappedFormat, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string MapFormat(String format)
        {
            String res = format;
            int index = -1;
            int cpt = 0;
            while ((index = res.IndexOf("%s")) >= 0)
            {
                res = res.Substring(0, index) + "{" + cpt++ + "}" + res.Substring(index + 2);
            }
            return res;
        }
    }
}