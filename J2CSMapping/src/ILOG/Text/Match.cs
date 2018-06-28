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
using System.Text.RegularExpressions;

namespace ILOG.J2CsMapping.Text
{
    /// <summary>
    /// .NET replacement for Matcher
    /// </summary>
    public class Matcher
    {
        private Match imatch;
        private Regex iregex;
        private String iinput;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regexp"></param>
        /// <param name="input"></param>
        public Matcher(Regex regexp, String input)
        {
            iregex = regexp;
            // If commented some tests failed !
            imatch = regexp.Match(input);
            iinput = input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public String ReplaceFirst(String replacement)
        {
            return iregex.Replace(iinput, replacement, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public String ReplaceAll(String replacement)
        {
            return iregex.Replace(iinput, replacement, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static String QuoteReplacement( String replacement )
        {
            return replacement.Replace("$", "$$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public String Group(int i)
        {
            return imatch.Groups[i].Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Find()
        {
            return imatch.Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Find(int start)
        {
            imatch = iregex.Match(iinput, start);
            return imatch.Success;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int End()
        {
            return imatch.Length;
        }
    }
}
