using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService
{
    public class AppParsing
    {
        public string[] PathHandlers { get; }
        public string OutputDir { get; }
        public string SourceName { get; }
        public string LogName { get; }
        public int ThubnailSized { get; }
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
    }
}
