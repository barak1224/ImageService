using System;
using System.Collections.Generic;
using ImageWeb.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageWeb.Controllers
{
    public class HomeController : Controller
    {
        static List<Student> students = new List<Student>()
        {
          new Student  { FirstName = "Barak", LastName = "Talmor", ID = 308146240 },
          new Student  { FirstName = "Iosi", LastName = "Ginerman", ID = 123456789 },
        };

        public ActionResult Index()
        {
            return View(students);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}