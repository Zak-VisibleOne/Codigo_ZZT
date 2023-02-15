using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using POSData;
using System.Data;
using System.Web;
using System.Collections.Generic;
using POSSystem.Utils;
//using System.Web.Http;
//using System.Net.Http.Headers;
namespace POSSystem.Controllers
{
    public class POSSystemApiController : ApiController
    {
        private POSSystemEntities Context { get; set; }
        //string apiurl = "";
        public POSSystemApiController()
        {
            Context = new POSSystemEntities();
            //var siteInfo = Context.SiteInformations.FirstOrDefault();
            //if (siteInfo != null)
            //{
            //    apiurl = siteInfo.SiteUrl;
            //}
        }
        public class ReturnMsg
        {
            public string msg { set; get; }
            public string eflag { set; get; }
        }
        public class ViewModelPurchase
        {
            public int id { set; get; }
            public string MemberCode { get; set; }
            public string CouponCode { get; set; }
            public string ReceiptNumber { get; set; }
        }
        [HttpGet]
        [ActionName("getpurchasedata")]
        public IEnumerable<ViewModelPurchase> GetPurchaseData()
        {
            sqlDescriptor sd = new sqlDescriptor();
            return sd.GetDataByQuery<ViewModelPurchase>("Purchase/GetPurchaseHead", new object[] { }).ToList();
        }
    }
}
