
using RepidShare.Entities;
using RepidShare.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace RepidShare.Admin.Controllers
{
    public class BulletinController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit Bulletin
        /// <summary>
        /// Add or Edit Bulletin for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveBulletin(string prm = "")
        {
            BulletinModel objBulletinModel = new BulletinModel();

            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int BulletinId;
                    //decrypt parameter and set in BulletinId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out BulletinId);
                    //Get Bulletin detail by  Bulletin Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Bulletin + "/GetBulletinById?BulletinId=" + BulletinId.ToString());
                    objBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<BulletinModel>().Result : null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Bulletin", "SaveBulletin Get");
            }

            return View("SaveBulletin", objBulletinModel);
        }



        /// <summary>
        /// Add or Edit  Bulletin based on BulletinID
        /// </summary>
        /// <param name="objBulletinModel"></param>
        /// <returns></returns>


        [ValidateAntiForgeryToken()]
        [Filters.Authorized]
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveBulletin(BulletinModel objBulletinModel, HttpPostedFileBase fileUpload)
        {
            try
            {
                if (fileUpload != null)
                {

                    string fileName = string.Empty;
                    string destinationPath = string.Empty;
                    fileName = Path.GetFileName(fileUpload.FileName);
                    string AttachmentType = Path.GetExtension(fileUpload.FileName);
                    Stream fs = fileUpload.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string ImageContent = Convert.ToBase64String(bytes);
                    objBulletinModel.AttachmentContent = bytes;
                    objBulletinModel.AttachmentName = fileName;
                    objBulletinModel.AttachmentSize = fs.Length;
                    objBulletinModel.AttachmentType = AttachmentType;
                }


                objBulletinModel.IsActive = true;
                objBulletinModel.CreatedBy = LoggedInUserID;
                ;
                //Insert or Update  Bulletin
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Bulletin + "/InsertUpdateBulletin", objBulletinModel);
                objBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<BulletinModel>().Result : null;

                //if error code is 0 means  Bulletin saved successfully
                if (Convert.ToInt32(objBulletinModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "Bulletin Saved Successfully";
                    return RedirectToAction("ViewBulletin", "Bulletin");
                }
                else if (Convert.ToInt32(objBulletinModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means Bulletin Name is duplicate set duplicate Bulletin error message.
                    objBulletinModel.Message = "Bulletin Duplicate not allowed";
                    objBulletinModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objBulletinModel.Message = "Error while adding record";
                    objBulletinModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Bulletin", "SaveBulletin POST");
            }
            return View("SaveBulletin", objBulletinModel);
        }

        #endregion

        #region View  Bulletin
        /// <summary>
        /// View  Bulletin List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewBulletin()
        {
            ViewBulletinModel ObjViewBulletinModel = new ViewBulletinModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewBulletinModel.CurrentPage = 1;
                ObjViewBulletinModel.PageSize = CommonUtils.PageSize;
                ObjViewBulletinModel.TotalPages = 0;

                //Get  Bulletin List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Bulletin + "/GetBulletinList", ObjViewBulletinModel);

                ObjViewBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewBulletinModel>().Result : null;
                //ObjViewBulletinModel = objBLBulletin.GetBulletinList(ObjViewBulletinModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewBulletinModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewBulletinModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Bulletin", "View GET");
            }
            return View("ViewBulletin", ObjViewBulletinModel);
        }

        /// <summary>
        /// View  Bulletin List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewBulletinModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewBulletin(ViewBulletinModel objViewBulletinModel)
        {
            try
            {

                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewBulletinModel.Message = objViewBulletinModel.MessageType = String.Empty;

                if (objViewBulletinModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Bulletin + "/DeleteBulletin", objViewBulletinModel);
                    objViewBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewBulletinModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewBulletinModel.Message = "Bulletin Deleted Successfully";
                        objViewBulletinModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewBulletinModel.Message = "Error while deleting record";
                        objViewBulletinModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Bulletin List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Bulletin + "/GetBulletinList", objViewBulletinModel);
                objViewBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewBulletinModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Bulletin", "View POST");
            }
            return PartialView("_BulletinList", objViewBulletinModel);
        }


        [Filters.Authorized]
        public ActionResult ActivateDeactivateBulletin(string prm = "")
        {
            BulletinModel objBulletinModel = new BulletinModel();

            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int BulletinId;
                    int Status;
                    //decrypt parameter and set in BulletinId variable
                    int.TryParse(CommonUtils.Decrypt(prm.Split('~')[0]), out BulletinId);
                    int.TryParse(prm.Split('~')[1], out Status);
                    //Get Bulletin detail by  Bulletin Id


                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Bulletin + "/UpdateBulletinStatusByID?BulletinId=" + BulletinId.ToString() + "&status=" + Status);
                    objBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<BulletinModel>().Result : null;

                    //serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetUserListById?UserId=" + UserId.ToString());
                    //objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;
                    //if (objUserLogin != null)
                    //{

                    //    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/UpdateUserStatusByID?UserId=" + BulletinId.ToString() + "&status=" + Status);
                    //    objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;


                    //    //Admin_UpdateUserStatusByID

                    //}


                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Bulletin", "ViewBulletin");
            }

            return RedirectToAction("ViewBulletin");
        }

        #endregion

        
    }
}
