using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Model;
using ImageService.Logging;
using ImageService.Logging.Model;
using System.Configuration;
using ImageService.Infrastructure;

namespace ImageService
{
    /// <summary>
    /// Service state as enum
    /// </summary>
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    /// <summary>
    /// Struct to save details about the service status
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    /// <summary>
    /// The class which managing the service and writing to the log
    /// </summary>
    public partial class ImageService : ServiceBase
    {
        #region Members
        private System.ComponentModel.IContainer components = null;

        private System.Diagnostics.EventLog eventLog1;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private ImageServer m_imageServer;          // The Image Server
        private IImageServiceModel modelImage;
        private IImageController controller;
        private ILoggingService logging; private AppParsing appPar;

        private int eventId = 1;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageService()
        {
            InitializeComponent();
            appPar = new AppParsing();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(appPar.SourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    appPar.SourceName, appPar.LogName);
            }
            eventLog1.Source = appPar.SourceName;
            eventLog1.Log = appPar.LogName;
        }

        /// <summary>
        /// The function starts the service flow
        /// </summary>
        /// <param name="args"> Args </param>
        protected override void OnStart(string[] args)
        {
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            eventLog1.WriteEntry("In OnStart");

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            MembersInitialize();
        }

        /// <summary>
        /// The function initializing the members of the service
        /// </summary>
        private void MembersInitialize()
        {
            modelImage = new ImageServiceModel(appPar.OutputDir, appPar.ThubnailSized);
            controller = new ImageController(modelImage);
            logging = new LoggingService();
            logging.MessageRecieved += OnMessage;

            m_imageServer = new ImageServer(controller, logging, appPar.PathHandlers);
        }

        /// <summary>
        /// The function stops the service flow
        /// </summary>
        protected override void OnStop()
        {
            // Update the service state to Pause Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSE_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Write a log entry.
            eventLog1.WriteEntry("In OnStop");

            // Update the service state to Paused.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            m_imageServer.CloseServer();
            logging.MessageRecieved -= OnMessage;
            Dispose();
        }

        /// <summary>
        /// The function monitoring the service for showing it is still working
        /// </summary>
        /// <param name="sender"> the class ImageService </param>
        /// <param name="args"> the message </param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        /// <summary>
        /// Rising
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageArgs"></param>
        private void OnMessage(object sender, MessageRecievedEventArgs messageArgs)
        {
            if (messageArgs.Status == MessageTypeEnum.INFO)
            {
                eventLog1.WriteEntry("INFO:" + messageArgs.Message);
            } else
            {
                eventLog1.WriteEntry("FAIL:" + messageArgs.Message);
            }
        }

    }
}
