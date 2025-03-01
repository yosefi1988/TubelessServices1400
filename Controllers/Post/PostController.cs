﻿
using TubelessServices.Models;
using TubelessServices.Models.Autenticator.Request;
using TubelessServices.Models.Autenticator.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using TubelessServices.Models.Response;
using TubelessServices.Controllers.Autenticator;
using TubelessServices.Controllers.Post;
using static TubelessServices.Controllers.Wallet.WalletController;
using static TubelessServices.Controllers.Post.PostCRUD;
using TubelessServices.Models.Post.Request;
using TubelessServices.Controllers.Wallet;
using TubelessServices.Models.Post;
using TubelessServices.Models.post.Response;
using TubelessServices.Models.post.Request;
using TubelessServices.Classes.Utility;
using TubelessServices.Models.post;

namespace TubelessServices.Controllers.Post
{

    [RoutePrefix("api/Post")]
    public class PostController : ApiController
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        UserCRUD userCRUD = new UserCRUD();
        WalletCRUD walletCRUD = new WalletCRUD();
        PostCRUD postCRUD = new PostCRUD();
        ResponsePostList response = new ResponsePostList();

        [HttpPost]
        [Route("NewPost2")]
        public string NewPost2(RegisterPost newPostRequest)
        {
            Tbl_User user = userCRUD.findUserByUserCode(newPostRequest.UserCode).First();

            int postId = postCRUD.regNewPost(newPostRequest, user);
            if (postId != 0)
            {
                Tbl_Wallet wallet = walletCRUD.getWallet(user);
                stores = storeCRUD.findStoreForApplication(newPostRequest.Store, newPostRequest.IDApplication);

                bool isPostFree = false;
                try
                {
                    isPostFree = (bool)stores.First().Tbl_ApplicationStorePermissions.Where(xx => xx.UserTypeCode == user.UserTypeCode).FirstOrDefault().IsPostFree;
                }
                catch { }

                if (isPostFree)
                {
                    response.transactionList = new List<Transaction>();

                    Tbl_WalletTransaction trans = new Tbl_WalletTransaction();
                    trans.RefrenceNo = postId.ToString();
                    response.appendTransaction(trans);
                    response.wallet = new UserWallet();
                    response.wallet.Amount = (long)wallet.Amount;
                    string ssssssss = new JavaScriptSerializer().Serialize(response);
                    return ssssssss;
                }
                else
                {
                    #region CreateTRANSACTION

                    int IDUser = user.Id;
                    int TransactionTypeCode;
                    
                    if (newPostRequest.IDApplication == (int)applications.Amlak)
                        TransactionTypeCode = (int)TransactionTypeCodeEnum.CreatePostInAmlak;      //ایجاد پست پولی در اپ املاک
                    else if(newPostRequest.IDApplication == (int)applications.Yafte)
                        TransactionTypeCode = (int)TransactionTypeCodeEnum.CreatePostInYafte;      //ایجاد پست پولی در اپ گمشده ها
                    else if(newPostRequest.IDApplication == (int)applications.Yadaki)
                        TransactionTypeCode = (int)TransactionTypeCodeEnum.CreatePostInYadaki;      //ایجاد پست پولی در اپ لوازم یدکی
                    else if(newPostRequest.IDApplication == (int)applications.Moz)
                        TransactionTypeCode = (int)TransactionTypeCodeEnum.CreatePostInMoz;      //ایجاد پست پولی در اپ مزایده 
                    else
                        TransactionTypeCode = (int)TransactionTypeCodeEnum.CreatePost;      //ایجاد پست


                    float zarib = walletCRUD.getZarib(user.UserCode, TransactionTypeCode);
                    float Amount = float.Parse(userCRUD.getTansactionValue(TransactionTypeCode)) * zarib;
                    int idApp = newPostRequest.IDApplication;

                    bool isWaletTransation = false;
                    try
                    {
                        if (newPostRequest.DirectPay == true)
                            isWaletTransation = false;
                        else
                            isWaletTransation = true;
                    }
                    catch (Exception ex) { }

                    Tbl_WalletTransaction trans = walletCRUD.registerNewTransaction(wallet.Id, IDUser, IDUser, idApp, Amount, zarib, TransactionTypeCode, postId + "", null, newPostRequest.IP, isWaletTransation);
                    if (trans != null)
                    { 
                        //if (wallet.Amount < (Decimal)Amount)
                        //    postCRUD.diActivePost(postId); 

                        response.transactionList = new List<Transaction>();
                        response.appendTransaction(trans);

                        response.wallet = new UserWallet();
                        response.wallet.Amount = (long)wallet.Amount + (long)Amount;

                        string ssssssss = new JavaScriptSerializer().Serialize(response);
                        return ssssssss;
                    }
                    else
                    {
                        postCRUD.diActivePost(postId);
                        ServerResponse responsex = new ServerResponse();
                        responsex.tubelessException.code = -2;
                        responsex.tubelessException.message = "error in create post";
                        string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                        return ssssssss;
                    }
                    #endregion
                }
            }
            else
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -4;
                responsex.tubelessException.message = "error in create post";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
        }


