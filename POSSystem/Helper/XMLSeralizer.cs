using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace CityU.Helper
{
    public static class XMLSeralizer
    {
        public static string SeralizeObject(object obj)
        {
            StringWriter sw = null;
            System.Xml.Serialization.XmlSerializer xs = null;
            string result = string.Empty;
            try
            {
                sw = new StringWriter();
                xs = new XmlSerializer(obj.GetType());
                xs.Serialize(sw, obj);
                result = sw.ToString();
                result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
            return result;
        }
        public static object DeserializeObject(string xml, Type type)
        {
            StringReader sr = null;
            XmlReader xr = null;
            System.Xml.Serialization.XmlSerializer xs = null;
            object result = null;
            try
            {
                XmlRootAttribute xRoot = new XmlRootAttribute();
                //xRoot.ElementName = "ListOfCategory";    
                // xRoot.Namespace = "http://www.cpandl.com";     
                xRoot.IsNullable = true;
                ///XmlSerializer xs = 


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                xRoot.ElementName = ((System.Xml.XmlElement)(xmlDoc.FirstChild)).Name;
                sr = new StringReader(xmlDoc.OuterXml);
                xr = new XmlTextReader(sr);
                //xs = new XmlSerializer(type);
                xs = new XmlSerializer(type, xRoot);
                result = xs.Deserialize(xr);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return result;
        }
    }
}