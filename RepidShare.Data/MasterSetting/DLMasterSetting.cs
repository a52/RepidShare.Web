using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLMasterSetting
    {
        #region Get And Insert Update  MasterSetting
        /// <summary>
        /// Get MasterSetting by MasterSettingId
        /// </summary>
        /// <param name="MasterSettingId"></param>
        /// <returns></returns>
        public DataTable GetMasterSettingById(int MasterSettingId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@MasterSettingID",MasterSettingId),
                                      };
            //Call SPGETMasterSettingBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetMasterSettingByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  MasterSetting
        /// </summary>
        /// <param name="objMasterSettingModel"></param>
        /// <returns></returns>
        public MasterSettingModel InsertUpdateMasterSetting(MasterSettingModel objMasterSettingModel)
        {
            try
            {
                objMasterSettingModel.VatTax = objMasterSettingModel.VatTax.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@MasterSettingID",objMasterSettingModel.MasterSettingID)
                                        ,new SqlParameter("@MasterSettingText",objMasterSettingModel.VatTax)                                        
                                        ,new SqlParameter("@IsActive", objMasterSettingModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objMasterSettingModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage


                                          };

                //If  MasterSettingId is 0 Than Insert  MasterSetting else Update  MasterSetting
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateMasterSetting, parmList);
                //set error code and message 
                objMasterSettingModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objMasterSettingModel.Message = Convert.ToString(pErrorMessage.Value);
                return objMasterSettingModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

      
    }
}
