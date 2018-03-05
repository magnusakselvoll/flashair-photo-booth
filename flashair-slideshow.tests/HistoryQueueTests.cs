using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace flashair_slideshow.tests
{
    [TestClass]
    public class HistoryQueueTests
    {
        [TestMethod]
        public void EmptyQueue()
        {
            var history = new HistoryQueue<string>(10);
            Assert.IsTrue(history.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void BadCapacity()
        {
            var history = new HistoryQueue<string>(-1);
        }

        [TestMethod]
        public void UnderCapacity()
        {
            const int capacity = 10;

            var history = new HistoryQueue<int>(capacity);

            const int numbersToAdd = 5;
            for (int i = 0; i < numbersToAdd; i++)
            {
                history.Enqueue(i);
            }

            Assert.IsTrue(history.Count == numbersToAdd);
            for (int i = 0; i < numbersToAdd; i++)
            {
                Assert.AreEqual(i, history[i]);
            }
        }

        [TestMethod]
        public void OnCapacity()
        {
            const int capacity = 100;

            var history = new HistoryQueue<int>(capacity);

            const int numbersToAdd = 100;
            for (int i = 0; i < numbersToAdd; i++)
            {
                history.Enqueue(i);
            }

            Assert.IsTrue(history.Count == numbersToAdd);
            for (int i = 0; i < numbersToAdd; i++)
            {
                Assert.AreEqual(i, history[i]);
            }
        }

        [TestMethod]
        public void OverCapacity()
        {
            const int capacity = 100001;

            var history = new HistoryQueue<int>(capacity);

            const int numbersToAdd = 200002;
            for (int i = 0; i < numbersToAdd; i++)
            {
                history.Enqueue(i);
            }

            Assert.IsTrue(history.Count == capacity);

            for (int i = 0; i < history.Count; i++)
            {
                Assert.AreEqual(numbersToAdd - capacity + i, history[i]);
            }
        }
    }
}
