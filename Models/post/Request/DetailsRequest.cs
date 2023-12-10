using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.post.Request
{
    public class DetailsRequest
    {
        public int UserCode { get; set; }
        public int IDPost { get; set; }
        public int IDApplication { get; set; }
        public string Store { get; set; }
        public string Ip { get; set; }
    }
}