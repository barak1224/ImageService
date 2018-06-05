using System;
using System.Collections.Generic;
using ImageWeb.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Communication;
using Infrastructure.Communication;
using ImageService.Infrastructure.Enums;

namespace ImageWeb.Controllers
{
    public class HomeController : Controller
    {
        static HomeModel m_homeModel = new HomeModel();
        static SettingsModel m_settingsModel = new SettingsModel();
        static LogModel m_logModel = new LogModel();
        static ModelCommunicationHandler m_communication = ModelCommunicationHandler.Instance;
        public ActionResult Index()
        {
            return View(m_homeModel);
        }

        public ActionResult About()
        {
            m_settingsModel.SendConfigRequest();
            return View(m_settingsModel);
        }

        public ActionResult Contact()
        {
            m_logModel.LogsRequest();
            return View(m_logModel);
        }

        public ActionResult DeleteHandler(string directory)
        {
            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.CloseCommand;
            mc.CommandMsg = directory;
            m_communication.Client.Send(mc.ToJSON());
            return About();
        }
    }
}