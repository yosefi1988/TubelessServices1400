using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models.Autenticator.Response;
using TubelessServices.Models.Post;
using TubelessServices.Models.Response;

namespace TubelessServices.Models.post.Response
{ 
    public class ResponsePost : ServerResponse
    {
        public ResponsePost()
        {
            tubelessException.code = 200;
            tubelessException.message = "ok";
        }
        public UserWallet wallet;
        public PostFullItem PostItem;
        public User creator;
    }
}