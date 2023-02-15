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
    public class MemberController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MemberController));
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
        public MemberController()
        {
            Context = new POSSystemEntities();
        }
        public class ViewModelMember
        {
            public int ID { get; set; }
            public string MemberCode { get; set; }
            public string MemberName { get; set; }
            public string MobileNumber { get; set; }
            public Nullable<int> TotalPoint { get; set; }
            public Nullable<double> TotalPurchaseAmount { get; set; }
            public string Method { get; set; }
        }
        public JsonResult GetMemberList()
        {
            var data = new List<object[]>();
            var list = Context.Members.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (Member l in list)
                {
                    data.Add(new object[] {
                        l.MemberCode
                       ,l.MemberName
                       ,l.MobileNumber
                       ,l.TotalPoint
                       , ""
                       ,l.ID
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
        public JsonResult GetMemberByID(int ID)
        {
            var data = Context.Members.Where(x => x.ID == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteMemberByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                context.Database.ExecuteSqlCommand(@"Delete Member where ID = {0}", new object[] { ID });
                context.SaveChanges();
                result.message = "Data successfully deleted";
                result.result = "success";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveUpdateMember(ViewModelMember m)
        {
            string user = (Session["User"] ?? "").ToString();
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    Member member = new Member();
                    member.MemberCode = m.MemberCode ?? "";
                    member.MemberName = m.MemberName ?? "";
                    member.MobileNumber = m.MobileNumber ?? "";
                    member.TotalPoint = m.TotalPoint;
                    member.TotalPurchaseAmount = m.TotalPurchaseAmount;
                    context.Members.Add(member);
                    context.SaveChanges();
                }
                result.message = "New member data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var data = context.Members.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (data != null)
                    {
                        data.MemberCode = m.MemberCode ?? "";
                        data.MemberName = m.MemberName ?? "";
                        data.MobileNumber = m.MobileNumber ?? "";
                        data.TotalPoint = m.TotalPoint;
                        data.TotalPurchaseAmount = m.TotalPurchaseAmount;
                        context.SaveChanges();
                        result.message = "Member has been updated successfully.";
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
