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
            IImageServiceModel imageModel = new ImageServiceModel("C:\\Users\\barak\\Desktop", 5);
            string error = imageModel.AddFile("C:\\Users\\barak\\Pictures\\chatWith.jpg", out result);
            Assert.AreEqual(result, true, error);
        }
    }
}
