using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Collections;
using ILOG.J2CsMapping.Collections.Generics;

namespace MappingTests {

    [TestFixture]
    public class IteratorAdapterTests {

        [Test]
        public void TestEmptyListIterator() {
            TestList(new ArrayList());
        }

        [Test]
        public void TestOneElementListIterator() {
            IList list = new ArrayList();
            list.Add("string1");
            TestList(list);
        }

        [Test]
        public void TestTwoElementListIterator() {
            IList list = new ArrayList();
            list.Add("string1");
            list.Add("string2");
            TestList(list);
        }

        [Test]
        public void TestThreeElementListIterator() {
            IList list = new ArrayList();
            list.Add("string1");
            list.Add("string2");
            list.Add("string3");
            TestList(list);
        }

        [Test]
        public void TestFourElementListIterator() {
            IList list = new ArrayList();
            list.Add("string1");
            list.Add("string2");
            list.Add("string3");
            list.Add("string4");
            TestList(list);
        }

        private void TestList(IList list) {
            IList lst;

            lst = TestLoop1(list.GetEnumerator());
            TestListIdentity(list, lst);

            lst = TestLoop1b(list.GetEnumerator());
            TestListIdentity(list, lst);

            if (list.Count % 2 == 0) {
                lst = TestLoop2(list.GetEnumerator());
                TestListIdentity(list, lst);
            }
        }

        private IList TestLoop1(IEnumerator e) {
            IList result = new ArrayList();
            IteratorAdapter it = new IteratorAdapter(e);
            while (it.HasNext()) {
                object item = it.Next();
                result.Add(item);
            }
            return result;
        }

        private IList TestLoop1b(IEnumerator e) {
            IList result = new ArrayList();
            IteratorAdapter it = new IteratorAdapter(e);
            while (it.HasNext()) {
                object item = it.Next();
                result.Add(item);
                if (!it.HasNext()) {
                    break;
                }
            }
            return result;
        }

        private IList TestLoop2(IEnumerator e) {
            IList result = new ArrayList();
            IteratorAdapter it = new IteratorAdapter(e);
            while (it.HasNext()) {
                object item1 = it.Next();
                object item2 = it.Next();
                result.Add(item1);
                result.Add(item2);
            }
            return result;
        }

        private void TestListIdentity(IList list1, IList list2) {
            Assert.AreEqual(list1.Count, list2.Count, "The size of the list differs");
            for (int i = 0; i < list1.Count; i++) {
                Assert.AreEqual(list1[i], list2[i], "The items of the lists are not the same");
            }
        }

        [Test]
        public void TestEmptyListIteratorGeneric() {
            TestListGeneric(new List<string>());
        }

        [Test]
        public void TestOneElementListIteratorGeneric() {
            IList<string> list = new List<string>();
            list.Add("string1");
            TestListGeneric(list);
        }

        [Test]
        public void TestTwoElementListIteratorGeneric() {
            IList<string> list = new List<string>();
            list.Add("string1");
            list.Add("string2");
            TestListGeneric(list);
        }

        [Test]
        public void TestThreeElementListIteratorGeneric() {
            IList<string> list = new List<string>();
            list.Add("string1");
            list.Add("string2");
            list.Add("string3");
            TestListGeneric(list);
        }

        [Test]
        public void TestFourElementListIteratorGeneric() {
            IList<string> list = new List<string>();
            list.Add("string1");
            list.Add("string2");
            list.Add("string3");
            list.Add("string4");
            TestListGeneric(list);
        }

        private void TestListGeneric(IList<string> list) {
            IList<string> lst;

            lst = TestLoop1Generic(list.GetEnumerator());
            TestListIdentityGeneric(list, lst);

            lst = TestLoop1bGeneric(list.GetEnumerator());
            TestListIdentityGeneric(list, lst);

            if (list.Count % 2 == 0) {
                lst = TestLoop2Generic(list.GetEnumerator());
                TestListIdentityGeneric(list, lst);
            }
        }

        private IList<string> TestLoop1Generic(IEnumerator<string> e) {
            IList<string> result = new List<string>();
            IteratorAdapter<string> it = new IteratorAdapter<string>(e);
            while (it.HasNext()) {
                string item = it.Next();
                result.Add(item);
            }
            return result;
        }

        private IList<string> TestLoop1bGeneric(IEnumerator<string> e) {
            IList<string> result = new List<string>();
            IteratorAdapter<string> it = new IteratorAdapter<string>(e);
            while (it.HasNext()) {
                string item = it.Next();
                result.Add(item);
                if (!it.HasNext()) {
                    break;
                }
            }
            return result;
        }

        private IList<string> TestLoop2Generic(IEnumerator<string> e) {
            IList<string> result = new List<string>();
            IteratorAdapter<string> it = new IteratorAdapter<string>(e);
            while (it.HasNext()) {
                string item1 = it.Next();
                string item2 = it.Next();
                result.Add(item1);
                result.Add(item2);
            }
            return result;
        }

        private void TestListIdentityGeneric(IList<string> list1, IList<string> list2) {
            Assert.AreEqual(list1.Count, list2.Count, "The size of the list differs");
            for (int i = 0; i < list1.Count; i++) {
                Assert.AreEqual(list1[i], list2[i], "The items of the lists are not the same");
            }
        }

    }
}
