using System.Web.Mvc;
namespace POSSystem.Controllers
{
    public class StockInoutController : Controller
    {
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
    }
}