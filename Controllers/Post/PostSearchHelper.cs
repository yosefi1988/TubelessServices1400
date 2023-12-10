
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models;
using TubelessServices.Models.Post.Request;

namespace TubelessServices.Controllers.Post
{
    public class PostSearchHelper
    {

        internal IEnumerable<Viw_postList> searchString(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.Search != null)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(
                    x =>
                    x.Title.Contains(request.Search) ||
                    x.Text.Contains(request.Search) ||
                   // x.CreatorName.Contains(request.Search) ||
                   // x.CreatorFamily.Contains(request.Search) ||
                    x.StateName.Contains(request.Search) ||
                    x.CityName.Contains(request.Search
             )
             );
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_MyPostListResult> searchMylistString(IEnumerable<Sp_MyPostListResult> postList, reqPostList request)
        {
            if (request.Search != null)
            {
                IEnumerable<Models.Sp_MyPostListResult> list = postList.Where(
                    x =>
                    x.Title.Contains(request.Search) ||
                    x.Text.Contains(request.Search) ||
                    //x.CreatorName.Contains(request.Search) ||
                    //x.CreatorFamily.Contains(request.Search) ||
                    x.StateName.Contains(request.Search) ||
                    x.CityName.Contains(request.Search));
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Viw_postList> checkIsActive(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.IsActive == true)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.IsActive == true);
                return list;
            }
            else
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.IsActive == false);
                return list;
            }
        }

        //internal IEnumerable<Viw_postList> checkIsDelete(IEnumerable<Viw_postList> postList, reqPostList request)
        //{
        //    if (request.IsDelete == true)
        //    {
        //        IEnumerable<Models.Viw_postList> list = postList.Where(x => x.IsDeleted == true);
        //        return list;
        //    }
        //    else
        //    {
        //        IEnumerable<Models.Viw_postList> list = postList.Where(x => x.IsDeleted == false);
        //        return list;
        //    }
        //}
        //internal IEnumerable<Viw_postList> checkReciveMessage(IEnumerable<Viw_postList> postList, reqPostList request)
        //{
        //    if (request.ReciveMessage == true)
        //    {
        //        IEnumerable<Models.Viw_postList> list = postList.Where(x => x.ReciveMessage == true);
        //        return list;
        //    }
        //    else
        //    {
        //        IEnumerable<Models.Viw_postList> list = postList.Where(x => x.ReciveMessage == false);
        //        return list;
        //    }
        //}

        internal IEnumerable<Viw_postList> checkAmount(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.PriceForVisit != 0)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.PriceForVsit == request.PriceForVisit);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Viw_postList> checkApplication(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.IDApplication != 0)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.IDApplication == request.IDApplication);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Viw_postList> checkMinAmount(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.PriceForVisitMin != 0)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.PriceForVsit >= request.PriceForVisitMin);
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Viw_postList> checkMaxAmount(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.PriceForVisitMax != 0)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.PriceForVsit <= request.PriceForVisitMax);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Viw_postList> checkCityCode(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.CityCode != 0)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.CityCode == request.CityCode);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Viw_postList> checkStateCode(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.StateCode != 0)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.StateCode == request.StateCode);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Viw_postList> checkTransactionTypeCode(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.ttc != 0)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.PostTypeCode == request.ttc);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Viw_postList> checkAppID(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.IDApplication != 0)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.IDApplication == request.IDApplication);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_MyPostListResult> checkMyPostAppID(IEnumerable<Sp_MyPostListResult> postList, reqPostList request)
        {
            if (request.IDApplication != 0)
            {
                IEnumerable<Models.Sp_MyPostListResult> list = postList.Where(x => x.IDApplication == request.IDApplication);
                return list;
            }
            else
            {
                return postList;
            }
        }


        internal IEnumerable<Viw_postList> checkCreatorUserCode(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.CreatorUserCode != null)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.CreatorUserCode == int.Parse(request.CreatorUserCode));
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Viw_postList> checkPublishDateTo(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.PublishDateTo != null)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.PublishDate <= DateTime.Parse(request.PublishDateTo));
                return list;
            }
            else
            {
                return postList;
            }
        }



        internal IEnumerable<Viw_postList> checkPublishDateFrom(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.PublishDateFrom != null)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.PublishDate >= DateTime.Parse(request.PublishDateFrom));
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Viw_postList> checkPublishDate(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.PublishDate != null)
            {
                //todo remove houre minute secound from Date
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.PublishDate == DateTime.Parse(request.PublishDate));
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Viw_postList> checkhExpireDateTo(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.ExpireDateTo != null)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.ExpireDate <= DateTime.Parse(request.ExpireDateTo));
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Viw_postList> checkExpireDateFrom(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.ExpireDateFrom != null)
            {
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.ExpireDate >= DateTime.Parse(request.ExpireDateFrom));
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Viw_postList> checkExpireDate(IEnumerable<Viw_postList> postList, reqPostList request)
        {
            if (request.ExpireDate != null)
            {
                //todo remove houre minute secound from Date
                IEnumerable<Models.Viw_postList> list = postList.Where(x => x.ExpireDate == DateTime.Parse(request.ExpireDate));
                return list;
            }
            else
            {
                return postList;
            }
        }



        //---------------------------------------------


        internal IEnumerable<Sp_postList_loggedinResult> user_searchString(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.Search != null)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(
                    x =>
                    x.Title.Contains(request.Search) ||
                    x.Text.Contains(request.Search) ||
                    //x.CreatorName.Contains(request.Search) ||
                    //x.CreatorFamily.Contains(request.Search) ||
                    x.StateName.Contains(request.Search) ||
                    x.CityName.Contains(request.Search));
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_postList_loggedinResult> user_checkIsActive(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.IsActive == true)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.IsActive == true);
                return list;
            }
            else
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.IsActive == false);
                return list;
            }
        }

        //internal IEnumerable<Sp_postList_loggedinResult> checkIsDelete(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        //{
        //    if (request.IsDelete == true)
        //    {
        //        IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.IsDeleted == true);
        //        return list;
        //    }
        //    else
        //    {
        //        IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.IsDeleted == false);
        //        return list;
        //    }
        //}
        //internal IEnumerable<Sp_postList_loggedinResult> checkReciveMessage(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        //{
        //    if (request.ReciveMessage == true)
        //    {
        //        IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.ReciveMessage == true);
        //        return list;
        //    }
        //    else
        //    {
        //        IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.ReciveMessage == false);
        //        return list;
        //    }
        //}

        internal IEnumerable<Sp_postList_loggedinResult> user_checkAmount(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.PriceForVisit != 0)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.PriceForVsit == request.PriceForVisit);
                return list;
            }
            else
            {
                return postList;
            }
        }
        
        internal IEnumerable<Sp_postList_loggedinResult> user_checkFav(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.Faved == true)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.IsFav == "True");
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Sp_postList_loggedinResult> user_checkBuy(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.Visited == true)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.IsSeen == "True");
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_postList_loggedinResult> user_checkMinAmount(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.PriceForVisitMin != 0)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.PriceForVsit >= request.PriceForVisitMin);
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Sp_postList_loggedinResult> user_checkMaxAmount(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.PriceForVisitMax != 0)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.PriceForVsit <= request.PriceForVisitMax);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_postList_loggedinResult> user_checkCityCode(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.CityCode != 0)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.CityCode == request.CityCode);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_postList_loggedinResult> user_checkStateCode(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.StateCode != 0)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.StateCode == request.StateCode);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_postList_loggedinResult> user_checkTransactionTypeCode(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.ttc != 0)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.PostTypeCode == request.ttc);
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_postList_loggedinResult> user_checkAppID(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.IDApplication != 0)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.IDApplication == request.IDApplication);
                return list;
            }
            else
            {
                return postList;
            }
        }


        internal IEnumerable<Sp_postList_loggedinResult> user_checkCreatorUserCode(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.CreatorUserCode != null)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.CreatorUserCode == int.Parse(request.CreatorUserCode));
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Sp_postList_loggedinResult> user_checkPublishDateTo(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.PublishDateTo != null)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.PublishDate <= DateTime.Parse(request.PublishDateTo));
                return list;
            }
            else
            {
                return postList;
            }
        }



        internal IEnumerable<Sp_postList_loggedinResult> user_checkPublishDateFrom(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.PublishDateFrom != null)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.PublishDate >= DateTime.Parse(request.PublishDateFrom));
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Sp_postList_loggedinResult> user_checkPublishDate(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.PublishDate != null)
            {
                //todo remove houre minute secound from Date
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.PublishDate == DateTime.Parse(request.PublishDate));
                return list;
            }
            else
            {
                return postList;
            }
        }

        internal IEnumerable<Sp_postList_loggedinResult> user_checkhExpireDateTo(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.ExpireDateTo != null)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.ExpireDate <= DateTime.Parse(request.ExpireDateTo));
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Sp_postList_loggedinResult> user_checkExpireDateFrom(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.ExpireDateFrom != null)
            {
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.ExpireDate >= DateTime.Parse(request.ExpireDateFrom));
                return list;
            }
            else
            {
                return postList;
            }
        }
        internal IEnumerable<Sp_postList_loggedinResult> user_checkExpireDate(IEnumerable<Sp_postList_loggedinResult> postList, reqPostList request)
        {
            if (request.ExpireDate != null)
            {
                //todo remove houre minute secound from Date
                IEnumerable<Sp_postList_loggedinResult> list = postList.Where(x => x.ExpireDate == DateTime.Parse(request.ExpireDate));
                return list;
            }
            else
            {
                return postList;
            }
        }

    }
}