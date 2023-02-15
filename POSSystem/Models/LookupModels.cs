using System.Collections.Generic;

namespace POSSystem.Models
{
    public class LookupModels
    {
        public LookupModels()
        { }
        public LookupModels(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string RefNo { get; set; }
        public string SalesOrder { get; set; }
    }
    public class select2option
    {
        public select2option()
        { }
        public select2option(string Text, string ID)
        {
            text = Text;
            id = ID;
        }
        public string id { get; set; }
        public string text { get; set; }

    }
    public class ValidationMessage
    {
        public ValidationMessage()
        { }
        public ValidationMessage(int row, string msg)
        {
            Row = row;
            Message = msg;
        }

        public int Row { get; set; }
        public string Message { get; set; }
    }
    public class ValidationMessageResult
    {
        public string Status { get; set; }
        public List<ValidationMessage> ValidationMessages { get; set; }
    }
}