using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageWeb.Models
{
    public class PhotosModel
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

        public PhotosModel(SettingsModel settings)
        {
            Images = new List<ImageItem>();
            Settings = settings;
            if (!settings.WasRequested)
                settings.SendConfigRequest();
            OutputDir = settings.OutputDirName;
        }
    }
}