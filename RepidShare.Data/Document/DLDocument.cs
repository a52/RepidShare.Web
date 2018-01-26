using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLDocument
    {
        #region Get And Insert Update  Document
        /// <summary>
        /// Get Document by DocumentId
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public DataTable GetDocumentById(int DocumentId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@DocumentID",DocumentId)
                                      };
            //Call SPGETDocumentBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetDocumentByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  Document
        /// </summary>
        /// <param name="objDocumentModel"></param>
        /// <returns></returns>
        public DocumentModel InsertUpdateDocument(DocumentModel objDocumentModel)
        {
            try
            {
                objDocumentModel.DocumentTitle = objDocumentModel.DocumentTitle.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;
                pErrorMessage.Size = 8000;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@DocumentID",objDocumentModel.DocumentID)
                                        ,new SqlParameter("@DocumentTitle",objDocumentModel.DocumentTitle)   
                                        ,new SqlParameter("@DocumentDescription",objDocumentModel.DocumentDescription)
                                        ,new SqlParameter("@DocumentHTML",objDocumentModel.DocumentHTML)
                                        ,new SqlParameter("@Price",objDocumentModel.Price)
                                        ,new SqlParameter("@PackIds",objDocumentModel.PackIds)
                                        ,new SqlParameter("@UserIds",objDocumentModel.UserIds)
                                        ,new SqlParameter("@IsPublished",objDocumentModel.IsPublished)  
                                        ,new SqlParameter("@IsMostPopular",objDocumentModel.IsMostPopular)  
                                        ,new SqlParameter("@SubCategoryID",objDocumentModel.SubCategoryID)  
                                        ,new SqlParameter("@GroupID",objDocumentModel.GroupID==null?0:objDocumentModel.GroupID)  
                                        ,new SqlParameter("@SelectedIds",objDocumentModel.SavedStep)                          
                                        ,new SqlParameter("@SelectedText",objDocumentModel.SelectedText)                          
                                        ,new SqlParameter("@IsActive", objDocumentModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objDocumentModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage


                                          };

                //If  DocumentId is 0 Than Insert  Document else Update  Document
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateDocument, parmList);
                //set error code and message 
                objDocumentModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objDocumentModel.Message = Convert.ToString(pErrorMessage.Value);
                return objDocumentModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Delete Sub Category
        /// </summary>
        /// <param name="objViewSubCategoryModel"></param>
        /// <returns></returns>
        public ViewDocumentModel DeleteDocument(ViewDocumentModel objViewDocumentModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewDocumentModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewDocumentModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@DocumentID",objViewDocumentModel.DeletedDocumentID)
                                         ,new SqlParameter("@CreatedBy",objViewDocumentModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteDocument, parmList);
                //set output parameter error code and error message
                objViewDocumentModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewDocumentModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewDocumentModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region View  Document
        /// <summary>
        /// Get  Document List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewDocumentModel">object of Model ViewDocumentModel </param>
        /// <returns></returns>
        public DataTable GetDocumentList(ViewDocumentModel objViewDocumentModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                     new SqlParameter("@CategoryID", objViewDocumentModel.FilterCategoryId)
                                     ,new SqlParameter("@SubCategoryID", objViewDocumentModel.FilterSubCatId)
                                     ,new SqlParameter("@DocumentTitle", objViewDocumentModel.FilterDocumentTitle)
                                     ,new SqlParameter("@CurrentPage", objViewDocumentModel.CurrentPage)
                                     ,new SqlParameter("@PageSize", objViewDocumentModel.PageSize)
                                     ,new SqlParameter("@SortBy", objViewDocumentModel.SortBy)
                                     ,new SqlParameter("@SortOrder", objViewDocumentModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetDocumentList, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Document Response
        /// <summary>
        /// Get All Document Response
        /// </summary>
        /// <param name="objViewDocumentResponseModel"></param>
        /// <returns></returns>
        public DataTable GetAllDocumentResponse(ViewDocumentResponseModel objViewDocumentResponseModel)
        {
            try
            {
                //Set Param values by objViewDocumentResponseModel
                SqlParameter[] Param = {                                           
                                           new SqlParameter("@DocumentTitle", objViewDocumentResponseModel.FilterDocumentTitle),
                                           new SqlParameter("@CreatFromDate",objViewDocumentResponseModel.CreateFromDate),
                                           new SqlParameter("@CreatEndDate",objViewDocumentResponseModel.CreateEndDate),
                                           new SqlParameter("@CurrentPage", objViewDocumentResponseModel.CurrentPage), 
                                           new SqlParameter("@PageSize",objViewDocumentResponseModel.PageSize), 
                                           new SqlParameter("@SortBy",string.IsNullOrWhiteSpace(objViewDocumentResponseModel.SortBy)  == true ?  "" : objViewDocumentResponseModel.SortBy), 
                                           new SqlParameter("@SortOrder", objViewDocumentResponseModel.SortOrder)
                                       };

                //Call spGetDocumentListForResponse Procedure 
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.WEB_GetDocumentListForResponse, Param);

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
        /// Get All Document Response User
        /// </summary>
        /// <param name="objViewDocumentUserResponseModel"></param>
        /// <returns></returns>
        public DataSet GetAllDocumentResponseUser(ViewDocumentUserResponseModel objViewDocumentUserResponseModel)
        {
            try
            {
                //Set Param values by objViewDocumentUserResponseModel
                SqlParameter[] Param = {                                           
                                           new SqlParameter("@DocumentID", objViewDocumentUserResponseModel.DocumentID),
                                           new SqlParameter("@UserName", objViewDocumentUserResponseModel.UserName),
                                           new SqlParameter("@CurrentPage", objViewDocumentUserResponseModel.CurrentPage), 
                                           new SqlParameter("@PageSize",objViewDocumentUserResponseModel.PageSize), 
                                           new SqlParameter("@SortBy",string.IsNullOrWhiteSpace(objViewDocumentUserResponseModel.SortBy)  == true ?  "" : objViewDocumentUserResponseModel.SortBy), 
                                           new SqlParameter("@SortOrder", objViewDocumentUserResponseModel.SortOrder)
                                       };

                //Call spGetDocumentResponseUserList Procedure 
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.WEB_GetDocumentResponseUserList, Param);

                if (ds != null)
                    return ds;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region My Document
        /// <summary>
        /// Get User Document List
        /// </summary>
        /// <param name="objViewDocumentResponseModel"></param>
        /// <returns></returns>
        public DataTable GetUserDocumentList(ViewDocumentResponseModel objViewDocumentResponseModel)
        {
            try
            {
                //Set Param values by objViewDocumentUserResponseModel
                SqlParameter[] Param = {                                           
                                           new SqlParameter("@UserId", objViewDocumentResponseModel.UserId),
                                           new SqlParameter("@DocumentTitle",objViewDocumentResponseModel.FilterDocumentTitle),
                                           new SqlParameter("@CurrentPage", objViewDocumentResponseModel.CurrentPage), 
                                           new SqlParameter("@PageSize",objViewDocumentResponseModel.PageSize), 
                                           new SqlParameter("@SortBy",string.IsNullOrWhiteSpace(objViewDocumentResponseModel.SortBy)  == true ?  "" : objViewDocumentResponseModel.SortBy), 
                                           new SqlParameter("@SortOrder", objViewDocumentResponseModel.SortOrder)
                                       };

                //Call spGetUserDocumentList Procedure 
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.WEB_GetUserDocumentList, Param);

                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion




    }
}
