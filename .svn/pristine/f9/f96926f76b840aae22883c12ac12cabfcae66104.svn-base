using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Collections;

namespace MappingTests
{
    [TestFixture]
    public class ArraysTests
    {

        public void assertCollectionAreTheSame<T>(T[] b, IList<T> a) where T : IComparable<T>
        {
            if (a.Count != b.Length)
                Assert.Fail("Size differt");
            for (int i = 0; i < a.Count; i++)
            {
                if ((object)a[i] != (object)b[i])
                {
                    Assert.Fail("Element " + i + " differt");
                }

            }
        }

        //
        // AsList
        //

        [Test]
        public void TestAsList()
        {
            String[] pattern = new String[] { "a", "b" };
            IList<String> res = ILOG.J2CsMapping.Collections.Generics.Arrays.AsList<String>(pattern);
            assertCollectionAreTheSame(pattern, res);
        }

        [Test]
        public void TestAsListEmpty()
        {
            String[] pattern = new String[] { };
            IList<String> res = ILOG.J2CsMapping.Collections.Generics.Arrays.AsList<String>(pattern);
            assertCollectionAreTheSame(pattern, res);
        }

        [Test]
        public void TestValueOftNull()
        {
            try
            {
                String[] pattern = null;
                IList<String> res = ILOG.J2CsMapping.Collections.Generics.Arrays.AsList<String>(pattern);
                assertCollectionAreTheSame(pattern, res);
            }
            catch (ArgumentException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        //
        // Fill 4Args
        //

        [Test]
        public void TestFill4Args()
        {
            String[] array = new String[2];
            Arrays.Fill(array, 0, 2, "1");
            assertCollectionAreTheSame(array, new String[] { "1", "1" });
        }

        [Test]
        public void TestFill4ArgsEmpty()
        {
            String[] array = new String[2];
            Arrays.Fill(array, 0, 2, "");
            assertCollectionAreTheSame(array, new String[] { "", "" });
        }

        [Test]
        public void TestFill4ArgsNull()
        {
            try
            {
                String[] array = null;
                Arrays.Fill(array, 0, 2, "");
                assertCollectionAreTheSame(array, null);
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
        // Fill 2Args
        //

        [Test]
        public void TestFill2Args()
        {
            String[] array = new String[2];
            Arrays.Fill(array, "1");
            assertCollectionAreTheSame(array, new String[] { "1", "1" });
        }

        [Test]
        public void TestFill2ArgsEmpty()
        {
            String[] array = new String[2];
            Arrays.Fill(array, "");
            assertCollectionAreTheSame(array, new String[] { "", "" });
        }

        [Test]
        public void TestFill2ArgsNull()
        {
            try
            {
                String[] array = null;
                Arrays.Fill(array, "");
                assertCollectionAreTheSame(array, null);
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
