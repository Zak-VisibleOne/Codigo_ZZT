using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
//using System;
//using System.IO;
//using OfficeOpenXml;
//using OfficeOpenXml.Table;

namespace POSSystem.Controllers
{
    public class CityController : Controller
    {
        //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CtyController));
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
        public CityController()
        {
            Context = new POSSystemEntities();
        }
        public JsonResult GetList()
        {
            var data = new List<object[]>();
            var list = Context.Cities.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (City l in list)
                {
                    data.Add(new object[] {
                        l.CtyId
                       ,l.CtyName
                       ,""
                    });
                }
            }
            return Json(
                    new
                    {
                        recordsTotal = list_count,
                        recordsFiltered = list_count,
                        data = data
                    }, JsonRequestBehavior.AllowGet
            );
        }
        public JsonResult GetDataByID(int ID)
        {
            var data = Context.Cities.Where(x => x.CtyId == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var obj = context.Cities.FirstOrDefault(x => x.CtyId == ID);
                context.Cities.Remove(obj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public class ViewModelCty
        {
            public int CtyId { get; set; }
            public string CtyName { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdate(ViewModelCty m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    City data = new City();
                    data.CtyName = m.CtyName ?? "";
                    context.Cities.Add(data);
                    context.SaveChanges();
                }
                result.message = "Data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var cat = context.Cities.Where(x => x.CtyId == m.CtyId).FirstOrDefault();
                    if (cat != null)
                    {
                        cat.CtyName = m.CtyName ?? "";
                        context.SaveChanges();
                        result.message = "Data has been updated successfully.";
                        result.result = "success";
                    }
                    else
                    {
                        result.message = "There is no data to update.";
                        result.result = "info";
                    }
                }
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}
