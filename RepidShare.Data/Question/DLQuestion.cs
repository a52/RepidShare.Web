using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLQuestion
    {
        /// <summary>
        /// Get Question Detail by Suvey Id and Suvery Question Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public DataTable GetQuestionDetail(int DocumentID, int QuestionId)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@DocumentID",DocumentID)
                                        ,new SqlParameter("@QuestionID",QuestionId)                                        
                                      };
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_GETQUESTIONDETAILBYID, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Question List based on ApplicationId and sorting, paging parameters
        /// </summary>
        /// <param name="objViewQuestionModel"></param>
        /// <returns></returns>
        public DataTable GetQuestionsList(ViewQuestionModel objViewQuestionModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                       new SqlParameter("@DocumentID", objViewQuestionModel.QuestionDetail.DocumentID)
                                       ,new SqlParameter("@StepID", objViewQuestionModel.QuestionDetail.StepID)
                                      ,new SqlParameter("CurrentPage", objViewQuestionModel.CurrentPage)
                                     ,new SqlParameter("PageSize", objViewQuestionModel.PageSize)
                                     ,new SqlParameter("SortBy", objViewQuestionModel.SortBy)
                                     ,new SqlParameter("SortOrder", objViewQuestionModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_GETQUESTIONLIST, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert or Update  Question with Question Properties and Options
        /// </summary>
        /// <param name="objViewQuestionModel"></param>
        /// <returns></returns>
        public ViewQuestionModel InsertUpdateQuestion(ViewQuestionModel objViewQuestionModel)
        {
            try
            {
                string DropDownValue = string.Empty;
                if (objViewQuestionModel != null && objViewQuestionModel.QuestionTypeDetail != null && objViewQuestionModel.QuestionTypeDetail.DropDownType != null)
                {
                    DropDownValue = objViewQuestionModel.QuestionTypeDetail.DropDownType.DropDownValue;
                }
                objViewQuestionModel.QuestionDetail.QuestionDescription = objViewQuestionModel.QuestionDetail.QuestionDescription.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;
                pErrorMessage.Size = 5000;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@QuestionID",objViewQuestionModel.QuestionDetail.QuestionID)
                                        ,new SqlParameter("@DocumentID", objViewQuestionModel.QuestionDetail.DocumentID)
                                       ,new SqlParameter("@StepID", objViewQuestionModel.QuestionDetail.StepID)
                                        ,new SqlParameter("@QuestionDescription",objViewQuestionModel.QuestionDetail.QuestionDescription)
                                        ,new SqlParameter("@ParentQuestionIDs",objViewQuestionModel.QuestionDetail.ParentQuestion)
                                        ,new SqlParameter("@ParentAnswer",objViewQuestionModel.QuestionDetail.ParentAnswer)
                                        ,new SqlParameter("@QuestionExplanation",objViewQuestionModel.QuestionDetail.Explanation)
                                        ,new SqlParameter("@QuestionHint",objViewQuestionModel.QuestionDetail.QuestionHint)
                                        ,new SqlParameter("@DropDownValues",DropDownValue)
                                        ,new SqlParameter("@QuestionType",objViewQuestionModel.QuestionDetail.QuestionType)
                                        ,new SqlParameter("@IsRequireResponse",objViewQuestionModel.QuestionDetail.IsRequireResponse)
                                        ,new SqlParameter("@QuestionPropertiesXml",objViewQuestionModel.QuestionProperties)
                                        ,new SqlParameter("@QuestionOptionsXml",objViewQuestionModel.QuestionOptions)
                                        ,new SqlParameter("@DisplayChoiceID",objViewQuestionModel.QuestionDetail.DisplayChoiceID)
                                        ,new SqlParameter("@IsActive", objViewQuestionModel.QuestionDetail.IsActive)
                                        ,new SqlParameter("@CreatedBy",objViewQuestionModel.QuestionDetail.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage


                                          };

                //If  CategoryId is 0 Than Insert  Category else Update  Category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_INSERTUPDATEQUESTION, parmList);
                //set error code and message 
                objViewQuestionModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewQuestionModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewQuestionModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ViewQuestionModel DeleteQuestion(ViewQuestionModel objViewQuestionModel)
        {
            try
            {

                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewQuestionModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewQuestionModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@QuestionID",objViewQuestionModel.DeletedQuestionID)
                                         ,new SqlParameter("@CreatedBy",objViewQuestionModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_DELETEQUESTION, parmList);
                //set output parameter error code and error message
                objViewQuestionModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewQuestionModel.Message = Convert.ToString(pErrorMessage.Value);

                return objViewQuestionModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
