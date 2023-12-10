using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Classes.Utility;
using TubelessServices.Controllers.Autenticator;
using TubelessServices.Controllers.Wallet;
using TubelessServices.Models.Response;

namespace TubelessServices.Models.Autenticator.Response
{
    public class TransactionResponse : ServerResponse
    {
        public TransactionResponse()
        {
            tubelessException.code = 200;
            tubelessException.message = "ok";        
        }
        public UserWallet wallet;
        public List<Transaction> transactionList;

        internal void appendTransaction(Tbl_WalletTransaction trans)
        {
            transactionList = new List<Transaction>();
            Transaction newTransaction = new Transaction();
            newTransaction.Amount = (long) trans.Amount;

            try
            {
                newTransaction.DateTime = Date.convertToPersianDate(trans.CreatedOn);
                newTransaction.Zarib = (float)trans.Zarib;
            }
            catch
            {
                newTransaction.DateTime = null;
            }
            newTransaction.RefrenceNo = trans.RefrenceNo;
            transactionList.Add(newTransaction);
        }
        
    }
}