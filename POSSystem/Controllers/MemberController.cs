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
        public MemberController()
        {
            Context = new POSSystemEntities();
        }
        public class ViewModelDepartment
        {
            public int ID { get; set; }
            public string DepName { get; set; }
            public string DepLocation { get; set; }
            public string Method { get; set; }
        }
        public JsonResult GetDepartmentList()
        {
            var data = new List<object[]>();
            var list = Context.Departments.ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (Department l in list)
                {
                    data.Add(new object[] {
                        l.ID
                       ,l.DepName
                       ,l.DepLocation
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
        public JsonResult GetDepartmentByID(int ID)
        {
            var data = Context.Departments.Where(x => x.ID == ID).FirstOrDefault();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDepartmentByID(int ID)
        {
            ResultMessage result = new ResultMessage();
            result.result = "success";
            result = DelDepartmentByID(ID);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ResultMessage DelDepartmentByID(int id)
        {
            ResultMessage rm = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                context.Database.ExecuteSqlCommand(@"Delete ace_report.Department where ID = {0}", new object[] { id });
                context.SaveChanges();
                rm.message = "Data successfully deleted";
                rm.result = "success";
                string user = (Session["User"] ?? "").ToString();
                logger.Info("Delete department with id no " + id + " by " + user);
            }
            return rm;
        }
        public JsonResult SaveUpdateDepartment(ViewModelDepartment m)
        {
            string user = (Session["User"] ?? "").ToString();
            ResultMessage result = new ResultMessage();
            if (m.Method == "add")
            {
                using (var context = new POSSystemEntities())
                {
                    Department dep = new Department();
                    dep.DepName = m.DepName ?? "";
                    dep.DepLocation = m.DepLocation ?? "";
                    dep.CreatedDate = DateTime.Now;
                    dep.CreatedUser = (Session["User"] ?? "").ToString();
                    context.Departments.Add(dep);
                    context.SaveChanges();
                    logger.Info("Department( " + m.DepName + ") created by user " + user + " in " + DateTime.Now.ToString());
                }
                result.message = "New department data save successfully.";
                result.result = "success";
            }
            else
            {
                using (var context = new POSSystemEntities())
                {
                    Department dep = new Department();
                    dep = context.Departments.Where(x => x.ID == m.ID).FirstOrDefault();
                    if (dep != null)
                    {
                        dep.DepName = m.DepName ?? "";
                        dep.DepLocation = m.DepLocation ?? "";
                        context.SaveChanges();
                        result.message = "Department has been updated successfully.";
                        result.result = "success";
                        logger.Info("System user: " + user + " add update department with id no " + m.ID + ".");
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
