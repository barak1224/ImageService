using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory

        /// <summary>
        /// The constructor of the class that holds the details of the command that the server sends
        /// </summary>
        /// <param name="id"> the command id </param>
        /// <param name="args"> the arguments of the command to execute </param>
        /// <param name="path"> the path of the directory that his handler need the execute the command </param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
