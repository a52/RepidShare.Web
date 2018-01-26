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
    public class DLUserLogin
    {
        public DataSet Login(UserLogin objUserLogin, out UserLogin outUserLogin)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", 0);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", "");
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = {
                                          new SqlParameter("@UserName",objUserLogin.UserName)
                                        ,new SqlParameter("@Password",objUserLogin.Password)
                                        ,pErrorCode
                                       ,pErrorMessage
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.PROC_UserLogin, parmList);

                //Get & set Output Parameter values in objApplicationMappingModel 
                objUserLogin.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objUserLogin.Message = Convert.ToString(pErrorMessage.Value);

                outUserLogin = objUserLogin;
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Register(Register objRegister)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", 0);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", "");
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                        // new SqlParameter("@UserId",objRegister.UserId)                                        
                                        new SqlParameter("@AccountType",objRegister.AccountType==null ?"":objRegister.AccountType)
                                        ,new SqlParameter("@Title",objRegister.Title==null ?"":objRegister.Title)
                                        ,new SqlParameter("@FirstName",objRegister.FirstName==null ?"":objRegister.FirstName)
                                        ,new SqlParameter("@MiddleName",objRegister.MiddleName==null ?"":objRegister.MiddleName)
                                          ,new SqlParameter("@LastName",objRegister.LastName==null ?"":objRegister.LastName)
                                        ,new SqlParameter("@Gender",objRegister.Gender==null ?"":objRegister.Gender)
                                        ,new SqlParameter("@Dob_day",objRegister.Dob_day==null ?"":objRegister.Dob_day)
                                        ,new SqlParameter("@Dob_month",objRegister.Dob_month==null ?"":objRegister.Dob_month)
                                        ,new SqlParameter("@Dob_year",objRegister.Dob_year==null ?"":objRegister.Dob_year)
                                          ,new SqlParameter("@Region",objRegister.Region==null ?"":objRegister.Region)
                                        ,new SqlParameter("@Country",objRegister.Country==null ?"":objRegister.Country)
                                        ,new SqlParameter("@DaytimeContactNumber",objRegister.DaytimeContactNumber==null ?"":objRegister.DaytimeContactNumber)
                                        ,new SqlParameter("@MobileTelephoneNumber",objRegister.MobileTelephoneNumber==null ?"":objRegister.MobileTelephoneNumber)
                                        ,new SqlParameter("@EmailAddress",objRegister.EmailAddress==null ?"":objRegister.EmailAddress)
                                          ,new SqlParameter("@UserName",objRegister.UserName==null ?"":objRegister.UserName)
                                        ,new SqlParameter("@Password",objRegister.Password==null ?"":objRegister.Password)
                                       ,pErrorCode
                                       ,pErrorMessage
                                      };

                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_RegisterUser, parmList);

                //Get & set Output Parameter values in objApplicationMappingModel 
                objRegister.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objRegister.Message = Convert.ToString(pErrorMessage.Value);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserLogin ChangePassword(UserLogin UserDetails)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", 0);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", "");
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = {
                                          new SqlParameter("@UserID",UserDetails.UserID)
                                        ,new SqlParameter("@Password",UserDetails.Password)
                                        ,new SqlParameter("RoleID",UserDetails.RoleID)
                                        ,pErrorCode
                                       ,pErrorMessage
                                      };

                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_CHANGEPASSWORD, parmList);

                UserDetails.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                //Get & set Output Parameter values in objApplicationMappingModel 
                return UserDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ForgotPassword(UserLogin UserDetails, out UserLogin outUserLogin)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", 0);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", "");
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = {
                                          new SqlParameter("@Email",UserDetails.Email)
                                        ,pErrorCode
                                       ,pErrorMessage
                                      };

                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_FORGOTPASSWORD, parmList);

                UserDetails.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                //Get & set Output Parameter values in objApplicationMappingModel 
                outUserLogin = UserDetails;
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region View  User List
        public DataTable GetUserList(ViewUserLoginModel objUserLoginList)
        {
            try
            {
                SqlParameter[] parmList = {

                                      new SqlParameter("@UserName", objUserLoginList.FilterUserName)
                                     ,new SqlParameter("@CurrentPage", objUserLoginList.CurrentPage)
                                     ,new SqlParameter("@PageSize", objUserLoginList.PageSize)
                                     ,new SqlParameter("@SortBy", objUserLoginList.SortBy)
                                     ,new SqlParameter("@SortOrder", objUserLoginList.SortOrder)

                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetUserList, parmList);

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
        /// <summary>
        /// Get User by UserId
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public DataTable GetUserListById(int UserId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@UserId",UserId),
                                      };
            //Call SPGETCATEGORYBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetUserByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }


        public String UpdateUserStatusByID(int UserId, int status)
        {
            try
            {
                //Set Param values by objViewParameters
                SqlParameter[] Param = {

                                           new SqlParameter("@UserID", UserId),
                                           new SqlParameter("@status", status)
                                       };

                //Call spGetDocumentResponse Procedure for view 
                return SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_UpdateUserStatusByID, Param).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #region User Section
        public DataSet SummaryView(int UserId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@UserId",UserId),
                                      };
            //Call SPGETCATEGORYBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.User_SummaryView, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds;
            return null;
        }

        public DataSet MyDocument(UserMyDocument objUserMyDocument)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@SearchType",objUserMyDocument.SearchType)
                                          ,new SqlParameter("@FolderID",objUserMyDocument.FolderId)
                                          ,new SqlParameter("@UserId",objUserMyDocument.UserId)
                                          ,new SqlParameter("@DocumentText",objUserMyDocument.FilterDocumentTitle)
                                           ,new SqlParameter("CurrentPage", objUserMyDocument.CurrentPage)
                                     ,new SqlParameter("PageSize", objUserMyDocument.PageSize)
                                     ,new SqlParameter("SortBy", objUserMyDocument.SortBy)
                                     ,new SqlParameter("SortOrder", objUserMyDocument.SortOrder)
                                      };
            //Call SPGETCATEGORYBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.User_MyDocument, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds;
            return null;
        }

        public DataSet MyTempletes(UserMyDocument objUserMyDocument)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@SearchType",objUserMyDocument.SearchType)
                                          ,new SqlParameter("@FolderID",objUserMyDocument.FolderId)
                                          ,new SqlParameter("@UserId",objUserMyDocument.UserId)
                                          ,new SqlParameter("@DocumentText",objUserMyDocument.FilterDocumentTitle)
                                           ,new SqlParameter("CurrentPage", objUserMyDocument.CurrentPage)
                                     ,new SqlParameter("PageSize", objUserMyDocument.PageSize)
                                     ,new SqlParameter("SortBy", objUserMyDocument.SortBy)
                                     ,new SqlParameter("SortOrder", objUserMyDocument.SortOrder)
                                      };
            //Call SPGETCATEGORYBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.User_SharedMyDocument, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds;
            return null;
        }

        public DataTable GetFolderByUserID(int UserId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@UserId",UserId),
                                      };
            //Call SPGETCATEGORYBYID stored procedure which will return dataset
            return SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetFolderByUserID, param);

        }


        public FolderModel InsertUpdateFolder(FolderModel objFolderModel)
        {
            try
            {
                //objFolderModel.FolderName = objFolderModel.FolderName.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = {
                                          new SqlParameter("@FolderID",objFolderModel.FolderId)
                                        ,new SqlParameter("@FolderName",objFolderModel.FolderName)
                                        , new SqlParameter("@UserId",objFolderModel.UserId)
                                        , new SqlParameter("@DocumentId",objFolderModel.DocumentId)
                                        ,new SqlParameter("@IsActive", objFolderModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objFolderModel.CreatedBy)
                                        ,pErrorCode
                                       ,pErrorMessage
                                          };

                //If  GroupId is 0 Than Insert  Group else Update  Group
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateFolder, parmList);
                //set error code and message 
                objFolderModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objFolderModel.Message = Convert.ToString(pErrorMessage.Value);
                return objFolderModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public ViewFolderModel DeleteFolder(ViewFolderModel objViewFolderModel)
        {

            SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewFolderModel.ErrorCode);
            pErrorCode.Direction = ParameterDirection.Output;
            pErrorCode.Size = 10;

            SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewFolderModel.Message);
            pErrorMessage.Direction = ParameterDirection.Output;
            pErrorMessage.Size = 100;

            SqlParameter[] parmList = {
                                           new SqlParameter("@FolderID",objViewFolderModel.DeletedFolderID)
                                         ,new SqlParameter("@CreatedBy",objViewFolderModel.UserId)
                                         ,pErrorCode
                                         ,pErrorMessage

                                        };
            //Call delete stored procedure to delete  category
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteFolder, parmList);
            //set output parameter error code and error message
            objViewFolderModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
            objViewFolderModel.Message = Convert.ToString(pErrorMessage.Value);
            return objViewFolderModel;

        }

        #endregion

        public DocumentModel RenameDocument(DocumentModel objDocumentModel)
        {
            try
            {
                //objFolderModel.FolderName = objFolderModel.FolderName.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = {
                                          new SqlParameter("@DocumentId",objDocumentModel.DocumentID)
                                        ,new SqlParameter("@DocuemntTitle",objDocumentModel.DocumentTitle)
                                        ,new SqlParameter("@CreatedBy",objDocumentModel.CreatedBy)
                                        ,pErrorCode
                                       ,pErrorMessage
                                          };

                //If  GroupId is 0 Than Insert  Group else Update  Group
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_RenameDocument, parmList);
                //set error code and message 
                objDocumentModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objDocumentModel.Message = Convert.ToString(pErrorMessage.Value);
                return objDocumentModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ViewFolderModel DeleteDocument(ViewFolderModel objViewFolderModel)
        {

            SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewFolderModel.ErrorCode);
            pErrorCode.Direction = ParameterDirection.Output;
            pErrorCode.Size = 10;

            SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewFolderModel.Message);
            pErrorMessage.Direction = ParameterDirection.Output;
            pErrorMessage.Size = 100;

            SqlParameter[] parmList = {
                                           new SqlParameter("@DocumentID",objViewFolderModel.DeletedDocumentID)
                                         ,new SqlParameter("@CreatedBy",objViewFolderModel.UserId)
                                         ,pErrorCode
                                         ,pErrorMessage

                                        };
            //Call delete stored procedure to delete  category
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_UserDeleteDocument, parmList);
            //set output parameter error code and error message
            objViewFolderModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
            objViewFolderModel.Message = Convert.ToString(pErrorMessage.Value);
            return objViewFolderModel;

        }

        public DataTable GetAllUserListForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetUserListForDDL, parmList);
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
        public List<DropdownModel> GetAllUserList()
        {
            try
            {
                List<DropdownModel> lstCategory = new List<DropdownModel>();
                //Get All  category list
                DataTable dtCategory = GetAllUserListForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtCategory.Rows)
                {
                    lstCategory.Add
                        (new DropdownModel()
                        {
                            ID = Convert.ToInt32(dr["UserID"]),
                            Value = Convert.ToString(dr["UserName"])
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

    }
}
