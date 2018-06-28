using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Text;
using ILOG.J2CsMapping.Collections.Generics;

namespace MappingTests {

    [TestFixture]
    public class MessageFormatTests {

        [Test]
        public void TestArguments1() {
            DoTest("0", "{0}", 0);
        }

        [Test]
        public void TestArguments2() {
            DoTest("1 0", "{1} {0}", 0, 1);
        }

        [Test]
        public void TestQuoteEscaping() {
            DoTest("'1 '0'", "''{1} ''{0}''", 0, 1);
        }

        [Test]
        public void TestBraceEscaping1() {
            DoTest("{1 {0{", "'{'{1} '{'{0}'{'", 0, 1);
        }

        [Test]
        public void TestBraceEscaping2() {
            DoTest("}1 }0}", "'}'{1} '}'{0}'}'", 0, 1);
        }

        private void DoTest(string expected, string pattern, params object[] args) {
            string txt = MessageFormat.Format(pattern, args);
            Assert.AreEqual(expected, txt, "The formatted string differs from what was expected.");
        }
    }
}