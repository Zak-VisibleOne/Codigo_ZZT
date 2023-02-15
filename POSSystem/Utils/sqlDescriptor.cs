using System;
using System.Collections.Generic;
using System.IO;
using POSData;

namespace POSSystem.Utils
{
    public class sqlDescriptor
    {
        public string sql_debug { get; set; }
        public string message { get; set; }
        private string get_sql(string path)
        {
            string myFile = System.Web.HttpContext.Current.Server.MapPath("~/SQL/") + path + ".sql";
            string extension = "";
            string sql = "";
            try
            {
                if (System.IO.File.Exists(myFile))
                {
                    extension = Path.GetExtension(myFile);
                    TextReader tr = new StreamReader(myFile);
                    if (extension == ".sql")
                    {
                        sql = tr.ReadToEnd();
                    }
                    tr.Close();
                }
                else
                {
                }
                this.sql_debug = sql;
                this.message = "descriptor ok";
            }
            catch (Exception e)
            {
                sql = "no file found";
                this.message = "descriptor error : \n" + e.ToString();
            }
            return sql;
        }

        public List<T> GetDataByQuery<T>(string path, object[] par) where T : class
        {
            return new CommonData<T>().GetDataByQuery(this.get_sql(path), par);
        }
    }
}