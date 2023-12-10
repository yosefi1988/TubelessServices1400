using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Post
{
    public class PostItem
    {
        public int ID { get; set; }
        public string CreatorFullName { get; set; }
        public int TTC { get; set; }
        public string TTN { get; set; }
        public string image { get; set; }
        public string icon { get; set; }

        public string RefrenceNo { get; set; }
        public string title { get; set; }
        public long Amount { get; set; }
        public float Zarib{ get; set; }
        public string DateTime{ get; set; }
        public string DateTimeExpire{ get; set; }

        public string StateName{ get; set; }
        public string CityName{ get; set; }

        public bool ReciveMessage { get; set; }
        public bool isFav { get; set; }
        public bool isSeen { get; set; }

        public string TitlePicture { get; set; }
        public string TextPicture { get; set; }

    }
}