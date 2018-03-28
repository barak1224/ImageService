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
            int thumbnailSize = 5;
            bool result;
            IImageServiceModel imageModel = new ImageServiceModel(@"C:\Users\Iosi\Desktop", thumbnailSize);
            string error = imageModel.AddFile(@"C:\Users\Iosi\Pictures\TMlogo2.png", out result);

            Assert.AreEqual(result, true, error);
        }
    }
}
