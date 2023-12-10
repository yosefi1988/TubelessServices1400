using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Script.Serialization;
using TubelessServices.Models;
using TubelessServices.Models.Message;
using TubelessServices.Models.Response;

namespace TubelessServices.Controllers
{
    [RoutePrefix("api/Common")]

    public class CommonController : ApiController
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

        [HttpPost]
        [Route("ContactUs")]
        public string ContactUs(Message message)
        {
            try
            {
                Tbl_Message tbl_Common_Message = new Tbl_Message();
                tbl_Common_Message.ApplicationID = message.ApplicationId;

                if (message.SenderUserID == null)
                {
                    tbl_Common_Message.SenderUserID = 10012;
                }
                else
                {
                    tbl_Common_Message.SenderUserID = message.SenderUserID;
                }
                tbl_Common_Message.ReciverUserID = 10009;
                tbl_Common_Message.Title = message.title;
                tbl_Common_Message.Text = message.text;

                if (message.type == null)
                {
                    tbl_Common_Message.Type = 2042;
                }
                else
                {
                    tbl_Common_Message.Type = message.type;
                }

                tbl_Common_Message.Date = DateTime.Now;
                tbl_Common_Message.Readed = false;
                tbl_Common_Message.ShowForReciver = true;
                tbl_Common_Message.ShowForSender = true;
                tbl_Common_Message.MetaData = message.MetaData;
                db.Tbl_Messages.InsertOnSubmit(tbl_Common_Message);
                db.SubmitChanges();

                sendMail(message.title + " - " + message.text);

                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = 200;
                responsex.tubelessException.message = "ok";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
            catch (Exception ex)
            {
                ServerResponse responsex = new ServerResponse();
                responsex.tubelessException.code = -1;
                responsex.tubelessException.message = "error in create message";
                string ssssssss = new JavaScriptSerializer().Serialize(responsex);
                return ssssssss;
            }
        }



        public static void sendMail(String dgStudent)
        {
            string htmlString = dgStudent;//getHtml(dgStudent); //here you will be getting an html string  
            Email3(htmlString); //Pass html string to Email function.  
        }


        static String emailSenderAddress = "test@balabarkaran.com";// "yosefi1988";
        static String emailpassword = "sajjad1367";
        static String emailReciverAddress = "yosefi1988@gmail.com";
        public static void Email(string htmlString)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(emailSenderAddress + "@gmail.com");
                message.To.Add(new MailAddress(emailReciverAddress));
                message.Subject = "Contact Us";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                //smtp.UseDefaultCredentials = true;

                smtp.Credentials = new NetworkCredential(emailSenderAddress, emailpassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) {

                String sss = ex.Message;
                string dfs = "l";
            }
        }
        protected static void Email2(string htmlString)
        {
            using (MailMessage mm = new MailMessage(emailReciverAddress, emailReciverAddress))
            {
                mm.Subject = "Contact Us";
                mm.Body = htmlString;
                //if (fuAttachment.HasFile)
                //{
                //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                //}
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(emailReciverAddress, emailpassword);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                //ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);
            }
        }

          

        public static void Email3(string body)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(emailSenderAddress),
                    To = { emailReciverAddress },
                    Subject = "contact us",
                    Body = body,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                using (SmtpClient smtpClient = new SmtpClient("mail.balabarkaran.com"))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(emailSenderAddress, "bYv@83h0");
                    smtpClient.Port = 25;   //26    //587
                    smtpClient.EnableSsl = false;
                    smtpClient.Send(message);
                }
            }
            catch (Exception excep)
            {
                //ignore it or you can retry .

                int a = 5;
                a++;
            }


            //try
            //{
            //var fromAddress = new MailAddress("yosefi1988@gmail.com", "sajjad");
            //var toAddress = new MailAddress("yosefi1988@email.com", "Mr Test");
            //const string fromPassword = "sajjad1367";
            //const string subject = "test"; 

            //var smtp = new SmtpClient
            //{
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
            //    Timeout = 20000
            //};

            //using (var message = new MailMessage(fromAddress, toAddress)
            //{
            //    Subject = subject,
            //    Body = body
            //})
            //{
            //    smtp.Send(message);
            //}
            //}
            //catch (Exception ec)
            //{

            //    int a = 5;
            //    a++; 
            //}

        }
         
        private static string getHtml(String grid)
        {
            try
            {
                string messageBody = "<font>The following are the records: </font><br><br>";
                //if (grid.RowCount == 0) return messageBody;
                string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style=\"color:#555555;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
                string htmlTdEnd = "</td>";
                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                messageBody += htmlTdStart + "Student Name" + htmlTdEnd;
                messageBody += htmlTdStart + "DOB" + htmlTdEnd;
                messageBody += htmlTdStart + "Email" + htmlTdEnd;
                messageBody += htmlTdStart + "Mobile" + htmlTdEnd;
                messageBody += htmlHeaderRowEnd;
                //Loop all the rows from grid vew and added to html td  
                //for (int i = 0; i <= grid.RowCount - 1; i++)
                {
                    messageBody = messageBody + htmlTrStart;
                    //messageBody = messageBody + htmlTdStart + grid.Rows[i].Cells[0].Value + htmlTdEnd; //adding student name  
                    //messageBody = messageBody + htmlTdStart + grid.Rows[i].Cells[1].Value + htmlTdEnd; //adding DOB  
                    //messageBody = messageBody + htmlTdStart + grid.Rows[i].Cells[2].Value + htmlTdEnd; //adding Email  
                    //messageBody = messageBody + htmlTdStart + grid.Rows[i].Cells[3].Value + htmlTdEnd; //adding Mobile  

                    messageBody = messageBody + htmlTdStart + grid + htmlTdEnd; //adding Mobile  
                    messageBody = messageBody + htmlTrEnd;
                }
                messageBody = messageBody + htmlTableEnd;
                return messageBody; // return HTML Table as string from this function  
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}











