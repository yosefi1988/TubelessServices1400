using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models;
using TubelessServices.Models.Category.Request;

namespace TubelessServices.Controllers.Category
{
    public class CategoryCRUD
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();

 

        internal IEnumerable<Tbl_DetailsLookup> getCategory(CategoryRequest request)
        {
            IEnumerable<Tbl_DetailsLookup> selectedCats = (from record in db.Tbl_DetailsLookups
                                                           where record.IDCategoryLookUp == request.CategoryCode
                                                           select record);

            return checkParentCode(selectedCats, request);
            return selectedCats;
        }

        internal IEnumerable<Tbl_DetailsLookup> checkParentCode(IEnumerable<Tbl_DetailsLookup> catList, CategoryRequest request)
        {
            //if (request.ParentID != 0)
            //{
            //IEnumerable<Models.Tbl_DetailsLookup> list = catList.Where(x => x.IDParent == request.ParentID);
            //return list;
            //}
            //else
            //{
            //IEnumerable<Models.Tbl_DetailsLookup> list = catList.Where(x => x.IDParent == null);
            //return list;
            //    return catList;
            //}

            IEnumerable<Models.Tbl_DetailsLookup> list = catList.ToList();//.Where(x => x.IDParent == request.ParentID);
            return list;
        }



        internal IEnumerable<Tbl_DetailsLookup> getCategory2(CategoryRequest request)
        {
            IEnumerable<Tbl_DetailsLookup> selectedCats = (from record in db.Tbl_DetailsLookups
                                                           where record.IDCategoryLookUp == request.CategoryCode
                                                           select record);

            return checkParentCode2(selectedCats, request);
            return selectedCats;
        }

        internal IEnumerable<Tbl_DetailsLookup> checkParentCode2(IEnumerable<Tbl_DetailsLookup> catList, CategoryRequest request)
        {
            if (request.ParentID != 0)
            {
                IEnumerable<Models.Tbl_DetailsLookup> list = catList.Where(x => x.IDParent == request.ParentID);
                return list;
            }
            else
            {
                IEnumerable<Models.Tbl_DetailsLookup> list = catList.Where(x => x.IDParent == null);
                return list;
                return catList;
            }
        }


        internal IEnumerable<Tbl_DetailsLookup> getCategoryDetailByBrothersID(CategoryRequest request)
        { 
            IEnumerable<Tbl_DetailsLookup> selectedCats = (from record in db.Tbl_DetailsLookups
                                                           where record.Id == request.BrothersID
                                                           select record);
            int? a = selectedCats.First().IDParent;
            IEnumerable<Tbl_DetailsLookup> selectedCats2 = (from record in db.Tbl_DetailsLookups
                                                           where record.IDParent == a
                                                           select record);

            return selectedCats2;
        }

        internal IEnumerable<Tbl_DetailsLookup> getCategoryByParentCode(CategoryRequest request)
        { 

            IEnumerable<Tbl_DetailsLookup> selectedCats = (from record in db.Tbl_DetailsLookups
                                                           where record.Id == request.ParentID
                                                           select record);
            return selectedCats;
        }
        internal IEnumerable<Viw_Site_Cat_level_1> getCat_level_1_for_site(CategoryRequest2 request)
        {

            IEnumerable<Viw_Site_Cat_level_1> selectedCats = (from record in db.Viw_Site_Cat_level_1s
                                                              select record).OrderByDescending(date => date.Id).ToList();
  

            IEnumerable<Models.Viw_Site_Cat_level_1> appList2 = selectedCats.Skip(request.pageIndex * request.pageSize);
            IEnumerable<Models.Viw_Site_Cat_level_1> appList3 = appList2.Take(request.pageSize);

            return appList3;

        }

        internal IEnumerable<Viw_Site_Cat_level_2> getCat_level_2_for_site(CategoryRequest2 request)
        {

            IEnumerable<Viw_Site_Cat_level_2> selectedCats = (from record in db.Viw_Site_Cat_level_2s
                                                              where record.IDParent == request.id
                                                              select record).OrderByDescending(date => date.Id).ToList();


            IEnumerable<Models.Viw_Site_Cat_level_2> appList2 = selectedCats.Skip(request.pageIndex * request.pageSize);
            IEnumerable<Models.Viw_Site_Cat_level_2> appList3 = appList2.Take(request.pageSize);

            return appList3;

        }
    }
}