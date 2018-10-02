using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesDetail;

namespace ProductUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Product p = new Product("book", 4, (decimal)12.99, (decimal)5.5 / 100);
            Assert.AreEqual(p.name, "book");
            Assert.AreEqual(p.count, 4);
            Assert.AreEqual(p.Tax(), (decimal)2.9);
        }
    }
}
