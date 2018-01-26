using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLCategory
    {
        #region Get And Insert Update  Category
        /// <summary>
        /// Get Category by CategoryId
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public DataTable GetCategoryById(int CategoryId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@CategoryID",CategoryId),
                                      };
            //Call SPGETCATEGORYBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetCategoryByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  Category
        /// </summary>
        /// <param name="objCategoryModel"></param>
        /// <returns></returns>
        public CategoryModel InsertUpdateCategory(CategoryModel objCategoryModel)
        {
            try
            {
                objCategoryModel.CategoryName = objCategoryModel.CategoryName.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@CategoryID",objCategoryModel.CategoryID)
                                        ,new SqlParameter("@CategoryName",objCategoryModel.CategoryName)   
                                        ,new SqlParameter("@Description",objCategoryModel.Description)  
                                        ,new SqlParameter("@ClassName",objCategoryModel.ClassName)
                                        ,new SqlParameter("@AttachmentID",objCategoryModel.AttachmentID)   
                                        ,new SqlParameter("@AttachmentType",objCategoryModel.AttachmentType)  
                                         ,new SqlParameter("@AttachmentName",objCategoryModel.AttachmentName)  
                                        ,new SqlParameter("@AttachmentSize",objCategoryModel.AttachmentSize)   
                                        ,new SqlParameter("@AttachmentContent",objCategoryModel.AttachmentContent)  
                                        ,new SqlParameter("@QuickLinks",objCategoryModel.QuickLinks) 
                                        ,new SqlParameter("@IsActive", objCategoryModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objCategoryModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage

                                          };

                //If  CategoryId is 0 Than Insert  Category else Update  Category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateCategory, parmList);
                //set error code and message 
                objCategoryModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objCategoryModel.Message = Convert.ToString(pErrorMessage.Value);
                return objCategoryModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="objViewCategoryModel"></param>
        /// <returns></returns>
        public ViewCategoryModel DeleteCategory(ViewCategoryModel objViewCategoryModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode",objViewCategoryModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewCategoryModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@CategoryID",objViewCategoryModel.DeletedCategoryID)
                                         ,new SqlParameter("@CreatedBy",objViewCategoryModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteCategory, parmList);
                //set output parameter error code and error message
                objViewCategoryModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewCategoryModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewCategoryModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region View  Category
        /// <summary>
        /// Get  Category List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewCategoryModel">object of Model ViewCategoryModel </param>
        /// <returns></returns>
        public DataTable GetCategoryList(ViewCategoryModel objViewCategoryModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("CategoryName", objViewCategoryModel.FilterCategoryName)
                                     ,new SqlParameter("CurrentPage", objViewCategoryModel.CurrentPage)
                                     ,new SqlParameter("PageSize", objViewCategoryModel.PageSize)
                                     ,new SqlParameter("SortBy", objViewCategoryModel.SortBy)
                                     ,new SqlParameter("SortOrder", objViewCategoryModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetCategoryList, parmList);

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

        #region  Category Drop Down
        /// <summary>
        /// Get All  Category List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCategoryListForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetCategoryListForDDL, parmList);
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
        /// Get All  Category List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllCategoryList()
        {
            try
            {
                List<DropdownModel> lstCategory = new List<DropdownModel>();
                //Get All  category list
                DataTable dtCategory = GetAllCategoryListForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtCategory.Rows)
                {
                    lstCategory.Add
                        (new DropdownModel()
                            {
                                ID = Convert.ToInt32(dr["CategoryID"]),
                                Value = Convert.ToString(dr["CategoryName"])
                            }
                        );
                }
                return lstCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String UpdateCategoryStatusByID(int CategoryId, int status)
        {
            try
            {
                //Set Param values by objViewParameters
                SqlParameter[] Param = {     

                                           new SqlParameter("@CategoryId", CategoryId),
                                           new SqlParameter("@status", status)
                                       };

                //Call spGetDocumentResponse Procedure for view 
                return SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_UpdateCategoryStatusByID, Param).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
