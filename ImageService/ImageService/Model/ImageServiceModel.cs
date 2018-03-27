using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            // get the time of creation of the file
            DateTime date = File.GetCreationTime(path);
            string yearFolder = Path.Combine(OutputFolder, date.Year.ToString());
            string monthFolder = Path.Combine(yearFolder, GetMonth(date.Month));

            //creates the folder. If it exists, it does nothing.
            if (!Directory.Exists(monthFolder))
            {
                Directory.CreateDirectory(monthFolder);
            }

            return CopyFile(path, monthFolder, out result);
        }

        private string CopyFile(string path, string monthFolder, out bool result)
        {
            string destFile = Path.Combine(monthFolder, Path.GetFileName(path));
            try
            {
                File.Copy(path, monthFolder);
                
                result = true;
                return String.Format("File from \"{0}\" added successfully to \"{1}\".", path, monthFolder);
            } catch (IOException e) {
                
                bool temp = Directory.Exists(destFile);
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
    }
}
