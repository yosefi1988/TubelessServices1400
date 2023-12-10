using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Post.Request
{
    public class RegisterPost
    {
        public int IDApplication { get; set; }
        public string UserCode { get; set; }        
        public string Title { get; set; }
        public string Text { get; set; }
        public int ttc { get; set; }
        public int StateCode { get; set; }
        public int CityCode { get; set; }       
        public string Amount { get; set; }
        public bool ReciveMessage { get; set; }        
        public string PublishDate { get; set; }
        public string ExpireDate { get; set; }                
        public string IP { get; set; }
        public List<RegisterPost_Item> Items { get; set; }
        public string Store { get; set; }
    }
}