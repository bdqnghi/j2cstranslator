using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Collections.Generics;

namespace Tests
{
    [TestFixture]
    public class EnumSetTests
    {

        enum EnumWithInnerClass
        {
            a, b, c, d, e, f
        }

        enum EnumWithAllInnerClass
        {
            a,
            b
        }

        enum EnumFoo
        {
            a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk, ll
        }

        enum EmptyEnum
        {
            // expected
        }

        enum HugeEnumWithInnerClass
        {
            a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk, ll, mm,
        }

        enum HugeEnum
        {
            a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk, ll, mm
        }

        [Test]
        public void TestNoneOf_LClass()
        {

            try
            {
                EnumSet<Enum>.NoneOf<Enum>(typeof(Enum));
                Assertion.Fail("Should throw Exception");
            }
            catch (Exception)
            {
                // expected
            }

            Type c = (Type)EnumWithAllInnerClass.a
                     .GetType();
            try
            {
                EnumSet<EnumWithAllInnerClass>.NoneOf<EnumWithAllInnerClass>(c);
                Assertion.Fail("Should throw Exception");
            }
            catch (Exception)
            {
                // expected
            }

            EnumSet<EnumWithAllInnerClass> setWithInnerClass = EnumSet<EnumWithAllInnerClass>
                    .NoneOf<EnumWithAllInnerClass>(typeof(EnumWithAllInnerClass));
            Assertion.AssertNotNull(setWithInnerClass);

            // test enum type with more than 64 elements
            Type hc = (Type)HugeEnumWithInnerClass.a
                .GetType();
            try
            {
                EnumSet<HugeEnumWithInnerClass>.NoneOf<HugeEnumWithInnerClass>(hc);
                Assertion.Fail("Should throw Exception");
            }
            catch (Exception)
            {
                // expected
            }

            EnumSet<HugeEnumWithInnerClass> hugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>
                .NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            Assertion.AssertNotNull(hugeSetWithInnerClass);
        }

        [Test]
        public void TestAllOf_LClass()
        {
            try
            {
                EnumSet<Enum>.AllOf<Enum>(typeof(Enum));
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception cce)
            {
                // expected
            }

            EnumSet<EnumFoo> enumSet = EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo));
            Assertion.AssertEquals("Size of enumSet should be 64", 64, enumSet.Count); 

            //Assertion.Assert(
            //        "enumSet should not contain null value", !enumSet.Contains(null)); 
            Assertion.Assert(
                    "enumSet should contain EnumFoo.a", enumSet.Contains(EnumFoo.a)); 
            Assertion.Assert(
                    "enumSet should contain EnumFoo.b", enumSet.Contains(EnumFoo.b)); 

            enumSet.Add(EnumFoo.a);
            Assertion.AssertEquals("Should be equal", 64, enumSet.Count); 

