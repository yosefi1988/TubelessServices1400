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

        public Config(Tbl_ApplicationStore store, int deviceId, Tbl_ApplicationStorePermission applicationStorePermission)
        {
            this.DeviceId = deviceId;
            Store storeItem = new Store();
            StorePermissions storePermissions = new StorePermissions();
            if (store != null)
            {
                storeItem.Id = store.ID;
                storeItem.IsFree = store.isFree;
                storeItem.Name = store.StoreName;

                if (applicationStorePermission != null)
                {
                    storePermissions.IsPostFree = applicationStorePermission.IsPostFree;
                    storePermissions.IsViewFree = applicationStorePermission.IsViewFree;
                    storePermissions.SendImageInPost = applicationStorePermission.SendImageInPost;
                    storeItem.Permissions = storePermissions;
                }

                StoreList.Add(storeItem);
            }
        }
    }

    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFree { get; set; }
        public StorePermissions Permissions { get; set; }
}
    public class StorePermissions
    {
        public bool? IsPostFree { get; set; }
        public bool? IsViewFree { get; set; }
        public bool? SendImageInPost { get; set; }
    }
    public class UMIC
    {
        public int? usercode { get; set; }
        public string mobile { get; set; }
        public bool IsConfirm { get; set; }
    }
}