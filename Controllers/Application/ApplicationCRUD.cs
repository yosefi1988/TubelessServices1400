using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models;

namespace TubelessServices.Controllers.Application
{
    public class ApplicationCRUD
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();

        internal IEnumerable<Viw_AppStore> getApplicationList(int typeID, int companyID)
        {
            IEnumerable<Viw_AppStore> appList1 = (from x in db.Viw_AppStores
                                                   where
                                                       x.typeID == typeID &&
                                                       x.companyID == companyID
                                                  select x).OrderByDescending(date => date.Views).ToList();


            return appList1;

        }

        internal IEnumerable<Viw_Site_AppList> getApplicationListByTypeCode(int typeCode,int pageIndex , int pageSize)
        {
            IEnumerable<Viw_Site_AppList> appList1 = (from x in db.Viw_Site_AppLists
                                                      where
                                                          x.AppTypeCode == typeCode
                                                      select x).OrderByDescending(date => date.Views).ToList();


            IEnumerable<Models.Viw_Site_AppList> appList2 = appList1.Skip(pageIndex * pageSize);
            IEnumerable<Models.Viw_Site_AppList> appList3 = appList2.Take(pageSize);
             
            return appList3;

        }
        internal IEnumerable<Viw_Site_AppList> getApplicationListByCompanyCode(int companyCode, int pageIndex, int pageSize)
        {
            IEnumerable<Viw_Site_AppList> appList1 = (from x in db.Viw_Site_AppLists
                                                      where
                                                          x.CompanyCode == companyCode
                                                      select x).OrderByDescending(date => date.Views).ToList();


            IEnumerable<Models.Viw_Site_AppList> appList2 = appList1.Skip(pageIndex * pageSize);
            IEnumerable<Models.Viw_Site_AppList> appList3 = appList2.Take(pageSize);

            return appList3;

        }
        internal IEnumerable<Viw_Site_AppList> getApplicationByStore(String storeName, int idApplication)
        {
            IEnumerable<Viw_Site_AppList> appList1 = (from x in db.Viw_Site_AppLists
                                                  where
                                                      x.StoreName.Equals(storeName) &&
                                                      x.ApplicationID == idApplication
                                                  select x).OrderByDescending(date => date.Views).ToList();


            return appList1;

        }

    }
}