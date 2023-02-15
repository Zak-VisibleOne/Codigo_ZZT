using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using POSData;
using System.Data;
using System.Web;
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

        //purchase data



        [HttpPost]
        [Route("api/form/savestorecode")]
        public HttpResponseMessage savestorecode()
        {
            var code = HttpContext.Current.Request.Params["code"];
            var name = HttpContext.Current.Request.Params["desc"];
            var location = HttpContext.Current.Request.Params["location"];
            var status = HttpContext.Current.Request.Params["status"];
            ReturnMsg msg = new ReturnMsg();
            try
            {
                using (var context = new POSSystemEntities())
                {
                    //var scodeObj = context.StoreCodes.Where(x => x.Code == code).FirstOrDefault();
                    //if (scodeObj == null)
                    //{
                    //    StoreCode data = new StoreCode();
                    //    data.Code = code;
                    //    data.Name = name;
                    //    data.Location = location;
                    //    data.Status = status;
                    //    data.Remark = "FromClient";
                    //    data.CreatedDate = DateTime.Now;
                    //    context.StoreCodes.Add(data);
                    //    context.SaveChanges();
                    //    msg.msg = "Success";
                    //    msg.eflag = "S";
                    //}
                    //else
                    //{
                    //    scodeObj.Code = code;
                    //    scodeObj.Name = name;
                    //    scodeObj.Location = location;
                    //    scodeObj.Status = status;
                    //    scodeObj.UpdatedDate = DateTime.Now;
                    //    context.SaveChanges();
                    //    msg.msg = "Success";
                    //    msg.eflag = "S";
                    //}
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
