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
    /// .NET replacement for pattern
    /// </summary>
    public class Pattern : Regex
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regexp"></param>
        public Pattern(String regexp)
            : base(regexp, RegexOptions.Singleline|RegexOptions.Compiled|RegexOptions.CultureInvariant)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regexp"></param>
        public Pattern(String regexp, RegexOptions options)
            : base(regexp, options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regexp"></param>
        /// <returns></returns>
        public static Pattern Compile(String regexp)
        {
            return new Pattern(regexp, RegexOptions.Singleline);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Matcher Matcher(String input)
        {
            return new Matcher((Regex)this, input);
        }
    }
}
