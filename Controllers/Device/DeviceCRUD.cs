using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models;
using TubelessServices.Models.Config;
using TubelessServices.Models.Devices.Requests;

namespace TubelessServices.Controllers.Device
{
    public class DeviceCRUD
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();

        internal int insertDevice(RequestDeviceRegister deviceRegister)
        {
            Tbl_Device device = new Tbl_Device();            
            device.DeviceID = deviceRegister.DeviceId;
            device.Serial = deviceRegister.Serial;
            device.Model = deviceRegister.Model;
            device.BuildId = deviceRegister.BuildId;
            device.AndroidVersion = deviceRegister.AndroidVersion + "";
            device.AndroidAPI = deviceRegister.AndroidAPI;
            device.Manufacturer = deviceRegister.Manufacturer;
            device.Brand = deviceRegister.Brand;
            device.Board = deviceRegister.Board;
            device.Display = deviceRegister.Display;
            device.CreatedOn = DateTime.Now;
            device.ModifiedOn = DateTime.Now;
            device.IP = deviceRegister.IP;
            device.PushNotificaionToken = deviceRegister.token;

            if(deviceRegister.IDUser == null)
                device.IDUser = 10012;
            else
                device.IDUser = int.Parse(deviceRegister.IDUser);

            try
            {
                db.Tbl_Devices.InsertOnSubmit(device);
                db.SubmitChanges();
                return device.Id;
            }
            catch
            {
                return 0;
            }
        }

        internal void updateDevice(RequestDeviceRegister deviceRegister, Tbl_Device device)
        {
            device.BuildId = deviceRegister.BuildId;
            device.AndroidVersion = deviceRegister.AndroidVersion + "";
            device.AndroidAPI = deviceRegister.AndroidAPI;
            
            device.Board = deviceRegister.Board;        
            device.ModifiedOn = DateTime.Now;
            device.IP = deviceRegister.IP;
            device.PushNotificaionToken = deviceRegister.token;
            db.SubmitChanges();
        }

        internal void updateDeviceUserId(Tbl_Device device, int newUserId)
        { 
            device.IDUser = newUserId;
            db.SubmitChanges();
        }
        internal bool insertDeviceApp(int deviceId, RequestDeviceRegister deviceRegister, int storeID)
        {
            Tbl_DeviceApp newDeviceApp = new Tbl_DeviceApp();
            newDeviceApp.CreateOn = DateTime.Now;
            newDeviceApp.IDApplication = deviceRegister.ApplicationId;
            newDeviceApp.IDDevice = deviceId;
            newDeviceApp.IDApplicationStore = storeID;

            try
            {
                db.Tbl_DeviceApps.InsertOnSubmit(newDeviceApp);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal bool insertDeviceSetting(int deviceId)
        {
            Tbl_Loginto_DeviceSetting newDeviceApp = new Tbl_Loginto_DeviceSetting();
            newDeviceApp.DeviceID = deviceId;
            newDeviceApp.AutoAdd = false;
            newDeviceApp.ReciveNotifs = false; 

            try
            {
                db.Tbl_Loginto_DeviceSettings.InsertOnSubmit(newDeviceApp);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal bool insertUserDevice(int userId, int deviceId)
        {
            tbl_UsersDevice newUserDevice = new tbl_UsersDevice();
            newUserDevice.IDDevice = deviceId;
            newUserDevice.IDUser = userId;

            try
            {
                db.tbl_UsersDevices.InsertOnSubmit(newUserDevice);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal List<UMIC> checkMIC(string deviceId)
        { 
            List<Tbl_Device> listDevice = findDeviceByDeviceID(deviceId);
            List<UMIC> resultList = new List<UMIC>();

            if(listDevice.Count == 1)
            {
                Tbl_Device device = listDevice.First();
                List<Viw_userDevice> listresult = (from p in db.Viw_userDevices
                                                where p.IDDevice == device.Id && p.Mobile != null
                                                select p).ToList();

                foreach (Viw_userDevice item in listresult)
                {
                    if (item.PhoneNumberConfirmed == true)
                    {
                        UMIC umic = new UMIC();
                        umic.IsConfirm = item.PhoneNumberConfirmed;
                        umic.usercode = item.UserCode;
                        umic.mobile = item.Mobile;
                        resultList.Add(umic);
                    }
                }
                return resultList;
            }
            return null;
        }

        internal bool updateDeviceApp(int deviceId, RequestDeviceRegister deviceRegister, int storeID)
        {
            List<Tbl_DeviceApp> deviceAppList = findDeviceApp(deviceId, deviceRegister.ApplicationId);
            Tbl_DeviceApp devApp = deviceAppList.First();
            devApp.LastRunApplicationOn = DateTime.Now;
            devApp.IDApplicationStore = storeID;
            try
            {
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

 
        internal List<Tbl_DeviceApp> findDeviceApp(int deviceId, int applicationId)
        {
            return (from x in db.Tbl_DeviceApps
                    where 
                        x.IDDevice.Equals(deviceId)
                        &&
                        x.IDApplication.Equals(applicationId)
                    select x).ToList();
        }

        internal List<Tbl_Device> findDeviceByDeviceID(string deviceID)
        {
            return (from x in db.Tbl_Devices where x.DeviceID.Equals(deviceID) select x).ToList();
        }
 


    }
}