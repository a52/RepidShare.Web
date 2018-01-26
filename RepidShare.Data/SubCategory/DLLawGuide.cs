using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;

namespace RepidShare.Data
{
   public class DLLawGuide
    {
        #region Get And Insert Update  LawGuide
        /// <summary>
        /// Get LawGuide by LawGuideId
        /// </summary>
        /// <param name="LawGuideId"></param>
        /// <returns></returns>
        public DataTable GetLawGuideById(int LawGuideId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@LawGuideID",LawGuideId),
                                      };
            //Call SPGETLawGuideBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetLawGuideByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  LawGuide
        /// </summary>
        /// <param name="objLawGuideModel"></param>
        /// <returns></returns>
        public LawGuideModel InsertUpdateLawGuide(LawGuideModel objLawGuideModel)
        {
            try
            {
                objLawGuideModel.LawGuideName = objLawGuideModel.LawGuideName.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@LawGuideID",objLawGuideModel.LawGuideID)
                                        ,new SqlParameter("@SubCategoryID",objLawGuideModel.SubCategoryID)
                                       
                                        ,new SqlParameter("@LawGuideName",objLawGuideModel.LawGuideName)   
                                        ,new SqlParameter("@Description",objLawGuideModel.Description)                           
                                        ,new SqlParameter("@IsActive", objLawGuideModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objLawGuideModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage
                                          };

                //If  LawGuideId is 0 Than Insert  LawGuide else Update  LawGuide
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateLawGuide, parmList);
                //set error code and message 
                objLawGuideModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objLawGuideModel.Message = Convert.ToString(pErrorMessage.Value);
                return objLawGuideModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete Sub Category
        /// </summary>
        /// <param name="objViewLawGuideModel"></param>
        /// <returns></returns>
        public ViewLawGuideModel DeleteLawGuide(ViewLawGuideModel objViewLawGuideModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewLawGuideModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewLawGuideModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@LawGuideID",objViewLawGuideModel.DeletedLawGuideID)
                                         ,new SqlParameter("@CreatedBy",objViewLawGuideModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteLawGuide, parmList);
                //set output parameter error code and error message
                objViewLawGuideModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewLawGuideModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewLawGuideModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region View  LawGuide
        /// <summary>
        /// Get  LawGuide List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewLawGuideModel">object of Model ViewLawGuideModel </param>
        /// <returns></returns>
        public DataTable GetLawGuideList(ViewLawGuideModel objViewLawGuideModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                     
                                     new SqlParameter("@SubCategoryId", objViewLawGuideModel.FilterSubCatId)
                                     ,new SqlParameter("@CurrentPage", objViewLawGuideModel.CurrentPage)
                                     ,new SqlParameter("@PageSize", objViewLawGuideModel.PageSize)
                                     ,new SqlParameter("@SortBy", objViewLawGuideModel.SortBy)
                                     ,new SqlParameter("@SortOrder", objViewLawGuideModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetLawGuideList, parmList);

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

        #region  LawGuide Drop Down
        /// <summary>
        /// Get All  LawGuide List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLawGuideListForDDL(int? CategoryID, int? GroupID)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     new SqlParameter("@CategoryId", CategoryID)
                                      };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetLawGuideListForDDL, parmList);
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
        /// Get All  LawGuide List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllLawGuideList(int? CategoryID, int? GroupID)
        {
            try
            {
                List<DropdownModel> lstLawGuide = new List<DropdownModel>();
                //Get All  LawGuide list
                DataTable dtLawGuide = GetAllLawGuideListForDDL(CategoryID, GroupID);
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtLawGuide.Rows)
                {
                    lstLawGuide.Add
                        (new DropdownModel()
                        {
                            ID = Convert.ToInt32(dr["LawGuideID"]),
                            Value = Convert.ToString(dr["LawGuideName"])
                        }
                        );
                }
                return lstLawGuide;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
