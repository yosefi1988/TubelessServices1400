using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Autenticator.Request
{
    public class reqTransaction
    {
        public int IDApplication { get; set; }
        public int IDApplicationVersion { get; set; }
        public string AndroidID { get; set; }
        public string IP { get; set; }


        public string UserCode { get; set; }
        public string Amount { get; set; }
        public string AmountMin { get; set; }
        public string AmountMax { get; set; }

        public string Date { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }
        

        public string refrenceNo { get; set; }
        public int ttc { get; set; }

        public string metaData{ get; set; }

        public int pageSize { get; set; }
        public int pageIndex { get; set; }

    }
}