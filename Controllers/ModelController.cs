using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSData;
namespace POSSystem.Controllers
{
    public class ModelController : Controller
    {
        public ActionResult Index()
        {
            //var studList = GetList();
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        //private List<Course_Table> GetList()
        //{
        //    List<Course_Table> stuDet = new List<Course_Table>();
        //    using (var context = new CityUEntities())
        //    {
        //        stuDet = context.Course_Table.ToList();
        //    }
        //    return stuDet;
        //}
        public ActionResult PDFUsingRotativa(string Code)
        {
            //var studList = GetList(); //Get Student List
            string header = Server.MapPath("~/SQL/Header.html");//Path of Header.html File
            string footer = Server.MapPath("~/SQL/Footer.html");//Path of Footer.html File
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"0\" " +
                                   "--footer-html \"{1}\" " +
                                   "--footer-spacing \"10\" " +
                                   "--page-offset 0 --footer-center [page] --footer-font-size 8 " + //get paging in center of footer
                                   "--header-font-size \"10\" ", header, footer);
            ViewBag.Code = Code;
            //Show View as PDF
            return new Rotativa.ViewAsPdf("Index", Code)//studList
            {
                CustomSwitches = customSwitches
            };
        }
        //public ActionResult PrintViewToPdf()
        //{
        //    var report = new ActionAsPdf("GeneratePdf");
        //    return report;
        //}

        //public ActionResult GeneratePdf()
        //{
        //    string Code = "CA1166";
        //    return Content(html, "text/html");
        //}
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