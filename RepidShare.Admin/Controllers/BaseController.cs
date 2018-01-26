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
    public class BaseController : Controller
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Properties
        public int LoggedInUserID
        {
            get
            {
                int _userID = 0;
                if (Session["UserId"] != null)
                    int.TryParse(Convert.ToString(Session["UserId"]), out _userID);
                return _userID;
            }
        }
        public int AppID
        {
            get
            {
                int _appID = 0;
                if (Session["AppID"] != null)
                    int.TryParse(Convert.ToString(Session["AppID"]), out _appID);
                return _appID;
            }
        }

        public int RoleID
        {
            get
            {
                int _roleID = 0;
                if (Session["RoleId"] != null)
                    int.TryParse(Convert.ToString(Session["RoleId"]), out _roleID);
                return _roleID;
            }
        }

        public string RoleType
        {
            get
            {
                string _roleType = "";
                if (Session["RoleType"] != null)
                    _roleType = Convert.ToString(Session["RoleType"]);
                return _roleType;
            }
        }

        public string LoggedInUserEmailID
        {
            get
            {
                string _emailID = "";
                if (Session["UserEmailID"] != null)
                    _emailID = Convert.ToString(Session["UserEmailID"]);
                return _emailID; // need to fetch from Session["UserEmailID"] 
            }
        }

        public int ApplicationRoleId
        {
            get
            {
                int _applicationRoleId = 0;
                if (Session["ApplicationRoleId"] != null)
                    _applicationRoleId = Convert.ToInt32(Session["ApplicationRoleId"]);
                return _applicationRoleId;
            }
        }





        #endregion

        #region ErrorLog
        /// <summary>
        /// Insert exception or error detail into the database.
        /// </summary>
        /// <param name="UserID">Id of Loggedin User</param>
        /// <param name="ErrorMsg">Detail of error</param>
        /// <param name="ErrorStack">Stack detail of exception</param>
        /// <param name="ControllerName">Name of controller</param>
        /// <param name="FunctionName">Name of action where exception or error occur.</param> 
        public void ErrorLog(Exception ex, string ControllerName, string FunctionName)
        {

        }
        #endregion

        #region Fill Dropdowns
        public void CaegoryDropDown(int CategoryId)
        {
            try
            {
                List<DropdownModel> objCategoryDDL = new List<DropdownModel>();
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Category + "/FillCaegoryDropDown");
                objCategoryDDL = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<DropdownModel>>().Result : null;
                ViewBag.CaegoryDropDown = new SelectList(objCategoryDDL, "Id", "Value", CategoryId);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SubCategory", "FillCaegoryDropDown");
                throw ex;
            }
        }

        public void GroupDropDown(int? GroupId)
        {
            try
            {
                List<DropdownModel> objGroupDDL = new List<DropdownModel>();
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Group + "/FillGroupDropDown");
                objGroupDDL = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<DropdownModel>>().Result : null;
                ViewBag.GroupDropDown = new SelectList(objGroupDDL, "Id", "Value", GroupId);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SubCategory", "FillGroupDropDown");
                throw ex;
            }
        }

        public void SubCatDropDown(int SubCatId, int? CategoryID, int? GroupID)
        {
            try
            {
                List<DropdownModel> objSubCatDDL = new List<DropdownModel>();
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.SubCategory + "/FillSubCategoryDropDown?CategoryID=" + CategoryID + "&GroupID=" + GroupID + "");
                objSubCatDDL = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<DropdownModel>>().Result : null;
                ViewBag.SubCatDropDown = new SelectList(objSubCatDDL, "Id", "Value", SubCatId);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SubCategory", "FillSubDropDown");
                throw ex;
            }
        }

        public void StepDropDown(int StepId, int DocumentId)
        {
            try
            {
                List<DropdownModel> objStepDDL = new List<DropdownModel>();
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Step + "/FillStepDropDownByDocumentId?DocumentId=" + DocumentId);
                objStepDDL = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<DropdownModel>>().Result : null;
                //For Step Mapping 
                ViewBag.objStepDDL = objStepDDL;
                ViewBag.StepDropDown = new SelectList(objStepDDL, "Id", "Value", StepId);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Step", "FillStepDropDown");
                throw ex;
            }
        }

        public void DropDownValue(string Ids = null)
        {
            try
            {
                List<DropdownModel> objDropdown = new List<DropdownModel>();
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.DropDown + "/FillDropDown");
                objDropdown = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<DropdownModel>>().Result : null;
                ViewBag.DropDownList = new MultiSelectList(objDropdown, "Id", "Value", string.IsNullOrWhiteSpace(Ids) ? null : Ids.Split(','));
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Step", "FillStepDropDown");
                throw ex;
            }
        }

        public void UserDropDown(string UserIds)
        {
            try
            {
                List<DropdownModel> objCategoryDDL = new List<DropdownModel>();
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/FillUserDropDown");
                objCategoryDDL = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<DropdownModel>>().Result : null;
                ViewBag.UserListDropDown = new MultiSelectList(objCategoryDDL, "Id", "Value", Convert.ToString(string.IsNullOrWhiteSpace(UserIds) ? "" : UserIds).Split(",".ToCharArray()));
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "UserList", "FillUserList");
                throw ex;
            }
        }

        #endregion


        #region DropDown Return Json Action
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetSubCategoryByOtherIds(int? CategoryID, int? GroupID)
        {
            try
            {
                SubCatDropDown(0, CategoryID, GroupID);
                return Json(ViewBag.SubCatDropDown);
            }
            catch (Exception ex)
            {
                //ErrorLog("userid", "MasterController", ex);
                return Json(ex);
            }
        }
        #endregion

        #region
        public void EmailTyepDropDown(int EmailID)
        {
            try
            {
                List<DropdownModel> objEmailTyepDDL = new List<DropdownModel>();
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Email + "/FillEmailTyepDropDown");
                objEmailTyepDDL = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<DropdownModel>>().Result : null;
                ViewBag.EmailTemplateDropDown = new SelectList(objEmailTyepDDL, "Id", "Value", EmailID);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "EmailTyepDropDown", "EmailTyepDropDown");
                throw ex;
            }
        }

        #endregion


        public ActionResult Download(string DocID)
        {
            HttpResponseMessage serviceResponse;
            UtilityWeb objUtilityWeb = new UtilityWeb();
            BulletinModel objBulletinModel = new BulletinModel();

            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Bulletin + "/GetBulletinById?BulletinId=" + DocID.ToString());
            objBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<BulletinModel>().Result : null;

            Response.Clear();
            MemoryStream ms = new MemoryStream(objBulletinModel.AttachmentContent);
            Response.ContentType = objBulletinModel.AttachmentType;
            Response.AddHeader("content-disposition", objBulletinModel.AttachmentType);
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();

            return new FileStreamResult(ms, objBulletinModel.AttachmentType);
        }
    }
}
