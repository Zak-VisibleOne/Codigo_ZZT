using POSData;
using POSSystem.Utils;
using POSSystem.Models;
using System;
using System.Data;
using System.Web.Mvc;
using System.Linq;
using static POSSystem.Models.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using POSSystem.Models;
//using System.IO.Compression;
//using System.Net;

namespace POSSystem.Controllers
{
    public class SystemManagerController : Controller
    {
        SystemManagerModel systemManager;
        private POSSystemEntities Context { get; set; }
        public string user { get; set; }
        public string SQLPath = "SystemManager/";
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SystemManagerController));

        public SystemManagerController()
        {
            systemManager = new SystemManagerModel();
            Context = new POSSystemEntities();
        }
        public ActionResult Index()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            else
            {
                user = User.ToString();
            }
            return View();
        }
        public ActionResult UserGroup()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ActionResult ChangePassword()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ActionResult SettingMenu()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ActionResult ControlPanel()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public ActionResult SystemLog()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        #region User Popup
        public ActionResult Users()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            HelperController obj = new HelperController();
            //ViewData["UserGroup"] = obj.ListUserGroup("P");
            return View();
        }
        public ActionResult getUserGroup()
        {
            POSSystemEntities db = new POSSystemEntities();
            return Json(db.UserGroups.Select(x => new
            {
                Code = x.GroupCode,
                Name = x.GroupName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getUsersList()
        {
            POSSystemEntities db = new POSSystemEntities();
            return Json(db.Users.Select(x => new
            {
                ID = x.Id,
                Name = x.UserCode
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUserListing(string Code, int start = 0, int length = 10, string search = "", string order = "", int draw = 1)
        {
            var data = new List<object[]>();
            sqlDescriptor sd = new sqlDescriptor();
            List<ViewModelUserInfo> list = new List<ViewModelUserInfo>();
            if (Code == "0" || Code == "")
            {
                list = sd.GetDataByQuery<ViewModelUserInfo>(SQLPath + "GetUserListing", new object[] {
            }).ToList();
            }
            else
            {
                list = sd.GetDataByQuery<ViewModelUserInfo>(SQLPath + "GetUserListingByUserGroupID", new object[] { Code }).ToList();
            }
            var list_count = list.Count;
            if (list.Count > 0)
            {
                list = (order == "asc" ? list.OrderBy(x => x.SrNo) : list.OrderByDescending(x => x.SrNo)).ToList();
                var users = list.Where(x => x.UserName.ToLower().Contains(search.ToLower()))
                    .Skip(start).Take(length).ToList();
                foreach (ViewModelUserInfo l in users)
                {
                    data.Add(new object[] {
                         l.SrNo
                        ,l.ImageUrl
                        ,l.UserCode
                        ,l.UserName
                        ,l.UserGroupCode
                        ,l.DeletedStatus
                        ,""
                        ,l.Id
                    });
                }
            }
            return Json(
                    new
                    {
                        draw = draw,
                        recordsTotal = list_count,
                        recordsFiltered = list_count,
                        data = data
                    }, JsonRequestBehavior.AllowGet
            );
        }
        public JsonResult SaveUser(ViewModelUserInfo user)
        {
            using (var db = new POSSystemEntities())
            {
                var twoFactor = db.TwoFactorDatas.FirstOrDefault(x => x.User == user.UserCode);
                if (twoFactor != null)
                {
                    twoFactor.Email = user.Email;
                }
                else
                {
                    twoFactor = new TwoFactorData();
                    twoFactor.User = user.UserCode;
                    twoFactor.Email = user.Email;
                    twoFactor.TwoFactorEnabled = false;
                    twoFactor.TFAEmail = false;
                    twoFactor.TFAGoogle = false;
                    db.TwoFactorDatas.Add(twoFactor);
                }
                db.SaveChanges();
            }
            return Json(systemManager.SaveNewUser(user), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getUserPermissionbyID(int ID)
        {
            ResultMessage result = new ResultMessage();
            var userdata = Context.UserPermissions.Where(x => x.UserID == ID).FirstOrDefault();
            if (userdata != null)
            {
                result.result = "success";
                result.data = userdata;
            }
            else
            {
                result.result = "error";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getUserbyID(int ID)
        {
            var userdata = systemManager.UsersList().Find(x => x.Id.Equals(ID));
            return Json(userdata, JsonRequestBehavior.AllowGet);
        }
        public partial class PermissionUser
        {
            public int UserID { get; set; }
            public Nullable<bool> AllowCreateCourse { get; set; }
            public Nullable<bool> AllowUpdateCourse { get; set; }
            public Nullable<bool> AllowDeleteCourse { get; set; }
            public Nullable<bool> AllowCreateDiscipline { get; set; }
            public Nullable<bool> AllowUpdateDiscipline { get; set; }
            public Nullable<bool> AllowDeleteDiscipline { get; set; }
            public Nullable<bool> AllowCreateLevel { get; set; }
            public Nullable<bool> AllowUpdateLevel { get; set; }
            public Nullable<bool> AllowDeleteLevel { get; set; }
            public Nullable<bool> AllowCreateDepartment { get; set; }
            public Nullable<bool> AllowUpdateDepartment { get; set; }
            public Nullable<bool> AllowDeleteDepartment { get; set; }
            public Nullable<bool> AllowCreateCategory { get; set; }
            public Nullable<bool> AllowUpdateCategory { get; set; }
            public Nullable<bool> AllowDeleteCategory { get; set; }
            public Nullable<bool> AllowCreateSemester { get; set; }
            public Nullable<bool> AllowUpdateSemester { get; set; }
            public Nullable<bool> AllowDeleteSemester { get; set; }
            public Nullable<bool> AllowCreatePublication { get; set; }
            public Nullable<bool> AllowUpdatePublication { get; set; }
            public Nullable<bool> AllowDeletePublication { get; set; }
            public Nullable<bool> AllowCreateStaffView { get; set; }
            public Nullable<bool> AllowUpdateStaffView { get; set; }
            public Nullable<bool> AllowDeleteStaffView { get; set; }
            public Nullable<bool> AllowCreatePosition { get; set; }
            public Nullable<bool> AllowUpdatePosition { get; set; }
            public Nullable<bool> AllowDeletePosition { get; set; }
            public Nullable<bool> AllowCreateResearchArea { get; set; }
            public Nullable<bool> AllowUpdateResearchArea { get; set; }
            public Nullable<bool> AllowDeleteResearchArea { get; set; }
            public Nullable<bool> AllowCreateUser { get; set; }
            public Nullable<bool> AllowUpdateUser { get; set; }
            public Nullable<bool> AllowDeleteUser { get; set; }
            public Nullable<bool> AllowCreateUserGroup { get; set; }
            public Nullable<bool> AllowUpdateUserGroup { get; set; }
            public Nullable<bool> AllowDeleteUserGroup { get; set; }
        }
        public JsonResult UpdateUserPermission(PermissionUser user)
        {
            ResultMessage result = new ResultMessage();
            var userObj = Context.UserPermissions.FirstOrDefault(x => x.UserID == user.UserID);
            if (userObj != null)
            {
                userObj.AllowCreateDepartment = user.AllowCreateDepartment ?? false;
                userObj.AllowUpdateDepartment = user.AllowUpdateDepartment ?? false;
                userObj.AllowDeleteDepartment = user.AllowDeleteDepartment ?? false;
                userObj.AllowCreateCategory = user.AllowCreateCategory ?? false;
                userObj.AllowUpdateCategory = user.AllowUpdateCategory ?? false;
                userObj.AllowDeleteCategory = user.AllowDeleteCategory ?? false;
                userObj.AllowCreateUser = user.AllowCreateUser ?? false;
                userObj.AllowUpdateUser = user.AllowUpdateUser ?? false;
                userObj.AllowDeleteUser = user.AllowDeleteUser ?? false;
                userObj.AllowCreateUserGroup = user.AllowCreateUserGroup ?? false;
                userObj.AllowUpdateUserGroup = user.AllowUpdateUserGroup ?? false;
                userObj.AllowDeleteUserGroup = user.AllowDeleteUserGroup ?? false;
                userObj.CreatedDate = DateTime.Now;
                userObj.ModifiedDate = DateTime.Now;
                Context.SaveChanges();
                result.message = "User permission has been updated.";
                result.result = "success";
            }
            else
            {
                UserPermission per = new UserPermission();
                per.UserID = user.UserID;
                per.AllowCreateDepartment = user.AllowCreateDepartment ?? false;
                per.AllowUpdateDepartment = user.AllowUpdateDepartment ?? false;
                per.AllowDeleteDepartment = user.AllowDeleteDepartment ?? false;
                per.AllowCreateCategory = user.AllowCreateCategory ?? false;
                per.AllowUpdateCategory = user.AllowUpdateCategory ?? false;
                per.AllowDeleteCategory = user.AllowDeleteCategory ?? false;
                per.AllowCreateUser = user.AllowCreateUser ?? false;
                per.AllowUpdateUser = user.AllowUpdateUser ?? false;
                per.AllowDeleteUser = user.AllowDeleteUser ?? false;
                per.AllowCreateUserGroup = user.AllowCreateUserGroup ?? false;
                per.AllowUpdateUserGroup = user.AllowUpdateUserGroup ?? false;
                per.AllowDeleteUserGroup = user.AllowDeleteUserGroup ?? false;
                per.ModifiedDate = DateTime.Now;
                Context.UserPermissions.Add(per);
                Context.SaveChanges();
                result.message = "User permission has been added.";
                result.result = "success";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateUser(User user)
        {
            using (var db = new POSSystemEntities())
            {
                var twoFactor = db.TwoFactorDatas.FirstOrDefault(x => x.User == user.UserCode);
                if (twoFactor != null)
                {
                    twoFactor.Email = user.Email;
                }
                else
                {
                    twoFactor = new TwoFactorData();
                    twoFactor.User = user.UserCode;
                    twoFactor.Email = user.Email;
                    twoFactor.TwoFactorEnabled = false;
                    twoFactor.TFAEmail = false;
                    twoFactor.TFAGoogle = false;
                    db.TwoFactorDatas.Add(twoFactor);
                }
                db.SaveChanges();
            }
            return Json(systemManager.UpdateUser(user), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteUser(int ID)
        {
            return Json(systemManager.Delete(ID), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region User Group Popup
        public JsonResult UserGroupList()
        {
            return Json(systemManager.UserGroupList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUserGroupListing(int start = 0, int length = 10, string search = "", string order = "", int draw = 1)
        {
            var data = new List<object[]>();
            sqlDescriptor sd = new sqlDescriptor();
            var list = sd.GetDataByQuery<ViewModelUserGroup>(SQLPath + "GetUserGroupListing", new object[] {
            }).ToList();
            var list_count = list.Count;
            if (list.Count > 0)
            {
                list = (order == "asc" ? list.OrderBy(x => x.SrNo) : list.OrderByDescending(x => x.SrNo)).ToList();
                var userGroups = list.Where(x => x.GroupName.ToLower().Contains(search.ToLower()))
                    .Skip(start).Take(length).ToList();
                foreach (ViewModelUserGroup l in userGroups)
                {
                    data.Add(new object[] {
                         l.SrNo
                        ,l.GroupCode
                        ,l.GroupName
                        , ""
                        ,l.ID
                    });
                }
            }
            return Json(
                    new
                    {
                        draw = draw,
                        recordsTotal = list_count,
                        recordsFiltered = list_count,
                        data = data
                    }, JsonRequestBehavior.AllowGet
            );
        }
        public JsonResult GetUserGroupbyID(int ID)
        {
            var userdata = systemManager.UserGroupList().Find(x => x.ID.Equals(ID));
            return Json(userdata, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserGroupbyCode(string code)
        {
            int id = 0;
            var groupObj = Context.UserGroups.Where(x => x.GroupCode == code).FirstOrDefault();
            if (groupObj != null)
            {
                id = groupObj.ID;
            }
            var userdata = systemManager.UserGroupList().Find(x => x.ID.Equals(id));
            return Json(userdata, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateUsergroup(UserGroup usergroup)
        {
            return Json(systemManager.UpdateUserGroup(usergroup), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddUserGroup(UserGroup usergroup)
        {
            return Json(systemManager.AddUserGroup(usergroup), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteUserGroup(int ID)
        {
            return Json(systemManager.DeleteUserGroup(ID), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Control Panel Code
        public JsonResult SiteInfoList()
        {
            return Json(systemManager.SiteInfo(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Globalconfig
        public ActionResult Globalconfig()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            return View();
        }
        public JsonResult GetConfigData()
        {
            var data = ConfigData();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowApiUrl()
        {
            ResultMessage result = new ResultMessage();
            var siteInfo = Context.SiteInformations.FirstOrDefault();
            if (siteInfo != null)
            {
                result.result = "success";
                result.message = siteInfo.SiteUrl;
            }
            else
            {
                result.result = "not found";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public ConfigModel ConfigData()
        {
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ConfigModel>("Configuration/GetConfigData", new object[] { }).FirstOrDefault();
            return result;
        }
        [ValidateInput(false)]
        public JsonResult UpdateMailFormat(ConfigModel m)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                Globalconfig data = new Globalconfig();
                data = context.Globalconfigs.FirstOrDefault();
                if (data == null)
                {
                    result.result = "error";
                    result.message = "There is no reocord for this item.";
                }
                data.ToEmail = m.ToEmail ?? "";
                data.MailSubjects = m.MailSubjects ?? "";
                data.MailBody = m.MailBody ?? "";
                data.SendMail = m.SendMail;
                data.ToEmailContact = m.ToEmailContact ?? "";
                data.MailSubjectsContact = m.MailSubjectsContact ?? "";
                data.MailBodyContact = m.MailBodyContact ?? "";
                data.ToEmailSubscribeUser = m.ToEmailSubscribeUser ?? "";
                data.MailSubjectSubscribe = m.MailSubjectSubscribe ?? "";
                data.MailBodySubscribe = m.MailBodySubscribe ?? "";
                context.SaveChanges();
                result.message = "Record has been updated successfully.";
                result.result = "success";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public JsonResult UpdateConfig(ConfigModel m)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                Globalconfig data = new Globalconfig();
                data = context.Globalconfigs.FirstOrDefault();
                if (data == null)
                {
                    result.result = "error";
                    result.message = "There is no reocord for this item.";
                }
                data.SiteName = m.SiteName;
                data.SiteMetaDescription = m.SiteMetaDescription;
                data.SiteMetaKeywords = m.SiteMetaKeywords;
                data.ShowAuthorMetaTag = m.ShowAuthorMetaTag;
                data.DatabaseType = m.DatabaseType;
                data.DatabaseUsername = m.DatabaseUsername;
                data.DatabaseName = m.DatabaseName;
                data.SendMail = m.SendMail;
                data.FromEmail = m.FromEmail;
                data.ToEmail = m.ToEmail;
                data.MailSubjects = m.MailSubjects;
                data.MailBody = m.MailBody;
                context.SaveChanges();
                result.message = "Record has been updated successfully.";
                result.result = "success";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region update all media url and api url
        public JsonResult SaveChangesSiteInfo(ConfigModel m)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var siteInfo = context.SiteInformations.FirstOrDefault();
                if (siteInfo != null)
                {
                    siteInfo.FrontSiteUrl = m.FrontSiteUrl ?? "";
                    siteInfo.SiteUrl = m.SiteUrl ?? "";
                    context.SaveChanges();
                }
                result.message = "Frontend site url has been updated successfully.";
                result.result = "success";
                logger.Info("Frontend site url has been updated successfully.");
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Password Action
        public JsonResult UpdatePassword(UserCential m)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                using (var entities = new POSSystemEntities())
                {
                    if (m.UserID != "" || m.UserName != "" || m.NewPassword != "" || m.ConfirmPassword != "")
                    {
                        var loginController = new LoginController();
                        var userId = m.UserID;
                        var username = m.UserName;
                        var newPassword = 0;
                        var confirmPassword = 0;
                        if (m.NewPassword != "")
                        {
                            newPassword = loginController.Encrypt(m.NewPassword ?? "");
                            confirmPassword = loginController.Encrypt(m.ConfirmPassword ?? "");
                        }
                        if (newPassword == confirmPassword)
                        {
                            var updatedUser = entities.Users.FirstOrDefault(u => u.UserCode.Equals(userId) && u.UserName.Equals(username));
                            if (updatedUser != null)
                            {
                                updatedUser.Password = newPassword;
                                entities.Users.Attach(updatedUser);
                                var entry = entities.Entry(updatedUser);
                                entry.Property(u => u.Password).IsModified = true;
                                entities.SaveChanges();
                            }
                            else
                            {
                                result.message = "Please check old password,User Not found!";
                                result.result = "info";
                                return Json(new { result }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            result.message = "Password missmatch";
                            result.result = "info";
                            return Json(new { result }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        result.message = "Field is empty";
                        result.result = "info";
                        return Json(new { result }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception occured in UpdatePassword(). Exception:" + ex.ToString() + "");
                result.message = "Operation is Failed";
                result.result = "error";
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            result.message = "Password is Saved!";
            result.result = "success";
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update()
        {
            try
            {
                var p = Request.Params;
                using (var entities = new POSSystemEntities())
                {
                    if (p["userId"] != "" || p["username"] != "" || p["oldPassword"] != "" || p["newPassword"] != "" || p["confirmPassword"] != "")
                    {
                        var loginController = new LoginController();
                        var userId = p["userId"];
                        var username = p["username"];
                        var oldPassword = 0;
                        if (p["oldPassword"] != "")
                        {
                            oldPassword = loginController.Encrypt(p["oldPassword"]);
                        }
                        var newPassword = 0;
                        var confirmPassword = 0;
                        if (p["newPassword"] != "")
                        {
                            newPassword = loginController.Encrypt(p["newPassword"]);
                            confirmPassword = loginController.Encrypt(p["confirmPassword"]);
                        }
                        if (newPassword == confirmPassword)
                        {
                            var updatedUser = entities.Users.FirstOrDefault(u => u.UserCode.Equals(userId) && u.UserName.Equals(username) && u.Password == oldPassword);

                            if (updatedUser != null)
                            {
                                updatedUser.Password = newPassword;
                                entities.Users.Attach(updatedUser);
                                var entry = entities.Entry(updatedUser);
                                entry.Property(u => u.Password).IsModified = true;
                                entities.SaveChanges();
                            }
                            else
                            {
                                return Json(new { isSuccess = false, message = "User Not found!" }, JsonRequestBehavior.DenyGet);
                            }
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "Password missmatch" }, JsonRequestBehavior.DenyGet);
                        }
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Field is empty" }, JsonRequestBehavior.DenyGet);
                    }
                }
            }
            catch
            {
                return Json(new { isSuccess = false, message = "Operation is Failed" }, JsonRequestBehavior.DenyGet);
            }
            return Json(new { isSuccess = true, message = "Password is Saved!" }, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult CheckPassword(string password)
        {
            logger.Info("System call CheckPassword() with password=" + password + ".");
            string returnMessage = string.Empty;
            if (password != "")
            {
                //bool status = false;
                int score;
                //status = PasswordCheck.IsStrongPassword(password);
                score = (int)PasswordCheck.GetPasswordStrength(password);
                switch (score)
                {
                    case 0:
                        returnMessage = "please type password, password can't blank.";
                        break;
                    case 1:
                        returnMessage = "Your password is veryweak.";
                        break;
                    case 2:
                        returnMessage = "Your password is weak.";
                        break;
                    case 3:
                        returnMessage = "Your password is strong.";//Your password is medium strength.
                        break;
                    case 4:
                        returnMessage = "Your password is strong.";
                        break;
                    case 5:
                        returnMessage = "Your password is very strong.";
                        break;
                }
            }
            else
            {

            }
            return Json(new { message = returnMessage }, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult CheckOldPassword(string User, string UserName, string Password)
        {
            logger.Info("System call CheckOldPassword with User=" + User + " ,UserName=" + UserName + ".");
            string returnMessage = string.Empty;
            var context = new POSSystemEntities();
            var loginController = new POSSystem.Controllers.LoginController();
            var oldPassword = 0;
            var typePassword = 0;
            typePassword = loginController.Encrypt(Password);
            var userpassword = context.Users.Where(x => x.UserCode == User && x.UserName == UserName).Select(u => u.Password).FirstOrDefault();
            oldPassword = Convert.ToInt32(userpassword);
            if (oldPassword != typePassword)
            {
                returnMessage = "Password does not match the old password.";
            }
            return Json(new { message = returnMessage }, JsonRequestBehavior.DenyGet);
        }
        public enum PasswordStrength
        {
            Blank = 0, VeryWeak = 1, Weak = 2, Medium = 3, Strong = 4, VeryStrong = 5
        }
        public static class PasswordCheck
        {
            public static PasswordStrength GetPasswordStrength(string password)
            {
                int score = 0;
                if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password.Trim()))
                    return PasswordStrength.Blank;
                if (!HasMinimumLength(password, 5))
                    return PasswordStrength.VeryWeak;
                if (HasMinimumLength(password, 8)) score++;
                if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password)) score++;
                if (HasDigit(password)) score++;
                if (HasSpecialChar(password)) score++;
                return (PasswordStrength)score;
            }
            public static bool IsStrongPassword(string password)
            {
                return HasMinimumLength(password, 8) && HasUpperCaseLetter(password) && HasLowerCaseLetter(password) && (HasDigit(password) || HasSpecialChar(password));
            }
            public static bool IsValidPassword(string password, int requiredLength, int requiredUniqueChars, bool requireNonAlphanumeric, bool requireLowercase, bool requireUppercase, bool requireDigit)
            {
                if (!HasMinimumLength(password, requiredLength)) return false;
                if (!HasMinimumUniqueChars(password, requiredUniqueChars)) return false;
                if (requireNonAlphanumeric && !HasSpecialChar(password)) return false;
                if (requireLowercase && !HasLowerCaseLetter(password)) return false;
                if (requireUppercase && !HasUpperCaseLetter(password)) return false;
                if (requireDigit && !HasDigit(password)) return false;
                return true;
            }
            #region Helper Methods

            public static bool HasMinimumLength(string password, int minLength)
            {
                return password.Length >= minLength;
            }

            public static bool HasMinimumUniqueChars(string password, int minUniqueChars)
            {
                return password.Distinct().Count() >= minUniqueChars;
            }

            public static bool HasDigit(string password)
            {
                return password.Any(c => char.IsDigit(c));
            }

            public static bool HasSpecialChar(string password)
            {
                // return password.Any(c => char.IsPunctuation(c)) || password.Any(c => char.IsSeparator(c)) || password.Any(c => char.IsSymbol(c));
                return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
            }

            public static bool HasUpperCaseLetter(string password)
            {
                return password.Any(c => char.IsUpper(c));
            }

            public static bool HasLowerCaseLetter(string password)
            {
                return password.Any(c => char.IsLower(c));
            }
            #endregion
        }
        #endregion
        public class ViewModalUserProfile
        {
            public string UserID { get; set; }
            public string UserCode { get; set; }
            public string UserName { get; set; }
            public string UserGroupCode { get; set; }
            public string Email { get; set; }
            public string PhoneNo { get; set; }
            public string ImageUrl { get; set; }
        }
        public JsonResult GetUserProfileData()
        {
            var data = UserProfileDataByCode();
            if (data != null)
            {
                //if (data.MediaUrl_enUS == "" || data.MediaUrl_enUS == null)
                //{
                //    data.MediaUrl_enUS = "\\MediaFolder\\user-6.jpg";
                //}
            }
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public ViewModalUserProfile UserProfileDataByCode()
        {
            var User = System.Web.HttpContext.Current.Session["User"];
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ViewModalUserProfile>("User/GetUserProfileDataByCode", new object[] { User.ToString() }).FirstOrDefault();
            return result;
        }
        #region Menu CMS Show Hide
        public JsonResult ShowHideCMSMenu(User m)
        {
            ResultMessage result = new ResultMessage();
            string user = (Session["User"] ?? "").ToString();
            var checkAdmin = Context.Users.Where(x => x.UserCode == user).FirstOrDefault();
            if (checkAdmin != null)
            {
                if (checkAdmin.UserGroupCode == "Admin")
                {
                    var UserInfo = Context.Users.Where(x => x.Id == m.Id).FirstOrDefault();
                    if (UserInfo != null)
                    {
                        //UserInfo.ShowPageMenu = m.ShowPageMenu ?? false;
                        Context.SaveChanges();
                        result.message = "Show/hide CMS menu data successfully. ";
                        result.result = "success";
                    }
                    else
                    {
                        result.message = "User not found";
                        result.result = "info";
                    }
                }
                else
                {
                    result.message = "Permission denied!. The cms menu can change only admin role.";
                    result.result = "error";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowHideRptMenu(int ID)
        {
            ResultMessage result = new ResultMessage();
            string user = (Session["User"] ?? "").ToString();
            var chkObj = Context.ReportMaintenances.Where(x => x.ID == ID).FirstOrDefault();
            if (chkObj != null)
            {
                if ((chkObj.Deleted ?? false) == false)
                {
                    chkObj.Deleted = true;
                }
                else
                {
                    chkObj.Deleted = false;
                }
                Context.SaveChanges();
                result.message = "Show/hide CMS menu data successfully. ";
                result.result = "success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public JsonResult GetLogData()
        {
            string from = Request.Params["LogFDate"];
            string to = Request.Params["LogTDate"];
            var data = new List<object[]>();
            sqlDescriptor sd = new sqlDescriptor();
            string locFile = string.Empty;
            locFile = "~/logs";
            string fname;
            bool exists = System.IO.Directory.Exists(Server.MapPath(locFile));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(locFile));
            }
            var dates = new List<DateTime>();
            if (from == "" || to == "")
            {
                dates.Add(DateTime.Now);
            }
            else
            {
                DateTime dfDate = Convert.ToDateTime("1753-01-01");
                DateTime logFDate = DateTime.Now;
                DateTime logTDate = DateTime.Now;
                logFDate = from == "" ? dfDate : Convert.ToDateTime(from);
                logTDate = to == "" ? dfDate : Convert.ToDateTime(to);

                for (var dt = logFDate; dt <= logTDate; dt = dt.AddDays(1))
                {
                    dates.Add(dt);
                }
            }
            List<ViewModelLog> list = new List<ViewModelLog>();
            foreach (var ditem in dates)
            {
                string logfileName = "cityU_" + ditem.ToString("yyyyMMdd") + ".log";
                fname = Path.Combine(Server.MapPath(locFile), logfileName);
                if (System.IO.File.Exists(fname))
                {
                    string[] lines = System.IO.File.ReadAllLines(fname);
                    using (StreamReader file = new StreamReader(fname, Encoding.UTF8))
                    {
                        //string zat = file.ReadToEnd();
                        int counter = 1;
                        string ln;

                        int index = 0;
                        int index1 = 0;

                        string dateTime = string.Empty;
                        string caller = string.Empty;
                        string action = string.Empty;

                        while ((ln = file.ReadLine()) != null)
                        {
                            ViewModelLog log = new ViewModelLog();
                            string textData = ln.ToString();
                            index = textData.IndexOf(" - ");
                            dateTime = textData.Substring(0, 19);
                            log.LogAction = textData.Substring(index + 3);
                            string tempString = textData.Substring(0, index);
                            index1 = tempString.IndexOf("]");
                            caller = tempString.Substring(index1 + 1);
                            log.LogDate = dateTime;
                            log.CallerName = caller;
                            log.SrNo = counter;

                            list.Add(log);
                            counter++;
                        }
                        file.Close();
                    }
                }
            }
            var list_count = list.Count;
            if (list.Count > 0)
            {
                foreach (ViewModelLog l in list)
                {
                    data.Add(new object[] {
                         l.SrNo
                         ,l.LogDate
                         ,l.CallerName
                         ,l.LogAction
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
        public JsonResult SaveDashboardSettings()
        {
            var result = new ResultMessage();
            string[] imgExtensions = { ".bmp", ".jpg", ".gif", ".png", ".jpeg", ".tiff", ".png", ".PNG", ".ico", ".svg" };
            var BackgroundColor = System.Web.HttpContext.Current.Request.Form["BackgroundColor"];
            var LogoImage = System.Web.HttpContext.Current.Request.Form["LogoImage"];
            var LogoFile = System.Web.HttpContext.Current.Request.Files["LogoFile"];
            var TableBgColor = System.Web.HttpContext.Current.Request.Form["TableBgColor"];
            var TableBgTitleColor = System.Web.HttpContext.Current.Request.Form["TableBgTitleColor"];
            var HoverColor = System.Web.HttpContext.Current.Request.Form["HoverColor"];
            var FontColor = System.Web.HttpContext.Current.Request.Form["FontColor"];
            var LoginBgColor = System.Web.HttpContext.Current.Request.Form["LoginBgColor"];
            var LoginDescription = System.Web.HttpContext.Current.Request.Form["LoginDescription"];
            var LoginLogo = System.Web.HttpContext.Current.Request.Files["LoginLogo"];
            var folder = System.Web.HttpContext.Current.Server.MapPath("~/images/logo/");
            var path = "/images/logo/";
            if (LoginLogo != null && LoginLogo.ContentLength > 0)
            {
                string extension = Path.GetExtension(LogoFile.FileName);
                if (imgExtensions.Contains(extension))
                {
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    //check file size less than 5M
                    LoginLogo.SaveAs(folder + LoginLogo.FileName.Split('/').LastOrDefault());
                }
            }
            if (LogoFile != null && LogoFile.ContentLength > 0)
            {
                string extension = Path.GetExtension(LogoFile.FileName);
                if (imgExtensions.Contains(extension))
                {
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    //check file size less than 5M
                    LogoFile.SaveAs(folder + LogoFile.FileName.Split('/').LastOrDefault());
                    using (var db = new POSSystemEntities())
                    {
                        var config = db.Globalconfigs.FirstOrDefault();
                        if (config != null)
                        {
                            config.DashboardColor = BackgroundColor;
                            config.LogoImage = path + LogoFile.FileName.Split('/').LastOrDefault();
                            config.TableBgTitleColor = TableBgTitleColor;
                            config.TableBgColor = TableBgColor;
                            config.HoverColor = HoverColor;
                            config.FontColor = FontColor;
                            config.LoginBgColor = LoginBgColor;
                            config.LoginDescription = LoginDescription;
                            config.LoginLogo = path + LoginLogo.FileName.Split('/').LastOrDefault(); ;
                            db.SaveChanges();
                            result.result = "success";
                            result.message = "Save successfully .";
                        }
                    }
                }
                else
                {
                    result.result = "error";
                    result.message = "Unsupported file format, please upload image file";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDashboardSettings()
        {
            var result = new ResultMessage();
            using (var db = new POSSystemEntities())
            {
                var config = db.Globalconfigs.FirstOrDefault();
                if (config != null)
                {
                    result.data = new
                    {
                        BackgroundColor = config.DashboardColor,
                        LogoImage = config.LogoImage,
                        TableBgColor = config.TableBgColor,
                        TableBgTitleColor = config.TableBgTitleColor,
                        HoverColor = config.HoverColor,
                        FontColor = config.FontColor,
                        LoginLogo = config.LoginLogo,
                        LoginDescription = config.LoginDescription,
                        LoginBgColor = config.LoginBgColor
                    };
                    result.result = "success";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserData()//string User
        {
            var result = new ResultMessage();
            string user = (Session["User"] ?? "").ToString();
            using (var db = new POSSystemEntities())
            {
                var userObj = db.Users.FirstOrDefault(x => x.UserCode == user);
                if (userObj != null)
                {
                    result.data = new UserCential
                    {
                        UserID = userObj.UserCode,
                        UserName = userObj.UserName
                    };
                    result.result = "success";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserList()
        {
            var result = new ResultMessage();
            using (var db = new POSSystemEntities())
            {
                var user = db.Users.Select(x => new
                {
                    Id = x.Id,
                    UserCode = x.UserCode,
                    UserName = x.UserName,
                    Permissions = new
                    {
                        //ShowSetupCourse = x.ShowSetupCourse ?? false,
                        //ShowSetupPosition = x.ShowSetupPosition ?? false,
                    }
                }).ToList();
                result.data = user;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowHideMenuPermission(User user)
        {
            var result = new ResultMessage();
            var data = Context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (data != null)
            {
                data.ShowSetupCategory = user.ShowSetupCategory ?? data.ShowSetupCategory;
                data.ShowSetupDepartment = user.ShowSetupDepartment ?? data.ShowSetupDepartment;
                data.ShowReportMenu = user.ShowReportMenu ?? data.ShowReportMenu;
                data.ShowUser = user.ShowUser ?? data.ShowUser;
                data.ShowUserGroup = user.ShowUserGroup ?? data.ShowUserGroup;
                data.ShowLogMenu = user.ShowLogMenu ?? data.ShowLogMenu;
                data.ShowThemeMenu = user.ShowThemeMenu ?? data.ShowThemeMenu;
                data.ShowMediaMenu = user.ShowMediaMenu ?? data.ShowMediaMenu;
                data.ShowSitePermissionMenu = user.ShowSitePermissionMenu ?? data.ShowSitePermissionMenu;
                Context.SaveChanges();
                result.result = "success";
                result.message = "Updated successfully .";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPermissionListByID(int ID)
        {
            var result = new ResultMessage();
            var userinfo = new User();
            var data = Context.Users.FirstOrDefault(x => x.Id == ID);
            if (data != null)
            {
                userinfo.ShowSetupCategory = data.ShowSetupCategory ?? data.ShowSetupCategory;
                userinfo.ShowSetupDepartment = data.ShowSetupDepartment ?? data.ShowSetupDepartment;
                userinfo.ShowReportMenu = data.ShowReportMenu ?? data.ShowReportMenu;
                userinfo.ShowUser = data.ShowUser ?? data.ShowUser;
                userinfo.ShowUserGroup = data.ShowUserGroup ?? data.ShowUserGroup;
                userinfo.ShowLogMenu = data.ShowLogMenu ?? data.ShowLogMenu;
                userinfo.ShowThemeMenu = data.ShowThemeMenu ?? data.ShowThemeMenu;
                userinfo.ShowMediaMenu = data.ShowMediaMenu ?? data.ShowMediaMenu;
                userinfo.ShowSitePermissionMenu = data.ShowSitePermissionMenu ?? data.ShowSitePermissionMenu;
            }
            result.data = userinfo;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateStatusComponent(int UserID, bool Status)
        {
            var result = new ResultMessage();
            using (var db = new POSSystemEntities())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == UserID);
                if (user != null)
                {
                    user.DeletedStatus = !Status;
                    db.SaveChanges();
                    result.result = "success";
                    result.message = "Updated successful .";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteMultipleUser(List<int> Ids)
        {
            ResultMessage result = new ResultMessage();
            if (Ids.Count > 0)
            {
                using (var dbContext = new POSSystemEntities())
                {
                    for (int i = 0; i < Ids.Count; i++)
                    {
                        int uid = Ids[i];
                        systemManager.Delete(uid);
                    }
                    result.message = "User list has been deleted successfully.";
                    result.result = "success";
                }
            }
            else
            {
                result.message = "There is no page for delete.";
                result.result = "info";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteMultipleUserGroup(List<int> Ids)
        {
            ResultMessage result = new ResultMessage();
            if (Ids.Count > 0)
            {
                using (var dbContext = new POSSystemEntities())
                {
                    for (int i = 0; i < Ids.Count; i++)
                    {
                        int gid = Ids[i];
                        systemManager.DeleteUserGroup(gid);
                    }
                    result.message = "User group list has been deleted successfully.";
                    result.result = "success";
                }
            }
            else
            {
                result.message = "There is no page for delete.";
                result.result = "info";
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveUserGroupPermission(User user)
        {
            ResultMessage result = new ResultMessage();
            using (var context = new POSSystemEntities())
            {
                var userList = context.Users.Where(x => x.UserGroupCode == user.UserGroupCode).ToList();
                if (userList.Count > 0)
                {
                    foreach (var userObj in userList)
                    {
                        //userObj.AllowCreatePage = user.AllowCreatePage ?? false;
                        context.SaveChanges();
                    }
                    result.message = "User group permission has been updated successfully.";
                    result.result = "success";
                }
                else
                {
                    result.message = "There is no link user for this group.";
                    result.result = "info";
                }
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserAccessByUserGroup(string UserGroup)
        {
            ResultMessage result = new ResultMessage();
            string user = (Session["User"] ?? "").ToString();
            var userdata = Context.Users.Where(x => x.UserCode == user && x.UserGroupCode == UserGroup).FirstOrDefault();
            if (userdata == null)
            {
                result.message = "There is no access.";
                result.result = "info";
            }
            else
            {
                result.message = "Get success user info by user group.";
                result.result = "success";
                result.data = userdata;
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetApprovalSetting()
        {
            var data = ApprovalSetting();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public UserPermission ApprovalSetting()
        {
            string User = (Session["User"] ?? "").ToString();
            var userObj = Context.Users.Where(x => x.UserCode == User).FirstOrDefault();
            UserPermission result = new UserPermission();
            if (userObj != null)
            {
                result = Context.UserPermissions.Where(x => x.UserID == userObj.Id).FirstOrDefault();
            }
            //sqlDescriptor sd = new sqlDescriptor();
            //var result = sd.GetDataByQuery<UserPermission>("SystemManager/GetApprovalSettingByUserCode", new object[] { User }).FirstOrDefault();
            return result;
        }
        public JsonResult IsCheckPermission()
        {
            bool data = false;
            var setting = Context.Globalconfigs.FirstOrDefault();
            if (setting != null)
            {
                data = setting.CheckPermission ?? false;
            }
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPerRptList()
        {
            var data = new List<object[]>();
            var list = Context.ReportMaintenances.ToList();
            var list_count = list.Count;
            string chkRpt;
            if (list.Count > 0)
            {
                foreach (ReportMaintenance l in list)
                {
                    if ((l.Deleted ?? false) == false)
                    {
                        chkRpt = "Tchk" + l.ID;//T = true
                    }
                    else
                    {
                        chkRpt = "Fchk" + l.ID;//F = false
                    }
                    data.Add(new object[] {
                        chkRpt
                       ,l.SubReport
                       ,l.MainReport
                       ,l.ID
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
        public JsonResult AllowAllAction(int UserID, bool SetAll)
        {
            ResultMessage result = new ResultMessage();
            if (SetAll == true)
            {
                var userObj = Context.UserPermissions.FirstOrDefault(x => x.UserID == UserID);
                if (userObj != null)
                {
                    userObj.AllowCreateDepartment = true;
                    userObj.AllowUpdateDepartment = true;
                    userObj.AllowDeleteDepartment = true;
                    userObj.AllowCreateCategory = true;
                    userObj.AllowUpdateCategory = true;
                    userObj.AllowDeleteCategory = true;
                    userObj.AllowCreateUser = true;
                    userObj.AllowUpdateUser = true;
                    userObj.AllowDeleteUser = true;
                    userObj.AllowCreateUserGroup = true;
                    userObj.AllowUpdateUserGroup = true;
                    userObj.AllowDeleteUserGroup = true;
                    Context.SaveChanges();
                    result.message = "User permission has been updated.";
                    result.result = "success";
                }
            }
            else
            {
                var userObj = Context.UserPermissions.FirstOrDefault(x => x.UserID == UserID);
                if (userObj != null)
                {
                    userObj.AllowCreateDepartment = false;
                    userObj.AllowUpdateDepartment = false;
                    userObj.AllowDeleteDepartment = false;
                    userObj.AllowCreateCategory = false;
                    userObj.AllowUpdateCategory = false;
                    userObj.AllowDeleteCategory = false;
                    userObj.AllowCreateUser = false;
                    userObj.AllowUpdateUser = false;
                    userObj.AllowDeleteUser = false;
                    userObj.AllowCreateUserGroup = false;
                    userObj.AllowUpdateUserGroup = false;
                    userObj.AllowDeleteUserGroup = false;
                    Context.SaveChanges();
                    result.message = "User permission has been updated.";
                    result.result = "success";
                }
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}