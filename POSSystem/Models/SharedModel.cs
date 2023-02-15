namespace POSSystem.Models
{
    public class SharedModel
    {
        
    }
    public class ResultMessage
    {
        public string result { get; set; }
        public string message { get; set; }
        public int cnt { get; set; }
        public double quantity { get; set; }
        public object data { get; set; }
        public object datalist { get; set; }
    }
    public class Select2
    {
        public string id { get; set; }
        public string text { get; set; }
        public string title { get; set; }
    }
    public class TrnNos
    {
        public int? TrnNo { get; set; }
        public int? AutoDocNo { get; set; }
        public string DocNo { get; set; }
        public string status { get; set; }
    }
}