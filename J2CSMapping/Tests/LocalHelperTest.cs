using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Globalization;
using ILOG.J2CsMapping.Util;

namespace Tests
{
    [TestFixture]
    public class LocalHelperTest
    {
        [Test]
        public void Test1()
        {
            CultureInfo info = CultureInfo.CurrentCulture;
            Locale l = LocaleHelper.CultureInfoToLocale(info);
            Assert.AreEqual(Locale.GetDefault(), l);
        }

        [Test]
        public void Test2()
        {
            Locale locale = Locale.ENGLISH;
            CultureInfo info = LocaleHelper.LocaleToCultureInfo(locale);
            Assert.AreEqual(CultureInfo.GetCultureInfoByIetfLanguageTag("en"), info);
        }
    }
}
