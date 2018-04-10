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
        private FileSystemWatcher m_dirWatcher;       // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private static readonly string[] extensions = { ".jpg", ".png", ".bmp", ".gif" };
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller"> The controller of the service </param>
        /// <param name="logging"> The class that hold the event handler to notify on new message to the event log </param>
        public DirectoyHandler(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
        }

        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;       

        /// <summary>
        /// The function getting a command to executing by controller or by itself (close)
        /// </summary>
        /// <param name="sender"> The class that call OnCreated with the event handler </param>
        /// <param name="e"> The details of the command that need to be execute </param>
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

        /// <summary>
        /// The function is closing the handler and raising the event to notify that the handler was closed
        /// </summary>
        private void CloseHandler()
        {
            m_dirWatcher.EnableRaisingEvents = false;
            m_dirWatcher.Dispose();
            DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(m_path, "was closed"));
        }


        /// <summary>
        /// The function creates the watchers to listen to the dirPath
        /// </summary>
        /// <param name="dirPath"> the directory to be handle by the handler </param>
        public void StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            m_dirWatcher = new FileSystemWatcher();
            FileSystemWatcher watcher = new FileSystemWatcher(m_path);
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
        |                           NotifyFilters.FileName | NotifyFilters.DirectoryName;
        }

        /// <summary>
        /// When new file created with one of the extensions, the EventHandler of the watcher
        /// </summary>
        /// <param name="sender"> The class that call OnCreated with the event handler </param>
        /// <param name="e"> The details of the new file that was added to the handler directory </param>
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            bool result;
            string extension = Path.GetExtension(e.FullPath);
            if (extensions.Contains(extension))
            {
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
}
