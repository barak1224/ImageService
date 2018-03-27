using ImageService.Commands;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;

        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        /**
         * Constructor of ImageServer 
         */
        public ImageServer(IImageController controller, ILoggingService logging, string[] pathHandlers)
        {
            m_controller = controller;
            m_logging = logging;
            foreach (string pathHandler in pathHandlers)
            {
                CreateHandler(pathHandler);
            }
        }

        /**
         * Creating a handler for a directory path
         * Input: path - directory path
         */
        public void CreateHandler(string path)
        {
            IDirectoryHandler handler = new DirectoyHandler(m_controller, m_logging);
            handler.StartHandleDirectory(path);
            CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += OnCloseHandler;
        }

        /**
         * When the server is closing, the function send event to all handler to be close.
         */
        public void CloseServer()
        {
            CommandRecievedEventArgs comArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, "*");
            CommandRecieved?.Invoke(this, comArgs);
        }

        /**
         * When a handler is closed, he will raise a event and this function will remove it from the CommandRecieved
         * Input: sender - who was sent the event, DirectoryCloseEventArgs - details about the handler
         */
        private void OnCloseHandler(object sender, DirectoryCloseEventArgs d)
        {
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            CommandRecieved -= handler.OnCommandRecieved;
            m_logging.Log(d.DirectoryPath + " " + d.Message, MessageTypeEnum.INFO);
        }
    }
}
