using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Config
{
    public class Config
    {
        public List<Store> StoreList = new List<Store>();
        public int DeviceId = 0;
        public List<UMIC> umic = new List<UMIC>();

        public Config(List<Tbl_ApplicationStore> stores, int deviceId)
        {
            this.DeviceId = deviceId;
            foreach (Tbl_ApplicationStore store in stores)
            {
                Store storeItem = new Store();
                storeItem.Id = store.ID;
                storeItem.IsFree = store.isFree;
                storeItem.Name = store.StoreName;
                StoreList.Add(storeItem);
            }
        }
    }

    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFree { get; set; }
    }
    public class UMIC
    {
        public int? usercode { get; set; }
        public string mobile { get; set; }
        public bool IsConfirm { get; set; }
    }
}