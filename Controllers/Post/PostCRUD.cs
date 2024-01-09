using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Controllers.Autenticator;
using TubelessServices.Models;
using TubelessServices.Models.Autenticator.Request;
using TubelessServices.Models.Post.Request;
using TubelessServices.Models.Post;
using System.Data.Linq;
using System.Globalization;
using TubelessServices.Classes.Utility;

namespace TubelessServices.Controllers.Post
{
    public class PostCRUD
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        UserCRUD userCRUD = new UserCRUD();
        public enum userTypeCode { user = 1, creator = 2, admin = 3 }
        public bool addPostToUserSeens(int idPost , int idUser)
        {
            try
            {
                if (postIsBuy(idPost,idUser))
                {
                    return true;
                }
                else
                {
                    Tbl_Visited_Post newVisitedPost = new Tbl_Visited_Post();
                    newVisitedPost.Date = DateTime.Now;
                    newVisitedPost.IDPost = idPost;
                    newVisitedPost.IDUser = idUser;

                    db.Tbl_Visited_Posts.InsertOnSubmit(newVisitedPost);
                    db.SubmitChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
        internal int regNewPost(RegisterPost newPostRequest,Tbl_User user)
        {
            Tbl_Post newPost = new Tbl_Post();
            try
            {
                newPost.IDUser = user.Id;
                newPost.IDApplication = newPostRequest.IDApplication;
                newPost.Title = newPostRequest.Title;
                newPost.Text = newPostRequest.Text;
                newPost.TypeCode = newPostRequest.ttc;
                newPost.StateCode = newPostRequest.StateCode;
                newPost.CityCode = newPostRequest.CityCode;
                newPost.PriceForVsit = Decimal.Parse(newPostRequest.Amount);
                  
                if (user.UserTypeCode == (int)userTypeCode.admin)
                    newPost.IsActive = true;
                else
                {
                    if (newPostRequest.IDApplication == (int)PostController.applications.Amlak)
                    {
                        if (user.UserTypeCode == (int)userTypeCode.admin || user.UserTypeCode == (int)userTypeCode.creator)
                        {
                            newPost.IsActive = true;
                        }
                        else
                        {
                            newPost.IsActive = false;
                        }
                    }
                    else if(newPostRequest.IDApplication == (int)PostController.applications.Tubeless)
                    {
                        if (user.UserTypeCode == (int)userTypeCode.admin || user.UserTypeCode == (int)userTypeCode.creator)
                        {
                            newPost.IsActive = true;
                        }
                        else
                        {
                            newPost.IsActive = false;
                        }
                    } 
                    else
                    {
                        newPost.IsActive = false;
                    }
                }

                newPost.IsDeleted = false;

                if (newPostRequest.ReciveMessage)
                    newPost.ReciveMessage = true;
                else
                    newPost.ReciveMessage = false;

                newPost.CreatedOn = DateTime.Now;
                newPost.PublishDate = DateTime.Parse(newPostRequest.PublishDate);
                newPost.ExpireDate = DateTime.Parse(newPostRequest.ExpireDate);
                newPost.IP = newPostRequest.IP;
                newPost.VisitCount = 0;

                db.Tbl_Posts.InsertOnSubmit(newPost);
                db.SubmitChanges();

                if (newPostRequest.ttc == 7)
                {
                    regNewMPostAmounts(newPostRequest , newPost.Id);
                }
                return newPost.Id;
            }
            catch (Exception e)
            {
                int a = 0;
                a++;
                return 0;
            }
        }
        internal int regNewMPostAmounts(RegisterPost newPostRequest, int idPost)
        {
            foreach (RegisterPost_Item item in newPostRequest.Items)
            {
                Tbl_Post_Amount newAmount = new Tbl_Post_Amount();
                newAmount.IdPost = idPost;
                newAmount.Value = item.Value;
                newAmount.Text = item.Text;
                db.Tbl_Post_Amounts.InsertOnSubmit(newAmount);
            }
            db.SubmitChanges();
            return 0;
        }

        internal void diActivePost(int postId)
        {
            var post = (from record in db.Tbl_Posts
                        where record.Id == postId
                        select record).Single(); 
            post.IsActive = false;
            db.SubmitChanges();
        }

        public bool togglePostFavs(int idPost, int idUser)
        {
            try
            {
                if (postIsFav(idPost, idUser))
                {
                    var favPost = (from record in db.Tbl_Faved_Posts
                                   where record.IDPost == idPost &&
                                         record.IDUser == idUser
                                   select record).First();

                    db.Tbl_Faved_Posts.DeleteOnSubmit(favPost);
                    db.SubmitChanges();
                    return true;
                }
                else
                {

                    Tbl_Faved_Post favPost = new Tbl_Faved_Post();
                    favPost.Date = DateTime.Now;
                    favPost.IDPost = idPost;
                    favPost.IDUser = idUser;

                    db.Tbl_Faved_Posts.InsertOnSubmit(favPost);
                    db.SubmitChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }

        }


        public bool toggleAcceptPost(int idPost, int idUser)
        {
            try
            {
                var post = (from record in db.Tbl_Posts
                            where record.Id == idPost
                            select record).First();

                bool currentState = post.IsActive;
                post.IsActive = !currentState;
                post.AcceptDate = DateTime.Now;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool toggleDeletePost(int idPost, int idUser)
        {
            try
            {
                var post = (from record in db.Tbl_Posts
                            where record.Id == idPost
                            select record).First();

                bool currentState = post.IsActive;
                post.IsDeleted = !currentState;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void updatePostCount(int idPost)
        {
            var post = (from record in db.Tbl_Posts
                        where record.Id == idPost
                        select record).Single();
            post.VisitCount = post.VisitCount + 1;
            //db.Tbl_Posts.s(post);
            db.SubmitChanges();
        }

        internal List<PostItem> getMyPostList(reqPostList reqPostList, int userId)
        {
            PostSearchHelper helper = new Post.PostSearchHelper();

            List<PostItem> newList = new List<PostItem>();
            IEnumerable<Sp_MyPostListResult> postList2 = db.Sp_MyPostList(userId + "");

            IEnumerable<Sp_MyPostListResult> finalList =
                helper.checkMyPostAppID(
                    helper.searchMylistString(postList2, reqPostList),
                reqPostList);

            IEnumerable<Models.Sp_MyPostListResult> transactionList2 = finalList.Skip(reqPostList.pageIndex * reqPostList.pageSize);
            IEnumerable<Models.Sp_MyPostListResult> transactionList3 = transactionList2.Take(reqPostList.pageSize);           
            return preparelist3(transactionList3);
        }

        private List<PostItem> preparelist3(IEnumerable<Sp_MyPostListResult> transactionList3)
        {
            List<PostItem> transactionListOut = new List<PostItem>();

            foreach (Sp_MyPostListResult postitem in transactionList3)
            {
                PostItem postItem = new PostItem();
                postItem.ID = postitem.IDPost;
                postItem.CreatorFullName = postitem.CreatorName + " " + postitem.CreatorFamily;
                postItem.TTC = postitem.PostTypeCode;
                postItem.TTN = postitem.PostTypeName;
                //postItem.image = postitem.
                //postItem.icon = postitem.;
                //postItem.RefrenceNo = postitem.Title;
                postItem.title = postitem.Title;
                postItem.Amount = (long)postitem.PriceForVsit;
                //postItem.Zarib = postitem.;

                
                postItem.DateTime = Date.convertToPersianDate(postitem.CreatedOn);
                postItem.DateTimeExpire = Date.convertToPersianDate(postitem.ExpireDate);
                //postItem.isFav = postitem.IsFav == "True" ? true : false;
                //postItem.isSeen = postitem.IsSeen == "True" ? true : false;
                postItem.TitlePicture = postitem.TitlePicture;
                transactionListOut.Add(postItem);
            }

            return transactionListOut;
        }

        internal List<PostItem> getPostList(reqPostList reqPostList, int userId)
        {
            PostSearchHelper helper = new Post.PostSearchHelper();
            if (userId == 0)
            {
                IEnumerable<Viw_postList> postList1 = (from x in db.Viw_postLists
                                                              where
                                                                  //x.IsActive == true &&
                                                                  x.IsDeleted == false
                                                              select x).OrderByDescending(date => date.CreatedOn).ToList();


                IEnumerable<Viw_postList> finalList =
                helper.checkIsActive(
                            //checkIsDelete(
                            //checkReciveMessage(
                            helper.checkExpireDate(
                                helper.checkExpireDateFrom(
                                    helper.checkhExpireDateTo(
                                        helper.checkPublishDateTo(
                                            helper.checkPublishDateFrom(
                                                helper.checkCreatorUserCode(
                                                    helper.checkTransactionTypeCode(
                                                        helper.checkStateCode(
                                                            helper.checkCityCode(
                                                                helper.checkMaxAmount(
                                                                    helper.checkMinAmount(
                                                                        helper.checkAmount(
                                                                            helper.checkAppID(
                                                                                helper.searchString(postList1, reqPostList),
                                                                            reqPostList),
                                                                        reqPostList),
                                                                    reqPostList),
                                                                reqPostList),
                                                            reqPostList),
                                                        reqPostList),
                                                    reqPostList),
                                                reqPostList),
                                            reqPostList),
                                        reqPostList),
                                    reqPostList),
                                reqPostList),
                            reqPostList),
                //reqPostList),
                //reqPostList),
                reqPostList);

                IEnumerable < Models.Viw_postList> transactionList2 = finalList.Skip(reqPostList.pageIndex * reqPostList.pageSize);
                IEnumerable<Models.Viw_postList> transactionList3 = transactionList2.Take(reqPostList.pageSize);

                return preparelist(transactionList3);
            }
            else
            {
                //1
                //میانگین پاسخ 500 میلی ثانیه برای دیتابیس خالی
                //List<PostItem> list = getPostList(reqPostList, 0);
                //foreach (PostItem postitem in list)
                //{
                //    bool isCodeExist = db.Tbl_Visited_Posts.Any(record =>
                //         record.IDPost == postitem.ID &&
                //         record.IDUser == userId);

                //    if (isCodeExist)
                //        postitem.isSeen = true;


                //    bool isCodeExist2 = db.Tbl_Faved_Posts.Any(record =>
                //         record.IDPost == postitem.ID &&
                //         record.IDUser == userId);

                //    if (isCodeExist2)
                //        postitem.isFav = true;
                //}
                //return list;

                //2
                //میانگین پاسخ 180 میلی ثانیه برای دیتابیس خالی
                List<PostItem> newList = new List<PostItem>();
                IEnumerable<Sp_postList_loggedinResult> postList2 = db.Sp_postList_loggedin(userId + "");


                IEnumerable<Sp_postList_loggedinResult> finalList =
                helper.user_checkIsActive(
                    helper.user_checkExpireDate(
                        helper.user_checkExpireDateFrom(
                            helper.user_checkhExpireDateTo(
                                helper.user_checkPublishDateTo(
                                    helper.user_checkPublishDateFrom(
                                        helper.user_checkCreatorUserCode(
                                            helper.user_checkTransactionTypeCode(
                                                helper.user_checkStateCode(
                                                    helper.user_checkCityCode(
                                                        helper.user_checkMaxAmount(
                                                            helper.user_checkMinAmount(
                                                                helper.user_checkAmount(
                                                                    helper.user_checkBuy(
                                                                        helper.user_checkFav(
                                                                            helper.user_checkAppID(
                                                                                helper.user_searchString(postList2, reqPostList),
                                                                            reqPostList),
                                                                        reqPostList),
                                                                    reqPostList),
                                                                reqPostList),
                                                            reqPostList),
                                                        reqPostList),
                                                    reqPostList),
                                                reqPostList),
                                            reqPostList),
                                        reqPostList),
                                    reqPostList),
                                reqPostList),
                            reqPostList),
                        reqPostList),
                    reqPostList),
                reqPostList);

                IEnumerable<Sp_postList_loggedinResult> transactionList2 = finalList.Skip(reqPostList.pageIndex * reqPostList.pageSize);
                IEnumerable<Sp_postList_loggedinResult> transactionList3 = transactionList2.Take(reqPostList.pageSize);

                return preparelist2(transactionList3);
            }
        }

        internal List<PostItem> getPostListForSite(reqPostList reqPostList, int userId)
        {
            PostSearchHelper helper = new Post.PostSearchHelper();
            IEnumerable<Viw_postList> postList1 = (from x in db.Viw_postLists
                                                    where
                                                        //x.IsActive == true &&
                                                        x.IsDeleted == false
                                                    select x).OrderByDescending(date => date.CreatedOn).ToList();


            IEnumerable<Viw_postList> finalList =
            helper.checkIsActive(
                //checkIsDelete(
                //checkReciveMessage(
                    //helper.checkTransactionTypeCode(
                        helper.checkAppID(
                            postList1,
                        reqPostList),
                    //reqPostList),
                //reqPostList),
            //reqPostList),
            reqPostList);

            IEnumerable<Models.Viw_postList> transactionList2 = finalList.Skip(reqPostList.pageIndex * reqPostList.pageSize);
            IEnumerable<Models.Viw_postList> transactionList3 = transactionList2.Take(reqPostList.pageSize);
            return preparelistForsite(transactionList3);
        }

        private List<PostItem> preparelistForsite(IEnumerable<Viw_postList> transactionList3)
        {
            List<PostItem> transactionListOut = new List<PostItem>();
            foreach (Models.Viw_postList postitem in transactionList3)
            {
                PostItem postItem = new PostItem();
                postItem.ID = postitem.IDPost;
                postItem.CreatorFullName = postitem.CreatorName + " " + postitem.CreatorFamily;
                postItem.TTC = postitem.PostTypeCode;
                postItem.TTN = postitem.PostTypeName;

                if (postitem.Text.Length > 500)
                    postItem.text = postitem.Text.Substring(0, 500);
                else
                    postItem.text = postitem.Text;

                //postItem.image = postitem.
                //postItem.icon = postitem.;
                //postItem.RefrenceNo = postitem.Title;
                postItem.title = postitem.Title;
                postItem.Amount = (long)postitem.PriceForVsit;
                //postItem.Zarib = postitem.;

                postItem.ReciveMessage = (bool)postitem.ReciveMessage;
                postItem.StateName = postitem.StateName;
                postItem.CityName = postitem.CityName;
                postItem.TitlePicture = postitem.TitlePicture;

                postItem.DateTime = Date.convertToPersianDate(postitem.CreatedOn);
                postItem.DateTimeExpire = Date.convertToPersianDate(postitem.ExpireDate);

                transactionListOut.Add(postItem);
            }

            return transactionListOut;
        }

        private List<PostItem> preparelist(IEnumerable<Viw_postList> transactionList3)
        {
            List<PostItem> transactionListOut = new List<PostItem>();
            foreach (Models.Viw_postList postitem in transactionList3)
            {
                PostItem postItem = new PostItem();
                postItem.ID = postitem.IDPost;
                postItem.CreatorFullName = postitem.CreatorName + " " + postitem.CreatorFamily;
                postItem.TTC = postitem.PostTypeCode;
                postItem.TTN = postitem.PostTypeName;
                //postItem.image = postitem.
                //postItem.icon = postitem.;
                //postItem.RefrenceNo = postitem.Title;
                postItem.title = postitem.Title;
                postItem.Amount = (long) postitem.PriceForVsit;
                //postItem.Zarib = postitem.;

                postItem.ReciveMessage = (bool) postitem.ReciveMessage;
                postItem.StateName = postitem.StateName;
                postItem.CityName = postitem.CityName;
                postItem.TitlePicture = postitem.TitlePicture;

                postItem.DateTime = Date.convertToPersianDate(postitem.CreatedOn);
                postItem.DateTimeExpire = Date.convertToPersianDate(postitem.ExpireDate);

                transactionListOut.Add(postItem);
            }

            return transactionListOut;
        }
        private List<PostItem> preparelist2(IEnumerable<Sp_postList_loggedinResult> transactionList3)
        {
            List<PostItem> transactionListOut = new List<PostItem>();

            foreach (Sp_postList_loggedinResult postitem in transactionList3)
            {
                PostItem postItem = new PostItem();
                postItem.ID = postitem.IDPost;
                postItem.CreatorFullName = postitem.CreatorName + " " + postitem.CreatorFamily;
                postItem.TTC = postitem.PostTypeCode;
                postItem.TTN = postitem.PostTypeName;
                //postItem.image = postitem.
                //postItem.icon = postitem.;
                //postItem.RefrenceNo = postitem.Title;
                postItem.title = postitem.Title;
                postItem.Amount = (long)postitem.PriceForVsit;
                //postItem.Zarib = postitem.;


                postItem.ReciveMessage = (bool)postitem.ReciveMessage;
                postItem.StateName = postitem.StateName;
                postItem.CityName = postitem.CityName;
                postItem.TitlePicture = postitem.TitlePicture;


                postItem.DateTime = Date.convertToPersianDate(postitem.CreatedOn); 
                postItem.DateTimeExpire = Date.convertToPersianDate(postitem.ExpireDate); 

                postItem.isFav = postitem.IsFav == "True" ? true :false;
                postItem.isSeen = postitem.IsSeen == "True" ? true : false;

                transactionListOut.Add(postItem);
            }

            return transactionListOut;
        }
        public bool AcceptPost(int idPost, int idUser)
        {
            try
            {
                var acceptPost = (from record in db.Tbl_Posts
                               where record.Id == idPost
                               select record).First();

                acceptPost.AcceptDate = DateTime.Now;
                acceptPost.IDUserAcceptor = idUser;
                acceptPost.IsActive = true;
                acceptPost.VisitCount = 0;                
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal Viw_PostDetail getPostDetails(int postId)
        {
            Viw_PostDetail selectedPost = (from record in db.Viw_PostDetails
                                            where record.Id == postId
                                select record).First();

            return selectedPost;
        }


        internal IEnumerable<Tbl_Post_Amount> getPostAmounts(int postId)
        {
            IEnumerable<Tbl_Post_Amount> selectedPostAmounts = (from record in db.Tbl_Post_Amounts
                                where record.IdPost == postId
                                select record);

            return selectedPostAmounts;
        }
        internal List<String> getPostImages(int postId)
        {
            List<String> retList = new List<String>();
            IEnumerable<Tbl_Post_Image> selectedPostImages = (from record in db.Tbl_Post_Images
                                where record.PostId == postId
                                select record);

            foreach (Tbl_Post_Image item in selectedPostImages)
            {
                retList.Add(item.Picture);
            }
            return retList;
        }
        public bool postIsFav(int idPost, int idUser)
        {
            bool isCodeExist = db.Tbl_Faved_Posts.Any(record =>
                                     record.IDPost == idPost &&
                                     record.IDUser == idUser);
            return isCodeExist;
        } 
        public bool postIsBuy(int idPost, int idUser)
        {
            bool isCodeExist = db.Tbl_Visited_Posts.Any(record =>
                         record.IDPost == idPost &&
                         record.IDUser == idUser);
            return isCodeExist;
        }
    }
}