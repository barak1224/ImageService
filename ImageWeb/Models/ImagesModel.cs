using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageWeb.Models
{
    public class ImagesModel
    {
        #region Members
        private string _outputDir;
        private SettingsModel _settings;
        #endregion

        #region Properties
        public string OutputDir { get; set; }
        public List<ImageItem> Images { get; set; }

        public SettingsModel Settings { get; set; }

        public int NumberOfImages
        {
            get
            {
                return Images.Count;
            }
        }


        #endregion

        public ImagesModel(SettingsModel settings)
        {
            Images = new List<ImageItem>();
            Settings = settings;
            if (!settings.WasRequested)
                settings.SettingsRequest();
            OutputDir = settings.OutputDirName;
        }

        public void SetPhotos()
        {
            string[] validExtensions = { ".jpg", ".png", ".gif", ".bmp" };
            Images.Clear();
            try
            {
                string thumbnailDir = OutputDir + @"\Thumbnail";
                if (!Directory.Exists(thumbnailDir))
                {
                    return;
                }
                DirectoryInfo di = new DirectoryInfo(thumbnailDir);

                foreach (DirectoryInfo yearDirInfo in di.GetDirectories())
                {
                    if (!Path.GetDirectoryName(yearDirInfo.FullName).EndsWith("Thumbnail"))
                    {
                        continue;
                    }
                    foreach (DirectoryInfo monthDirInfo in yearDirInfo.GetDirectories())
                    {
                        foreach (FileInfo fileInfo in monthDirInfo.GetFiles())
                        {
                            if (validExtensions.Contains(fileInfo.Extension.ToLower()))
                            {
                                if (Images.Find( x => (x.ImageFullUrl == fileInfo.FullName)) == null)
                                {
                                    Images.Add(new ImageItem(fileInfo.FullName));
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeletePhoto(string fullUrl)
        {
            try
            {
                foreach (ImageItem photo in Images)
                {
                    if (photo.ImageFullUrl.Equals(fullUrl))
                    {
                        Images.Remove(photo);
                        File.Delete(fullUrl);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}