﻿using Communication;
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
        private IImageServiceModel m_modelImage;
        private TCPServiceServer m_tcpServer;

        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        /// <summary>
        /// Constructor of ImageServer
        /// </summary>
        /// <param name="controller"> The controller of the service </param>
        /// <param name="logging"> The class that hold the event handler to notify on new message to the event log </param>
        /// <param name="pathHandlers"> the path of the directories that need to create for them handlers</param>
        public ImageServer(ILoggingService logging)
        {
            AppParsing appPar = AppParsing.Instance;
            m_modelImage = new ImageServiceModel(appPar.OutputDir, appPar.ThumbnailSize);
            m_controller = new ImageController(m_modelImage);
            m_logging = logging;
            foreach (string pathHandler in appPar.PathHandlers)
            {
                CreateHandler(pathHandler);
                logging.Log($"Handler for {pathHandler} was created", MessageTypeEnum.INFO);
            }
            m_controller.PassCommandReceived += CommandRecievedSend;
            m_tcpServer = new TCPServiceServer(8001, ref m_controller);
            m_tcpServer.Start();
        }

        private void CommandRecievedSend(object sender, CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(sender, e);
        }

        /// <summary>
        /// Creating a handler for a directory path
        /// </summary>
        /// <param name="path"> directory path </param>
        public void CreateHandler(string path)
        {
            IDirectoryHandler handler = new DirectoyHandler(m_controller, m_logging);
            handler.StartHandleDirectory(path);
            CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += OnCloseHandler;
        }

        /// <summary>
        /// When the server is closing, the function send event to all handler to be close.
        /// </summary>
        public void CloseServer()
        {
            CommandRecievedEventArgs comArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, "*");
            CommandRecieved?.Invoke(this, comArgs);
        }

        /// <summary>
        /// When a handler is closed, he will raise a event and this function will remove it from the CommandRecieved
        /// </summary>
        /// <param name="sender"> who was sent the event </param>
        /// <param name="d"> details about the handler </param>
        private void OnCloseHandler(object sender, DirectoryCloseEventArgs d)
        {
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            CommandRecieved -= handler.OnCommandRecieved;
            m_logging.Log(d.DirectoryPath + " " + d.Message, MessageTypeEnum.INFO);
        }
    }
}
