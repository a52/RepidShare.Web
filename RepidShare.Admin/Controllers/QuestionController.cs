using RepidShare.Entities;
using RepidShare.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RepidShare.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();
        private CommonUtils objCommonUtils = new CommonUtils();
        /// <summary>
        /// Add or Edit Question for Document
        /// </summary>
        /// <param name="prm"></param>
        /// <returns></returns>
        public ActionResult SaveQuestion(string prm)
        {
            int DocumentId, DocumentQuestionId, StepID;
            ViewQuestionModel objViewQuestionModel = new ViewQuestionModel();
            QuestionDetailModel objQuestionDetailModel = new QuestionDetailModel();
            try
            {
                if (!String.IsNullOrEmpty(prm))
                {

                    string strPrm = CommonUtils.Decrypt(prm);
                    //If strPrm contain , means question is in edit mode else question is in add mode.
                    if (strPrm.Contains(",") && strPrm.Split(',').Length > 2)
                    {
                        int.TryParse(strPrm.Split(',')[0], out DocumentId);
                        int.TryParse(strPrm.Split(',')[1], out DocumentQuestionId);
                        int.TryParse(strPrm.Split(',')[2], out StepID);
                    }
                    else if (strPrm.Contains(",") && strPrm.Split(',').Length > 1)
                    {
                        int.TryParse(strPrm.Split(',')[0], out DocumentId);
                        int.TryParse(strPrm.Split(',')[1], out DocumentQuestionId);
                        StepID = 0;
                    }
                    else
                    {
                        int.TryParse(strPrm, out DocumentId);
                        //set DocumentQuestionId equal to 0 in case of Add Question.
                        DocumentQuestionId = 0;
                        StepID = 0;
                    }
                    //if Document Id is 0 means some change in parameter. then set message Document not exist and redirect to view Document page.
                    if (DocumentId == 0)
                    {
                        TempData["NoticeMessage"] = "Document Not Exist";
                        return RedirectToAction("ViewDocument", "Document");
                    }

                    objQuestionDetailModel.DocumentID = DocumentId;
                    objQuestionDetailModel.QuestionID = DocumentQuestionId;
                    objQuestionDetailModel.StepID = StepID;

                    //initial set of current page, pageSize , Total pages
                    objViewQuestionModel.CurrentPage = 1;
                    objViewQuestionModel.TotalPages = 0;

                    objViewQuestionModel.QuestionDetail = objQuestionDetailModel;
                    //Get Question Type List to fill dropdown of Question Type
                    FillQuestionType();


                    if (StepID > 0)
                    {
                        objViewQuestionModel.PageSize = int.MaxValue - 1;
                        //ViewBag.ParentQuestionList
                        List<DropdownModel> objDropdown = new List<DropdownModel>();


                        List<DropdownModel> objParentSeSession = new List<DropdownModel>();
                        //Get Question in Add or Edit Model and also show Question List based on Document and sorting paging parameters
                        serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Question + "/GetQuestions", objViewQuestionModel);
                        objViewQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewQuestionModel>().Result : null;
                        foreach (QuestionDetailModel objItem in objViewQuestionModel.QuestionsList)
                        {
                            DropdownModel objDropdownModel = new DropdownModel();
                            objDropdownModel.ID = objItem.QuestionID;
                            objDropdownModel.Value = objItem.QuestionDescription;
                            if (objItem.QuestionType == CommonUtils.QuestionType.DropDown.ToString() || objItem.QuestionType == CommonUtils.QuestionType.SingleSelect.ToString())
                            {
                                objDropdown.Add(objDropdownModel);
                                objParentSeSession.Add(new DropdownModel { ID = objItem.QuestionID, Value = objItem.ParentDDLText });
                            }
                        }

                        Session["ParentSeSession"] = null;
                        Session["ParentSeSession"] = objParentSeSession;

                        ViewBag.ParentQuestionList = new SelectList(objDropdown, "Id", "Value", objViewQuestionModel.QuestionDetail.ParentQuestion);
                        List<DropdownModel> objDropdownParent = new List<DropdownModel>();

                        if (Convert.ToInt32(objViewQuestionModel.QuestionDetail.ParentQuestion) > 0)
                        {
                            if (Session["ParentSeSession"] != null)
                            {
                                string OptionText = objParentSeSession.Where(x => x.ID == Convert.ToInt32(objViewQuestionModel.QuestionDetail.ParentQuestion)).FirstOrDefault().Value;
                                StringReader theReader = new StringReader("<XmlDS>" + OptionText + "</XmlDS>");
                                DataSet theDataSet = new DataSet();
                                theDataSet.ReadXml(theReader);

                                foreach (DataRow item in theDataSet.Tables[0].Rows)
                                {
                                    if (item["Value"].ToString() == objViewQuestionModel.QuestionDetail.ParentAnswer.ToString())
                                    {
                                        objViewQuestionModel.QuestionDetail.ParentAnswer = item["ID"].ToString();
                                    }
                                    objDropdownParent.Add(new DropdownModel
                                    {
                                        ID = Convert.ToInt32(item["ID"]),
                                        Value = item["Value"].ToString()//,
                                        //Selected = item["Value"].ToString() == objViewQuestionModel.QuestionDetail.ParentAnswer.ToString() ? true : false
                                    });
                                }
                            }
                        }
                        ViewBag.ParentAnswerList = new SelectList(objDropdownParent, "Id", "Value", objViewQuestionModel.QuestionDetail.ParentAnswer);

                        if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.DropDown.ToString())
                        {
                            DropDownValue(objViewQuestionModel.QuestionDetail.DropDownXML);
                        }
                    }
                    else
                    {
                        objViewQuestionModel.PageSize = CommonUtils.PageSize;
                        //Get Question in Add or Edit Model and also show Question List based on Document and sorting paging parameters
                        serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Question + "/GetQuestions", objViewQuestionModel);
                        objViewQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewQuestionModel>().Result : null;
                    }


                    objViewQuestionModel.QuestionTypeDetail = new QuestionTypeDetailModel();
                    //if question is in edit mode than set Question properties based on quetion type
                    if (objViewQuestionModel != null && objViewQuestionModel.QuestionDetail != null && !String.IsNullOrEmpty(objViewQuestionModel.QuestionDetail.QuestionType))
                    {

                        objViewQuestionModel.QuestionTypeDetail = objCommonUtils.SetQuestionProperties(objViewQuestionModel.QuestionDetail.QuestionType, objViewQuestionModel.QuestionPropertyList, objViewQuestionModel.QuestionTypeDetail, objViewQuestionModel.QuestionDetail.QuestionOptionsList, objViewQuestionModel.QuestionDetail.DropDownXML);

                    }
                    //if Success Message is not null and empty means question save successfully than set Success Message
                    if (TempData["QuestionSucessMessage"] != null && Convert.ToString(TempData["QuestionSucessMessage"]) != "")
                    {
                        objViewQuestionModel.Message = Convert.ToString(TempData["QuestionSucessMessage"]);
                        objViewQuestionModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                        TempData["QuestionSucessMessage"] = null;
                    }
                    else if (objViewQuestionModel.QuestionDetail.IsPublish && objViewQuestionModel.QuestionDetail.QuestionID <= 0)
                    {
                        objViewQuestionModel.Message = "Can Not Edit Document";
                        objViewQuestionModel.MessageType = CommonUtils.MessageType.Notice.ToString().ToLower();
                    }

                    StepDropDown(objViewQuestionModel.QuestionDetail.StepID, objViewQuestionModel.QuestionDetail.DocumentID);
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Question", "SaveQuestion Get");
            }
            return View("SaveQuestion", objViewQuestionModel);
        }

        /// <summary>
        /// Save Quesetion , Delete Question , paging and sorting functionality based on parameter.
        /// </summary>
        /// <param name="objViewQuestionModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        // [Filters.Authorized]
        public ActionResult SaveQuestion(ViewQuestionModel objViewQuestionModel)
        {
            try
            {

                //set Message and Message Type to empty
                objViewQuestionModel.Message = objViewQuestionModel.MessageType = String.Empty;
                int ErrorCode = 0;
                String ErrorMessage = "";
                List<DisplayChoiceModel> lstDisplayChoice = new List<DisplayChoiceModel>();
                if (!String.IsNullOrEmpty(objViewQuestionModel.ActionType))
                {
                    switch (objViewQuestionModel.ActionType)
                    {
                        case "save":
                            // if Action Type is save than set IsActive and Created By property
                            objViewQuestionModel.QuestionDetail.IsActive = true;
                            objViewQuestionModel.QuestionDetail.CreatedBy = LoggedInUserID;
                            //Get All Display Choice for single select and Multi select question type
                            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Question + "/GetAllDisplayChoice");
                            lstDisplayChoice = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<DisplayChoiceModel>>().Result : null;

                            //lstDisplayChoice = objCommonUtils.GetAllDisplayChoice();
                            // if question type is single select than set question Option List and get XML of Question Options and set display choice ID(radio)
                            if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.SingleSelect.ToString())
                            {
                                objViewQuestionModel.QuestionDetail.QuestionOptionsList = objViewQuestionModel.QuestionTypeDetail.SingleSelect.QuestionOptions;
                                objViewQuestionModel.QuestionOptions = CommonUtils.GetBulkXML(objViewQuestionModel.QuestionDetail.QuestionOptionsList);
                                objViewQuestionModel.QuestionDetail.DisplayChoiceID = lstDisplayChoice.Where(o => o.DisplayChoiceName == CommonUtils.DisplayChoice.Radio.ToString()).Select(o => o.DisplayChoiceID).SingleOrDefault();
                            }
                            else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.MultiSelect.ToString())
                            {
                                // if question type is Multi select than set question Option List and get XML of Question Options and set display choice ID(checkbox)
                                objViewQuestionModel.QuestionDetail.QuestionOptionsList = objViewQuestionModel.QuestionTypeDetail.MultiSelect.QuestionOptions;
                                objViewQuestionModel.QuestionOptions = CommonUtils.GetBulkXML(objViewQuestionModel.QuestionDetail.QuestionOptionsList);
                                objViewQuestionModel.QuestionDetail.DisplayChoiceID = lstDisplayChoice.Where(o => o.DisplayChoiceName == CommonUtils.DisplayChoice.CheckBox.ToString()).Select(o => o.DisplayChoiceID).SingleOrDefault();
                            }
                            //Fill Question Properties based on QuestionTypeDetail Model
                            objViewQuestionModel.QuestionPropertyList = objCommonUtils.FillQuestionProperties(objViewQuestionModel.QuestionTypeDetail);
                            if (objViewQuestionModel.QuestionPropertyList != null && objViewQuestionModel.QuestionPropertyList.Count > 0)
                            {
                                //If QuestionPropertyList count greater than 0 get xml of Question Property List
                                objViewQuestionModel.QuestionProperties = CommonUtils.GetBulkXML(objViewQuestionModel.QuestionPropertyList);
                            }



                            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Question + "/InsertUpdateQuestion", objViewQuestionModel);
                            objViewQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewQuestionModel>().Result : null;

                            //Insert or Update Document Question with Question Type ,Question Type Detail and Question Property
                            //objViewQuestionModel = objBLQuestion.InsertUpdateQuestion(objViewQuestionModel);

                            if (objViewQuestionModel.ErrorCode == 0)
                            {
                                //if Error code is 0 than set Save Success Message and Redirect to same page in Add Question Mode
                                TempData["QuestionSucessMessage"] = "Question Saved";
                                return RedirectToAction("SaveQuestion", new { prm = CommonUtils.Encrypt(Convert.ToString(objViewQuestionModel.QuestionDetail.DocumentID)) });
                            }
                            else
                            {
                                //if Error Code is not 0 than set error message 
                                objViewQuestionModel.Message = "Question Error";
                                objViewQuestionModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();

                            }
                            break;
                        case "deletequestion":

                            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Question + "/DeleteQuestion", objViewQuestionModel);
                            objViewQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewQuestionModel>().Result : null;

                            //objBLQuestion.DeleteDocumentQuestion(objViewQuestionModel.DeletedQuestionID, LoggedInUserID, out ErrorCode, out ErrorMessage);

                            if (Convert.ToInt32(ErrorCode).Equals(0))
                            {
                                //if error code 0 means delete successfully than set Delete success message.
                                TempData["QuestionSucessMessage"] = "Question Delete";
                                return RedirectToAction("SaveQuestion", new { prm = CommonUtils.Encrypt(Convert.ToString(objViewQuestionModel.QuestionDetail.DocumentID)) });
                            }
                            else
                            {
                                //if error code is not 0 means delete error  than set Delete error message.
                                objViewQuestionModel.Message = "Question Error";
                                objViewQuestionModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                            }
                            ModelState.Clear();
                            break;
                        default:
                            objViewQuestionModel.QuestionDetail.QuestionID = 0;
                            //In case of paging and sorting  clease Model State to Remove validation.
                            ModelState.Clear();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Question", "SaveQuestion Post");
            }
            //Fill Question Type drop down and Get QuestionList if error code is not 0 and return view.  
            FillQuestionType();

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Question + "/GetQuestions", objViewQuestionModel);
            objViewQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewQuestionModel>().Result : null;

            //objViewQuestionModel = objBLQuestion.GetQuestions(objViewQuestionModel);
            return View("SaveQuestion", objViewQuestionModel);
        }

        /// <summary>
        /// Fill Question Type Drop down
        /// </summary>
        private void FillQuestionType()
        {
            try
            {
                List<QuestionTypeModel> lstQuestionTypeModel = new List<QuestionTypeModel>();

                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Question + "/GetQuesetionTypeList");
                lstQuestionTypeModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<QuestionTypeModel>>().Result : null;

                //lstQuestionTypeModel = objBLQuestionType.GetQuesetionTypeList();
                ViewBag.QuestionTypeList = new SelectList(lstQuestionTypeModel, "QuestionType", "QuestionTypeName");
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Question", "FillQuestionType");
                throw ex;
            }
        }

        /// <summary>
        /// Get Question Type Detail when change Question Type dropdown
        /// </summary>
        /// <param name="objViewQuestionModel">object of Model ViewQuestionModel </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQuestionTypeDetail(ViewQuestionModel objViewQuestionModel)
        {
            try
            {
                objViewQuestionModel.QuestionTypeDetail = new QuestionTypeDetailModel();
                //if Question Type is Single Line than set SingleLineTextType Model and Return Partial view _AddSingleLineText
                if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.SingleLine.ToString())
                {
                    objViewQuestionModel.QuestionTypeDetail.SingleLineTextType = new SingleLineModel();
                    return PartialView("_AddSingleLineText", objViewQuestionModel);
                }
                else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.MultiLine.ToString())
                {
                    //if Question Type is Multi Line than set MultiLineTextType Model and Return Partial view _AddMultiLineText
                    objViewQuestionModel.QuestionTypeDetail.MultiLineTextType = new MultiLineModel();
                    return PartialView("_AddMultiLineText", objViewQuestionModel);
                }
                else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.Number.ToString())
                {
                    //if Question Type is Number than set NumberType Model and Return Partial view _AddNumber
                    objViewQuestionModel.QuestionTypeDetail.NumberType = new NumberModel();
                    return PartialView("_AddNumber", objViewQuestionModel);
                }
                else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.SingleSelect.ToString())
                {
                    //if Question Type is SingleSelect than set SingleSelect Model and Return Partial view _AddSingleSelect
                    objViewQuestionModel.QuestionTypeDetail.SingleSelect = new SingleSelectModel();
                    //Add Default four Options in Question Option List
                    objViewQuestionModel.QuestionTypeDetail.SingleSelect.QuestionOptions = AddDefaultOptions();
                    return PartialView("_AddSingleSelect", objViewQuestionModel);
                }
                else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.MultiSelect.ToString())
                {
                    //if Question Type is MultiSelect than set MultiSelect Model and Return Partial view _AddMultiSelect
                    objViewQuestionModel.QuestionTypeDetail.MultiSelect = new MultiSelectModel();
                    //Add Default four Options in Question Option List
                    objViewQuestionModel.QuestionTypeDetail.MultiSelect.QuestionOptions = AddDefaultOptions();
                    return PartialView("_AddMultiSelect", objViewQuestionModel);
                }
                else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.DateAndTime.ToString())
                {
                    //if Question Type is Date And Time than set DateAndTimeType Model and Return Partial view _AddDateAndTime
                    objViewQuestionModel.QuestionTypeDetail.DateAndTimeType = new DateAndTimeModel();
                    return PartialView("_AddDateAndTime", objViewQuestionModel);
                }
                else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.DropDown.ToString())
                {
                    DropDownValue();
                    objViewQuestionModel.QuestionTypeDetail.DropDownType = new SingleLineModel();
                    return PartialView("_AddDropDown", objViewQuestionModel);
                }
                else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.Price_Question.ToString())
                {
                    //DropDownValue();
                    objViewQuestionModel.QuestionTypeDetail.SingleLineTextType = new SingleLineModel();
                    return PartialView("_AddSingleLineText", objViewQuestionModel);
                }
                else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.Price.ToString())
                {
                    //DropDownValue();
                    objViewQuestionModel.QuestionTypeDetail.SingleLineTextType = new SingleLineModel();
                    return PartialView("_AddSingleLineText", objViewQuestionModel);
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Question", "GetQuestionTypeDetail");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Added default 4  question options in  case of single select or Multi select Question  Type
        /// </summary>
        /// <returns></returns>
        private List<QuestionOptionsModel> AddDefaultOptions()
        {
            List<QuestionOptionsModel> lstDocumentQuesetionOptions = new List<QuestionOptionsModel>();
            for (int i = 0; i < 4; i++)
            {
                QuestionOptionsModel objQuestionOptionModel = new QuestionOptionsModel();
                lstDocumentQuesetionOptions.Add(objQuestionOptionModel);
            }
            return lstDocumentQuesetionOptions;
        }

        /// <summary>
        /// Add New Question Option in case of Single Select or MultiSelect Question Type  Add button click
        /// </summary>
        /// <param name="objViewQuestionModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewSelection(ViewQuestionModel objViewQuestionModel)
        {
            try
            {
                if (objViewQuestionModel != null && objViewQuestionModel.QuestionTypeDetail != null)
                {
                    //Add New Option in single Select Option Type 
                    if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.SingleSelect.ToString())
                    {
                        QuestionOptionsModel objQuestionOptionModel = new QuestionOptionsModel();
                        objViewQuestionModel.QuestionTypeDetail.SingleSelect.QuestionOptions.Add(objQuestionOptionModel);
                        return PartialView("_AddSingleSelect", objViewQuestionModel);
                    }
                    else if (objViewQuestionModel.QuestionDetail.QuestionType == CommonUtils.QuestionType.MultiSelect.ToString())
                    {
                        //Add New Option in Multi Select Option Type 
                        QuestionOptionsModel objQuestionOptionModel = new QuestionOptionsModel();
                        objViewQuestionModel.QuestionTypeDetail.MultiSelect.QuestionOptions.Add(objQuestionOptionModel);
                        return PartialView("_AddMultiSelect", objViewQuestionModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Question", "AddNewSelection");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Refresh question list based on paging and sorting
        /// </summary>
        /// <param name="objViewQuestionModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RefreshQuestionList(ViewQuestionModel objViewQuestionModel)
        {
            try
            {
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Question + "/GetQuestions", objViewQuestionModel);
                objViewQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewQuestionModel>().Result : null;

                // objViewQuestionModel = objBLQuestion.GetQuestions(objViewQuestionModel);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Question", "RefreshQuestionList");
            }
            return PartialView("_ViewQuestionList", objViewQuestionModel);
        }

        [HttpPost]
        public ActionResult RedirectURL(ViewQuestionModel objViewQuestionModel)
        {
            try
            {
                return Json(RepidShare.Utility.CommonUtils.Encrypt(objViewQuestionModel.QuestionDetail.DocumentID.ToString() + "," + objViewQuestionModel.QuestionDetail.QuestionID.ToString() + "," + objViewQuestionModel.QuestionDetail.StepID.ToString()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Question", "AddNewSelection");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMappingList(int DocumentId)
        {
            ViewQuestionModel objViewQuestionModel = new ViewQuestionModel();
            QuestionDetailModel objQuestionDetailModel = new QuestionDetailModel();

            objQuestionDetailModel.DocumentID = DocumentId;
            objQuestionDetailModel.StepID = 0;

            //initial set of current page, pageSize , Total pages
            objViewQuestionModel.CurrentPage = 1;
            objViewQuestionModel.TotalPages = 0;


            objViewQuestionModel.QuestionDetail = objQuestionDetailModel;
            objViewQuestionModel.QuestionDetail.ID = DocumentId;
            //Get Question Type List to fill dropdown of Question Type
            FillQuestionType();

            objViewQuestionModel.PageSize = int.MaxValue - 1;
            //ViewBag.ParentQuestionList
            List<DropdownModel> objDropdown = new List<DropdownModel>();
            List<DropdownModel> objDropdownParent = new List<DropdownModel>();
            ViewBag.ParentAnswerList = new SelectList(objDropdownParent, "Id", "Value");

            List<DropdownModel> objParentSeSession = new List<DropdownModel>();
            //Get Question in Add or Edit Model and also show Question List based on Document and sorting paging parameters
            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Question + "/GetQuestions", objViewQuestionModel);
            objViewQuestionModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewQuestionModel>().Result : null;
            foreach (QuestionDetailModel objItem in objViewQuestionModel.QuestionsList)
            {
                DropdownModel objDropdownModel = new DropdownModel();
                objDropdownModel.ID = objItem.QuestionID;
                objDropdownModel.Value = objItem.QuestionDescription;
                if (objItem.QuestionType == CommonUtils.QuestionType.DropDown.ToString() || objItem.QuestionType == CommonUtils.QuestionType.SingleSelect.ToString())
                {
                    objParentSeSession.Add(new DropdownModel { ID = objItem.QuestionID, Value = objItem.ParentDDLText });
                }
                objDropdown.Add(objDropdownModel);
            }

            Session["ParentSeSession"] = null;
            Session["ParentSeSession"] = objParentSeSession;

            ViewBag.ParentQuestionList = new SelectList(objDropdown, "Id", "Value");

            return this.Json(ViewBag.ParentQuestionList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ParentAnswerList(int QuestionId)
        {
            List<DropdownModel> objParentSeSession = new List<DropdownModel>();
            if (Session["ParentSeSession"] != null)
            {
                string OptionText = ((List<DropdownModel>)Session["ParentSeSession"]).Where(x => x.ID == QuestionId).FirstOrDefault().Value;
                StringReader theReader = new StringReader("<XmlDS>" + OptionText + "</XmlDS>");
                DataSet theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);

                foreach (DataRow item in theDataSet.Tables[0].Rows)
                {
                    objParentSeSession.Add(new DropdownModel
                    {
                        ID = Convert.ToInt32(item["ID"]),
                        Value = item["Value"].ToString()//,
                        //Selected = item["QuestionDropDownID"].ToString() == objQuestionAnswerModel.AnswerDetail.ToString() ? true : false
                    });
                }
            }
            return Json(new SelectList(objParentSeSession, "Id", "Value"), JsonRequestBehavior.AllowGet);
        }
    }
}
