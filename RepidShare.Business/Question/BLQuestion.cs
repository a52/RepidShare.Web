using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities;
using System.Data;
using RepidShare.Data;


namespace RepidShare.Business
{
    public class BLQuestion : BLBase
    {
        private DLQuestion objDLQuestion = new DLQuestion();
        /// <summary>
        /// Get Question Detail with Question Properties and Options, Questions List  based on 
        /// </summary>
        /// <param name="objViewQuestionModel"></param>
        /// <returns></returns>
        public ViewQuestionModel GetQuestions(ViewQuestionModel objViewQuestionModel)
        {
            try
            {
                List<QuestionPropertyModel> lstQuestionPropertyModel = new List<QuestionPropertyModel>();
                List<QuestionOptionsModel> lstQuestionOptionsModel = new List<QuestionOptionsModel>();
                //get Question Detail with Properties and its values.
                DataTable dtQuesetionDetail = objDLQuestion.GetQuestionDetail(objViewQuestionModel.QuestionDetail.ID, objViewQuestionModel.QuestionDetail.QuestionID);

                if (dtQuesetionDetail != null && dtQuesetionDetail.Rows.Count > 0)
                {
                    //fill  Question detail Model && Temp Changes
                    objViewQuestionModel.QuestionDetail = GetDataRowToEntity<QuestionDetailModel>(dtQuesetionDetail.Rows[0]);

                    //Fill Question Property List
                    for (int i = 0; i < dtQuesetionDetail.Rows.Count; i++)
                    {
                        QuestionPropertyModel objQuestionPropertyModel = new QuestionPropertyModel();
                        QuestionOptionsModel objQuestionOptionsModel = new QuestionOptionsModel();
                        objQuestionPropertyModel = GetDataRowToEntity<QuestionPropertyModel>(dtQuesetionDetail.Rows[i]);
                        //Fill Question Options
                        objQuestionOptionsModel = GetDataRowToEntity<QuestionOptionsModel>(dtQuesetionDetail.Rows[i]);
                        if (objQuestionPropertyModel != null && objQuestionPropertyModel.QuestionPropertyID > 0)
                        {
                            //Add Question Property in List lstQuestionPropertyModel
                            lstQuestionPropertyModel.Add(objQuestionPropertyModel);
                        }
                        if (objQuestionOptionsModel != null && objQuestionOptionsModel.QuestionOptionsID > 0)
                        {
                            //Add Question Options in List lstQuestionOptionsModel
                            lstQuestionOptionsModel.Add(objQuestionOptionsModel);
                        }
                    }
                }
                //set QuestionPropertyList in ViewQuestionModel object
                objViewQuestionModel.QuestionPropertyList = lstQuestionPropertyModel;
                //set QuestionOptionsList in ViewQuestionModel object
                objViewQuestionModel.QuestionDetail.QuestionOptionsList = lstQuestionOptionsModel;
                //Get All Questions based on  Application Mapping and sorting and paging parameters
                objViewQuestionModel = GetQuestionsList(objViewQuestionModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objViewQuestionModel;
        }

        /// <summary>
        /// Get Question List based on  and sorting and paging parameters.
        /// </summary>
        /// <param name="objViewQuestionModel"></param>
        /// <returns></returns>
        public ViewQuestionModel GetQuestionsList(ViewQuestionModel objViewQuestionModel)
        {
            List<QuestionDetailModel> lstQuestionDetailModel = new List<QuestionDetailModel>();
            try
            {
                objViewQuestionModel.SortBy = objViewQuestionModel.SortBy ?? String.Empty;

                //Get All Questions based on  Application Mapping and sorting and paging parameters
                DataTable dtQuestion = objDLQuestion.GetQuestionsList(objViewQuestionModel);

                foreach (DataRow dr in dtQuestion.Rows)
                {

                    QuestionDetailModel itemQuestionDetailModel = GetDataRowToEntity<QuestionDetailModel>(dr);
                    lstQuestionDetailModel.Add(itemQuestionDetailModel);
                }

                objViewQuestionModel.QuestionsList = lstQuestionDetailModel;
                //set Paging based on Question List
                if (objViewQuestionModel != null && objViewQuestionModel.QuestionsList != null && objViewQuestionModel.QuestionsList.Count > 0)
                {
                    int totalRecord = objViewQuestionModel.QuestionsList[0].TotalCount;

                    if (decimal.Remainder(totalRecord, objViewQuestionModel.PageSize) > 0)
                        objViewQuestionModel.TotalPages = (totalRecord / objViewQuestionModel.PageSize + 1);
                    else
                        objViewQuestionModel.TotalPages = totalRecord / objViewQuestionModel.PageSize;


                }
                else
                {
                    objViewQuestionModel.TotalPages = 0;
                    objViewQuestionModel.CurrentPage = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objViewQuestionModel;
        }

        /// <summary>
        /// Insert or Update  Question 
        /// </summary>
        /// <param name="objViewQuestionModel">Object of Model ViewQuestionModel</param>
        /// <returns></returns>
        public ViewQuestionModel InsertUpdateQuestion(ViewQuestionModel objViewQuestionModel)
        {
            try
            {
                //call InsertUpdateQuestion Method of dataLayer and return ViewQuestionModel
                return objDLQuestion.InsertUpdateQuestion(objViewQuestionModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete  Question
        /// </summary>
        public ViewQuestionModel DeleteQuestion(ViewQuestionModel objViewQuestionModel)
        {
            //Delete  Category by DeletedCategoryID 
            return objDLQuestion.DeleteQuestion(objViewQuestionModel);
        }
    }
}
