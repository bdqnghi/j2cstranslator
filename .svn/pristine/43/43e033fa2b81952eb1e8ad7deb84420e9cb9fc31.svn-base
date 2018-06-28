using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Util;

namespace MappingTests
{
    [TestFixture]
    public class Int32UtilTests
    {
        //
        // Decode
        //

        [Test]
        public void TestDecodeDecimal()
        {
            String pattern = "10";
            int res = Int32Helper.Decode(pattern);
            Assert.AreEqual(res, 10);
        }

        [Test]
        public void TestDecodeHexadecimal()
        {
            String pattern = "0xA";
            int res = Int32Helper.Decode(pattern);
            Assert.AreEqual(res, 10);
        }

        [Test]
        public void TestDecodeOctal()
        {
            String pattern = "0234";
            int res = Int32Helper.Decode(pattern);
            Assert.AreEqual(res, 156);
        }

        [Test]
        public void TestDecodeError()
        {
            try
            {
                String pattern = "toto";
                int res = Int32Helper.Decode(pattern);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void TestNumberOfTrailingZeros()
        {
            Assertion.AssertEquals(32, Int32Helper.NumberOfTrailingZeros(0x0));
            Assertion.AssertEquals(31, Int32Helper.NumberOfTrailingZeros(Int32.MinValue));
            Assertion.AssertEquals(0, Int32Helper.NumberOfTrailingZeros(Int32.MaxValue));

            Assertion.AssertEquals(0, Int32Helper.NumberOfTrailingZeros(0x1));
            Assertion.AssertEquals(3, Int32Helper.NumberOfTrailingZeros(0x8));
            Assertion.AssertEquals(0, Int32Helper.NumberOfTrailingZeros(0xF));

            Assertion.AssertEquals(4, Int32Helper.NumberOfTrailingZeros(0x10));
            Assertion.AssertEquals(7, Int32Helper.NumberOfTrailingZeros(0x80));
            Assertion.AssertEquals(4, Int32Helper.NumberOfTrailingZeros(0xF0));

            Assertion.AssertEquals(8, Int32Helper.NumberOfTrailingZeros(0x100));
            Assertion.AssertEquals(11, Int32Helper.NumberOfTrailingZeros(0x800));
            Assertion.AssertEquals(8, Int32Helper.NumberOfTrailingZeros(0xF00));

            Assertion.AssertEquals(12, Int32Helper.NumberOfTrailingZeros(0x1000));
            Assertion.AssertEquals(15, Int32Helper.NumberOfTrailingZeros(0x8000));
            Assertion.AssertEquals(12, Int32Helper.NumberOfTrailingZeros(0xF000));

            Assertion.AssertEquals(16, Int32Helper.NumberOfTrailingZeros(0x10000));
            Assertion.AssertEquals(19, Int32Helper.NumberOfTrailingZeros(0x80000));
            Assertion.AssertEquals(16, Int32Helper.NumberOfTrailingZeros(0xF0000));

            Assertion.AssertEquals(20, Int32Helper.NumberOfTrailingZeros(0x100000));
            Assertion.AssertEquals(23, Int32Helper.NumberOfTrailingZeros(0x800000));
            Assertion.AssertEquals(20, Int32Helper.NumberOfTrailingZeros(0xF00000));

            Assertion.AssertEquals(24, Int32Helper.NumberOfTrailingZeros(0x1000000));
            Assertion.AssertEquals(27, Int32Helper.NumberOfTrailingZeros(0x8000000));
            Assertion.AssertEquals(24, Int32Helper.NumberOfTrailingZeros(0xF000000));

            Assertion.AssertEquals(28, Int32Helper.NumberOfTrailingZeros(0x10000000));
            // Assertion.AssertEquals(31, Int32Helper.NumberOfTrailingZeros(0x80000000));
            // Assertion.AssertEquals(28, Int32Helper.NumberOfTrailingZeros(0xF0000000));
        }

        [Test]
        public void TestBitCount()
        {
            Assertion.AssertEquals(0, Int32Helper.BitCount(0x0));
            Assertion.AssertEquals(1, Int32Helper.BitCount(0x1));
            Assertion.AssertEquals(1, Int32Helper.BitCount(0x2));
            Assertion.AssertEquals(2, Int32Helper.BitCount(0x3));
            Assertion.AssertEquals(1, Int32Helper.BitCount(0x4));
            Assertion.AssertEquals(2, Int32Helper.BitCount(0x5));
            Assertion.AssertEquals(2, Int32Helper.BitCount(0x6));
            Assertion.AssertEquals(3, Int32Helper.BitCount(0x7));
            Assertion.AssertEquals(1, Int32Helper.BitCount(0x8));
            Assertion.AssertEquals(2, Int32Helper.BitCount(0x9));
            Assertion.AssertEquals(2, Int32Helper.BitCount(0xA));
            Assertion.AssertEquals(3, Int32Helper.BitCount(0xB));
            Assertion.AssertEquals(2, Int32Helper.BitCount(0xC));
            Assertion.AssertEquals(3, Int32Helper.BitCount(0xD));
            Assertion.AssertEquals(3, Int32Helper.BitCount(0xE));
            Assertion.AssertEquals(4, Int32Helper.BitCount(0xF));

            Assertion.AssertEquals(8, Int32Helper.BitCount(0xFF));
            Assertion.AssertEquals(12, Int32Helper.BitCount(0xFFF));
            Assertion.AssertEquals(16, Int32Helper.BitCount(0xFFFF));
            Assertion.AssertEquals(20, Int32Helper.BitCount(0xFFFFF));
            Assertion.AssertEquals(24, Int32Helper.BitCount(0xFFFFFF));
            Assertion.AssertEquals(28, Int32Helper.BitCount(0xFFFFFFF));
            // Assertion.AssertEquals(32, Int32Helper.BitCount(0xFFFFFFFF));
        }

        [Test]
        public void TestToString()
        {
            Assertion.AssertEquals("Returned incorrect octal string", "17777777777", IlNumber.ToString(
                    2147483647, 8));
            Assertion.Assert("Returned incorrect hex string--wanted 7fffffff but got: "
                    + IlNumber.ToString(2147483647, 16), IlNumber.ToString(
                    2147483647, 16).Equals("7fffffff"));
            Assertion.AssertEquals("Incorrect string returned", "1111111111111111111111111111111", IlNumber.ToString(2147483647, 2)
                    );
            Assertion.AssertEquals("Incorrect string returned", "2147483647", IlNumber
                    .ToString(2147483647, 10));

            Assertion.AssertEquals("Returned incorrect octal string", "-17777777777", IlNumber.ToString(
                    -2147483647, 8));
            Assertion.Assert("Returned incorrect hex string--wanted -7fffffff but got: "
                    + IlNumber.ToString(-2147483647, 16), IlNumber.ToString(
                    -2147483647, 16).Equals("-7fffffff"));
            Assertion.AssertEquals("Incorrect string returned",
                            "-1111111111111111111111111111111", IlNumber
                    .ToString(-2147483647, 2));
            Assertion.AssertEquals("Incorrect string returned", "-2147483647", IlNumber.ToString(-2147483647,
                    10));

            Assertion.AssertEquals("Returned incorrect octal string", "-20000000000", IlNumber.ToString(
                    -2147483648, 8));
            Assertion.Assert("Returned incorrect hex string--wanted -80000000 but got: "
                    + IlNumber.ToString(-2147483648, 16), IlNumber.ToString(
                    -2147483648, 16).Equals("-80000000"));
            Assertion.AssertEquals("Incorrect string returned",
                            "-10000000000000000000000000000000", IlNumber
                    .ToString(-2147483648, 2));
            Assertion.AssertEquals("Incorrect string returned", "-2147483648", IlNumber.ToString(-2147483648,
                    10));
        }

        [Test]
        public void TestDecodeL()
        {
            Assertion.AssertEquals(0, Int32Helper.Decode("0"));
            Assertion.AssertEquals(1, Int32Helper.Decode("1"));
            Assertion.AssertEquals(-1, Int32Helper.Decode("-1"));
            Assertion.AssertEquals(0xF, Int32Helper.Decode("0xF"));
            Assertion.AssertEquals(0xF, Int32Helper.Decode("#F"));
            Assertion.AssertEquals(0xF, Int32Helper.Decode("0XF"));
            Assertion.AssertEquals(07, Int32Helper.Decode("07"));

            try
            {
                Int32Helper.Decode("9.2");
                Assertion.Fail("Expected NumberFormatException with floating point string.");
            }
            catch (Exception) { }

            try
            {
                Int32Helper.Decode("");
                Assertion.Fail("Expected NumberFormatException with empty string.");
            }
            catch (Exception) { }

            try
            {
                Int32Helper.Decode(null);
                //undocumented NPE, but seems consistent across JREs
                Assertion.Fail("Expected NullPointerException with null string.");
            }
            catch (NullReferenceException) { }
        }
    }
}
