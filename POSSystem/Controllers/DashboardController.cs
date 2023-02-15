using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
//using static KZ.Models.ViewModel_api;
using POSData;
using System.Linq;
using static POSSystem.Models.ViewModel;
using POSSystem.Utils;

namespace POSSystem.Controllers
{
    public class DashboardController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DashboardController));
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private POSSystemEntities Context { get; set; }
        public string SQLPathSyM = "SystemManager/";
        public DashboardController()
        {
            Context = new POSSystemEntities();
        }
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public JsonResult ActivitiesLog()
        {
            var result = new ResultMessage();
            var logs = new List<ViewModelLog>();
            var finallogs = new List<ViewModelLog>();
            var folder = Server.MapPath("~/logs");
            DirectoryInfo info = new DirectoryInfo(folder);
            if (Directory.Exists(folder))
            {
                var files = info.GetFiles().OrderByDescending(x => x.CreationTime);
                var file = files.FirstOrDefault();
                var readfile = new StreamReader(file.FullName);
                var line = "";
                var count = 0;
                int index = 0;
                int index1 = 0;
                string dateTime = string.Empty;
                string caller = string.Empty;
                string action = string.Empty;
                while ((line = readfile.ReadLine()) != null)
                {
                    ViewModelLog log = new ViewModelLog();
                    string textData = line;
                    index = textData.IndexOf(" - ");
                    dateTime = textData.Substring(0, 19);
                    log.LogAction = textData.Substring(index + 3);
                    string tempString = textData.Substring(0, index);
                    index1 = tempString.IndexOf("]");
                    caller = tempString.Substring(index1 + 1);
                    log.LogDate = dateTime;
                    log.CallerName = caller;
                    log.SrNo = count;
                    logs.Add(log);
                    count++;
                }
                readfile.Close();
            }
            for (var i = logs.Count - 1; i >= 0; i--)
            {
                finallogs.Add(logs[i]);
            }
            result.data = finallogs;
            result.result = "success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
    }
}