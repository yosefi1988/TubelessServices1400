using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TubelessServices.Controllers.Autenticator
{
    public class UserHelper
    {
        public enum userNameType { email = 1, mobile = 2, simcard = 3, username = 4, codeMelli = 5 }
        internal userNameType getUserNameType(string username)
        {     
            //todo fix v2 09999816652       
            Regex regexMobile = new Regex(@"(0|\+98)?([ ]|-|[()]){0,2}9[1|2|3|4|9|0]([ ]|-|[()]){0,2}(?:[0-9]([ ]|-|[()]){0,2}){8}");
            Regex regexEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (regexMobile.IsMatch(username))
                return userNameType.mobile;

            if (regexEmail.IsMatch(username))
                return userNameType.email;

            if (check_codeMelli(username))
                return userNameType.codeMelli;

            if (username.Contains("simcard:"))
                return userNameType.simcard;

            return userNameType.username;
        }


        public bool check_codeMelli(string nationalCode)
        {
            try
            {                 
                //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد                 
                var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
                if (allDigitEqual.Contains(nationalCode))
                    return false;

                //عملیات شرح داده شده در بالا                 
                var chArray = nationalCode.ToCharArray();
                var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
                var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
                var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
                var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
                var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
                var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
                var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
                var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
                var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
                var a = Convert.ToInt32(chArray[9].ToString());
                var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
                var c = b % 11;
                return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
            }
            catch
            {
                return false;
            }
        }
    }
}