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
    public class GroupController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit Group
        /// <summary>
        /// Add or Edit Group for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveGroup(string prm = "")
        {
            GroupModel objGroupModel = new GroupModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int GroupId;
                    //decrypt parameter and set in GroupId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out GroupId);
                    //Get Group detail by  Group Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Group + "/GetGroupById?GroupId=" + GroupId.ToString());
                    objGroupModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<GroupModel>().Result : null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Group", "SaveGroup Get");
            }

            return View("SaveGroup", objGroupModel);
        }



        /// <summary>
        /// Add or Edit  Group based on GroupID
        /// </summary>
        /// <param name="objGroupModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Filters.Authorized]
        public ActionResult SaveGroup(GroupModel objGroupModel)
        {
            try
            {

                objGroupModel.IsActive = true;
                objGroupModel.CreatedBy = LoggedInUserID;

                //Insert or Update  Group
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Group + "/InsertUpdateGroup", objGroupModel);
                objGroupModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<GroupModel>().Result : null;

                //if error code is 0 means  Group saved successfully
                if (Convert.ToInt32(objGroupModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "Group Saved Successfully";
                    return RedirectToAction("ViewGroup", "Group");
                }
                else if (Convert.ToInt32(objGroupModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means Group Name is duplicate set duplicate Group error message.
                    objGroupModel.Message = "Group Duplicate not allowed";
                    objGroupModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objGroupModel.Message = "Error while adding record";
                    objGroupModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Group", "SaveGroup POST");
            }
            return View("SaveGroup", objGroupModel);
        }

        #endregion

        #region View  Group
        /// <summary>
        /// View  Group List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewGroup()
        {
            ViewGroupModel ObjViewGroupModel = new ViewGroupModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewGroupModel.CurrentPage = 1;
                ObjViewGroupModel.PageSize = CommonUtils.PageSize;
                ObjViewGroupModel.TotalPages = 0;
                //Get  Group List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Group + "/GetGroupList", ObjViewGroupModel);
                ObjViewGroupModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewGroupModel>().Result : null;
                //ObjViewGroupModel = objBLGroup.GetGroupList(ObjViewGroupModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewGroupModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewGroupModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Group", "View GET");
            }
            return View("ViewGroup", ObjViewGroupModel);
        }

        /// <summary>
        /// View  Group List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewGroupModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewGroup(ViewGroupModel objViewGroupModel)
        {
            try
            {
                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewGroupModel.Message = objViewGroupModel.MessageType = String.Empty;

                if (objViewGroupModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Group + "/DeleteGroup", objViewGroupModel);
                    objViewGroupModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewGroupModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewGroupModel.Message = "Group Deleted Successfully";
                        objViewGroupModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewGroupModel.Message = "Error while deleting record";
                        objViewGroupModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Group List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Group + "/GetGroupList", objViewGroupModel);
                objViewGroupModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewGroupModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Group", "View POST");
            }
            return PartialView("_GroupList", objViewGroupModel);
        }

        #endregion

    }
}
