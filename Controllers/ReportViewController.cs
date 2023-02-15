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
            var code = HttpContext.Request.Params["code"];
            var examiner = HttpContext.Request.Params["examiner"];
            var people_post = HttpContext.Request.Params["people_post"];
            var people_group = HttpContext.Request.Params["people_group"];
            var discipline = HttpContext.Request.Params["discipline"];
            var cat = HttpContext.Request.Params["cat"];
            //var depID = HttpContext.Request.Params["dept"];
            //var fromDateTime = DateTime.Now.ToString();
            //var toDateTime = DateTime.Now.ToString();
            string rptName = SubReport.Replace("'", "");
            //ReportViewer viewer = new ReportViewer();
            //viewer = createReport(rptName, code, people_post, people_group, filter_discipline);
            ViewBag.ReportViewer = createReport(rptName, code, people_post, people_group, discipline, examiner, cat);
            ViewBag.Message = rptName;
            return View();
        }
        public ReportViewer createReport(string rptName, string code, string people_post, string people_group, string filter_discipline, string examiner, string cat)
        {
            //var ds = dataSet;
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conx = new SqlConnection(connectionString);
            string SQLs;
            //var courseObj = Context.Course_Table.Where(x => x.code == code).FirstOrDefault();
            //string tlastr = string.Empty;
            //string atsstr = string.Empty;
            switch (rptName)
            {
                case "Customer":
                    CustomerListing dsCust = new CustomerListing();
                    SQLs = "select * from Customer";//"where (Course_Table.outdated = 0) and LEFT(Offer,1) <> 'x'";
                    //if (code != "")
                    //{
                    //    SQLs += " and Course_Table.code ='" + code + "'";
                    //}
                    SqlDataAdapter adp = new SqlDataAdapter(SQLs, conx);
                    adp.Fill(dsCust, dsCust.Customer.TableName);
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetCustomerListing", dsCust.Tables[0]));
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\CustomerListing.rdlc";
                    break;
                case "Supplier":
                    SupplierListing dsSup = new SupplierListing();
                    SQLs = "select * from Supplier";
                    SqlDataAdapter adpaim = new SqlDataAdapter(SQLs, conx);
                    adpaim.Fill(dsSup, dsSup.Supplier.TableName);
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetSupplierListing", dsSup.Tables[0]));
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\SupplierListing.rdlc";
                    break;
                case "Sale Person":
                    //Report.DataSet.Semester ds_Semester = new Report.DataSet.Semester();
                    //    SQLs = "select * from [ace_report].Semester";
                    //    SqlDataAdapter adpsem = new SqlDataAdapter(SQLs, conx);
                    //    adpsem.Fill(ds_Semester, ds_Semester._Semester.TableName);
                    //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Semester", ds_Semester.Tables[0]));
                    //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\Semester.rdlc";
                    break;
                case "Stock":
                    StockListing dsStock = new StockListing();
                    SQLs = "select * from Stock";
                    SqlDataAdapter adpstk = new SqlDataAdapter(SQLs, conx);
                    adpstk.Fill(dsStock, dsStock.Stock.TableName);
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetStockListing", dsStock.Tables[0]));
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\StockListing.rdlc";
                    break;
                case "Warehouse":

                    break;
                case "Sale Summary":
                    SaleListing dsSale = new SaleListing();
                    SQLs = "SELECT [ID],[SaleDate],[InvoiceNo],[LocationID],[CustID],[SalesPersonID],[TotalAmount],[Discount],[PaidAmount],[Balance],[CashDown],[Remark],[CrtUser],[CrtDate],[CrtTime],[UpdUser],[UptDate] FROM[dbo].[SaleHeader]";
                    SqlDataAdapter adpsalesum = new SqlDataAdapter(SQLs, conx);
                    adpsalesum.Fill(dsSale, dsSale.SaleHeader.TableName);
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetSaleListing", dsSale.Tables[0]));
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\SaleListing.rdlc";
                    break;
                case "Sale Detail":

                    break;
                case "Purchase Summary":
                    PurchaseListing dsPurchase = new PurchaseListing();
                    SQLs = "SELECT [ID],[PurchaseDate],[InvoiceNo],[LocationID],[SupID],[SalesPersonID],[TotalAmount],[Discount],[PaidAmount],[Balance],[CashDown],[Remark],[CrtUser],[CrtDate],[CrtTime],[UpdUser],[UptDate] FROM[dbo].[PurchaseHeader]";
                    SqlDataAdapter adpPurSum = new SqlDataAdapter(SQLs, conx);
                    adpPurSum.Fill(dsPurchase, dsPurchase.PurchaseHeader.TableName);
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetPurchaseListing", dsPurchase.Tables[0]));
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\PurchaseListing.rdlc";
                    break;
                case "Purchase Detail":

                    break;
                default:
                    break;
            }
            return reportViewer;
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
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
            //public string WorkFlowStepID { get; set; }
            //public string Dataset { get; set; }
        }
    }
}