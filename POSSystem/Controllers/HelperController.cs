//using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
//using System.Data;
//using System.Xml;
//using CityU.Helper;
using POSSystem.Models;

namespace POSSystem.Controllers
{
    public class HelperController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(HelperController));
        #region Private variables
        protected SqlConnection sqlConnection = null;
        protected SqlCommand sqlCommand = null;
        #endregion

        public string strConn = string.Empty;
        public HelperController()
        {
            strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        }
        public string IsSelected(string itemValue, string dataValue)
        {
            return (itemValue == dataValue ? "selected" : null);
        }
        public string IsChecked(string itemValue, string dataValue)
        {
            return (itemValue == dataValue ? "checked" : null);
        }
        public string IsGSTSelected(string itemValue, string dataValue)
        {
            if (itemValue == null)
            {
                itemValue = "*****";
            }
            string code = dataValue.Split('^')[0];
            return (itemValue.Equals(code) ? "selected" : null);
        }
        public string IsRequired(bool itemValue)
        {
            return (itemValue == true ? "required" : null);
        }
        public string IsReadonly(bool itemValue)
        {
            return (itemValue == true ? "readony" : null);
        }       
        public class Select2PagedResult
        {
            public int Total { get; set; }
            public List<Select2> Results { get; set; }
        }
    }
}
