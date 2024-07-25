
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

namespace TubelessServices.Controllers.Wallet
{

    [RoutePrefix("api/Wallet")]
    public class WalletController : ApiController
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        UserCRUD userCRUD = new UserCRUD();
        WalletCRUD walletCRUD = new WalletCRUD();
        PostCRUD postCRUD = new PostCRUD();

        TransactionResponse response = new TransactionResponse();

        [HttpPost]
        [Route("WalletChargeTransaction")]
        public string WalletChargeTransaction(reqTransaction transaction)
        {
            reqTransaction newTransaction = transaction;
            newTransaction.ttc = (int) TransactionTypeCodeEnum.WalletSharje;        //شارژ کیف پول
            return WalletTransaction(transaction);
        }

        public enum TransactionTypeCodeEnum {
            startGift = 4 ,
            SeePost = 6 ,
            WalletSharje = 5,
            CreatePost = 9,
            CreatePostInYafte = 5058,
            CreatePostInAmlak = 8070,
            CreatePostInYadaki = 6056,
            CreatePostInMoz = 9137,
            SeePostFree = 1009}
        

        [HttpPost]
        [Route("WalletTransaction")]
        public string WalletTransaction(reqTransaction transaction)
        {
            int walletId = 0;
            try
            {
                walletId = walletCRUD.getWalletByUserCode(transaction.UserCode);
            }
            catch
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1;
                responsex.tubelessException.message = "you have not wallet";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
            
            if (walletId != 0)
            {
                int IDUser = userCRUD.getUserId(transaction.UserCode);
                int TransactionTypeCode = transaction.ttc;
                float zarib = walletCRUD.getZarib(transaction.UserCode, TransactionTypeCode);
                float Amount = float.Parse(transaction.Amount) * zarib;
                int idApp = userCRUD.getIdApplication(transaction.IDApplicationVersion);

                Tbl_WalletTransaction trans = walletCRUD.registerNewTransaction(walletId, IDUser, IDUser, idApp, Amount, zarib, TransactionTypeCode, transaction.refrenceNo, transaction.metaData, transaction.IP);
                if(trans != null)
                {
                    if(TransactionTypeCode == (int) TransactionTypeCodeEnum.SeePost || TransactionTypeCode == (int)TransactionTypeCodeEnum.SeePostFree)
                    {                        
                        postCRUD.addPostToUserSeens(int.Parse(transaction.refrenceNo), IDUser);
                    }
                }

                response.transactionList = new List<Transaction>();
                response.appendTransaction(trans);

                response.wallet = new UserWallet();
                response.wallet.Amount = (long)walletCRUD.getWallet(IDUser).Amount + (long)Amount;

                string ssssssss = new JavaScriptSerializer().Serialize(response);
                return ssssssss;
            }
            else
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1;
                responsex.tubelessException.message = "you have not wallet";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
        }


        [HttpPost]
        [Route("WalletTransactionList")]
        public string WalletTransactionList(reqTransaction transaction)
        {
            int userId = userCRUD.getUserId(transaction.UserCode);
            List<Transaction> xxxxxxxxx = walletCRUD.getWalletTransactionList(transaction, userId);
            response.transactionList = xxxxxxxxx;

            //WalletCRUD userCRUD = new WalletCRUD();
            response.wallet = new UserWallet();
            response.wallet.Amount = (long)walletCRUD.getWallet(userId).Amount;
            string ssssssss = new JavaScriptSerializer().Serialize(response);
            return ssssssss;
        }
    }
}