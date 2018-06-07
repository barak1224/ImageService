using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Communication;
using System.IO;

namespace ImageWeb.Models
{
    public class HomeModel
    {
        private ModelCommunicationHandler m_client;
        public bool IsConnected { get; set; }
        public ConfigInfo m_conf;
        public int NumberOfPhotos { get; set; }
        
        public List<Student> Students = new List<Student>()
        {
          new Student  { FirstName = "Barak", LastName = "Talmor", ID = 308146240 },
          new Student  { FirstName = "Iosi", LastName = "Ginerman", ID = 332494830 },
        };

        public HomeModel()
        {
            m_client = ModelCommunicationHandler.Instance;
            IsConnected = m_client.IsConnected;
            m_conf = ConfigInfo.Instance;
            m_conf.SendConfigRequest();
        }

        public void CountNumberOfPhotos()
        {
            int count = 0;
            string Directory = m_conf.OutputDir;
            Directory = Directory + "\\Thumbnail";
            DirectoryInfo directoryName = new DirectoryInfo(Directory);
            foreach (string exetension in m_conf.ValidExtensions)
            {
                count += directoryName.GetFiles(exetension, SearchOption.AllDirectories).Length;
            }
            NumberOfPhotos = count;
        }
    }
}