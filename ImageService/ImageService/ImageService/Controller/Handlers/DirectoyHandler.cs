using ImageService.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private List<FileSystemWatcher> m_dirWatcher;       // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private string[] extensions;
        #endregion

        public DirectoyHandler(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
        }

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.RequestDirPath.Equals(m_path))
            {
                bool result;
                string message = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                if (result)
                {
                    m_logging.Log(message, MessageTypeEnum.INFO);
                } else
                {
                    m_logging.Log(message, MessageTypeEnum.FAIL);
                }
            }
        }

        public void StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            extensions = new string[]
            {
                "jpg", "png", "bmp", "gif"
            };
            m_dirWatcher = new List<FileSystemWatcher>();
            foreach (string ext in extensions)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(m_path, ext);
                watcher.Created += Watcher_CreatedFile;
                m_dirWatcher.Add(watcher);
            }
        }

        /**
         * When new file created with one of the extensions, the EventHandler of the watcher
         * calls 
         **/
        private void Watcher_CreatedFile(object sender, FileSystemEventArgs e)
        {
            //TODO
        }
    }
}
