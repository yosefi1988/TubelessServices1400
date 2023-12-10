using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TubelessServices.Models;
using TubelessServices.Models.Autenticator.Request;
using static TubelessServices.Controllers.Autenticator.UserHelper;

namespace TubelessServices.Controllers.Autenticator
{
    public class UserCRUD
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        UserHelper userHelper = new UserHelper();


        internal Tbl_User insertUser(Login loginAccount)
        {
            Tbl_User tbl_user = new Tbl_User();        

            tbl_user.PhoneNumberConfirmed = false;
            tbl_user.IsActive = true;
            tbl_user.IsDeleted = false;
            tbl_user.CreatedOn = DateTime.Now;
            tbl_user.UserTypeCode = 1;

            userNameType type = userHelper.getUserNameType(loginAccount.UserName);
            switch (type)
            {
                case userNameType.email:
                    tbl_user.Email = loginAccount.UserName;
                    break;
                case userNameType.mobile:
                    tbl_user.Mobile = loginAccount.UserName;
                    tbl_user.Password = (loginAccount.Password);
                    break;
                case userNameType.simcard:
                    tbl_user.SimCardId = loginAccount.UserName;
                    break;
                case userNameType.username:
                    tbl_user.UserName = loginAccount.UserName;
                    tbl_user.Password = (loginAccount.Password);
                    break;
                case userNameType.codeMelli:
                    tbl_user.CodeMelli = loginAccount.UserName;
                break;
            }
            //tbl_user.Name = null;
            //tbl_user.Family = null;
            //tbl_user.ProfileImage = (loginAccount.ProfileImage); 

            tbl_user.Avatar = (loginAccount.UserImage);

            db.Tbl_Users.InsertOnSubmit(tbl_user);
            db.SubmitChanges();
            return tbl_user;
        }

        internal int getIdApplication(int iDApplicationVersion)
        {
            return (from x in db.Tbl_ApplicationVersions where x.Id == iDApplicationVersion select x).ToList().First().ApplicationID;
        }


        internal int getUserId(int userCode)
        {
            return findUserByUserCode(userCode).FirstOrDefault().Id;
        }
         
        internal int getUserId(string userCode)
        {
            try
            {
                return findUserByUserCode(userCode).FirstOrDefault().Id;
            }
            catch
            {
                return 0;
            }
        }

        internal List<Tbl_User> findUserByUserCode(int userCode)
        {
            return findUserByUserCode(userCode+"");
        }
        internal List<Tbl_User> findUserByID(int userId)
        {
            return (from x in db.Tbl_Users where x.Id.Equals(userId) select x).ToList();
        }

        internal List<Tbl_User> findUserByUserCode(string userCode)
        {
            return (from x in db.Tbl_Users where x.UserCode.Equals(userCode) select x).ToList();
        }

        internal List<Viw_ElectedUserList> findElectedUserByUserCode(string userCode , int iDApplication)
        {
            return (from x in db.Viw_ElectedUserLists
                    where 
                    x.UserCode.Equals(userCode) &&
                    x.IDApplication == iDApplication &&
                    x.UsersIsActive == true && 
                    x.UsersIsDelete == false &&
                    x.ElectedUsersIsDelete == false &&
                    x.ElectedUsersIsActive == true 
                    select x).ToList();
        }
        internal List<Viw_ElectedUserList> findElectedUserByID(string id , int iDApplication)
        {
            return (from x in db.Viw_ElectedUserLists
                    where 
                    x.UserId.Equals(id) &&
                    x.IDApplication == iDApplication &&
                    x.UsersIsActive == true && 
                    x.UsersIsDelete == false &&
                    x.ElectedUsersIsDelete == false &&
                    x.ElectedUsersIsActive == true 
                    select x).ToList();
        }

        internal List<Tbl_User> findUserByMobile(string number)
        {
            return (from x in db.Tbl_Users where x.Mobile.Equals(number) select x).ToList();
        }
        internal List<Viw_userDevice> findUserByDeviceID(string androidID)
        {
            return (from x in db.Viw_userDevices where x.DeviceID.Equals(androidID) select x).ToList();
        }
        
        internal List<Tbl_User> findUserByUserName(String username)
        {
            userNameType type = userHelper.getUserNameType(username);
            switch (type)
            {
                case userNameType.email:
                    return (from x in db.Tbl_Users
                            where x.Email.Equals(username)                        
                            select x).ToList();
                case userNameType.mobile:
                    return (from x in db.Tbl_Users
                            where x.Mobile.Equals(username)
                            select x).ToList();

                case userNameType.simcard:
                    return (from x in db.Tbl_Users
                            where x.SimCardId.Equals(username)
                            select x).ToList();

                case userNameType.username:
                    return (from x in db.Tbl_Users
                            where x.UserName.Equals(username)
                            select x).ToList();
                case userNameType.codeMelli:
                    return (from x in db.Tbl_Users
                            where x.CodeMelli.Equals(username)
                            select x).ToList();
            }
            return null;
        }


