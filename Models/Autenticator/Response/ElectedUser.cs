using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Autenticator.Response
{
    public class ElectedUser : User
    { 
        public string UserId;
        public string CountPosts;
        public string IDApplication;
        public string UserCreatedOn;
        public bool? IsElected;
        public string ElectedDate;
        public string Title;
        public string Comment;
        public int? Star;
        public bool? ElectedUserIsActive;
        public bool? ElectedUserIsDeleted;
        public string StateName; 
        public string Location;
        public string Tel;
        public string Address;

    }
}