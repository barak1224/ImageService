using Service.Controller;
using Service.Controller.Handlers;
using Service.Infrastructure.Enums;
using Service.Logging;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Server
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

        public void CreateHandler(string path)
        {
            IDirectoryHandler handler = new DirectoyHandler(m_controller, m_logging);
            handler.StartHandleDirectory(path);
            CommandRecieved += handler.OnCommandRecieved;
        }
    }
}
