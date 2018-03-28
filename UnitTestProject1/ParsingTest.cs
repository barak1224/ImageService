using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageService;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class ParsingTest
    {
        [TestMethod]
        public void TestParsingApp()
        {
            AppParsing app = new AppParsing();
            Assert.AreEqual(app.SourceName, "ImageServiceSource");
            Assert.AreEqual(app.LogName, "ImageServiceLog");
            Assert.AreEqual(app.ThubnailSized, 120);
            Assert.AreEqual(app.OutputDir, "C:\\Users\\barak\\Images");
            Assert.AreEqual(app.PathHandlers[0], "C:\\Users\\barak\\Downloads");
            Assert.AreEqual(app.PathHandlers[1], "C:\\Users\\barak\\Pictures");
        }
    }
}
