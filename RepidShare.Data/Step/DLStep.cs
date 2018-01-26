using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLStep
    {
        #region Get And Insert Update  Step
        /// <summary>
        /// Get Step by StepId
        /// </summary>
        /// <param name="StepId"></param>
        /// <returns></returns>
        public DataTable GetStepById(int StepId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@StepID",StepId),
                                      };
            //Call SPGETStepBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetStepByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  Step
        /// </summary>
        /// <param name="objStepModel"></param>
        /// <returns></returns>
        public StepModel InsertUpdateStep(StepModel objStepModel)
        {
            try
            {
                objStepModel.StepName = objStepModel.StepName.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@StepID",objStepModel.StepID)
                                        ,new SqlParameter("@StepName",objStepModel.StepName)                                        
                                        ,new SqlParameter("@StepDiscription",objStepModel.StepDiscription==null?string.Empty:objStepModel.StepDiscription)                                        
                                        ,new SqlParameter("@IsActive", objStepModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objStepModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage


                                          };

                //If  StepId is 0 Than Insert  Step else Update  Step
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateStep, parmList);
                //set error code and message 
                objStepModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objStepModel.Message = Convert.ToString(pErrorMessage.Value);
                return objStepModel;
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
        public ViewStepModel DeleteStep(ViewStepModel objViewStepModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewStepModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewStepModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@StepID",objViewStepModel.DeletedStepID)
                                         ,new SqlParameter("@CreatedBy",objViewStepModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteStep, parmList);
                //set output parameter error code and error message
                objViewStepModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewStepModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewStepModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region View  Step
        /// <summary>
        /// Get  Step List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewStepModel">object of Model ViewStepModel </param>
        /// <returns></returns>
        public DataTable GetStepList(ViewStepModel objViewStepModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@StepText", objViewStepModel.FilterStepName)
                                     ,new SqlParameter("@CurrentPage", objViewStepModel.CurrentPage)
                                     ,new SqlParameter("@PageSize", objViewStepModel.PageSize)
                                     ,new SqlParameter("@SortBy", objViewStepModel.SortBy)
                                     ,new SqlParameter("@SortOrder", objViewStepModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetStepList, parmList);

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

        #region  Step Drop Down
        /// <summary>
        /// Get All  Step List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllStepListForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetStepListForDDL, parmList);
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAllStepListForDDLByDocumentId(int DocumentId)
        {
            try
            {
                SqlParameter[] parmList = { new SqlParameter("DocumentId", DocumentId) };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetStepListForDDLByDocumentId, parmList);
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetStepByDocumentIdAndUserIdDT(int DocumentId, int UserId)
        {
            try
            {
                SqlParameter[] parmList = { new SqlParameter("DocumentId", DocumentId),
                                          new SqlParameter("UserId", UserId)};
                //DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetStepByDocumentIdANDUserID, parmList);
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, "Admin_GetStepSatusByUserIdDocumentId", parmList);
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
        /// Get All  Step List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllStepList()
        {
            try
            {
                List<DropdownModel> lstStep = new List<DropdownModel>();
                //Get All  Step list
                DataTable dtStep = GetAllStepListForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtStep.Rows)
                {
                    lstStep.Add
                        (new DropdownModel()
                            {
                                ID = Convert.ToInt32(dr["StepID"]),
                                Value = Convert.ToString(dr["StepName"])
                            }
                        );
                }
                return lstStep;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropdownModel> GetAllStepListByDocumentId(int DocumentId)
        {
            try
            {
                List<DropdownModel> lstStep = new List<DropdownModel>();
                //Get All  Step list
                DataTable dtStep = GetAllStepListForDDLByDocumentId(DocumentId);
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtStep.Rows)
                {
                    lstStep.Add
                        (new DropdownModel()
                        {
                            ID = Convert.ToInt32(dr["StepID"]),
                            Value = Convert.ToString(dr["StepName"])
                        }
                        );
                }
                return lstStep;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StepModel> GetStepByDocumentIdAndUserId(int DocumentId, int UserId)
        {
            try
            {
                List<StepModel> lstStep = new List<StepModel>();
                //Get All  Step list
                DataTable dtStep = GetStepByDocumentIdAndUserIdDT(DocumentId, UserId);
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtStep.Rows)
                {
                    lstStep.Add
                        (new StepModel()
                        {
                            StepID = Convert.ToInt32(dr["StepID"]),
                            StepName = Convert.ToString(dr["StepName"]),
                            stepStatus = Convert.ToInt16(dr["IsComplete"])
                        }
                        );
                }
                return lstStep;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
