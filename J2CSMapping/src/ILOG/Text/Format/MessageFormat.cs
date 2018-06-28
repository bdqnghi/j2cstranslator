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
using System.Text;

namespace ILOG.J2CsMapping.Text {

    /// <summary>
    /// Utility method for message formating
    /// </summary>
    public class MessageFormat {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Format(string pattern, params object[] args) {
            if (args == null) {
                args = new object[0];
            }
            // Translate the escaping of special characters to the .NET format:
            // '' -> '
            // '{' -> {{
            // '}' -> }}
            if (pattern.IndexOf("''") != -1 || pattern.IndexOf("'{'") != -1 || pattern.IndexOf("'}'") != -1) {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                while (i < pattern.Length) {
                    char c = pattern[i];
                    if (c == '\'') {
                        c = pattern[++i];
                        switch (c) {
                            case '\'':
                                sb.Append(c);
                                i++;
                                break;
                            case '{':
                                c = pattern[++i];
                                if (c == '\'') {
                                    sb.Append("{{");
                                    i++;
                                } else {
                                    sb.Append("'{");
                                }
                                break;
                            case '}':
                                c = pattern[++i];
                                if (c == '\'') {
                                    sb.Append("}}");
                                    i++;
                                } else {
                                    sb.Append("'}");
                                }
                                break;
                        }
                    } else {
                        sb.Append(c);
                        i++;
                    }
                }
                pattern = sb.ToString();
            }
            return string.Format(pattern, args);
        }
    }
}
