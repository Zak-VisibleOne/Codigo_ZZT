using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace POSSystem.Controllers
{
    public class SupplierController : Controller
    {
        //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CategoryController));
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
        public SupplierController()
        {
            Context = new POSSystemEntities();
        }
        public ActionResult getTsp()
        {
            POSSystemEntities db = new POSSystemEntities();
            return Json(db.Townships.Select(x => new
            {
                TspId = x.TspId,
                TspName = x.TspName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCty()
        {
            POSSystemEntities db = new POSSystemEntities();
            return Json(db.Cities.Select(x => new
            {
                CtyId = x.CtyId,
                CtyName = x.CtyName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetList()
        {
            var data = new List<object[]>();
            var list = Context.Suppliers.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (Supplier l in list)
                {
                    data.Add(new object[] {
                        l.SrNo
                       ,l.SupName
                       ,"Tsp"
                       ,""
                       ,l.SupId
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
            var data = Context.Suppliers.Where(x => x.SupId == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDataByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var catObj = context.Suppliers.FirstOrDefault(x => x.SupId == ID);
                context.Suppliers.Remove(catObj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public class ViewModelSupplier
        {
            public int SupId { get; set; }
            public Nullable<int> SrNo { get; set; }
            public string SupName { get; set; }
            public int CtyId { get; set; }
            public int TspId { get; set; }
            public string Address1 { get; set; }
            public string Phone1 { get; set; }
            public string Email { get; set; }
            public string Website { get; set; }
            public string Gender { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdate(ViewModelSupplier m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    int no = 0;
                    string strCmd = @"select isnull(Max(SrNo),0) as MaxNo from Supplier";
                    no = Context.Database.SqlQuery<int>(strCmd, new object[] { }).FirstOrDefault();
                    Supplier data = new Supplier();
                    data.SrNo = no + 1;
                    data.SupName = m.SupName ?? "";
                    data.CtyId = m.CtyId;
                    data.TspId = m.TspId;
                    data.Address1 = m.Address1 ?? "";
                    data.Phone1 = m.Phone1 ?? "";
                    data.Email = m.Email;
                    data.WebSite = m.Website;
                    data.Gender = m.Gender;
                    context.Suppliers.Add(data);
                    context.SaveChanges();
                }
                result.message = "Data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var data = context.Suppliers.Where(x => x.SupId == m.SupId).FirstOrDefault();
                    if (data != null)
                    {
                        data.SupName = m.SupName ?? "";
                        data.CtyId = m.CtyId;
                        data.TspId = m.TspId;
                        data.Address1 = m.Address1 ?? "";
                        data.Phone1 = m.Phone1 ?? "";
                        data.Email = m.Email;
                        data.WebSite = m.Website;
                        data.Gender = m.Gender;
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
        public JsonResult DeleteMultiple(List<int> Ids)
        {
            ResultMessage result = new ResultMessage();
            if (Ids.Count > 0)
            {
                using (var dbContext = new POSSystemEntities())
                {
                    for (int i = 0; i < Ids.Count; i++)
                    {
                        int id = Ids[i];
                        var obj = dbContext.Suppliers.SingleOrDefault(x => x.SupId == id);
                        if (obj != null)
                        {
                            dbContext.Suppliers.Remove(obj);
                            dbContext.SaveChanges();
                        }
                    }
                    result.message = "List has been deleted successfully.";
                    result.result = "success";
                }
            }
            else
            {
                result.message = "There is no data for delete.";
                result.result = "info";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}
