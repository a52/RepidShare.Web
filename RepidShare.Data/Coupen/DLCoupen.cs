using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLCoupen
    {
        #region Get And Insert Update  Coupen
        /// <summary>
        /// Get Coupen by CoupenId
        /// </summary>
        /// <param name="CoupenId"></param>
        /// <returns></returns>
        public DataTable GetCoupenById(int CoupenId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@CoupenID",CoupenId),
                                      };
            //Call SPGETCoupenBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetCoupenByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  Coupen
        /// </summary>
        /// <param name="objCoupenModel"></param>
        /// <returns></returns>
        public CoupenModel InsertUpdateCoupen(CoupenModel objCoupenModel)
        {
            try
            {
                objCoupenModel.CoupenCode = objCoupenModel.CoupenCode.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@CoupenID",objCoupenModel.CoupenID)
                                        ,new SqlParameter("@CoupenCode",objCoupenModel.CoupenCode)
                                        ,new SqlParameter("@OffValue",objCoupenModel.OffValue)
                                        ,new SqlParameter("@IsActive", objCoupenModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objCoupenModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage


                                          };

                //If  CoupenId is 0 Than Insert  Coupen else Update  Coupen
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateCoupen, parmList);
                //set error code and message 
                objCoupenModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objCoupenModel.Message = Convert.ToString(pErrorMessage.Value);
                return objCoupenModel;
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
        public ViewCoupenModel DeleteCoupen(ViewCoupenModel objViewCoupenModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewCoupenModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewCoupenModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@CoupenID",objViewCoupenModel.DeletedCoupenID)
                                         ,new SqlParameter("@CreatedBy",objViewCoupenModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteCoupen, parmList);
                //set output parameter error code and error message
                objViewCoupenModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewCoupenModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewCoupenModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region View  Coupen
        /// <summary>
        /// Get  Coupen List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewCoupenModel">object of Model ViewCoupenModel </param>
        /// <returns></returns>
        public DataTable GetCoupenList(ViewCoupenModel objViewCoupenModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@CoupenCode", objViewCoupenModel.FilterCoupenText)
                                     ,new SqlParameter("@CurrentPage", objViewCoupenModel.CurrentPage)
                                     ,new SqlParameter("@PageSize", objViewCoupenModel.PageSize)
                                     ,new SqlParameter("@SortBy", objViewCoupenModel.SortBy)
                                     ,new SqlParameter("@SortOrder", objViewCoupenModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetCoupenList, parmList);

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

        #region  Coupen Drop Down
        /// <summary>
        /// Get All  Coupen List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCoupenListForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetCoupenListForDDL, parmList);
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
        /// Get All  Coupen List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllCoupenList()
        {
            try
            {
                List<DropdownModel> lstCoupen = new List<DropdownModel>();
                //Get All  Coupen list
                DataTable dtCoupen = GetAllCoupenListForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtCoupen.Rows)
                {
                    lstCoupen.Add
                        (new DropdownModel()
                            {
                                ID = Convert.ToInt32(dr["CoupenID"]),
                                Value = Convert.ToString(dr["CoupenText"])
                            }
                        );
                }
                return lstCoupen;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
