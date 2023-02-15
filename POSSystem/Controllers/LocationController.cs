using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
namespace POSSystem.Controllers
{
    public class LocationController : Controller
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
        public LocationController()
        {
            Context = new POSSystemEntities();
        }
        public JsonResult GetLocationList()
        {
            var data = new List<object[]>();
            var list = Context.Locations.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (Location l in list)
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
        public JsonResult GetLocationByID(int ID)
        {
            var data = Context.Locations.Where(x => x.ID == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteLocByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var locObj = context.Locations.FirstOrDefault(x => x.ID == ID);
                context.Locations.Remove(locObj);
                context.SaveChanges();
            }
            result.result = "success";
            result.message = "Data successfully deleted";
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public class ViewModelLocation
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Alias { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdateLocation(ViewModelLocation m)
        {
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    Location loc = new Location();
                    loc.Alias = m.Alias;
                    loc.Name = m.Name;
                    context.Locations.Add(loc);
                    context.SaveChanges();
                }
                result.message = "Location save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var loc = context.Locations.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (loc != null)
                    {
                        loc.Alias = m.Alias;
                        loc.Name = m.Name;
                        context.SaveChanges();
                        result.message = "Location has been updated successfully.";
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
