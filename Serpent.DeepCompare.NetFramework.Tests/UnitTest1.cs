using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Serpent.DeepCompare.NetFramework.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public class TestClass
        {
            private readonly string nisse = "ahaj";

            public int IntValue { get; set; }

            public string StringValue { get; set; }

            public TestClass2 Klass2 { get; set; }

        }

        public class TestClass2
        {
            public int IntValue2 { get; set; }

            public string StringValue2 { get; set; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var instance1 = new TestClass
            {
                IntValue = 1,
                StringValue = "123",
            };

            var instance2 = new TestClass
            {
                IntValue = 1,
                StringValue = "123",
            };

            var x = Compare.AreEqual(instance1, instance2);
            Assert.IsTrue(x);

        }

        [TestMethod]
        public void TestMethod2()
        {
            var instance1 = new TestClass
            {
                IntValue = 1,
                StringValue = "123",
                Klass2 = new TestClass2()
                {
                    IntValue2 = 2,
                    StringValue2 = "2"
                }
            };

            var instance2 = new TestClass
            {
                IntValue = 1,
                StringValue = "123",
                Klass2 = new TestClass2()
                {
                    IntValue2 = 2,
                    StringValue2 = "2"
                }
            };

            var x = Compare.AreEqual(instance1, instance2);
            Assert.IsTrue(x);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var instance1 = new TestClass
            {
                IntValue = 1,
                StringValue = "123",
                Klass2 = new TestClass2()
                {
                    IntValue2 = 2,
                    StringValue2 = "2"
                }
            };

            var instance2 = new TestClass
            {
                IntValue = 1,
                StringValue = "123",
                Klass2 = new TestClass2()
                {
                    IntValue2 = 2,
                    StringValue2 = "3"
                }
            };

            var x = Compare.AreEqual(instance1, instance2);
            Assert.IsFalse(x);
        }
    }
}