            EnumSet<EnumFoo> anotherSet = EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo));
            Assertion.AssertEquals("Should be equal", enumSet, anotherSet); 
            // Assertion.Assert("Should not be identical", !enumSet.Equals(anotherSet));
            Assert.AreNotSame(enumSet, anotherSet, "Should not be identical");

            // test enum with more than 64 elements
            EnumSet<HugeEnum> hugeEnumSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            Assertion.AssertEquals(65, hugeEnumSet.Count);

            //Assertion.Assert(!hugeEnumSet.Contains(null));
            Assertion.Assert(hugeEnumSet.Contains(HugeEnum.a));
            Assertion.Assert(hugeEnumSet.Contains(HugeEnum.b));

            hugeEnumSet.Add(HugeEnum.a);
            Assertion.AssertEquals(65, hugeEnumSet.Count);

            EnumSet<HugeEnum> anotherHugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            Assertion.AssertEquals(hugeEnumSet, anotherHugeSet);
            //Assertion.Assert(!hugeEnumSet.Equals(anotherHugeSet));
            Assert.AreNotSame(hugeEnumSet, anotherHugeSet);
        }

        [Test]
        public void Testadd_E()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            set.Add(EnumFoo.a);
            set.Add(EnumFoo.b);

            /*try
            {
                set.Add(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }*/

            // test enum type with more than 64 elements
            /*ISet rawSet = set;
            try
            {
                rawSet.Add(HugeEnumWithInnerClass.b);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }
            */
            set.Clear();
            /*try
            {
                set.Add(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }*/

            bool result = set.Add(EnumFoo.a);
            Assertion.AssertEquals("Size should be 1:", 1, set.Count); 
            Assertion.Assert("Return value should be true", result); 

            result = set.Add(EnumFoo.a);
            Assertion.AssertEquals("Size should be 1:", 1, set.Count); 
            Assertion.Assert("Return value should be false", !result); 

            set.Add(EnumFoo.b);
            Assertion.AssertEquals("Size should be 2:", 2, set.Count); 

            /*rawSet = set;
            try
            {
                rawSet.Add(EnumWithAllInnerClass.a);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }

            try
            {
                rawSet.Add(EnumWithInnerClass.a);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }

            try
            {
                rawSet.Add(new Object());
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }
            */
            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            result = hugeSet.Add(HugeEnum.a);
            Assertion.Assert(result);

            result = hugeSet.Add(HugeEnum.a);
            Assertion.Assert(!result);

            /*
            rawSet = hugeSet;
            try
            {
                rawSet.Add(HugeEnumWithInnerClass.b);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }

            try
            {
                rawSet.Add(new Object());
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }
            */
            result = hugeSet.Add(HugeEnum.mm);
            Assertion.Assert(result);
            result = hugeSet.Add(HugeEnum.mm);
            Assertion.Assert(!result);
            Assertion.AssertEquals(2, hugeSet.Count);

        }

        [Test]
        public void TestaddAll_LCollection()
        {

            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            Assertion.AssertEquals("Size should be 0:", 0, set.Count); 

            try
            {
                set.AddAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            ISet<EmptyEnum> emptySet = EnumSet<EmptyEnum>.NoneOf<EmptyEnum>(typeof(EmptyEnum));
            EmptyEnum[] elements = (EmptyEnum[])Enum.GetValues(typeof(EmptyEnum));
            for (int i = 0; i < elements.Length; i++)
            {
                emptySet.Add((EmptyEnum)elements[i]);
            }
            //bool result = set.AddAll(emptySet);
            //Assertion.Assert(!result);

            ICollection<EnumFoo> collection = new List<EnumFoo>();
            collection.Add(EnumFoo.a);
            collection.Add(EnumFoo.b);
            bool result = set.AddAll(collection);
            Assertion.Assert("addAll should be successful", result); 
            Assertion.AssertEquals("Size should be 2:", 2, set.Count); 

            set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));

            /*ICollection<Int32> rawCollection = new List<Int32>();
            result = set.AddAll(rawCollection);
            Assertion.Assert(!result);
            rawCollection.Add(1);
            try
            {
                set.AddAll(rawCollection);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }
            */
            ISet<EnumFoo> fullSet = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            fullSet.Add(EnumFoo.a);
            fullSet.Add(EnumFoo.b);
            result = set.AddAll(fullSet);
            Assertion.Assert("addAll should be successful", result); 
            Assertion.AssertEquals("Size of set should be 2", 2, set.Count); 

            try
            {
                fullSet.AddAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            ISet<EnumWithInnerClass> fullSetWithSubclass = EnumSet<EnumWithInnerClass>.NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            EnumWithInnerClass[]  elements1 = (EnumWithInnerClass[])Enum.GetValues(typeof(EnumWithInnerClass));
            for (int i = 0; i < elements1.Length; i++)
            {
                fullSetWithSubclass.Add((EnumWithInnerClass)elements1[i]);
            }
            /*try
            {
                set.AddAll(fullSetWithSubclass);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }*/
            ISet<EnumWithInnerClass> setWithSubclass = fullSetWithSubclass;
            result = setWithSubclass.AddAll(setWithSubclass);
            Assertion.Assert("Should return false", !result); 

            ISet<EnumWithInnerClass> anotherSetWithSubclass = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            EnumWithInnerClass[] elements2 = (EnumWithInnerClass[])Enum.GetValues(typeof(EnumWithInnerClass));
            for (int i = 0; i < elements2.Length; i++)
            {
                anotherSetWithSubclass.Add((EnumWithInnerClass)elements2[i]);
            }
            result = setWithSubclass.AddAll(anotherSetWithSubclass);
            Assertion.Assert("Should return false", !result); 

            anotherSetWithSubclass.Remove(EnumWithInnerClass.a);
            result = setWithSubclass.AddAll(anotherSetWithSubclass);
            Assertion.Assert("Should return false", !result); 

            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            Assertion.AssertEquals(0, hugeSet.Count);

            try
            {
                hugeSet.AddAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            result = hugeSet.AddAll(hugeSet);
            Assertion.Assert(!result);

            hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            ICollection<HugeEnum> hugeCollection = new List<HugeEnum>();
            hugeCollection.Add(HugeEnum.a);
            hugeCollection.Add(HugeEnum.b);
            result = hugeSet.AddAll(hugeCollection);
            Assertion.Assert(result);
            Assertion.AssertEquals(2, set.Count);

            hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));

            /*rawCollection = new List<Int32>();
            result = hugeSet.AddAll(rawCollection);
            Assertion.Assert(!result);
            rawCollection.Add(1);
            try
            {
                hugeSet.AddAll(rawCollection);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }*/

            EnumSet<HugeEnum> aHugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            aHugeSet.Add(HugeEnum.a);
            aHugeSet.Add(HugeEnum.b);
            result = hugeSet.AddAll(aHugeSet);
            Assertion.Assert(result);
            Assertion.AssertEquals(2, hugeSet.Count);

            try
            {
                aHugeSet.AddAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            ISet<HugeEnumWithInnerClass> hugeSetWithSubclass = EnumSet<HugeEnumWithInnerClass>.AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            /*try
            {
                hugeSet.AddAll(hugeSetWithSubclass);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }*/
            ISet<HugeEnumWithInnerClass> hugeSetWithInnerSubclass = hugeSetWithSubclass;
            result = hugeSetWithInnerSubclass.AddAll(hugeSetWithInnerSubclass);
            Assertion.Assert(!result);

            ISet<HugeEnumWithInnerClass> anotherHugeSetWithSubclass = EnumSet<HugeEnumWithInnerClass>
                    .AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            result = hugeSetWithSubclass.AddAll(anotherHugeSetWithSubclass);
            Assertion.Assert(!result);

            anotherHugeSetWithSubclass.Remove(HugeEnumWithInnerClass.a);
            result = setWithSubclass.AddAll(anotherSetWithSubclass);
            Assertion.Assert(!result);

        }

        [Test]
        public void Testremove_LObject()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            EnumFoo[] elements = (EnumFoo[])Enum.GetValues(typeof(EnumFoo));
            for (int i = 0; i < elements.Length; i++)
            {
                set.Add((EnumFoo)elements[i]);
            }

            //bool result = set.Remove(null);
            //Assertion.Assert("'set' does not contain null", !result); 

            bool result = set.Remove(EnumFoo.a);
            Assertion.Assert("Should return true", result); 
            result = set.Remove(EnumFoo.a);
            Assertion.Assert("Should return false", !result); 

            Assertion.AssertEquals("Size of set should be 63:", 63, set.Count); 

            /*result = set.Remove(EnumWithInnerClass.a);
            Assertion.Assert("Should return false", !result); 
            result = set.Remove(EnumWithInnerClass.f);
            Assertion.Assert("Should return false", !result); 
            */
            // test enum with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));

            //result = hugeSet.Remove(null);
            Assertion.Assert("'set' does not contain null", !result); 

            result = hugeSet.Remove(HugeEnum.a);
            Assertion.Assert("Should return true", result); 
            result = hugeSet.Remove(HugeEnum.a);
            Assertion.Assert("Should return false", !result); 

            Assertion.AssertEquals("Size of set should be 64:", 64, hugeSet.Count); 

            /*result = hugeSet.Remove(HugeEnumWithInnerClass.a);
            Assertion.Assert("Should return false", !result); 
            result = hugeSet.Remove(HugeEnumWithInnerClass.f);
            Assertion.Assert("Should return false", !result); */
        }

        [Test]
        public void Testequals_LObject()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            EnumFoo[] elements = (EnumFoo[])Enum.GetValues(typeof(EnumFoo));
            for (int i = 0; i < elements.Length; i++)
            {
                set.Add((EnumFoo)elements[i]);
            }

            Assertion.Assert("Should return false", !set.Equals(null)); 
            Assertion.Assert(
                    "Should return false", !set.Equals(new Object())); 

            ISet<EnumFoo> anotherSet = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            elements = (EnumFoo[])Enum.GetValues(typeof(EnumFoo));
            for (int i = 0; i < elements.Length; i++)
            {
                anotherSet.Add((EnumFoo)elements[i]);
            }
            Assertion.Assert("Should return true", set.Equals(anotherSet)); 

            anotherSet.Remove(EnumFoo.a);
            Assertion.Assert(
                    "Should return false", !set.Equals(anotherSet)); 

            ISet<EnumWithInnerClass> setWithInnerClass = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            elements = (EnumFoo[])Enum.GetValues(typeof(EnumWithInnerClass));
            for (int i = 0; i < elements.Length; i++)
            {
                setWithInnerClass.Add((EnumWithInnerClass)elements[i]);
            }

            Assertion.Assert(
                    "Should return false", !set.Equals(setWithInnerClass)); 

            setWithInnerClass.Clear();
            set.Clear();
            //Assertion.Assert("Should be equal", set.Equals(setWithInnerClass));
            Assert.AreNotSame(set, setWithInnerClass, "Should be equal");
            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            //Assertion.Assert(hugeSet.Equals(set));
            Assert.AreNotSame(hugeSet, set);

            hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            Assertion.Assert(!hugeSet.Equals(null));
            //Assertion.Assert(!hugeSet.Equals(new Object()));

            ISet<HugeEnum> anotherHugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            anotherHugeSet.Remove(HugeEnum.a);
            Assertion.Assert(!hugeSet.Equals(anotherHugeSet));

            ISet<HugeEnumWithInnerClass> hugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>
                    .AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            //Assertion.Assert(!hugeSet.Equals(hugeSetWithInnerClass));
            Assert.AreNotSame(hugeSet, hugeSetWithInnerClass);

            hugeSetWithInnerClass.Clear();
            hugeSet.Clear();
            //Assertion.Assert(hugeSet.Equals(hugeSetWithInnerClass));
            Assert.AreNotSame(hugeSet, hugeSetWithInnerClass);
        }

        [Test]
        public void Testclear()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            set.Add(EnumFoo.a);
            set.Add(EnumFoo.b);
            Assertion.AssertEquals("Size should be 2", 2, set.Count); 

            set.Clear();

            Assertion.AssertEquals("Size should be 0", 0, set.Count); 

            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            Assertion.AssertEquals(65, hugeSet.Count);

            bool result = hugeSet.Contains(HugeEnum.aa);
            Assertion.Assert(result);

            hugeSet.Clear();
            Assertion.AssertEquals(0, hugeSet.Count);
            result = hugeSet.Contains(HugeEnum.aa);
            Assertion.Assert(!result);
        }

        [Test]
        public void Testsize()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            set.Add(EnumFoo.a);
            set.Add(EnumFoo.b);
            Assertion.AssertEquals("Size should be 2", 2, set.Count); 

            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            hugeSet.Add(HugeEnum.a);
            hugeSet.Add(HugeEnum.bb);
            Assertion.AssertEquals("Size should be 2", 2, hugeSet.Count); 
        }

        [Test]
        public void TestComplementOf_LEnumSet()
        {

            try
            {
                EnumSet<EnumFoo>.ComplementOf((EnumSet<EnumFoo>)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }

            EnumSet<EnumWithInnerClass> set = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            set.Add(EnumWithInnerClass.d);
            set.Add(EnumWithInnerClass.e);
            set.Add(EnumWithInnerClass.f);

            Assertion.AssertEquals("Size should be 3:", 3, set.Count); 

            EnumSet<EnumWithInnerClass> complementOfE = EnumSet<EnumWithInnerClass>.ComplementOf<EnumWithInnerClass>(set);
            Assertion.Assert(set.Contains(EnumWithInnerClass.d));
            Assertion.AssertEquals(
                    "complementOfE should have size 3", 3, complementOfE.Count); 
            Assertion.Assert("complementOfE should contain EnumWithSubclass.a:",  
                    complementOfE.Contains(EnumWithInnerClass.a));
            Assertion.Assert("complementOfE should contain EnumWithSubclass.b:", 
                    complementOfE.Contains(EnumWithInnerClass.b));
            Assertion.Assert("complementOfE should contain EnumWithSubclass.c:", 
                    complementOfE.Contains(EnumWithInnerClass.c));

            // test enum type with more than 64 elements
            EnumSet<HugeEnum> hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            Assertion.AssertEquals(0, hugeSet.Count);
            ISet<HugeEnum> complementHugeSet = EnumSet<HugeEnum>.ComplementOf<HugeEnum>(hugeSet);
            Assertion.AssertEquals(65, complementHugeSet.Count);

            hugeSet.Add(HugeEnum.A);
            hugeSet.Add(HugeEnum.mm);
            complementHugeSet = EnumSet<HugeEnum>.ComplementOf<HugeEnum>(hugeSet);
            Assertion.AssertEquals(63, complementHugeSet.Count);

            try
            {
                EnumSet<HugeEnum>.ComplementOf((EnumSet<HugeEnum>)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException)
            {
                // expected
            }
        }

        [Test]
        public void Testcontains_LObject()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            EnumFoo[] elements = (EnumFoo[])Enum.GetValues(typeof(EnumFoo));
            for (int i = 0; i < elements.Length; i++)
            {
                set.Add((EnumFoo)elements[i]);
            }
            // bool result = set.Contains(null);
            //Assertion.Assert("Should not contain null:", !result); 

            bool result = set.Contains(EnumFoo.a);
            Assertion.Assert("Should contain EnumFoo.a", result); 
            result = set.Contains(EnumFoo.ll);
            Assertion.Assert("Should contain EnumFoo.ll", result); 

            result = set.Contains(EnumFoo.b);
            Assertion.Assert("Should contain EnumFoo.b", result); 

            //result = set.Contains(new Object());
            //Assertion.Assert("Should not contain Object instance", !result); 

            //result = set.Contains(EnumWithInnerClass.a);
            //Assertion.Assert("Should not contain EnumWithSubclass.a", !result); 

            set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            set.Add(EnumFoo.aa);
            set.Add(EnumFoo.bb);
            set.Add(EnumFoo.cc);

            Assertion.AssertEquals("Size of set should be 3", 3, set.Count); 
            Assertion.Assert("set should contain EnumFoo.aa", set.Contains(EnumFoo.aa)); 

            ISet<EnumWithInnerClass> setWithSubclass = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            setWithSubclass.Add(EnumWithInnerClass.a);
            setWithSubclass.Add(EnumWithInnerClass.b);
            setWithSubclass.Add(EnumWithInnerClass.c);
            setWithSubclass.Add(EnumWithInnerClass.d);
            setWithSubclass.Add(EnumWithInnerClass.e);
            setWithSubclass.Add(EnumWithInnerClass.f);
            result = setWithSubclass.Contains(EnumWithInnerClass.f);
            Assertion.Assert("Should contain EnumWithSubclass.f", result); 

            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            hugeSet.Add(HugeEnum.a);
            result = hugeSet.Contains(HugeEnum.a);
            Assertion.Assert(result);

            result = hugeSet.Contains(HugeEnum.b);
            Assertion.Assert(result);

            //result = hugeSet.Contains(null);
            //Assertion.Assert(!result);

            result = hugeSet.Contains(HugeEnum.a);
            Assertion.Assert(result);

            result = hugeSet.Contains(HugeEnum.ll);
            Assertion.Assert(result);

            //result = hugeSet.Contains(new Object());
            //Assertion.Assert(!result);

            // result = hugeSet.Contains(typeof(Enum));
            //Assertion.Assert(!result);

        }

        [Test]
        public void TestcontainsAll_LCollection()
        {
            EnumSet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            EnumFoo[] elements = (EnumFoo[])Enum.GetValues(typeof(EnumFoo));
            for (int i = 0; i < elements.Length; i++)
            {
                set.Add((EnumFoo)elements[i]);
            }
            try
            {
                set.ContainsAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            EnumSet<EmptyEnum> emptySet = EnumSet<EmptyEnum>.NoneOf<EmptyEnum>(typeof(EmptyEnum));
            EmptyEnum[] elements1 = (EmptyEnum[])Enum.GetValues(typeof(EmptyEnum));
            for (int i = 0; i < elements1.Length; i++)
            {
                emptySet.Add((EmptyEnum)elements1[i]);
            }
            // bool result = set.ContainsAll(emptySet);
            //Assertion.Assert("Should return true", result); 

            //ICollection rawCollection = new ArrayList();
            //result = set.ContainsAll(rawCollection);
            //Assertion.Assert("Should contain empty collection:", result); 

            //rawCollection.Add(1);
            //result = set.ContainsAll(rawCollection);
            //Assertion.Assert("Should return false", !result); 

            //rawCollection.Add(EnumWithInnerClass.a);
            //result = set.ContainsAll(rawCollection);
            //Assertion.Assert("Should return false", !result); 

            EnumSet<EnumFoo> rawSet = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            bool result = set.ContainsAll(rawSet);
            Assertion.Assert("Should contain empty set", result); 

            emptySet = EnumSet<EmptyEnum>.NoneOf<EmptyEnum>(typeof(EmptyEnum));
            //result = set.ContainsAll(emptySet);
            //Assertion.Assert("No class cast should be performed on empty set", result); 

            ICollection<EnumFoo> collection = new List<EnumFoo>();
            collection.Add(EnumFoo.a);
            result = set.ContainsAll(collection);
            Assertion.Assert("Should contain all elements in collection", result); 

            EnumSet<EnumFoo> fooSet = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            fooSet.Add(EnumFoo.a);
            result = set.ContainsAll(fooSet);
            Assertion.Assert("Should return true", result); 

            set.Clear();
            try
            {
                set.ContainsAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            ICollection<EnumWithInnerClass> collectionWithSubclass = new List<EnumWithInnerClass>();
            collectionWithSubclass.Add(EnumWithInnerClass.a);
            //result = set.ContainsAll(collectionWithSubclass);
            //Assertion.Assert("Should return false", !result); 

            EnumSet<EnumWithInnerClass> setWithSubclass = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            setWithSubclass.Add(EnumWithInnerClass.a);
            //result = set.ContainsAll(setWithSubclass);
            //Assertion.Assert("Should return false", !result); 

            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            hugeSet.Add(HugeEnum.a);
            hugeSet.Add(HugeEnum.b);
            hugeSet.Add(HugeEnum.aa);
            hugeSet.Add(HugeEnum.bb);
            hugeSet.Add(HugeEnum.cc);
            hugeSet.Add(HugeEnum.dd);

            ISet<HugeEnum> anotherHugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            hugeSet.Add(HugeEnum.b);
            hugeSet.Add(HugeEnum.cc);
            result = hugeSet.ContainsAll(anotherHugeSet);
            Assertion.Assert(result);

            try
            {
                hugeSet.ContainsAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            ISet<HugeEnumWithInnerClass> hugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>
                    .NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            hugeSetWithInnerClass.Add(HugeEnumWithInnerClass.a);
            hugeSetWithInnerClass.Add(HugeEnumWithInnerClass.b);
            result = hugeSetWithInnerClass.ContainsAll(hugeSetWithInnerClass);
            Assertion.Assert(result);
            //result = hugeSet.ContainsAll(hugeSetWithInnerClass);
            //Assertion.Assert(!result);

            /*rawCollection = new ArrayList();
            result = hugeSet.ContainsAll(rawCollection);
            Assertion.Assert("Should contain empty collection:", result); 

            rawCollection.Add(1);
            result = hugeSet.ContainsAll(rawCollection);
            Assertion.Assert("Should return false", !result); 

            rawCollection.Add(EnumWithInnerClass.a);
            result = set.ContainsAll(rawCollection);
            Assertion.Assert("Should return false", !result); */

            // rawSet = EnumSet.NoneOf(typeof(HugeEnum));
            //result = hugeSet.ContainsAll(rawSet);
            // Assertion.Assert("Should contain empty set", result); 

            EnumSet<HugeEnumWithInnerClass> emptyHugeSet
                = EnumSet<HugeEnumWithInnerClass>.NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            //result = hugeSet.ContainsAll(emptyHugeSet);
            //Assertion.Assert("No class cast should be performed on empty set", result); 

            ICollection<HugeEnum> hugeCollection = new List<HugeEnum>();
            hugeCollection.Add(HugeEnum.a);
            result = hugeSet.ContainsAll(hugeCollection);
            Assertion.Assert("Should contain all elements in collection", result); 

            hugeSet.Clear();
            try
            {
                hugeSet.ContainsAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            ICollection<HugeEnumWithInnerClass> hugeCollectionWithSubclass = new List<HugeEnumWithInnerClass>();
            hugeCollectionWithSubclass.Add(HugeEnumWithInnerClass.a);
            //result = hugeSet.ContainsAll(hugeCollectionWithSubclass);
            //Assertion.Assert("Should return false", !result); 

            EnumSet<HugeEnumWithInnerClass> hugeSetWithSubclass = EnumSet<HugeEnumWithInnerClass>
                    .NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            hugeSetWithSubclass.Add(HugeEnumWithInnerClass.a);
            //result = hugeSet.ContainsAll(hugeSetWithSubclass);
            //Assertion.Assert("Should return false", !result); 
        }

        [Test]
        public void TestCopyOf_LCollection()
        {
            /*try
            {
                EnumSet.CopyOf((Collection)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }

            ICollection collection = new ArrayList();
            try
            {
                EnumSet.CopyOf(collection);
                Assertion.Fail("Should throw IllegalArgumentException"); 
            }
            catch (Exception e)
            {
                // expected
            }

            collection.Add(new Object());
            try
            {
                EnumSet.CopyOf(collection);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }
            */
            ICollection<EnumFoo> enumCollection = new List<EnumFoo>();
            enumCollection.Add(EnumFoo.b);

            EnumSet<EnumFoo> copyOfEnumCollection = EnumSet<EnumFoo>.CopyOf<EnumFoo>(enumCollection);
            Assertion.AssertEquals("Size of copyOfEnumCollection should be 1:", 
                    1, copyOfEnumCollection.Count);
            Assertion.Assert("copyOfEnumCollection should contain EnumFoo.b:", 
                    copyOfEnumCollection.Contains(EnumFoo.b));

            //enumCollection.Add(null);
            //Assertion.AssertEquals("Size of enumCollection should be 2:", 
            //        2, enumCollection.Count);

            /*try
            {
                copyOfEnumCollection = EnumSet<EnumFoo>.CopyOf<EnumFoo>(enumCollection);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException)
            {
                // expected
            }*/

            /*ICollection rawEnumCollection = new ArrayList();
            rawEnumCollection.Add(EnumFoo.a);
            rawEnumCollection.Add(EnumWithInnerClass.a);
            try
            {
                EnumSet.CopyOf(rawEnumCollection);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }
            */
            // test enum type with more than 64 elements
            ICollection<HugeEnum> hugeEnumCollection = new List<HugeEnum>();
            hugeEnumCollection.Add(HugeEnum.b);

            EnumSet<HugeEnum> copyOfHugeEnumCollection = EnumSet<HugeEnum>.CopyOf<HugeEnum>(hugeEnumCollection);
            Assertion.AssertEquals(1, copyOfHugeEnumCollection.Count);
            Assertion.Assert(copyOfHugeEnumCollection.Contains(HugeEnum.b));

            //hugeEnumCollection.Add(null);
//            Assertion.AssertEquals(2, hugeEnumCollection.Count);

            /*try
            {
                copyOfHugeEnumCollection = EnumSet<HugeEnum>.CopyOf<HugeEnum>(hugeEnumCollection);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException)
            {
                // expected
            }*/
            /*
            rawEnumCollection = new ArrayList();
            rawEnumCollection.Add(HugeEnum.a);
            rawEnumCollection.Add(HugeEnumWithInnerClass.a);
            try
            {
                EnumSet.CopyOf(rawEnumCollection);
                Assertion.Fail("Should throw Exception"); 
            }
            catch (Exception e)
            {
                // expected
            }*/
        }

        [Test]
        public void TestCopyOf_LEnumSet()
        {
            EnumSet<EnumWithInnerClass> enumSet = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            enumSet.Add(EnumWithInnerClass.a);
            enumSet.Add(EnumWithInnerClass.f);
            EnumSet<EnumWithInnerClass> copyOfE = EnumSet<EnumWithInnerClass>.CopyOf(enumSet);
            Assertion.AssertEquals("Size of enumSet and copyOfE should be equal", 
                    enumSet.Count, copyOfE.Count);

            Assertion.Assert("EnumWithSubclass.a should be contained in copyOfE", 
                    copyOfE.Contains(EnumWithInnerClass.a));
            Assertion.Assert("EnumWithSubclass.f should be contained in copyOfE", 
                    copyOfE.Contains(EnumWithInnerClass.f));

            EnumWithInnerClass[] enumValue = copyOfE.ToArray();
            Assertion.AssertEquals("enumValue[0] should be identical with EnumWithSubclass.a", 
                    enumValue[0], EnumWithInnerClass.a);
            Assertion.AssertEquals("enumValue[1] should be identical with EnumWithSubclass.f", 
                    enumValue[1], EnumWithInnerClass.f);

            /*try
            {
                EnumSet.CopyOf((EnumSet)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }*/

            // test enum type with more than 64 elements
            EnumSet<HugeEnumWithInnerClass> hugeEnumSet = EnumSet<HugeEnumWithInnerClass>
                .NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            hugeEnumSet.Add(HugeEnumWithInnerClass.a);
            hugeEnumSet.Add(HugeEnumWithInnerClass.f);
            EnumSet<HugeEnumWithInnerClass> copyOfHugeEnum = EnumSet<HugeEnumWithInnerClass>.CopyOf<HugeEnumWithInnerClass>(hugeEnumSet);
            Assertion.AssertEquals(enumSet.Count, copyOfE.Count);

            Assertion.Assert(copyOfHugeEnum.Contains(HugeEnumWithInnerClass.a));
            Assertion.Assert(copyOfHugeEnum.Contains(HugeEnumWithInnerClass.f));

            HugeEnumWithInnerClass[] hugeEnumValue = copyOfHugeEnum.ToArray();
            Assertion.AssertEquals(hugeEnumValue[0], HugeEnumWithInnerClass.a);
            Assertion.AssertEquals(hugeEnumValue[1], HugeEnumWithInnerClass.f);
        }

        [Test]
        public void TestremoveAll_LCollection()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            try
            {
                set.RemoveAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            set = EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo));
            Assertion.AssertEquals("Size of set should be 64:", 64, set.Count); 

            try
            {
                set.RemoveAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            ICollection<EnumFoo> collection = new List<EnumFoo>();
            collection.Add(EnumFoo.a);

            bool result = set.RemoveAll(collection);
            Assertion.Assert("Should return true", result); 
            Assertion.AssertEquals("Size of set should be 63", 63, set.Count); 

            /*collection = new ArrayList();
            result = set.RemoveAll(collection);
            Assertion.Assert("Should return false", !result); 
            */
            ISet<EmptyEnum> emptySet = EnumSet<EmptyEnum>.NoneOf<EmptyEnum>(typeof(EmptyEnum));
            //result = set.RemoveAll(emptySet);
            //Assertion.Assert("Should return false", !result); 

            EnumSet<EnumFoo> emptyFooSet = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            result = set.RemoveAll(emptyFooSet);
            Assertion.Assert("Should return false", !result); 

            emptyFooSet.Add(EnumFoo.a);
            result = set.RemoveAll(emptyFooSet);
            Assertion.Assert("Should return false", !result); 

            ISet<EnumWithInnerClass> setWithSubclass = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            //result = set.RemoveAll(setWithSubclass);
            //Assertion.Assert("Should return false", !result); 

            setWithSubclass.Add(EnumWithInnerClass.a);
            //result = set.RemoveAll(setWithSubclass);
            //Assertion.Assert("Should return false", !result); 

            ISet<EnumFoo> anotherSet = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            anotherSet.Add(EnumFoo.a);

            set = EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo));
            result = set.RemoveAll(anotherSet);
            Assertion.Assert("Should return true", result); 
            Assertion.AssertEquals("Size of set should be 63:", 63, set.Count); 

            ISet<EnumWithInnerClass> setWithInnerClass = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            setWithInnerClass.Add(EnumWithInnerClass.a);
            setWithInnerClass.Add(EnumWithInnerClass.b);

            ISet<EnumWithInnerClass> anotherSetWithInnerClass = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            anotherSetWithInnerClass.Add(EnumWithInnerClass.c);
            anotherSetWithInnerClass.Add(EnumWithInnerClass.d);
            result = anotherSetWithInnerClass.RemoveAll(setWithInnerClass);
            Assertion.Assert("Should return false", !result); 

            anotherSetWithInnerClass.Add(EnumWithInnerClass.a);
            result = anotherSetWithInnerClass.RemoveAll(setWithInnerClass);
            Assertion.Assert("Should return true", result); 
            Assertion.AssertEquals("Size of anotherSetWithInnerClass should remain 2", 
                    2, anotherSetWithInnerClass.Count);

            anotherSetWithInnerClass.Remove(EnumWithInnerClass.c);
            anotherSetWithInnerClass.Remove(EnumWithInnerClass.d);
            //result = anotherSetWithInnerClass.Remove(setWithInnerClass);
            //Assertion.Assert("Should return false", !result); 

            /*ISet rawSet = EnumSet.AllOf(typeof(EnumWithAllInnerClass));
            result = rawSet.RemoveAll(EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo)));
            Assertion.Assert("Should return false", !result); 
            */
            setWithInnerClass = EnumSet<EnumWithInnerClass>.AllOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            anotherSetWithInnerClass = EnumSet<EnumWithInnerClass>.AllOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            setWithInnerClass.Remove(EnumWithInnerClass.a);
            anotherSetWithInnerClass.Remove(EnumWithInnerClass.f);
            result = setWithInnerClass.RemoveAll(anotherSetWithInnerClass);
            Assertion.Assert("Should return true", result); 
            Assertion.AssertEquals("Size of setWithInnerClass should be 1", 1, setWithInnerClass.Count); 

            result = setWithInnerClass.Contains(EnumWithInnerClass.f);
            Assertion.Assert("Should return true", result); 

            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));

            ICollection<HugeEnum> hugeCollection = new List<HugeEnum>();
            hugeCollection.Add(HugeEnum.a);

            result = hugeSet.RemoveAll(hugeCollection);
            Assertion.Assert(result);
            Assertion.AssertEquals(64, hugeSet.Count);

            /*collection = new ArrayList();
            result = hugeSet.RemoveAll(collection);
            Assertion.Assert(!result);
            */
            ISet<HugeEnum> emptyHugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            result = hugeSet.RemoveAll(emptyHugeSet);
            Assertion.Assert(!result);

            ISet<HugeEnumWithInnerClass> hugeSetWithSubclass = EnumSet<HugeEnumWithInnerClass>
                    .NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            //result = hugeSet.RemoveAll(hugeSetWithSubclass);
            //Assertion.Assert(!result);

            hugeSetWithSubclass.Add(HugeEnumWithInnerClass.a);
            // result = hugeSet.RemoveAll(hugeSetWithSubclass);
            // Assertion.Assert(!result);

            ISet<HugeEnum> anotherHugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            anotherHugeSet.Add(HugeEnum.a);

            hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            result = hugeSet.RemoveAll(anotherHugeSet);
            Assertion.Assert(result);
            Assertion.AssertEquals(63, set.Count);

            ISet<HugeEnumWithInnerClass> hugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>
                    .NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            hugeSetWithInnerClass.Add(HugeEnumWithInnerClass.a);
            hugeSetWithInnerClass.Add(HugeEnumWithInnerClass.b);

            ISet<HugeEnumWithInnerClass> anotherHugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>
                    .NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            anotherHugeSetWithInnerClass.Add(HugeEnumWithInnerClass.c);
            anotherHugeSetWithInnerClass.Add(HugeEnumWithInnerClass.d);
            //result = anotherHugeSetWithInnerClass.RemoveAll(setWithInnerClass);
            //Assertion.Assert("Should return false", !result); 

            anotherHugeSetWithInnerClass.Add(HugeEnumWithInnerClass.a);
            result = anotherHugeSetWithInnerClass.RemoveAll(hugeSetWithInnerClass);
            Assertion.Assert(result);
            Assertion.AssertEquals(2, anotherHugeSetWithInnerClass.Count);

            anotherHugeSetWithInnerClass.Remove(HugeEnumWithInnerClass.c);
            anotherHugeSetWithInnerClass.Remove(HugeEnumWithInnerClass.d);
            //result = anotherHugeSetWithInnerClass.Remove(hugeSetWithInnerClass);
            //Assertion.Assert(!result);

            /*rawSet = EnumSet<HugeEnumWithInnerClass>.AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            result = rawSet.RemoveAll(EnumSet.AllOf(typeof(HugeEnum)));
            Assertion.Assert(!result);
            */
            hugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>.AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            anotherHugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>.AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            hugeSetWithInnerClass.Remove(HugeEnumWithInnerClass.a);
            anotherHugeSetWithInnerClass.Remove(HugeEnumWithInnerClass.f);
            result = hugeSetWithInnerClass.RemoveAll(anotherHugeSetWithInnerClass);
            Assertion.Assert(result);
            Assertion.AssertEquals(1, hugeSetWithInnerClass.Count);

            result = hugeSetWithInnerClass.Contains(HugeEnumWithInnerClass.f);
            Assertion.Assert(result);
        }

        [Test]
        public void TestretainAll_LCollection()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo));

           /* try
            {
                set.RetainAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }*/

            set.Clear();
            bool result = set.RetainAll(null);
            Assertion.Assert("Should return false", !result); 

            /*ICollection rawCollection = new ArrayList();
            result = set.RetainAll(rawCollection);
            Assertion.Assert("Should return false", !result); 

            rawCollection.Add(EnumFoo.a);
            result = set.RetainAll(rawCollection);
            Assertion.Assert("Should return false", !result); 

            rawCollection.Add(EnumWithInnerClass.a);
            result = set.RetainAll(rawCollection);
            Assertion.Assert("Should return false", !result); 
            Assertion.AssertEquals("Size of set should be 0:", 0, set.Count); 

            rawCollection.Remove(EnumFoo.a);
            result = set.RetainAll(rawCollection);
            Assertion.Assert("Should return false", !result); 
            */
            ISet<EnumFoo> anotherSet = EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo));
            result = set.RetainAll(anotherSet);
            Assertion.Assert("Should return false", !result); 
            Assertion.AssertEquals("Size of set should be 0", 0, set.Count); 

            ISet<EnumWithInnerClass> setWithInnerClass = EnumSet<EnumWithInnerClass>
                    .AllOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            //result = set.RetainAll(setWithInnerClass);
            //Assertion.Assert("Should return true", result); 
            Assertion.AssertEquals("Size of set should be 0", 0, set.Count); 

            setWithInnerClass = EnumSet<EnumWithInnerClass>.NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            //result = set.RetainAll(setWithInnerClass);
            //Assertion.Assert("Should return true", result); 

            ISet<EmptyEnum> emptySet = EnumSet<EmptyEnum>.AllOf<EmptyEnum>(typeof(EmptyEnum));
            // result = set.RetainAll(emptySet);
            //Assertion.Assert("Should return true", result); 

            ISet<EnumWithAllInnerClass> setWithAllInnerClass = EnumSet<EnumWithAllInnerClass>
                    .AllOf<EnumWithAllInnerClass>(typeof(EnumWithAllInnerClass));
            //result = set.RetainAll(setWithAllInnerClass);
            //Assertion.Assert("Should return true", result); 

            set.Add(EnumFoo.a);
            //result = set.RetainAll(setWithInnerClass);
            //Assertion.Assert("Should return true", result); 
           // Assertion.AssertEquals("Size of set should be 0", 0, set.Count); 

            setWithInnerClass = EnumSet<EnumWithInnerClass>.AllOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            setWithInnerClass.Remove(EnumWithInnerClass.f);
            ISet<EnumWithInnerClass> anotherSetWithInnerClass = EnumSet<EnumWithInnerClass>
                    .NoneOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            anotherSetWithInnerClass.Add(EnumWithInnerClass.e);
            anotherSetWithInnerClass.Add(EnumWithInnerClass.f);

            result = setWithInnerClass.RetainAll(anotherSetWithInnerClass);
            Assertion.Assert("Should return true", result); 
            result = setWithInnerClass.Contains(EnumWithInnerClass.e);
            Assertion.Assert("Should contain EnumWithInnerClass.e", result); 
            result = setWithInnerClass.Contains(EnumWithInnerClass.b);
            Assertion.Assert("Should not contain EnumWithInnerClass.b", !result); 
            Assertion.AssertEquals("Size of set should be 1:", 1, setWithInnerClass.Count); 

            anotherSetWithInnerClass = EnumSet<EnumWithInnerClass>.AllOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            result = setWithInnerClass.RetainAll(anotherSetWithInnerClass);

            Assertion.Assert("Return value should be false", !result); 

            /*rawCollection = new ArrayList();
            rawCollection.Add(EnumWithInnerClass.e);
            rawCollection.Add(EnumWithInnerClass.f);
            result = setWithInnerClass.RetainAll(rawCollection);
            Assertion.Assert("Should return false", !result); 
            */
            set = EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo));
            set.Remove(EnumFoo.a);
            anotherSet = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            anotherSet.Add(EnumFoo.a);
            result = set.RetainAll(anotherSet);
            Assertion.Assert("Should return true", result); 
            Assertion.AssertEquals("size should be 0", 0, set.Count); 

            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));

            /*try
            {
                hugeSet.RetainAll(null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }*/

            hugeSet.Clear();
            result = hugeSet.RetainAll(null);
            Assertion.Assert(!result);
            /*
            rawCollection = new ArrayList();
            result = hugeSet.RetainAll(rawCollection);
            Assertion.Assert(!result);

            rawCollection.Add(HugeEnum.a);
            result = hugeSet.RetainAll(rawCollection);
            Assertion.Assert(!result);

            rawCollection.Add(HugeEnumWithInnerClass.a);
            result = hugeSet.RetainAll(rawCollection);
            Assertion.Assert(!result);
            Assertion.AssertEquals(0, set.Count);

            rawCollection.Remove(HugeEnum.a);
            result = set.RetainAll(rawCollection);
            Assertion.Assert(!result);
            */
            ISet<HugeEnum> anotherHugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            result = hugeSet.RetainAll(anotherHugeSet);
            Assertion.Assert(!result);
            Assertion.AssertEquals(0, hugeSet.Count);

            ISet<HugeEnumWithInnerClass> hugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>
                    .AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            //result = hugeSet.RetainAll(hugeSetWithInnerClass);
            //Assertion.Assert(result);
            Assertion.AssertEquals(0, hugeSet.Count);

            hugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>.NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            //result = hugeSet.RetainAll(hugeSetWithInnerClass);
            //Assertion.Assert(result);

            ISet<HugeEnumWithInnerClass> hugeSetWithAllInnerClass = EnumSet<HugeEnumWithInnerClass>
                    .AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            //result = hugeSet.RetainAll(hugeSetWithAllInnerClass);
            //Assertion.Assert(result);

            hugeSet.Add(HugeEnum.a);
            // result = hugeSet.RetainAll(hugeSetWithInnerClass);
            //Assertion.Assert(result);
           // Assertion.AssertEquals(0, hugeSet.Count);

            hugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>.AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            hugeSetWithInnerClass.Remove(HugeEnumWithInnerClass.f);
            ISet<HugeEnumWithInnerClass> anotherHugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>
                    .NoneOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            anotherHugeSetWithInnerClass.Add(HugeEnumWithInnerClass.e);
            anotherHugeSetWithInnerClass.Add(HugeEnumWithInnerClass.f);

            result = hugeSetWithInnerClass.RetainAll(anotherHugeSetWithInnerClass);
            Assertion.Assert(result);
            result = hugeSetWithInnerClass.Contains(HugeEnumWithInnerClass.e);
            Assertion.Assert("Should contain HugeEnumWithInnerClass.e", result); 
            result = hugeSetWithInnerClass.Contains(HugeEnumWithInnerClass.b);
            Assertion.Assert("Should not contain HugeEnumWithInnerClass.b", !result); 
            Assertion.AssertEquals("Size of hugeSet should be 1:", 1, hugeSetWithInnerClass.Count); 

            anotherHugeSetWithInnerClass = EnumSet<HugeEnumWithInnerClass>.AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            result = hugeSetWithInnerClass.RetainAll(anotherHugeSetWithInnerClass);

            Assertion.Assert("Return value should be false", !result); 
            /*
            rawCollection = new ArrayList();
            rawCollection.Add(HugeEnumWithInnerClass.e);
            rawCollection.Add(HugeEnumWithInnerClass.f);
            result = hugeSetWithInnerClass.RetainAll(rawCollection);
            Assertion.Assert(!result);
            */
            hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            hugeSet.Remove(HugeEnum.a);
            anotherHugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            anotherHugeSet.Add(HugeEnum.a);
            result = hugeSet.RetainAll(anotherHugeSet);
            Assertion.Assert(result);
            Assertion.AssertEquals(0, hugeSet.Count);
        }
        /*
        [Test]
        public void Testiterator()
        {
            ISet<EnumFoo> set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            set.Add(EnumFoo.a);
            set.Add(EnumFoo.b);

            IEnumerator<EnumFoo> iterator = set.GetEnumerator();
            IEnumerator<EnumFoo> anotherIterator = set.GetEnumerator();
            Assertion.Assert("Should not be same", !iterator.Equals(anotherIterator)); 
            /*try
            {
                iterator.Remove();
                Assertion.Fail("Should throw IllegalStateException"); 
            }
            catch (Exception e)
            {
                // expectedd
            }
            *
            Assertion.Assert("Should has next element:", iterator.HasNext()); 
            Assertion.AssertSame("Should be identical", EnumFoo.a, iterator.Next()); 
            iterator.Remove();
            Assertion.Assert("Should has next element:", iterator.HasNext()); 
            Assertion.AssertSame("Should be identical", EnumFoo.b, iterator.Next()); 
            Assertion.Assert("Should not has next element:", !iterator.HasNext()); 
            Assertion.Assert("Should not has next element:", !iterator.HasNext()); 

            Assertion.AssertEquals("Size should be 1:", 1, set.Count); 

            try
            {
                iterator.Next();
                Assertion.Fail("Should throw NoSuchElementException"); 
            }
            catch (Exception e)
            {
                // expected
            }
            set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            set.Add(EnumFoo.a);
            iterator = set.Iterator();
            Assertion.AssertEquals("Should be equal", EnumFoo.a, iterator.Next()); 
            iterator.Remove();
            try
            {
                iterator.Remove();
                Assertion.Fail("Should throw IllegalStateException"); 
            }
            catch (Exception e)
            {
                // expected
            }

            ISet<EmptyEnum> emptySet = EnumSet<EmptyEnum>.AllOf<EmptyEnum>(typeof(EmptyEnum));
            IEnumerator<EmptyEnum> emptyIterator = emptySet.GetEnumerator();
            try
            {
                emptyIterator.Next();
                Assertion.Fail("Should throw NoSuchElementException"); 
            }
            catch (Exception e)
            {
                // expected
            }

            ISet<EnumWithInnerClass> setWithSubclass = EnumSet<EnumWithInnerClass>
                    .AllOf<EnumWithInnerClass>(typeof(EnumWithInnerClass));
            setWithSubclass.Remove(EnumWithInnerClass.e);
            IEnumerator<EnumWithInnerClass> iteratorWithSubclass = setWithSubclass
                    .GetEnumerator();
            Assertion.AssertSame("Should be same", EnumWithInnerClass.a, iteratorWithSubclass.Next()); 

            Assertion.Assert("Should return true", iteratorWithSubclass.HasNext()); 
            Assertion.AssertSame("Should be same", EnumWithInnerClass.b, iteratorWithSubclass.Next()); 

            setWithSubclass.Remove(EnumWithInnerClass.c);
            Assertion.Assert("Should return true", iteratorWithSubclass.HasNext()); 
            Assertion.AssertSame("Should be same", EnumWithInnerClass.c, iteratorWithSubclass.Next()); 

            Assertion.Assert("Should return true", iteratorWithSubclass.HasNext()); 
            Assertion.AssertSame("Should be same", EnumWithInnerClass.d, iteratorWithSubclass.Next()); 

            setWithSubclass.Add(EnumWithInnerClass.e);
            Assertion.Assert("Should return true", iteratorWithSubclass.HasNext()); 
            Assertion.AssertSame("Should be same", EnumWithInnerClass.f, iteratorWithSubclass.Next()); 

            set = EnumSet<EnumFoo>.NoneOf<EnumFoo>(typeof(EnumFoo));
            iterator = set.GetEnumerator();
            try
            {
                iterator.Next();
                Assertion.Fail("Should throw NoSuchElementException"); 
            }
            catch (Exception e)
            {
                // expected
            }

            set.Add(EnumFoo.a);
            iterator = set.GetEnumerator();
            Assertion.AssertEquals("Should return EnumFoo.a", EnumFoo.a, iterator.Next()); 
            Assertion.AssertEquals("Size of set should be 1", 1, set.Count); 
            iterator.Remove();
            Assertion.AssertEquals("Size of set should be 0", 0, set.Count); 
            Assertion.Assert("Should return false", !set.Contains(EnumFoo.a)); 

            set.Add(EnumFoo.a);
            set.Add(EnumFoo.b);
            iterator = set.GetEnumerator();
            Assertion.AssertEquals("Should be equals", EnumFoo.a, iterator.Next()); 
            iterator.Remove();
            try
            {
                iterator.Remove();
                Assertion.Fail("Should throw IllegalStateException"); 
            }
            catch (Exception e)
            {
                // expected
            }

            Assertion.Assert("Should have next element", iterator.HasNext()); 
            try
            {
                iterator.Remove();
                Assertion.Fail("Should throw IllegalStateException"); 
            }
            catch (Exception e)
            {
                // expected
            }
            Assertion.AssertEquals("Size of set should be 1", 1, set.Count); 
            Assertion.Assert("Should have next element", iterator.HasNext()); 
            Assertion.AssertEquals("Should return EnumFoo.b", EnumFoo.b, iterator.Next()); 
            set.Remove(EnumFoo.b);
            Assertion.AssertEquals("Size of set should be 0", 0, set.Count); 
            iterator.Remove();
            Assertion.Assert("Should return false", !set.Contains(EnumFoo.a)); 

            // RI's bug, EnumFoo.b should not exist at the moment.
            Assertion.Assert("Should return false", !set.Contains(EnumFoo.b)); 

            // test enum type with more than 64 elements
            ISet<HugeEnum> hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            hugeSet.Add(HugeEnum.a);
            hugeSet.Add(HugeEnum.b);

            IEnumerator<HugeEnum> hIterator = hugeSet.GetEnumerator();
            IEnumerator<HugeEnum> anotherHugeIterator = hugeSet.GetEnumerator();
            Assertion.AssertNotSame(hIterator, anotherHugeIterator);
            try
            {
                hIterator.Remove();
                Assertion.Fail("Should throw IllegalStateException"); 
            }
            catch (Exception e)
            {
                // expectedd
            }

            Assertion.Assert(hIterator.HasNext());
            Assertion.AssertSame(HugeEnum.a, hIterator.Next());
            hIterator.Remove();
            Assertion.Assert(hIterator.HasNext());
            Assertion.AssertSame(HugeEnum.b, hIterator.Next());
            Assertion.Assert(!hIterator.HasNext());
            Assertion.Assert(!hIterator.HasNext());

            Assertion.AssertEquals(1, hugeSet.Count);

            try
            {
                hIterator.Next();
                Assertion.Fail("Should throw NoSuchElementException"); 
            }
            catch (Exception e)
            {
                // expected
            }

            ISet<HugeEnumWithInnerClass> hugeSetWithSubclass = EnumSet<HugeEnumWithInnerClass>
                    .AllOf<HugeEnumWithInnerClass>(typeof(HugeEnumWithInnerClass));
            hugeSetWithSubclass.Remove(HugeEnumWithInnerClass.e);
            IEnumerator<HugeEnumWithInnerClass> hugeIteratorWithSubclass = hugeSetWithSubclass
                    .GetEnumerator();
            Assertion.AssertSame(HugeEnumWithInnerClass.a, hugeIteratorWithSubclass.Next());

            Assertion.Assert(hugeIteratorWithSubclass.HasNext());
            Assertion.AssertSame(HugeEnumWithInnerClass.b, hugeIteratorWithSubclass.Next());

           // setWithSubclass.Remove(HugeEnumWithInnerClass.c);
            Assertion.Assert(hugeIteratorWithSubclass.HasNext());
            Assertion.AssertSame(HugeEnumWithInnerClass.c, hugeIteratorWithSubclass.Next());

            Assertion.Assert(hugeIteratorWithSubclass.HasNext());
            Assertion.AssertSame(HugeEnumWithInnerClass.d, hugeIteratorWithSubclass.Next());

            hugeSetWithSubclass.Add(HugeEnumWithInnerClass.e);
            Assertion.Assert(hugeIteratorWithSubclass.HasNext());
            Assertion.AssertSame(HugeEnumWithInnerClass.f, hugeIteratorWithSubclass.Next());

            hugeSet = EnumSet<HugeEnum>.NoneOf<HugeEnum>(typeof(HugeEnum));
            hIterator = hugeSet.GetEnumerator();
            try
            {
                hIterator.Next();
                Assertion.Fail("Should throw NoSuchElementException"); 
            }
            catch (Exception e)
            {
                // expected
            }

            hugeSet.Add(HugeEnum.a);
            hIterator = hugeSet.GetEnumerator();
            Assertion.AssertEquals(HugeEnum.a, hIterator.Next());
            Assertion.AssertEquals(1, hugeSet.Count);
            hIterator.Remove();
            Assertion.AssertEquals(0, hugeSet.Count);
            Assertion.Assert(!hugeSet.Contains(HugeEnum.a));

            hugeSet.Add(HugeEnum.a);
            hugeSet.Add(HugeEnum.b);
            hIterator = hugeSet.GetEnumerator();
            hIterator.Next();
            hIterator.Remove();

            Assertion.Assert(hIterator.HasNext());
            try
            {
                hIterator.Remove();
                Assertion.Fail("Should throw IllegalStateException"); 
            }
            catch (Exception e)
            {
                // expected
            }
            Assertion.AssertEquals(1, hugeSet.Count);
            Assertion.Assert(hIterator.HasNext());
            Assertion.AssertEquals(HugeEnum.b, hIterator.Next());
            hugeSet.Remove(HugeEnum.b);
            Assertion.AssertEquals(0, hugeSet.Count);
            hIterator.Remove();
            Assertion.Assert(!hugeSet.Contains(HugeEnum.a));
            // RI's bug, EnumFoo.b should not exist at the moment.
            Assertion.Assert("Should return false", !set.Contains(EnumFoo.b)); 

            // Regression for HARMONY-4728
            hugeSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            hIterator = hugeSet.GetEnumerator();
            for (int i = 0; i < 63; i++)
            {
                hIterator.Next();
            }
            Assertion.AssertSame(HugeEnum.ll, hIterator.Next());
        }*/

        [Test]
        public void TestOf_E()
        {
            EnumSet<EnumWithInnerClass> enumSet = EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a);
            Assertion.AssertEquals("enumSet should have length 1:", 1, enumSet.Count); 

            Assertion.Assert("enumSet should contain EnumWithSubclass.a:", 
                    enumSet.Contains(EnumWithInnerClass.a));

            /* try
             {
                 EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>((EnumWithInnerClass)null);
                 Assertion.Fail("Should throw NullReferenceException"); 
             }
             catch (NullReferenceException npe)
             {
                 // expected
             }*/

            // test enum type with more than 64 elements
            EnumSet<HugeEnumWithInnerClass> hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a);
            Assertion.AssertEquals(1, hugeEnumSet.Count);

            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.a));
        }

        [Test]
        public void TestOf_EE()
        {
            EnumSet<EnumWithInnerClass> enumSet = EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a,
                    EnumWithInnerClass.b);
            Assertion.AssertEquals("enumSet should have length 2:", 2, enumSet.Count); 

            Assertion.Assert("enumSet should contain EnumWithSubclass.a:", 
                    enumSet.Contains(EnumWithInnerClass.a));
            Assertion.Assert("enumSet should contain EnumWithSubclass.b:", 
                    enumSet.Contains(EnumWithInnerClass.b));

            /*try
            {
                EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>((EnumWithInnerClass)null, EnumWithInnerClass.a);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }

            try
            {
                EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a, (EnumWithInnerClass)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }

            try
            {
                EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>((EnumWithInnerClass)null, (EnumWithInnerClass)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }*/

            enumSet = EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a, EnumWithInnerClass.a);
            Assertion.AssertEquals("Size of enumSet should be 1", 
                    1, enumSet.Count);

            // test enum type with more than 64 elements
            EnumSet<HugeEnumWithInnerClass> hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a,
                    HugeEnumWithInnerClass.b);
            Assertion.AssertEquals(2, hugeEnumSet.Count);

            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.a));
            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.b));

            /*try
            {
                EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>((HugeEnumWithInnerClass)null, HugeEnumWithInnerClass.a);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }

            try
            {
                EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a, (HugeEnumWithInnerClass)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }

            try
            {
                EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>((HugeEnumWithInnerClass)null, (HugeEnumWithInnerClass)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }*/

            hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a, HugeEnumWithInnerClass.a);
            Assertion.AssertEquals(1, hugeEnumSet.Count);
        }

        [Test]
        public void TestOf_EEE()
        {
            EnumSet<EnumWithInnerClass> enumSet = EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a,
                    EnumWithInnerClass.b, EnumWithInnerClass.c);
            Assertion.AssertEquals("Size of enumSet should be 3:", 3, enumSet.Count); 

            Assertion.Assert(
                    "enumSet should contain EnumWithSubclass.a:", enumSet.Contains(EnumWithInnerClass.a)); 
            Assertion.Assert("Should return true", enumSet.Contains(EnumWithInnerClass.c)); 

            /* try
             {
                 EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>((EnumWithInnerClass)null, null, null);
                 Assertion.Fail("Should throw NullReferenceException"); 
             }
             catch (NullReferenceException npe)
             {
                 // expected
             }*/

            enumSet = EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a, EnumWithInnerClass.b,
                    EnumWithInnerClass.b);
            Assertion.AssertEquals("enumSet should contain 2 elements:", 2, enumSet.Count); 

            // test enum type with more than 64 elements
            EnumSet<HugeEnumWithInnerClass> hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a,
                    HugeEnumWithInnerClass.b, HugeEnumWithInnerClass.c);
            Assertion.AssertEquals(3, hugeEnumSet.Count);

            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.a));
            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.c));

            /*try
            {
                EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>((HugeEnumWithInnerClass)null, null, null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }*/

            hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a, HugeEnumWithInnerClass.b,
                    HugeEnumWithInnerClass.b);
            Assertion.AssertEquals(2, hugeEnumSet.Count);
        }

        [Test]
        public void TestOf_EEEE()
        {
            EnumSet<EnumWithInnerClass> enumSet = EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a,
                    EnumWithInnerClass.b, EnumWithInnerClass.c,
                    EnumWithInnerClass.d);
            Assertion.AssertEquals("Size of enumSet should be 4", 4, enumSet.Count); 

            Assertion.Assert(
                    "enumSet should contain EnumWithSubclass.a:", enumSet.Contains(EnumWithInnerClass.a)); 
            Assertion.Assert("enumSet should contain EnumWithSubclass.d:", enumSet 
                    .Contains(EnumWithInnerClass.d));

            /*try
            {
                EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>((EnumWithInnerClass)null, null, null, null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }*/

            // test enum type with more than 64 elements
            EnumSet<HugeEnumWithInnerClass> hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a,
                    HugeEnumWithInnerClass.b, HugeEnumWithInnerClass.c,
                    HugeEnumWithInnerClass.d);
            Assertion.AssertEquals(4, hugeEnumSet.Count);

            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.a));
            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.d));

            /* try
             {
                 EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>((HugeEnumWithInnerClass)null, null, null, null);
                 Assertion.Fail("Should throw NullReferenceException"); 
             }
             catch (NullReferenceException npe)
             {
                 // expected
             }*/
        }

        [Test]
        public void TestOf_EEEEE()
        {
            EnumSet<EnumWithInnerClass> enumSet = EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a,
                    EnumWithInnerClass.b, EnumWithInnerClass.c,
                    EnumWithInnerClass.d, EnumWithInnerClass.e);
            Assertion.AssertEquals("Size of enumSet should be 5:", 5, enumSet.Count); 

            Assertion.Assert("Should return true", enumSet.Contains(EnumWithInnerClass.a)); 
            Assertion.Assert("Should return true", enumSet.Contains(EnumWithInnerClass.e)); 

            /*try
            {
                EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>((EnumWithInnerClass)null, null, null, null, null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }*/

            // test enum with more than 64 elements
            EnumSet<HugeEnumWithInnerClass> hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a,
                    HugeEnumWithInnerClass.b, HugeEnumWithInnerClass.c,
                    HugeEnumWithInnerClass.d, HugeEnumWithInnerClass.e);
            Assertion.AssertEquals(5, hugeEnumSet.Count);

            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.a));
            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.e));

            /*try
            {
                EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>((HugeEnumWithInnerClass)null, null, null, null, null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }*/
        }

        [Test]
        public void TestOf_EEArray()
        {
            EnumWithInnerClass[] enumArray = new EnumWithInnerClass[] {
                EnumWithInnerClass.b, EnumWithInnerClass.c };
            EnumSet<EnumWithInnerClass> enumSet = EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a,
                    enumArray);
            Assertion.AssertEquals("Should be equal", 3, enumSet.Count); 

            Assertion.Assert("Should return true", enumSet.Contains(EnumWithInnerClass.a)); 
            Assertion.Assert("Should return true", enumSet.Contains(EnumWithInnerClass.c)); 

            try
            {
                EnumSet<EnumWithInnerClass>.Of<EnumWithInnerClass>(EnumWithInnerClass.a, (EnumWithInnerClass[])null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException npe)
            {
                // expected
            }

            EnumFoo[] foos = { EnumFoo.a, EnumFoo.c, EnumFoo.d };
            EnumSet<EnumFoo> set = EnumSet<EnumFoo>.Of<EnumFoo>(EnumFoo.c, foos);
            Assertion.AssertEquals("size of set should be 1", 3, set.Count); 
            Assertion.Assert("Should contain EnumFoo.a", set.Contains(EnumFoo.a)); 
            Assertion.Assert("Should contain EnumFoo.c", set.Contains(EnumFoo.c)); 
            Assertion.Assert("Should contain EnumFoo.d", set.Contains(EnumFoo.d)); 

            // test enum type with more than 64 elements
            HugeEnumWithInnerClass[] hugeEnumArray = new HugeEnumWithInnerClass[] {
                HugeEnumWithInnerClass.b, HugeEnumWithInnerClass.c };
            EnumSet<HugeEnumWithInnerClass> hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a,
                    hugeEnumArray);
            Assertion.AssertEquals(3, hugeEnumSet.Count);

            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.a));
            Assertion.Assert(hugeEnumSet.Contains(HugeEnumWithInnerClass.c));

            try
            {
                EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.a, (HugeEnumWithInnerClass[])null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException)
            {
                // expected
            }

            HugeEnumWithInnerClass[] huges = { HugeEnumWithInnerClass.a, HugeEnumWithInnerClass.c, HugeEnumWithInnerClass.d };
            EnumSet<HugeEnumWithInnerClass> hugeSet = EnumSet<HugeEnumWithInnerClass>.Of<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.c, huges);
            Assertion.AssertEquals(3, hugeSet.Count);
            Assertion.Assert(hugeSet.Contains(HugeEnumWithInnerClass.a));
            Assertion.Assert(hugeSet.Contains(HugeEnumWithInnerClass.c));
            Assertion.Assert(hugeSet.Contains(HugeEnumWithInnerClass.d));
        }

        [Test]
        public void TestRange_EE()
        {
            /*try
            {
                EnumSet<EnumWithInnerClass>.Range<EnumWithInnerClass>(EnumWithInnerClass.c, null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            try
            {
                EnumSet<EnumWithInnerClass>.Range<EnumWithInnerClass>(null, EnumWithInnerClass.c);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            try
            {
                EnumSet<EnumWithInnerClass>.Range<EnumWithInnerClass>(null, (EnumWithInnerClass)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            try
            {
                EnumSet<EnumWithInnerClass>.Range<EnumWithInnerClass>(EnumWithInnerClass.b, EnumWithInnerClass.a);
                Assertion.Fail("Should throw IllegalArgumentException"); 
            }
            catch (Exception e)
            {
                // expected
            }*/

            EnumSet<EnumWithInnerClass> enumSet = EnumSet<EnumWithInnerClass>.Range<EnumWithInnerClass>(
                    EnumWithInnerClass.a, EnumWithInnerClass.a);
            Assertion.AssertEquals("Size of enumSet should be 1", 1, enumSet.Count); 

            enumSet = EnumSet<EnumWithInnerClass>.Range<EnumWithInnerClass>(
                    EnumWithInnerClass.a, EnumWithInnerClass.c);
            Assertion.AssertEquals("Size of enumSet should be 3", 3, enumSet.Count); 

            // test enum with more than 64 elements
            /*try
            {
                EnumSet<HugeEnumWithInnerClass>.Range<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.c, null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            try
            {
                EnumSet<HugeEnumWithInnerClass>.Range<HugeEnumWithInnerClass>(null, HugeEnumWithInnerClass.c);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }

            try
            {
                EnumSet<HugeEnumWithInnerClass>.Range<HugeEnumWithInnerClass>(null, (HugeEnumWithInnerClass)null);
                Assertion.Fail("Should throw NullReferenceException"); 
            }
            catch (NullReferenceException e)
            {
                // expected
            }*/

            try
            {
                EnumSet<HugeEnumWithInnerClass>.Range<HugeEnumWithInnerClass>(HugeEnumWithInnerClass.b, HugeEnumWithInnerClass.a);
                Assertion.Fail("Should throw IllegalArgumentException"); 
            }
            catch (Exception e)
            {
                // expected
            }

            EnumSet<HugeEnumWithInnerClass> hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Range<HugeEnumWithInnerClass>(
                    HugeEnumWithInnerClass.a, HugeEnumWithInnerClass.a);
            Assertion.AssertEquals(1, hugeEnumSet.Count);

            hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Range<HugeEnumWithInnerClass>(
                    HugeEnumWithInnerClass.c, HugeEnumWithInnerClass.aa);
            Assertion.AssertEquals(51, hugeEnumSet.Count);

            hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Range<HugeEnumWithInnerClass>(
                    HugeEnumWithInnerClass.a, HugeEnumWithInnerClass.mm);
            Assertion.AssertEquals(65, hugeEnumSet.Count);

            hugeEnumSet = EnumSet<HugeEnumWithInnerClass>.Range<HugeEnumWithInnerClass>(
                    HugeEnumWithInnerClass.b, HugeEnumWithInnerClass.mm);
            Assertion.AssertEquals(64, hugeEnumSet.Count);
        }

        [Test]
        public void TestClone()
        {
            EnumSet<EnumFoo> enumSet = EnumSet<EnumFoo>.AllOf<EnumFoo>(typeof(EnumFoo));
            EnumSet<EnumFoo> clonedEnumSet = enumSet.Clone();
            Assertion.AssertEquals(enumSet, clonedEnumSet);
            Assert.AreNotSame(enumSet,clonedEnumSet);
            Assertion.Assert(clonedEnumSet.Contains(EnumFoo.a));
            Assertion.Assert(clonedEnumSet.Contains(EnumFoo.b));
            Assertion.AssertEquals(64, clonedEnumSet.Count);

            // test enum type with more than 64 elements
            EnumSet<HugeEnum> hugeEnumSet = EnumSet<HugeEnum>.AllOf<HugeEnum>(typeof(HugeEnum));
            EnumSet<HugeEnum> hugeClonedEnumSet = hugeEnumSet.Clone();
            Assertion.AssertEquals(hugeEnumSet, hugeClonedEnumSet);
            Assert.AreNotSame(hugeEnumSet,hugeClonedEnumSet);
            Assertion.Assert(hugeClonedEnumSet.Contains(HugeEnum.a));
            Assertion.Assert(hugeClonedEnumSet.Contains(HugeEnum.b));
            Assertion.AssertEquals(65, hugeClonedEnumSet.Count);

            hugeClonedEnumSet.Remove(HugeEnum.a);
            Assertion.AssertEquals(64, hugeClonedEnumSet.Count);
            Assertion.Assert(!hugeClonedEnumSet.Contains(HugeEnum.a));
            Assertion.AssertEquals(65, hugeEnumSet.Count);
            Assertion.Assert(hugeEnumSet.Contains(HugeEnum.a));
        }

        /*
    [Test]
    public void Testserialization()  {
        EnumSet<EnumFoo> set = EnumSet.AllOf(typeof(EnumFoo));
        SerializationTest.VerifySelf(set);
    }
    
    [Test]
    public void testSerializationCompatibility()  {
        EnumSet<EnumFoo> set = EnumSet.AllOf(typeof(EnumFoo));
        SerializationTest.VerifyGolden(this, set);
    }*/
    }
}
