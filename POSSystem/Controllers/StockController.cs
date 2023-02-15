using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using POSSystem.Utils;
namespace POSSystem.Controllers
{
    public class StockController : Controller
    {
        private POSSystemEntities Context { get; set; }
        public string SQLPath = "Stock/";
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public StockController()
        {
            Context = new POSSystemEntities();
        }
        public class ViewModelStock
        {
            public int ID { get; set; }
            public string StockCode { get; set; }
            public string StockDescription { get; set; }
            public string ImageUrl { get; set; }
            public Nullable<int> CategoryID { get; set; }
            public Nullable<double> SalePrice { get; set; }
            public Nullable<double> PurchasePrice { get; set; }
            public string Remark { get; set; }
            public string Method { get; set; }
        }
        public JsonResult GetStockList()
        {
            var data = new List<object[]>();
            var list = Context.Stocks.ToList();
            //sqlDescriptor sd = new sqlDescriptor();
            //List<ViewModelM> list = new List<ViewModelM>();
            //list = sd.GetDataByQuery<ViewModelM>(SQLPath + "GetStockList", new object[] { 0 }).ToList();
            var list_count = list.Count;
            string category = string.Empty;
            if (list.Count > 0)
            {
                foreach (Stock l in list)
                {
                    var catObj = Context.Categories.Where(x => x.ID == l.CategoryID).FirstOrDefault();
                    if (catObj != null)
                    {
                        category = catObj.CategoryName;
                    }
                    data.Add(new object[] {
                        l.StockCode
                       ,l.StockDescription
                       ,l.SalePrice ?? 0
                       ,l.PurchasePrice ?? 0
                       ,category
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
        public ActionResult getCategory()
        {
            POSSystemEntities db = new POSSystemEntities();
            return Json(db.Categories.Select(x => new
            {
                ID = x.ID,
                CategoryName = x.CategoryName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public JsonResult SaveUpdateStock(ViewModelStock m)
        {
            ResultMessage result = new ResultMessage();

            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    Stock data = new Stock();
                    data.StockCode = m.StockCode ?? "";
                    data.StockDescription = m.StockDescription ?? "";
                    data.ImageUrl = m.ImageUrl ?? "";
                    data.CategoryID = m.CategoryID ?? 0;
                    data.SalePrice = m.SalePrice ?? 0;
                    data.PurchasePrice = m.PurchasePrice ?? 0;
                    data.Remark = "";
                    data.CreatedDate = DateTime.Now;
                    data.CreatedBy = "Admin";
                    context.Stocks.Add(data);
                    context.SaveChanges();
                }
                result.message = "Data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var data = context.Stocks.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (data != null)
                    {
                        data.StockCode = m.StockCode ?? "";
                        data.StockDescription = m.StockDescription ?? "";
                        data.ImageUrl = m.ImageUrl ?? "";
                        data.CategoryID = m.CategoryID ?? 0;
                        data.SalePrice = m.SalePrice ?? 0;
                        data.PurchasePrice = m.PurchasePrice ?? 0;
                        data.Remark = "";
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
        public JsonResult GetStockDataByID(int ID)
        {
            var data = GetData(ID);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public ViewModelStock GetData(int ID)
        {
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ViewModelStock>(SQLPath + "GetStockByID", new object[] { ID }).FirstOrDefault();
            return result;
        }
        public JsonResult DeleteStockByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var obj = context.Stocks.FirstOrDefault(x => x.ID == ID);
                context.Stocks.Remove(obj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}
