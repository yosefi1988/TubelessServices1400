using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models;

namespace TubelessServices.Controllers.media
{
    public class PictureCRUD
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();

        public bool InsertNewBlogPictures(int blogId, String imageUrl)
        {
            try
            {
                Tbl_Post_Image blogPic = new Tbl_Post_Image();
                blogPic.PostId = blogId;
                blogPic.Picture = imageUrl;
                blogPic.isDeleted = false;
                blogPic.isDisabled = false;
                blogPic.CreatedOn = DateTime.Now;

                db.Tbl_Post_Images.InsertOnSubmit(blogPic);
                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                string sss =  ex.Message;
                return false;
            }
        }
        public bool UpdateBlogTextPicture(int blogId, String imageUrl)
        {
            try
            {
                Tbl_Post result = (from p in db.Tbl_Posts
                                   where p.Id == blogId
                                          select p).SingleOrDefault();

                result.TextPicture = imageUrl;
                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateBlogTitlePicture(int blogId, String imageUrl)
        {
            try
            {
                Tbl_Post result = (from p in db.Tbl_Posts
                                   where p.Id == blogId
                                          select p).SingleOrDefault();

                result.TitlePicture = imageUrl;
                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateNewProfileImage(int userId, String imageUrl)
        {
            try
            {
                //Tbl_Common_User result = (from p in db.Tbl_Common_Users
                //                          where p.ID == userId
                //                          select p).SingleOrDefault();

                //result.ProfileImage = imageUrl;
                //db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateNewAvatar(int userId, String imageUrl)
        {
            try
            {
                //Tbl_Common_User result = (from p in db.Tbl_Common_Users
                //                          where p.ID == userId
                //                          select p).SingleOrDefault();

                //result.UserImage = imageUrl;
                //db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }       
}