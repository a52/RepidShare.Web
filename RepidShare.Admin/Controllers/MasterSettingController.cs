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
    public class MasterSettingController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit MasterSetting
        /// <summary>
        /// Add or Edit MasterSetting for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveMasterSetting(string prm = "")
        {
            MasterSettingModel objMasterSettingModel = new MasterSettingModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int MasterSettingId;
                    //decrypt parameter and set in MasterSettingId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out MasterSettingId);
                    //Get MasterSetting detail by  MasterSetting Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.MasterSetting + "/GetMasterSettingById?MasterSettingId=" + MasterSettingId.ToString());
                    objMasterSettingModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<MasterSettingModel>().Result : null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "MasterSetting", "SaveMasterSetting Get");
            }

            return View("SaveMasterSetting", objMasterSettingModel);
        }



        /// <summary>
        /// Add or Edit  MasterSetting based on MasterSettingID
        /// </summary>
        /// <param name="objMasterSettingModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Filters.Authorized]
        public ActionResult SaveMasterSetting(MasterSettingModel objMasterSettingModel)
        {
            try
            {

                objMasterSettingModel.IsActive = true;
                objMasterSettingModel.CreatedBy = LoggedInUserID;

                //Insert or Update  MasterSetting
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.MasterSetting + "/InsertUpdateMasterSetting", objMasterSettingModel);
                objMasterSettingModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<MasterSettingModel>().Result : null;

                //if error code is 0 means  MasterSetting saved successfully
                if (Convert.ToInt32(objMasterSettingModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "MasterSetting Saved Successfully";
                    return RedirectToAction("Index", "Home");
                }
                else if (Convert.ToInt32(objMasterSettingModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means MasterSetting Name is duplicate set duplicate MasterSetting error message.
                    objMasterSettingModel.Message = "MasterSetting Duplicate not allowed";
                    objMasterSettingModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objMasterSettingModel.Message = "Error while adding record";
                    objMasterSettingModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "MasterSetting", "SaveMasterSetting POST");
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

         

    }
}
