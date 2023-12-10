using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Autenticator.Response
{
    public class User
    {
        public string Name;
        public string Family;
        public string Avatar;
        public string ProfileImage;
        public string Mobile;
        public string Email;
        public bool? MobileNumberConfirmed;
        public long? UserCode;
        public string CodeMelli;
        public string UserName;
        public string SimcardID;
        public int? UserTypeCode;
        public bool? UserIsActive;
        public bool? UserIsDeleted;
        public string UserCreateDate;

    }
}