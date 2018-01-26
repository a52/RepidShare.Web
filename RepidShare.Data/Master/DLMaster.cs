using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLMaster
    {
        #region Get And Insert Update  Master
        /// <summary>
        /// Get Master by MasterId
        /// </summary>
        /// <param name="MasterId"></param>
        /// <returns></returns>
        public DataTable GetMasterById(int MasterId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@MasterID",MasterId),
                                      };
            //Call SPGETMasterBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetMasterByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  Master
        /// </summary>
        /// <param name="objMasterModel"></param>
        /// <returns></returns>
        public MasterModel InsertUpdateMaster(MasterModel objMasterModel)
        {
            try
            {
                objMasterModel.MasterValue = objMasterModel.MasterValue.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@MasterID",objMasterModel.MasterID)
                                        ,new SqlParameter("@MasterValue",objMasterModel.MasterValue)
                                        ,new SqlParameter("@Description",objMasterModel.Description)
                                        ,new SqlParameter("@IsActive", objMasterModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objMasterModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage


                                          };

                //If  MasterId is 0 Than Insert  Master else Update  Master
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateMaster, parmList);
                //set error code and message 
                objMasterModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objMasterModel.Message = Convert.ToString(pErrorMessage.Value);
                return objMasterModel;
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
        public ViewMasterModel DeleteMaster(ViewMasterModel objViewMasterModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewMasterModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewMasterModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@MasterID",objViewMasterModel.DeletedMasterID)
                                         ,new SqlParameter("@CreatedBy",objViewMasterModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteMaster, parmList);
                //set output parameter error code and error message
                objViewMasterModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewMasterModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewMasterModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region View  Master
        /// <summary>
        /// Get  Master List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewMasterModel">object of Model ViewMasterModel </param>
        /// <returns></returns>
        public DataTable GetMasterList(ViewMasterModel objViewMasterModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@MasterValue", objViewMasterModel.FilterMasterText)
                                     ,new SqlParameter("@CurrentPage", objViewMasterModel.CurrentPage)
                                     ,new SqlParameter("@PageSize", objViewMasterModel.PageSize)
                                     ,new SqlParameter("@SortBy", objViewMasterModel.SortBy)
                                     ,new SqlParameter("@SortOrder", objViewMasterModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetMasterList, parmList);

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

        #region  Master Drop Down
        /// <summary>
        /// Get All  Master List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllMasterListForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetMasterListForDDL, parmList);
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
        /// Get All  Master List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllMasterList()
        {
            try
            {
                List<DropdownModel> lstMaster = new List<DropdownModel>();
                //Get All  Master list
                DataTable dtMaster = GetAllMasterListForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtMaster.Rows)
                {
                    lstMaster.Add
                        (new DropdownModel()
                            {
                                ID = Convert.ToInt32(dr["MasterID"]),
                                Value = Convert.ToString(dr["MasterText"])
                            }
                        );
                }
                return lstMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
