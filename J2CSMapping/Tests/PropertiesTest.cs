using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILOG.J2CsMapping.Util;
using NUnit.Framework;
using System.IO;

namespace Tests
{

    [TestFixture]
    public class PropertiesTest
    {

        Properties tProps;

        byte[] propsFile;

        [Test]
        public void test_Constructor()
        {
            Properties p = new Properties();
            // do something to avoid getting a variable unused warning
            p.Clear();
        }     

        [Test]
        public void test_getPropertyLjava_lang_String()
        {
            Assertion.AssertEquals("Did not retrieve property", "this is a test property",
                    tProps.GetProperty("test.prop"));
        }

        [Test]
        public void test_getPropertyLjava_lang_StringLjava_lang_String()
        {
            Assertion.AssertEquals("Did not retrieve property", "this is a test property",
                    tProps.GetProperty("test.prop", "Blarg"));
            Assertion.AssertEquals("Did not return default value", "Gabba", tProps
                    .GetProperty("notInThere.prop", "Gabba"));
        }

        [Test]
        public void test_listLjava_io_PrintStream()
        {
            TextWriter ps = new StringWriter();
            Properties myProps = new Properties();
            String propList;
            myProps.SetProperty("Abba", "Cadabra");
            myProps.SetProperty("Open", "Sesame");
            myProps.List(ps);
            ps.Flush();
            propList = ps.ToString();
            Assertion.Assert("Property list innacurate", (propList
                    .IndexOf("Abba=Cadabra") >= 0)
                    && (propList.IndexOf("Open=Sesame") >= 0));
        }

        [Test]
        public void test_listLjava_io_PrintWriter()
        {
            TextWriter pw = new StringWriter();
            Properties myProps = new Properties();
            String propList;
            myProps.SetProperty("Abba", "Cadabra");
            myProps.SetProperty("Open", "Sesame");
            myProps.List(pw);
            pw.Flush();
            propList = pw.ToString();
            Assertion.Assert("Property list innacurate", (propList
                    .IndexOf("Abba=Cadabra") >= 0)
                    && (propList.IndexOf("Open=Sesame") >= 0));
        }


        [Test]
        public void test_loadLjava_io_InputStream()
        {
            Properties prop = new Properties();
            TextReader sr = new StringReader(WriteProperties());
            //InputStream iss = new ByteArrayInputStream(writeProperties());
            prop.Load(sr);
            sr.Close();
            Assertion.AssertEquals("Failed to load correct properties", "harmony.tests", prop
                    .GetProperty("test.pkg"));
            Assertion.AssertNull("Load failed to parse incorrectly", prop
                    .GetProperty("commented.entry"));

            prop = new Properties();
            prop.Load(new StringReader("="));
            Assertion.Assert("Failed to add empty key", prop[""].Equals(""));

            prop = new Properties();
            prop.Load(new StringReader(" = "));
            Assertion.Assert("Failed to add empty key2", prop[""].Equals(""));

            prop = new Properties();
            prop.Load(new StringReader(" a= b"));
            Assertion.AssertEquals("Failed to ignore whitespace", "b", prop["a"]);

            prop = new Properties();
            prop.Load(new StringReader(" a b"));
            Assertion.AssertEquals("Failed to interpret whitespace as =", "b", prop["a"]);

            prop = new Properties();
            prop.Load(new StringReader("#\u008d\u00d2\na=\u008d\u00d3"/*
                    .GetBytes("ISO8859_1")*/
                                            ));
            Assertion.AssertEquals("Failed to parse chars >= 0x80", "\u008d\u00d3", prop
                    ["a"]);

            // TROUBLE HERE
            /*prop = new Properties();
            prop.Load(new StringReader(
                    "#properties file\r\nfred=1\r\n#last comment"
                            .GetBytes("ISO8859_1")));
            Assertion.AssertEquals("Failed to load when last line contains a comment", "1",
                    prop["fred"]);*/
            //
            // Regression tests for HARMONY-5414
            prop = new Properties();
            prop.Load(new StringReader("a=\\u1234z"));

            prop = new Properties();
            try
            {
                prop.Load(new StringReader("a=\\u123"));
                Assertion.Fail("Expected IllegalArgumentException due to invalid Unicode sequence");
            }
            catch (ArgumentException e)
            {
                // Expected
            }

            prop = new Properties();
            try
            {
                prop.Load(new StringReader("a=\\u123z"));
                Assertion.Fail("Expected IllegalArgumentException due to invalid unicode sequence");
            }
            catch (ArgumentException)
            {
                // Expected
            }

            prop = new Properties();
            Properties expected = new Properties();
            expected["a"] = "\u0000";
            prop.Load(new StringReader("a=\\"));
            Assertion.AssertEquals("Failed to read trailing slash value", expected, prop);

            prop = new Properties();
            expected = new Properties();
            expected["a"] = "\u1234\u0000";
            prop.Load(new StringReader("a=\\u1234\\"));
            Assertion.AssertEquals("Failed to read trailing slash value #2", expected, prop);

            prop = new Properties();
            expected = new Properties();
            expected["a"] = "q";
            prop.Load(new StringReader("a=\\q"));
            Assertion.AssertEquals("Failed to read slash value #3", expected, prop);
        }

