using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    using ILOG.J2CsMapping.Collections.Generics;
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    // These test results are used for CListIterator (C#) check
    [TestFixture]
    public class ListIteratorTest
    {

        private static readonly String[] items = { "a", "b", "c" };
        private IList<String> list;

        [SetUp]
        public void SetUp()
        {
            list = new List<string>(items);
        }

        [Test]
        public void ShouldReadThreeElementsAndFail()
        {
            IListIterator<String> it = new ArrayListIterator<String>(list);
            Assert.AreEqual(it.Next(), "a");
            Assert.AreEqual(it.Next(), "b");
            Assert.AreEqual(it.Next(), "c");
            try
            {
                it.Next();
                Assert.Fail("Expected NoSuchElementException");
            }
            catch (InvalidOperationException e)
            {
            }
        }

        [Test]
        public void ShouldReadTheSecondElementThreeTimes()
        {
            IListIterator<String> it = new ArrayListIterator<string>(list);
            Assert.AreEqual(it.Next(), "a");
            Assert.AreEqual(it.Next(), "b");
            Assert.AreEqual(it.Previous(), "b");
            Assert.AreEqual(it.Next(), "b");
            Assert.AreEqual(it.Next(), "c");
            try
            {
                it.Next();
                Assert.Fail("Expected NoSuchElementException");
            }
            catch (InvalidOperationException e)
            {
            }
        }

        [Test]
        public void ShouldAddToBeginAndNextDoesntReadIt()
        {
            IListIterator<String> it = new ArrayListIterator<string>(list);
            it.Add("d");
            Assert.AreEqual(it.Next(), "a");
            Assert.AreEqual(it.Next(), "b");
            Assert.AreEqual(it.Next(), "c");
            try
            {
                it.Next();
                Assert.Fail("Expected NoSuchElementException");
            }
            catch (InvalidOperationException e)
            {
            }
        }

        [Test]
        public void ShouldAddToBeginAndPreviousReadIt()
        {
            IListIterator<String> it = new ArrayListIterator<string>(list);
            it.Add("d");
            Assert.AreEqual(it.Previous(), "d");
            Assert.AreEqual(it.Next(), "d");
            Assert.AreEqual(it.Next(), "a");
        }

        [Test]
        public void ShouldAddToMiddleAndPreviousReadIt()
        {
            IListIterator<String> it = new ArrayListIterator<string>(list);
            Assert.AreEqual(it.Next(), "a");
            it.Add("d");
            Assert.AreEqual(it.Previous(), "d");
            Assert.AreEqual(it.Next(), "d");
        }

        [Test]
        public void ShouldAddToMiddleAndNextDoesntReadIt()
        {
            IListIterator<String> it = new ArrayListIterator<string>(list);
            Assert.AreEqual(it.Next(), "a");
            it.Add("d");
            Assert.AreEqual(it.Next(), "b");
            Assert.AreEqual(it.Next(), "c");
            try
            {
                it.Next();
                Assert.Fail("Expected NoSuchElementException");
            }
            catch (InvalidOperationException e)
            {
            }
        }

        [Test]
        public void ShouldAddToEndAndNextDoesntReadIt()
        {
            IListIterator<String> it = new ArrayListIterator<string>(list);
            Assert.AreEqual(it.Next(), "a");
            Assert.AreEqual(it.Next(), "b");
            Assert.AreEqual(it.Next(), "c");
            it.Add("d");
            try
            {
                it.Next();
                Assert.Fail("Expected NoSuchElementException");
            }
            catch (InvalidOperationException e)
            {
            }
        }

    }
}
