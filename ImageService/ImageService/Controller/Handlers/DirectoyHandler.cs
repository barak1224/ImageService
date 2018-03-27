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
        private Dictionary<int, Func<>> dict;
        #endregion

        public DirectoyHandler(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
        } 

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.RequestDirPath.Equals(m_path) || e.RequestDirPath.Equals("*")) 
            {
                if(e.CommandID == (int)CommandEnum.CloseCommand)
                {
                    CloseHandler();
                }
            }
        }

        private void CloseHandler()
        {
            foreach (FileSystemWatcher watcher in m_dirWatcher)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(m_path, "was closed"));
        }

        /**
         * The function creates the watchers to listen to the dirPath
         * Input: string dirPath - the directory to be handle by the handler
         **/
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
                watcher.Created += OnCreate;
                m_dirWatcher.Add(watcher);
            }
        }

        /**
         * When new file created with one of the extensions, the EventHandler of the watcher
         * calls 
         **/
        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            bool result;
            string[] args = { e.FullPath };
            string message = m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);   
            if (result)
            {
                m_logging.Log(message, MessageTypeEnum.INFO);
            }
            else
            {
                m_logging.Log(message, MessageTypeEnum.FAIL);
            }
        }
    }
}
