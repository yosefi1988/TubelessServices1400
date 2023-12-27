
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

namespace TubelessServices.Controllers.Application
{
 
    [RoutePrefix("api/Application")] 
    public class ApplicationController : ApiController
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        UserCRUD userCRUD = new UserCRUD();
        WalletCRUD walletCRUD = new WalletCRUD();
        ApplicationCRUD AppsCRUD = new ApplicationCRUD();
        ResponseAppList response = new ResponseAppList();


        [HttpPost]
        [Route("ApplicationDetails")]
        public string ApplicationDetails(RegisterPost newPostRequest)
        {


            return "";
        }

        [HttpPost]
        [Route("ApplicationList")]
        public string ApplicationList(reqPostList requestPostList)
        {
            IEnumerable<Viw_AppStore> xxxxxxxxx = AppsCRUD.getApplicationList(8057, 8063);
            //response.postList = xxxxxxxxx;

            //WalletCRUD userCRUD = new WalletCRUD();            
            //response.wallet.Amount = (long)walletCRUD.getWallet(userId).Amount;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
        }


        [HttpPost]
        [Route("ApplicationListByTypeCode")] 

        public string ApplicationListForSiteByTypeCode(reqAppList requestPostList)
        {
            IEnumerable<Viw_Site_AppList> xxxxxxxxx = AppsCRUD.getApplicationListByTypeCode(requestPostList.id, requestPostList.pageIndex, requestPostList.pageSize);
            response.postList = xxxxxxxxx.ToList();
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
        }

        [HttpPost]
        [Route("ApplicationListByCompanyCode")] 
        public string ApplicationListForSiteByCompanyCode(reqAppList requestPostList)
        {
            IEnumerable<Viw_Site_AppList> xxxxxxxxx = AppsCRUD.getApplicationListByCompanyCode(requestPostList.id, requestPostList.pageIndex, requestPostList.pageSize);
            response.postList = xxxxxxxxx.ToList();
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
        }
    }
}
