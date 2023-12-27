using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using TubelessServices.Classes;
using TubelessServices.Classes.Utility;
using TubelessServices.Models;
using TubelessServices.Models.Loginto;
using TubelessServices.Models.Response;

namespace TubelessServices.Controllers.finger
{
    public class fingerController : ApiController
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

        private const string URL = "https://fcm.googleapis.com/fcm/send";
        //private const string URL = "http://localhost:3071/Api/Blog/NewsList?count=100";
        //private string DATA;// = @"{""data"": {""message"": ""{""ServiceType"":1}""},""to"": ""cbTxtjbVWE8:APA91bGzitWQk3z4E-yqJW6O7DLnij4Y6xtXcdcfLfYaOKQ-gwSh3kk4fjCW0zL1U0_kEFD8e-l7RUP2pWwC1FlidDEc-FuQttw2eNe66JxGkSbG5FwqlReum_thvwLvJRu8VfvUDwCcxjz9Z-sK_CZkq6fkYFT6kw""}";

        LoginRequest loginRequest = new LoginRequest();


        public async Task<string> Read(String rcode, String mcode)
        {

            var searchedSiteItem = (from p in db.Tbl_Loginto_Sites
                                    where p.ID.Equals(rcode)
                                    select p).FirstOrDefault();
            if (searchedSiteItem != null)
            {
                var searchedDeviceItem = (from p in db.Tbl_Devices
                                          where p.DeviceID.Equals(mcode)
                                          select p).FirstOrDefault();
                if (searchedDeviceItem != null)
                {
                    Tbl_Loginto_DeviceSite searchedDeviceSite = (from p in db.Tbl_Loginto_DeviceSites
                                              where p.DeviceID.Equals(searchedDeviceItem.Id) && p.SiteID.Equals(searchedSiteItem.ID)
                                              select p).FirstOrDefault();
                    if (searchedDeviceSite == null)
                    {
                        Tbl_Loginto_DeviceSetting setting = (from p in db.Tbl_Loginto_DeviceSettings
                                                  where p.DeviceID.Equals(searchedDeviceItem.Id) 
                                                  select p).FirstOrDefault();
                        if (setting == null)
                        {
                            ServerResponse response = new ServerResponse();
                            response.tubelessException.code = -1120;
                            response.tubelessException.message = "setting not found";
                            return new JavaScriptSerializer().Serialize(response);
                        }
                        else
                        {

                            if (setting.AutoAdd == true)
                            {
                                //add to device site
                                Tbl_Loginto_DeviceSite newDeviceSite = new Tbl_Loginto_DeviceSite();
                                newDeviceSite.DeviceID = searchedDeviceItem.Id;
                                newDeviceSite.SiteID = searchedSiteItem.ID;
                                newDeviceSite.AddedAuto = true;
                                newDeviceSite.LoginCount = 0;

                                db.Tbl_Loginto_DeviceSites.InsertOnSubmit(newDeviceSite);
                                db.SubmitChanges();

                                //call google
                                return CallGoogle(searchedDeviceItem.PushNotificaionToken, prepareMessage(searchedSiteItem, newDeviceSite), newDeviceSite, mcode);
                            }
                            else
                            {
                                ServerResponse response = new ServerResponse();
                                response.tubelessException.code = -1107;
                                response.tubelessException.message = "user not register login to this site ";
                                return new JavaScriptSerializer().Serialize(response);
                            }
                        }
                    }
                    else
                    {
                        //call google
                        return CallGoogle(searchedDeviceItem.PushNotificaionToken, prepareMessage(searchedSiteItem, searchedDeviceSite), searchedDeviceSite, mcode);
                    }
                }
                else
                {
                    ServerResponse response = new ServerResponse();
                    response.tubelessException.code = -1106;
                    response.tubelessException.message = "android id not found for login";
                    return new JavaScriptSerializer().Serialize(response);
                }
            }
            else
            {
                ServerResponse response = new ServerResponse();
                response.tubelessException.code = -1105;
                response.tubelessException.message = "site address not found for login";
                return new JavaScriptSerializer().Serialize(response);
            }


        }

         
        private string prepareMessage(Tbl_Loginto_Site site , Tbl_Loginto_DeviceSite deviceSite)
        {
            LogintoNotification newlogintoMessage = new LogintoNotification();
            AesBase64Wrapper ssss = new AesBase64Wrapper();
            newlogintoMessage.message = ssss.Encrypt("شما در حال ورود به \"" + site.Title + "\" هستید","123");
            newlogintoMessage.title = ssss.Encrypt("ورود به سایت","123");
            newlogintoMessage.Sid = ssss.Encrypt(site.ID.ToString() , "123");

            if (deviceSite.LoginCount == 0)
            {
                newlogintoMessage.adminMessage = ssss.Encrypt("به لاگینتو خوش آمدید","123");
            }
            else
            {
                newlogintoMessage.lastLogin = utility.PersianDateString((DateTime)deviceSite.LastLoginDate);
            }

            return new JavaScriptSerializer().Serialize(newlogintoMessage);
        }



