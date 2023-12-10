using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Post.Request
{
    public class reqPostList
    {
        public string UserCode { get; set; }

        public string PublishDateTo { get; set; }
        public string PublishDate { get; set; }
        public string PublishDateFrom { get; set; }
        public string ExpireDate { get; set; }
        public string ExpireDateFrom { get; set; }
        public string ExpireDateTo { get; set; }
        public string CreatorUserCode { get; set; }
        public int IDApplication { get; set; }
        public int ttc { get; set; }
        public int StateCode { get; set; }
        public int CityCode { get; set; }
        public int PriceForVisit { get; set; }
        public int PriceForVisitMin { get; set; }
        public int PriceForVisitMax { get; set; }
        public bool IsActive { get; set; }
        public bool Visited { get; set; }
        public bool Faved { get; set; }


        //public bool IsDelete { get; set; }
        //public bool ReciveMessage { get; set; }







        public string Search { get; set; }          







        


        public int pageSize { get; set; }
        public int pageIndex { get; set; }

    }
}
