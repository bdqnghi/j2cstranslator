using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Util;

namespace Tests
{
    [TestFixture]
    public class StringTokenizerTests
    {

        [Test]
        public void test_ConstructorLjava_lang_String()
        {
            // Test for method java.util.StringTokenizer(java.lang.String)
            Assertion.Assert("Used in tests", true);
        }

        [Test]
        public void test_ConstructorLjava_lang_StringLjava_lang_String()
        {
            // Test for method java.util.StringTokenizer(java.lang.String,
            // java.lang.String)
            StringTokenizer st = new StringTokenizer("This:is:a:test:String", ":");
            Assertion.Assert("Created incorrect tokenizer", st.CountTokens() == 5
                    && (st.NextElement().Equals("This")));
        }
        [Test]
        public void test_ConstructorLjava_lang_StringLjava_lang_StringZ()
        {
            // Test for method java.util.StringTokenizer(java.lang.String,
            // java.lang.String, boolean)
            StringTokenizer st = new StringTokenizer("This:is:a:test:String", ":",
                    true);
            st.NextElement();
            Assertion.Assert("Created incorrect tokenizer", st.CountTokens() == 8
                    && (st.NextElement().Equals(":")));
        }

        [Test]
        public void test_countTokens()
        {
            // Test for method int java.util.StringTokenizer.countTokens()
            StringTokenizer st = new StringTokenizer("This is a test String");

            Assertion.AssertEquals("Incorrect token count returned", 5, st.CountTokens());
        }

        [Test]
        public void test_hasMoreElements()
        {
            // Test for method boolean java.util.StringTokenizer.hasMoreElements()

            StringTokenizer st = new StringTokenizer("This is a test String");
            st.NextElement();
            Assertion.Assert("hasMoreElements returned incorrect value", st
                    .HasMoreElements());
            st.NextElement();
            st.NextElement();
            st.NextElement();
            st.NextElement();
            Assertion.Assert("hasMoreElements returned incorrect value", !st
                    .HasMoreElements());
        }

        [Test]
        public void test_hasMoreTokens()
        {
            // Test for method boolean java.util.StringTokenizer.hasMoreTokens()
            StringTokenizer st = new StringTokenizer("This is a test String");
            for (int counter = 0; counter < 5; counter++)
            {
                Assertion.Assert(
                        "StringTokenizer incorrectly reports it has no more tokens",
                        st.HasMoreTokens());
                st.NextToken();
            }
            Assertion.Assert("StringTokenizer incorrectly reports it has more tokens",
                    !st.HasMoreTokens());
        }

        [Test]
        public void test_nextElement()
        {
            // Test for method java.lang.Object
            // java.util.StringTokenizer.nextElement()
            StringTokenizer st = new StringTokenizer("This is a test String");
            Assertion.AssertEquals("nextElement returned incorrect value", "This", ((String)st
                    .NextElement()));
            Assertion.AssertEquals("nextElement returned incorrect value", "is", ((String)st
                    .NextElement()));
            Assertion.AssertEquals("nextElement returned incorrect value", "a", ((String)st
                    .NextElement()));
            Assertion.AssertEquals("nextElement returned incorrect value", "test", ((String)st
                    .NextElement()));
            Assertion.AssertEquals("nextElement returned incorrect value", "String", ((String)st
                    .NextElement()));
            try
            {
                st.NextElement();
                Assertion.Fail(
                        "nextElement failed to throw a NoSuchElementException when it should have been out of elements");
            }
            catch (Exception e)
            {
                return;
            }
        }

        [Test]
        public void test_nextToken()
        {
            // Test for method java.lang.String
            // java.util.StringTokenizer.nextToken()
            StringTokenizer st = new StringTokenizer("This is a test String");
            Assertion.AssertEquals("nextToken returned incorrect value",
                    "This", st.NextToken());
            Assertion.AssertEquals("nextToken returned incorrect value",
                    "is", st.NextToken());
            Assertion.AssertEquals("nextToken returned incorrect value",
                    "a", st.NextToken());
            Assertion.AssertEquals("nextToken returned incorrect value",
                    "test", st.NextToken());
            Assertion.AssertEquals("nextToken returned incorrect value",
                    "String", st.NextToken());
            try
            {
                st.NextToken();
                Assertion.Fail(
                        "nextToken failed to throw a NoSuchElementException when it should have been out of elements");
            }
            catch (Exception e)
            {
                return;
            }
        }

        [Test]
        public void test_nextTokenLjava_lang_String()
        {
            // Test for method java.lang.String
            // java.util.StringTokenizer.nextToken(java.lang.String)
            StringTokenizer st = new StringTokenizer("This is a test String");
            Assertion.AssertEquals("nextToken(String) returned incorrect value with normal token String",
                    "This", st.NextToken(" "));
            Assertion.AssertEquals("nextToken(String) returned incorrect value with custom token String",
                    " is a ", st.NextToken("tr"));
            Assertion.AssertEquals("calling nextToken() did not use the new default delimiter list",
                    "es", st.NextToken());
        }

        [SetUp]
        protected void setUp()
        {
        }

        [TearDown]
        protected void tearDown()
        {
        }

    }
}
