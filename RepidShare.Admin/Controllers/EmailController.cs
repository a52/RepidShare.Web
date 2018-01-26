using RepidShare.Entities.Email;
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
    public class EmailController : BaseController
    {

        HttpResponseMessage serviceResponse;
        CommonUtils objCommonUtils = new CommonUtils();
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        /// <summary>
        /// Set createdBy 
        /// </summary>
        public int createdBy
        {
            get
            {
                int _userID = 0;
                if (Session["UserId"] != null)
                    int.TryParse(Convert.ToString(Session["UserId"]), out _userID);
                return _userID;
            }
        }
        //
        // GET: /Email/

        public ActionResult Index()
        {
            return View();
        }
        [Filters.Authorized]
        public ActionResult ViewAllEmailTemplate()
        {
            ViewEmailTemplateModel objViewEmailTemplateModel = new ViewEmailTemplateModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                objViewEmailTemplateModel.CurrentPage = 1;
                objViewEmailTemplateModel.PageSize = CommonUtils.PageSize;
                objViewEmailTemplateModel.TotalPages = 0;
                //Get  SubCategory List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Email + "/GetEmailTemplateList", objViewEmailTemplateModel);
                objViewEmailTemplateModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewEmailTemplateModel>().Result : null;
                //ObjViewSubCategoryModel = objBLSubCategory.GetSubCategoryList(ObjViewSubCategoryModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    objViewEmailTemplateModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    objViewEmailTemplateModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }

                //CaegoryDropDown(objViewEmailTemplateModel.FilterCategoryId);
                // EmailTyepDropDown(ObjViewSubCategoryModel.FilterCategoryId);
                //GroupDropDown(ObjViewSubCategoryModel.FilterGroupID);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "ViewAllEmailTemplate", "ViewAllEmailTemplate");
            }
            //, ObjViewSubCategoryModel
            return View("ViewAllEmailTemplate", objViewEmailTemplateModel);
        }

        [Filters.Authorized]
        public ActionResult SaveEmailTemplate(string prm = "")
        {
            EmailTemplate objEmailTemplate = new EmailTemplate();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int EmailDetailID;
                    //decrypt parameter and set in SubCategoryId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out EmailDetailID);
                    //Get SubCategory detail by  SubCategory Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Email + "/GetEmailTemplateById?EmailDetailID=" + EmailDetailID.ToString());
                    objEmailTemplate = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<EmailTemplate>().Result : null;

                }
                EmailTyepDropDown(objEmailTemplate.EmailID);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SaveEmailTemplate", "SaveEmailTemplate Get");
            }

            return View("SaveEmailTemplate", objEmailTemplate);
        }


        [HttpPost, ValidateInput(false)]
        [Filters.Authorized]
        public ActionResult SaveEmailTemplate(EmailTemplate objEmailTemplate)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                objEmailTemplate.IsActive = true;
                objEmailTemplate.CreatedBy = LoggedInUserID;

                //Insert or Update  SubCategory
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Email + "/InsertUpdateEmailDetail", objEmailTemplate);
                objEmailTemplate = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<EmailTemplate>().Result : null;

                //if error code is 0 means  SubCategory saved successfully
                if (Convert.ToInt32(objEmailTemplate.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "EmailTemplate Saved Successfully";
                    return RedirectToAction("ViewAllEmailTemplate", "Email");
                }
                else if (Convert.ToInt32(objEmailTemplate.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means SubCategory Name is duplicate set duplicate SubCategory error message.
                    objEmailTemplate.Message = "EmailTemplate Duplicate not allowed";
                    objEmailTemplate.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objEmailTemplate.Message = "Error while adding record";
                    objEmailTemplate.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                EmailTyepDropDown(objEmailTemplate.EmailDetailID);

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SaveEmailTemplate", "SaveEmailTemplate Get");
            }
            return View("SaveEmailTemplate", objEmailTemplate);
        }


        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewAllEmailTemplate(ViewEmailTemplateModel objViewEmailTemplateModel)
        {
            try
            {

                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewEmailTemplateModel.Message = objViewEmailTemplateModel.MessageType = String.Empty;

                if (objViewEmailTemplateModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Email + "/DeleteEmailTemplate", objViewEmailTemplateModel);
                    objViewEmailTemplateModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewEmailTemplateModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewEmailTemplateModel.Message = "Email Template Deleted Successfully";
                        objViewEmailTemplateModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewEmailTemplateModel.Message = "Error while deleting record";
                        objViewEmailTemplateModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Category List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Email + "/GetEmailTemplateList", objViewEmailTemplateModel);
                objViewEmailTemplateModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewEmailTemplateModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "_EmailTemplateList", "View POST");
            }
            return PartialView("_EmailTemplateList", objViewEmailTemplateModel);
        }



    }
}
