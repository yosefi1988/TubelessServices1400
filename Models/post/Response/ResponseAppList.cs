using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models.Autenticator.Response;
using TubelessServices.Models.Post;
using TubelessServices.Models.Response;

namespace TubelessServices.Models.post.Response
{ 
    public class ResponseAppList : ServerResponse
    {
        public List<Viw_Site_AppList> postList;

    }
}