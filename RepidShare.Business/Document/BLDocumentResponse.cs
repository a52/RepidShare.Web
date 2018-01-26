
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities;
using System.Data;
using RepidShare.Data;
using RepidShare.Entities.DocumentResponse;

namespace RepidShare.Business
{
    public class BLDocumentResponse : BLBase
    {
        /// <summary>
        /// Get  Document Response For Save
        /// </summary>
        /// <param name="ObjDocumentApplicationMappingDetail"></param>
        /// <returns></returns>
        public DocumentResponseDetailModel GetDocumentResponseForSave(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {

            DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();
            //Get Document detail into datatable 
            DataSet dtDocument = objDLDocumentResponse.GetDocumentResponseForSave(objDocumentResponseDetailModel);

            if (dtDocument != null && dtDocument.Tables.Count > 0 && dtDocument.Tables[0].Rows.Count > 0)
            {
                int totalRecord = Convert.ToInt32(dtDocument.Tables[0].Rows[0]["TotalCount"]);
                //calculating total paging 
                objDocumentResponseDetailModel.TotalPages = TotalPage(totalRecord, 1);
                objDocumentResponseDetailModel.MaxPage = Convert.ToInt32(dtDocument.Tables[0].Rows[0]["MaxPage"]);
            }
            if (dtDocument != null && dtDocument.Tables.Count > 1 && dtDocument.Tables[1].Rows.Count > 0)
            {

                if (objDocumentResponseDetailModel != null)
                {

                    //2. fill Document question details
                    objDocumentResponseDetailModel.Questions = new List<ViewQuestionAnswerModel>();

                    for (int i = 0; i < dtDocument.Tables[1].Rows.Count; i++)
                    {
                        ViewQuestionAnswerModel objQuestionModel = new ViewQuestionAnswerModel();
                        //get question from list
                        objQuestionModel = GetDataRowToEntity<ViewQuestionAnswerModel>(dtDocument.Tables[1].Rows[i]);

                        if (objQuestionModel != null && !objDocumentResponseDetailModel.Questions.Any(a => a.QuestionID == objQuestionModel.QuestionID))
                        {

                            DataView ViewQuestion = new DataView(dtDocument.Tables[1]);
                            //filter Document question into Data view by question id.
                            ViewQuestion.RowFilter = "QuestionID=" + objQuestionModel.QuestionID;
                            DataTable dtFilter = ViewQuestion.ToTable();
                            objQuestionModel.QuestionOptionsList = new List<QuestionOptionsModel>();
                            objQuestionModel.QuestionPropertyList = new List<QuestionPropertyModel>();
                            for (int j = 0; j < dtFilter.Rows.Count; j++)
                            {
                                //fill question option detail

                                QuestionOptionsModel objQuestionOptionModel = new QuestionOptionsModel();
                                objQuestionOptionModel = GetDataRowToEntity<QuestionOptionsModel>(dtFilter.Rows[j]);
                                if (objQuestionOptionModel != null && objQuestionOptionModel.QuestionOptionsID > 0)
                                {
                                    objQuestionModel.QuestionOptionsList.Add(objQuestionOptionModel);
                                    if (objQuestionOptionModel.IsSelected)
                                    {
                                        //Set single and multi selection detail.
                                        if (!String.IsNullOrWhiteSpace(objQuestionModel.SelectedAnswers))
                                        {

                                            objQuestionModel.SelectedAnswers = objQuestionModel.SelectedAnswers + "," + Convert.ToString(objQuestionOptionModel.QuestionOptionsID);
                                        }
                                        else
                                        {
                                            objQuestionModel.SelectedAnswers = Convert.ToString(objQuestionOptionModel.QuestionOptionsID);
                                        }
                                    }
                                }
                                //fill question property
                                QuestionPropertyModel objQuestionPropertyModel = new QuestionPropertyModel();
                                objQuestionPropertyModel = GetDataRowToEntity<QuestionPropertyModel>(dtFilter.Rows[j]);
                                if (objQuestionPropertyModel != null && !objQuestionModel.QuestionPropertyList.Any(a => a.QuestionTypeID == objQuestionPropertyModel.QuestionTypeID && a.QuestionPropertyID == objQuestionPropertyModel.QuestionPropertyID))
                                {
                                    objQuestionModel.QuestionPropertyList.Add(objQuestionPropertyModel);
                                }
                            }
                            objDocumentResponseDetailModel.Questions.Add(objQuestionModel);
                        }
                    }

                    //fill Document result detail
                    objDocumentResponseDetailModel.Result = GetDataRowToEntity<DocumentResultModel>(dtDocument.Tables[1].Rows[0]);

                }
            }
            return objDocumentResponseDetailModel;
        }

        /// <summary>
        /// Get  Document Response For view
        /// </summary>
        /// <param name="ObjDocumentApplicationMappingDetail"></param>
        /// <returns></returns>
        public DocumentResponseDetailModel GetDocumentResponseForView(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {

            DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();
            //Get Document response detail with Document ,question , question property , question option and Document result.
            DataTable dtDocument = objDLDocumentResponse.GetDocumentResponseForView(objDocumentResponseDetailModel);
            if (dtDocument != null && dtDocument.Rows.Count > 0)
            {
                if (objDocumentResponseDetailModel != null)
                {
                    //fill Document master detail 
                    int maxAttempt = 0;
                    int noOfAttempt = 0;
                    int.TryParse(Convert.ToString(dtDocument.Rows[0]["MaxNoOfAttempt"]), out maxAttempt);
                    objDocumentResponseDetailModel.MaxNoOfAttempt = maxAttempt;

                    int.TryParse(Convert.ToString(dtDocument.Rows[0]["NoOfAttempt"]), out noOfAttempt);
                    objDocumentResponseDetailModel.NoOfAttempt = noOfAttempt;
                    //1. Fill Document detail  and application mapping detail

                    //objDocumentResponseDetailModel.DocumentApplicationDetail = GetDataRowToEntity<DocumentApplicationMappingModel>(dtDocument.Rows[0]);

                    //2. fill Document question details
                    objDocumentResponseDetailModel.Questions = new List<ViewQuestionAnswerModel>();

                    for (int i = 0; i < dtDocument.Rows.Count; i++)
                    {
                        ViewQuestionAnswerModel objQuestionModel = new ViewQuestionAnswerModel();
                        //get question from list
                        objQuestionModel = GetDataRowToEntity<ViewQuestionAnswerModel>(dtDocument.Rows[i]);

                        if (objQuestionModel != null && !objDocumentResponseDetailModel.Questions.Any(a => a.QuestionID == objQuestionModel.QuestionID))
                        {

                            DataView ViewQuestion = new DataView(dtDocument);
                            //Filter question into view
                            ViewQuestion.RowFilter = "QuestionID=" + objQuestionModel.QuestionID;
                            DataTable dtFilter = ViewQuestion.ToTable();
                            objQuestionModel.QuestionOptionsList = new List<QuestionOptionsModel>();
                            objQuestionModel.QuestionPropertyList = new List<QuestionPropertyModel>();
                            for (int j = 0; j < dtFilter.Rows.Count; j++)
                            {
                                //fill question option detail
                                QuestionOptionsModel objQuestionOptionModel = new QuestionOptionsModel();
                                objQuestionOptionModel = GetDataRowToEntity<QuestionOptionsModel>(dtFilter.Rows[j]);
                                if (objQuestionOptionModel != null && objQuestionOptionModel.QuestionOptionsID > 0)
                                {
                                    objQuestionModel.QuestionOptionsList.Add(objQuestionOptionModel);
                                    //get detail of single/multi selection
                                    if (objQuestionOptionModel.IsSelected)
                                    {
                                        if (!String.IsNullOrWhiteSpace(objQuestionModel.SelectedAnswers))
                                        {
                                            objQuestionModel.SelectedAnswers = objQuestionModel.SelectedAnswers + "," + Convert.ToString(objQuestionOptionModel.QuestionOptionsID);
                                        }
                                        else
                                        {
                                            objQuestionModel.SelectedAnswers = Convert.ToString(objQuestionOptionModel.QuestionOptionsID);
                                        }
                                    }
                                }
                                //fill question property
                                QuestionPropertyModel objQuestionPropertyModel = new QuestionPropertyModel();
                                objQuestionPropertyModel = GetDataRowToEntity<QuestionPropertyModel>(dtFilter.Rows[j]);
                                if (objQuestionPropertyModel != null && !objQuestionModel.QuestionPropertyList.Any(a => a.QuestionTypeID == objQuestionPropertyModel.QuestionTypeID && a.QuestionPropertyID == objQuestionPropertyModel.QuestionPropertyID))
                                {
                                    objQuestionModel.QuestionPropertyList.Add(objQuestionPropertyModel);
                                }
                            }
                            objDocumentResponseDetailModel.Questions.Add(objQuestionModel);
                        }
                    }

                    //fill result detail
                    objDocumentResponseDetailModel.Result = GetDataRowToEntity<DocumentResultModel>(dtDocument.Rows[0]);

                    int totalRecord = Convert.ToInt32(dtDocument.Rows[0]["TotalCount"]);
                    //calculating total paging 
                    objDocumentResponseDetailModel.TotalPages = TotalPage(totalRecord, objDocumentResponseDetailModel.PageSize);
                }
            }
            return objDocumentResponseDetailModel;
        }

        /// <summary>
        /// Get Document response detail
        /// </summary>
        /// <param name="ObjDocumentApplicationMappingDetail"></param>
        /// <returns></returns>
        public DocumentResponseDetailModel GetDocumentPreview(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            //objDocumentResponseDetailModel.DocumentApplicationDetail = new DocumentApplicationMappingModel();
            DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();
            //Get Document with question detail into datatable
            DataTable dtDocument = objDLDocumentResponse.GetDocumentPreview(objDocumentResponseDetailModel);
            if (dtDocument != null && dtDocument.Rows.Count > 0)
            {
                if (objDocumentResponseDetailModel != null)
                {
                    //1. Fill Document detail  and application mapping detail

                    //objDocumentResponseDetailModel.DocumentApplicationDetail = GetDataRowToEntity<DocumentApplicationMappingModel>(dtDocument.Rows[0]);

                    //2. fill Document question details
                    objDocumentResponseDetailModel.Questions = new List<ViewQuestionAnswerModel>();

                    for (int i = 0; i < dtDocument.Rows.Count; i++)
                    {
                        ViewQuestionAnswerModel objQuestionModel = new ViewQuestionAnswerModel();
                        //get question from list
                        objQuestionModel = GetDataRowToEntity<ViewQuestionAnswerModel>(dtDocument.Rows[i]);

                        if (objQuestionModel != null && !objDocumentResponseDetailModel.Questions.Any(a => a.QuestionID == objQuestionModel.QuestionID))
                        {
                            DataView ViewQuestion = new DataView(dtDocument);
                            ViewQuestion.RowFilter = "QuestionID=" + objQuestionModel.QuestionID;
                            DataTable dtFilter = ViewQuestion.ToTable();
                            objQuestionModel.QuestionOptionsList = new List<QuestionOptionsModel>();
                            objQuestionModel.QuestionPropertyList = new List<QuestionPropertyModel>();
                            for (int j = 0; j < dtFilter.Rows.Count; j++)
                            {
                                //fill question option detail
                                QuestionOptionsModel objQuestionOptionModel = new QuestionOptionsModel();
                                objQuestionOptionModel = GetDataRowToEntity<QuestionOptionsModel>(dtFilter.Rows[j]);
                                if (objQuestionOptionModel != null && objQuestionOptionModel.QuestionOptionsID > 0)
                                {
                                    objQuestionModel.QuestionOptionsList.Add(objQuestionOptionModel);
                                    if (objQuestionOptionModel.IsSelected)
                                    {
                                        if (!String.IsNullOrWhiteSpace(objQuestionModel.SelectedAnswers))
                                        {
                                            objQuestionModel.SelectedAnswers = objQuestionModel.SelectedAnswers + "," + Convert.ToString(objQuestionOptionModel.QuestionOptionsID);
                                        }
                                        else
                                        {
                                            objQuestionModel.SelectedAnswers = Convert.ToString(objQuestionOptionModel.QuestionOptionsID);
                                        }
                                    }
                                }
                                //fill question property
                                QuestionPropertyModel objQuestionPropertyModel = new QuestionPropertyModel();
                                objQuestionPropertyModel = GetDataRowToEntity<QuestionPropertyModel>(dtFilter.Rows[j]);
                                if (objQuestionPropertyModel != null && !objQuestionModel.QuestionPropertyList.Any(a => a.QuestionTypeID == objQuestionPropertyModel.QuestionTypeID && a.QuestionPropertyID == objQuestionPropertyModel.QuestionPropertyID))
                                {
                                    objQuestionModel.QuestionPropertyList.Add(objQuestionPropertyModel);
                                }
                            }
                            objDocumentResponseDetailModel.Questions.Add(objQuestionModel);
                        }
                    }
                }
            }
            return objDocumentResponseDetailModel;
        }

        /// <summary>
        /// Insert or Update Document Answer 
        /// </summary>
        /// <param name="objViewQuestionModel">Object of Model ViewQuestionModel</param>
        /// <returns></returns>
        public DocumentResponseDetailModel InsertUpdateDocumentResult(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            try
            {
                DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();
                //call InsertUpdateDocumentQuestion Method of dataLayer and return DocumentResponseDetailModel
                return objDLDocumentResponse.InsertUpdateDocumentResult(objDocumentResponseDetailModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GetDocumentPreviewTemp(int DocumentId, int UserId)
        {
            DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();
            return objDLDocumentResponse.GetDocumentPreviewTemp(DocumentId, UserId);
        }

        public List<ActivityModel> GetUserDocumentHistory(int DocumentId, int UserId)
        {
            DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();
            List<ActivityModel> objListActivityModel = new List<ActivityModel>();

            DataTable dt = objDLDocumentResponse.GetDocumentHistory(DocumentId, UserId);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objListActivityModel.Add(GetDataRowToEntity<ActivityModel>(dt.Rows[i]));
            }
            return objListActivityModel;
        }

        #region Price Question
        public PriceQuestionModel InsertUpdatePriceQuestion(PriceQuestionModel objPriceQuestionModel)
        {
            DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();
            return objDLDocumentResponse.InsertUpdatePriceQuestion(objPriceQuestionModel);
        }

        public PriceQuestionModel GetPriceQuestion(PriceQuestionModel objPriceQuestionModel)
        {
            DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();
            DataSet dt = objDLDocumentResponse.GetPriceQuestion(objPriceQuestionModel);
            if (dt.Tables.Count > 0)
            {
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    objPriceQuestionModel = GetDataRowToEntity<PriceQuestionModel>(dt.Tables[0].Rows[i]);
                }
            }
            if (objPriceQuestionModel != null)
            {
                objPriceQuestionModel.dtPriceDetails = new List<PriceDetailsQuestionModel>();
                if (dt.Tables.Count > 1)
                {
                    for (int i = 0; i < dt.Tables[1].Rows.Count; i++)
                    {
                        objPriceQuestionModel.dtPriceDetails.Add(GetDataRowToEntity<PriceDetailsQuestionModel>(dt.Tables[1].Rows[i]));
                    }
                }
            }

            return objPriceQuestionModel;
        }
        #endregion

        public void InsertStepSatus(UserDetailModel objobjUserDetailModel)
        {
            try
            {
                DLDocumentResponse objDLDocumentResponse = new DLDocumentResponse();               
               objDLDocumentResponse.InsertStepSatus(objobjUserDetailModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
