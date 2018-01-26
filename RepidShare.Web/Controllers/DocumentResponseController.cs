using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepidShare.Utility;
using RepidShare.Entities;
using System.Net.Http;
using System.Net;
using HtmlAgilityPack;
using System.Data;
using RepidShare.Entities.DocumentResponse;

namespace RepidShare.Web.Controllers
{
    public class DocumentResponseController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region View Document Response


        /// <summary>
        /// View Document response 
        /// </summary>
        /// <returns></returns>
        //[Filters.MenuAccess]
        public ActionResult ViewDocumentResponse(string prm)
        {
            //Object of Model.
            DocumentResponseDetailModel objDocumentDetailModel = new DocumentResponseDetailModel();
            try
            {

                int userid = 0;
                int DocumentID = 0;
                String PageName = string.Empty;

                if (!String.IsNullOrEmpty(prm))
                {
                    //Decrypt query string 
                    string inputParameter = CommonUtils.Decrypt(prm).ToString().Replace("?", "").ToLower();

                    //get query string vaule and set into paramtere of userid, Documentid and pagename
                    if (inputParameter.IndexOf("userid") > -1 || inputParameter.IndexOf("Documentid") > -1)
                    {
                        foreach (var item in inputParameter.Split('&'))
                        {
                            //id of the user to see response
                            if (item.IndexOf("userid") > -1)
                            {
                                int.TryParse(item.Replace("userid=", ""), out userid);
                            }
                            //id of Document 
                            if (item.IndexOf("Documentid") > -1)
                            {
                                int.TryParse(item.Replace("Documentid=", ""), out DocumentID);
                            }
                            // Name of the page from where this url open
                            if (item.IndexOf("pagename") > -1)
                            {
                                PageName = Convert.ToString(item.Replace("pagename=", ""));
                            }

                        }
                    }
                }

                //check page name is blank or null
                if (string.IsNullOrWhiteSpace(PageName))
                {
                    //if its null then by default set referral url to my Document.
                    //objDocumentDetailModel.ReferralUrl = "/Document/myDocument";
                    //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewUserDocument;//"My Documents";
                    objDocumentDetailModel.ReferralUrl = "/Home/Home";
                    objDocumentDetailModel.UrlTitle = "Home";
                }
                else if (string.IsNullOrWhiteSpace(objDocumentDetailModel.ReferralUrl))
                {
                    //check where from this page url is open 
                    switch (PageName)
                    {
                        case "home":
                            // come from Home page.
                            objDocumentDetailModel.ReferralUrl = "/Home/Home";
                            objDocumentDetailModel.UrlTitle = "Home";

                            break;
                        case "myDocument":
                            //come from My Document page.
                            objDocumentDetailModel.ReferralUrl = "/Document/myDocument";
                            //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewUserDocument;//  "My Documents";
                            break;
                        case "Documentuser":
                            //come from View Document user
                            objDocumentDetailModel.ReferralUrl = "/Document/viewDocumentuser?prm=" + CommonUtils.Encrypt(Convert.ToString(DocumentID));
                            //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewDocumentUser;// "View Document User";
                            break;
                        default:
                            //by default set to my Document page.
                            objDocumentDetailModel.ReferralUrl = "/Document/myDocument";
                            //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewUserDocument;//"My Documents";
                            break;
                    }
                }


                //Set default parameter
                objDocumentDetailModel.DocumentID = DocumentID;
                objDocumentDetailModel.UserId = userid;
                objDocumentDetailModel.ApplicationRoleID = ApplicationRoleId;
                objDocumentDetailModel.NoOfAttempt = 0;
                objDocumentDetailModel.CurrentPage = 1;
                objDocumentDetailModel.PageSize = CommonUtils.PageSize;
                objDocumentDetailModel.IsReadOnly = true;

                //Get Document response detail model from database.
                //Set false for view Document 

                GetDocumentResponse(objDocumentDetailModel, false);

                /* if @ErrorCode =50 then Unautorized 
                   if @ErrorCode =51 then Document not published  
                   if @ErrorCode =52 then Document completed
                   if @ErrorCode =53 then Document expired
                   if @ErrorCode =54 then Document response completed and multiple response not allowed 
                   if @ErrorCode =55 then Document response not exist
                  */
                //check error code 
                switch (objDocumentDetailModel.ErrorCode)
                {
                    case 50:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgViewDocumentUnauthorized;
                        //return RedirectToAction("MyDocument", "Document");
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 55:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgDocumentResponseNotExist;
                        //return RedirectToAction("MyDocument", "Document");
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 61:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResource.msgDocumentNotExist;
                        //return RedirectToAction("MyDocument", "Document");
                        return Redirect(objDocumentDetailModel.ReferralUrl);
                }
            }
            //if there is any error exception then set error message and log detail of error into database.
            catch (Exception ex)
            {
                ErrorLog(ex, "DocumentResponse", "ViewDocumentResponse GET");
                //objDocumentDetailModel.Message = String.Format(DocumentResponse.msgViewDocumentError, "View Document Response");
                objDocumentDetailModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
            }
            //return view
            return View(objDocumentDetailModel);
        }

