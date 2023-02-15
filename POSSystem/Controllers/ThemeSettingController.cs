using POSSystem.Models;
using POSData;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
namespace POSSystem.Controllers
{
    public class ThemeSettingController : Controller
    {
        private POSSystemEntities Context { get; set; }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ThemeSettingController));
        public ThemeSettingController()
        {
            Context = new POSSystemEntities();
        }
        // GET: ThemeSetting
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ActionResult Theme()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        [ValidateInput(false)]
        public JsonResult SaveData(ThemeInfo info)
        {
            var data = new ResultMessage();
            using (var db = new POSSystemEntities())
            {
                var theme = new ThemeSetting();
                if (info.Method == "add")
                {                    
                    theme.Data = info.CssData;
                    theme.Javascript = info.JsData;
                    db.ThemeSettings.Add(theme);
                }
                else
                {
                    theme = db.ThemeSettings.FirstOrDefault(x=>x.ID==info.ID);
                    if (theme != null)
                    {
                        theme.Data = info.CssData;
                        theme.Javascript = info.JsData;
                    }                    
                }                
                db.SaveChanges();
                data.result = "success";
                data.message = "Save successfully .";
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetScript()
        {
            var data = new ResultMessage();
            using (var db = new POSSystemEntities())
            {
                var result = db.ThemeSettings.FirstOrDefault();
                if (result != null)
                {
                    data.data = new
                    {
                        CSS=result.Data,
                        JavaScript=result.Javascript
                    };
                }
                data.result = "success";
                data.message = "Save successfully .";
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAdditionalData(int ID)
        {
            var result = new ResultMessage();
            using (var db = new POSSystemEntities())
            {
                var data = db.ThemeSettings.FirstOrDefault(x=>x.ID==ID);
                if (data != null)
                {
                    db.ThemeSettings.Remove(data);
                    db.SaveChanges();
                    result.result = "success";
                    result.message = "Deleted successful .";
                }
                else
                {
                    result.result = "error";
                    result.message = "Data not found .";
                }               
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAddtionalDataList(int start = 0, int length = 10, string search = "", string order = "", int draw = 0)
        {
            var data = new List<object[]>();
            var totalCount = 0;
            using (var db = new POSSystemEntities())
            {
                var additionalData = db.ThemeSettings.ToList();
                additionalData = (order == "asc" ? additionalData.OrderBy(x => x.ID).Where(x => x.Data.ToLower().Contains(search.ToLower()))
                    : additionalData.OrderByDescending(x => x.ID)).Where(x => x.Data.ToLower().Contains(search.ToLower())).ToList();
                totalCount = additionalData.Count;
                var dataList = additionalData.Skip(start).Take(length).ToList();
                foreach (var l in dataList)
                {
                    data.Add(new object[] {
                         l.ID
                        , l.Data??""
                        , l.Javascript ?? ""
                        , ""
                    });
                }
            }
            return Json(
                    new
                    {
                        draw = draw,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount,
                        data = data
                    }, JsonRequestBehavior.AllowGet
            );
        }
    }    
    public class ThemeInfo
    {
        public int ID { get; set; }
        public string CssData { get; set; }
        public string JsData { get; set; }
        public string Method { get; set; }
    }
}