using POSSystem.Models;
using POSData;
using System.Linq;
using System.Web.Mvc;
namespace POSSystem.Controllers
{
    public class ReportingController : Controller
    {
        // GET: Reporting
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ActionResult GetReportList()
        {
            var result = new ResultMessage();
            using (var db = new POSSystemEntities())
            {
                var rptList = db.ReportMaintenances.Where(y => y.Deleted == false).GroupBy(x => x.MainReport).Select(x => x.Select(r => r).OrderBy(t => t.SrNo).ToList()).ToList();
                if (rptList.Count > 0)
                {
                    result.data = rptList;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
