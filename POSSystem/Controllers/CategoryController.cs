using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
namespace POSSystem.Controllers
{
    public class CategoryController : Controller
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
        public CategoryController()
        {
            Context = new POSSystemEntities();
        }
        public JsonResult GetCatList()
        {
            var data = new List<object[]>();
            var list = Context.Categories.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (Category l in list)
                {
                    data.Add(new object[] {
                        l.CategoryCode
                       ,l.CategoryName
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
        public JsonResult GetCatByID(int ID)
        {
            var data = Context.Categories.Where(x => x.ID == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCatByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var catObj = context.Categories.FirstOrDefault(x => x.ID == ID);
                context.Categories.Remove(catObj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public class ViewModelCat
        {
            public int ID { get; set; }
            public string CategoryCode { get; set; }
            public string CategoryName { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdateCat(ViewModelCat m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    Category cat = new Category();
                    cat.CategoryCode =m.CategoryCode;
                    cat.CategoryName = m.CategoryName;
                    context.Categories.Add(cat);
                    context.SaveChanges();
                }
                result.message = "Category data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var cat = context.Categories.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (cat != null)
                    {
                        cat.CategoryName = m.CategoryName;
                        context.SaveChanges();
                        result.message = "Category has been updated successfully.";
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
