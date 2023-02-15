using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using POSData;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using POSSystem.Report.DataSet;
//using System.Text.RegularExpressions;
//using POSSystem.Models;
//using static POSSystem.Models.ViewModel;
using Microsoft.Reporting.WebForms;

namespace POSSystem.Controllers
{
    public class ReportViewController : Controller
    {
        //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ReportViewController));
        private POSSystemEntities Context { get; set; }
        public string SQLPath = "ReportView/";
        public ReportViewController()
        {
            Context = new POSSystemEntities();
        }
        public ActionResult Index()
        {
            var signIn = System.Web.HttpContext.Current.Session["User"];
            if (signIn == null)
            {
                return RedirectToAction("../Login");
            }
            //var FromDate = HttpContext.Request.Params["FromDate"];
            //var ToDate = HttpContext.Request.Params["ToDate"];
            var SubReport = HttpContext.Request.Params["subreport"];
            var member = HttpContext.Request.Params["member"];
            string rptName = SubReport.Replace("'", "");
            //ReportViewer viewer = new ReportViewer();
            ViewBag.ReportViewer = createReport(rptName, member);
            ViewBag.Message = rptName;
            return View();
        }
        public ReportViewer createReport(string rptName, string member)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conx = new SqlConnection(connectionString);
            string SQLs;
            switch (rptName)
            {
                case "Coupon Used":
                    CustomerListing dsCust = new CustomerListing();
                    SQLs = "SELECT dbo.PurchaseHead.* FROM dbo.PurchaseHead";
                    //if (member != "")
                    //{
                    //    SQLs += " and MemberCode ='" + member + "'";
                    //}
                    SqlDataAdapter adp = new SqlDataAdapter(SQLs, conx);
                    adp.Fill(dsCust, dsCust.Customer.TableName);
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetCouponUsed", dsCust.Tables[0]));
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\ReportCouponUsed.rdlc";
                    break;
                default:
                    break;
            }
            return reportViewer;
        }
        public ReportViewer GenerateReport(ReportFilterModel filter)
        {
            var reportviewer = new ReportViewer();
            return reportviewer;
        }
        public class ReportFilterModel
        {
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string User { get; set; }
            public string SubReport { get; set; }
        }
    }
}