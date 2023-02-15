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
using System.Security.Authentication;
using System.IO;
using Newtonsoft.Json;
//using System.Web.Http;
//using System.Net.Http.Headers;
namespace POSSystem.Controllers
{
    public class PointSystemApiController : ApiController
    {
        private POSSystemEntities Context { get; set; }
        public PointSystemApiController()
        {
            Context = new POSSystemEntities();
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
        
        [HttpPost]
        public HttpResponseMessage CalculatePoint()
        {
            ReturnMsg msg = new ReturnMsg();
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            var httpWebRequest = System.Net.WebRequest.Create(@"https://localhost:44361/api/possystemapi/getpurchasedata") as HttpWebRequest;
            httpWebRequest.ContentType = "application /json;";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 100000;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var purchaseLst = JsonConvert.DeserializeObject<List<ViewModelPurchase>>(result);
                if (purchaseLst.Count > 0)
                {
                    foreach (var item in purchaseLst)
                    {
                        if (item.ItemName == "Non Alcohol")
                        {
                            int point = Convert.ToInt32(item.TotalPrice / 10);
                            var memberObj = Context.Members.Where(x => x.MemberCode == item.MemberCode).FirstOrDefault();
                            if (memberObj != null)
                            {
                                memberObj.TotalPoint = memberObj.TotalPoint + point;
                                Context.SaveChanges();
                                msg.msg = "Success";
                                msg.eflag = "S";
                            }
                        }
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, msg);
        }

        [HttpPost]
        public HttpResponseMessage AddPointToMember()
        {
            var code = HttpContext.Current.Request.Params["membercode"];
            var point = HttpContext.Current.Request.Params["point"];
            ReturnMsg msg = new ReturnMsg();
            try
            {
                using (var context = new POSSystemEntities())
                {
                    var mObj = context.Members.Where(x => x.MemberCode == code).FirstOrDefault();
                    if (mObj != null)
                    {
                        mObj.TotalPoint = 100;
                        context.SaveChanges();
                        msg.msg = "Success";
                        msg.eflag = "S";
                    }
                    else
                    {
                        msg.msg = "Member not found.";
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

    }
}
