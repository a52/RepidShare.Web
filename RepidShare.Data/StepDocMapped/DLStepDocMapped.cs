using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Data
{
    public class DLStepDocMapped
    {
        #region Get And Insert Update  StepDocMapped
        /// <summary>
        /// Get StepDocMapped by StepDocMappedId
        /// </summary>
        /// <param name="StepDocMappedId"></param>
        /// <returns></returns>
        public DataTable GetStepDocMappedById(int StepDocMappedId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@StepDocMappedID",StepDocMappedId),
                                      };
            //Call SPGETStepDocMappedBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_GETSTEPDOCMAPPEDBYID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  StepDocMapped
        /// </summary>
        /// <param name="objStepDocMappedModel"></param>
        /// <returns></returns>
        public StepDocMappedModel InsertUpdateStepDocMapped(StepDocMappedModel objStepDocMappedModel)
        {
            try
            {
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@StepDocMappedID",objStepDocMappedModel.StepDocMappedID)
                                        ,new SqlParameter("@DocumentID",objStepDocMappedModel.DocumentID)                                        
                                        ,new SqlParameter("@StepID",objStepDocMappedModel.StepID)                                        
                                        ,new SqlParameter("@DisplayOrder",objStepDocMappedModel.DisplayOrder)                                        
                                        ,new SqlParameter("@IsActive", objStepDocMappedModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objStepDocMappedModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage

                                          };

                //If  StepDocMappedId is 0 Than Insert  StepDocMapped else Update  StepDocMapped
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_INSERTUPDATESTEPDOCMAPPED, parmList);
                //set error code and message 
                objStepDocMappedModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objStepDocMappedModel.Message = Convert.ToString(pErrorMessage.Value);
                return objStepDocMappedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Delete StepDocMapped
        /// </summary>
        /// <param name="objViewSubCategoryModel"></param>
        /// <returns></returns>
        public ViewStepDocMappedModel DeleteStepDocMapped(ViewStepDocMappedModel objViewStepDocMappedModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewStepDocMappedModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewStepDocMappedModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@StepDocMappedID",objViewStepDocMappedModel.DeletedStepDocMappedID)
                                         ,new SqlParameter("@CreatedBy",objViewStepDocMappedModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_DELETESTEPDOCMAPPED, parmList);
                //set output parameter error code and error message
                objViewStepDocMappedModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewStepDocMappedModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewStepDocMappedModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region View  StepDocMapped
       
        public DataTable GetStepDocMappedByDocumentID(int StepID,int DocumentID)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                     new SqlParameter("@DocumentID", DocumentID)
                                     ,new SqlParameter("@StepID", StepID)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_GETSTEPDOCMAPPEDBYDOCUMENTID, parmList);

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

        
    }
}