        [Test]
        public void test_propertyNames()
        {
            foreach (String p in tProps.PropertyNames())
            {
                Assertion.Assert("Incorrect names returned", p.Equals("test.prop")
                        || p.Equals("bogus.prop"));
            }
        }

        [Test]
        public void test_saveLjava_io_OutputStreamLjava_lang_String()
        {
            Properties myProps = new Properties();
            myProps.SetProperty("Property A", "aye");
            myProps.SetProperty("Property B", "bee");
            myProps.SetProperty("Property C", "see");

            try
            {
                Stream outs = new FileStream("c://temp//toto.properties2", FileMode.Create);
                myProps.Save(outs, "A Header");
                outs.Close();

                outs = new FileStream("c://temp//toto.properties2", FileMode.Open);
                Properties myProps2 = new Properties();
                StreamReader ins = new StreamReader(outs);
                myProps2.Load(ins);
                ins.Close();
                outs.Close();

                foreach (String nextKey in myProps.PropertyNames())
                {
                    Assertion.Assert("Stored property list not equal to original", myProps2
                            .GetProperty(nextKey).Equals(myProps.GetProperty(nextKey)));
                }
            }
            catch (IOException e)
            {
                Assertion.Fail();
            }
        }

        [Test]
        public void test_setPropertyLjava_lang_StringLjava_lang_String()
        {
            Properties myProps = new Properties();
            myProps.SetProperty("Yoink", "Yabba");
            Assertion.AssertEquals("Failed to set property", "Yabba", myProps
                    .GetProperty("Yoink"));
            myProps.SetProperty("Yoink", "Gab");
            Assertion.AssertEquals("Failed to reset property", "Gab", myProps
                    .GetProperty("Yoink"));
        }

        [Test]
        public void test_storeLjava_io_OutputStreamLjava_lang_String()
        {
            Properties myProps = new Properties();
            myProps["Property A"] = " aye\\\f\t\n\r\b";
            myProps["Property B"] = "b ee#!=:";
            myProps["Property C"] = "see";

            Properties myProps2 = new Properties();
            Stream outs = new FileStream("c://temp//toto.properties2", FileMode.Create);
            // ByteArrayOutputStream outs = new ByteArrayOutputStream();
            myProps.Store(outs, "A Header");
            outs.Close();

            outs = new FileStream("c://temp//toto.properties2", FileMode.Open);
            //ByteArrayInputStream ins = new ByteArrayInputStream(outs.toByteArray());
            StreamReader ins = new StreamReader(outs);
            myProps2.Load(ins);
            ins.Close();

            foreach (String nextKey in myProps.PropertyNames())
            {
                Assertion.Assert("Stored property list not equal to original", myProps2
                        .GetProperty(nextKey).Equals(myProps.GetProperty(nextKey)));
            }
        }

        [Test]
        public void testLoadSingleLine()
        {
            Properties props = new Properties();
            TextReader sr = new StringReader("hello");
            props.Load(sr);
            Assertion.AssertEquals(1, props.Count);
        }

        [SetUp]
        protected void setUp()
        {
            tProps = new Properties();
            tProps["test.prop"] = "this is a test property";
            tProps["bogus.prop"] = "bogus";
        }

        [TearDown]
        protected void tearDown()
        {
            tProps = null;
        }


        protected string WriteProperties()
        {
            TextWriter tw = new StringWriter();
            tw.WriteLine("#commented.entry=Bogus");
            tw.WriteLine("test.pkg=harmony.tests");
            tw.WriteLine("test.proj=Automated Tests");
            tw.Close();
            return tw.ToString();
        }
    }
}
