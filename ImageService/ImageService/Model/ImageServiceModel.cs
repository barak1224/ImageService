using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Model
{
    /// <summary>
    /// The model for the service. In charge of performing the actions needed, like moving files.
    /// </summary>
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string OutputFolder { get; set; }
        private int Thumbnail { get; set; }

        private static Regex r = new Regex(":");

        #endregion

        /// <summary>
        /// Constructor.
        /// Gets the output folder (where to save the images) and the thumbnail size.
        /// </summary>
        /// <param name="m_OutputFolder"></param>
        /// <param name="m_thumbnailSize"></param>
        public ImageServiceModel(string outputFolder, int thumbnailSize)
        {
            OutputFolder = outputFolder;
            Thumbnail = thumbnailSize;
        }

        /// <summary>
        /// Method in charge of actually moving a file.
        /// It takes the original image, moves it an creates its thumbnail counterpart.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"> a bool variable to check if the  </param>
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {
            int counter = 1;
            Thread.Sleep(100);
            DirectoryInfo outDir = Directory.CreateDirectory(OutputFolder);
            outDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            // get the time of creation of the file
            DateTime date;
            try
            {
                date = GetDateTakenFromImage(path);
            }
            catch (Exception)
            {
                //The file has no film date to be extract
                date = File.GetCreationTime(path);
            }
            string dateFolder = Path.Combine(date.Year.ToString(), GetMonth(date.Month));
            string destPath = Path.Combine(OutputFolder, dateFolder);
            string destFile = Path.Combine(destPath, Path.GetFileName(path));

            //creates the folder. If it exists, it does nothing.
            Directory.CreateDirectory(destPath);
            bool exists = File.Exists(destFile);
            if (exists)
            {
                string tentativePath = destFile;
                while (File.Exists(tentativePath))
                {

                    tentativePath = Path.Combine(destPath, Path.GetFileNameWithoutExtension(destFile) + " (" + counter + ")" + Path.GetExtension(destFile));
                    counter++;
                }
                destFile = tentativePath;
            }

            try
            {
                File.Move(path, destFile);
            } catch (Exception e)
            {
                result = false;
                return String.Format("Moving the file \"{0}\" was faild",path);
            }

            try
            {
                AddToThumbnail(destFile, dateFolder);
            } catch (Exception e)
            {
                result = false;
                return String.Format("Copying the file \"{0}\" into a thumbnail copy was failed", path);
            }
            result = true;
            return String.Format("The file \"{0}\" added successfully to \"{1}\".", path, OutputFolder);
        }

        /// <summary>
        /// Creates and saves the thumbnail version for an image.
        /// </summary>
        /// <param name="file"> the file we need to create for him a thumbnail copy </param>
        /// <param name="destFolder"> The outputdir folder where hold the thumbnail folder </param>
        private void AddToThumbnail(string file, string destFolder)
        {
            string thumbnailFolder = Path.Combine(OutputFolder, Path.Combine(@"Thumbnail", destFolder));
            if (!Directory.Exists(thumbnailFolder))
            {
                Directory.CreateDirectory(thumbnailFolder);
            }

            using (Image source = Image.FromFile(file))
            using (Image thumb = source.GetThumbnailImage(100, 100, null, IntPtr.Zero))
            {
                thumb.Save(Path.Combine(thumbnailFolder, Path.GetFileName(file)));
                thumb.Dispose();
            }
        }

        /// <summary>
        /// Gets the name of the month by number.
        /// </summary>
        /// <param name="monthNumber"> converting the number month into a string month </param>
        /// <returns>English name of the monthNumber</returns>
        private static string GetMonth(int monthNumber)
        {
            switch (monthNumber)
            {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
                default: return "Default";
            }
        }

        /// <summary>
        /// Gets the date (year and month) in which the image what created.
        /// </summary>
        /// <param name="path"> the path of the file we need to move </param>
        /// <returns>A DateTime instance for the data the image was created.</returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }
    }
}