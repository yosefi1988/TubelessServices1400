using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models.Autenticator.Response;
using TubelessServices.Models.Post;
using TubelessServices.Models.Response;

namespace TubelessServices.Models.post.Response
{ 
    public class ResponsePostList : TransactionResponse
    {
        public List<PostItem> postList;
        

        //internal void appendTransaction( trans)
        //{
        //    postList = new List<PostItem>();
        //    Transaction newTransaction = new Transaction();
        //    newTransaction.Amount = (long)trans.Amount;

        //    //todo persian dat0e
        //    newTransaction.DateTime = trans.CreatedOn + "";
        //    newTransaction.RefrenceNo = trans.RefrenceNo;
        //    newTransaction.Zarib = (float)trans.Zarib;
        //    postList.Add(newTransaction);
        //}


    }
}