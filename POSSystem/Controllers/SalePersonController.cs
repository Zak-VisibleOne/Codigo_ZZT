using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace POSSystem.Controllers
{
    public class SalePersonController : Controller
    {
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
        public SalePersonController()
        {
            Context = new POSSystemEntities();
        }
        public JsonResult GetSalePersonList()
        {
            var data = new List<object[]>();
            var list = Context.SalePersons.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (SalePerson l in list)
                {
                    data.Add(new object[] {
                        l.SalePersonCode
                       ,l.SalePersonName
                       ,l.Address
                       ,l.Mobile
                       ,""
                       ,l.ID
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
        public JsonResult GetSalePersonByID(int ID)
        {
            var data = Context.SalePersons.Where(x => x.ID == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSalePerson(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var saleObj = context.SalePersons.FirstOrDefault(x => x.ID == ID);
                context.SalePersons.Remove(saleObj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public class ViewModelSalePerson
        {
            public int ID { get; set; }
            public string SalePersonCode { get; set; }
            public string SalePersonName { get; set; }
            public string Address { get; set; }
            public string Mobile { get; set; }
            public string Remarks { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdateSalePerson(ViewModelSalePerson m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    SalePerson data = new SalePerson();
                    data.SalePersonCode = m.SalePersonCode;
                    data.SalePersonName = m.SalePersonName;
                    data.Address = m.Address;
                    data.Mobile = m.Mobile;
                    data.Remarks = m.Remarks;
                    context.SalePersons.Add(data);
                    context.SaveChanges();
                }
                result.message = "Data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var data = context.SalePersons.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (data != null)
                    {
                        data.SalePersonCode = m.SalePersonCode;
                        data.SalePersonName = m.SalePersonName;
                        data.Address = m.Address;
                        data.Mobile = m.Mobile;
                        data.Remarks = m.Remarks;
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
