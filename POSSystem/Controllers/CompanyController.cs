using POSData;
using POSSystem.Models;
using System.Linq;
using System.Web.Mvc;
//using System.Collections.Generic;
namespace POSSystem.Controllers
{
    public class CompanyController : Controller
    {
        //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CompanyController));
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
        public CompanyController()
        {
            Context = new POSSystemEntities();
        }
        public JsonResult GetCompanyData()
        {
            var data = Context.Companies.FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public class ViewModelCompany
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Website { get; set; }
            public string Note { get; set; }
            //public Nullable<byte> ExpirePeriod { get; set; }
            public string ImageUrl { get; set; }
            public string Method { get; set; }
        }
        public JsonResult SaveUpdateCompany(ViewModelCompany m)
        {
            ResultMessage result = new ResultMessage();
            var comObj = Context.Companies.FirstOrDefault();
            if (comObj == null)
            {
                m.Method = "add";
            }
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    Company com = new Company();
                    com.Name = m.Name;
                    com.Title = m.Title;
                    com.Phone = m.Phone;
                    com.Address = m.Address;
                    com.Email = m.Email;
                    com.Website = m.Website;
                    com.Note = m.Note;
                    com.ImageUrl = m.ImageUrl;
                    context.Companies.Add(com);
                    context.SaveChanges();
                }
                result.message = "Company data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    var com = context.Companies.Where(x => x.Id == m.Id).FirstOrDefault();
                    if (com != null)
                    {
                        com.Name = m.Name;
                        com.Title = m.Title;
                        com.Phone = m.Phone;
                        com.Address = m.Address;
                        com.Email = m.Email;
                        com.Website = m.Website;
                        com.Note = m.Note;
                        com.ImageUrl = m.ImageUrl;
                        context.SaveChanges();
                        result.message = "Company has been updated successfully.";
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
