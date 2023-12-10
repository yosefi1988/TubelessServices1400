using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Autenticator.Request
{
    public class Login
    {
        public int IDApplicationVersion { get; set; }
        public int IDApplication { get; set; }
        public string AndroidID { get; set; }
        public string IP { get; set; }
        public string ID { get; set; }


        public string UserCode { get; set; }
        public string UserName { get; set; }

        //public string Email { get; set; }
        //public string MobileNumber { get; set; }
        //public string SimcardID { get; set; }
        //public string ProfileImage { get; set; }

        public string UserImage { get; set; }

        public string Password { get; set; }
        public string UserMoarefID { get; set; }
        public string MetaData { get; set; }

        public string ToString()
        {
            string retVal =

                "AndroidID:" + this.AndroidID +
                "UserName:" + this.UserCode;
                //"Password:" + "***" +
                //"Email:" + this.Email +
                //"MobileNumber:" + this.MobileNumber +
                //"UserImage:" + this.UserImage +
                //"ProfileImage:" + this.ProfileImage +
                //"IDApplicationVersion:" + this.IDApplicationVersion +
                //"UserMoarefID" + this.UserName;
            return retVal;
        }

    }
}