        /// <summary>
        /// View Document response POST
        /// </summary>
        /// <returns>return partial view</returns>
        [HttpPost]
        //[Filters.Authorized]
        public ActionResult ViewDocumentResponse(DocumentResponseDetailModel objDocumentDetailModel)
        {
            try
            {
                //check no of attempt change on view page then set current page to 1
                if (objDocumentDetailModel != null && !string.IsNullOrWhiteSpace(objDocumentDetailModel.ActionType) && objDocumentDetailModel.ActionType == "changeattempt")
                {
                    objDocumentDetailModel.CurrentPage = 1;
                }

                //Get Document response detail model from database.
                //Set false for view Document 
                GetDocumentResponse(objDocumentDetailModel, false);
            }
            //if there is any error exception then set error message and log detail of error into database.
            catch (Exception ex)
            {
                ErrorLog(ex, "DocumentResponse", "ViewDocumentResponse Post");
                //objDocumentDetailModel.Message = String.Format(DocumentResponse.msgViewDocumentError, "View Document Response");
                objDocumentDetailModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
            }
            return PartialView("_ViewDocumentQueAnsList", objDocumentDetailModel);
        }

        //[Filters.MenuAccess]
        public ActionResult ViewDocumentPreview(string prm)
        {
            //Object of Model.
            DocumentResponseDetailModel objDocumentDetailModel = new DocumentResponseDetailModel();
            try
            {

                int DocumentID = 0;
                String PageName = string.Empty;
                if (!String.IsNullOrEmpty(prm))
                {
                    //Decrypt query string 
                    string inputParameter = CommonUtils.Decrypt(prm).ToString().Replace("?", "").ToLower();

                    //Get query string paramtere  and set value 
                    // get Document id 
                    if (inputParameter.IndexOf("Documentid") > -1)
                    {
                        foreach (var item in inputParameter.Split('&'))
                        {

                            if (item.IndexOf("Documentid") > -1)
                            {
                                int.TryParse(item.Replace("Documentid=", ""), out DocumentID);
                            }
                            // get page name where from this page url is open.
                            if (item.IndexOf("pagename") > -1)
                            {
                                PageName = Convert.ToString(item.Replace("pagename=", ""));
                            }

                        }
                    }
                }

                //check page name is null or blank then set by default url.
                if (string.IsNullOrWhiteSpace(PageName))
                {

                    objDocumentDetailModel.ReferralUrl = "/Document/ViewDocument";
                    //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewDocument;//"View Document";
                }
                else if (string.IsNullOrWhiteSpace(objDocumentDetailModel.ReferralUrl))
                {
                    switch (PageName)
                    {
                        case "home":
                            objDocumentDetailModel.ReferralUrl = "/Home/Home";
                            objDocumentDetailModel.UrlTitle = "Home";

                            break;

                        case "Documentuser":
                            objDocumentDetailModel.ReferralUrl = "/Document/viewDocumentuser?prm=" + CommonUtils.Encrypt(Convert.ToString(DocumentID));
                            //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewDocumentUser;//"View Document User";
                            break;
                        default:
                            objDocumentDetailModel.ReferralUrl = "/Document/ViewDocument";
                            //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewDocument;//"View Document";
                            break;

                    }

                }

                //Set default parameter
                objDocumentDetailModel.DocumentID = DocumentID;
                objDocumentDetailModel.IsReadOnly = false;
                //Get Document response detail model from database.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DocumentResponse + "/GetDocumentPreview", objDocumentDetailModel);
                objDocumentDetailModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentResponseDetailModel>().Result : null;

                //objDocumentDetailModel = objBLDocumentResponse.GetDocumentPreview(objDocumentDetailModel);

                switch (objDocumentDetailModel.ErrorCode)
                {

                    case 61:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResource.msgDocumentNotExist;
                        return Redirect(objDocumentDetailModel.ReferralUrl);
                }
                if (objDocumentDetailModel != null && objDocumentDetailModel.Questions != null && objDocumentDetailModel.Questions.Count() > 0)
                {
                    CommonUtils objCommonUtils = new CommonUtils();
                    //set question type detail for questions
                    foreach (ViewQuestionAnswerModel objViewQuestionAnswerModel in objDocumentDetailModel.Questions)
                    {
                        objViewQuestionAnswerModel.QuestionTypeDetail = new QuestionTypeDetailModel();
                        if (!String.IsNullOrEmpty(objViewQuestionAnswerModel.QuestionType))
                        {
                            objViewQuestionAnswerModel.QuestionTypeDetail = objCommonUtils.SetQuestionProperties(objViewQuestionAnswerModel.QuestionType, objViewQuestionAnswerModel.QuestionPropertyList, objViewQuestionAnswerModel.QuestionTypeDetail, objViewQuestionAnswerModel.QuestionOptionsList);
                        }


                    }
                }
            }
            //handle exception and set error message and log.
            catch (Exception ex)
            {
                ErrorLog(ex, "DocumentResponse", "ViewDocumentPreview GET");
                //objDocumentDetailModel.Message = String.Format(DocumentResponse.msgViewDocumentError, "View Document Preview");
                objDocumentDetailModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
            }
            //return view
            return View(objDocumentDetailModel);
        }

