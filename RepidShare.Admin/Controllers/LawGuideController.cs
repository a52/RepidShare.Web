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
    public class LawGuideController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit category
        /// <summary>
        /// Add or Edit Category for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveLawGuide(string prm = "")
        {
            LawGuideModel objLawGuideModel = new LawGuideModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int LawGuideId;
                    //decrypt parameter and set in LawGuideId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out LawGuideId);
                    //Get LawGuide detail by  LawGuide Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.LawGuide + "/GetLawGuideById?LawGuideId=" + LawGuideId.ToString());
                    objLawGuideModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<LawGuideModel>().Result : null;

                }
                SubCatDropDown(objLawGuideModel.SubCategoryID, null, null);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "LawGuide", "SaveLawGuide Get");
            }

            return View("SaveLawGuide", objLawGuideModel);
        }



        /// <summary>
        /// Add or Edit  LawGuide based on LawGuideID
        /// </summary>
        /// <param name="objLawGuideModel"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Filters.Authorized]
        public ActionResult SaveLawGuide(LawGuideModel objLawGuideModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                objLawGuideModel.IsActive = true;
                objLawGuideModel.CreatedBy = LoggedInUserID;

                //Insert or Update  LawGuide
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.LawGuide + "/InsertUpdateLawGuide", objLawGuideModel);
                objLawGuideModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<LawGuideModel>().Result : null;

                //if error code is 0 means  LawGuide saved successfully
                if (Convert.ToInt32(objLawGuideModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "LawGuide Saved Successfully";
                    return RedirectToAction("ViewLawGuide", "LawGuide");
                }
                else if (Convert.ToInt32(objLawGuideModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means LawGuide Name is duplicate set duplicate LawGuide error message.
                    objLawGuideModel.Message = "LawGuide Duplicate not allowed";
                    objLawGuideModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objLawGuideModel.Message = "Error while adding record";
                    objLawGuideModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                SubCatDropDown(objLawGuideModel.SubCategoryID, null, null);

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "LawGuide", "SaveLawGuide POST");
            }
            return View("SaveLawGuide", objLawGuideModel);
        }

        #endregion

        #region View  LawGuide
        /// <summary>
        /// View  LawGuide List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewLawGuide()
        {
            ViewLawGuideModel ObjViewLawGuideModel = new ViewLawGuideModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewLawGuideModel.CurrentPage = 1;
                ObjViewLawGuideModel.PageSize = CommonUtils.PageSize;
                ObjViewLawGuideModel.TotalPages = 0;
                //Get  LawGuide List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.LawGuide + "/GetLawGuideList", ObjViewLawGuideModel);
                ObjViewLawGuideModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewLawGuideModel>().Result : null;
                //ObjViewLawGuideModel = objBLLawGuide.GetLawGuideList(ObjViewLawGuideModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewLawGuideModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewLawGuideModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }

                SubCatDropDown(ObjViewLawGuideModel.FilterSubCatId,null,null);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "LawGuide", "View GET");
            }
            return View("ViewLawGuide", ObjViewLawGuideModel);
        }

        /// <summary>
        /// View  LawGuide List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewLawGuideModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewLawGuide(ViewLawGuideModel objViewLawGuideModel)
        {
            try
            {
                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewLawGuideModel.Message = objViewLawGuideModel.MessageType = String.Empty;

                if (objViewLawGuideModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.LawGuide + "/DeleteLawGuide", objViewLawGuideModel);
                    objViewLawGuideModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewLawGuideModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewLawGuideModel.Message = "LawGuide Deleted Successfully";
                        objViewLawGuideModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewLawGuideModel.Message = "Error while deleting record";
                        objViewLawGuideModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  LawGuide List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.LawGuide + "/GetLawGuideList", objViewLawGuideModel);
                objViewLawGuideModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewLawGuideModel>().Result : null;

                SubCatDropDown(objViewLawGuideModel.FilterSubCatId, null, null);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "LawGuide", "View POST");
            }
            return PartialView("_LawGuideList", objViewLawGuideModel);
        }

        #endregion

    }
}
