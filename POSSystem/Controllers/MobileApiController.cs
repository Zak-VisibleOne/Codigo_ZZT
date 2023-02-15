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
    public class MobileApiController : ApiController
    {
        private POSSystemEntities Context { get; set; }
        public MobileApiController()
        {
            Context = new POSSystemEntities();
        }
        public class ReturnMsg
        {
            public string msg { set; get; }
            public string eflag { set; get; }
        }

        
        
       


    }
}
