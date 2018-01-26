
using RepidShare.Entities;
using RepidShare.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;


namespace RepidShare.Admin.Controllers
{
    public class DropDownController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();
        
        #region Add Edit DropDown
        /// <summary>
        /// Add or Edit DropDown for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveDropDown(string prm = "")
        {
            DropDownModel objDropDownModel = new DropDownModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int DropDownId;
                    //decrypt parameter and set in DropDownId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out DropDownId);
                    //Get DropDown detail by  DropDown Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.DropDown + "/GetDropDownById?DropDownId=" + DropDownId.ToString());
                    objDropDownModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DropDownModel>().Result : null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "DropDown", "SaveDropDown Get");
            }

            return View("SaveDropDown", objDropDownModel);
        }



        /// <summary>
        /// Add or Edit  DropDown based on DropDownID
        /// </summary>
        /// <param name="objDropDownModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Filters.Authorized]
        public ActionResult SaveDropDown(DropDownModel objDropDownModel)
        {
            try
            {

                objDropDownModel.IsActive = true;
                objDropDownModel.CreatedBy = LoggedInUserID;

                //Insert or Update  DropDown
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DropDown + "/InsertUpdateDropDown", objDropDownModel);
                objDropDownModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DropDownModel>().Result : null;

                //if error code is 0 means  DropDown saved successfully
                if (Convert.ToInt32(objDropDownModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "DropDown Saved Successfully";
                    return RedirectToAction("ViewDropDown", "DropDown");
                }
                else if (Convert.ToInt32(objDropDownModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means DropDown Name is duplicate set duplicate DropDown error message.
                    objDropDownModel.Message = "DropDown Duplicate not allowed";
                    objDropDownModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objDropDownModel.Message = "Error while adding record";
                    objDropDownModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "DropDown", "SaveDropDown POST");
            }
            return View("SaveDropDown", objDropDownModel);
        }

        #endregion

        #region View  DropDown
        /// <summary>
        /// View  DropDown List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewDropDown()
        {
            ViewDropDownModel ObjViewDropDownModel = new ViewDropDownModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewDropDownModel.CurrentPage = 1;
                ObjViewDropDownModel.PageSize = CommonUtils.PageSize;
                ObjViewDropDownModel.TotalPages = 0;

                //Get  DropDown List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DropDown + "/GetDropDownList", ObjViewDropDownModel);

                ObjViewDropDownModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDropDownModel>().Result : null;
                //ObjViewDropDownModel = objBLDropDown.GetDropDownList(ObjViewDropDownModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewDropDownModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewDropDownModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "DropDown", "View GET");
            }
            return View("ViewDropDown", ObjViewDropDownModel);
        }

        /// <summary>
        /// View  DropDown List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewDropDownModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewDropDown(ViewDropDownModel objViewDropDownModel)
        {
            try
            {
              
                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewDropDownModel.Message = objViewDropDownModel.MessageType = String.Empty;

                if (objViewDropDownModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DropDown + "/DeleteDropDown", objViewDropDownModel);
                    objViewDropDownModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDropDownModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewDropDownModel.Message = "DropDown Deleted Successfully";
                        objViewDropDownModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewDropDownModel.Message = "Error while deleting record";
                        objViewDropDownModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  DropDown List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DropDown + "/GetDropDownList", objViewDropDownModel);
                objViewDropDownModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDropDownModel>().Result : null;
              
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "DropDown", "View POST");
            }
            return PartialView("_DropDownList", objViewDropDownModel);
        }

        #endregion
    }
}
