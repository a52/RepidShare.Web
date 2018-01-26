using RepidShare.Entities;
using RepidShare.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RepidShare.Admin.Controllers
{
    public class CoupenController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit Coupen
        /// <summary>
        /// Add or Edit Coupen for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveCoupen(string prm = "")
        {
            CoupenModel objCoupenModel = new CoupenModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int CoupenId;
                    //decrypt parameter and set in CoupenId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out CoupenId);
                    //Get Coupen detail by  Coupen Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Coupen + "/GetCoupenById?CoupenId=" + CoupenId.ToString());
                    objCoupenModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<CoupenModel>().Result : null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Coupen", "SaveCoupen Get");
            }

            return View("SaveCoupen", objCoupenModel);
        }



        /// <summary>
        /// Add or Edit  Coupen based on CoupenID
        /// </summary>
        /// <param name="objCoupenModel"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Filters.Authorized]
        public ActionResult SaveCoupen(CoupenModel objCoupenModel)
        {
            try
            {

                objCoupenModel.IsActive = true;
                objCoupenModel.CreatedBy = LoggedInUserID;

                //Insert or Update  Coupen
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Coupen + "/InsertUpdateCoupen", objCoupenModel);
                objCoupenModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<CoupenModel>().Result : null;

                //if error code is 0 means  Coupen saved successfully
                if (Convert.ToInt32(objCoupenModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "Coupen Saved Successfully";
                    return RedirectToAction("ViewCoupen", "Coupen");
                }
                else if (Convert.ToInt32(objCoupenModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means Coupen Name is duplicate set duplicate Coupen error message.
                    objCoupenModel.Message = "Coupen Duplicate not allowed";
                    objCoupenModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objCoupenModel.Message = "Error while adding record";
                    objCoupenModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Coupen", "SaveCoupen POST");
            }
            return View("SaveCoupen", objCoupenModel);
        }

        #endregion

        #region View  Coupen
        /// <summary>
        /// View  Coupen List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewCoupen()
        {
            ViewCoupenModel ObjViewCoupenModel = new ViewCoupenModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewCoupenModel.CurrentPage = 1;
                ObjViewCoupenModel.PageSize = CommonUtils.PageSize;
                ObjViewCoupenModel.TotalPages = 0;
                //Get  Coupen List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Coupen + "/GetCoupenList", ObjViewCoupenModel);
                ObjViewCoupenModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewCoupenModel>().Result : null;
                //ObjViewCoupenModel = objBLCoupen.GetCoupenList(ObjViewCoupenModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewCoupenModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewCoupenModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Coupen", "View GET");
            }
            return View("ViewCoupen", ObjViewCoupenModel);
        }

        /// <summary>
        /// View  Coupen List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewCoupenModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewCoupen(ViewCoupenModel objViewCoupenModel)
        {
            try
            {
                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewCoupenModel.Message = objViewCoupenModel.MessageType = String.Empty;

                if (objViewCoupenModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Coupen + "/DeleteCoupen", objViewCoupenModel);
                    objViewCoupenModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewCoupenModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewCoupenModel.Message = "Coupen Deleted Successfully";
                        objViewCoupenModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewCoupenModel.Message = "Error while deleting record";
                        objViewCoupenModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Coupen List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Coupen + "/GetCoupenList", objViewCoupenModel);
                objViewCoupenModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewCoupenModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Coupen", "View POST");
            }
            return PartialView("_CoupenList", objViewCoupenModel);
        }

        #endregion

    }
}
