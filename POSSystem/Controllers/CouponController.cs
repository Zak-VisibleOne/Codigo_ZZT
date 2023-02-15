using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
//using System.Data.Entity.Validation;
using System;
using System.Data;

namespace POSSystem.Controllers
{
    public class CouponController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DepartmentController));
        private POSSystemEntities Context { get; set; }
        //Index page
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public CouponController()
        {
            Context = new POSSystemEntities();
        }
        public class ViewModelCoupon
        {
            public int ID { get; set; }
            public string CouponCode { get; set; }
            public string CouponName { get; set; }
            public string QRCodeImageUrl { get; set; }
            public Nullable<int> AvailableQty { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public Nullable<double> DiscountAmount { get; set; }
            public string Method { get; set; }
        }
        public JsonResult GetCouponList()
        {
            var data = new List<object[]>();
            var list = Context.Coupons.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (Coupon l in list)
                {
                    data.Add(new object[] {
                        l.CouponCode
                       ,l.CouponName
                       ,l.AvailableQty
                       ,Convert.ToDateTime(l.StartDate).ToString("dd/MM/yyyy")
                       ,Convert.ToDateTime(l.EndDate).ToString("dd/MM/yyyy")
                       ,l.DiscountAmount
                       , ""
                    });
                }
            }
            return Json(
                    new
                    {
                        //draw = draw,
                        recordsTotal = list_count,
                        recordsFiltered = list_count,
                        data = data
                    }, JsonRequestBehavior.AllowGet
            );
        }
        public JsonResult GetCouponByID(int ID)
        {
            var data = Context.Coupons.Where(x => x.ID == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCoupontByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                context.Database.ExecuteSqlCommand(@"Delete Coupon where ID = {0}", new object[] { ID });
                context.SaveChanges();
                result.message = "Data successfully deleted";
                result.result = "success";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveUpdateDepartment(ViewModelCoupon m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    Coupon coupon = new Coupon();
                    coupon.CouponCode = m.CouponCode ?? "";
                    coupon.CouponName = m.CouponName ?? "";
                    coupon.QRCodeImageUrl = "/";
                    coupon.AvailableQty = m.AvailableQty;
                    coupon.StartDate = Convert.ToDateTime(m.StartDate);
                    coupon.EndDate = Convert.ToDateTime(m.EndDate);
                    coupon.DiscountAmount = m.DiscountAmount;
                    context.Coupons.Add(coupon);
                    context.SaveChanges();
                }
                result.message = "New coupon data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var coupon = context.Coupons.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (coupon != null)
                    {
                        coupon.CouponCode = m.CouponCode ?? "";
                        coupon.CouponName = m.CouponName ?? "";
                        coupon.QRCodeImageUrl = "/";
                        coupon.AvailableQty = m.AvailableQty;
                        coupon.StartDate = Convert.ToDateTime(m.StartDate);
                        coupon.EndDate = Convert.ToDateTime(m.EndDate);
                        coupon.DiscountAmount = m.DiscountAmount;
                        context.SaveChanges();
                        result.message = "Coupon has been updated successfully.";
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
