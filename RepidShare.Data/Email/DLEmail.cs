using RepidShare.Entities;
using RepidShare.Entities.Email;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Data.Email
{
    public class DLEmail
    {
        public DataTable GetAllDropdownlistForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_EmailTypeDrowdown, parmList);
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DropdownModel> GetEmailTyepDropDownList()
        {
            try
            {
                List<DropdownModel> lstCategory = new List<DropdownModel>();
                //Get All  category list
                DataTable dtCategory = GetAllDropdownlistForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtCategory.Rows)
                {
                    lstCategory.Add
                        (new DropdownModel()
                        {
                            ID = Convert.ToInt32(dr["EmailID"]),
                            Value = Convert.ToString(dr["EmailTitle"])
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

        // GetEmailTemplateList


        public DataTable GetEmailTemplateList(ViewEmailTemplateModel ViewEmailTemplateModelList)
        {
            try
            {
                SqlParameter[] parmList = { 
                                      new SqlParameter("@CurrentPage", ViewEmailTemplateModelList.CurrentPage)
                                     ,new SqlParameter("@PageSize", ViewEmailTemplateModelList.PageSize)
                                     ,new SqlParameter("@SortBy", ViewEmailTemplateModelList.SortBy)
                                     ,new SqlParameter("@SortOrder", ViewEmailTemplateModelList.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_EmailTemplateList, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // FillEmailTyepDropDown()

        public DataTable GetEmailTemplateById(int EmailDetailID)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@EmailDetailID",EmailDetailID),
                                      };
            //Call SPGETSubCategoryBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetEmailTemplateByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }


        public EmailTemplate InsertUpdateEmailDetail(EmailTemplate objEmailTemplate)
        {
            try
            {
                // objCategoryModel.CategoryName = objCategoryModel.CategoryName.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;
                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@EmailDetailID",objEmailTemplate.EmailDetailID)
                                        ,new SqlParameter("@EmailID",objEmailTemplate.EmailID)   
                                        ,new SqlParameter("@EmailSubject",objEmailTemplate.EmailSubject)   
                                        ,new SqlParameter("@Content",objEmailTemplate.Content)   
                                        
                                        ,new SqlParameter("@IsActive", objEmailTemplate.IsActive)
                                        ,new SqlParameter("@CreatedBy",objEmailTemplate.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage

                                          };

                //If  CategoryId is 0 Than Insert  Category else Update  Category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateEmailDetail, parmList);
                //set error code and message 
                objEmailTemplate.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objEmailTemplate.Message = Convert.ToString(pErrorMessage.Value);
                return objEmailTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public ViewEmailTemplateModel DeleteEmailTemplate(ViewEmailTemplateModel objEmailTemplateModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objEmailTemplateModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objEmailTemplateModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@EmailDetailID",objEmailTemplateModel.DeletedEmailDetailID)
                                         ,new SqlParameter("@CreatedBy",objEmailTemplateModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteEmailTemplate, parmList);
                //set output parameter error code and error message
                objEmailTemplateModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objEmailTemplateModel.Message = Convert.ToString(pErrorMessage.Value);
                return objEmailTemplateModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetEmailSettingByEmailID(int EmailID)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@EmailID",EmailID),
                                      };
            //Call SPGETSubCategoryBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetEmailSettingByEmailID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
    }
}
