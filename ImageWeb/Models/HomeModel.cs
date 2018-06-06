using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Communication;

namespace ImageWeb.Models
{
    public class HomeModel
    {
        private ModelCommunicationHandler m_client;
        public bool IsConnected { get; set; }
        
        public List<Student> Students = new List<Student>()
        {
          new Student  { FirstName = "Barak", LastName = "Talmor", ID = 308146240 },
          new Student  { FirstName = "Iosi", LastName = "Ginerman", ID = 332494830 },
        };

        public HomeModel()
        {
            m_client = ModelCommunicationHandler.Instance;
            IsConnected = m_client.IsConnected;
        }
    }
}