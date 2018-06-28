using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Collections.Generics;

namespace Tests
{
    [TestFixture]
    public class EnumMapTests
    {

        enum Size
        {
            Small, Middle, Big
        }

        enum Color
        {
            Red, Green, Blue
        }

        enum Empty
        {
            //Empty
        }

        /*private class MockEntry<K, V> : KeyValuePair<K, V> {
            private K key;

            private V value;

            public MockEntry(K key, V value) {
                this.key = key;
                this.value = value;
            }

            public int hashCode() {
                return (key == null ? 0 : key.hashCode())
                        ^ (value == null ? 0 : value.hashCode());
            }

            public K getKey() {
                return key;
            }

            public V getValue() {
                return value;
            }

            public V setValue(V obj) {
                V oldValue = value;
                value = obj;
                return oldValue;
            }
        }*/

        [Test]
        public void ConstructorLjava_lang_Class()
        {

            EnumMap<Color, Double> enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            Assertion.AssertEquals("Return ZERO for non mapped key", enumColorMap.Put( //$NON-NLS-1$
                    Color.Green, 2), 0);
            Assertion.AssertEquals("Get returned incorrect value for given key", 2, //$NON-NLS-1$
                    enumColorMap.Get(Color.Green));


            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            Assertion.AssertEquals("Return ZERO for non mapped key", enumSizeMap.Put( //$NON-NLS-1$
                    Size.Big, 2), 0);
            Assertion.AssertEquals("Get returned incorrect value for given key", 2, //$NON-NLS-1$
                    enumSizeMap.Get(Size.Big));


            enumSizeMap = new EnumMap<Size, Int32>(Size.Middle.GetType());
            Assertion.AssertEquals("Return ZERO for non mapped key", enumSizeMap.Put( //$NON-NLS-1$
                    Size.Small, 1), 0);
            Assertion.AssertEquals("Get returned incorrect value for given key", 1, //$NON-NLS-1$
                    enumSizeMap.Get(Size.Small));
        }

        [Test]
        public void ConstructorLjava_util_EnumMap()
        {
            EnumMap<Color, Double> enumColorMap = null;

            enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            Double double1 = 1;
            enumColorMap.Put(Color.Green, 2);
            enumColorMap.Put(Color.Blue, double1);

            EnumMap<Color, Double> enumMap = new EnumMap<Color, Double>(enumColorMap);
            Assertion.AssertEquals("Constructor fails", 2, enumMap.Get(Color.Green)); //$NON-NLS-1$
            Assertion.AssertEquals("Constructor fails", double1, enumMap.Get(Color.Blue)); //$NON-NLS-1$
            Assertion.AssertEquals("Constructor fails", enumMap.Get(Color.Red), default(Double)); //$NON-NLS-1$
            enumMap.Put(Color.Red, 1);
            Assertion.AssertEquals("Wrong value", 1, enumMap.Get(Color.Red)); //$NON-NLS-1$

        }

        [Test]
        public void test_ConstructorLjava_util_Map()
        {
            EnumMap<Color, Double> enumColorMap = null;

            enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            EnumMap<Color, Double> enumMap = new EnumMap<Color, Double>(enumColorMap);
            enumColorMap.Put(Color.Blue, 3);
            enumMap = new EnumMap<Color, Double>(enumColorMap);

            IDictionary<Color, Double> hashColorMap = new Dictionary<Color, Double>();
            hashColorMap[Color.Green] = 2;
            enumMap = new EnumMap<Color, Double>(hashColorMap);
            Assertion.AssertEquals("Constructor fails", 2, enumMap.Get(Color.Green)); //$NON-NLS-1$
            Assertion.AssertEquals("Constructor fails", enumMap.Get(Color.Red), default(Double)); //$NON-NLS-1$
            enumMap.Put(Color.Red, 1);
            Assertion.AssertEquals("Wrong value", 1, enumMap.Get(Color.Red)); //$NON-NLS-1$          
        }

