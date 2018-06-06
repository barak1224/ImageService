﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ImageWeb.Models
{
    public class ImageItem
    {

        #region Properties
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageUrl")]
        public string ImageFullThumbnailUrl { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageRelativePathThumbnail")]
        public string ImageRelativePathThumbnail { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageRelativePath")]
        public string ImageRelativePath { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageFullUrl")]
        public string ImageFullUrl { get; set; }
        #endregion

        public ImageItem(string imageUrl)
        {
            try
            {
                ImageFullThumbnailUrl = imageUrl;
                ImageFullUrl = imageUrl.Replace(@"Thumbnail\", string.Empty);
                Name = Path.GetFileNameWithoutExtension(ImageFullThumbnailUrl);
                Month = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(ImageFullThumbnailUrl));
                Year = Path.GetFileNameWithoutExtension(Path.GetDirectoryName((Path.GetDirectoryName(ImageFullThumbnailUrl))));
                string strDirName;

                int intLocation, intLength;

                intLength = imageUrl.Length;
                intLocation = imageUrl.IndexOf("OutputDir");

                strDirName = imageUrl.Substring(intLocation, intLength - intLocation);

                ImageRelativePathThumbnail = @"~\" + strDirName;
                ImageRelativePath = ImageRelativePathThumbnail.Replace(@"Thumbnail\", string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}