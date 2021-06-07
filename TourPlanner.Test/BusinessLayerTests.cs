using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using TourPlanner.BusinessLayer.TourFactory;
using Moq;
using TourPlanner.Models;
using NUnit.Framework;
using TourPlanner.BusinessLayer.SerializationFactory;

namespace TourPlanner.Test
{
    class BusinessLayerTests
    {
        private Mock<TourFactoryImpl> _mock = new();
        [SetUp]
        public void Init()
        {
            _mock.Setup((tl) => tl.GetItems()).ReturnsAsync(new List<Tour> { new Tour("test", "", "", 1, "", "") });
            _mock.CallBase = true;
        }
        [Test]
        public async Task Test_Search_Invalid_Case_Sensitive()
        {
            List<Tour> ret = (await _mock.Object.Search("Test",true)).ToList();
            Assert.AreEqual(0, ret.Count);
        }
        [Test]
        public async Task Test_Search_Valid_Case_Sensitive()
        {
            List<Tour> ret = (await _mock.Object.Search("test", true)).ToList();
            Assert.AreEqual(1, ret.Count);
        }
        [Test]
        public async Task Test_Search_Valid_Not_Case_Sensitive()
        {
            List<Tour> ret = (await _mock.Object.Search("Test")).ToList();
            Assert.AreNotEqual(0,ret.Count);
        }
        [Test]
        public void Test_Search_Null_Case_Sensitive()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () => await _mock.Object.Search(null, true));
        }
        [Test]
        public void Test_Search_Null_Not_Case_Sensitive()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () => await _mock.Object.Search(null));
        }
        [Test]
        public void Test_Deserialize_Invalid_Empty()
        {
            Assert.IsNull(SerializationFactory.GetInstance().Deserialize(""));
        }
        [Test]
        public void Test_Deserialize_Invalid_Null()
        {
            Assert.Throws<ArgumentNullException>(()=>SerializationFactory.GetInstance().Deserialize(null));
        }
        [Test]
        public void Test_Deserialize_Valid()
        {
            List<Tour> tours = SerializationFactory.GetInstance().Deserialize("[{\"Name\":\"Test\"}]").ToList();
            Assert.AreEqual("Test", tours[0].Name);
        }
        [Test]
        public void Test_Serialize_Invalid_Null()
        {
            string data=SerializationFactory.GetInstance().Serialize(null);
            Assert.AreEqual("null",data);
        }
        [Test]
        public void Test_Serialize_Valid()
        {
            string data = SerializationFactory.GetInstance().Serialize(new List<Tour>() { new Tour("name", "description", "path", 1, "Wien", "Kärnten"),});
            Assert.IsNotNull(data);
            Assert.IsNotEmpty(data);
        }
    }
}
