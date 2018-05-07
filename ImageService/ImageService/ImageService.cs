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
using ImageService.Commands;

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
        private ILoggingService logging;
        private AppParsing appPar;

        private int eventId = 1;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageService()
        {
            InitializeComponent();
            appPar = AppParsing.Instance;
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
            MembersInitialize();
            appPar.Reload();
            LogCommand loggg = new LogCommand();
            string s = loggg.Execute(null, out bool result);
            logging.Log(s, MessageTypeEnum.INFO);
            logging.Log("In OnStart", MessageTypeEnum.INFO);

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// The function initializing the members of the service
        /// </summary>
        private void MembersInitialize()
        {
            logging = new LoggingService();
            logging.MessageRecieved += OnMessage;

            m_imageServer = new ImageServer(logging);
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
            logging.Log("In OnStop", MessageTypeEnum.INFO);

            // Update the service state to Paused.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            m_imageServer.CloseServer();
            logging.MessageRecieved -= OnMessage;
            eventLog1.Clear();
            Dispose();
        }

        /// <summary>
        /// Getting a message to write to the logging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageArgs"></param>
        private void OnMessage(object sender, MessageRecievedEventArgs messageArgs)
        {
            switch ((int)messageArgs.Status)
            {
                case (int)MessageTypeEnum.INFO:
                    eventLog1.WriteEntry("INFO:" + messageArgs.Message, EventLogEntryType.Information, eventId++);
                    break;
                case (int)MessageTypeEnum.FAIL:
                    eventLog1.WriteEntry("FAIL:" + messageArgs.Message, EventLogEntryType.FailureAudit, eventId++);
                    break;
                case (int)MessageTypeEnum.WARNING:
                    eventLog1.WriteEntry("WARNING:" + messageArgs.Message, EventLogEntryType.Warning, eventId++);
                    break;
            }
        }
    }
}
