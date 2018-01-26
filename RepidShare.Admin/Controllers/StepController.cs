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
    public class StepController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit Step
        /// <summary>
        /// Add or Edit Step for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveStep(string prm = "")
        {
            StepModel objStepModel = new StepModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int StepId;
                    //decrypt parameter and set in StepId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out StepId);
                    //Get Step detail by  Step Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Step + "/GetStepById?StepId=" + StepId.ToString());
                    objStepModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<StepModel>().Result : null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Step", "SaveStep Get");
            }

            return View("SaveStep", objStepModel);
        }



        /// <summary>
        /// Add or Edit  Step based on StepID
        /// </summary>
        /// <param name="objStepModel"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Filters.Authorized]
        public ActionResult SaveStep(StepModel objStepModel)
        {
            try
            {

                objStepModel.IsActive = true;
                objStepModel.CreatedBy = LoggedInUserID;

                //Insert or Update  Step
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Step + "/InsertUpdateStep", objStepModel);
                objStepModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<StepModel>().Result : null;

                //if error code is 0 means  Step saved successfully
                if (Convert.ToInt32(objStepModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "Step Saved Successfully";
                    return RedirectToAction("ViewStep", "Step");
                }
                else if (Convert.ToInt32(objStepModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means Step Name is duplicate set duplicate Step error message.
                    objStepModel.Message = "Step Duplicate not allowed";
                    objStepModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objStepModel.Message = "Error while adding record";
                    objStepModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Step", "SaveStep POST");
            }
            return View("SaveStep", objStepModel);
        }

        #endregion

        #region View  Step
        /// <summary>
        /// View  Step List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewStep()
        {
            ViewStepModel ObjViewStepModel = new ViewStepModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewStepModel.CurrentPage = 1;
                ObjViewStepModel.PageSize = CommonUtils.PageSize;
                ObjViewStepModel.TotalPages = 0;
                //Get  Step List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Step + "/GetStepList", ObjViewStepModel);
                ObjViewStepModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewStepModel>().Result : null;
                //ObjViewStepModel = objBLStep.GetStepList(ObjViewStepModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewStepModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewStepModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Step", "View GET");
            }
            return View("ViewStep", ObjViewStepModel);
        }

        /// <summary>
        /// View  Step List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewStepModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewStep(ViewStepModel objViewStepModel)
        {
            try
            {
                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewStepModel.Message = objViewStepModel.MessageType = String.Empty;

                if (objViewStepModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Step + "/DeleteStep", objViewStepModel);
                    objViewStepModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewStepModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewStepModel.Message = "Step Deleted Successfully";
                        objViewStepModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewStepModel.Message = "Error while deleting record";
                        objViewStepModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Step List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Step + "/GetStepList", objViewStepModel);
                objViewStepModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewStepModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Step", "View POST");
            }
            return PartialView("_StepList", objViewStepModel);
        }

        #endregion

    }
}
