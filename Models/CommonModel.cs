using POSSystem.Utils;
using POSData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
//using System.Text;
using static POSSystem.Models.ViewModel;

namespace POSSystem.Models
{
    public class CommonModel
    {
        POSSystemEntities Context = new POSSystemEntities();
        public class ViewModelSaleHead
        {
            public int id { get; set; }
            public string CustName { get; set; }
            public string InvoiceNo { get; set; }
            public string SaleDate { get; set; }
            public string Address1 { get; set; }
            public string Phone1 { get; set; }
            public string TspName { get; set; }
            public double Balance { get; set; }
        }
        public ViewModelSaleHead GetSaleInvoiceHead(int HeaderID)
        {
            sqlDescriptor sd = new sqlDescriptor();
            var data = sd.GetDataByQuery<ViewModelSaleHead>("Sale/GetInvoiceHeader", new object[] { HeaderID }).FirstOrDefault();
            if (data != null)
            {
                //if ((data.effective_semester ?? "") == "")
                //{
                //    data.effective_semester = "A";
                //}
                //if (data.last_update != null)
                //{
                //    data.last_update = Convert.ToDateTime(data.last_update).ToString("dd MMMM yyy");

                //}
            }
            return data;
        }
        public class ListtoDataTableConverter
        {
            public DataTable ToDataTable<T>(List<T> items)
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
        }
        public class ViewModelSaleDetail
        {
            public int ID { get; set; }
            public int SrNo { get; set; }
            public string StockDescription { get; set; }
            public int Qty { get; set; }
            public double SalePrice { get; set; }
            public double Amount { get; set; }
        }
        public List<ViewModelSaleDetail> GetSaleDeail(int HeaderID)
        {
            List<ViewModelSaleDetail> lst = new List<ViewModelSaleDetail>();
            sqlDescriptor sd = new sqlDescriptor();
            var list = sd.GetDataByQuery<ViewModelSaleDetail>("Sale/GetInvoiceDetail", new object[] { HeaderID }).ToList();
            if (list.Count > 0)
            {
                foreach (ViewModelSaleDetail l in list)
                {
                    lst.Add(new ViewModelSaleDetail
                    {
                        //SrNo = sr + 1,
                        StockDescription = l.StockDescription,
                        Qty = l.Qty,
                        SalePrice = l.SalePrice,
                        Amount = l.Amount
                    });
                }
            }
            return lst;
        }
    }
}