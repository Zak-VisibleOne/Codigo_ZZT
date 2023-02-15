using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
//using System.Collections.Generic;
namespace POSSystem.Controllers
{
    public class PaymentReceiveController : Controller
    {
        //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(PaymentReceiveController));
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
        public PaymentReceiveController()
        {
            Context = new POSSystemEntities();
        }
    }
}
