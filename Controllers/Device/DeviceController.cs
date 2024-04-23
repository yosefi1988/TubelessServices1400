using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TubelessServices.Controllers.Autenticator;
using TubelessServices.Controllers.Post;
using TubelessServices.Controllers.Wallet;
using TubelessServices.Models;
using TubelessServices.Models.Config;
using TubelessServices.Models.Devices.Requests;
using TubelessServices.Models.post.Response;
using TubelessServices.Models.Post.Request;

namespace TubelessServices.Controllers.Device
{
 
    [RoutePrefix("api/Device")]
    public class DeviceController : ApiController
    {
        DeviceCRUD deviceCRUD = new DeviceCRUD();


        UserCRUD userCRUD = new UserCRUD();
        WalletCRUD walletCRUD = new WalletCRUD();
        PostCRUD postCRUD = new PostCRUD();
        ConfigResponse response = new ConfigResponse();


        [HttpPost]
        [Route("RegDevice")]
        public string NewDevice(RequestDeviceRegister deviceRegister)
        {
            Store.StoreCRUD storeCRUD = new Store.StoreCRUD();

            int deviceId = 0;
            List<Tbl_Device> listDevice = deviceCRUD.findDeviceByDeviceID(deviceRegister.DeviceId);
            Tbl_ApplicationStore requestFromStore = new Tbl_ApplicationStore();
            Tbl_ApplicationStorePermission selectedStorePermisions = new Tbl_ApplicationStorePermission();


            if (listDevice.Count == 0)
            {
                deviceId = deviceCRUD.insertDevice(deviceRegister);
            }
            else
            {
                deviceId = listDevice.ToList().First().Id;
                deviceCRUD.updateDevice(deviceRegister, listDevice.First());
            }


            int storeID = 0;
            //int userTypeCode = 0;
            //int userId = 0;

            //device app
            if (deviceId == 0)
            {
                //error
            }
            else
            { 
                requestFromStore = storeCRUD.findStoreForApplication(deviceRegister).FirstOrDefault();

                if (requestFromStore != null)
                {
                    storeID = requestFromStore.ID;
                    //if(deviceRegister.IDUser == null)
                    //{
                    //    userId = (int) listDevice.First().IDUser;
                    //}
                    //else
                    //{
                    //    userId = int.Parse(deviceRegister.IDUser);
                    //}
                    //userTypeCode = userCRUD.findUserByID(userId).First().UserTypeCode;
                    if (listDevice.ToList().Count == 0)
                    {
                        selectedStorePermisions = storeCRUD.findStoreForApplicationPermissions(deviceRegister, 1);      //UserTypeCode = 1  normalUser
                    }
                    else
                    {
                        if(listDevice.ToList().First().Tbl_User == null)
                            selectedStorePermisions = storeCRUD.findStoreForApplicationPermissions(deviceRegister, 1);      //UserTypeCode = 1  normalUser
                        else
                            selectedStorePermisions = storeCRUD.findStoreForApplicationPermissions(deviceRegister, listDevice.ToList().First().Tbl_User.UserTypeCode);
                    }
                }

                //insert or update
                List<Tbl_DeviceApp> listDeviceApp = deviceCRUD.findDeviceApp(deviceId,deviceRegister.ApplicationId);
                if(listDeviceApp.Count == 0)
                {
                    //new Device app
                    deviceCRUD.insertDeviceApp(deviceId,deviceRegister, storeID);
                }
                else
                {
                    //update device app
                    deviceCRUD.updateDeviceApp(deviceId, deviceRegister, storeID);
                }

            }

            //Config config = new Config(stores, deviceId);
            Config config = new Config(requestFromStore, deviceId, selectedStorePermisions);

            config.umic = deviceCRUD.checkMIC(deviceRegister.DeviceId);

            //return Config
            response.config = config;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
 
        }


        [HttpPost]
        [Route("RegDeviceOldAdapter")]
        public string NewDevice2(RequestDeviceRegister deviceRegister)
        {
            Store.StoreCRUD storeCRUD = new Store.StoreCRUD();

            int deviceId = 0;
            List<Tbl_Device> listDevice = deviceCRUD.findDeviceByDeviceID(deviceRegister.DeviceId);
            Tbl_ApplicationStore requestFromStore = new Tbl_ApplicationStore();
            Tbl_ApplicationStorePermission selectedStorePermisions = new Tbl_ApplicationStorePermission();

            if (listDevice.Count == 0)
            {
                deviceId = deviceCRUD.insertDevice(deviceRegister);
            }
            else
            {
                deviceId = listDevice.ToList().First().Id;
                deviceCRUD.updateDevice(deviceRegister, listDevice.First());
            }

            int storeID = 0;
            int userTypeCode = 0;

            //device app
            if (deviceId == 0)
            {
                //error
            }
            else
            {

                requestFromStore = storeCRUD.findStoreForApplication(deviceRegister).FirstOrDefault();

                if (requestFromStore != null)
                {
                    storeID = requestFromStore.ID;
                    userTypeCode = userCRUD.findUserByID(Int16.Parse(deviceRegister.IDUser)).First().UserTypeCode;
                    selectedStorePermisions = storeCRUD.findStoreForApplicationPermissions(deviceRegister, userTypeCode);
                }

                //insert or update
                List<Tbl_DeviceApp> listDeviceApp = deviceCRUD.findDeviceApp(deviceId, deviceRegister.ApplicationId);
                if (listDeviceApp.Count == 0)
                {
                    //new Device app
                    deviceCRUD.insertDeviceApp(deviceId, deviceRegister, storeID);
                    if(deviceRegister.ApplicationId == 1011)
                    {
                        deviceCRUD.insertDeviceSetting(deviceId);
                    }
                }
                else
                {
                    //update device app
                    deviceCRUD.updateDeviceApp(deviceId, deviceRegister, storeID);
                }

            }

            Config config = new Config(requestFromStore, deviceId, selectedStorePermisions);
            config.umic = deviceCRUD.checkMIC(deviceRegister.DeviceId);

            //return Config
            response.config = config;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;

        }

    }
}
