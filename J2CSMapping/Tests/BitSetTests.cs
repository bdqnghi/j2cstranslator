using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Collections;

namespace Tests
{
    [TestFixture]
    public class BitSetTests
    {
        [Test]
        public void test_clearII()
        {           
            BitSet bitset = new BitSet();
            for (int i = 0; i < 20; i++)
            {
                bitset.Set(i);
            }
            bitset.Clear(10, 10);
        }

        [Test]
        public void test_flipII()
        {
            BitSet bitset = new BitSet();
            for (int i = 0; i < 20; i++)
            {
                bitset.Set(i);
            }
            bitset.Flip(10, 10);
        }

        [Test]
        public void test_getII()
        {
            BitSet bitset = new BitSet(30);
            bitset.Get(3, 3);
        }

        [Test]
        public void test_setII()
        {
            BitSet bitset = new BitSet(30);
            bitset.Set(29, 29);
        }
    }
}