        private string CallGoogle(string Token, string message, Tbl_Loginto_DeviceSite searchedDeviceSite, String androidid)
        {
            DateTime start = DateTime.Now;

            ManualResetEvent syncEvent = new ManualResetEvent(false);
            Notification notification = new Notification();
            notification.data = new Data();
            notification.data.message = message;
            notification.to = Token;
            string DATA = new JavaScriptSerializer().Serialize(notification); ;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            //request.ContentType = "text/xml;charset=utf-8";
            request.Headers["Authorization"] = "key=AAAA_9qL6tc:APA91bE9j-8XT1N7jHmGxfLGISuocvpvgTpUo-ZAcrBjERBY2-RuBje82mjRk6kNsK2FBpBUnboN9Kb29iPfjfWq5yQL14oMLzdvh-dFJnIte5gUodE6D4hUqkOSjaTMYrUEqWqOGnrM";

            request.ContentLength = DATA.Length;
            //request.KeepAlive = false;
            //request.Timeout = 60 * 1000;
            //request.ProtocolVersion = HttpVersion.Version10;
            //request.AllowWriteStreamBuffering = false;

            try
            {
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }
            }
            catch (Exception ex)
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1109;
                responsex.tubelessException.message = "error in conection : " + ex.Message;
                return new JavaScriptSerializer().Serialize(responsex);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);

                    //The UI thread is blocked waiting for this to return
                    //await DoWorkAsync();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    NotificationResponse dc = json_serializer.Deserialize<NotificationResponse>(response);

                    if (dc.success >= 1)
                    {
                        //add to list
                        loginRequest = new LoginRequest();
                        loginRequest.id = androidid;
                        loginRequest.wait = true;
                        loginRequestList.Add(loginRequest);


                        //find item in list
                        LoginRequest requester = loginRequestList.Find(x => x.id == loginRequest.id);
                        while (DateTime.Now.Subtract(start).Seconds < 58)
                        {
                            if (requester.wait)
                            {
                                System.Threading.Thread.Sleep(1500);
                            }
                            else
                            {
                                //update count and date
                                if (searchedDeviceSite.LoginCount == 0)
                                {
                                    searchedDeviceSite.LoginCount = 1;
                                }
                                else
                                {
                                    searchedDeviceSite.LoginCount = searchedDeviceSite.LoginCount + 1;
                                }
                                searchedDeviceSite.LastLoginDate = DateTime.Now;
                                db.SubmitChanges();

                                loginRequestList.Remove(requester);

                                ServerResponse responsex3 = new ServerResponse();
                                responsex3.tubelessException.code = 1103;
                                responsex3.tubelessException.message = "login ok";
                                return new JavaScriptSerializer().Serialize(responsex3);
                            }
                        }

                        loginRequestList.Remove(requester);
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1111;
                        responsex2.tubelessException.message = "user not accept in valid time";
                        return new JavaScriptSerializer().Serialize(responsex2);


                        //return "Ok(data1)";
                    }
                    else if (dc.failure >= 1)
                    {
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1110;
                        responsex2.tubelessException.message = "user application not register";
                        return new JavaScriptSerializer().Serialize(responsex2);
                    }
                    else
                    {
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1108;
                        responsex2.tubelessException.message = "can not send notification";
                        return new JavaScriptSerializer().Serialize(responsex2);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);

                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1109;
                responsex.tubelessException.message = "can not call Gapi : " + e.Message;
                return new JavaScriptSerializer().Serialize(responsex);
            }

            //var data = await db.UserMaster.ToListAsync();
            return "Ok(data2)";


            //System.Threading.Thread.Sleep(15000);
        }

        public string SendMessageDirect(LogintoNotification newlogintoMessage)
        {
            AesBase64Wrapper ssss = new AesBase64Wrapper();


            DateTime start = DateTime.Now;

            ManualResetEvent syncEvent = new ManualResetEvent(false);
            Notification notification = new Notification();
            notification.data = new Data();

            LogintoNotification newlogintoMessageOutput = new LogintoNotification();
            newlogintoMessageOutput.message = ssss.Encrypt(newlogintoMessage.message, "1234");
            newlogintoMessageOutput.title = ssss.Encrypt(newlogintoMessage.title , "1234");
            newlogintoMessageOutput.Sid = ssss.Encrypt(newlogintoMessage.Sid.ToString(), "1234");
            newlogintoMessageOutput.type = 102;
 
            notification.data.message = new JavaScriptSerializer().Serialize(newlogintoMessageOutput);
            notification.to = newlogintoMessage.token;
            string DATA = new JavaScriptSerializer().Serialize(notification);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            //request.ContentType = "text/xml;charset=utf-8";
            request.Headers["Authorization"] = "key=AAAA_9qL6tc:APA91bE9j-8XT1N7jHmGxfLGISuocvpvgTpUo-ZAcrBjERBY2-RuBje82mjRk6kNsK2FBpBUnboN9Kb29iPfjfWq5yQL14oMLzdvh-dFJnIte5gUodE6D4hUqkOSjaTMYrUEqWqOGnrM";

            request.ContentLength = DATA.Length;
            //request.KeepAlive = false;
            //request.Timeout = 60 * 1000;
            //request.ProtocolVersion = HttpVersion.Version10;
            //request.AllowWriteStreamBuffering = false;

            try
            {
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }
            }
            catch (Exception ex)
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1109;
                responsex.tubelessException.message = "error in conection : " + ex.Message;
                return new JavaScriptSerializer().Serialize(responsex);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);

                    //The UI thread is blocked waiting for this to return
                    //await DoWorkAsync();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    NotificationResponse dc = json_serializer.Deserialize<NotificationResponse>(response);

                    if (dc.success >= 1)
                    {
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1111;
                        responsex2.tubelessException.message = "your Message Success recived";
                        return new JavaScriptSerializer().Serialize(responsex2);
                    }
                    else if (dc.failure >= 1)
                    {
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1117;
                        responsex2.tubelessException.message = "your Message failed";
                        return new JavaScriptSerializer().Serialize(responsex2);
                    }
                    else
                    {
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1118;
                        responsex2.tubelessException.message = "can not send notification";
                        return new JavaScriptSerializer().Serialize(responsex2);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);

                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1109;
                responsex.tubelessException.message = "can not call Gapi : " + e.Message;
                return new JavaScriptSerializer().Serialize(responsex);
            }

            //var data = await db.UserMaster.ToListAsync();
            return "Ok(data2)";


            //System.Threading.Thread.Sleep(15000);
        }
        public string SendMessage(LogintoNotification newlogintoMessage)
        {
            AesBase64Wrapper ssss = new AesBase64Wrapper();


            DateTime start = DateTime.Now;

            ManualResetEvent syncEvent = new ManualResetEvent(false);
            Notification notification = new Notification();
            notification.data = new Data();

            LogintoNotification newlogintoMessageOutput = new LogintoNotification();
            newlogintoMessageOutput.message = ssss.Encrypt(newlogintoMessage.message, "1234");
            newlogintoMessageOutput.title = ssss.Encrypt(newlogintoMessage.title , "1234");
            newlogintoMessageOutput.Sid = ssss.Encrypt(newlogintoMessage.Sid.ToString(), "1234");
            newlogintoMessageOutput.type = 101;

            Tbl_Device device = (from p in db.Tbl_Devices
                                 where p.DeviceID.Equals(newlogintoMessage.token)
                                 select p).FirstOrDefault();


            notification.data.message = new JavaScriptSerializer().Serialize(newlogintoMessageOutput);
            notification.to = device.PushNotificaionToken;
            string DATA = new JavaScriptSerializer().Serialize(notification);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            //request.ContentType = "text/xml;charset=utf-8";
            request.Headers["Authorization"] = "key=AAAA_9qL6tc:APA91bE9j-8XT1N7jHmGxfLGISuocvpvgTpUo-ZAcrBjERBY2-RuBje82mjRk6kNsK2FBpBUnboN9Kb29iPfjfWq5yQL14oMLzdvh-dFJnIte5gUodE6D4hUqkOSjaTMYrUEqWqOGnrM";

            request.ContentLength = DATA.Length;
            //request.KeepAlive = false;
            //request.Timeout = 60 * 1000;
            //request.ProtocolVersion = HttpVersion.Version10;
            //request.AllowWriteStreamBuffering = false;

            try
            {
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }
            }
            catch (Exception ex)
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1109;
                responsex.tubelessException.message = "error in conection : " + ex.Message;
                return new JavaScriptSerializer().Serialize(responsex);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);

                    //The UI thread is blocked waiting for this to return
                    //await DoWorkAsync();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    NotificationResponse dc = json_serializer.Deserialize<NotificationResponse>(response);

                    if (dc.success >= 1)
                    {
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1111;
                        responsex2.tubelessException.message = "your Message Success recived";
                        return new JavaScriptSerializer().Serialize(responsex2);
                    }
                    else if (dc.failure >= 1)
                    {
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1117;
                        responsex2.tubelessException.message = "your Message failed";
                        return new JavaScriptSerializer().Serialize(responsex2);
                    }
                    else
                    {
                        ServerResponse responsex2 = new ServerResponse();
                        responsex2.tubelessException.code = -1118;
                        responsex2.tubelessException.message = "can not send notification";
                        return new JavaScriptSerializer().Serialize(responsex2);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);

                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1109;
                responsex.tubelessException.message = "can not call Gapi : " + e.Message;
                return new JavaScriptSerializer().Serialize(responsex);
            }

            //var data = await db.UserMaster.ToListAsync();
            return "Ok(data2)";


            //System.Threading.Thread.Sleep(15000);
        }

        public static bool wait = true;
        private static List<LoginRequest> loginRequestList = new List<LoginRequest>();

        public string veryfi(String Sid, String androidid)
        {
            try
            {
                //string dddddddd = "sPkjDztJav1/zrTsevCkiuchwwVZZeNHBcgPiVVI1FlYxuntnatu6YSuffxy30FJ2NnC0vIEFIgvfEwsUhlS9eTOMQ9GBHC/LVNBEqjpt1g3bFcy53ihcpKno8HqaGjS";
                //string ddddddd = Encrypt.DecryptString(dddddddd, "123");

                String Sid1 = Sid;
                String androidid1 = androidid;

                loginRequestList.Find(x => x.id == androidid).wait = false;
                //return "ok";
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = 1104;
                responsex.tubelessException.message = "login ok response mobile";
                return new JavaScriptSerializer().Serialize(responsex);
            }catch(Exception e)
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1113;
                responsex.tubelessException.message = "-1113 = error login: " +e.Message;
                return new JavaScriptSerializer().Serialize(responsex);
            }
        }

 
        public string updateSettings(Setting setting)
        {
            Tbl_Device device = (from p in db.Tbl_Devices
                                          where p.DeviceID.Equals(setting.AndroidId)
                                          select p).FirstOrDefault();

            Tbl_Loginto_DeviceSetting settings = (from p in db.Tbl_Loginto_DeviceSettings
                                                 where p.DeviceID.Equals(device.Id)
                                                 select p).FirstOrDefault();
            if (settings != null)
            {
                settings.ReciveNotifs = setting.ReciveNotifs;
                settings.AutoAdd = setting.AutoAdd;


                db.SubmitChanges();

                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = 1105;
                responsex.tubelessException.message = "setting update";
                return new JavaScriptSerializer().Serialize(responsex);
            }
            else
            {
                if (insertLogintoSetting(device.Id) >= 1)
                {
                    ServerResponse responsex = new ServerResponse();
                    responsex.tubelessException.code = 1109;
                    responsex.tubelessException.message = "setting created";
                    return new JavaScriptSerializer().Serialize(responsex);
                }
                else
                {
                    ServerResponse responsex = new ServerResponse();
                    responsex.tubelessException.code = -1114;
                    responsex.tubelessException.message = "setting not update";
                    return new JavaScriptSerializer().Serialize(responsex);
                }
            }
        }

        public string removeMyLogin(String AndroidId, String SiteId)
        {
            try
            {
                var searchedDeviceItem = (from p in db.Tbl_Devices
                                          where p.DeviceID.Equals(AndroidId)
                                          select p).FirstOrDefault();
                var searchedSiteItem = (from p in db.Tbl_Loginto_DeviceSites
                                        where p.DeviceID.Equals(searchedDeviceItem.Id) && p.SiteID == Guid.Parse(SiteId)
                                        select p).FirstOrDefault();

                db.Tbl_Loginto_DeviceSites.DeleteOnSubmit(searchedSiteItem);
                db.SubmitChanges();


                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = 1110;
                responsex.tubelessException.message = "remove success";
                return new JavaScriptSerializer().Serialize(responsex);
            }
            catch (Exception ex)
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1119;
                responsex.tubelessException.message = "can not remove my logedin site";
                return new JavaScriptSerializer().Serialize(responsex);
            }
        }

        public string siteProperies(String AndroidId, String SiteId)
        {

            var searchedSiteItem = (from p in db.View_loginto_users_site_lists
                                    where p.AndroidID.Equals(AndroidId) && p.SiteCode == Guid.Parse(SiteId)
                                    select p).FirstOrDefault();

            if (searchedSiteItem != null)
            {
                SiteListResponse listOutput = new SiteListResponse();

                var item = (from p in db.Tbl_Loginto_Sites
                            where p.ID.Equals(searchedSiteItem.SiteCode)
                            select p).FirstOrDefault();
                Site site = new Site()
                {
                    id = item.ID,
                    Title = item.Title,
                    Address = item.Address,
                    Description = item.Description,
                    JoinDate = item.JoinDate,
                    AdminName = item.AdminName,
                    AdminMobile = item.AdminMobile,
                    CategoryID = item.CategoryID,
                    IsActive = item.IsActive,
                };
                listOutput.site = site;

                listOutput.tubelessException.code = 1107;
                listOutput.tubelessException.message = "site ok";
                return new JavaScriptSerializer().Serialize(listOutput);
            }
            else
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1116;
                responsex.tubelessException.message = "site notfound";
                return new JavaScriptSerializer().Serialize(responsex);
            }
        }
        public string logedInSites(String AndroidId)
        {
            var searchedSiteItem = (from p in db.View_loginto_users_site_lists
                                    where p.AndroidID.Equals(AndroidId)
                                    select p).AsEnumerable() ;

            if (searchedSiteItem != null)
            {
                SiteListResponse listOutput = new SiteListResponse();
                foreach (View_loginto_users_site_list item in searchedSiteItem)
                {
                    Site site = new Site()
                    {
                        id = item.SiteCode,
                        Title = item.Title,
                        AddedAuto = item.AddedAuto,
                        IsActive = item.IsActive,
                        LastLoginDate = item.LastLoginDate,
                        LoginCount = item.LoginCount,
                    };
                    listOutput.siteList.Add(site);
                }
                listOutput.site = null;
                listOutput.tubelessException.code = 1106;
                listOutput.tubelessException.message = "list ok";
                return new JavaScriptSerializer().Serialize(listOutput);
            }
            else
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1115;
                responsex.tubelessException.message = "list Empty";
                return new JavaScriptSerializer().Serialize(responsex);
            }
        }

        public string registerSite(Site site)
        {

            var searchedSite = (from p in db.Tbl_Loginto_Sites
                                where p.Address.Equals(site.Address)
                                select p).FirstOrDefault();
            if (searchedSite == null)
            {
                Tbl_Loginto_Site newDeviceSite = new Tbl_Loginto_Site();
                newDeviceSite.ID = Guid.NewGuid();
                newDeviceSite.Title = site.Title;
                newDeviceSite.Address = site.Address.ToLower();
                newDeviceSite.Description = site.Description;
                newDeviceSite.JoinDate = DateTime.Now;
                newDeviceSite.AdminName = site.AdminName;
                newDeviceSite.AdminMobile = site.AdminMobile;
                newDeviceSite.CategoryID = 10017;
                newDeviceSite.IsActive = true;

                db.Tbl_Loginto_Sites.InsertOnSubmit(newDeviceSite);
                db.SubmitChanges();

                SiteResponse responsex = new SiteResponse();
                responsex.tubelessException.code = 1107;
                responsex.siteCode = newDeviceSite.ID .ToString();
                responsex.tubelessException.message = "OK";
                return new JavaScriptSerializer().Serialize(responsex);
            }
            else
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1104;
                responsex.tubelessException.message = "site not register";
                return new JavaScriptSerializer().Serialize(responsex);
            }
        }
        public string registerLogin(String AndroidId , String siteName)
        {
            try
            {
                var searchedSiteItem = (from p in db.Tbl_Loginto_Sites
                                    where p.Address.Equals(siteName.ToLower())
                                    select p).FirstOrDefault();
                if (searchedSiteItem != null)
                {
                    var searchedDeviceItem = (from p in db.Tbl_Devices
                                        where p.DeviceID.Equals(AndroidId)
                                        select p).FirstOrDefault();
                    if (searchedDeviceItem != null)
                    {
                        var searchedDeviceSite = (from p in db.Tbl_Loginto_DeviceSites
                                                  where p.DeviceID.Equals(searchedDeviceItem.Id) && p.SiteID.Equals(searchedSiteItem.ID)
                                                  select p).FirstOrDefault();
                        if (searchedDeviceSite == null)
                        {
                            
                            Tbl_Loginto_DeviceSite newDeviceSite = new Tbl_Loginto_DeviceSite();
                            newDeviceSite.DeviceID = searchedDeviceItem.Id;
                            newDeviceSite.SiteID = searchedSiteItem.ID;
                            newDeviceSite.AddedAuto = false;
                            newDeviceSite.LoginCount = 0;

                            db.Tbl_Loginto_DeviceSites.InsertOnSubmit(newDeviceSite);
                            db.SubmitChanges();

                            ServerResponse responsex = new ServerResponse();
                            responsex.tubelessException.code = 1100;
                            responsex.tubelessException.message = "add new DeviceSite";
                            return new JavaScriptSerializer().Serialize(responsex);
                        }
                        else
                        {
                            ServerResponse responsex = new ServerResponse();
                            responsex.tubelessException.code = 1101;
                            responsex.tubelessException.message = "added DeviceSite";
                            return new JavaScriptSerializer().Serialize(responsex);
                        }
                    }
                    else
                    {
                        ServerResponse response = new ServerResponse();
                        response.tubelessException.code = -1103;
                        response.tubelessException.message = "android id not found";
                        return new JavaScriptSerializer().Serialize(response);
                    }
                }
                else
                {
                    ServerResponse response = new ServerResponse();
                    response.tubelessException.code = -1101;
                    response.tubelessException.message = "site address not found ";
                    return new JavaScriptSerializer().Serialize(response);
                }
            }
            catch (Exception ex)
            {
                string sssss = ex.Message;
                ServerResponse response = new ServerResponse();
                response.tubelessException.code = -1102;
                response.tubelessException.message = "error in add new DeviceSite 2";
                return new JavaScriptSerializer().Serialize(response);
            }
        }


        private int? insertLogintoSetting(int deviceID)
        {
            try
            {
                Tbl_Loginto_DeviceSetting tbl_Setting = new Tbl_Loginto_DeviceSetting();
                tbl_Setting.DeviceID = deviceID;
                tbl_Setting.ReciveNotifs = true;
                tbl_Setting.AutoAdd = true;

                db.Tbl_Loginto_DeviceSettings.InsertOnSubmit(tbl_Setting);
                db.SubmitChanges();

                return tbl_Setting.DeviceID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

    }

    class LoginRequest
    {
        public string id;
        public bool wait;
    }
    public class Site
    {
        public Guid id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string AdminName { get; set; }
        public string AdminMobile { get; set; }
        public long? CategoryID { get; set; }
        public bool? AddedAuto { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public long? LoginCount { get; set; }
        public long ID { get; set; }
        public DateTime? JoinDate { get; set; }
        public bool? IsActive { get; set; }
    }
    public class Setting
    {
        public String AndroidId { get; set; }

        public bool AutoAdd { get; set; }
        public bool ReciveNotifs { get; set; }
    }


    public class Notification
    {
        public Data data { get; set; }
        public string to { get; set; }

    }
    public class Data
    {
        public string message { get; set; }
    }

    public class NotificationResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
    }



}