        [Test]
        public void clear()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Small, 1);
            enumSizeMap.Clear();
            Assertion.AssertEquals("Failed to clear all elements", enumSizeMap.Get(Size.Small), default(Int32)); //$NON-NLS-1$
        }


        [Test]
        public void containsKeyLjava_lang_Object()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            Assertion.Assert("Returned true for uncontained key", !enumSizeMap //$NON-NLS-1$
                    .ContainsKey(Size.Small));
            enumSizeMap.Put(Size.Small, 1);
            Assertion.Assert("Returned false for contained key", enumSizeMap //$NON-NLS-1$
                    .ContainsKey(Size.Small));
            /*
            enumSizeMap.Put(Size.Big, null);
            Assertion.Assert("Returned false for contained key", enumSizeMap //$NON-NLS-1$
                    .ContainsKey(Size.Big));*/

        }
        [Test]
        public void clone()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            Int32 integer = Int32.Parse("3"); //$NON-NLS-1$
            enumSizeMap.Put(Size.Small, integer);
            EnumMap<Size, Int32> enumSizeMapClone = (EnumMap<Size, Int32>)enumSizeMap.Clone();
            Assertion.AssertEquals("Should not be same", enumSizeMap, enumSizeMapClone); //$NON-NLS-1$
            Assertion.AssertEquals("Clone answered unequal EnumMap", enumSizeMap, //$NON-NLS-1$
                    enumSizeMapClone);

            Assertion.AssertEquals("Should be same", enumSizeMap.Get(Size.Small), //$NON-NLS-1$
                    enumSizeMapClone.Get(Size.Small));
            Assertion.AssertEquals("Clone is not shallow clone", integer, enumSizeMapClone //$NON-NLS-1$
                    .Get(Size.Small));
            enumSizeMap.Remove(Size.Small);
            Assertion.AssertEquals("Clone is not shallow clone", integer, enumSizeMapClone //$NON-NLS-1$
                    .Get(Size.Small));
        }

        [Test]
        public void containsValueLjava_lang_Object()
        {
            EnumMap<Size, Double> enumSizeMap = new EnumMap<Size, Double>(typeof(Size));
            Double double1 = 3;
            Double double2 = 3;

            Assertion.Assert("Returned true for uncontained value", !enumSizeMap //$NON-NLS-1$
                    .ContainsValue(double1));
            enumSizeMap.Put(Size.Middle, 2);
            enumSizeMap.Put(Size.Small, double1);
            Assertion.Assert("Returned false for contained value", enumSizeMap //$NON-NLS-1$
                    .ContainsValue(double1));
            Assertion.Assert("Returned false for contained value", enumSizeMap //$NON-NLS-1$
                    .ContainsValue(double2));
            Assertion.Assert("Returned false for contained value", enumSizeMap //$NON-NLS-1$
                    .ContainsValue(2));
            Assertion.Assert("Returned true for uncontained value", !enumSizeMap //$NON-NLS-1$
                    .ContainsValue(1));


        }

        [Test]
        public void test_entrySet()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);

            ISet<KeyValuePair<Size, int>> set = enumSizeMap.EntrySet();
            ISet<KeyValuePair<Size, int>> set1 = enumSizeMap.EntrySet();
            Assertion.AssertSame("Should be same", set1, set); //$NON-NLS-1$                                

            // The set is backed by the map so changes to one are reflected by the
            // other.
            enumSizeMap.Put(Size.Big, 3);
            enumSizeMap.Remove(Size.Big);

            Assertion.AssertEquals("Wrong size", 1, set.Count); //$NON-NLS-1$
            set.Clear();
            Assertion.AssertEquals("Wrong size", 0, set.Count); //$NON-NLS-1$

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            set = enumSizeMap.EntrySet();

            enumSizeMap.Put(Size.Middle, 1);

            Assertion.AssertEquals("Wrong size", 1, set.Count); //$NON-NLS-1$

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            set = enumSizeMap.EntrySet();

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            enumSizeMap.Put(Size.Big, 3);

            set = enumSizeMap.EntrySet();
            KeyValuePair<Size, Int32>[] array = set.ToArray();
            Assertion.AssertEquals("Wrong length", 2, array.Length); //$NON-NLS-1$
            KeyValuePair<Size, Int32> entry = (KeyValuePair<Size, Int32>)array[0];
            Assertion.AssertEquals("Wrong key", Size.Middle, entry.Key); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong value", 1, entry.Value); //$NON-NLS-1$

            KeyValuePair<Size, Int32>[] array1 = new KeyValuePair<Size, Int32>[10];
            array1 = set.ToArray();
            Assertion.AssertEquals("Wrong length", 2, array1.Length); //$NON-NLS-1$
            entry = (KeyValuePair<Size, Int32>)array[0];
            Assertion.AssertEquals("Wrong key", Size.Middle, entry.Key); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong value", 1, entry.Value); //$NON-NLS-1$

            array1 = new KeyValuePair<Size, Int32>[10];
            array1 = set.ToArray(array1);
            Assertion.AssertEquals("Wrong length", 10, array1.Length); //$NON-NLS-1$
            entry = (KeyValuePair<Size, Int32>)array[1];
            Assertion.AssertEquals("Wrong key", Size.Big, entry.Key); //$NON-NLS-1$
            // NO NULL in .NET Assertion.AssertNull("Should be null", array1[2]); //$NON-NLS-1$

            set = enumSizeMap.EntrySet();
            Assertion.Assert("Returned false when the object can be removed", set //$NON-NLS-1$
                    .Remove(entry));

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            // enumSizeMap.put(Size.Big, null);
            set = enumSizeMap.EntrySet();
            IEnumerator<KeyValuePair<Size, Int32>> iter = set.GetEnumerator();
            iter.MoveNext();
            entry = (KeyValuePair<Size, Int32>)iter.Current;
            Assertion.Assert("Returned false for contained object", set.Contains(entry)); //$NON-NLS-1$
            iter.MoveNext();
            entry = (KeyValuePair<Size, Int32>)iter.Current;
            Assertion.Assert("Returned false for contained object", set.Contains(entry)); //$NON-NLS-1$
            enumSizeMap.Put(Size.Middle, 1);
            enumSizeMap.Remove(Size.Big);
            Assertion.AssertEquals("Wrong size", 1, set.Count); //$NON-NLS-1$      
            enumSizeMap.Put(Size.Big, 2);

            // iter.Remove();

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            set = enumSizeMap.EntrySet();
            iter = set.GetEnumerator();
            iter.MoveNext();
            entry = (KeyValuePair<Size, Int32>)iter.Current;
            Assertion.AssertEquals("Wrong key", Size.Middle, entry.Key); //$NON-NLS-1$

            Assertion.Assert("Returned false for contained object", set.Contains(entry)); //$NON-NLS-1$
            enumSizeMap.Put(Size.Middle, 3);
            // hum hum ... Assertion.Assert("Returned false for contained object", set.Contains(entry)); //$NON-NLS-1$

            iter.MoveNext();
            //The following test case fails on RI.
            Assertion.AssertEquals("Wrong key", Size.Middle, entry.Key); //$NON-NLS-1$
            set.Clear();
            Assertion.AssertEquals("Wrong size", 0, set.Count); //$NON-NLS-1$

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);

            set = enumSizeMap.EntrySet();
            iter = set.GetEnumerator();
            iter.MoveNext();
            entry = (KeyValuePair<Size, Int32>)iter.Current;
            Assertion.AssertEquals("Wrong key", Size.Middle, entry.Key); //$NON-NLS-1$
            iter.MoveNext();
            entry = (KeyValuePair<Size, Int32>)iter.Current;
            // Assertion.AssertEquals("Wrong key", Size.Big, entry.Key); //$NON-NLS-1$
            /*iter.Remove();
            Assertion.AssertEquals("Wrong size", 1, set.Count); //$NON-NLS-1$*/

        }

        [Test]
        public void equalsLjava_lang_Object()
        {
            EnumMap<Size, Int32> enumMap = new EnumMap<Size, Int32>(typeof(Size));
            enumMap.Put(Size.Small, 1);

            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            Assertion.Assert("Returned true for unequal EnumMap", !enumSizeMap //$NON-NLS-1$
                    .Equals(enumMap));
            enumSizeMap.Put(Size.Small, 1);
            Assertion.Assert("Returned false for equal EnumMap", enumSizeMap //$NON-NLS-1$
                    .Equals(enumMap));
            // enumSizeMap.put(Size.Big, null);
            // Assertion.Assert("Returned true for unequal EnumMap", enumSizeMap //$NON-NLS-1$
            //         .Equals(enumMap));

            //enumMap.Put(Size.Middle, null);
            //Assertion.Assert("Returned true for unequal EnumMap", !enumSizeMap //$NON-NLS-1$
            //        .Equals(enumMap));
            enumMap.Remove(Size.Middle);
            enumMap.Put(Size.Big, 3);
            Assertion.Assert("Returned true for unequal EnumMap", !enumSizeMap //$NON-NLS-1$
                    .Equals(enumMap));
            //enumMap.Put(Size.Big, null);
            //Assertion.Assert("Returned false for equal EnumMap", enumSizeMap //$NON-NLS-1$
            //       .Equals(enumMap));

            IDictionary<Size, Int32> hashMap = new Dictionary<Size, Int32>();
            hashMap[Size.Small] = 1;
            Assertion.Assert("Returned true for unequal EnumMap", !hashMap //$NON-NLS-1$
                    .Equals(enumMap));
            //hashMap.Put(Size.Big, null);
            //Assertion.Assert("Returned false for equal EnumMap", enumMap.Equals(hashMap)); //$NON-NLS-1$

            Assertion.Assert("Should return false", !enumSizeMap //$NON-NLS-1$
                    .Equals(1));
        }

        [Test]
        public void keySet()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 2);
            //enumSizeMap.Put(Size.Big, null);
            ISet<Size> set = enumSizeMap.KeySet();

            ISet<Size> set1 = enumSizeMap.KeySet();
            Assertion.AssertSame("Should be same", set1, set); //$NON-NLS-1$

            Assertion.Assert("Returned false for contained object", set//$NON-NLS-1$
                    .Contains(Size.Middle));
            //Assertion.Assert("Returned false for contained object", set//$NON-NLS-1$
            //        .Contains(Size.Big));
            Assertion.Assert("Returned true for uncontained object", !set //$NON-NLS-1$
                    .Contains(Size.Small));
            //Assertion.Assert("Returned true for uncontained object", !set //$NON-NLS-1$
            //        .Contains(1));
            //Assertion.Assert("Returned false when the object can be removed", set //$NON-NLS-1$
            //        .Remove(Size.Big));
            Assertion.Assert("Returned true for uncontained object", !set //$NON-NLS-1$
                    .Contains(Size.Big));
            Assertion.Assert("Returned true when the object can not be removed", !set //$NON-NLS-1$
                    .Remove(Size.Big));
            //Assertion.Assert("Returned true when the object can not be removed", !set //$NON-NLS-1$
            //        .Remove(1));

            // The set is backed by the map so changes to one are reflected by the
            // other.
            enumSizeMap.Put(Size.Big, 3);
            Assertion.Assert("Returned false for contained object", set//$NON-NLS-1$
                    .Contains(Size.Big));
            enumSizeMap.Remove(Size.Big);
            Assertion.Assert("Returned true for uncontained object", !set //$NON-NLS-1$
                    .Contains(Size.Big));

            Assertion.AssertEquals("Wrong size", 1, set.Count); //$NON-NLS-1$
            set.Clear();
            Assertion.AssertEquals("Wrong size", 0, set.Count); //$NON-NLS-1$

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            enumSizeMap.Put(Size.Big, 0);
            set = enumSizeMap.KeySet();
            ICollection<Size> c = new List<Size>();
            c.Add(Size.Big);
            // Assertion.Assert("Should return true", set.ContainsAll(c)); //$NON-NLS-1$
            c.Add(Size.Small);
            Assertion.Assert("Should return false", !set.ContainsAll(c)); //$NON-NLS-1$
            Assertion.Assert("Should return true", set.RemoveAll(c)); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong size", 1, set.Count); //$NON-NLS-1$
            Assertion.Assert("Should return false", !set.RemoveAll(c)); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong size", 1, set.Count); //$NON-NLS-1$

            enumSizeMap.Put(Size.Big, 0);
            Assertion.AssertEquals("Wrong size", 2, set.Count); //$NON-NLS-1$
            Assertion.Assert("Should return true", set.RetainAll(c)); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong size", 1, set.Count); //$NON-NLS-1$
            Assertion.Assert("Should return false", !set.RetainAll(c)); //$NON-NLS-1$
            Assertion.AssertEquals(1, set.Count);
            Size[] array = set.ToArray();
            Assertion.AssertEquals("Wrong length", 1, array.Length); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong key", Size.Big, array[0]); //$NON-NLS-1$

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            enumSizeMap.Put(Size.Big, 0);
            set = enumSizeMap.KeySet();
            c = new List<Size>();
            //c.Add(Color.Blue);
            //Assertion.Assert("Should return false", !set.Remove(c)); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong size", 2, set.Count); //$NON-NLS-1$
            Assertion.Assert("Should return true", set.RetainAll(c)); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong size", 0, set.Count); //$NON-NLS-1$

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            enumSizeMap.Put(Size.Big, 0);
            set = enumSizeMap.KeySet();

            IEnumerator<Size> iter = set.GetEnumerator();
            iter.MoveNext();
            Enum enumKey = (Enum)iter.Current;
            Assertion.Assert("Returned false for contained object", set.Contains((Size)enumKey)); //$NON-NLS-1$
            iter.MoveNext();
            enumKey = (Enum)iter.Current;
            Assertion.Assert("Returned false for contained object", set.Contains((Size)enumKey)); //$NON-NLS-1$

            enumSizeMap.Remove(Size.Big);
            Assertion.Assert("Returned true for uncontained object", !set //$NON-NLS-1$
                    .Contains((Size)enumKey));
            // iter.Remove();
            /*
                                    try {
                                        iter.remove();
                                        fail("Should throw IllegalStateException"); //$NON-NLS-1$
                                    } catch (IllegalStateException e) {
                                        // Expected
                                    }
                                    assertFalse("Returned true for uncontained object", set //$NON-NLS-1$
                                            .contains(enumKey));
            */
            iter = set.GetEnumerator();
            iter.MoveNext();
            enumKey = (Enum)iter.Current;
            Assertion.Assert("Returned false for contained object", set.Contains((Size)enumKey)); //$NON-NLS-1$
            enumSizeMap.Put(Size.Middle, 3);
            Assertion.Assert("Returned false for contained object", set.Contains((Size)enumKey)); //$NON-NLS-1$

            enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            enumSizeMap.Put(Size.Middle, 1);
            enumSizeMap.Put(Size.Big, 0);
            set = enumSizeMap.KeySet();
            iter = set.GetEnumerator();
            /*try {
                iter.remove();
                fail("Should throw IllegalStateException"); //$NON-NLS-1$
            } catch (IllegalStateException e) {
                // Expected
            }*/
            iter.MoveNext();
            enumKey = (Enum)iter.Current;
            Assertion.AssertEquals("Wrong key", Size.Middle, enumKey); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong key", Size.Middle, enumKey); //$NON-NLS-1$
            Assertion.Assert("Returned true for unequal object", !iter.Equals(enumKey)); //$NON-NLS-1$
            // iter.Remove();
            //Assertion.Assert("Returned true for uncontained object", !set //$NON-NLS-1$
            //       .Contains((Size)enumKey));
            /*try {
                iter.remove();
                fail("Should throw IllegalStateException"); //$NON-NLS-1$
            } catch (IllegalStateException e) {
                // Expected
            }*/

            Assertion.AssertEquals("Wrong size", 2, set.Count); //$NON-NLS-1$
            iter.MoveNext();
            enumKey = (Enum)iter.Current;
            Assertion.AssertEquals("Wrong key", Size.Big, enumKey); //$NON-NLS-1$
            /*iter.remove();
            try {
                iter.next();
                fail("Should throw NoSuchElementException"); //$NON-NLS-1$
            } catch (NoSuchElementException e) {
                // Expected
            }*/
        }

        [Test]
        public void getLjava_lang_Object()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            Assertion.AssertEquals("Get returned non-null for non mapped key", enumSizeMap //$NON-NLS-1$
                    .Get(Size.Big), 0);
            enumSizeMap.Put(Size.Big, 1);
            Assertion.AssertEquals("Get returned incorrect value for given key", 1, //$NON-NLS-1$
                    enumSizeMap.Get(Size.Big));

            Assertion.AssertEquals("Get returned non-null for non mapped key", enumSizeMap //$NON-NLS-1$
                    .Get(Size.Small), 0);
            Assertion.AssertEquals("Get returned non-null for non existent key", enumSizeMap //$NON-NLS-1$
                    .Get(Color.Red), 0);
            Assertion.AssertEquals("Get returned non-null for non existent key", enumSizeMap //$NON-NLS-1$
                    .Get(1), 0);
            Assertion.AssertEquals("Get returned non-null for non existent key", enumSizeMap //$NON-NLS-1$
                    .Get(null), 0);

            EnumMap<Color, Double> enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            Assertion.AssertEquals("Get returned non-null for non mapped key", enumColorMap //$NON-NLS-1$
                    .Get(Color.Green), 0);
            enumColorMap.Put(Color.Green, 2);
            Assertion.AssertEquals("Get returned incorrect value for given key", 2, //$NON-NLS-1$
                    enumColorMap.Get(Color.Green));
            Assertion.AssertEquals("Get returned non-null for non mapped key", enumColorMap //$NON-NLS-1$
                    .Get(Color.Blue), 0);

            enumColorMap.Put(Color.Green, 4);
            Assertion.AssertEquals("Get returned incorrect value for given key", //$NON-NLS-1$
                    4, enumColorMap.Get(Color.Green));
            enumColorMap.Put(Color.Green, Int32.Parse("3"));//$NON-NLS-1$
            Assertion.AssertEquals("Get returned incorrect value for given key", Int32.Parse( //$NON-NLS-1$
                    "3"), enumColorMap.Get(Color.Green));//$NON-NLS-1$
            enumColorMap.Put(Color.Green, 0);
            Assertion.AssertEquals("Can not handle null value", enumColorMap.Get(Color.Green), 0); //$NON-NLS-1$
            Single f = Single.Parse("3.4");//$NON-NLS-1$
            enumColorMap.Put(Color.Green, f);
            Assertion.AssertEquals("Get returned incorrect value for given key", f, //$NON-NLS-1$
                    enumColorMap.Get(Color.Green), 0.000001);
        }

        [Test]
        public void test_putLjava_lang_ObjectLjava_lang_Object()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            /*try {
                enumSizeMap.put(Color.Red, 2);
                fail("Expected ClassCastException"); //$NON-NLS-1$
            } catch (ClassCastException e) {
                // Expected
            }*/
            Assertion.AssertEquals("Return non-null for non mapped key", enumSizeMap.Put( //$NON-NLS-1$
                    Size.Small, 1), 0);

            EnumMap<Color, Double> enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            /*try {
                enumColorMap.put(Size.Big, 2);
                fail("Expected ClassCastException"); //$NON-NLS-1$
            } catch (ClassCastException e) {
                // Expected
            }
            try {
                enumColorMap.put(null, 2);
                fail("Expected NullPointerException"); //$NON-NLS-1$
            } catch (NullPointerException e) {
                // Expected
            }*/
            Assertion.AssertEquals("Return non-null for non mapped key", enumColorMap.Put( //$NON-NLS-1$
                    Color.Green, 2), 0);
            Assertion.AssertEquals("Return wrong value", 2, enumColorMap.Put(Color.Green, //$NON-NLS-1$
                    4));
            Assertion.AssertEquals("Return wrong value", 4, enumColorMap.Put( //$NON-NLS-1$
                    Color.Green, Int32.Parse("3")));//$NON-NLS-1$
            Assertion.AssertEquals("Return wrong value", Int32.Parse("3"), enumColorMap.Put( //$NON-NLS-1$//$NON-NLS-2$
                    Color.Green, 0));
            Single f = Single.Parse("3.4");//$NON-NLS-1$
            Assertion.AssertEquals("Return non-null for non mapped key", enumColorMap.Put( //$NON-NLS-1$
                    Color.Green, f), 0);
            Assertion.AssertEquals("Return non-null for non mapped key", enumColorMap.Put( //$NON-NLS-1$
                    Color.Blue, 2), 0);
            Assertion.AssertEquals("Return wrong value", 2, enumColorMap.Put(Color.Blue, //$NON-NLS-1$
                    4));
        }

        [Test]
        public void putAllLjava_util_Map()
        {
            EnumMap<Color, Double> enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            enumColorMap.Put(Color.Green, 2);

            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            //enumColorMap.PutAll(enumSizeMap);

            enumSizeMap.Put(Size.Big, 1);
            /*try {
                enumColorMap.putAll(enumSizeMap);
                fail("Expected ClassCastException"); //$NON-NLS-1$
            } catch (ClassCastException e) {
                // Expected
            }*/

            EnumMap<Color, Double> enumColorMap1 = new EnumMap<Color, Double>(typeof(Color));
            enumColorMap1.Put(Color.Blue, 3);
            enumColorMap.PutAll(enumColorMap1);
            Assertion.AssertEquals("Get returned incorrect value for given key", 3, //$NON-NLS-1$
                    enumColorMap.Get(Color.Blue));
            Assertion.AssertEquals("Wrong Size", 2, enumColorMap.Count); //$NON-NLS-1$

            enumColorMap = new EnumMap<Color, Double>(typeof(Color));

            IDictionary<Color, Double> hashColorMap = null;
            /*try {
                enumColorMap.putAll(hashColorMap);
                fail("Expected NullPointerException"); //$NON-NLS-1$
            } catch (NullPointerException e) {
                // Expected
            }*/

            hashColorMap = new Dictionary<Color, Double>();
            enumColorMap.PutAll(hashColorMap);

            hashColorMap[Color.Green] = 2;
            enumColorMap.PutAll(hashColorMap);
            Assertion.AssertEquals("Get returned incorrect value for given key", 2, //$NON-NLS-1$
                    enumColorMap.Get(Color.Green));
            Assertion.AssertEquals("Get returned non-null for non mapped key", enumColorMap //$NON-NLS-1$
                    .Get(Color.Red), 0);
            hashColorMap[Color.Red] = 1;
            enumColorMap.PutAll(hashColorMap);
            Assertion.AssertEquals("Get returned incorrect value for given key", //$NON-NLS-1$
                    2, enumColorMap.Get(Color.Green));
            //hashColorMap[Size.Big] = 3;
            /*try {
                enumColorMap.putAll(hashColorMap);
                fail("Expected ClassCastException"); //$NON-NLS-1$
            } catch (ClassCastException e) {
                // Expected
            }*/

            hashColorMap = new Dictionary<Color, Double>();
            //hashColorMap.Put(1, 1);
            /*try {
                enumColorMap.putAll(hashColorMap);
                fail("Expected ClassCastException"); //$NON-NLS-1$
            } catch (ClassCastException e) {
                // Expected
            }*/
        }
        [Test]
        public void test_removeLjava_lang_Object()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            Assertion.AssertEquals("Remove of non-mapped key returned non-null", enumSizeMap //$NON-NLS-1$
                    .Remove(Size.Big), 0);
            enumSizeMap.Put(Size.Big, 3);
            enumSizeMap.Put(Size.Middle, 2);

            Assertion.AssertEquals("Get returned non-null for non mapped key", enumSizeMap //$NON-NLS-1$
                    .Get(Size.Small), 0);
            Assertion.AssertEquals("Remove returned incorrect value", 3, enumSizeMap //$NON-NLS-1$
                    .Remove(Size.Big));
            Assertion.AssertEquals("Get returned non-null for non mapped key", enumSizeMap //$NON-NLS-1$
                    .Get(Size.Big), 0);
            Assertion.AssertEquals("Remove of non-mapped key returned non-null", enumSizeMap //$NON-NLS-1$
                    .Remove(Size.Big), 0);
            /*Assertion.AssertNull("Remove of non-existent key returned non-null", enumSizeMap //$NON-NLS-1$
                    .Remove(Color.Red));
            Assertion.AssertNull("Remove of non-existent key returned non-null", enumSizeMap //$NON-NLS-1$
                    .Remove(4));
            Assertion.AssertNull("Remove of non-existent key returned non-null", enumSizeMap //$NON-NLS-1$
                    .Remove(null));*/

            EnumMap<Color, Double> enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            Assertion.AssertEquals("Get returned non-null for non mapped key", enumColorMap //$NON-NLS-1$
                    .Get(Color.Green), 0);
            enumColorMap.Put(Color.Green, 4);
            Assertion.AssertEquals("Remove returned incorrect value", 4, //$NON-NLS-1$
                    enumColorMap.Remove(Color.Green));
            Assertion.AssertEquals("Get returned non-null for non mapped key", enumColorMap //$NON-NLS-1$
                    .Get(Color.Green), 0);
            enumColorMap.Put(Color.Green, 0);
            Assertion.AssertEquals("Can not handle null value", enumColorMap //$NON-NLS-1$
                    .Remove(Color.Green), 0);
            Assertion.AssertEquals("Get returned non-null for non mapped key", enumColorMap //$NON-NLS-1$
                    .Get(Color.Green), 0);
        }

        [Test]
        public void test_size()
        {
            EnumMap<Size, Int32> enumSizeMap = new EnumMap<Size, Int32>(typeof(Size));
            Assertion.AssertEquals("Wrong size", 0, enumSizeMap.Count); //$NON-NLS-1$
            enumSizeMap.Put(Size.Small, 1);
            Assertion.AssertEquals("Wrong size", 1, enumSizeMap.Count); //$NON-NLS-1$
            enumSizeMap.Put(Size.Small, 0);
            Assertion.AssertEquals("Wrong size", 1, enumSizeMap.Count); //$NON-NLS-1$
            /*try {
                enumSizeMap.put(Color.Red, 2);
                fail("Expected ClassCastException"); //$NON-NLS-1$
            } catch (ClassCastException e) {
                // Expected
            }*/
            Assertion.AssertEquals("Wrong size", 1, enumSizeMap.Count); //$NON-NLS-1$

            enumSizeMap.Put(Size.Middle, 0);
            Assertion.AssertEquals("Wrong size", 2, enumSizeMap.Count); //$NON-NLS-1$
            enumSizeMap.Remove(Size.Big);
            Assertion.AssertEquals("Wrong size", 2, enumSizeMap.Count); //$NON-NLS-1$
            enumSizeMap.Remove(Size.Middle);
            Assertion.AssertEquals("Wrong size", 1, enumSizeMap.Count); //$NON-NLS-1$
            //enumSizeMap.Remove(Color.Green);
            Assertion.AssertEquals("Wrong size", 1, enumSizeMap.Count); //$NON-NLS-1$

            EnumMap<Color, Double> enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            enumColorMap.Put(Color.Green, 2);
            Assertion.AssertEquals("Wrong size", 1, enumColorMap.Count); //$NON-NLS-1$
            enumColorMap.Remove(Color.Green);
            Assertion.AssertEquals("Wrong size", 0, enumColorMap.Count); //$NON-NLS-1$

            EnumMap<Empty, Double> enumEmptyMap = new EnumMap<Empty, Double>(typeof(Empty));
            Assertion.AssertEquals("Wrong size", 0, enumEmptyMap.Count); //$NON-NLS-1$
        }

        [Test]
        public void test_values()
        {
            EnumMap<Color, Double> enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            enumColorMap.Put(Color.Red, 1);
            enumColorMap.Put(Color.Blue, 0);
            ICollection<double> collection = enumColorMap.Values();

            ICollection<double> collection1 = enumColorMap.Values();
            Assertion.AssertSame("Should be same", collection1, collection); //$NON-NLS-1$
            /*try {
                collection.add(new Integer(1));
                fail("Should throw UnsupportedOperationException"); //$NON-NLS-1$
            } catch (UnsupportedOperationException e) {
                // Expected
            }*/

            Assertion.Assert("Returned false for contained object", collection//$NON-NLS-1$
                    .Contains(1));
            //Assertion.Assert("Returned false for contained object", collection//$NON-NLS-1$
            //        .Contains(null));
            Assertion.Assert("Returned true for uncontained object", !collection //$NON-NLS-1$
                    .Contains(2));

            /*Assertion.Assert("Returned false when the object can be removed", collection //$NON-NLS-1$
                    .Remove(null));
            Assertion.Assert("Returned true for uncontained object", !collection //$NON-NLS-1$
                    .Contains(null));
            Assertion.Assert("Returned true when the object can not be removed", //$NON-NLS-1$
                    !collection.Remove(null));*/

            // The set is backed by the map so changes to one are reflected by the
            // other.
            enumColorMap.Put(Color.Blue, 3);
            Assertion.Assert("Returned false for contained object", collection//$NON-NLS-1$
                    .Contains(3));
            enumColorMap.Remove(Color.Blue);
            Assertion.Assert("Returned true for uncontained object", !collection//$NON-NLS-1$
                    .Contains(3));

            Assertion.AssertEquals("Wrong size", 1, collection.Count); //$NON-NLS-1$
            collection.Clear();
            Assertion.AssertEquals("Wrong size", 0, collection.Count); //$NON-NLS-1$

            enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            enumColorMap.Put(Color.Red, 1);
            enumColorMap.Put(Color.Blue, 0);
            collection = enumColorMap.Values();
            IList<double> c = new List<double>();
            c.Add(1);
            //Assertion.Assert("Should return true", collection.ContainsAll(c)); //$NON-NLS-1$
            c.Add(3.4);
            //Assertion.Assert("Should return false",! collection.ContainsAll(c)); //$NON-NLS-1$
            //Assertion.Assert("Should return true", collection.RemoveAll(c)); //$NON-NLS-1$
            //Assertion.AssertEquals("Wrong size", 1, collection.Count); //$NON-NLS-1$
            //Assertion.Assert("Should return false", !collection.RemoveAll(c)); //$NON-NLS-1$
            /*Assertion.AssertEquals("Wrong size", 1, collection.Count); //$NON-NLS-1$
            try {
                collection.AddAll(c);
                fail("Should throw UnsupportedOperationException"); //$NON-NLS-1$
            } catch (UnsupportedOperationException e) {
                // Expected
            }*/

            enumColorMap.Put(Color.Red, 1);
            Assertion.AssertEquals("Wrong size", 2, collection.Count); //$NON-NLS-1$
            /*Assertion.Assert("Should return true", collection.RetainAll(c)); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong size", 1, collection.Count); //$NON-NLS-1$
            Assertion.Assert("Should return false", !collection.RetainAll(c)); //$NON-NLS-1$*/
            Assertion.AssertEquals(2, collection.Count);
            double[] array = collection.ToArray();
            Assertion.AssertEquals("Wrong length", 2, array.Length); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong key", 1, array[0]); //$NON-NLS-1$

            enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            enumColorMap.Put(Color.Red, 1);
            enumColorMap.Put(Color.Blue, 0);
            collection = enumColorMap.Values();

            Assertion.AssertEquals("Wrong size", 2, collection.Count); //$NON-NLS-1$
            Assertion.Assert("Returned true when the object can not be removed", //$NON-NLS-1$
                    !collection.Remove(Int32.Parse("10"))); //$NON-NLS-1$

            IEnumerator<double> iter = enumColorMap.Values().GetEnumerator();
            iter.MoveNext();
            double value = iter.Current;
            Assertion.Assert("Returned false for contained object", collection //$NON-NLS-1$
                    .Contains(value));
            iter.MoveNext();
            value = iter.Current;
            Assertion.Assert("Returned false for contained object", collection //$NON-NLS-1$
                    .Contains(value));

            enumColorMap.Put(Color.Green, 1);
            enumColorMap.Remove(Color.Blue);
            Assertion.Assert("Returned true for uncontained object", !collection //$NON-NLS-1$
                    .Contains(value));
            /*iter.Remove();
            try {
                iter.remove();
                fail("Should throw IllegalStateException"); //$NON-NLS-1$
            } catch (IllegalStateException e) {
                // Expected
            }*/
            Assertion.Assert("Returned true for uncontained object", !collection //$NON-NLS-1$
                    .Contains(value));

            iter = enumColorMap.Values().GetEnumerator();
            iter.MoveNext();
            value = iter.Current;
            Assertion.Assert("Returned false for contained object", collection //$NON-NLS-1$
                    .Contains(value));
            enumColorMap.Put(Color.Green, 3);
            Assertion.Assert("Returned false for contained object", collection //$NON-NLS-1$
                    .Contains(value));
            Assertion.Assert("Returned false for contained object", collection //$NON-NLS-1$
                    .Remove(Int32.Parse("1"))); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong size", 1, collection.Count); //$NON-NLS-1$
            collection.Clear();
            Assertion.AssertEquals("Wrong size", 0, collection.Count); //$NON-NLS-1$

            enumColorMap = new EnumMap<Color, Double>(typeof(Color));
            Int32 integer1 = 1;
            enumColorMap.Put(Color.Green, integer1);
            enumColorMap.Put(Color.Blue, 0);
            collection = enumColorMap.Values();
            iter = enumColorMap.Values().GetEnumerator();
            /* try {
                 iter.remove();
                 fail("Should throw IllegalStateException"); //$NON-NLS-1$
             } catch (IllegalStateException e) {
                 // Expected
             }*/
            iter.MoveNext();
            value = iter.Current;
            Assertion.AssertEquals("Wrong value", integer1, value); //$NON-NLS-1$
            Assertion.AssertEquals("Wrong value", integer1, value); //$NON-NLS-1$
            Assertion.Assert("Returned true for unequal object", !iter.Equals(value)); //$NON-NLS-1$
            /*iter.Remove();
            Assertion.Assert("Returned true for unequal object", !iter.Equals(value)); //$NON-NLS-1$
            try {
                iter.Remove();
                fail("Should throw IllegalStateException"); //$NON-NLS-1$
            } catch (IllegalStateException e) {
                // Expected
            }*/
            Assertion.AssertEquals("Wrong size", 2, collection.Count); //$NON-NLS-1$
            iter.MoveNext();
            value = iter.Current;
            Assertion.Assert("Returned true for unequal object", !iter.Equals(value)); //$NON-NLS-1$
            //iter.Remove();
            /*try {
                iter.next();
                fail("Should throw NoSuchElementException"); //$NON-NLS-1$
            } catch (NoSuchElementException e) {
                // Expected
            }*/
        }
        /*
                                    public void testSerializationSelf() throws Exception {
                                        EnumMap enumColorMap = new EnumMap<Color, Double>(Color.class);
                                        enumColorMap.put(Color.Blue, 3);
                                        SerializationTest.verifySelf(enumColorMap);
                                    }


                                    public void testSerializationCompatibility() throws Exception {
                                        EnumMap enumColorMap = new EnumMap<Color, Double>(Color.class);
                                        enumColorMap.put(Color.Red, 1);
                                        enumColorMap.put(Color.Blue, 3);
                                        SerializationTest.verifyGolden(this, enumColorMap);
                                    }
                                }
                                */
    }
}
