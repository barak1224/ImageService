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
    /*
     *Class in charge of dealing with the basic functions for the files organization.
     */
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string OutputFolder { get; set; }
        private int Thumbnail { get; set; }

        #endregion

        // Constructor
        public ImageServiceModel(string m_OutputFolder, int m_thumbnailSize)
        {
            OutputFolder = m_OutputFolder;
            Thumbnail = m_thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            int counter = 1;
            Thread.Sleep(1000);
            // get the time of creation of the file
            DateTime date;
            try
            {
                date = GetDateTakenFromImage(path);
            } catch (Exception) {
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

            File.Move(path, destFile);
            AddToThumbnail(destFile, dateFolder);
            result = true;
            return String.Format("File from \"{0}\" added successfully to \"{1}\".", path, OutputFolder);
        }

        private void AddToThumbnail(string destFile, string destFolder)
        {
            string thumbnailFolder = Path.Combine(OutputFolder, Path.Combine(@"Thumbnail", destFolder));
            if (!Directory.Exists(thumbnailFolder))
            {
                Directory.CreateDirectory(thumbnailFolder);
            }

            using (Image source = Image.FromFile(destFile))
            using (Image thumb = source.GetThumbnailImage(100, 100, null, IntPtr.Zero))
            {
                thumb.Save(Path.Combine(thumbnailFolder, Path.ChangeExtension(Path.GetFileName(destFile), "thumb")));
                thumb.Dispose();
            }
        }

        private string CopyFile(string path, string monthFolder, out bool result)
        {
            try
            {
                File.Copy(path, monthFolder);
                result = true;

                return String.Format("File from \"{0}\" added successfully to \"{1}\".", path, monthFolder);
            } catch (IOException) {
                bool temp = Directory.Exists(Path.Combine(monthFolder, Path.GetFileName(path)));
                result = temp;
                return String.Format("A file called \"{0}\" already exists at \"{1}\".", Path.GetFileName(path), monthFolder);
            }
        }

        private void CreateFolder(string path, out bool result)
        {
            result = true; //might change
            Directory.CreateDirectory(path);
        }

        private void MoveFile(string path, out bool result)
        {
            throw new NotImplementedException();
        }

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

        private static Regex r = new Regex(":");

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
