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
    public class TownshipController : Controller
    {
        //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(TspController));
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
        public TownshipController()
        {
            Context = new POSSystemEntities();
        }
        public JsonResult GetList()
        {
            var data = new List<object[]>();
            var list = Context.Townships.ToList();
            var list_count = list.Count;
            string cty = "";
            if (list.Count > 0)
            {
                foreach (Township l in list)
                {
                    var ctyObj = Context.Cities.Where(x => x.CtyId == l.CtyId).FirstOrDefault();
                    if (ctyObj != null)
                    {
                        cty = ctyObj.CtyName;
                    }
                    else
                    {
                        cty = "";
                    }
                    data.Add(new object[] {
                        l.TspId
                       ,l.TspName
                       ,cty
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
            var data = Context.Townships.Where(x => x.TspId == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var obj = context.Townships.FirstOrDefault(x => x.TspId == ID);
                context.Townships.Remove(obj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public class ViewModelCty
        {
            public int TspId { get; set; }
            public int CtyId { get; set; }
            public string TspName { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdate(ViewModelCty m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    Township data = new Township();
                    data.TspName = m.TspName ?? "";
                    data.CtyId = m.CtyId;
                    context.Townships.Add(data);
                    context.SaveChanges();
                }
                result.message = "Data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var data = context.Townships.Where(x => x.TspId == m.TspId).FirstOrDefault();
                    if (data != null)
                    {
                        data.TspName = m.TspName ?? "";
                        data.CtyId = m.CtyId;
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
