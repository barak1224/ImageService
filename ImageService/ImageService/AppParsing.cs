﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService
{
    /// <summary>
    /// The funciton is parsing the app config
    /// </summary>
    public class AppParsing
    {
        public List<string> PathHandlers { get; set; }
        public string OutputDir { get; set; }
        public string SourceName { get; }
        public string LogName { get; }
        public int ThumbnailSize { get; set; }
        public int Port { get; set; }
        public string IP { get; set; }

        public static AppParsing Instance = new AppParsing();

        /// <summary>
        /// The constructor parsing the app.config file and save as members
        /// </summary>
        private AppParsing()
        {
            PathHandlers = new List<string>(ConfigurationManager.AppSettings["Handler"].Split(';'));
            OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            SourceName = ConfigurationManager.AppSettings["SourceName"];
            LogName = ConfigurationManager.AppSettings["LogName"];
            int ts;
            if (int.TryParse(ConfigurationManager.AppSettings["ThumbnailSized"], out ts))
            {
                ThumbnailSize = ts;
            }
            else
            {
                ThumbnailSize = 120;
            }
            if (int.TryParse(ConfigurationManager.AppSettings["Port"], out ts))
            {
                Port = ts;
            } else
            {
                Port = 8000;
            }
            IP = ConfigurationManager.AppSettings["IP"];
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        public void Reload()
        {
            PathHandlers = new List<string>(ConfigurationManager.AppSettings["Handler"].Split(';'));
            OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            int ts;
            if (int.TryParse(ConfigurationManager.AppSettings["ThumbnailSized"], out ts))
            {
                ThumbnailSize = ts;
            }
            else
            {
                ThumbnailSize = 120;
            }
            if (int.TryParse(ConfigurationManager.AppSettings["Port"], out ts))
            {
                Port = ts;
            }
            else
            {
                Port = 8000;
            }
            IP = ConfigurationManager.AppSettings["IP"];
        }

        /// <summary>
        /// Removes the dir.
        /// </summary>
        /// <param name="dir">The dir.</param>
        public void RemoveDir(string dir)
        {
            PathHandlers.Remove(dir);
        }
    }
}