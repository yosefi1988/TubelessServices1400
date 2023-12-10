using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Classes.Utility;
using TubelessServices.Controllers.Autenticator;
using TubelessServices.Controllers.Wallet;
using TubelessServices.Models.Response;

namespace TubelessServices.Models.Autenticator.Response
{
    public class LoginResponse : ServerResponse
    {
        public LoginResponse()
        {
            tubelessException.code = 200;
            tubelessException.message = "ok";        
        }

        public long UserCode;
        public string CodeMelli;
        public string UserName;
        public string Email;
        public string Mobile;
        public bool MobileNumberConfirmed;
        //public long UserPassword;
        public string SimcardID;
        public string Name;
        public string Family;
        public string Avatar;
        public string ProfileImage;
        public int UserTypeCode;
        public bool IsActive;
        public bool IsDeleted;
        public string CreateDate; 

        public UserWallet wallet;

        internal void setUser(Tbl_User user , Boolean chekBazar)
        {
            if (user == null)
            {

            }
            else
            {
                this.UserCode = (long)user.UserCode;
                this.UserName = user.UserName;
                this.CodeMelli = user.CodeMelli;
                this.Email = user.Email;
                this.Mobile = user.Mobile;
                this.MobileNumberConfirmed = (bool)user.PhoneNumberConfirmed;
                //this.UserPassword = user.Password;
                this.SimcardID = user.SimCardId;
                this.Name = user.Name;
                this.Family = user.Family;
                this.Avatar = user.Avatar;
                this.ProfileImage = user.ProfileImage;

                if (chekBazar)
                {
                    this.UserTypeCode = 1;
                }
                else
                {
                    this.UserTypeCode = (int)user.UserTypeCode;
                }
                this.IsActive = user.IsActive;
                this.IsDeleted = user.IsDeleted;
                this.CreateDate = Date.convertToPersianDate(user.CreatedOn);

                WalletCRUD userCRUD = new WalletCRUD();
                this.wallet = new UserWallet();
                wallet.Amount = (long)userCRUD.getWallet(user).Amount;
            }
        }
        internal void setUser(Viw_ElectedUserList user)
        {
            if (user == null)
            {

            }
            else
            {
                this.UserCode = (long)user.UserCode;
                this.UserName = user.UserName;
                this.CodeMelli = user.CodeMelli;
                this.Email = user.Email;
                this.Mobile = user.Mobile;
                this.MobileNumberConfirmed = (bool)user.PhoneNumberConfirmed;
                this.SimcardID = user.SimCardId;
                this.Name = user.Name;
                this.Family = user.Family;
                this.Avatar = user.Avatar;
                this.ProfileImage = user.ProfileImage;
                this.UserTypeCode = (int)user.UserTypeCode;
                this.IsActive = (bool) user.UsersIsActive;
                this.IsDeleted = (bool)user.UsersIsDelete;
                this.CreateDate = Date.convertToPersianDate(user.CreatedOn);



                 
            }
        }

    }
}