        #endregion

        #region Save Document Response

        /// <summary>
        /// Save Document response 
        /// </summary>
        /// <returns></returns>
        //[Filters.MenuAccess]
        public ActionResult SaveDocumentResponse(string prm)
        {
            //Object of Model.
            DocumentResponseDetailModel objDocumentDetailModel = new DocumentResponseDetailModel();
            try
            {

                int userid = 0;
                int DocumentID = 0;
                String PageName = string.Empty;
                int currentpage = 1;
                //check query string 
                if (!String.IsNullOrEmpty(prm))
                {
                    //Decrypt
                    string inputParameter = CommonUtils.Decrypt(prm).ToString().Replace("?", "").ToLower();

                    //get query string value
                    if (inputParameter.IndexOf("userid") > -1 || inputParameter.IndexOf("documentid") > -1)
                    {
                        foreach (var item in inputParameter.Split('&'))
                        {
                            if (item.IndexOf("userid") > -1)
                            {
                                int.TryParse(item.Replace("userid=", ""), out userid);
                            }
                            if (item.IndexOf("documentid") > -1)
                            {
                                int.TryParse(item.Replace("documentid=", ""), out DocumentID);
                            }
                            if (item.IndexOf("pagename") > -1)
                            {
                                PageName = Convert.ToString(item.Replace("pagename=", ""));
                            }
                            if (item.IndexOf("currentpage") > -1)
                            {
                                int.TryParse(item.Replace("currentpage=", ""), out currentpage);
                            }

                        }
                    }
                }

                objDocumentDetailModel.UrlPageName = PageName;

                if (string.IsNullOrWhiteSpace(PageName))
                {

                    //objDocumentDetailModel.ReferralUrl = "/Document/myDocument";
                    //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewUserDocument;// "My Documents";
                    objDocumentDetailModel.ReferralUrl = "/User/SummaryView";
                    objDocumentDetailModel.UrlTitle = "User";
                }
                else if (string.IsNullOrWhiteSpace(objDocumentDetailModel.ReferralUrl))
                {
                    switch (PageName)
                    {
                        //case "home":
                        //    objDocumentDetailModel.ReferralUrl = "/Home/Home";
                        //    objDocumentDetailModel.UrlTitle = "Home";

                        //    break;
                        case "myDocument":
                            objDocumentDetailModel.ReferralUrl = "/User/myDocument";
                            //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewUserDocument;//"My Documents";
                            break;
                        case "Documentuser":
                            objDocumentDetailModel.ReferralUrl = "/Document/viewDocumentuser?prm=" + CommonUtils.Encrypt(Convert.ToString(DocumentID));
                            //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewDocumentUser;// "View Document User";
                            break;
                        default:
                            objDocumentDetailModel.ReferralUrl = "/User/myDocument";
                            //objDocumentDetailModel.UrlTitle = IIMSDocumentEntities.Resource.DocumentResource.lblViewUserDocument;//"My Documents";
                            break;
                    }

                }

                //Set default parameter
                objDocumentDetailModel.DocumentID = DocumentID;
                objDocumentDetailModel.UserId = LoggedInUserID;
                objDocumentDetailModel.ApplicationRoleID = ApplicationRoleId;
                objDocumentDetailModel.CurrentPage = currentpage;
                objDocumentDetailModel.PageSize = 1;
                objDocumentDetailModel.IsReadOnly = false;
                //fill Document response detail model from database.
                objDocumentDetailModel = GetDocumentResponse(objDocumentDetailModel, true);
                /* if @ErrorCode =50 then Unautorized 
                   if @ErrorCode =51 then Document not published  
                   if @ErrorCode =52 then Document completed
                   if @ErrorCode =53 then Document expired
                   if @ErrorCode =54 then Document response completed and multiple response not allowed 
                   if @ErrorCode =55 then Document response not exist
                   if @ErrorCode =56 then Document response already started by mobile.
                   if @ErrorCode =61 then Document not exists.
                  */
                switch (objDocumentDetailModel.ErrorCode)
                {
                    case 50:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgSaveDocumentUnauthorized;
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 51:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgDocumentNotPublished;
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 52:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgDocumentCompleted;
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 53:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgDocumentExpired;
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 54:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgDocumentSingleResponse;
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 55:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgDocumentResponseNotExist;
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 56:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResponse.msgDocumentByMobile;
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                    case 61:
                        //Session["ResultNoticeMessage"] = IIMSDocumentEntities.Resource.DocumentResource.msgDocumentNotExist;
                        return Redirect(objDocumentDetailModel.ReferralUrl);

                }


                for (int i = 0; i < objDocumentDetailModel.Questions.Count; i++)
                {
                    if (objDocumentDetailModel.Questions[i].QuestionType == RepidShare.Utility.CommonUtils.QuestionType.Price_Question.ToString())
                    {
                        objDocumentDetailModel.Questions[i].objPriceQuestionModel = new PriceQuestionModel();
                        objDocumentDetailModel.Questions[i].objPriceQuestionModel.QuestionId = objDocumentDetailModel.Questions[i].QuestionID;
                        objDocumentDetailModel.Questions[i].objPriceQuestionModel.ResultDetailId = objDocumentDetailModel.Questions[i].ResultDetailID;
                        serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DocumentResponse + "/GetPriceQuestion", objDocumentDetailModel.Questions[i].objPriceQuestionModel);
                        objDocumentDetailModel.Questions[i].objPriceQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<PriceQuestionModel>().Result : null;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "DocumentResponse", "SaveDocumentResponse Get");
                //objDocumentDetailModel.Message = String.Format(DocumentResponse.msgViewDocumentError, "Save Document Response");
                objDocumentDetailModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
            }
            return View(objDocumentDetailModel);
        }



