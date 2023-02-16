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
using POSSystem.Filters;
//using System.Web.Http;
//using System.Net.Http.Headers;
namespace POSSystem.Controllers
{
    //[BasicAuthentication]
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
            public string ItemName { get; set; }
            public double Price { get; set; }
            public int Qty { get; set; }
            public double TotalPrice { get; set; }
        }
        [HttpGet]
        public HttpResponseMessage IsCouponAvailable()
        {
            var code = HttpContext.Current.Request.Params["couponcode"];
            ReturnMsg msg = new ReturnMsg();
            try
            {
                using (var context = new POSSystemEntities())
                {
                    var cObj = context.Coupons.Where(x => x.CouponCode == code).FirstOrDefault();
                    if (cObj != null)
                    {
                        if (cObj.AvailableQty < 30)
                        {
                            msg.msg = "Coupon available";
                            msg.eflag = "Available";
                        }
                        else
                        {
                            msg.msg = "Coupon not available";
                            msg.eflag = "NotAvailable";
                        }
                    }
                    else
                    {
                        msg.msg = "Coupon not found.";
                        msg.eflag = "E";
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, msg);
                }
            }
            catch (Exception e)
            {
                msg.msg = e.Message;
                msg.eflag = "E";
                return Request.CreateResponse(HttpStatusCode.BadRequest, msg);
                throw;
            }
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
