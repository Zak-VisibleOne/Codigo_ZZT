using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Data;
using System.Configuration;
using POSSystem.Utils;
namespace POSSystem.Controllers
{
    public class PurchaseController : Controller
    {
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private POSSystemEntities Context { get; set; }
        public string SQLPath = "Purchase/";
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ActionResult InvoiceSample()
        {
            return View();
        }
        public PurchaseController()
        {
            Context = new POSSystemEntities();
        }
        #region DropDown
        public class ViewModelDropdown
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
        public class dropDown
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        public ActionResult getSupplier()
        {
            sqlDescriptor sd = new sqlDescriptor();
            var list = sd.GetDataByQuery<dropDown>(SQLPath + "GetSupplierList", new object[] { }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSalePerson()
        {
            sqlDescriptor sd = new sqlDescriptor();
            var list = sd.GetDataByQuery<dropDown>(SQLPath + "GetSalePersonList", new object[] { }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public class ViewModelPurchaseHeader
        {
            public int ID { get; set; }
            public int LocationID { get; set; }
            public string PurchaseDate { get; set; }
            public string InvoiceNo { get; set; }
            public int SupID { get; set; }
            public int SalesPersonID { get; set; }
            public Nullable<double> TotalAmount { get; set; }
            public Nullable<double> Discount { get; set; }
            public Nullable<double> PaidAmount { get; set; }
            public Nullable<double> Balance { get; set; }
            public string Method { get; set; }
        }
        public JsonResult GetPurchaseList()
        {
            var data = new List<object[]>();
            sqlDescriptor sd = new sqlDescriptor();
            var list = Context.PurchaseHeaders.ToList();
            //List<ViewModelSaleHeader> list = new List<ViewModelSaleHeader>();
            //list = sd.GetDataByQuery<ViewModelSaleHeader>(SQLPath + "GetSaleList", new object[] { "" }).ToList();
            var list_count = list.Count;
            string name = string.Empty;
            if (list.Count > 0)
            {
                foreach (PurchaseHeader l in list)
                {
                    var custObj = Context.Suppliers.Where(x => x.SupId == l.SupID).FirstOrDefault();
                    if (custObj != null)
                    {
                        name = custObj.SupName;
                    }
                    data.Add(new object[] {
                       l.InvoiceNo
                       ,name
                       ,Convert.ToDateTime(l.PurchaseDate).ToString("dd/MM/yyyy")
                       ,l.TotalAmount ?? 0
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
        #region Get Header Data
        public JsonResult GetPurchaseHeader(int ID)
        {
            var data = PurchaseHeader(ID);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public ViewModelPurchaseHeader PurchaseHeader(int ID)
        {
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ViewModelPurchaseHeader>("Purchase/GetPurchaseHeader", new object[] { ID }).FirstOrDefault();
            return result;
        }
        #endregion
        public JsonResult GetSupplierByID(int ID)
        {
            var data = Context.Suppliers.Where(x => x.SupId == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GenerateInvoice()
        {
            string numbers = "1234567890";
            string characters = numbers;
            int length = 10;
            string id = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (id.IndexOf(character) != -1);
                id += character;
            }
            string InvoiceNo = "Inv" + id;
            return Json(new { InvoiceNo }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPurchaseDetaiList(int HeaderID)
        {
            var data = new List<object[]>();
            sqlDescriptor sd = new sqlDescriptor();
            //var list = sd.GetDataByQuery<VehDetailListingModel>(SQLPath + "GetBlogDetailListing", new object[] { BookNo }).ToList();
            var list = Context.PurchaseDetails.Where(x => x.RefHeaderID == HeaderID).ToList();
            var list_count = list.Count;
            string Desc = "";
            string code = "";
            if (list.Count > 0)
            {
                foreach (PurchaseDetail l in list)
                {
                    var stockObj = Context.Stocks.Where(x => x.ID == l.StockID).FirstOrDefault();
                    if (stockObj != null)
                    {
                        Desc = stockObj.StockDescription ?? "";
                        code = stockObj.StockCode;
                    }
                    data.Add(new object[] {
                         l.SrNo ?? 0
                        ,code
                        ,Desc
                        ,l.Qty ?? 0
                        ,l.PurchasePrice ?? 0
                        ,l.Amount
                        ,""
                        ,l.ID
                        ,l.StockID
                        ,l.RefHeaderID
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
        //[ValidateInput(false)]
        public JsonResult SavePurchaseHeader(ViewModelPurchaseHeader m)
        {
            ResultMessage result = new ResultMessage();
            SharedModel sm = new SharedModel();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    if (m.PurchaseDate == null)
                    {
                        m.PurchaseDate = DateTime.Now.Date.ToShortDateString();
                    }
                    PurchaseHeader purchase = new PurchaseHeader();
                    purchase.InvoiceNo = m.InvoiceNo;
                    purchase.PurchaseDate = Convert.ToDateTime(m.PurchaseDate);
                    purchase.SupID = m.SupID;
                    purchase.LocationID = m.LocationID;
                    purchase.SalesPersonID = m.SalesPersonID;
                    purchase.TotalAmount = m.TotalAmount ?? 0;
                    purchase.Discount = m.Discount ?? 0;
                    purchase.PaidAmount = m.PaidAmount ?? 0;
                    purchase.Balance = m.Balance ?? 0;
                    purchase.CashDown = true;
                    purchase.Remark = "";
                    purchase.CrtDate = DateTime.Now;
                    context.PurchaseHeaders.Add(purchase);
                    context.SaveChanges();
                }
                result.message = "New pages save successfully.";
                result.result = "success";
            }
            else
            {
                #region update page
                using (var context = new POSSystemEntities())
                {
                    if (m.PurchaseDate == null)
                    {
                        m.PurchaseDate = DateTime.Now.Date.ToShortDateString();
                    }
                    var purchase = context.PurchaseHeaders.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (purchase != null)
                    {
                        purchase.PurchaseDate = Convert.ToDateTime(m.PurchaseDate);
                        purchase.SupID = m.SupID;
                        purchase.SalesPersonID = m.SalesPersonID;
                        purchase.TotalAmount = m.TotalAmount ?? 0;
                        purchase.Discount = m.Discount ?? 0;
                        purchase.PaidAmount = m.PaidAmount ?? 0;
                        purchase.Balance = m.Balance ?? 0;
                        purchase.UptDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
                result.message = "Update pages save successfully.";
                result.result = "success";
                #endregion
            }
            int headid = 0;
            var header = Context.PurchaseHeaders.ToList();
            if (header.Count > 0)
            {
                headid = Context.PurchaseHeaders.Max(u => u.ID);
            }
            else
            {
                headid = 1;
            }
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("HeaderID", Convert.ToString(headid));
            return Json(new { result, ret }, JsonRequestBehavior.AllowGet);
        }
        #region Delete Sale
        public JsonResult DeletePurchase(int HeaderID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                context.Database.ExecuteSqlCommand(@"Delete PurchaseHeader where ID = {0}", new object[] { HeaderID });
                context.Database.ExecuteSqlCommand(@"Delete PurchaseDetail where RefHeaderID = {0}", new object[] { HeaderID });
                context.SaveChanges();
                result.message = "Data successfully deleted";
                result.result = "success";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region SaveDetail
        public class DetailViewModel
        {
            public int ID { get; set; }
            public Nullable<int> RefHeaderID { get; set; }
            public Nullable<int> SrNo { get; set; }
            public Nullable<int> StockID { get; set; }
            public Nullable<int> Qty { get; set; }
            public Nullable<double> PurchasePrice { get; set; }
            public Nullable<double> Amount { get; set; }
            public Nullable<double> Discount { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SavePurchaseDetail(DetailViewModel m)
        {
            ResultMessage result = new ResultMessage();
            int srNo = 0;
            string strCmd = @"select isnull(Max(SrNo),0) as MaxSrNo  from PurchaseDetail where RefHeaderID = " + m.RefHeaderID ?? 0 + "";
            srNo = Context.Database.SqlQuery<int>(strCmd, new object[] { }).FirstOrDefault();
            SharedModel sm = new SharedModel();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    PurchaseDetail detail = new PurchaseDetail();
                    detail.RefHeaderID = m.RefHeaderID;
                    detail.SrNo = srNo + 1;
                    detail.StockID = m.StockID;
                    detail.Description = "";
                    detail.Qty = 1;
                    detail.PurchasePrice = m.PurchasePrice ?? 0;
                    detail.Amount = m.Amount ?? 0;
                    detail.Discount = m.Discount ?? 0;
                    context.PurchaseDetails.Add(detail);
                    context.SaveChanges();
                }
                result.message = "New save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var detail = context.PurchaseDetails.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.Qty = m.Qty;
                        detail.Amount = m.Amount ?? 0;
                        detail.Discount = m.Discount ?? 0;
                        context.SaveChanges();
                    }
                }
                result.message = "Update save successfully.";
                result.result = "success";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSaleDetail(int ID)
        {
            ResultMessage result = new ResultMessage();
            result.result = "success";
            result = DeleteDetail(ID);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ResultMessage DeleteDetail(int ID)
        {
            ResultMessage rm = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                context.Database.ExecuteSqlCommand(@"Delete PurchaseDetail where ID = {0}", new object[] { ID });
                context.SaveChanges();
                rm.message = "Data successfully deleted";
                rm.result = "success";
            }
            return rm;
        }
        #endregion
        public JsonResult GetStockList()
        {
            var data = new List<object[]>();
            var list = Context.Stocks.ToList();
            var list_count = list.Count;
            string catName = string.Empty;
            if (list.Count > 0)
            {
                foreach (Stock l in list)
                {
                    var cat = Context.Categories.Where(x => x.ID == l.CategoryID).FirstOrDefault();
                    if (cat != null)
                    {
                        catName = cat.CategoryName;
                    }
                    data.Add(new object[] {
                        l.StockCode
                       ,l.StockDescription
                       ,l.SalePrice ?? 0
                       ,l.PurchasePrice ?? 0
                       ,catName
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
        public JsonResult CalculateTotalAmount(int RefHeaderID)
        {
            var detail = Context.PurchaseDetails.Where(x => x.RefHeaderID == RefHeaderID).ToList();
            double TotalAmount = 0;
            double Balance = 0;
            if (detail.Count > 0)
            {
                foreach (var item in detail)
                {
                    TotalAmount += item.Amount ?? 0;
                }
                var head = Context.PurchaseHeaders.Where(x => x.ID == RefHeaderID).FirstOrDefault();
                if (head != null)
                {
                    head.TotalAmount = TotalAmount;
                    head.Balance = head.TotalAmount - (head.PaidAmount ?? 0) - (head.Discount ?? 0);
                    Balance = head.Balance ?? 0;
                    Context.SaveChanges();
                }
            }
            return Json(new { TotalAmount, Balance }, JsonRequestBehavior.AllowGet);
        }
    }
}
