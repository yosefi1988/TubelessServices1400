using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
namespace TubelessServices.Models.Loginto
{
    public class LogintoNotification
    {
        public string token { get; set; }
        public string adminMessage { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string lastLogin { get; set; }
        public string Sid { get; set; }
        public int type { get; set; }

    }
}