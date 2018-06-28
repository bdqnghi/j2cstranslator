using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Collections;
using System.Collections;

namespace Tests
{

    [TestFixture]
    public class HashSetTest
    {

        HashedSet hs;

        static Object[] objArray;

        static HashSetTest()
        {
            objArray = new Object[1000];
            for (int i = 0; i < objArray.Length; i++)
                objArray[i] = i;
        }

        [Test]
        public void test_Constructor()
        {
            // Test for method java.util.HashSet()
            HashedSet hs2 = new HashedSet();
            Assertion.AssertEquals("Created incorrect HashSet", 0, hs2.Count);
        }

        [Test]
        public void test_ConstructorI()
        {
            // Test for method java.util.HashSet(int)
            HashedSet hs2 = new HashedSet(5);
            Assertion.AssertEquals("Created incorrect HashSet", 0, hs2.Count);
            try
            {
                new HashedSet(-1);
            }
            catch (ArgumentException e)
            {
                return;
            }
            Assertion.Fail(
                    "Failed to throw IllegalArgumentException for capacity < 0");
        }

        [Test]
        public void test_ConstructorIF()
        {
            // Test for method java.util.HashSet(int, float)
            HashedSet hs2 = new HashedSet(5, (float)0.5);
            Assertion.AssertEquals("Created incorrect HashSet", 0, hs2.Count);
            /*try
            {
                new HashedSet(0, 0);
            }
            catch (ArgumentException e)
            {
                return;
            }
            Assertion.Fail(
                    "Failed to throw IllegalArgumentException for initial load factor <= 0");*/
        }

        [Test]
        public void test_ConstructorLjava_util_Collection()
        {
            // Test for method java.util.HashSet(java.util.Collection)
            HashedSet hs2 = new HashedSet(Arrays.AsList(objArray));
            for (int counter = 0; counter < objArray.Length; counter++)
                Assertion.Assert("HashSet does not contain correct elements", hs
                        .Contains(objArray[counter]));
            Assertion.Assert("HashSet created from collection incorrect size",
                    hs2.Count == objArray.Length);
        }

        [Test]
        public void test_addLjava_lang_Object()
        {
            // Test for method boolean java.util.HashSet.add(java.lang.Object)
            int size = hs.Count;
            hs.Add(8);
            Assertion.Assert("Added element already contained by set", hs.Count == size);
            hs.Add(-9);
            Assertion.Assert("Failed to increment set size after add",
                    hs.Count == size + 1);
            Assertion.Assert("Failed to add element to set", hs.Contains(-9));
        }

        [Test]
        public void test_clear()
        {
            // Test for method void java.util.HashSet.clear()
            ISet orgSet = new HashedSet();
            for (int i1 = 0; i1 < objArray.Length; i1++)
                hs.Add(objArray[i1]);
            hs.Clear();
            IEnumerator i = orgSet.GetEnumerator();
            Assertion.AssertEquals("Returned non-zero size after clear", 0, hs.Count);
            while (i.MoveNext())
                Assertion.Assert("Failed to clear set", !hs.Contains(i.Current));
        }

        /*[Test]
        public void test_clone()
        {
            // Test for method java.lang.Object java.util.HashSet.clone()
            HashedSet hs2 = (HashedSet)hs.Clone();
            Assertion.Assert("clone returned an equivalent HashSet", hs != hs2);
            Assertion.Assert("clone did not return an equal HashSet", hs.Equals(hs2));
        }*/

        [Test]
        public void test_containsLjava_lang_Object()
        {
            // Test for method boolean java.util.HashSet.contains(java.lang.Object)
            Assertion.Assert("Returned false for valid object", hs.Contains(objArray[90]));
            Assertion.Assert("Returned true for invalid Object", !hs
                    .Contains(new Object()));

            /*HashedSet s = new HashedSet();
            s.Add(null);
            Assertion.Assert("Cannot handle null", s.Contains(null));*/
        }

        [Test]
        public void test_isEmpty()
        {
            // Test for method boolean java.util.HashSet.isEmpty()
            Assertion.Assert("Empty set returned false", new HashedSet().Count == 0);
            Assertion.Assert("Non-empty set returned true", !(hs.Count == 0));
        }

        [Test]
        public void test_iterator()
        {
            // Test for method java.util.Iterator java.util.HashSet.iterator()
            IEnumerator i = hs.GetEnumerator();
            int x = 0;
            while (i.MoveNext())
            {
                Assertion.Assert("Failed to iterate over all elements", hs.Contains(i
                        .Current));
                ++x;
            }
            Assertion.Assert("Returned iteration of incorrect size", hs.Count == x);

            /*HashedSet s = new HashedSet();
            s.Add(null);
            Assertion.Assert("Cannot handle null", s.GetEnumerator().MoveNext());*/
        }

        [Test]
        public void test_removeLjava_lang_Object()
        {
            // Test for method boolean java.util.HashSet.remove(java.lang.Object)
            int size = hs.Count;
            hs.Remove(98);
            Assertion.Assert("Failed to remove element", !hs.Contains(98));
            Assertion.Assert("Failed to decrement set size", hs.Count == size - 1);

            /*HashedSet s = new HashedSet();
            s.Add(null);
            Assertion.Assert("Cannot handle null", s.Remove(null));*/
        }

        [Test]
        public void test_size()
        {
            // Test for method int java.util.HashSet.size()
            Assertion.Assert("Returned incorrect size", hs.Count == (objArray.Length));
            hs.Clear();
            Assertion.AssertEquals("Cleared set returned non-zero size", 0, hs.Count);
        }


        [SetUp]
        protected void setUp()
        {
            hs = new HashedSet();
            for (int i = 0; i < objArray.Length; i++)
                hs.Add(objArray[i]);
            //hs.Add(null);
        }
    }
}


