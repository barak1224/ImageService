using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageWeb.Models
{
    public class ConfigInfo
    {
        private static ConfigInfo m_instance;

        static ConfigInfo Instance {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ConfigInfo();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public int ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "OutputDir")]
        public string OutputDir { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Directories")]
        public List<string> Directories { get; set; }

    }
}