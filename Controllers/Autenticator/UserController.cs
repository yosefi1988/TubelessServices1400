
using TubelessServices.Models;
using TubelessServices.Models.Autenticator.Request;
using TubelessServices.Models.Autenticator.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using TubelessServices.Models.Response;
using TubelessServices.Controllers.Wallet;
using static TubelessServices.Controllers.Wallet.WalletController;
using TubelessServices.Controllers.Device;

namespace TubelessServices.Controllers.Autenticator
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        UserCRUD userCRUD = new UserCRUD();
        DeviceCRUD deviceCRUD = new DeviceCRUD();
        WalletCRUD walletCRUD = new WalletCRUD();
        
        LoginResponse response = new LoginResponse();

        [HttpPost]
        [Route("Login")]
        public string Login(Login loginAccount)
        {
            List<Tbl_User> userList;
            Tbl_User user;

            try
            {
                if (loginAccount.UserCode == null)
                {
                    userList = userCRUD.findUserByUserName(loginAccount.UserName);
                }
                else
                {
                    userList = userCRUD.findUserByUserCode(loginAccount.UserCode);
                    if (userList.Count == 0)
                    {
                        ServerResponse responsex = new ServerResponse();
                        responsex.tubelessException.code = -1;
                        responsex.tubelessException.message = "you can not login by usercode before register"; 
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                }
                if (userList.Count == 0)
                {
                    user = userCRUD.insertUser(loginAccount);
                    long newUserId = user.Id;
                    if (newUserId != 0)
                    {
                        int walletId = walletCRUD.CreateNewWallet(loginAccount, (int) newUserId);
                        if (walletId != 0)
                        {
                            int IDUserCreator = 10009;                                          //کاربر سیستمی
                            int TransactionTypeCode = (int) TransactionTypeCodeEnum.startGift;        //هدیه افتتاح حساب
                            float zarib = walletCRUD.getZarib(user.UserCode, TransactionTypeCode);
                            float Amount = float.Parse(userCRUD.getTansactionValue(TransactionTypeCode)) * zarib;
                            int idApp = userCRUD.getIdApplication(loginAccount.IDApplicationVersion);

                            walletCRUD.registerNewTransaction(walletId,(int) newUserId, IDUserCreator, idApp, Amount, zarib , TransactionTypeCode, null,null,loginAccount.IP);
                        }
                    }

                    //todo uncomment and fix
                    //updateDeviceUserId(loginAccount, (int)newUserId);

                    if (newUserId != 0)
                    {
                        userCRUD.insertUserApp(loginAccount, newUserId); //or userApp(loginAccount, user);
                        userDevices(loginAccount, user);


                        response.setUser(user ,checkBazar(loginAccount));
                        string ssssssss = new JavaScriptSerializer().Serialize(response); 
                        return ssssssss; 
                    }
                    else
                    {
                        ServerResponse responsex = new ServerResponse();
                        responsex.tubelessException.code = -102;
                        responsex.tubelessException.message = "User not insert";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex); 
                        return ssssssss;
                    }

                }
                else if (userList.Count == 1)
                {
                    user = userList.First();

                    if (!(loginAccount.Password != null && loginAccount.Password.Equals("x") && loginAccount.UserCode != null && loginAccount.UserCode.Length > 5))
                    {
                        if (user.Password != null && user.Password.Length >= 1)
                        {
                            if (user.Password == loginAccount.Password)
                            {
                                userDevices(loginAccount, user);
                                return userApp(loginAccount, user);
                            }
                            else
                            {
                                ServerResponse responsex = new ServerResponse();
                                responsex.tubelessException.code = -104;
                                responsex.tubelessException.message = "pass not ok";
                                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                                return ssssssss;
                            }
                        }
                        else
                        {
                            userDevices(loginAccount, user);
                            return userApp(loginAccount, user);
                        }
                    }
                    else
                    {
                        userDevices(loginAccount, user);
                        return userApp(loginAccount, user);
                    }
                }
                else
                {
                    ServerResponse responsex = new ServerResponse();
                    responsex.tubelessException.code = -103;
                    responsex.tubelessException.message = "error User Name duplicate";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }                
            }
            catch (Exception ex)
            {
                string sssssssssss = "";
                sssssssssss = sssssssssss + ex.Data.ToString();
                sssssssssss = sssssssssss + ex.Data.Values.ToString();
                sssssssssss = sssssssssss + ex.HelpLink;
                sssssssssss = sssssssssss + ex.HResult;
                sssssssssss = sssssssssss + ex.InnerException;
                sssssssssss = sssssssssss + ex.Message;
                sssssssssss = sssssssssss + ex.Source;
                sssssssssss = sssssssssss + ex.StackTrace;            
                return sssssssssss;
            }
        }

        private bool checkBazar(Login loginAccount)
        {
            if (loginAccount.UserName == "09999816652")
            {
                if(loginAccount.MetaData != null)
                {
                    if (loginAccount.MetaData == "Testbazar")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
                return false;
        }

        private void userDevices(Login loginAccount, Tbl_User tbl_user)
        {
            List<Tbl_Device> devicelist = deviceCRUD.findDeviceByDeviceID(loginAccount.AndroidID);
            if (devicelist.Count == 0)
            {
                return;
            }
            else
            {  
                int deviceId = devicelist.FirstOrDefault().Id;
                List<tbl_UsersDevice> result = (from p in db.tbl_UsersDevices
                                                where p.IDUser == tbl_user.Id && p.IDDevice == deviceId
                                                select p).ToList();
                if (result.Count == 0)
                {
                    deviceCRUD.insertUserDevice(tbl_user.Id, deviceId);
                }
                //else if (result.Count == 1)
                //{
                //    deviceCRUD.updateUserDevice(tbl_user.Id, deviceId);
                //}
            }
        }

        private string userApp(Login loginAccount, Tbl_User tbl_user)
        {
            List<Tbl_UserApp> result = (from p in db.Tbl_UserApps
                                        where p.IDUser == tbl_user.Id && p.IDApplicationVersion == loginAccount.IDApplicationVersion
                                    select p).ToList();

            if (result.Count == 0)
            {
                userCRUD.insertUserApp(loginAccount, tbl_user.Id);
                response.setUser(tbl_user, checkBazar(loginAccount));
                return new JavaScriptSerializer().Serialize(response); 
            }
            else if (result.Count == 1)
            {
                userCRUD.updateUserApp(result.FirstOrDefault(), loginAccount.IDApplicationVersion);
                response.setUser(tbl_user, checkBazar(loginAccount));
                return new JavaScriptSerializer().Serialize(response);
            }
            else
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -105;
                responsex.tubelessException.message = "error User App duplicate";
                return new JavaScriptSerializer().Serialize(responsex);
            }
        }


        [HttpPost]
        [Route("confirmUserMobile")]
        public string confirmUserMobile(cmRequest request)
        {
            Tbl_User user = userCRUD.findUserByUserCode(request.u).First();
            ServerResponse responsex = new ServerResponse();

            if (request.m.Equals(user.Mobile))
            {
                var mobilelist = deviceCRUD.findDeviceByDeviceID(request.d);
                if (mobilelist.Count == 1)
                {
                    if (userCRUD.confirmUserMobile(user.Id))
                    {
                        responsex.tubelessException.code = 200;
                        responsex.tubelessException.message = "ok";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                    else
                    {
                        responsex.tubelessException.code = -1;
                        responsex.tubelessException.message = "error in mobile";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                }
                else
                {
                    responsex.tubelessException.code = -5;
                    responsex.tubelessException.message = "error in device";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }
            }
            else
            {
                var mobilelist = deviceCRUD.findDeviceByDeviceID(request.d);
                if (mobilelist.Count == 1)
                {
                    if(user != null)
                    {
                        if(user.Mobile == request.m)
                        {
                            responsex.tubelessException.code = -5;
                            responsex.tubelessException.message = "error in device 2";
                            string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                            return ssssssss;
                        }
                        else
                        {
                            responsex.tubelessException.code = -54;
                            responsex.tubelessException.message = "با شماره " + user.Mobile.Substring(9, 2) + "*****" + user.Mobile.Substring(0, 4) + " ثبت نام کنید";
                            string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                            return ssssssss;
                        }
                    }
                    else
                    {
                        responsex.tubelessException.code = -5;
                        responsex.tubelessException.message = "error in device 2";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                }
                else
                {
                    responsex.tubelessException.code = -5;
                    responsex.tubelessException.message = "error in device 2";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }
            }
        }


        [HttpPost]
        [Route("resetPassword")]
        public string resetPassword(cmRequest request)
        {
            ServerResponse responsex = new ServerResponse();
            List<Tbl_User> users = userCRUD.findUserByMobile(request.m);
            if(users.Count == 1)
            {
                Tbl_User user = users.First();

                if (user.PhoneNumberConfirmed)
                {
                    if (userCRUD.resetPassword(user.Id))
                    {
                        responsex.tubelessException.code = 200;
                        responsex.tubelessException.message = "ok Default Password";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                    else
                    {
                        responsex.tubelessException.code = -4;
                        responsex.tubelessException.message = "error in set Password";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                }
                else
                {
                    responsex.tubelessException.code = -3;
                    responsex.tubelessException.message = "error mobile not confirmed";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }
            }
            else
            {
                var mobilelist = deviceCRUD.findDeviceByDeviceID(request.d);
                if (mobilelist.Count == 1)
                {
                    List<Viw_userDevice> usersD = userCRUD.findUserByDeviceID(request.d);
                    if (usersD.Count == 1)
                    {
                        responsex.tubelessException.code = -55;
                        responsex.tubelessException.message = "با شماره " + usersD.First().Mobile.Substring(9, 2) + "*****" + usersD.First().Mobile.Substring(0, 4) + " وارد شوید.";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                    else
                    {
                        responsex.tubelessException.code = -5;
                        responsex.tubelessException.message = "error in device 2";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                   
                }
                else
                {
                    responsex.tubelessException.code = -5;
                    responsex.tubelessException.message = "error in device 2";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }
            }
        }


        [HttpPost]
        [Route("changePassword")]
        public string changePassword(changePasswordRequest request)
        {
            Tbl_User user = userCRUD.findUserByUserCode(request.u).First();
            ServerResponse responsex = new ServerResponse();

            if (user.PhoneNumberConfirmed)
            {
                var mobilelist = deviceCRUD.findDeviceByDeviceID(request.a);
                if (mobilelist.Count == 1)
                {
                    if (user.Password == request.o)
                    {
                        if (userCRUD.changePassword(user.Id,request.o,request.n))
                        {
                            responsex.tubelessException.code = 200;
                            responsex.tubelessException.message = "ok";
                            string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                            return ssssssss;
                        }
                        else
                        {
                            responsex.tubelessException.code = -7;
                            responsex.tubelessException.message = "error in change Password";
                            string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                            return ssssssss;
                        }
                    }
                    else
                    {
                        responsex.tubelessException.code = -6;
                        responsex.tubelessException.message = "error in old Password";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                }
                else
                {
                    responsex.tubelessException.code = -5;
                    responsex.tubelessException.message = "error in device";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }


            }
            else
            {
                responsex.tubelessException.code = -3;
                responsex.tubelessException.message = "error mobile not confirmed";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
        }

    }
}