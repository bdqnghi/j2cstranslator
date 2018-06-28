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
using System.Globalization;

namespace ILOG.J2CsMapping.Util.Culture
{
    /// <summary>
    /// 
    /// </summary>
    public class CultureInfoHelper
    {
        public static CultureInfo ENGLISH = CultureInfo.GetCultureInfo("en");
        public static CultureInfo FRENCH = CultureInfo.GetCultureInfo("fr");
        public static CultureInfo FRANCE = CultureInfo.GetCultureInfo("fr-FR");
        public static CultureInfo GERMAN = CultureInfo.GetCultureInfo("de");
        public static CultureInfo SPANISH = CultureInfo.GetCultureInfo("es");
        public static CultureInfo UK = CultureInfo.GetCultureInfo("en-GB");
        public static CultureInfo US = CultureInfo.GetCultureInfo("en-US");

        public static CultureInfo ITALIAN = CultureInfo.GetCultureInfo("it");
        public static CultureInfo JAPANESE = CultureInfo.GetCultureInfo("jp");
        public static CultureInfo KOREAN = CultureInfo.GetCultureInfo("kr");
        public static CultureInfo CHINESE = CultureInfo.GetCultureInfo("cn");
        public static CultureInfo GERMANY = CultureInfo.GetCultureInfo("de");
        public static CultureInfo ITALY = CultureInfo.GetCultureInfo("it");
        public static CultureInfo JAPAN = CultureInfo.GetCultureInfo("jp");
        public static CultureInfo KOREA = CultureInfo.GetCultureInfo("kr");
        public static CultureInfo CHINA = CultureInfo.GetCultureInfo("cn");
        public static CultureInfo TAIWAN = CultureInfo.GetCultureInfo("tw");
        public static CultureInfo CANADA = CultureInfo.GetCultureInfo("ca");
        public static CultureInfo CANADA_FRENCH = CultureInfo.GetCultureInfo("ca-FR");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static CultureInfo CreateCultureInfo(String language, String country)
        {
            return new CultureInfo(language + (country != null && country.Length > 0 ? "-" + country : ""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static string GetCountry(CultureInfo ci)
        {
            string name = ci.Name;
            int sep1 = name.IndexOf('-');
            if (sep1 == -1)
            {
                return "";
            }
            else
            {
                int sep2 = name.IndexOf('/', sep1);
                if (sep2 == -1)
                {
                    return name.Substring(sep1 + 1);
                }
                else
                {
                    return name.Substring(sep1 + 1, sep2 - sep1 - 1);
                }
            }
        }
    }
}
