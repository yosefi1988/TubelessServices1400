
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
using TubelessServices.Classes.Utility;

namespace TubelessServices.Controllers.Wallet
{
    public class WalletCRUD
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        UserCRUD userCRUD = new UserCRUD();


        public Tbl_WalletTransaction registerNewTransaction(
            int walletId, 
            int newUserId, 
            int IDUserTransactionCreator, 
            int IdApp, 
            float Amount, 
            float zarib, 
            int TransactionTypeCode, 
            String RefrenceNo, 
            String MetaData, 
            String ip,
            bool isWaletTransaction)
        {
            try
            {
                Tbl_WalletTransaction transaction = new Tbl_WalletTransaction();
                if (Amount == 0)
                    return transaction;

                transaction.IDUser = newUserId;
                transaction.IDUserCreator = IDUserTransactionCreator;
                transaction.IDApplication = IdApp;
                transaction.Amount = (decimal)Amount;
                transaction.Zarib = zarib;
                transaction.TransactionTypeCode = TransactionTypeCode;
                transaction.RefrenceNo = RefrenceNo;
                transaction.MetaData = MetaData;
                transaction.TransactionTypeCode = TransactionTypeCode;
                transaction.CreatedOn = DateTime.Now;
                transaction.isWalletTransaction = isWaletTransaction;
                transaction.IP = ip;

                db.Tbl_WalletTransactions.InsertOnSubmit(transaction);
                db.SubmitChanges();
                return transaction;
            }
            catch
            {
                return null;
            }
        }

        internal int getWalletByUserCode(string userCode)
        {
            return getWallet(userCRUD.findUserByUserCode(userCode).FirstOrDefault()).Id;
        }

        internal int getWalletByUserCode(int userCode)
        {
            return getWallet(userCRUD.findUserByUserCode(userCode).FirstOrDefault()).Id;
        }

        internal float getZarib(string userCode, int TransactionTypeCode)
        {
            return getZarib(int.Parse(userCode), TransactionTypeCode);
        }
        internal float getZarib(int? userCode, int transactionTypeCode)
        {
            //todo fix        
            //return 2.5f;
            return 1f;
        }

        public int CreateNewWallet(Login loginAccount, int userId)
        {
            TubelessServices.Models.Tbl_Wallet wallet = new TubelessServices.Models.Tbl_Wallet();
            wallet.IDUser = userId;
            wallet.ModifiedOn = DateTime.Now;

            db.Tbl_Wallets.InsertOnSubmit(wallet);
            db.SubmitChanges();
            return wallet.Id;
        }

        public Models.Tbl_Wallet getWallet(int userId)
        {
            Models.Tbl_Wallet userWallet = (from x in db.Tbl_Wallets
                                            where x.IDUser == userId
                                        select x).ToList().FirstOrDefault();

            return userWallet;
        }
        public Models.Tbl_Wallet getWallet(Tbl_User user)
        {
            return getWallet(user.Id);
        }

        internal List<Transaction> getWalletTransactionList(reqTransaction transaction,int userId)
        {            
            IEnumerable<Models.Viw_WalletTransaction2> transactionList1 = (from x in db.Viw_WalletTransaction2s
                                                                           where x.IDUser == userId
                                                                           select x).OrderByDescending(date => date.CreatedOn).ToList();

            IEnumerable<Models.Viw_WalletTransaction2> finalList =
            checkDateTo(
                checkDateFrom(
                    checkDate(
                        checkTransactionTypeCode(
                            checkMaxAmount(
                                checkMinAmount(
                                    checkAmount(
                                        checkAppID(transactionList1, transaction),
                                    transaction),
                                transaction),
                            transaction),
                        transaction),
                    transaction),
                transaction),
            transaction);

            IEnumerable<Models.Viw_WalletTransaction2> transactionList2 = finalList.Skip(transaction.pageIndex * transaction.pageSize);
            IEnumerable<Models.Viw_WalletTransaction2> transactionList3 = transactionList2.Take(transaction.pageSize);

            return preparelist(transactionList3);
        }

        private IEnumerable<Viw_WalletTransaction2> checkDateTo(IEnumerable<Viw_WalletTransaction2> transactionList1, reqTransaction transaction)
        {
            if (transaction.DateTo != null)
            {
                IEnumerable<Models.Viw_WalletTransaction2> listttc = transactionList1.Where(x => x.CreatedOn <= DateTime.Parse(transaction.DateTo));
                return listttc;
            }
            else
            {
                return transactionList1;
            }
        }

