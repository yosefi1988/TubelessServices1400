using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public string CreatorFullName { get; set; }
        public int TTC { get; set; }
        public string TTN { get; set; }
        public string image { get; set; }
        public string icon { get; set; }

        public string RefrenceNo { get; set; }
        public string IdPost { get; set; }
        public string title { get; set; }
        public long Amount { get; set; }
        public float Zarib{ get; set; }
        public string DateTime{ get; set; }

    }
}