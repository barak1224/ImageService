using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }

        public string Message { get; set; }             // The Message That goes to the logger

        /// <summary>
        /// The constructor of the class that holds the details of the handler that closed
        /// </summary>
        /// <param name="dirPath"> the directory path </param>
        /// <param name="message"> a message of the handler that has closed </param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }

    }
}