        internal List<Viw_ElectedUserList> findElectedUserByUserName(String username,int iDApplication)
        {
            userNameType type = userHelper.getUserNameType(username);
            switch (type)
            {
                case userNameType.email:
                    return (from x in db.Viw_ElectedUserLists
                            where x.Email.Equals(username) &&
                    x.IDApplication == iDApplication &&
                    x.UsersIsActive == true &&
                    x.UsersIsDelete == false &&
                    x.ElectedUsersIsDelete == false &&
                    x.ElectedUsersIsActive == true
                            select x).ToList();
                case userNameType.mobile:
                    return (from x in db.Viw_ElectedUserLists
                            where x.Mobile.Equals(username) &&
                    x.IDApplication == iDApplication &&
                    x.UsersIsActive == true &&
                    x.UsersIsDelete == false &&
                    x.ElectedUsersIsDelete == false &&
                    x.ElectedUsersIsActive == true
                            select x).ToList();

                case userNameType.simcard:
                    return (from x in db.Viw_ElectedUserLists
                            where x.SimCardId.Equals(username) &&
                    x.IDApplication == iDApplication &&
                    x.UsersIsActive == true &&
                    x.UsersIsDelete == false &&
                    x.ElectedUsersIsDelete == false &&
                    x.ElectedUsersIsActive == true
                            select x).ToList();

                case userNameType.username:
                    return (from x in db.Viw_ElectedUserLists
                            where x.UserName.Equals(username) &&
                    x.IDApplication == iDApplication &&
                    x.UsersIsActive == true &&
                    x.UsersIsDelete == false &&
                    x.ElectedUsersIsDelete == false &&
                    x.ElectedUsersIsActive == true
                            select x).ToList();
                case userNameType.codeMelli:
                    return (from x in db.Viw_ElectedUserLists
                            where x.CodeMelli.Equals(username) &&
                    x.IDApplication == iDApplication &&
                    x.UsersIsActive == true &&
                    x.UsersIsDelete == false &&
                    x.ElectedUsersIsDelete == false &&
                    x.ElectedUsersIsActive == true
                            select x).ToList();
            }
            return null;
        }


        internal void insertUserApp(Login loginAccount, long newUserId)
        {
            try
            {
                Tbl_UserApp tbl_userApp = new Tbl_UserApp();
                tbl_userApp.IDUser = (int)newUserId;
                tbl_userApp.IDApplicationVersion = loginAccount.IDApplicationVersion;
                tbl_userApp.LastLoginOn = DateTime.Now;
                if (loginAccount.UserMoarefID != null && loginAccount.UserMoarefID.Length != 0)
                {
                    tbl_userApp.IDUserMoaref = int.Parse(loginAccount.UserMoarefID);
                }

                db.Tbl_UserApps.InsertOnSubmit(tbl_userApp);
                db.SubmitChanges();

                //WriteToFile("testFile.txt", " insertUserApp mehod  ok:" + "newUserId:" + newUserId, login);
            }
            catch (Exception ex)
            {
                string sssssssssss = "\n";
                sssssssssss = sssssssssss + ex.Data.ToString() + "\n";
                sssssssssss = sssssssssss + ex.Data.Values.ToString() + "\n";

                sssssssssss = sssssssssss + ex.HelpLink + "\n";
                sssssssssss = sssssssssss + ex.HResult + "\n";
                sssssssssss = sssssssssss + ex.InnerException + "\n";
                sssssssssss = sssssssssss + ex.Message + "\n";
                sssssssssss = sssssssssss + ex.Source + "\n";
                sssssssssss = sssssssssss + ex.StackTrace + "\n";

                //WriteToFile("testFile.txt", " insertUserApp mehod not ok:" + "newUserId:" + newUserId + sssssssssss, loginAccount);
            }

        }

        internal String getTansactionValue(int id)
        {
            Tbl_DetailsLookup userWallet = (from x in db.Tbl_DetailsLookups
                                            where x.Id == id
                                        select x).ToList().FirstOrDefault();

            return userWallet.Value;
        }

        internal void updateUserApp(Tbl_UserApp userApp, int iDApplicationVersion)
        {
            Tbl_UserApp Tbl_Common_UserAppUpdate = userApp;
            Tbl_Common_UserAppUpdate.LastLoginOn = DateTime.Now;
            //Tbl_Common_UserAppUpdate.IDApplicationVersion = iDApplicationVersion;
            db.SubmitChanges();
        }

        public bool confirmUserMobile(int idUser)
        {
            try
            {
                var user = (from record in db.Tbl_Users
                            where record.Id == idUser
                            select record).First();

                bool currentState = user.PhoneNumberConfirmed;
                user.PhoneNumberConfirmed = true;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool resetPassword(int idUser)
        {
            try
            {
                var user = (from record in db.Tbl_Users
                            where record.Id == idUser
                            select record).First();

                bool currentState = user.PhoneNumberConfirmed;
                user.PhoneNumberConfirmed = false;
                user.Password = user.Mobile.Substring(user.Mobile.Length - 4, 4);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Viw_ElectedUserList> findElectedUserByApplicationId(string applicationId)
        {
            return (
                from x in db.Viw_ElectedUserLists
                where 
                x.IDApplication.Equals(applicationId) && 
                x.UsersIsActive == true &&
                x.UsersIsDelete == false &&
                x.ElectedUsersIsActive == true &&
                x.ElectedUsersIsDelete == false &&
                x.F_UserId != null
                select x
                ).ToList();
        }
        public bool changePassword(int idUser,string oldPasswors, string newPasswors)
        {
            try
            {
                var user = (from record in db.Tbl_Users
                            where record.Id == (idUser)
                            select record).First();

                if(user.Password == oldPasswors)
                {
                    user.Password = newPasswors;
                }
                else { return false; }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}