        private IEnumerable<Viw_WalletTransaction2> checkDateFrom(IEnumerable<Viw_WalletTransaction2> transactionList1, reqTransaction transaction)
        {
            if (transaction.DateFrom != null)
            {                
                IEnumerable<Models.Viw_WalletTransaction2> listttc = transactionList1.Where(x => x.CreatedOn >= DateTime.Parse(transaction.DateFrom));
                return listttc;
            }
            else
            {
                return transactionList1;
            }
        }

        private IEnumerable<Viw_WalletTransaction2> checkDate(IEnumerable<Viw_WalletTransaction2> transactionList1, reqTransaction transaction)
        {
            if (transaction.Date != null)
            {
                //todo remove houre minute secound from Date
                IEnumerable<Models.Viw_WalletTransaction2> listttc = transactionList1.Where(x => x.CreatedOn == DateTime.Parse(transaction.Date));
                return listttc;
            }
            else
            {
                return transactionList1;
            }
        }

        private IEnumerable<Viw_WalletTransaction2> checkTransactionTypeCode(IEnumerable<Viw_WalletTransaction2> transactionList1, reqTransaction transaction)
        {
            if (transaction.ttc != 0)
            {
                IEnumerable<Models.Viw_WalletTransaction2> listttc = transactionList1.Where(x => x.TransactionTypeCode == transaction.ttc);
                return listttc;
            }
            else
            {
                return transactionList1;
            }
        }

        private IEnumerable<Viw_WalletTransaction2> checkMaxAmount(IEnumerable<Viw_WalletTransaction2> transactionList1, reqTransaction transaction)
        {
            if (transaction.AmountMax != null)
            {
                IEnumerable<Models.Viw_WalletTransaction2> listAmount = transactionList1.Where(x => x.Amount <= Decimal.Parse(transaction.AmountMax));
                return listAmount;
            }
            else
            {
                return transactionList1;
            }
        }

        private IEnumerable<Viw_WalletTransaction2> checkMinAmount(IEnumerable<Viw_WalletTransaction2> transactionList1, reqTransaction transaction)
        {
            if (transaction.AmountMin != null)
            {
                IEnumerable<Models.Viw_WalletTransaction2> listAmount = transactionList1.Where(x => x.Amount >= Decimal.Parse(transaction.AmountMin));
                return listAmount;
            }
            else
            {
                return transactionList1;
            }
        }

        private IEnumerable<Viw_WalletTransaction2> checkAmount(IEnumerable<Viw_WalletTransaction2> transactionList1, reqTransaction transaction)
        {
            if (transaction.Amount != null)
            {
                IEnumerable<Models.Viw_WalletTransaction2> listAmount = transactionList1.Where(x => x.Amount == Decimal.Parse(transaction.Amount));
                return listAmount;
            }
            else
            {
                return transactionList1;
            }
        }

        private IEnumerable<Viw_WalletTransaction2> checkAppID(IEnumerable<Viw_WalletTransaction2> transactionList1, reqTransaction transaction)
        {
            if (transaction.IDApplication != 0)
            {
                IEnumerable<Models.Viw_WalletTransaction2> listAppID = transactionList1.Where(x => x.IDApplication == transaction.IDApplication);
                return listAppID;
            }
            else
            {
                return transactionList1;
            }
        }

        private List<Transaction> preparelist(IEnumerable<Viw_WalletTransaction2> transactionList3)
        {
            List<Transaction> transactionListOut = new List<Transaction>();
            foreach (Models.Viw_WalletTransaction2 transaction in transactionList3)
            {
                Transaction transactionItem = new Transaction();
                transactionItem.Amount = (long)transaction.Amount;
                transactionItem.CreatorFullName = transaction.TransactionCreatorName + " " + transaction.TransactionCreatorFamily;
                transactionItem.TTC = transaction.TransactionTypeCode;
                transactionItem.TTN = transaction.TransactionTypeName;
                transactionItem.image = transaction.ImageUrl;
                transactionItem.icon = transaction.icon;
                //transactionItem.Zarib = transaction.;

                if (transactionItem.TTC != 5)            //5 = شارژ کیف پول
                    transactionItem.title = transaction.PostTitle;
                
                transactionItem.DateTime = Date.convertToPersianDate(transaction.CreatedOn);

                //if (transaction.TransactionTypeCode == (int)WalletController.TransactionTypeCodeEnum.Wall etSharje)
                transactionItem.RefrenceNo = transaction.RefrenceNo;
                transactionItem.IdPost = transaction.IDPost.ToString();
                transactionItem.ID = transaction.TransactionID;

                transactionListOut.Add(transactionItem);
            }

            return transactionListOut;
        }
    }
}