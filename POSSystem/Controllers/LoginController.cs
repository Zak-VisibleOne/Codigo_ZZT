using POSSystem.Models;
using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using POSData;
using System.IO;
//using KZ.Helper.ReCaptchaV3;

namespace POSSystem.Controllers
{
    public class LoginController : Controller
    {
        private const string key = "qaz123!@@)(*";
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LoginController));
        #region
        public ActionResult Index()
        {
            //logger.Info("Call LoginController Index page.");
            if (Session.IsNewSession || HttpContext.Session["isLogin"] == null)
            {
                return View();
            }
            else
            {
                if (Request.QueryString["code"] != null)
                {
                }
                return Redirect("Dashboard");
            }
        }
        public ActionResult Failed()
        {
            return View("Js_error");
        }
        string GetIpAddress()
        {
            var ipAddress = string.Empty;
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (!string.IsNullOrEmpty(Request.UserHostAddress))
            {
                ipAddress = Request.UserHostAddress;
            }
            return ipAddress;
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateGoogleCaptcha]
        public ActionResult auth(string username, string password, string CaptchaText)
        {
            LoginModel lm = new LoginModel();
            SharedModel sm = new SharedModel();
            ResultMessage rm = new ResultMessage();

            string IPAddress = string.Empty;
            string SearchName = string.Empty;

            HttpContext curContext = System.Web.HttpContext.Current;
            String strHostName = curContext.Request.UserHostAddress.ToString();
            IPAddress = Dns.GetHostAddresses(strHostName).GetValue(0).ToString();

            int maxUser = 100;
            int CurrentLogin = lm.CurrentLogin(username);
            var isLoggedIn = lm.isLoggedIn(username);

            string errorMessage = String.Empty;
            var result = lm.auth(username, Encrypt(password), password, ref errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                rm.result = "false";
                rm.message = errorMessage;
                goto ExitFun;
            }
            lm.updateLog(IPAddress, Session.SessionID, username);

            //var message = ValidateReCaptcha();
            var message = "success";
            if (message == "success")
            {
                if (CurrentLogin < maxUser)
                {
                    if (result.Count == 1)
                    {
                        Session["isLogin"] = true;
                        Session["User"] = result[0].UserCode;
                        Session["Uname"] = result[0].UserName;
                        Session["IsLoginLimit"] = result[0].IsLoginLimit;
                        Session["Company"] = result[0].DefaultCompany;
                        using (var db = new POSSystemEntities())
                        {
                            rm.result = "true";
                        }
                        rm.message = "Login success";
                    }
                    else
                    {
                        rm.result = "false";
                        rm.message = "Username Or Password not match";
                    }
                }
                else
                {
                    rm.result = "false";
                    rm.message = "User already has max user active login, please wait until other user logout.";
                }
            }
            else
            {
                rm.result = "false";
                rm.message = message;
            }

        ExitFun:
            return Json(new { rm }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            //logger.Info("Call Logout");
            LoginModel lm = new LoginModel();
            if (HttpContext.Session["User"] != null)
            {
                var User = HttpContext.Session["User"].ToString();
                lm.UpdateState(User, 0);
            }
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        public JsonResult IsSessionAvailable()
        {
            return Json(Session["User"] != null, JsonRequestBehavior.AllowGet);
        }
        public int Encrypt(string Password)
        {
            int newPassword = 0;
            int pLen = Password.Length;
            for (int i = 0; i < pLen; i++)
            {
                newPassword = newPassword + ((Encoding.ASCII.GetBytes(Password.ToString().Substring(i, 1))[0] - pLen + i + 1) * ((i + 1) * (i + 1) * (i + 1)));
            }
            return newPassword;
        }
        #endregion
    }
}