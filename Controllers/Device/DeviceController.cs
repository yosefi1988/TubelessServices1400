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
            List<Tbl_ApplicationStore> stores = new List<Tbl_ApplicationStore>();

            if (listDevice.Count == 0)
            {
                deviceId = deviceCRUD.insertDevice(deviceRegister);
            }
            else
            {
                deviceId = listDevice.ToList().First().Id;
                deviceCRUD.updateDevice(deviceRegister, listDevice.First());
            }


            //device app
            if (deviceId == 0)
            {
                //error
            }
            else
            {
                int storeID = 0;
                stores = storeCRUD.findStoreForApplication(deviceRegister);

                if(stores.Count == 1)
                {
                    storeID = stores.First().ID;
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

            Config config = new Config(stores, deviceId);
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
            List<Tbl_ApplicationStore> stores = new List<Tbl_ApplicationStore>();

            if (listDevice.Count == 0)
            {
                deviceId = deviceCRUD.insertDevice(deviceRegister);
            }
            else
            {
                deviceId = listDevice.ToList().First().Id;
                deviceCRUD.updateDevice(deviceRegister, listDevice.First());
            }


            //device app
            if (deviceId == 0)
            {
                //error
            }
            else
            {
                int storeID = 0;
                stores = storeCRUD.findStoreForApplication(deviceRegister);

                if (stores.Count == 1)
                {
                    storeID = stores.First().ID;
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

            Config config = new Config(stores, deviceId);
            config.umic = deviceCRUD.checkMIC(deviceRegister.DeviceId);

            //return Config
            response.config = config;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;

        }

    }
}
