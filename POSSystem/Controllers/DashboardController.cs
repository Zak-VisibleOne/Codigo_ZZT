using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
//using static KZ.Models.ViewModel_api;
using POSData;
using System.Linq;
using static POSSystem.Models.ViewModel;
using POSSystem.Utils;

namespace POSSystem.Controllers
{
    public class DashboardController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DashboardController));
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private POSSystemEntities Context { get; set; }
        public string SQLPathSyM = "SystemManager/";
        public DashboardController()
        {
            Context = new POSSystemEntities();
        }
        public ActionResult Index()
        {
            //return Redirect("~/ProgramCourse");
            //return RedirectToAction("Index", "ProgramCourse");
            //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DashboardController));
            var User = System.Web.HttpContext.Current.Session["User"];
            if (User == null)
            {
                return RedirectToAction("../Login");
            }
            var sqlDB = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            var dbName = sqlDB.InitialCatalog;
            ViewData["DName"] = dbName;
            Session["DBName"] = dbName;
            return View();
        }
        #region LoadLatestMemberReg
        public JsonResult LoadLatestMemberReg()
        {
            return Json(GetLatestMemberReg(), JsonRequestBehavior.AllowGet);
        }
        public class memberData
        {
            public int ID { get; set; }
            public string ChineseName { get; set; }
            public string KOLStatus { get; set; }
            public string MobilePhone { get; set; }
            public string AccountNo { get; set; }
        }
        public List<memberData> GetLatestMemberReg()
        {
            List<memberData> lst = new List<memberData>();
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetLatestMemberReg", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new memberData
                    {
                        ID = Convert.ToInt32(rdr["ID"].ToString()),
                        ChineseName = rdr["ChineseName"].ToString(),
                        KOLStatus = rdr["KOLStatus"].ToString(),
                        MobilePhone = rdr["MobilePhone"].ToString(),
                        AccountNo = rdr["AccountNo"].ToString()
                    });
                }
                return lst;
            }
        }
        #endregion
        public JsonResult ActivitiesLog()
        {
            var result = new ResultMessage();
            var logs = new List<ViewModelLog>();
            var finallogs = new List<ViewModelLog>();
            var folder = Server.MapPath("~/logs");
            DirectoryInfo info = new DirectoryInfo(folder);
            if (Directory.Exists(folder))
            {
                var files = info.GetFiles().OrderByDescending(x => x.CreationTime);
                var file = files.FirstOrDefault();
                var readfile = new StreamReader(file.FullName);
                var line = "";
                var count = 0;
                int index = 0;
                int index1 = 0;
                string dateTime = string.Empty;
                string caller = string.Empty;
                string action = string.Empty;
                while ((line = readfile.ReadLine()) != null)
                {
                    ViewModelLog log = new ViewModelLog();
                    string textData = line;
                    index = textData.IndexOf(" - ");
                    dateTime = textData.Substring(0, 19);
                    log.LogAction = textData.Substring(index + 3);
                    string tempString = textData.Substring(0, index);
                    index1 = tempString.IndexOf("]");
                    caller = tempString.Substring(index1 + 1);
                    log.LogDate = dateTime;
                    log.CallerName = caller;
                    log.SrNo = count;
                    logs.Add(log);
                    count++;
                }
                readfile.Close();
            }
            for (var i = logs.Count - 1; i >= 0; i--)
            {
                finallogs.Add(logs[i]);
            }
            result.data = finallogs;
            result.result = "success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region Search
        public class searchResultModel
        {
            public int PageID { get; set; }
            public int PageDetailID { get; set; }
            public int ContentID { get; set; }
            public string ContentName { get; set; }
            public string PageTitle { get; set; }
            public int SrNo { get; set; }
        }
        public JsonResult GetSearchResult()
        {
            string wildtext = Request.Params["Search"];
            var data = new List<object[]>();
            sqlDescriptor sd = new sqlDescriptor();
            var DatabaseName = Session["DBName"];
            var list_count = 0;
            int srNo = 1;
            var tableList = sd.GetDataByQuery<ViewModelColun>(SQLPathSyM + "GetTableName", new object[] { DatabaseName }).ToList();
            if (tableList.Count > 0)
            {
                foreach (var itemTable in tableList)
                {
                    if (itemTable.TABLE_NAME == "ContentTran")
                    {
                        var ColList = sd.GetDataByQuery<ViewModelColun>(SQLPathSyM + "GetColumnName", new object[] { itemTable.TABLE_NAME }).ToList();
                        foreach (var ColItem in ColList)
                        {
                            //string strQuery = @"select PageID,PageDetailID,SrNo,ContentID,ContentName from " + itemTable.TABLE_NAME + " where [" + ColItem.COLUMN_NAME + "] like '%" + wildtext + "%'";
                            //var resultRow = Context.Database.SqlQuery<searchResultModel>(strQuery).ToList();
                            var resultRow = sd.GetDataByQuery<searchResultModel>("Configuration/" + "GetSearchResult", new object[] { ColItem.COLUMN_NAME, wildtext }).ToList();
                            if (resultRow.Count > 0)
                            {
                                foreach (searchResultModel l in resultRow)
                                {
                                    data.Add(new object[] {
                                    srNo
                                    ,l.PageTitle
                                    ,l.ContentName
                                    ,""
                                    ,l.ContentID
                                    ,l.PageID
                                    ,l.PageDetailID
                                });
                                    srNo++;
                                }
                            }

                        }
                    }
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
        #endregion
    }
}