        /// <summary>
        /// Save Document response 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        //[Filters.Authorized]
        public ActionResult SaveDocumentResponse(DocumentResponseDetailModel objDocumentDetailModel)
        {
            try
            {
                if (objDocumentDetailModel.Result != null)
                {

                    Boolean IsRedirect = false;
                    //Get action type 
                    switch (objDocumentDetailModel.ActionType.ToLower())
                    {
                        case "previous":
                            objDocumentDetailModel.CurrentPage = objDocumentDetailModel.CurrentPage - 1;
                            break;
                        case "next":
                            objDocumentDetailModel.CurrentPage = objDocumentDetailModel.CurrentPage + 1;
                            break;

                        case "custom":
                            objDocumentDetailModel.CurrentPage = objDocumentDetailModel.CurrentPage;
                            break;
                        //Save
                        case "save":
                            IsRedirect = true;
                            break;
                        case "submit":
                            IsRedirect = true;
                            objDocumentDetailModel.Result.IsCompleted = true;

                            break;
                    }
                   
                    //objDocumentDetailModel.Result.NoOfAttempt = objDocumentDetailModel.Result.NoOfAttempt == 0 ? 1 : objDocumentDetailModel.Result.NoOfAttempt;
                    objDocumentDetailModel.CommonCreatedBy = LoggedInUserID;
                    for (int i = 0; i < objDocumentDetailModel.Questions.Count(); i++)
                    {
                        if (!string.IsNullOrWhiteSpace(objDocumentDetailModel.Questions[i].SelectedAnswers))
                        {
                            //radio or checkbox
                            if (objDocumentDetailModel.Questions[i].SelectedAnswers.Split(',').Length > 0)
                            {
                                //checkbox
                                if (objDocumentDetailModel.Questions[i].QuestionOptionsList != null)
                                {
                                    for (int j = 0; j < objDocumentDetailModel.Questions[i].QuestionOptionsList.Count(); j++)
                                    {
                                        //options
                                        if (objDocumentDetailModel.Questions[i].SelectedAnswers.Split(',').Contains(Convert.ToString(objDocumentDetailModel.Questions[i].QuestionOptionsList[j].QuestionOptionsID)))
                                        {
                                            objDocumentDetailModel.Questions[i].QuestionOptionsList[j].IsSelected = true;
                                        }
                                        else
                                        {
                                            objDocumentDetailModel.Questions[i].QuestionOptionsList[j].IsSelected = false;
                                        }
                                    }
                                }
                            }


                        }
                    }


                    //answer into xml from model 
                    if (objDocumentDetailModel.Questions != null)
                    {
                        objDocumentDetailModel.QuestionAnswerXml = string.Empty;
                        objDocumentDetailModel.QuestionAnswerXml += CommonUtils.GetBulkXML(objDocumentDetailModel.Questions);
                    }

                    //objDocumentDetailModel.Result.DocumentTargetAudienceID = LoggedInUserID;

                    //Insert or Update Document Question  Answer
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DocumentResponse + "/InsertUpdateDocumentResult", objDocumentDetailModel);
                    objDocumentDetailModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentResponseDetailModel>().Result : null;

                    //objDocumentDetailModel = objBLDocumentResponse.InsertUpdateDocumentResult(objDocumentDetailModel);
                    DocumentResponseDetailModel objTemp = GetDocumentResponse(objDocumentDetailModel, true);


                    if (objDocumentDetailModel.Questions.Where(x => x.QuestionType == RepidShare.Utility.CommonUtils.QuestionType.Price_Question.ToString()).ToList().Count > 0)
                    {
                        var objPrice_Temp = objTemp.Questions.Where(x => x.QuestionType == RepidShare.Utility.CommonUtils.QuestionType.Price_Question.ToString()).ToList();
                        var objPrice_Question = objDocumentDetailModel.Questions.Where(x => x.QuestionType == RepidShare.Utility.CommonUtils.QuestionType.Price_Question.ToString()).ToList();
                        for (int i = 0; i < objPrice_Question.Count; i++)
                        {
                            if (objPrice_Temp.Where(x => x.QuestionID == objPrice_Question[i].QuestionID).ToList().Count > 0 && objPrice_Question[i].ResultDetailID == 0)
                            {
                                objPrice_Question[i].ResultDetailID = objPrice_Temp.Where(x => x.QuestionID == objPrice_Question[i].QuestionID).FirstOrDefault().ResultDetailID;
                            }
                            DataTable dtPriceDetails = new DataTable();
                            //dtPriceDetails.Columns.Add("ResultDetailId"); dtPriceDetails.Columns.Add("QuestionId"); dtPriceDetails.Columns.Add("InstAmt");
                            //dtPriceDetails.Columns.Add("TaxAmt"); dtPriceDetails.Columns.Add("IneteAmt"); dtPriceDetails.Columns.Add("TotalAmt");
                            //dtPriceDetails.Columns.Add("DateAmt");dtPriceDetails.Columns.Add("PenaltyAmt");

                            objPrice_Question[i].objPriceQuestionModel.QuestionId = objPrice_Question[i].QuestionID;
                            //objPrice_Question[i].objPriceQuestionModel.ResultDetailId = objPrice_Question[i].ResultDetailID;
                            objPrice_Question[i].objPriceQuestionModel.PrintAmt = !string.IsNullOrWhiteSpace(objPrice_Question[i].AnswerDetail) ? Convert.ToDecimal((objPrice_Question[i].AnswerDetail)) : 0;

                            if (!string.IsNullOrWhiteSpace(objPrice_Question[i].objPriceQuestionModel.TableInnerHTML))
                            {
                                string[] TableRows = objPrice_Question[i].objPriceQuestionModel.TableInnerHTML.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                                for (int k = 0; k < TableRows.Length; k++)
                                {
                                    string[] TableCols = TableRows[k].Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                                    decimal InstAmt = 0;
                                    if (!string.IsNullOrWhiteSpace(TableCols[0]) && Convert.ToDecimal(TableCols[0]) > 0)
                                        InstAmt = Convert.ToDecimal(TableCols[0]);
                                    else
                                        InstAmt = 0;

                                    decimal TaxAmt = objPrice_Question[i].objPriceQuestionModel.TaxAmt;

                                    decimal IneteAmt = 0; ;
                                    if (!string.IsNullOrWhiteSpace(objPrice_Question[i].objPriceQuestionModel.FixedAmt) && Convert.ToDecimal(objPrice_Question[i].objPriceQuestionModel.FixedAmt) == 1)
                                        IneteAmt = Convert.ToDecimal(TableCols[1]);
                                    else if (!string.IsNullOrWhiteSpace(objPrice_Question[i].objPriceQuestionModel.FixedAmt) && Convert.ToDecimal(objPrice_Question[i].objPriceQuestionModel.FixedAmt) == 0)
                                        IneteAmt = Convert.ToDecimal(TableCols[1]);
                                    else if (!string.IsNullOrWhiteSpace(objPrice_Question[i].objPriceQuestionModel.FixedAmt) && Convert.ToDecimal(objPrice_Question[i].objPriceQuestionModel.FixedAmt) == 3)
                                    {
                                        if (Convert.ToDecimal(TableCols[1]) > 0)
                                            IneteAmt = InstAmt / Convert.ToDecimal(TableCols[1]);
                                        else
                                            IneteAmt = 0;
                                    }
                                    else
                                        IneteAmt = 0;

                                    decimal TotalAmt = InstAmt + TaxAmt + IneteAmt;
                                    string DateAmt = TableCols[2];
                                    string PenaltyAmt = TableCols[3];
                                    objPrice_Question[i].objPriceQuestionModel.dtPriceDetails.Add(
                                        new PriceDetailsQuestionModel
                                        {
                                            ResultDetailId = objPrice_Question[i].objPriceQuestionModel.ResultDetailId,
                                            QuestionId = objPrice_Question[i].objPriceQuestionModel.QuestionId,
                                            InstAmt = InstAmt,
                                            TaxAmt = TaxAmt,
                                            IneteAmt = IneteAmt,
                                            TotalAmt = TotalAmt,
                                            DateAmt = DateAmt,
                                            PenaltyAmt = PenaltyAmt
                                        });
                                }
                            }


                            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DocumentResponse + "/InsertUpdatePriceQuestion", objPrice_Question[i].objPriceQuestionModel);
                            objPrice_Question[i].objPriceQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<PriceQuestionModel>().Result : null;
                        }
                    }



                    if (objDocumentDetailModel.ErrorCode == 0)
                    {
                        ModelState.Clear();

                        //if Error code is 0 than set Save Success Message and Redirect  

                        //save & submit
                        if (IsRedirect)
                        {
                            //Session["ResultSucessMessage"] = objDocumentDetailModel.Result.IsCompleted ? String.Format(CommonResource.msgSubmitSuccess, "Document Response") : String.Format(CommonResource.msgSaveSuccess, "Document Response");
                            //return Json(new { url = "/User/MyDocument", JsonRequestBehavior.AllowGet });
                            return Json(new { url = "/User/SummaryView", JsonRequestBehavior.AllowGet });
                        }
                        //next preview 
                        else
                        {
                            string redirecturl = "/DocumentResponse/SaveDocumentResponse?prm=" + CommonUtils.Encrypt("userid=" + objDocumentDetailModel.UserId + "&Documentid=" + objDocumentDetailModel.DocumentID + "&pagename=" + objDocumentDetailModel.UrlPageName + "&currentpage=" + objDocumentDetailModel.CurrentPage + "");

                            return Json(new { url = redirecturl, JsonRequestBehavior.AllowGet });
                        }
                    }
                    else
                    {
                        GetDocumentResponse(objDocumentDetailModel, true);
                        //if Error Code is not 0 than set error message 
                        //objDocumentDetailModel.Message = String.Format(CommonResource.msgSaveError, "Document Response");
                        objDocumentDetailModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();

                    }

                }


            }
            catch (Exception ex)
            {
                ErrorLog(ex, "DocumentResponse", "SaveDocumentResponse Post");
                //if Error Code is not 0 than set error message 
                //objDocumentDetailModel.Message = String.Format(CommonResource.msgSaveError, "Document Response");
                objDocumentDetailModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
            }
            ModelState.AddModelError(string.Empty, "AJAX Post");
            return PartialView("_SaveDocumentQueAnsList", objDocumentDetailModel);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get Document response detail
        /// </summary>
        /// <param name="objDocumentDetailModel">object of DocumentResponseDetailModel</param>
        /// <param name="IsSave">if true then Fetch record for Save else for View.</param>
        public DocumentResponseDetailModel GetDocumentResponse(DocumentResponseDetailModel objDocumentDetailModel, bool IsSave)
        {
            if (IsSave)
            {
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DocumentResponse + "/GetDocumentResponseForSave", objDocumentDetailModel);
                objDocumentDetailModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentResponseDetailModel>().Result : null;

                //objDocumentDetailModel = objBLDocumentResponse.GetDocumentResponseForSave(objDocumentDetailModel);
            }
            else
            {
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DocumentResponse + "/GetDocumentResponseForView", objDocumentDetailModel);
                objDocumentDetailModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentResponseDetailModel>().Result : null;

                //objDocumentDetailModel = objBLDocumentResponse.GetDocumentResponseForView(objDocumentDetailModel);
            }
            if (objDocumentDetailModel != null && objDocumentDetailModel.Questions != null && objDocumentDetailModel.Questions.Count() > 0)
            {
                CommonUtils objCommonUtils = new CommonUtils();
                //set question type property
                foreach (ViewQuestionAnswerModel objViewQuestionAnswerModel in objDocumentDetailModel.Questions)
                {
                    objViewQuestionAnswerModel.QuestionTypeDetail = new QuestionTypeDetailModel();
                    if (!String.IsNullOrEmpty(objViewQuestionAnswerModel.QuestionType))
                    {
                        objViewQuestionAnswerModel.QuestionTypeDetail = objCommonUtils.SetQuestionProperties(objViewQuestionAnswerModel.QuestionType, objViewQuestionAnswerModel.QuestionPropertyList, objViewQuestionAnswerModel.QuestionTypeDetail, objViewQuestionAnswerModel.QuestionOptionsList);
                    }


                }
            }

            return objDocumentDetailModel;
        }


        #endregion
        [HttpPost]
        public void InsertStepSatus(UserDetailModel objUserDetailModel)
        {
            try
            {
                if (objUserDetailModel != null)
                {
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.DocumentResponse + "/InsertStepSatus", objUserDetailModel);
                    objUserDetailModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserDetailModel>().Result : null;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
