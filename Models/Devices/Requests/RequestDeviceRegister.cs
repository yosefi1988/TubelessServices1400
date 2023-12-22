using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Devices.Requests
{
    public class RequestDeviceRegister
    {
        public string DeviceId { get; set; }
        public int AndroidAPI { get; set; }
        public string AndroidVersion { get; set; }
        public int ApplicationId { get; set; }
        public string Board { get; set; }
        public string Brand { get; set; }
        public string BuildId { get; set; }
        public string Display { get; set; }
        public string IP { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public string Store { get; set; }
        public string token { get; set; }
        public string IDUser { get; set; }
    }
}