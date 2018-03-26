using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        //private string m_OutputFolder;            // The Output Folder
        //private int m_thumbnailSize;              // The Size Of The Thumbnail

        private string OutputFolder { get; set; }
        private int Thumbnail { get; set; }

        public ImageServiceModel(string m_OutputFolder, int m_thumbnailSize)
        {
            OutputFolder = m_OutputFolder;
            Thumbnail = m_thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {

            throw new NotImplementedException(); 
        }

        #endregion

    }
}
