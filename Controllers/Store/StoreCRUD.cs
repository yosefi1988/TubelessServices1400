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

        internal List<Tbl_ApplicationStore> findStoreForApplication(string store, int applicationId)
        {
            List<Tbl_ApplicationStore> storelist = (from x in db.Tbl_ApplicationStores
                                                    where x.StoreName.Equals(store.ToLower()) && x.IDApplication == applicationId
                                                    select x).ToList();
            return storelist;
        }
    }
}