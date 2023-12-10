using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models.post;

namespace TubelessServices.Models.Post
{
    public class PostFullItem : PostItem
    {
        public int CreatorID { get; set; }
        public int ApplicationID { get; set; }
        public string Text { get; set; }
        public int StateCode { get; set; }
        public string StateName { get; set; }
        public int CityCode { get; set; }
        public string CityName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool ReciveMessage { get; set; }

        public string PublishDate { get; set; }
        //public string ExpireDate { get; set; }
        public string AcceptDate { get; set; }
        //public string CreateDate { get; set; }
        public string ModifiedDate { get; set; }
        public int ViewCount { get; set; }

        //public List<PostAmount> Amounts = new List<PostAmount>();
        public List<PostAmount> Amounts;

        public List<String> Images;

    }
}