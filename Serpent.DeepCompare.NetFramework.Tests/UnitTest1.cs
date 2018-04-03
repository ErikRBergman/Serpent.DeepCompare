namespace Serpent.DeepCompare.NetFramework.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEnumerableEqual()
        {
            var collection1 = new[]
                                  {
                                      new TestClass
                                          {
                                              IntValue = 1
                                          },
                                      new TestClass
                                          {
                                              IntValue = 2
                                          },
                                  };

            var collection2 = new[]
                                  {
                                      new TestClass
                                          {
                                              IntValue = 1
                                          },
                                      new TestClass
                                          {
                                              IntValue = 2
                                          },
                                  };

            Assert.IsTrue(Compare.AreEqual(collection1, collection2));
        }

        [TestMethod]
        public void TestEnumerableNotEqual_Different_Size_Collections()
        {
            var collection1 = new[]
                                  {
                                      new TestClass
                                          {
                                              IntValue = 1
                                          },
                                      new TestClass
                                          {
                                              IntValue = 2
                                          },
                                      new TestClass
                                          {
                                              IntValue = 3
                                          },
                                  };

            var collection2 = new[]
                                  {
                                      new TestClass
                                          {
                                              IntValue = 1
                                          },
                                      new TestClass
                                          {
                                              IntValue = 2
                                          },
                                  };

            Assert.IsFalse(Compare.AreEqual(collection1, collection2));
        }

        [TestMethod]
        public void TestEnumerableNotEqual_Different_Size_Collections2()
        {
            var collection1 = new[]
                                  {
                                      new TestClass
                                          {
                                              IntValue = 1
                                          },
                                      new TestClass
                                          {
                                              IntValue = 2
                                          },
                                      new TestClass
                                          {
                                              IntValue = 3
                                          },
                                  };

            var collection2 = new[]
                                  {
                                      new TestClass
                                          {
                                              IntValue = 1
                                          },
                                      new TestClass
                                          {
                                              IntValue = 2
                                          },
                                  };

            Assert.IsFalse(Compare.AreEqual(collection2, collection1));
        }


        [TestMethod]
        public void TestEnumerableNotEqual()
        {
            var collection1 = new[]
                                  {
                                      new TestClass
                                          {
                                              IntValue = 1
                                          },
                                      new TestClass
                                          {
                                              IntValue = 3
                                          },
                                  };

            var collection2 = new[]
                                  {
                                      new TestClass
                                          {
                                              IntValue = 1
                                          },
                                      new TestClass
                                          {
                                              IntValue = 2
                                          },
                                  };

            Assert.IsFalse(Compare.AreEqual(collection1, collection2));
        }

        [TestMethod]
        public void TestMethod1()
        {
            var instance1 = new TestClass
                                {
                                    IntValue = 1,
                                    StringValue = "123"
                                };

            var instance2 = new TestClass
                                {
                                    IntValue = 1,
                                    StringValue = "123"
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
                                    Klass2 = new TestClass2
                                                 {
                                                     IntValue2 = 2,
                                                     StringValue2 = "2"
                                                 }
                                };

            var instance2 = new TestClass
                                {
                                    IntValue = 1,
                                    StringValue = "123",
                                    Klass2 = new TestClass2
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
                                    Klass2 = new TestClass2
                                                 {
                                                     IntValue2 = 2,
                                                     StringValue2 = "2"
                                                 }
                                };

            var instance2 = new TestClass
                                {
                                    IntValue = 1,
                                    StringValue = "123",
                                    Klass2 = new TestClass2
                                                 {
                                                     IntValue2 = 2,
                                                     StringValue2 = "3"
                                                 }
                                };

            var x = Compare.AreEqual(instance1, instance2);
            Assert.IsFalse(x);
        }

        public class TestClass
        {
            private readonly string nisse = "ahaj";

            public TestClass2[] Class2Array { get; set; }

            public int IntValue { get; set; }

            public TestClass2 Klass2 { get; set; }

            public string StringValue { get; set; }
        }

        public class TestClass2
        {
            public int IntValue2 { get; set; }

            public string StringValue2 { get; set; }
        }

        public class TestClass3
        {
            public int IntValue2 { get; set; }

            public string StringValue2 { get; set; }
        }
    }
}