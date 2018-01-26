using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLDropDown
    {
        #region Get And Insert Update  DropDown
        /// <summary>
        /// Get DropDown by DropDownId
        /// </summary>
        /// <param name="DropDownId"></param>
        /// <returns></returns>
        public DataTable GetDropDownById(int DropDownId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@DropDownID",DropDownId),
                                      };
            //Call SPGETDropDownBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetDropDownByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  DropDown
        /// </summary>
        /// <param name="objDropDownModel"></param>
        /// <returns></returns>
        public DropDownModel InsertUpdateDropDown(DropDownModel objDropDownModel)
        {
            try
            {
                objDropDownModel.DropDownText = objDropDownModel.DropDownText.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@DropDownID",objDropDownModel.QuestionDropDownID)
                                        ,new SqlParameter("@DropDownName",objDropDownModel.DropDownText)      
                                        ,new SqlParameter("@IsActive", objDropDownModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objDropDownModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage

                                          };

                //If  DropDownId is 0 Than Insert  DropDown else Update  DropDown
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateDropDown, parmList);
                //set error code and message 
                objDropDownModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objDropDownModel.Message = Convert.ToString(pErrorMessage.Value);
                return objDropDownModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete DropDown
        /// </summary>
        /// <param name="objViewDropDownModel"></param>
        /// <returns></returns>
        public ViewDropDownModel DeleteDropDown(ViewDropDownModel objViewDropDownModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode",objViewDropDownModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewDropDownModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@DropDownID",objViewDropDownModel.DeletedDropDownID)
                                         ,new SqlParameter("@CreatedBy",objViewDropDownModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  DropDown
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteDropDown, parmList);
                //set output parameter error code and error message
                objViewDropDownModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewDropDownModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewDropDownModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region View  DropDown
        /// <summary>
        /// Get  DropDown List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewDropDownModel">object of Model ViewDropDownModel </param>
        /// <returns></returns>
        public DataTable GetDropDownList(ViewDropDownModel objViewDropDownModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("DropDownName", objViewDropDownModel.FilterDropDownName)
                                     ,new SqlParameter("CurrentPage", objViewDropDownModel.CurrentPage)
                                     ,new SqlParameter("PageSize", objViewDropDownModel.PageSize)
                                     ,new SqlParameter("SortBy", objViewDropDownModel.SortBy)
                                     ,new SqlParameter("SortOrder", objViewDropDownModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetDropDownList, parmList);

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

        #region  DropDown Drop Down
        /// <summary>
        /// Get All  DropDown List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllDropDownListForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetDropDownListForDDL, parmList);
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
        /// Get All  DropDown List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllDropDownList()
        {
            try
            {
                List<DropdownModel> lstDropDown = new List<DropdownModel>();
                //Get All  DropDown list
                DataTable dtDropDown = GetAllDropDownListForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtDropDown.Rows)
                {
                    lstDropDown.Add
                        (new DropdownModel()
                            {
                                ID = Convert.ToInt32(dr["QuestionDropDownID"]),
                                Value = Convert.ToString(dr["DropDownText"])
                            }
                        );
                }
                return lstDropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
