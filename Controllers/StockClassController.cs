using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
namespace POSSystem.Controllers
{
    public class StockClassController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LocationController));
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
        public StockClassController()
        {
            Context = new POSSystemEntities();
        }
        public JsonResult GetStockClassList()
        {
            var data = new List<object[]>();
            var list = Context.StockClasses.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (StockClass l in list)
                {
                    data.Add(new object[] {
                        l.Alias
                       ,l.Name
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
        public JsonResult GetStockClassByID(int ID)
        {
            var data = Context.StockClasses.Where(x => x.ID == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteStockClassByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var classObj = context.StockClasses.FirstOrDefault(x => x.ID == ID);
                context.StockClasses.Remove(classObj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public class ViewModelStockClass
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Alias { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdateStockClass(ViewModelStockClass m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    StockClass stockclass = new StockClass();
                    stockclass.Alias = m.Alias;
                    stockclass.Name = m.Name;
                    context.StockClasses.Add(stockclass);
                    context.SaveChanges();
                }
                result.message = "Stock class save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var loc = context.StockClasses.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (loc != null)
                    {
                        loc.Alias = m.Alias;
                        loc.Name = m.Name;
                        context.SaveChanges();
                        result.message = "Stock class has been updated successfully.";
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
