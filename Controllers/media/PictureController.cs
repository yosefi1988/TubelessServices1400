using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using TubelessServices.Models;
using TubelessServices.Models.Response;

namespace TubelessServices.Controllers.media
{
    [RoutePrefix("api/Upload")]

    public class PictureController : ApiController
    {
        DataClassesAutenticatorDataContext db = new DataClassesAutenticatorDataContext();
        //https://www.c-sharpcorner.com/article/uploading-image-to-server-using-web-api-2-0/

        PictureCRUD pictureCRUD = new PictureCRUD();


        [Route("blog/addBlogPicture")]
        [AllowAnonymous]
        public async Task<String> addBlogPicture()
        {
            ServerResponse responsex = new ServerResponse();

            try
            {
                var httpRequest = HttpContext.Current.Request;

                var BlogId = httpRequest.Form.Get("BlogId");

                int index = 0;
                foreach (string file in httpRequest.Files)
                {
                    index++;
                    var TitlePicture = httpRequest.Files.Get("TitlePicture");
                    if (file.ToLower() == "titlepicture")
                    {
                        #region textpicture
                        var postedFileTitlePicture = httpRequest.Files.Get("TitlePicture");
                        if (postedFileTitlePicture != null && postedFileTitlePicture.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  
                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                            var ext = postedFileTitlePicture.FileName.Substring(postedFileTitlePicture.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                return response(-121,"Please Upload image of type .jpg,.png.");
                            }
                            else if (postedFileTitlePicture.ContentLength > MaxContentLength)
                            {
                                return response(-122,"Please Upload a file upto 2 mb.");
                            }
                            else
                            {
                                var filePath = HttpContext.Current.Server.MapPath("~/Images/Blog/" + DateTime.Now.Year + "/" +
                                                                BlogId + "_" +
                                                                postedFileTitlePicture.FileName + "_" +
                                                                DateTime.Now.Year +
                                                                DateTime.Now.Month +
                                                                DateTime.Now.Day +
                                                                DateTime.Now.Hour +
                                                                DateTime.Now.Minute +
                                                                DateTime.Now.Second +
                                                                DateTime.Now.Millisecond +
                                                                extension);
                                //if (!Directory.Exists("~/Images/Blog/" + DateTime.Now.Year + "/"))
                                //{
                                //    DirectoryInfo di = Directory.CreateDirectory("~/Images/Blog/" + DateTime.Now.Year + "/");
                                //}

                                try
                                {
                                    postedFileTitlePicture.SaveAs(filePath);

                                //    if (extension == ".jpg" || extension == ".png")
                                //    {
                                //        //save low quality of pic
                                //        //using (Bitmap bitmap = (Bitmap)Image.FromFile("file.jpg"))
                                //        using (Bitmap bitmap = (Bitmap)Image.FromFile(filePath))
                                //        {
                                //            using (Bitmap newBitmap = new Bitmap(bitmap))
                                //            {
                                //                //if (!Directory.Exists(filePath.Replace("\\Images\\Blog\\", "\\Images\\Blog\\Thumbnail\\")))
                                //                //{ 
                                //                //    DirectoryInfo di = Directory.CreateDirectory(filePath.Replace("\\Images\\Blog\\", "\\Images\\Blog\\Thumbnail\\"));
                                //                //}

                                //                newBitmap.SetResolution(80, 100);
                                //                if (extension == ".jpg")
                                //                {
                                //                    newBitmap.Save(filePath.Replace("\\Images\\Blog\\", "\\Images\\Blog\\Thumbnail\\"), ImageFormat.Jpeg);
                                //                }
                                //                else if (extension == ".png")
                                //                {
                                //                    newBitmap.Save(filePath.Replace("\\Images\\Blog\\", "\\Images\\Blog\\Thumbnail\\"), ImageFormat.Png);
                                //                }
                                //            }
                                //        }
                                //    }
                                }
                                catch (Exception ex)
                                {
                                    return response(-128,"cannot save Blog title Picture file :" + ex.Message);
                                }

                                if (pictureCRUD.UpdateBlogTitlePicture(int.Parse(BlogId), createUrl(filePath, "Blog")))
                                {
                                    continue;
                                }
                                else
                                {
                                    return response(-129,"cannot update blog Title picture");
                                }
                            }
                        }
                        #endregion
                    }

                    var TextPicture = httpRequest.Form.Get("TextPicture");
                    if (file.ToLower() == "textpicture")
                    {
                        #region textpicture
                        var postedFileTextPicture = httpRequest.Files.Get("TextPicture");
                        if (postedFileTextPicture != null && postedFileTextPicture.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  
                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                            var ext = postedFileTextPicture.FileName.Substring(postedFileTextPicture.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                return response(-121,"Please Upload image of type .jpg,.png.");
                            }
                            else if (postedFileTextPicture.ContentLength > MaxContentLength)
                            {
                                return response(-122,"Please Upload a file upto 2 mb.");
                            }
                            else
                            {
                                var filePath = HttpContext.Current.Server.MapPath("~/Images/Blog/" + DateTime.Now.Year + "/" +
                                                                BlogId + "_" +
                                                                postedFileTextPicture.FileName + "_" +
                                                                DateTime.Now.Year +
                                                                DateTime.Now.Month +
                                                                DateTime.Now.Day +
                                                                DateTime.Now.Hour +
                                                                DateTime.Now.Minute +
                                                                DateTime.Now.Second +
                                                                DateTime.Now.Millisecond +
                                                                extension);
                                try
                                {
                                    postedFileTextPicture.SaveAs(filePath);
                                }
                                catch (Exception ex)
                                {
                                    return response(-128,"cannot save Blog TextPicture file :" + ex.Message); 
                                }

                                if (pictureCRUD.UpdateBlogTextPicture(int.Parse(BlogId), createUrl(filePath, "Blog")))
                                {
                                    continue;
                                }
                                else
                                {
                                    return response(-129,"cannot update blog TextPicture"); 
                                }
                            }
                        }
                        #endregion
                    }

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        #region ImageContent
                        int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            return response(-121,"Please Upload image of type .jpg,.png.");
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            return response(-122,"Please Upload a file upto 2 mb.");
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Images/Blog/" + DateTime.Now.Year + "/" +
                                postedFile.FileName +
                                DateTime.Now.Year +
                                DateTime.Now.Month +
                                DateTime.Now.Day +
                                DateTime.Now.Hour +
                                DateTime.Now.Minute +
                                DateTime.Now.Second +
                                DateTime.Now.Millisecond +
                                extension);

                            try
                            {
                                postedFile.SaveAs(filePath);
                            }
                            catch (Exception ex)
                            {
                                return response(-127,"Cannot save Blog image file :" + ex.Message);
                            }


                            if (pictureCRUD.InsertNewBlogPictures(int.Parse(BlogId), createUrl(filePath, "Blog")))
                            {
                                if (index == httpRequest.Files.Count)
                                {
                                    return response(120,"Update Blog images");
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                responsex.tubelessException.code = -126;
                                responsex.tubelessException.message = "Cannot Insert Blog pictures";
                            }

                        }
                        #endregion
                    }
                    return response(-124,"Image Updated error.");
                }
                if (httpRequest.Files.Count == 0)
                {
                    return response(-123,"Please Upload a image.");
                }
                else
                {
                    return response(120,"Update images");
                }
            }
            catch (Exception ex)
            {
                return response(-125, "Blog Image Update some Error : " + ex.Message);
            }

        }

        internal String response(int code , String text)
        {
            ServerResponse responsex = new ServerResponse();
            responsex.tubelessException.code = code;
            responsex.tubelessException.message = text;
            string ssssssss = new JavaScriptSerializer().Serialize(responsex);
            return ssssssss;
        }

        private string createUrl(string filePath, String folder)
        {
            string apPath2 = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            String final = filePath.Substring(apPath2.Length - 1, (filePath.Length - apPath2.Length) + 1);
            String uriString = "http://" + HttpContext.Current.Request.Url.Host;

            //uriString += "/Project/ImageBrowser.aspx";
            //Uri uri = new Uri(uriString);
            //String url = uri.AbsoluteUri;



            //String ssssssssss = "apPath:"+ apPath + "XXXXXXX filePath:" + filePath + "XXXXXXX apPath2:" + apPath2 + "url:" + url;
            return uriString + final;
        }

        [Route("user/UserAvatarProfileImage")]
        [AllowAnonymous]
        public async Task<String> UserAvatarProfileImage()
        {
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            //dict.Add("error", message);
            //return Request.CreateResponse(HttpStatusCode.BadRequest, dict);

            ServerResponse response = new ServerResponse();

            try
            {
                var httpRequest = HttpContext.Current.Request;

                //Save File
                {

                    //var postedFile = httpRequest.Files[file];
                    var postedFile = httpRequest.Files.Get("Avatar");
                    var UserId = httpRequest.Form.Get("UserId");
                    var Type = httpRequest.Form.Get("Type");

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            response.tubelessException.code = -121;
                            response.tubelessException.message = "Please Upload image of type .jpg,.png.";
                            return new JavaScriptSerializer().Serialize(response);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            response.tubelessException.code = -122;
                            response.tubelessException.message = "Please Upload a file upto 2 mb.";
                            return new JavaScriptSerializer().Serialize(response);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Images/Avatar/" + DateTime.Now.Year + "/" +
                                UserId + "_" +
                                postedFile.FileName + "_" +
                                DateTime.Now.Year +
                                DateTime.Now.Month +
                                DateTime.Now.Day +
                                DateTime.Now.Hour +
                                DateTime.Now.Minute +
                                DateTime.Now.Second +
                                DateTime.Now.Millisecond +
                                extension);

                            try
                            {
                                postedFile.SaveAs(filePath);
                            }
                            catch (Exception ex)
                            {
                                response.tubelessException.code = -120;
                                response.tubelessException.message = "cannot save Profile or Avatar file :" + ex.Message;
                                return new JavaScriptSerializer().Serialize(response);
                            }

                            if (Type.ToLower() == "avatar")
                            {
                                if (pictureCRUD.UpdateNewAvatar(int.Parse(UserId), createUrl(filePath, "Avatar")))
                                {
                                    response.tubelessException.code = 118;
                                    response.tubelessException.message = "Update New Avatar for user";
                                    return new JavaScriptSerializer().Serialize(response);
                                }
                                else
                                {
                                    response.tubelessException.code = -118;
                                    response.tubelessException.message = "cannot update user Avatar";
                                    return new JavaScriptSerializer().Serialize(response);
                                }
                            }
                            else if (Type.ToLower() == "profile")
                            {
                                if (pictureCRUD.UpdateNewProfileImage(int.Parse(UserId), createUrl(filePath, "Avatar")))
                                {
                                    response.tubelessException.code = 119;
                                    response.tubelessException.message = "Update User Profile image";
                                    return new JavaScriptSerializer().Serialize(response);
                                }
                                else
                                {
                                    response.tubelessException.code = -119;
                                    response.tubelessException.message = "cannot update user profile";
                                    return new JavaScriptSerializer().Serialize(response);
                                }
                            }

                        }
                    }


                    response.tubelessException.code = -124;
                    response.tubelessException.message = "Image Updated error.";
                    return new JavaScriptSerializer().Serialize(response);
                }

                response.tubelessException.code = -123;
                response.tubelessException.message = "Please Upload a image.";
                return new JavaScriptSerializer().Serialize(response);
            }
            catch (Exception ex)
            {
                response.tubelessException.code = -124;
                response.tubelessException.message = "Image Updated some Error : " + ex.Message;
                return new JavaScriptSerializer().Serialize(response);
            }
        }

        [AcceptVerbs("GET")]
        [Route("blog/deleteBlogPic")]
        public string deleteBlogPic(int bid, int bpid, string userId, string type)
        {
            return "";
            //    if (bpid > 0 && userId.Length > 1)
            //    {
            //        Tbl_Common_Blog_Picture existing = db.Tbl_Common_Blog_Pictures.Single(p => p.Id == bpid);
            //        Tbl_Common_Blog existingBlog = db.Tbl_Common_Blogs.Single(p => p.ID == bid);

            //        if (TubelessUtil.isAdmin(userId))
            //        {
            //            try
            //            {
            //                if (type.ToLower() == "album")
            //                {
            //                    if (existing != null)
            //                    {
            //                        existing.isDisabled = true;
            //                    }
            //                    db.SubmitChanges();
            //                }
            //                if (type.ToLower() == "textpic")
            //                {
            //                    if (existingBlog != null)
            //                    {
            //                        existingBlog.TextPicture = null;
            //                    }
            //                    db.SubmitChanges();
            //                }
            //                if (type.ToLower() == "titlepic")
            //                {
            //                    if (existingBlog != null)
            //                    {
            //                        existingBlog.TitlePicture = null;
            //                    }
            //                    db.SubmitChanges();
            //                }



            //                ServerResponse response = new ServerResponse();
            //                response.tubelessException.code = 121;
            //                response.tubelessException.message = "delete post picture ok";
            //                return new JavaScriptSerializer().Serialize(response);
            //            }
            //            catch (Exception ex)
            //            {
            //                ServerResponse response = new ServerResponse();
            //                response.tubelessException.code = -130;
            //                response.tubelessException.message = "error on delete post picture from databse: " + ex.Message;
            //                return new JavaScriptSerializer().Serialize(response);
            //            }
            //        }
            //        if (userId.ToString().Equals(existingBlog.UserID.ToString()))
            //        {
            //            if (type.ToLower() == "album")
            //            {
            //                //var data = db.Tbl_Common_Blogs.SingleOrDefault(p => p.ID == id);
            //                if (existing != null)
            //                {
            //                    existing.isDeleted = true;
            //                }
            //                db.SubmitChanges();
            //            }
            //            if (type.ToLower() == "textpic")
            //            {
            //                if (existingBlog != null)
            //                {
            //                    existingBlog.TextPicture = null;
            //                }
            //                db.SubmitChanges();
            //            }
            //            if (type.ToLower() == "titlepic")
            //            {
            //                if (existingBlog != null)
            //                {
            //                    existingBlog.TitlePicture = null;
            //                }
            //                db.SubmitChanges();
            //            }

            //            ServerResponse response = new ServerResponse();
            //            response.tubelessException.code = 121;
            //            response.tubelessException.message = "delete post picture ok";
            //            return new JavaScriptSerializer().Serialize(response);
            //        }
            //        else
            //        {
            //            ServerResponse response = new ServerResponse();
            //            response.tubelessException.code = -131;
            //            response.tubelessException.message = "ridi dadash";
            //            return new JavaScriptSerializer().Serialize(response);
            //        }

            //    }
            //    else
            //    {
            //        ServerResponse response = new ServerResponse();
            //        response.tubelessException.code = -131;
            //        response.tubelessException.message = "ridi dadash";
            //        return new JavaScriptSerializer().Serialize(response);
            //    }
            //}

        }
    }
}

