using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILOG.J2CsMapping.IO;
using NUnit.Framework;
using System.IO;

namespace Tests
{
    [TestFixture]
    public class CharArrayReaderTests
    {

        char[] hw = { 'H', 'e', 'l', 'l', 'o', 'W', 'o', 'r', 'l', 'd' };

        CharArrayReader cr;

        [Test]
        public void test_ConstructorC()
        {
            cr = new CharArrayReader(hw);
            Assertion.Assert("Failed to create reader", cr.Ready());
        }

        [Test]
        public void test_ConstructorCII()
        {
            cr = new CharArrayReader(hw, 5, 5);
            Assertion.Assert("Failed to create reader", cr.Ready());

            int c = cr.Read();
            Assertion.Assert("Created incorrect reader--returned '" + (char)c
                    + "' intsead of 'W'", c == 'W');
        }

        [Test]
        public void test_close()
        {
            cr = new CharArrayReader(hw);
            cr.Close();
            try
            {
                cr.Read();
                Assertion.Fail("Failed to throw exception on read from closed stream");
            }
            catch (IOException e)
            {
                // Expected
            }

            // No-op
            cr.Close();
        }

        [Test]
        public void test_markI()
        {
            cr = new CharArrayReader(hw);
            cr.Skip(5L);
            cr.Mark(100);
            cr.Read();
            cr.Reset();
            Assertion.AssertEquals("Failed to mark correct position", 'W', cr.Read());
        }

        [Test]
        public void test_markSupported()
        {
            cr = new CharArrayReader(hw);
            Assertion.Assert("markSupported returned false", cr.MarkSupported());
        }

        [Test]
        public void test_read()
        {
            cr = new CharArrayReader(hw);
            Assertion.AssertEquals("Read returned incorrect char", 'H', cr.Read());
            cr = new CharArrayReader(new char[] { '\u8765' });
            Assertion.Assert("Incorrect double byte char", cr.Read() == '\u8765');
        }

        [Test]
        public void test_readCII()
        {
            char[] c = new char[11];
            cr = new CharArrayReader(hw);
            cr.Read(c, 1, 10);
            Assertion.Assert("Read returned incorrect chars", new String(c, 1, 10)
                    .Equals(new String(hw, 0, 10)));
        }

        [Test]
        public void test_ready()
        {
            cr = new CharArrayReader(hw);
            Assertion.Assert("ready returned false", cr.Ready());
            cr.Skip(1000);
            Assertion.Assert("ready returned true", !cr.Ready());
            cr.Close();

            try
            {
                cr.Ready();
                Assertion.Fail("No exception 1");
            }
            catch (IOException e)
            {
                // expected
            }
            try
            {
                cr = new CharArrayReader(hw);
                cr.Close();
                cr.Ready();
                Assertion.Fail("No exception 2");
            }
            catch (IOException e)
            {
                // expected
            }
        }

        [Test]
        public void test_reset()
        {
            cr = new CharArrayReader(hw);
            cr.Skip(5L);
            cr.Mark(100);
            cr.Read();
            cr.Reset();
            Assertion.AssertEquals("Reset failed to return to marker position", 'W', cr
                    .Read());

            // Regression for HARMONY-4357
            String str = "offsetHello world!";
            char[] data = new char[str.Length];
            data = str.ToCharArray(0, str.Length);
            // str.GetChars(0, str.Length, data, 0);
            int offsetLength = 6;
            int length = data.Length - offsetLength;

            CharArrayReader reader = new CharArrayReader(data, offsetLength, length);
            reader.Reset();
            for (int i = 0; i < length; i++)
            {
                Assertion.AssertEquals(data[offsetLength + i], (char)reader.Read());
            }
        }

        [Test]
        public void test_skipJ()
        {
            cr = new CharArrayReader(hw);
            long skipped = cr.Skip(5L);

            Assertion.AssertEquals("Failed to skip correct number of chars", 5L, skipped);
            Assertion.AssertEquals("Skip skipped wrong chars", 'W', cr.Read());
        }

        /// <summary>
        /// Tears down the fixture, for example, close a network connection. This
        /// method is called after a test is executed.
        /// </summary>
        [TearDown]
        protected void tearDown()
        {
            if (cr != null)
                cr.Close();
        }
    }
}
