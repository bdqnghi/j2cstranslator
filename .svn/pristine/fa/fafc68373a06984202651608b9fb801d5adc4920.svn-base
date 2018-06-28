using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ILOG.J2CsMapping.Util
{
    public class LocaleHelper
    {

        public static Locale CultureInfoToLocale(CultureInfo info)
        {
            String ietfLanguageTag = info.IetfLanguageTag;
            String language = ParseIETFForLanguage(ietfLanguageTag);
            String country = ParseIETFForCountry(ietfLanguageTag);
            String variant = ParseIETFForVariant(ietfLanguageTag);
            return new Locale(language, country, variant);         
        }

        private static string ParseIETFForVariant(string p)
        {
            return "";
        }

        private static string ParseIETFForCountry(string p)
        {
            if (p.Length > 2)
            {
                if (p[2] == '-')
                {
                    return p.Substring(3);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private static string ParseIETFForLanguage(string p)
        {
            return p.Substring(0, 2);
        }

        public static CultureInfo LocaleToCultureInfo(Locale info)
        {
            String language = info.GetLanguage();
            String variant = info.GetVariant();
            String country = info.GetCountry();
            String ietfTag = BuildIETFTag(language, country, variant);
            return CultureInfo.GetCultureInfoByIetfLanguageTag(ietfTag);          
        }

        private static string BuildIETFTag(string language, string country, string variant)
        {
            String ietfTag = language;
            if (country != null && !country.Equals(""))
            {
                ietfTag += "-" + country;
            }
            return ietfTag;
        }


    }
}
