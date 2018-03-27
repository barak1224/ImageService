using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Model;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddFile()
        {
            bool result = false;
            ImageServiceModel imageModel = new ImageServiceModel("C:\\Users\\Iosi\\Desktop", 5);
            imageModel.AddFile("C:\\Users\\Iosi\\Pictures\\final.jpg", out result);
            Assert.AreEqual(result, true);
        }
    }
}
