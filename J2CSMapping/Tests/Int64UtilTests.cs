using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Util;

namespace MappingTests
{
    [TestFixture]
    public class Int64UtilTests
    {
        //
        // Decode
        //

        [Test]
        public void TestDecodeDecimal()
        {
            String pattern = "10";
            long res = Int64Helper.Decode(pattern);
            Assert.AreEqual(res, 10);
        }

        [Test]
        public void TestDecodeHexadecimal()
        {
            String pattern = "0xA";
            long res = Int64Helper.Decode(pattern);
            Assert.AreEqual(res, 10);
        }

        [Test]
        public void TestDecodeOctal()
        {
            String pattern = "0234";
            long res = Int64Helper.Decode(pattern);
            Assert.AreEqual(res, 156);
        }

        [Test]
        public void TestDecodeError()
        {
            try
            {
                String pattern = "toto";
                long res = Int64Helper.Decode(pattern);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void TestDecode()
        {
            Assertion.AssertEquals("Returned incorrect value for hex string", 255L, Int64Helper.Decode(
                    "0xFF"));
            Assertion.AssertEquals("Returned incorrect value for dec string", -89000L, Int64Helper.Decode(
                    "-89000"));
            Assertion.AssertEquals("Returned incorrect value for 0 decimal", 0, Int64Helper.Decode("0")
                    );
            Assertion.AssertEquals("Returned incorrect value for 0 hex", 0, Int64Helper.Decode("0x0")
                    );
            /*Assertion.Assert(
                    "Returned incorrect value for most negative value decimal",
                    Int64Helper.Decode("-9223372036854775808") == 0x8000000000000000L);
            Assertion.Assert(
                    "Returned incorrect value for most negative value hex",
                    Int64Helper.Decode("-0x8000000000000000") == 0x8000000000000000L);*/
            Assertion.Assert(
                    "Returned incorrect value for most positive value decimal",
                    Int64Helper.Decode("9223372036854775807") == 0x7fffffffffffffffL);
            Assertion.Assert(
                    "Returned incorrect value for most positive value hex",
                    Int64Helper.Decode("0x7fffffffffffffff") == 0x7fffffffffffffffL);
            //Assertion.Assert("Failed for 07654321765432 " + Int64Helper.Decode("07654321765432"), Int64Helper.Decode("07654321765432")
            //         == 07654321765432L);

            bool exception = false;
            try
            {
                Int64Helper
                        .Decode("999999999999999999999999999999999999999999999999999999");
            }
            catch (Exception e)
            {
                // Correct
                exception = true;
            }
            Assertion.Assert("Failed to throw exception for value > ilong", exception);

            exception = false;
            try
            {
                Int64Helper.Decode("9223372036854775808");
            }
            catch (Exception e)
            {
                // Correct
                exception = true;
            }
            Assertion.Assert("Failed to throw exception for MAX_VALUE + 1", exception);

            exception = false;
            try
            {
                Int64Helper.Decode("-9223372036854775809");
            }
            catch (Exception e)
            {
                // Correct
                exception = true;
            }
            Assertion.Assert("Failed to throw exception for MIN_VALUE - 1", exception);

            exception = false;
            try
            {
                Int64Helper.Decode("0x8000000000000000");
            }
            catch (Exception e)
            {
                // Correct
                exception = true;
            }
            Assertion.Assert("Failed to throw exception for hex MAX_VALUE + 1", exception);

            exception = false;
            try
            {
                Int64Helper.Decode("-0x8000000000000001");
            }
            catch (Exception e)
            {
                // Correct
                exception = true;
            }
            Assertion.Assert("Failed to throw exception for hex MIN_VALUE - 1", exception);

            exception = false;
            try
            {
                Int64Helper.Decode("42325917317067571199");
            }
            catch (Exception e)
            {
                // Correct
                exception = true;
            }
            Assertion.Assert("Failed to throw exception for 42325917317067571199",
                    exception);
        }

        [Test]
        public void TestDecode2()
        {
            Assertion.AssertEquals(0, Int64Helper.Decode("0"));
            Assertion.AssertEquals(1, Int64Helper.Decode("1"));
            Assertion.AssertEquals(-1, Int64Helper.Decode("-1"));
            Assertion.AssertEquals(0xF, Int64Helper.Decode("0xF"));
            Assertion.AssertEquals(0xF, Int64Helper.Decode("#F"));
            Assertion.AssertEquals(0xF, Int64Helper.Decode("0XF"));
            Assertion.AssertEquals(07, Int64Helper.Decode("07"));

            try
            {
                Int64Helper.Decode("9.2");
                Assertion.Fail("Expected NumberFormatException with floating point string.");
            }
            catch (Exception e) { }

            try
            {
                Int64Helper.Decode("");
                Assertion.Fail("Expected NumberFormatException with empty string.");
            }
            catch (Exception e) { }

            try
            {
                Int64Helper.Decode(null);
                Assertion.Fail("Expected NullPointerException with null string.");
            }
            catch (NullReferenceException e) { }
        }

        [Test]
        public void TestNumberOfLeadingZeros()
        {
            Assertion.AssertEquals(64, Int64Helper.NumberOfLeadingZeros(0x0L));
            Assertion.AssertEquals(63, Int64Helper.NumberOfLeadingZeros(0x1));
            Assertion.AssertEquals(62, Int64Helper.NumberOfLeadingZeros(0x2));
            Assertion.AssertEquals(62, Int64Helper.NumberOfLeadingZeros(0x3));
            Assertion.AssertEquals(61, Int64Helper.NumberOfLeadingZeros(0x4));
            Assertion.AssertEquals(61, Int64Helper.NumberOfLeadingZeros(0x5));
            Assertion.AssertEquals(61, Int64Helper.NumberOfLeadingZeros(0x6));
            Assertion.AssertEquals(61, Int64Helper.NumberOfLeadingZeros(0x7));
            Assertion.AssertEquals(60, Int64Helper.NumberOfLeadingZeros(0x8));
            Assertion.AssertEquals(60, Int64Helper.NumberOfLeadingZeros(0x9));
            Assertion.AssertEquals(60, Int64Helper.NumberOfLeadingZeros(0xA));
            Assertion.AssertEquals(60, Int64Helper.NumberOfLeadingZeros(0xB));
            Assertion.AssertEquals(60, Int64Helper.NumberOfLeadingZeros(0xC));
            Assertion.AssertEquals(60, Int64Helper.NumberOfLeadingZeros(0xD));
            Assertion.AssertEquals(60, Int64Helper.NumberOfLeadingZeros(0xE));
            Assertion.AssertEquals(60, Int64Helper.NumberOfLeadingZeros(0xF));
            Assertion.AssertEquals(59, Int64Helper.NumberOfLeadingZeros(0x10));
            Assertion.AssertEquals(56, Int64Helper.NumberOfLeadingZeros(0x80));
            Assertion.AssertEquals(56, Int64Helper.NumberOfLeadingZeros(0xF0));
            Assertion.AssertEquals(55, Int64Helper.NumberOfLeadingZeros(0x100));
            Assertion.AssertEquals(52, Int64Helper.NumberOfLeadingZeros(0x800));
            Assertion.AssertEquals(52, Int64Helper.NumberOfLeadingZeros(0xF00));
            Assertion.AssertEquals(51, Int64Helper.NumberOfLeadingZeros(0x1000));
            Assertion.AssertEquals(48, Int64Helper.NumberOfLeadingZeros(0x8000));
            Assertion.AssertEquals(48, Int64Helper.NumberOfLeadingZeros(0xF000));
            Assertion.AssertEquals(47, Int64Helper.NumberOfLeadingZeros(0x10000));
            Assertion.AssertEquals(44, Int64Helper.NumberOfLeadingZeros(0x80000));
            Assertion.AssertEquals(44, Int64Helper.NumberOfLeadingZeros(0xF0000));
            Assertion.AssertEquals(43, Int64Helper.NumberOfLeadingZeros(0x100000));
            Assertion.AssertEquals(40, Int64Helper.NumberOfLeadingZeros(0x800000));
            Assertion.AssertEquals(40, Int64Helper.NumberOfLeadingZeros(0xF00000));
            Assertion.AssertEquals(39, Int64Helper.NumberOfLeadingZeros(0x1000000));
            Assertion.AssertEquals(36, Int64Helper.NumberOfLeadingZeros(0x8000000));
            Assertion.AssertEquals(36, Int64Helper.NumberOfLeadingZeros(0xF000000));
            Assertion.AssertEquals(35, Int64Helper.NumberOfLeadingZeros(0x10000000));
            // ?? Assertion.AssertEquals(0, Int64Helper.NumberOfLeadingZeros(0x80000000));
           // ??  Assertion.AssertEquals(0, Int64Helper.NumberOfLeadingZeros(0xF0000000));

            Assertion.AssertEquals(1, Int64Helper.NumberOfLeadingZeros(Int64.MaxValue));
            Assertion.AssertEquals(0, Int64Helper.NumberOfLeadingZeros(Int64.MinValue));
        }

        [Test]
        public void TestNumberOfTrailingZeros()
        {
            Assertion.AssertEquals(64, Int64Helper.NumberOfTrailingZeros(0x0));
            Assertion.AssertEquals(63, Int64Helper.NumberOfTrailingZeros(Int64.MinValue));
            Assertion.AssertEquals(0, Int64Helper.NumberOfTrailingZeros(Int64.MaxValue));

            Assertion.AssertEquals(0, Int64Helper.NumberOfTrailingZeros(0x1));
            Assertion.AssertEquals(3, Int64Helper.NumberOfTrailingZeros(0x8));
            Assertion.AssertEquals(0, Int64Helper.NumberOfTrailingZeros(0xF));

            Assertion.AssertEquals(4, Int64Helper.NumberOfTrailingZeros(0x10));
            Assertion.AssertEquals(7, Int64Helper.NumberOfTrailingZeros(0x80));
            Assertion.AssertEquals(4, Int64Helper.NumberOfTrailingZeros(0xF0));

            Assertion.AssertEquals(8, Int64Helper.NumberOfTrailingZeros(0x100));
            Assertion.AssertEquals(11, Int64Helper.NumberOfTrailingZeros(0x800));
            Assertion.AssertEquals(8, Int64Helper.NumberOfTrailingZeros(0xF00));

            Assertion.AssertEquals(12, Int64Helper.NumberOfTrailingZeros(0x1000));
            Assertion.AssertEquals(15, Int64Helper.NumberOfTrailingZeros(0x8000));
            Assertion.AssertEquals(12, Int64Helper.NumberOfTrailingZeros(0xF000));

            Assertion.AssertEquals(16, Int64Helper.NumberOfTrailingZeros(0x10000));
            Assertion.AssertEquals(19, Int64Helper.NumberOfTrailingZeros(0x80000));
            Assertion.AssertEquals(16, Int64Helper.NumberOfTrailingZeros(0xF0000));

            Assertion.AssertEquals(20, Int64Helper.NumberOfTrailingZeros(0x100000));
            Assertion.AssertEquals(23, Int64Helper.NumberOfTrailingZeros(0x800000));
            Assertion.AssertEquals(20, Int64Helper.NumberOfTrailingZeros(0xF00000));

            Assertion.AssertEquals(24, Int64Helper.NumberOfTrailingZeros(0x1000000));
            Assertion.AssertEquals(27, Int64Helper.NumberOfTrailingZeros(0x8000000));
            Assertion.AssertEquals(24, Int64Helper.NumberOfTrailingZeros(0xF000000));

            Assertion.AssertEquals(28, Int64Helper.NumberOfTrailingZeros(0x10000000));
            Assertion.AssertEquals(31, Int64Helper.NumberOfTrailingZeros(0x80000000));
            Assertion.AssertEquals(28, Int64Helper.NumberOfTrailingZeros(0xF0000000));
        }

        [Test]
        public void TestBitCount()
        {
            Assertion.AssertEquals(0, Int64Helper.BitCount(0x0));
            Assertion.AssertEquals(1, Int64Helper.BitCount(0x1));
            Assertion.AssertEquals(1, Int64Helper.BitCount(0x2));
            Assertion.AssertEquals(2, Int64Helper.BitCount(0x3));
            Assertion.AssertEquals(1, Int64Helper.BitCount(0x4));
            Assertion.AssertEquals(2, Int64Helper.BitCount(0x5));
            Assertion.AssertEquals(2, Int64Helper.BitCount(0x6));
            Assertion.AssertEquals(3, Int64Helper.BitCount(0x7));
            Assertion.AssertEquals(1, Int64Helper.BitCount(0x8));
            Assertion.AssertEquals(2, Int64Helper.BitCount(0x9));
            Assertion.AssertEquals(2, Int64Helper.BitCount(0xA));
            Assertion.AssertEquals(3, Int64Helper.BitCount(0xB));
            Assertion.AssertEquals(2, Int64Helper.BitCount(0xC));
            Assertion.AssertEquals(3, Int64Helper.BitCount(0xD));
            Assertion.AssertEquals(3, Int64Helper.BitCount(0xE));
            Assertion.AssertEquals(4, Int64Helper.BitCount(0xF));

            Assertion.AssertEquals(8, Int64Helper.BitCount(0xFF));
            Assertion.AssertEquals(12, Int64Helper.BitCount(0xFFF));
            Assertion.AssertEquals(16, Int64Helper.BitCount(0xFFFF));
            Assertion.AssertEquals(20, Int64Helper.BitCount(0xFFFFF));
            Assertion.AssertEquals(24, Int64Helper.BitCount(0xFFFFFF));
            Assertion.AssertEquals(28, Int64Helper.BitCount(0xFFFFFFF));
            //Assertion.AssertEquals(64, Int64Helper.BitCount(0xFFFFFFFFFFFFFFFFL));
        }
    }
}
