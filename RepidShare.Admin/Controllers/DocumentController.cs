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
    public class DocumentController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit Document
        /// <summary>
        /// Add or Edit Document for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveDocument(string prm = "")
        {
            DocumentModel objDocumentModel = new DocumentModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int DocumentId;
                    //decrypt parameter and set in DocumentId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
                    //Get Document detail by  Document Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Document + "/GetDocumentById?DocumentId=" + DocumentId.ToString());
                    objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;
                }
                CaegoryDropDown(objDocumentModel.CategoryID);
                SubCatDropDown(objDocumentModel.SubCategoryID, objDocumentModel.CategoryID, null);
                StepDropDown(0, objDocumentModel.DocumentID);

                List<DropdownModel> objSavedStep = new List<DropdownModel>();
                if (ViewBag.objStepDDL != null)
                {
                    List<DropdownModel> objStepList = ViewBag.objStepDDL;

                    if (!string.IsNullOrWhiteSpace(objDocumentModel.SavedStep))
                    {
                        string[] SavedItem = objDocumentModel.SavedStep.Split(',');
                        foreach (var item in SavedItem)
                        {
                            int id = Convert.ToInt32(item);
                            var tempItem = objStepList.Where(val => val.ID == id).FirstOrDefault();
                            objSavedStep.Add(tempItem);
                            objStepList = objStepList.Where(val => val.ID != id).ToList();
                        }
                    }

                    ViewBag.objSavedStep = new SelectList(objSavedStep, "Id", "Value");
                    ViewBag.objStepDDL = new SelectList(objStepList, "Id", "Value");
                }
                GroupDropDown(objDocumentModel.GroupID);
                FillPackList(objDocumentModel.PackIds, objDocumentModel.DocumentID);
                UserDropDown("");
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "SaveDocument Get");
            }

            return View("SaveDocument", objDocumentModel);
        }

        public void FillPackList(string PackIds, int DocumentId)
        {
            ViewDocumentModel ObjViewDocumentModel = new ViewDocumentModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewDocumentModel.CurrentPage = 1;
                ObjViewDocumentModel.PageSize = int.MaxValue - 1;
                ObjViewDocumentModel.TotalPages = 0;
                //Get  Document List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetDocumentList", ObjViewDocumentModel);
                ObjViewDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentModel>().Result : null;
                //ObjViewDocumentModel = objBLDocument.GetDocumentList(ObjViewDocumentModel);
                if (ObjViewDocumentModel != null && ObjViewDocumentModel.DocumentList != null)
                {
                    //ViewBag.PackList = ObjViewDocumentModel.DocumentList.Where(x => x.PackIds == string.Empty && x.DocumentID != DocumentId).ToList();
                    ViewBag.PackList = new MultiSelectList(ObjViewDocumentModel.DocumentList.Where(x => x.PackIds == string.Empty && x.DocumentID != DocumentId).ToList(), "DocumentID", "DocumentTitle", Convert.ToString(string.IsNullOrWhiteSpace(PackIds) ? "" : PackIds).Split(",".ToCharArray()));
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "View GET");
            }

        }

        /// <summary>
        /// Add or Edit  Document based on DocumentID
        /// </summary>
        /// <param name="objDocumentModel"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Filters.Authorized]
        public ActionResult SaveDocument(DocumentModel objDocumentModel)
        {
            try
            {

                objDocumentModel.IsActive = true;
                objDocumentModel.CreatedBy = LoggedInUserID;

                //Insert or Update  Document
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/InsertUpdateDocument", objDocumentModel);
                objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;

                //if error code is 0 means  Document saved successfully
                if (Convert.ToInt32(objDocumentModel.ErrorCode) == 0)
                {
                    TempData["SucessMessage"] = "Document Saved Successfully";
                    return RedirectToAction("ViewDocument", "Document");
                }
                else if (Convert.ToInt32(objDocumentModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means Document Name is duplicate set duplicate Document error message.
                    objDocumentModel.Message = "Document Duplicate not allowed";
                    objDocumentModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objDocumentModel.Message = "Error while adding record";
                    objDocumentModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                CaegoryDropDown(objDocumentModel.CategoryID);
                SubCatDropDown(objDocumentModel.SubCategoryID, objDocumentModel.CategoryID, null);
                StepDropDown(0, objDocumentModel.DocumentID);
                UserDropDown(objDocumentModel.UserIds);
                List<DropdownModel> objSavedStep = new List<DropdownModel>();
                if (ViewBag.objStepDDL != null)
                {
                    List<DropdownModel> objStepList = ViewBag.objStepDDL;

                    if (!string.IsNullOrWhiteSpace(objDocumentModel.SavedStep))
                    {
                        string[] SavedItem = objDocumentModel.SavedStep.Split(',');
                        foreach (var item in SavedItem)
                        {
                            int id = Convert.ToInt32(item);
                            var tempItem = objStepList.Where(val => val.ID == id).FirstOrDefault();
                            objSavedStep.Add(tempItem);
                            objStepList = objStepList.Where(val => val.ID != id).ToList();
                        }
                    }

                    ViewBag.objSavedStep = new SelectList(objSavedStep, "Id", "Value");
                    ViewBag.objStepDDL = new SelectList(objStepList, "Id", "Value");
                }

                GroupDropDown(objDocumentModel.GroupID);
                FillPackList(objDocumentModel.PackIds, objDocumentModel.DocumentID);
                 
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "SaveDocument POST");
            }
            return View("SaveDocument", objDocumentModel);
        }

        #endregion

        #region View  Document
        /// <summary>
        /// View  Document List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewDocument()
        {
            ViewDocumentModel ObjViewDocumentModel = new ViewDocumentModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewDocumentModel.CurrentPage = 1;
                ObjViewDocumentModel.PageSize = CommonUtils.PageSize;
                ObjViewDocumentModel.TotalPages = 0;
                //Get  Document List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetDocumentList", ObjViewDocumentModel);
                ObjViewDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentModel>().Result : null;
                //ObjViewDocumentModel = objBLDocument.GetDocumentList(ObjViewDocumentModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewDocumentModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewDocumentModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
                CaegoryDropDown(ObjViewDocumentModel.FilterCategoryId);
                SubCatDropDown(ObjViewDocumentModel.FilterSubCatId, ObjViewDocumentModel.FilterCategoryId, null);
                StepDropDown(0, 0);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "View GET");
            }
            return View("ViewDocument", ObjViewDocumentModel);
        }

        /// <summary>
        /// View  Document List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewDocumentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewDocument(ViewDocumentModel objViewDocumentModel)
        {
            try
            {
                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewDocumentModel.Message = objViewDocumentModel.MessageType = String.Empty;

                if (objViewDocumentModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/DeleteDocument", objViewDocumentModel);
                    objViewDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewDocumentModel.Message = "Document Deleted Successfully";
                        objViewDocumentModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewDocumentModel.Message = "Error while deleting record";
                        objViewDocumentModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Document List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetDocumentList", objViewDocumentModel);
                objViewDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentModel>().Result : null;

                CaegoryDropDown(objViewDocumentModel.FilterCategoryId);
                SubCatDropDown(objViewDocumentModel.FilterSubCatId, objViewDocumentModel.FilterCategoryId, null);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "View POST");
            }
            return PartialView("_DocumentList", objViewDocumentModel);
        }

        #endregion

        #region Document HTML
        public ActionResult DocumentHTML(string prm = "")
        {
            DocumentModel objDocumentModel = new DocumentModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int DocumentId;
                    //decrypt parameter and set in DocumentId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
                    //Get Document detail by  Document Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Document + "/GetDocumentById?DocumentId=" + DocumentId.ToString());
                    objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "SaveDocument Get");
            }

            return View("DocumentHTML", objDocumentModel);
        }

        [HttpPost, ValidateInput(false)]
        [Filters.Authorized]
        public ActionResult DocumentHTML(DocumentModel objDocumentModel)
        {
            DocumentModel objDocumentModelTemp = new DocumentModel();
            try
            {
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Document + "/GetDocumentById?DocumentId=" + objDocumentModel.DocumentID.ToString());
                objDocumentModelTemp = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;

                objDocumentModelTemp.IsActive = true;
                objDocumentModelTemp.CreatedBy = LoggedInUserID;
                objDocumentModelTemp.DocumentHTML = objDocumentModel.DocumentHTML;

                //Insert or Update  Document
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/InsertUpdateDocument", objDocumentModelTemp);
                objDocumentModelTemp = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;

                //if error code is 0 means  Document saved successfully
                if (Convert.ToInt32(objDocumentModelTemp.ErrorCode) == 0)
                {
                    TempData["SucessMessage"] = "Document Saved Successfully";
                    return RedirectToAction("ViewDocument", "Document");
                }
                else if (Convert.ToInt32(objDocumentModelTemp.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means Document Name is duplicate set duplicate Document error message.
                    objDocumentModelTemp.Message = "Document Duplicate not allowed";
                    objDocumentModelTemp.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objDocumentModelTemp.Message = "Error while adding record";
                    objDocumentModelTemp.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "SaveDocument POST");
            }
            return View("SaveDocument", objDocumentModelTemp);
        }

        #endregion

        #region View Document Response
        /// <summary>
        /// View All Document Response
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewDocumentResponse()
        {
            ViewDocumentResponseModel ObjViewDocumentResponseModel = new ViewDocumentResponseModel();
            try
            {

                //initial set of current page, pageSize , Total pages
                ObjViewDocumentResponseModel.CurrentPage = 1;
                ObjViewDocumentResponseModel.PageSize = CommonUtils.PageSize;
                ObjViewDocumentResponseModel.TotalPages = 0;
                ObjViewDocumentResponseModel.AppID = AppID;
                //Get Document Response List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetAllDocumentResponse", ObjViewDocumentResponseModel);
                ObjViewDocumentResponseModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentResponseModel>().Result : null;


            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "ViewDocumentResponse GET");
            }
            return View("ViewDocumentResponseList", ObjViewDocumentResponseModel);
        }

        /// <summary>
        /// View All Document Response
        /// </summary>
        /// <param name="ObjViewDocumentResponseModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewDocumentResponse(ViewDocumentResponseModel ObjViewDocumentResponseModel)
        {
            try
            {

                ObjViewDocumentResponseModel.Message = ObjViewDocumentResponseModel.MessageType = String.Empty;

                //Get Document Response List based on searching , sorting and paging parameter.
                ObjViewDocumentResponseModel.AppID = AppID;
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetAllDocumentResponse", ObjViewDocumentResponseModel);
                ObjViewDocumentResponseModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentResponseModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "ViewDocumentResponse POST");
            }
            return PartialView("_ViewDocumentResponseList", ObjViewDocumentResponseModel);
        }

        /// <summary>
        /// View Document User According DocumentId
        /// </summary>
        /// <param name="prm"></param>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewDocumentUser(string prm = "")
        {
            ViewDocumentUserResponseModel ObjViewDocumentUserResponseModel = new ViewDocumentUserResponseModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewDocumentUserResponseModel.CurrentPage = 1;
                ObjViewDocumentUserResponseModel.PageSize = CommonUtils.PageSize;
                ObjViewDocumentUserResponseModel.TotalPages = 0;
                ObjViewDocumentUserResponseModel.AppID = AppID;

                //if prm(Paramter) is not empty means Document user View according to DocumentId
                if (!String.IsNullOrEmpty(prm))
                {
                    int DocumentId;
                    //prm Decrypt by Utility
                    int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);

                    //Get Document User By DocumentId
                    ObjViewDocumentUserResponseModel.DocumentID = DocumentId;

                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetAllDocumentResponseUser", ObjViewDocumentUserResponseModel);
                    ObjViewDocumentUserResponseModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentUserResponseModel>().Result : null;

                    ObjViewDocumentUserResponseModel.AppID = AppID;
                }

                //Set Success Message if comes from save Document page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(Session["ResultSucessMessage"])))
                {
                    ObjViewDocumentUserResponseModel.Message = Convert.ToString(Session["ResultSucessMessage"]);
                    ObjViewDocumentUserResponseModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    Session["ResultSucessMessage"] = null;
                }
                else if (!String.IsNullOrEmpty(Convert.ToString(Session["ResultNoticeMessage"])))
                {
                    ObjViewDocumentUserResponseModel.Message = Convert.ToString(Session["ResultNoticeMessage"]);
                    ObjViewDocumentUserResponseModel.MessageType = CommonUtils.MessageType.Notice.ToString().ToLower();
                    Session["ResultNoticeMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "ViewDocumentUser GET");
            }
            return View("ViewDocumentUser", ObjViewDocumentUserResponseModel);
        }

        /// <summary>
        /// View Document User According DocumentId
        /// </summary>
        /// <param name="ObjViewDocumentUserResponseModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewDocumentUser(ViewDocumentUserResponseModel ObjViewDocumentUserResponseModel)
        {
            try
            {
                ObjViewDocumentUserResponseModel.Message = ObjViewDocumentUserResponseModel.MessageType = String.Empty;
                //Get Document User List based on searching , sorting and paging parameter.
                ObjViewDocumentUserResponseModel.AppID = AppID;

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetAllDocumentResponseUser", ObjViewDocumentUserResponseModel);
                ObjViewDocumentUserResponseModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentUserResponseModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "ViewDocumentUser POST");
            }
            return PartialView("_ViewDocumentUserList", ObjViewDocumentUserResponseModel);
        }
        #endregion

        #region My Document
        /// <summary>
        /// View User Document List by UserId
        /// </summary>
        /// <param name="prm"></param>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult MyDocument(string prm = "")
        {
            ViewDocumentResponseModel ObjViewDocumentResponseModel = new ViewDocumentResponseModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewDocumentResponseModel.CurrentPage = 1;
                ObjViewDocumentResponseModel.PageSize = CommonUtils.PageSize;
                ObjViewDocumentResponseModel.TotalPages = 0;
                ObjViewDocumentResponseModel.AppID = AppID;
                ObjViewDocumentResponseModel.ApplicationRoleId = ApplicationRoleId;

                //if prm(Paramter) is not empty means Document user View according to Userid Otherwise Get Document by Logged In UserID
                if (!String.IsNullOrEmpty(prm))
                {
                    int userId;
                    //prm Decrypt by Utility
                    int.TryParse(CommonUtils.Decrypt(prm), out userId);
                    //Get User Document List By UserID
                    ObjViewDocumentResponseModel.UserId = userId;

                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetUserDocumentList", ObjViewDocumentResponseModel);
                    ObjViewDocumentResponseModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentResponseModel>().Result : null;

                    ObjViewDocumentResponseModel.AppID = AppID;
                }
                else
                {
                    ObjViewDocumentResponseModel.UserId = LoggedInUserID;
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetUserDocumentList", ObjViewDocumentResponseModel);
                    ObjViewDocumentResponseModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentResponseModel>().Result : null;
                    ObjViewDocumentResponseModel.AppID = AppID;
                }

                //Set Success Message if comes from save Document page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(Session["ResultSucessMessage"])))
                {
                    ObjViewDocumentResponseModel.Message = Convert.ToString(Session["ResultSucessMessage"]);
                    ObjViewDocumentResponseModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    Session["ResultSucessMessage"] = null;
                }
                else if (!String.IsNullOrEmpty(Convert.ToString(Session["ResultNoticeMessage"])))
                {
                    ObjViewDocumentResponseModel.Message = Convert.ToString(Session["ResultNoticeMessage"]);
                    ObjViewDocumentResponseModel.MessageType = CommonUtils.MessageType.Notice.ToString().ToLower();
                    Session["ResultNoticeMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "MyDocument GET");
            }
            return View("ViewUserDocumentList", ObjViewDocumentResponseModel);
        }

        /// <summary>
        /// View User Document List by UserId
        /// </summary>
        /// <param name="ObjViewDocumentResponseModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult MyDocument(ViewDocumentResponseModel ObjViewDocumentResponseModel)
        {
            try
            {
                ObjViewDocumentResponseModel.ApplicationRoleId = ApplicationRoleId;
                ObjViewDocumentResponseModel.Message = ObjViewDocumentResponseModel.MessageType = String.Empty;
                //Get User Document List based on searching , sorting and paging parameter.
                ObjViewDocumentResponseModel.AppID = AppID;
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetUserDocumentList", ObjViewDocumentResponseModel);
                ObjViewDocumentResponseModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentResponseModel>().Result : null;
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "MyDocument POST");
            }
            return PartialView("_ViewUserDocumentList", ObjViewDocumentResponseModel);
        }
        #endregion

        #region Copy Step's
        public ActionResult EditStep(string prm = "")
        {
            return PartialView("_stepEdit", prm);
        }
        #endregion
    }
}
