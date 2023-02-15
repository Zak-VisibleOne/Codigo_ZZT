using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
namespace POSSystem.Controllers
{
    public class DiscountTypeController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CategoryController));
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
        public DiscountTypeController()
        {
            Context = new POSSystemEntities();
        }
        public JsonResult GetDiscountTypeList()
        {
            var data = new List<object[]>();
            var list = Context.DiscountTypes.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (DiscountType l in list)
                {
                    data.Add(new object[] {
                        l.Name
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
        public JsonResult GetDiscountTypeByID(int ID)
        {
            var data = Context.DiscountTypes.Where(x => x.ID == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDiscountTypeByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var disObj = context.DiscountTypes.FirstOrDefault(x => x.ID == ID);
                context.DiscountTypes.Remove(disObj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public class ViewModelDiscountType
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdateDiscountType(ViewModelDiscountType m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    DiscountType dis = new DiscountType();
                    dis.Name = m.Name;
                    context.DiscountTypes.Add(dis);
                    context.SaveChanges();
                }
                result.message = "Discount type save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var dis = context.DiscountTypes.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (dis != null)
                    {
                        dis.Name = m.Name;
                        context.SaveChanges();
                        result.message = "Discount type has been updated successfully.";
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
