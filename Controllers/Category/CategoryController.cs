using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TubelessServices.Models;
using TubelessServices.Models.Category;
using TubelessServices.Models.Category.Request;
using TubelessServices.Models.Category.Response;
using TubelessServices.Models.Response;

namespace TubelessServices.Controllers.Category
{
    [RoutePrefix("api/Category")]
    public class CategoryController : ApiController
    {
        CategoryCRUD categoryCRUD = new CategoryCRUD();
        CategoryResponse response = new CategoryResponse();

        [HttpPost]
        [Route("GetCategory")]
        public string Category(CategoryRequest request)
        {
            List<CategoryItem> catlist = new List<CategoryItem>();

            IEnumerable<Tbl_DetailsLookup> catList = categoryCRUD.getCategory(request);
            foreach (Tbl_DetailsLookup item in catList)
            {
                CategoryItem catItem = new CategoryItem();
                catItem.ID = item.Id;
                catItem.Title = item.Name;
                catItem.Value = item.Value;
                catlist.Add(catItem);
            }

            response.catlist = catlist;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;

            //ServerResponse responsex = new ServerResponse();
            //responsex.tubelessException.code = -5;
            //responsex.tubelessException.message = "user not found";
            //string sssssss = new JavaScriptSerializer().Serialize(responsex);
            //return sssssss;

            return "";
            
        }

        [HttpPost]
        [Route("GetCategoryNew")]
        public string CategoryNew(CategoryRequest request)
        {
            List<CategoryItem> catlist = new List<CategoryItem>();

            IEnumerable<Tbl_DetailsLookup> catList;
            if (request.CategoryCode != 0)
            {
                catList = categoryCRUD.getCategory(request);
            }
            else
            {
                if (request.ParentID != 0)
                {
                    catList = categoryCRUD.getCategoryByParentCode(request);

                }
                else if (request.BrothersID != 0)
                {
                    catList = categoryCRUD.getCategoryDetailByBrothersID(request);
                }
                else
                {
                    catList = new List<Tbl_DetailsLookup>();
                }
            }

            foreach (Tbl_DetailsLookup item in catList)
            {
                CategoryItem catItem = new CategoryItem();
                catItem.ID = item.Id;
                catItem.HID = item.IDParent;
                catItem.Title = item.Name;
                catItem.Statment = item.Value;
                catItem.Value = item.Value;
                catItem.Image = item.ImageUrl;
                catItem.Icon = item.icon;

                if (item.Selectable != null && item.Selectable == true)
                {
                    catItem.Selectable = true;
                    catItem.SelectableS = "true";
                }
                else
                {
                    if (item.IDCategoryLookUp == request.SelectableCategory)
                    {
                        catItem.SelectableS = "true";
                        catItem.Selectable = true;
                    }
                    else
                    {
                        catItem.SelectableS = "false";
                        catItem.Selectable = false;
                    }
                }
                catlist.Add(catItem);
            }

            response.catlist = catlist;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;

            //ServerResponse responsex = new ServerResponse();
            //responsex.tubelessException.code = -5;
            //responsex.tubelessException.message = "user not found";
            //string sssssss = new JavaScriptSerializer().Serialize(responsex);
            //return sssssss;

            return "";

        }

