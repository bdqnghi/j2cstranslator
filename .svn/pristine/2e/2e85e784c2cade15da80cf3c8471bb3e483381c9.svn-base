using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Util;

namespace MappingTests
{
    [TestFixture]
    public class StingUtilTests
    {
        //
        // ReplaceFirst
        //

        [Test]
        public void TestReplaceFirstBehavior()
        {
            String pattern = "aaabbshhhaaa";
            String res = StringUtil.ReplaceFirst(pattern, "aaa", "XXX");
            Assert.AreEqual(res, "XXXbbshhhaaa");
        }

        [Test]
        public void TestReplaceFirstEmpty()
        {
            String pattern = "";
            String res = StringUtil.ReplaceFirst(pattern, "", "");
            Assert.AreEqual(res, pattern);
        }

        [Test]
        public void TestReplaceFirstBehavior2()
        {
            String pattern = "aaabbshhhaaa";
            String res = StringUtil.ReplaceFirst(pattern, "ZZZ", "XXX");
            Assert.AreEqual(res, pattern);
        }

        [Test]
        public void TestReplaceFirstBehavior3()
        {
            String pattern = "aaabbshhhaaa";
            String res = StringUtil.ReplaceFirst(pattern, "aa", "XXX");
            Assert.AreEqual(res, "XXXabbshhhaaa");
        }

        [Test]
        public void TestReplaceFirstNullBehavior()
        {
            try
            {
                String pattern = null;
                String res = StringUtil.ReplaceFirst(pattern, "aaa", "XXX");
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        //
        // Capitalize
        //

        [Test]
        public void TestCapitalizeBehavior()
        {
            String pattern = "aaabbshhhaaa";
            String res = StringUtil.Capitalize(pattern);
            Assert.AreEqual(res, "Aaabbshhhaaa");
        }

        [Test]
        public void TestCapitalizeBehaviorEmpty()
        {
            String pattern = "";
            String res = StringUtil.Capitalize(pattern);
            Assert.AreEqual(res, "");
        }

        [Test]
        public void TestCapitalizeNullBehavior()
        {
            try
            {
                String pattern = null;
                String res = StringUtil.Capitalize(pattern);
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        //
        // CopyValueOf
        //

        [Test]
        public void TestCopyValueOfBehavior()
        {
            char[] pattern = new char[] { 'a', 'b', 'c' };
            String res = StringUtil.CopyValueOf(pattern);
            Assert.AreEqual(res, new String(pattern));
        }

        [Test]
        public void TestCopyValueOfBehaviorEmpty()
        {
            char[] pattern = new char[] {};
            String res = StringUtil.CopyValueOf(pattern);
            Assert.AreEqual(res, new String(pattern));
        }

        [Test]
        public void TestCopyValueOfNullBehavior()
        {
            try
            {
                char[] pattern = null;
                String res = StringUtil.CopyValueOf(pattern);
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        //
        // NewString
        //

        [Test]
        public void TestNewStringBehavior()
        {
            char[] pattern = new char[] { 'a', 'b', 'c' };
            String res = StringUtil.NewString(pattern);
            Assert.AreEqual(res, new String(pattern));
        }

        [Test]
        public void TestNewStringBehaviorEmpty()
        {
            char[] pattern = new char[] { };
            String res = StringUtil.NewString(pattern);
            Assert.AreEqual(res, new String(pattern));
        }

        [Test]
        public void TestNewStringNullBehavior()
        {
            try
            {
                char[] pattern = null;
                String res = StringUtil.NewString(pattern);
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        //
        // StartsWith(String, String , int)
        //


        [Test]
        public void TestStartsWithBehaviorTrue()
        {
            String str = "+Toto";
            String pattern = "Toto";
            int index = 1;
            bool res = StringUtil.StartsWith(str, pattern, index);
            Assert.AreEqual(res, true);
        }

        [Test]
        public void TestStartsWithBehaviorFalse()
        {
            String str = "+Toto";
            String pattern = "Tuto";
            int index = 1;
            bool res = StringUtil.StartsWith(str, pattern, index);
            Assert.AreEqual(res, false);
        }

        [Test]
        public void TestStartsWithBehavior()
        {
            String str = "Toto";
            String pattern = "Toto";
            int index = 0;
            bool res = StringUtil.StartsWith(str, pattern, index);
            Assert.AreEqual(res, true);
        }

        [Test]
        public void TestStartsWithOutOfBoundBehavior()
        {
            String str = "Toto";
            String pattern = "to";
            int index = 10;
            bool res = StringUtil.StartsWith(str, pattern, index);
            Assert.AreEqual(res, false);
        }

        [Test]
        public void TestStartsWithOutOfBoundBehavior2()
        {
            String str = "Toto";
            String pattern = "to";
            int index = -1;
            bool res = StringUtil.StartsWith(str, pattern, index);
            Assert.AreEqual(res, false);
        }

        [Test]
        public void TestStartsWithEmpty()
        {
            String str = "";
            String pattern = "";
            int index = 1;
            bool res = StringUtil.StartsWith(str, pattern, index);
            Assert.AreEqual(res, false);
        }
    }
}