        public enum applications
        {
            Yafte = 5,
            Yadaki = 4,
            Moz = 2 ,
            Estekhdam = 3,
            Amlak = 1008,
            Tubeless = 1010
        }
        bool canSendDirectPost(int iDApplication)
        {
            if ((iDApplication == (int) applications.Yafte || 
                iDApplication == (int)applications.Yadaki || 
                iDApplication == (int)applications.Moz || 
                iDApplication == (int)applications.Amlak ||
                iDApplication == (int)applications.Estekhdam))
            {
                return true;
            }
            return false;
        }

        bool sendPostIsFree(int iDApplication)
        {
            return sendPostIsFree(iDApplication, 0);
        }

        bool sendPostIsFree(int iDApplication , int UserTypeCode)
        {
            if (iDApplication == (int)applications.Estekhdam)
            {
                return true;
            }
            else if ( iDApplication == (int)applications.Amlak && (UserTypeCode == (int)userTypeCode.admin || UserTypeCode == (int)userTypeCode.user))
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("toggleFavPost")]
        public string toggleFavPost(FavRequest request)
        {
            Tbl_User user = userCRUD.findUserByUserCode(request.UserCode).First();
            ServerResponse responsex = new ServerResponse();

            if (postCRUD.togglePostFavs(request.IDPost, user.Id))
            {
                responsex.tubelessException.code = 200;
                responsex.tubelessException.message = "ok";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
            else
            {
                responsex.tubelessException.code = -1;
                responsex.tubelessException.message = "error in FavPost";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
        }


        [HttpPost]
        [Route("toggleAcceptPost")]
        public string toggleAcceptPost(FavRequest request)
        {
            Tbl_User user = userCRUD.findUserByUserCode(request.UserCode).First();
            ServerResponse responsex = new ServerResponse();

            if (postCRUD.toggleAcceptPost(request.IDPost, user.Id))
            {
                responsex.tubelessException.code = 200;
                responsex.tubelessException.message = "ok";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
            else
            {
                responsex.tubelessException.code = -2;
                responsex.tubelessException.message = "error in AcceptPost";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
        }

        [HttpPost]
        [Route("toggleDeletePost")]
        public string toggleDeletePost(FavRequest request)
        {
            Tbl_User user = userCRUD.findUserByUserCode(request.UserCode).First();
            ServerResponse responsex = new ServerResponse();

            if (postCRUD.toggleDeletePost(request.IDPost, user.Id))
            {
                responsex.tubelessException.code = 200;
                responsex.tubelessException.message = "ok";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
            else
            {
                responsex.tubelessException.code = -3;
                responsex.tubelessException.message = "error in DeletePost";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
        }

        [HttpPost]
        [Route("PostList")]
        public string PostList(reqPostList requestPostList)
        {
            if (requestPostList.UserCode == null)
            {
                List<PostItem> xxxxxxxxx = postCRUD.getPostList(requestPostList, 0);
                response.postList = xxxxxxxxx;
            }
            else
            {
                int userId = userCRUD.getUserId(requestPostList.UserCode);
                if (userId == 0)
                {
                    ServerResponse responsex = new ServerResponse();
                    responsex.tubelessException.code = -5;
                    responsex.tubelessException.message = "user not found";
                    string sssssss = new JavaScriptSerializer().Serialize(responsex);
                    return sssssss;
                }
                else
                {
                    List<PostItem> xxxxxxxxx = postCRUD.getPostList(requestPostList, userId);
                    response.postList = xxxxxxxxx;
                }
            }

            //WalletCRUD userCRUD = new WalletCRUD();            
            //response.wallet.Amount = (long)walletCRUD.getWallet(userId).Amount;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
        }

        [HttpPost]
        [Route("PostListWithAd")]
        public string PostListWithAd(reqPostList requestPostList)
        {
            List<PostItem> adItems = new List<PostItem>();
            PostItem adItem = null;
            stores = storeCRUD.findStoreForApplication(requestPostList.Store, requestPostList.IDApplication);

            if ((bool)stores.First().AdEnable)
            {
                adItems = postCRUD.getAdList(requestPostList, 0);
                //checkOwnerAd1 postCRUD.getAdList(requestPostList, 0)

                if (adItems.Count >= 1 && checkOwnerAmountForAd(adItems[0]))
                {
                    adItem = adItems[0];
                }
                else
                {
                    if(adItems.Count >= 2 && checkOwnerAmountForAd(adItems[1]))
                    {
                        adItem = adItems[1];
                    }
                    else if (adItems.Count >= 3 && checkOwnerAmountForAd(adItems[2]))
                    {
                        adItem = adItems[2];
                    }                    
                }

            }

            if (adItem != null)
                requestPostList.pageSize = requestPostList.pageSize - 1;

            if (requestPostList.UserCode == null)
            {
                List<PostItem> postlist = postCRUD.getPostList(requestPostList, 0);
                response.postList = postlist;
                if(adItem != null)
                    response.postList.Add(adItem);
            }
            else
            {
                int userId = userCRUD.getUserId(requestPostList.UserCode);
                if (userId == 0)
                {
                    ServerResponse responsex = new ServerResponse();
                    responsex.tubelessException.code = -5;
                    responsex.tubelessException.message = "user not found";
                    string sssssss = new JavaScriptSerializer().Serialize(responsex);
                    return sssssss;
                }
                else
                {
                    List<PostItem> postlist = postCRUD.getPostList(requestPostList, userId); 
                    response.postList = postlist;
                    if (adItem != null)
                        response.postList.Add(adItem);
                }
            }

            //WalletCRUD userCRUD = new WalletCRUD();            
            //response.wallet.Amount = (long)walletCRUD.getWallet(userId).Amount;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
        }
         
        private bool checkOwnerAmountForAd(PostItem postItem)
        {
            if(walletCRUD.getWallet(Int32.Parse(postItem.IdOwner)).Amount  >= postItem.Amount * 5)
                return true;
            else
                return false;
        }

        [HttpPost]
        [Route("MyPostList")]
        public string MyPostList(reqPostList requestPostList)
        {
            if (requestPostList.UserCode != null)
            {
                int userId = userCRUD.getUserId(requestPostList.UserCode);
                if (userId != 0)
                {
                    List<PostItem> xxxxxxxxx = postCRUD.getMyPostList(requestPostList, userId);
                    response.postList = xxxxxxxxx;
                    string ssssssss = new JavaScriptSerializer().Serialize(response);
                    return ssssssss;
                }
                else
                {
                    ServerResponse responsex = new ServerResponse();
                    responsex.tubelessException.code = -61;
                    responsex.tubelessException.message = "user not found";
                    string sssssss = new JavaScriptSerializer().Serialize(responsex);
                    return sssssss;
                }
            }
            else
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -62;
                responsex.tubelessException.message = "user not found";
                string sssssss = new JavaScriptSerializer().Serialize(responsex);
                return sssssss;
            }
        }

        [HttpPost]
        [Route("AcceptPost")]
        public string AcceptPost(FavRequest request)
        {
            Tbl_User user = userCRUD.findUserByUserCode(request.UserCode).First();
            ServerResponse responsex = new ServerResponse();

            if (user.UserTypeCode == (int)userTypeCode.admin)
            {
                if (postCRUD.AcceptPost(request.IDPost, user.Id))
                {
                    responsex.tubelessException.code = 200;
                    responsex.tubelessException.message = "ok";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }
                else
                {
                    responsex.tubelessException.code = -2;
                    responsex.tubelessException.message = "error in accept post";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }
            }
            else
            {
                responsex.tubelessException.code = -1;
                responsex.tubelessException.message = "error in accept post";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
        }

        List<Tbl_ApplicationStore> stores = new List<Tbl_ApplicationStore>();
        Store.StoreCRUD storeCRUD = new Store.StoreCRUD();

        [HttpPost]
        [Route("PostDetailsNew2")]
        public string PostDetailsNew2(DetailsRequest request)
        {
            ResponsePost response = new ResponsePost();

            //Initialize
            int idApp = request.IDApplication;
            stores = storeCRUD.findStoreForApplication(request.Store, request.IDApplication);
            Tbl_User userCaller = userCRUD.findUserByUserCode(request.UserCode).First();
            Viw_PostDetail postDetails = postCRUD.getPostDetails(request.IDPost);

            Models.Tbl_Wallet walletCallerUser = walletCRUD.getWallet((int)userCaller.Id);
            Tbl_User userCreator;
            bool postIsBuy = false;

            if (userCaller.Id == (int)postDetails.IDUser)
                userCreator = userCaller;
            else
                userCreator = userCRUD.findUserByID((int)postDetails.IDUser).First();


            float Amount;
            int TransactionTypeCode = 0;

            bool isViewFree = false;
            try
            {
                isViewFree = (bool)stores.First().Tbl_ApplicationStorePermissions.Where(xx => xx.UserTypeCode == userCaller.UserTypeCode).FirstOrDefault().IsViewFree;
            }
            catch { }

            if (isViewFree == false)
                postIsBuy = postCRUD.postIsBuy(request.IDPost, (int)userCaller.Id);

            if (isViewFree || postIsBuy || (userCaller.Id == userCreator.Id))
            {
                //رایگان یا خریداری شده یا پست خودش
                TransactionTypeCode = (int)TransactionTypeCodeEnum.SeePostFree;
                float zarib = walletCRUD.getZarib(userCaller.UserCode, TransactionTypeCode);
                Amount = (-1 * (float)postDetails.PriceForVsit) * zarib;

                //Tbl_WalletTransaction trans = walletCRUD.registerNewTransaction(walletCallerUser.Id, (int)userCaller.Id, IDUserCreator, idApp, Amount, zarib, TransactionTypeCode, postDetails.Id + "", null, request.Ip);
                //if (trans != null)
                //{

                if (postIsBuy == false)
                    postCRUD.addPostToUserSeens(request.IDPost, (int)userCaller.Id);

                //}
                postCRUD.updatePostCount(postDetails.Id);

                IEnumerable<Tbl_Post_Amount> getPostAmount = postCRUD.getPostAmounts(request.IDPost);
                List<String> postImages = postCRUD.getPostImages(request.IDPost);
                response.creator = preparePostCreator(userCreator);
                bool postIsFav = postCRUD.postIsFav(request.IDPost, (int)userCaller.Id);
                return preparePostDetailsResponse(response, postDetails, (long)Amount, walletCallerUser, postIsFav, postImages, getPostAmount, postIsBuy, zarib);
            }
            else
            {
                //دیدن پست پولی
                TransactionTypeCode = (int)TransactionTypeCodeEnum.SeePost;
                float zarib = walletCRUD.getZarib(userCaller.UserCode, TransactionTypeCode);
                Amount = (-1 * (float)postDetails.PriceForVsit) * zarib;

                if (walletCallerUser.Amount + (decimal)Amount >= 0)
                {
                    //پولش به اندازه کافی هست
                    Tbl_WalletTransaction trans = walletCRUD.registerNewTransaction(walletCallerUser.Id, (int)userCaller.Id, (int)userCaller.Id, idApp, Amount, zarib, TransactionTypeCode, postDetails.Id + "", null, request.Ip,true);
                    if (trans != null)
                    {
                        postCRUD.addPostToUserSeens(request.IDPost, (int)userCaller.Id);
                    }
                    postCRUD.updatePostCount(postDetails.Id);
                    //if (userCaller.PhoneNumberConfirmed)

                    IEnumerable<Tbl_Post_Amount> getPostAmount = postCRUD.getPostAmounts(request.IDPost);
                    List<String> postImages = postCRUD.getPostImages(request.IDPost);
                    response.creator = preparePostCreator(userCreator);
                    bool postIsFav = postCRUD.postIsFav(request.IDPost, (int)userCaller.Id);

                    return preparePostDetailsResponse(response, postDetails, (long)Amount, walletCallerUser, postIsFav, postImages, getPostAmount, postIsBuy, zarib);
                    //else
                    //    return mobileNotConfirmed();

                }
                else
                {
                    //پول به اندازه کافی نداره
                    //return error
                    ServerResponse responsex = new ServerResponse();
                    responsex.tubelessException.code = -10;
                    responsex.tubelessException.message = "Amount not enough 1";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }
            }
        }


        [HttpPost]
        [Route("ClickOnAdShowDetails")]
        public string ClickOnAdShowDetails(DetailsRequest request)
        {
            ResponsePost response = new ResponsePost();

            //Initialize
            int idCallerApp = request.IDApplication;
            stores = storeCRUD.findStoreForApplication(request.Store, request.IDApplication);
            Viw_PostDetail postDetails = postCRUD.getPostDetails(request.IDPost);

            Tbl_User userCaller = userCRUD.findUserByUserCode(request.UserCode).First();
            Tbl_Wallet walletCallerUser = walletCRUD.getWallet((int)userCaller.Id);

            Tbl_Wallet walletOwner;
            Tbl_User userOwner;

            if (userCaller.Id == (int)postDetails.IDUser)
            {
                userOwner = userCaller;
                walletOwner = walletCallerUser;
            }
            else
            {
                userOwner = userCRUD.findUserByID((int)postDetails.IDUser).First();
            }

            response.creator = preparePostCreator(userOwner);
            bool postIsBuy = false;
            int IDUserCaller = userCaller.Id;

            float AmountOwner;
            float AmountUser;

            if (userCaller.Id != userOwner.Id)
                postIsBuy = postCRUD.postIsBuy(request.IDPost, (int)userCaller.Id);


            if (postIsBuy || (userCaller.Id == userOwner.Id))
            {                
                //رایگان یا خریداری شده یا پست خودش
                int TransactionTypeCode = 0;
                TransactionTypeCode = (int)TransactionTypeCodeEnum.SeePostFree;
                float zarib = walletCRUD.getZarib(userCaller.UserCode, TransactionTypeCode);
                //AmountOwner = (-1 * (float)postDetails.PriceForVsit) * zarib;
                //AmountUser = (-1 * (float)postDetails.PriceForVsit) * zarib;

                //Tbl_WalletTransaction trans = walletCRUD.registerNewTransaction(walletCallerUser.Id, (int)userCaller.Id, IDUserCreator, idApp, Amount, zarib, TransactionTypeCode, postDetails.Id + "", null, request.Ip);
                //if (trans != null)
                //{
                postCRUD.addPostToUserSeens(request.IDPost, (int)userCaller.Id);
                //}
                postCRUD.updatePostCount(postDetails.Id);

                IEnumerable<Tbl_Post_Amount> getPostAmount = postCRUD.getPostAmounts(request.IDPost);
                List<String> postImages = postCRUD.getPostImages(request.IDPost);
                bool postIsFav = postCRUD.postIsFav(request.IDPost, (int)userCaller.Id);

                return preparePostDetailsResponse(response, postDetails,0, walletCallerUser, postIsFav, postImages, getPostAmount, postIsBuy, zarib);
            }
            else
            {
                walletOwner = walletCRUD.getWallet((int)userOwner.Id);

                //دیدن پست پولی
                int TransactionTypeCodeForOwner = (int)TransactionTypeCodeEnum.AdSeenByOwner;
                float zaribForOwner = walletCRUD.getZarib(userOwner.UserCode, TransactionTypeCodeForOwner);
                AmountOwner = (-1 * (float)(postDetails.PriceForVsit) * zaribForOwner);

                if (walletOwner.Amount + (decimal)AmountOwner >= 0)
                {
                    //پول آگهی گذار به اندازه کافی هست

                    //تراکنش آگهی گذار
                    Tbl_WalletTransaction transOwner = walletCRUD.registerNewTransaction(
                        walletOwner.Id, 
                        (int)userOwner.Id, 
                        (int)userOwner.Id,
                        idCallerApp,
                        AmountOwner,
                        zaribForOwner,
                        TransactionTypeCodeForOwner, 
                        postDetails.Id + "", 
                        null, 
                        request.Ip,
                        true);

                    int TransactionTypeCode = (int)TransactionTypeCodeEnum.AdSeenByUser;
                    float zaribUser = walletCRUD.getZarib(userCaller.UserCode, TransactionTypeCode);
                    AmountUser = float.Parse(userCRUD.getTansactionValue(TransactionTypeCode)) * zaribUser;


                    //تراکنش آگهی بیننده
                    Tbl_WalletTransaction trans = walletCRUD.registerNewTransaction(
                        walletCallerUser.Id, 
                        (int)userCaller.Id, 
                        IDUserCaller,
                        idCallerApp,
                        AmountUser,
                        zaribUser, 
                        TransactionTypeCode, 
                        postDetails.Id + "", 
                        null, 
                        request.Ip,
                        true);

                    if (trans != null)
                    {
                        postCRUD.addPostToUserSeens(request.IDPost, (int)userCaller.Id);
                    }
                    postCRUD.updatePostCount(postDetails.Id);
                    IEnumerable<Tbl_Post_Amount> getPostAmount = postCRUD.getPostAmounts(request.IDPost);
                    List<String> postImages = postCRUD.getPostImages(request.IDPost);
                    bool postIsFav = postCRUD.postIsFav(request.IDPost, (int)userCaller.Id);

                    //if (userCaller.PhoneNumberConfirmed)
                    return preparePostDetailsResponse(response, postDetails, (long)AmountUser, walletCallerUser, postIsFav, postImages, getPostAmount, postIsBuy, zaribUser);
                    //else
                    //    return mobileNotConfirmed();

                }
                else
                {
                    //پول به اندازه کافی نداره
                    //return error
                    ServerResponse responsex = new ServerResponse();
                    responsex.tubelessException.code = -10;
                    responsex.tubelessException.message = "Amount not enough 1";
                    string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                    return ssssssss;
                }
            }
        }


        private User preparePostCreator(Tbl_User userCreator)
        {
            return new User()
            {
                UserCode = userCreator.UserCode,
                Email = userCreator.Email,
                UserName = userCreator.UserName,
                Mobile = userCreator.Mobile,
                MobileNumberConfirmed = userCreator.PhoneNumberConfirmed,
                Name = userCreator.Name,
                Family = userCreator.Family,
                Avatar = userCreator.Avatar,
                UserTypeCode = userCreator.UserTypeCode,
                UserIsActive = userCreator.IsActive,
                UserCreateDate = userCreator.CreatedOn.ToString()
            };
        }

        private string mobileNotConfirmed()
        {
            ServerResponse responsex = new ServerResponse();
            responsex.tubelessException.code = -11;
            responsex.tubelessException.message = "mobile Not confirmed";
            string ssssssss = new JavaScriptSerializer().Serialize(responsex);
            return ssssssss;
        }

        private string preparePostDetailsResponse(
            ResponsePost response, 
            Viw_PostDetail postDetails,
            long Amount, 
            Tbl_Wallet wallet,
            bool postIsFav , 
            List<String> postImages,
            IEnumerable<Tbl_Post_Amount> getPostAmount,
            bool postIsBuy,
            float zarib)
        {
            response.wallet = new UserWallet();
            response.wallet.Amount = (long)wallet.Amount + (long)Amount;
            response.PostItem = new PostFullItem();

            if (postDetails.AcceptDate != null)
                response.PostItem.AcceptDate = Date.convertToPersianDate(postDetails.AcceptDate);

            response.PostItem.Amount = (long)postDetails.PriceForVsit;
            response.PostItem.ApplicationID = (int)postDetails.IDApplication;
            response.PostItem.CityCode = (int)postDetails.CityCode;
            response.PostItem.CityName = postDetails.CityName;
            //response.PostItem.CreateDate = Date.convertToPersianDate(postDetails.CreatedOn);
            response.PostItem.DateTime = Date.convertToPersianDate(postDetails.CreatedOn);
            response.PostItem.CreatorID = (int)postDetails.IDUser;
            //response.PostItem.ExpireDate = Date.convertToPersianDate(postDetails.ExpireDate);
            response.PostItem.DateTimeExpire = Date.convertToPersianDate(postDetails.ExpireDate);

            response.PostItem.icon = postDetails.icon;
            response.PostItem.ID = postDetails.Id;
            response.PostItem.image = postDetails.ImageUrl;
            response.PostItem.IsActive = postDetails.IsActive;
            response.PostItem.IsDeleted = postDetails.IsDeleted;
            response.PostItem.isFav = postIsFav;
            response.PostItem.isSeen = postIsBuy;


            if (getPostAmount.ToList().Count > 0)
            {
                response.PostItem.Amounts = new List<PostAmount>();
                foreach (Tbl_Post_Amount item in getPostAmount)
                {
                    PostAmount amount = new PostAmount();
                    amount.Text = item.Text;
                    amount.Value = item.Value;
                    response.PostItem.Amounts.Add(amount);
                }
            }

            if (postImages.ToList().Count > 0)
            {
                response.PostItem.Images = new List<string>();
                response.PostItem.Images = postImages;
            }


            if (postDetails.ModifiedOn != null)
                response.PostItem.ModifiedDate = Date.convertToPersianDate(postDetails.ModifiedOn);

            response.PostItem.PublishDate = Date.convertToPersianDate(postDetails.PublishDate);
            response.PostItem.ReciveMessage = (bool)postDetails.ReciveMessage;
            response.PostItem.StateCode = (int)postDetails.StateCode;
            response.PostItem.StateName = postDetails.StateName;

            response.PostItem.TextPicture = postDetails.TextPicture;
            response.PostItem.TitlePicture = postDetails.TitlePicture;

            response.PostItem.Text = postDetails.Text;
            response.PostItem.title = postDetails.Title;
            response.PostItem.TTC = postDetails.TypeCode;
            response.PostItem.TTN = postDetails.TypeName;
            response.PostItem.ViewCount = (int)postDetails.VisitCount;
            response.PostItem.Zarib = zarib;


            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
        }

        [HttpPost]
        [HttpGet]
        [Route("PostListForSite")]
        public string PostListForSite(reqPostList requestPostList)
        {
            List<PostItem> xxxxxxxxx = postCRUD.getPostListForSite(requestPostList, 0);
            response.postList = xxxxxxxxx;

            //WalletCRUD userCRUD = new WalletCRUD();            
            //response.wallet.Amount = (long)walletCRUD.getWallet(userId).Amount;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
        }
    }
}