using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageService.Model;

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
            string error = imageModel.AddFile("C:\\Users\\Iosi\\Pictures\\TMlogo3.png", out result);
            Assert.AreEqual(result, true, error);
        }
    }
}
