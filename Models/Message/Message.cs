using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Message
{
    public class Message
    {
        public int ApplicationId { get; set; }
        public Int16? SenderUserID { get; set; }
        public Int16? ReciverUserID { get; set; }
        public string MetaData { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public Int16? type { get; set; }
    }
}