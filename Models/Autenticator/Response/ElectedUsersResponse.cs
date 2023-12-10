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
    public class ElectedUsersResponse : ServerResponse
    {
        public ElectedUsersResponse()
        {
            tubelessException.code = 200;
            tubelessException.message = "ok";        
        }

        public List<ElectedUser> electedUserList = new List<ElectedUser>();

        internal void setUserHalf(List<Viw_ElectedUserList> userList)
        {
            foreach(var user in userList)
            {
                ElectedUser NItem = new ElectedUser();
                if (user == null)
                {

                }
                else
                {
                    NItem.UserId = user.UserId.ToString();
                    NItem.Title = user.Title;
                    NItem.Name = user.Name;
                    NItem.Family = user.Family;
                    NItem.Avatar = user.Avatar;
                    NItem.Star = user.Star;
                    NItem.StateName = user.StateName;
                    this.electedUserList.Add(NItem);
                }
            }
        }

        internal void setUserFull(List<Viw_ElectedUserList> userList)
        {
            foreach (var user in userList)
            {
                ElectedUser NItem = new ElectedUser();
                if (user == null)
                {

                }
                else
                {
                    NItem.Name = user.Name;
                    NItem.Family = user.Family;
                    NItem.Avatar = user.Avatar;
                    NItem.ProfileImage = user.ProfileImage;
                    NItem.UserTypeCode = user.UserTypeCode;
                    NItem.Email = user.Email;
                    NItem.Mobile = user.Mobile;
                    NItem.MobileNumberConfirmed = user.PhoneNumberConfirmed;
                    NItem.UserCode = user.UserCode;
                    NItem.CodeMelli = user.CodeMelli;
                    NItem.UserName = user.UserName;
                    NItem.SimcardID = user.SimCardId;
                    NItem.Location = user.Location;
                    NItem.Tel = user.Tel;
                    NItem.Address = user.Address;

                    NItem.UserIsActive = user.UsersIsActive;
                    NItem.UserIsDeleted = user.UsersIsDelete;

                    if (user.CreatedOn != null)
                        NItem.UserCreateDate = Date.convertToPersianDate(user.CreatedOn);
                    NItem.UserId = user.UserId.ToString();

                    NItem.CountPosts = user.CountPosts.ToString();
                    NItem.IDApplication = user.IDApplication.ToString();
                    NItem.UserCreatedOn = user.UserCreatedOn.ToString();
                    NItem.IsElected = user.IsElected;

                    if (user.ElectedDate != null)
                        NItem.ElectedDate = Date.convertToPersianDate(user.ElectedDate);

                    NItem.Title = user.Title;
                    NItem.Comment = user.Comment;
                    NItem.Star = user.Star;
                    NItem.ElectedUserIsActive = user.ElectedUsersIsActive;
                    NItem.ElectedUserIsDeleted = user.ElectedUsersIsDelete;
                    NItem.StateName = user.StateName;
                    this.electedUserList.Add(NItem);
                }
            }
        }
    }
}