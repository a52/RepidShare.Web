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
    public class MasterController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit Master
        /// <summary>
        /// Add or Edit Master for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveMaster(string prm = "")
        {
            MasterModel objMasterModel = new MasterModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int MasterId;
                    //decrypt parameter and set in MasterId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out MasterId);
                    //Get Master detail by  Master Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Master + "/GetMasterById?MasterId=" + MasterId.ToString());
                    objMasterModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<MasterModel>().Result : null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Master", "SaveMaster Get");
            }

            return View("SaveMaster", objMasterModel);
        }



        /// <summary>
        /// Add or Edit  Master based on MasterID
        /// </summary>
        /// <param name="objMasterModel"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Filters.Authorized]
        public ActionResult SaveMaster(MasterModel objMasterModel)
        {
            try
            {

                objMasterModel.IsActive = true;
                objMasterModel.CreatedBy = LoggedInUserID;

                //Insert or Update  Master
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Master + "/InsertUpdateMaster", objMasterModel);
                objMasterModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<MasterModel>().Result : null;

                //if error code is 0 means  Master saved successfully
                if (Convert.ToInt32(objMasterModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "Master Saved Successfully";
                    return RedirectToAction("ViewMaster", "Master");
                }
                else if (Convert.ToInt32(objMasterModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means Master Name is duplicate set duplicate Master error message.
                    objMasterModel.Message = "Master Duplicate not allowed";
                    objMasterModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objMasterModel.Message = "Error while adding record";
                    objMasterModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Master", "SaveMaster POST");
            }
            return View("SaveMaster", objMasterModel);
        }

        #endregion

        #region View  Master
        /// <summary>
        /// View  Master List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewMaster()
        {
            ViewMasterModel ObjViewMasterModel = new ViewMasterModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewMasterModel.CurrentPage = 1;
                ObjViewMasterModel.PageSize = CommonUtils.PageSize;
                ObjViewMasterModel.TotalPages = 0;
                //Get  Master List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Master + "/GetMasterList", ObjViewMasterModel);
                ObjViewMasterModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewMasterModel>().Result : null;
                //ObjViewMasterModel = objBLMaster.GetMasterList(ObjViewMasterModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewMasterModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewMasterModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Master", "View GET");
            }
            return View("ViewMaster", ObjViewMasterModel);
        }

        /// <summary>
        /// View  Master List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewMasterModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewMaster(ViewMasterModel objViewMasterModel)
        {
            try
            {
                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewMasterModel.Message = objViewMasterModel.MessageType = String.Empty;

                if (objViewMasterModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Master + "/DeleteMaster", objViewMasterModel);
                    objViewMasterModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewMasterModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewMasterModel.Message = "Master Deleted Successfully";
                        objViewMasterModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewMasterModel.Message = "Error while deleting record";
                        objViewMasterModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Master List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Master + "/GetMasterList", objViewMasterModel);
                objViewMasterModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewMasterModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Master", "View POST");
            }
            return PartialView("_MasterList", objViewMasterModel);
        }

        #endregion

    }
}
