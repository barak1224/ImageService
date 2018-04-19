using System;
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
        public string[] PathHandlers { get; set; }
        public string OutputDir { get; set; }
        public string SourceName { get; }
        public string LogName { get; }
        public int ThubnailSized { get; set; }

        /// <summary>
        /// The constructor parsing the app.config file and save as members
        /// </summary>
        public AppParsing()
        {
            PathHandlers = ConfigurationManager.AppSettings["Handler"].Split(';');
            OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            SourceName = ConfigurationManager.AppSettings["SourceName"];
            LogName = ConfigurationManager.AppSettings["LogName"];
            int ts;
            if(int.TryParse(ConfigurationManager.AppSettings["ThumbnailSized"], out ts))
            {
                ThubnailSized = ts;
            }
        }

        public void Reload()
        {
            PathHandlers = ConfigurationManager.AppSettings["Handler"].Split(';');
            OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            int ts;
            if (int.TryParse(ConfigurationManager.AppSettings["ThumbnailSized"], out ts))
            {
                ThubnailSized = ts;
            }
        }
    }
}
