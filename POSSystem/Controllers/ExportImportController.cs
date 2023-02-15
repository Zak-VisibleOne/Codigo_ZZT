using POSData;
//using CityU.Utils;
//using CityU.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
//using System.Data.Entity.Validation;
using System;
using static POSSystem.Models.CommonModel;
using static POSSystem.Models.ViewModel;
using System.Data;
//using ClosedXML.Excel;
using System.IO;
using System.Text;
//using CsvHelper;
using System.Globalization;
using POSSystem.Models;
using Rotativa.Options;
namespace POSSystem.Controllers
{
    public class ExportImportController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ExportImportController));
        private POSSystemEntities Context { get; set; }
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ExportImportController()
        {
            Context = new POSSystemEntities();
        }
        public ActionResult exportExcel()
        {
            return View();
        }
        public ActionResult exportCSV()
        {
            string name = Request.QueryString["name"];
            string user = (Session["User"] ?? "").ToString();
            logger.Info("Exported csv(" + name + ") by " + user + " in " + DateTime.Now.ToLongDateString());
            var list = new List<viewExport>();
            int srNo = 1;
            if (name == "Department")
            {
                var semObj = Context.Departments.ToList();
                foreach (var l in semObj)
                {
                    var data = new viewExport();
                    data.SrNo = srNo;
                    data.Name = l.DepName;
                    list.Add(data);
                    srNo++;
                }
            }
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);
           
            memoryStream.Position = 0;
            return File(memoryStream, "text/csv", name + "_" + DateTime.Now.ToString("yyyymmddMMss") + ".csv");
        }
        #region One Document for one Course
        public ActionResult PdfDocument(string ListCourse, bool Part1, bool Part2, bool Part3)
        {
            string footer = Server.MapPath("~/Staticpage/Footer.html");//Path of Footer.html File
            string customSwitches = string.Format(
                                   "--header-spacing \"0\" " +
                                   "--footer-html \"{0}\" " +
                                   "--footer-spacing \"10\" " +
                                   "--page-offset 0 --footer-right [page] --footer-font-size 8 " //get paging in center of footer
                                  , footer);
            ViewBag.ListCourse = ListCourse;
            ViewBag.Part1 = Part1;
            ViewBag.Part2 = Part2;
            ViewBag.Part3 = Part3;
            //string user = (Session["User"] ?? "").ToString();
            //logger.Info("Generated pdf with course code" + Code + " by " + user + " in " + DateTime.Now.ToLongDateString());
            return new Rotativa.ViewAsPdf("PDF", ListCourse)
            {
                CustomSwitches = customSwitches,
                //PageMargins = new Margins { Bottom = 5, Top = 5 },
            };
        }
        #endregion
    }
}