        [HttpPost]
        [Route("GetCategoryNew2")]
        public string CategoryNew2(CategoryRequest request)
        {
            List<CategoryItem> catlist = new List<CategoryItem>();

            IEnumerable<Tbl_DetailsLookup> catList;
            if (request.CategoryCode != 0)
            {
                catList = categoryCRUD.getCategory2(request);
            }
            else
            {
                if (request.ParentID != 0)
                {
                    catList = categoryCRUD.getCategoryByParentCode(request);

                }
                else if (request.BrothersID != 0)
                {
                    catList = categoryCRUD.getCategoryDetailByBrothersID(request);
                }
                else
                {
                    catList = new List<Tbl_DetailsLookup>();
                }
            }

            foreach (Tbl_DetailsLookup item in catList)
            {
                CategoryItem catItem = new CategoryItem();
                catItem.ID = item.Id;
                catItem.HID = item.IDParent;
                catItem.Title = item.Name;
                catItem.Statment = item.Value;
                catItem.Value = item.Value;
                catItem.Image = item.ImageUrl;
                catItem.Icon = item.icon;

                if (item.Selectable != null && item.Selectable == true)
                {
                    catItem.Selectable = true;
                    catItem.SelectableS = "true";
                }
                else
                {
                    if (item.IDCategoryLookUp == request.SelectableCategory)
                    {
                        catItem.SelectableS = "true";
                        catItem.Selectable = true;
                    }
                    else
                    {
                        catItem.SelectableS = "false";
                        catItem.Selectable = false;
                    }
                }
                catlist.Add(catItem);
            }

            response.catlist = catlist;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;

            //ServerResponse responsex = new ServerResponse();
            //responsex.tubelessException.code = -5;
            //responsex.tubelessException.message = "user not found";
            //string sssssss = new JavaScriptSerializer().Serialize(responsex);
            //return sssssss;

            return "";

        }



        [HttpPost]
        [Route("CategoryLevel1")]
        public string GetCategoryLevel1ForSite(CategoryRequest2 request)
        {
            List<Viw_Site_Cat_level_1> catlist = new List<Viw_Site_Cat_level_1>();

            IEnumerable<Viw_Site_Cat_level_1> catList = null;
            catList = categoryCRUD.getCat_level_1_for_site(request);
            

            foreach (Viw_Site_Cat_level_1 item in catList)
            {
                Viw_Site_Cat_level_1 catItem = new Viw_Site_Cat_level_1();
                catItem.Id = item.Id;
                catItem.Name= item.Name;
                catItem.ImageUrl= item.ImageUrl;
                catItem.icon= item.icon;
                 
                catlist.Add(catItem);
            }

            response.cat1list = catlist;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;

            //ServerResponse responsex = new ServerResponse();
            //responsex.tubelessException.code = -5;
            //responsex.tubelessException.message = "user not found";
            //string sssssss = new JavaScriptSerializer().Serialize(responsex);
            //return sssssss;

            return "";

        }


        [HttpPost]
        [Route("CategoryLevel2")]
        public string GetCategoryLevel2ForSite(CategoryRequest2 request)
        { 
            List<Viw_Site_Cat_level_2> catlist = new List<Viw_Site_Cat_level_2>();

            IEnumerable<Viw_Site_Cat_level_2> catList = null; 
            catList = categoryCRUD.getCat_level_2_for_site(request); 

            foreach (Viw_Site_Cat_level_2 item in catList)
            {
                Viw_Site_Cat_level_2 catItem = new Viw_Site_Cat_level_2();
                catItem.Id = item.Id;
                catItem.Name = item.Name;
                catItem.ImageUrl = item.ImageUrl;
                catItem.icon = item.icon;

                catlist.Add(catItem);
            }

            response.cat2list = catlist;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;

            //ServerResponse responsex = new ServerResponse();
            //responsex.tubelessException.code = -5;
            //responsex.tubelessException.message = "user not found";
            //string sssssss = new JavaScriptSerializer().Serialize(responsex);
            //return sssssss;

            return "";

        }


        //[HttpPost]
        //[Route("ApplicationListByCompanyCode")]
        //public string ApplicationListForSiteByCompanyCode(reqAppList requestPostList)
        //{
        //    IEnumerable<Viw_Site_AppList> xxxxxxxxx = AppsCRUD.getApplicationListByCompanyCode(requestPostList.id, requestPostList.pageIndex, requestPostList.pageSize);
        //    response.postList = xxxxxxxxx.ToList();
        //    string ssssssss = new JavaScriptSerializer().Serialize(response);
        //    return ssssssss;
        //}
    }
}
