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
        static ImagesModel m_imagesModel = new ImagesModel(m_settingsModel);
        static ModelCommunicationHandler m_communication = ModelCommunicationHandler.Instance;
        static string m_directory;
        public ActionResult Index()
        {
            m_homeModel.CountNumberOfPhotos();
            return View(m_homeModel);
        }

        public ActionResult Settings()
        {
            m_settingsModel.SettingsRequest();
            return View(m_settingsModel);
        }

        public ActionResult Contact()
        {
            m_logModel.LogsRequest();
            return View(m_logModel);
        }

        public ActionResult Images()
        {
            m_imagesModel.SetPhotos();
            return View(m_imagesModel);
        }

        public ActionResult HandlerDeleter()
        {
            return View(m_settingsModel);
        }

        public ActionResult CheckHandlerDelete(string directory)
        {
            m_directory = directory;
            return RedirectToAction("HandlerDeleter");
        }

        public ActionResult DeleteHandler()
        {
            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.CloseCommand;
            mc.CommandMsg = m_directory;
            m_communication.Client.Send(mc.ToJSON());
            m_settingsModel.SettingsRequest();
            return RedirectToAction("Settings");
        }
    }
}