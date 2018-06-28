using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Util;

namespace MappingTests
{
    [TestFixture]
    public class BooleanUtilTests
    {
        //
        // ValueOf
        //

        [Test]
        public void TestValueOftrue()
        {
            String pattern = "true";
            bool? res = BooleanUtil.ValueOf(pattern);
            Assert.AreEqual(res, true);
        }

        [Test]
        public void TestValueOffalse()
        {
            String pattern = "false";
            bool? res = BooleanUtil.ValueOf(pattern);
            Assert.AreEqual(res, false);
        }

        [Test]
        public void TestValueOfTRUE()
        {
            String pattern = "TRUE";
            bool? res = BooleanUtil.ValueOf(pattern);
            Assert.AreEqual(res, true);
        }

        [Test]
        public void TestValueOfFALSE()
        {
            String pattern = "FALSE";
            bool? res = BooleanUtil.ValueOf(pattern);
            Assert.AreEqual(res, false);
        }

        [Test]
        public void TestValueOfTrue()
        {
            String pattern = "True";
            bool? res = BooleanUtil.ValueOf(pattern);
            Assert.AreEqual(res, true);
        }

        [Test]
        public void TestValueOfFalse()
        {
            String pattern = "False";
            bool? res = BooleanUtil.ValueOf(pattern);
            Assert.AreEqual(res, false);
        }

        [Test]
        public void TestValueOfEmpty()
        {
            try
            {
                String pattern = "";
                bool? res = BooleanUtil.ValueOf(pattern);
                Assert.AreEqual(res, false);
            }
            catch (FormatException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestValueOftNull()
        {
            try
            {
                String pattern = null;
                bool? res = BooleanUtil.ValueOf(pattern);
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
