using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLSubCategory
    {
        #region Get And Insert Update  SubCategory
        /// <summary>
        /// Get SubCategory by SubCategoryId
        /// </summary>
        /// <param name="SubCategoryId"></param>
        /// <returns></returns>
        public DataTable GetSubCategoryById(int SubCategoryId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@SubCategoryID",SubCategoryId),
                                      };
            //Call SPGETSubCategoryBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetSubCategoryByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  SubCategory
        /// </summary>
        /// <param name="objSubCategoryModel"></param>
        /// <returns></returns>
        public SubCategoryModel InsertUpdateSubCategory(SubCategoryModel objSubCategoryModel)
        {
            try
            {
                objSubCategoryModel.SubCatName = objSubCategoryModel.SubCatName.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@SubCategoryID",objSubCategoryModel.SubCategoryID)
                                        ,new SqlParameter("@CategoryID",objSubCategoryModel.CategoryID)
                                        ,new SqlParameter("@SubCategoryName",objSubCategoryModel.SubCatName)   
                                         ,new SqlParameter("@Description",objSubCategoryModel.Description)  
                                        ,new SqlParameter("@ClassName",objSubCategoryModel.ClassName) 
                                         ,new SqlParameter("@AttachmentID",objSubCategoryModel.AttachmentID)   
                                        ,new SqlParameter("@AttachmentType",objSubCategoryModel.AttachmentType)  
                                         ,new SqlParameter("@AttachmentName",objSubCategoryModel.AttachmentName)  
                                        ,new SqlParameter("@AttachmentSize",objSubCategoryModel.AttachmentSize)   
                                        ,new SqlParameter("@AttachmentContent",objSubCategoryModel.AttachmentContent)  
                                        ,new SqlParameter("@IsActive", objSubCategoryModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objSubCategoryModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage
                                          };

                //If  SubCategoryId is 0 Than Insert  SubCategory else Update  SubCategory
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateSubCategory, parmList);
                //set error code and message 
                objSubCategoryModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objSubCategoryModel.Message = Convert.ToString(pErrorMessage.Value);
                return objSubCategoryModel;
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
        public ViewSubCategoryModel DeleteSubCategory(ViewSubCategoryModel objViewSubCategoryModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewSubCategoryModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewSubCategoryModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@SubCategoryID",objViewSubCategoryModel.DeletedSubCategoryID)
                                         ,new SqlParameter("@CreatedBy",objViewSubCategoryModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteSubCategory, parmList);
                //set output parameter error code and error message
                objViewSubCategoryModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewSubCategoryModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewSubCategoryModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region View  SubCategory
        /// <summary>
        /// Get  SubCategory List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewSubCategoryModel">object of Model ViewSubCategoryModel </param>
        /// <returns></returns>
        public DataTable GetSubCategoryList(ViewSubCategoryModel objViewSubCategoryModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@SubCategoryName", objViewSubCategoryModel.FilterSubCatName)
                                     ,new SqlParameter("@CategoryId", objViewSubCategoryModel.FilterCategoryId)
                                     ,new SqlParameter("@CurrentPage", objViewSubCategoryModel.CurrentPage)
                                     ,new SqlParameter("@PageSize", objViewSubCategoryModel.PageSize)
                                     ,new SqlParameter("@SortBy", objViewSubCategoryModel.SortBy)
                                     ,new SqlParameter("@SortOrder", objViewSubCategoryModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetSubCategoryList, parmList);

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

        #region  SubCategory Drop Down
        /// <summary>
        /// Get All  SubCategory List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSubCategoryListForDDL(int? CategoryID, int? GroupID)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     new SqlParameter("@CategoryId", CategoryID)
                                      };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetSubCategoryListForDDL, parmList);
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
        /// Get All  SubCategory List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllSubCategoryList(int? CategoryID, int? GroupID)
        {
            try
            {
                List<DropdownModel> lstSubCategory = new List<DropdownModel>();
                //Get All  SubCategory list
                DataTable dtSubCategory = GetAllSubCategoryListForDDL(CategoryID, GroupID);
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtSubCategory.Rows)
                {
                    lstSubCategory.Add
                        (new DropdownModel()
                            {
                                ID = Convert.ToInt32(dr["SubCategoryID"]),
                                Value = Convert.ToString(dr["SubCatName"])
                            }
                        );
                }
                return lstSubCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public String UpdateSubCategoryStatusByID(int SubCategoryId, int status)
        {
            try
            {
                //Set Param values by objViewParameters
                SqlParameter[] Param = {     

                                           new SqlParameter("@SubCategoryId", SubCategoryId),
                                           new SqlParameter("@status", status)
                                       };

                //Call spGetDocumentResponse Procedure for view 
                return SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_UpdateSubCategoryStatusByID, Param).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    }
}
