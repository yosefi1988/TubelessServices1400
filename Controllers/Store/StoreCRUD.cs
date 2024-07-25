using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models;
using TubelessServices.Models.Devices.Requests;

namespace TubelessServices.Controllers.Store
{
    public class StoreCRUD
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        internal List<Tbl_ApplicationStore> findStoreForApplication(RequestDeviceRegister request)
        {
            return findStoreForApplication(request.Store,request.ApplicationId);
        }

        internal Tbl_ApplicationStorePermission findStoreForApplicationPermissions(RequestDeviceRegister request, int userTypeCode)
        {
            return findStoreForApplicationPermissionsx(request.Store, request.ApplicationId, userTypeCode);
        }


        internal List<Tbl_ApplicationStore> findStoreForApplication(string store, int applicationId)
        {
            List<Tbl_ApplicationStore> storelist = (from x in db.Tbl_ApplicationStores
                                                    where x.StoreName.Equals(store.ToLower()) && x.IDApplication == applicationId
                                                    select x).ToList();
            return storelist;
        }



        internal Tbl_ApplicationStorePermission findStoreForApplicationPermissionsx(string store, int applicationId, int userTypeCode)
        {
            var id = 1;
            var query = from applicationStore in db.Tbl_ApplicationStores
                        join applicationStorePermission in db.Tbl_ApplicationStorePermissions
                        on applicationStore.ID equals applicationStorePermission.ApplicationStoreId
                        where applicationStore.StoreName.Equals(store) && applicationStore.IDApplication == applicationId && applicationStorePermission.UserTypeCode == userTypeCode
                        select new { ApplicationStorePermission = applicationStorePermission };
            var x1 = query.ToList();
            if (x1.Count == 1)
                return x1.First().ApplicationStorePermission;
            else
                return null;
        }

         
    }
}