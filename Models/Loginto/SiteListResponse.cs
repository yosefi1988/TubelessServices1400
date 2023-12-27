using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Controllers.finger;
using TubelessServices.Models.Response;

namespace TubelessServices.Models.Loginto
{

    public class SiteListResponse : ServerResponse
    {
        public List<Site> siteList = new List<Site>();
        public Site site = new Site();
    }


}