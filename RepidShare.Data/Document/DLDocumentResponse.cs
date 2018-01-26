/*
 * Created By :- Vishal gupta 
 * Date:- 18 Feb 2015
 * Desc:-Initial creation.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RepidShare.Entities;
using System.Data.SqlClient;
using RepidShare.Entities.DocumentResponse;

namespace RepidShare.Data
{
    public class DLDocumentResponse
    {

        /// <summary>
        /// Get Document response detail
        /// </summary>
        /// <param name="UserSessionModel"></param>
        /// <returns>Object of DataTable</returns>
        public DataSet GetDocumentResponseForSave(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            try
            {
                //create error code and error message output parameter.
                int ErrorCode = 0;
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 100);

                pErrorMessage.Direction = ParameterDirection.Output;



                //Set Param values by objViewParameters
                SqlParameter[] Param = {
                                           new SqlParameter("@DocumentID", objDocumentResponseDetailModel.DocumentID),
                                           new SqlParameter("@UserId", objDocumentResponseDetailModel.UserId),
                                           new SqlParameter("@ApplicationRoleID", objDocumentResponseDetailModel.ApplicationRoleID),
                                           new SqlParameter("@CurrentPage",objDocumentResponseDetailModel.CurrentPage),
                                           new SqlParameter("@PageSize",objDocumentResponseDetailModel.PageSize),
                                            new SqlParameter("@FolderId",objDocumentResponseDetailModel.FolderId),
                                           new SqlParameter("@IsSave",objDocumentResponseDetailModel.isSaved),
                                            new SqlParameter("@MaxPage",objDocumentResponseDetailModel.MaxPage)
                                             ,pErrorCode
                                       ,pErrorMessage
                                       };

                //Call spGetDocumentResponse for Save Procedure 
                DataSet dt = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_GetDocumentResponseForSaveStep, Param);
                //set Error code into model
                objDocumentResponseDetailModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objDocumentResponseDetailModel.Message = Convert.ToString(pErrorMessage.Value);
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                //throw exception.
                throw ex;
            }
        }



        /// <summary>
        /// Get Document response detail for view
        /// </summary>
        /// <param name="UserSessionModel"></param>
        /// <returns>Object of DataTable</returns>
        public DataTable GetDocumentResponseForView(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            try
            {
                //create error code and error message paramtere for sql procedure
                int ErrorCode = 0;
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 100);

                pErrorMessage.Direction = ParameterDirection.Output;



                //Set Param values by objViewParameters
                SqlParameter[] Param = {
                                           new SqlParameter("@DocumentID", objDocumentResponseDetailModel.DocumentID),
                                           new SqlParameter("@UserId", objDocumentResponseDetailModel.UserId),
                                           new SqlParameter("@ApplicationRoleID", objDocumentResponseDetailModel.ApplicationRoleID),
                                           new SqlParameter("@NoOfAttempt", objDocumentResponseDetailModel.NoOfAttempt),
                                           new SqlParameter("@CurrentPage",objDocumentResponseDetailModel.CurrentPage),
                                           new SqlParameter("@PageSize",objDocumentResponseDetailModel.PageSize)
                                             ,pErrorCode
                                       ,pErrorMessage

                                       };

                //Call spGetDocumentResponse Procedure for view 
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_GETDocumentRESPONSEFORVIEW, Param);
                //Set error code into Model
                objDocumentResponseDetailModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objDocumentResponseDetailModel.Message = Convert.ToString(pErrorMessage.Value);
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Get Document preview
        /// </summary>
        /// <param name="UserSessionModel"></param>
        /// <returns>Object of DataTable</returns>
        public DataTable GetDocumentPreview(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            try
            {
                //create error code and error message for sql proc.
                int ErrorCode = 0;
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 100);

                pErrorMessage.Direction = ParameterDirection.Output;



                //Set Param values by objViewParameters
                SqlParameter[] Param = {
                                           new SqlParameter("@DocumentID", objDocumentResponseDetailModel.DocumentID)
                                             ,pErrorCode
                                       ,pErrorMessage

                                       };

                //Call spGetDocumentResponse Procedure for preview
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_GETDocumentPREVIEW, Param);
                //set error code into Model
                objDocumentResponseDetailModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objDocumentResponseDetailModel.Message = Convert.ToString(pErrorMessage.Value);
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Insert or Update Document Question answer detail
        /// </summary>
        /// <param name="objDocumentResponseDetailModel"></param>
        /// <returns></returns>
        public DocumentResponseDetailModel InsertUpdateDocumentResult(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            try
            {
                //Create error code and error message into sql proc
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = {
                                          new SqlParameter("@DocumentResultID",objDocumentResponseDetailModel.Result.DocumentResultID)
                                        ,new SqlParameter("@DocumentTargetAudienceID",objDocumentResponseDetailModel.Result.DocumentTargetAudienceID)
                                        ,new SqlParameter("@NoOfAttempt",objDocumentResponseDetailModel.Result.NoOfAttempt)
                                        ,new SqlParameter("@IsCompleted",objDocumentResponseDetailModel.Result.IsCompleted)
                                        ,new SqlParameter("@CreatedBy",objDocumentResponseDetailModel.CommonCreatedBy)
                                        ,new SqlParameter("@QuestionAnswerXml",objDocumentResponseDetailModel.QuestionAnswerXml)
                                        ,pErrorCode
                                       ,pErrorMessage


                                          };

                //Exucute sql insertion/updation procedure for sql Document result detail.
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_INSERTUPDATEDocumentRESPONSE, parmList);
                //set error code and message 
                objDocumentResponseDetailModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objDocumentResponseDetailModel.Message = Convert.ToString(pErrorMessage.Value);
                return objDocumentResponseDetailModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public String GetDocumentPreviewTemp(int DocumentId, int UserId)
        {
            try
            {
                //Set Param values by objViewParameters
                SqlParameter[] Param = {
                                           new SqlParameter("@DocumentID", DocumentId),
                                           new SqlParameter("@UserId", UserId)
                                       };

                //Call spGetDocumentResponse Procedure for view 
                return SQLHelper.ExecuteScalar(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_GetDocumentPreviewTemp, Param).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDocumentHistory(int DocumentId, int UserId)
        {
            try
            {


                //Set Param values by objViewParameters
                SqlParameter[] Param = {
                                           new SqlParameter("@DocumentID", DocumentId),
                                           new SqlParameter("@UserId", UserId)
                                       };

                //Call spGetDocumentResponse Procedure for view 
                return SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetDocumentHistory, Param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region PriceQuestion
        public PriceQuestionModel InsertUpdatePriceQuestion(PriceQuestionModel objPriceQuestionModel)
        {
            try
            {
                DataTable dtPriceDetails = new DataTable();
                dtPriceDetails.Columns.Add("ResultDetailId"); dtPriceDetails.Columns.Add("QuestionId"); dtPriceDetails.Columns.Add("InstAmt");
                dtPriceDetails.Columns.Add("TaxAmt"); dtPriceDetails.Columns.Add("IneteAmt"); dtPriceDetails.Columns.Add("TotalAmt");
                dtPriceDetails.Columns.Add("DateAmt"); dtPriceDetails.Columns.Add("PenaltyAmt");

                for (int i = 0; i < objPriceQuestionModel.dtPriceDetails.Count; i++)
                {
                    dtPriceDetails.Rows.Add(objPriceQuestionModel.dtPriceDetails[i].ResultDetailId, objPriceQuestionModel.dtPriceDetails[i].QuestionId,
                                        objPriceQuestionModel.dtPriceDetails[i].InstAmt, objPriceQuestionModel.dtPriceDetails[i].TaxAmt, objPriceQuestionModel.dtPriceDetails[i].IneteAmt, objPriceQuestionModel.dtPriceDetails[i].TotalAmt, objPriceQuestionModel.dtPriceDetails[i].DateAmt, objPriceQuestionModel.dtPriceDetails[i].PenaltyAmt);
                }

                //Set Param values by objViewParameters
                SqlParameter[] Param = {
                                           new SqlParameter("@ResultDetailId", objPriceQuestionModel.ResultDetailId),
                                           new SqlParameter("@QuestionId", objPriceQuestionModel.QuestionId),
                                           new SqlParameter("@PrintAmt", objPriceQuestionModel.PrintAmt),
                                           new SqlParameter("@PriceAmtText", objPriceQuestionModel.PriceAmtText),
                                           new SqlParameter("@TaxAmt",objPriceQuestionModel.TaxAmt),
                                           new SqlParameter("@TaxAmtText",objPriceQuestionModel.TaxAmtText),
                                           new SqlParameter("@TotalAmt", objPriceQuestionModel.TotalAmt),
                                           new SqlParameter("@TotalText", objPriceQuestionModel.TotalText),
                                           new SqlParameter("@TaxTypeValue", objPriceQuestionModel.TaxTypeValue),
                                           new SqlParameter("@InstNoValue",objPriceQuestionModel.InstNoValue),
                                           new SqlParameter("@TableInnerHTML",objPriceQuestionModel.TableInnerHTML),
                                           new SqlParameter("@DivInnerHTML",objPriceQuestionModel.DivInnerHTML)
                                           //new SqlParameter("@FixedAmt",objPriceQuestionModel.FixedAmt)
                                           //new SqlParameter("@dtPriceDetails",dtPriceDetails)
                                       };

                //Call spGetDocumentResponse for Save Procedure 
                DataSet dt = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_InsertUpdatePriceQuestion, Param);

                return objPriceQuestionModel;
            }
            catch (Exception ex)
            {
                //throw exception.
                throw ex;
            }
        }

        public DataSet GetPriceQuestion(PriceQuestionModel objPriceQuestionModel)
        {
            try
            {

                //Set Param values by objViewParameters
                SqlParameter[] Param = {

                                           new SqlParameter("@ResultDetailId", objPriceQuestionModel.ResultDetailId)
                                           ,new SqlParameter("@QuestionId", objPriceQuestionModel.QuestionId)
                                       };

                //Call spGetDocumentResponse for Save Procedure 
                DataSet dt = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_GetPriceQuestion, Param);

                return dt;
            }
            catch (Exception ex)
            {
                //throw exception.
                throw ex;
            }
        }
        #endregion

       
        public void InsertStepSatus(UserDetailModel objUserDetailModel)
        {
            try
            {
                //Create error code and error message into sql proc
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = {
                                          new SqlParameter("@UserId",objUserDetailModel.UserID)
                                        ,new SqlParameter("@DocumentId",objUserDetailModel.DocumentID)
                                        ,new SqlParameter("@StepId",objUserDetailModel.StepID)
                                        ,new SqlParameter("@StatusId",objUserDetailModel.StatusId)  
                                        ,new SqlParameter("@StatusName",objUserDetailModel.StatusName)
                                        ,pErrorCode
                                       ,pErrorMessage
                                          };

                //Exucute sql insertion/updation procedure for sql Document result detail.
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "Web_InsertStepSatus", parmList);
                //set error code and message                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
