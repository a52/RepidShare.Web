using RepidShare.Data;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Business
{
    public class BLUserLogin : BLBase
    {
        DLUserLogin objDLUserLogin = new DLUserLogin();

        public UserLogin Login(UserLogin objUserLogin)
        {
            UserLogin outUserLogin = null;
            //Call DL for Insert Update  operation 
            DataSet ds = objDLUserLogin.Login(objUserLogin, out outUserLogin);

            if (outUserLogin != null && outUserLogin.ErrorCode == 0 && ds.Tables.Count>0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                outUserLogin = GetDataRowToEntity<UserLogin>(ds.Tables[0].Rows[0]);
            }

            if (outUserLogin != null && outUserLogin.ErrorCode == 0 && ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
            {
                outUserLogin.objMasterSetting = new MasterSettingModel();
                outUserLogin.objMasterSetting = GetDataRowToEntity<MasterSettingModel>(ds.Tables[1].Rows[0]);
            }
            return outUserLogin;            
        }

        public UserLogin Register(Register objRegister)
        {
            UserLogin outUserLogin = null;
            //Call DL for Insert Update  operation 
            DataTable dt = objDLUserLogin.Register(objRegister);

            if (objRegister != null && objRegister.ErrorCode == 0 && dt != null && dt.Rows.Count > 0)
            {
                outUserLogin = GetDataRowToEntity<UserLogin>(dt.Rows[0]);
            }
            else
            {
                outUserLogin = new UserLogin();
                outUserLogin.ErrorCode = 1;
            }
            return outUserLogin;
        }


        public UserLogin ForgotPassword(UserLogin objUserLogin)
        {
            UserLogin outUserLogin = null;
            //Call DL for Insert Update  operation 
            DataTable dt = objDLUserLogin.ForgotPassword(objUserLogin, out outUserLogin);

            if (outUserLogin != null && outUserLogin.ErrorCode == 0 && dt != null && dt.Rows.Count > 0)
            {
                outUserLogin = GetDataRowToEntity<UserLogin>(dt.Rows[0]);
            }
            return outUserLogin;
        }


        public UserLogin ChangePassword(UserLogin objUserLogin)
        {
            return objDLUserLogin.ChangePassword(objUserLogin);
        }

        #region View  Username
        /// <summary>
        /// Get  SubCategory List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewUserLoginModel">object of Model ViewSubCategoryModel</param>
        /// <returns></returns>
        public ViewUserLoginModel GetUsernameList(ViewUserLoginModel objViewUserLoginModel)
        {
            List<UserLogin> lstUserLogin = new List<UserLogin>();
            //if FilterSubCategoryName is NULL than set to empty
            objViewUserLoginModel.FilterUserName = objViewUserLoginModel.FilterUserName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewUserLoginModel.SortBy = objViewUserLoginModel.SortBy ?? String.Empty;

            //call GetSubCategoryList Method which will retrun datatable of  SubCategory
            DataTable dtSubCategory = objDLUserLogin.GetUserList(objViewUserLoginModel);

            //fetch each row of datable
            foreach (DataRow dr in dtSubCategory.Rows)
            {
                //Convert datarow into Model object and set Model object property
                UserLogin itemUserLogin = GetDataRowToEntity<UserLogin>(dr);
                //Add  SubCategory in List
                lstUserLogin.Add(itemUserLogin);
            }

            //set SubCategory List of Model ViewSubCategoryModel
            objViewUserLoginModel.UserLoginList = lstUserLogin;
            //if  SubCategory List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewUserLoginModel != null && objViewUserLoginModel.UserLoginList != null && objViewUserLoginModel.UserLoginList.Count > 0)
            {
                objViewUserLoginModel.CurrentPage = objViewUserLoginModel.UserLoginList[0].CurrentPage;
                int totalRecord = objViewUserLoginModel.UserLoginList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewUserLoginModel.PageSize) > 0)
                    objViewUserLoginModel.TotalPages = (totalRecord / objViewUserLoginModel.PageSize + 1);
                else
                    objViewUserLoginModel.TotalPages = totalRecord / objViewUserLoginModel.PageSize;
            }
            else
            {
                objViewUserLoginModel.TotalPages = 0;
                objViewUserLoginModel.CurrentPage = 1;
            }
            return objViewUserLoginModel;
        }



        /// <summary>
        /// Get User by UserId
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns>UserLogin</returns>
        public Register GetUserListById(int UserId)
        {
            //Call GetCategoryBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLUserLogin.GetUserListById(UserId);
            Register objRegister = new Register();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objRegister = GetDataRowToEntity<Register>(dt.Rows[0]);
            }

            return objRegister;
        }


        /// <summary>
        /// Get User by UserId
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns>UserLogin</returns>
        public void UpdateUserStatusByID(int UserId, int status)
        {
            //Call GetCategoryBYId method of dataLayer which will return Datatable.
            objDLUserLogin.UpdateUserStatusByID(UserId, status);


        }

        #endregion

        public UserSummaryModel SummaryView(int UserId)
        {
            UserSummaryModel objUserSummaryModel = new UserSummaryModel();

            DataSet dtSummaryModel = objDLUserLogin.SummaryView(UserId);

            if (dtSummaryModel != null && dtSummaryModel.Tables.Count > 0)
            {

                objUserSummaryModel = GetDataRowToEntity<UserSummaryModel>(dtSummaryModel.Tables[0].Rows[0]);

                if (dtSummaryModel != null && dtSummaryModel.Tables.Count > 1)
                {
                    objUserSummaryModel.objListActivityModel = new List<ActivityModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtSummaryModel.Tables[1].Rows)
                    {
                        objUserSummaryModel.objListActivityModel.Add(GetDataRowToEntity<ActivityModel>(dr));
                    }
                }
            }

            return objUserSummaryModel;

        }

        public UserMyDocument MyDocument(UserMyDocument objUserSummaryModel)
        {
            //UserMyDocument objUserSummaryModel = new UserMyDocument();

            int PageSize = objUserSummaryModel.PageSize;
            int CurrentPage = objUserSummaryModel.CurrentPage;
            string SortBy = objUserSummaryModel.SortBy;
            int SortOrder = objUserSummaryModel.SortOrder;

            DataSet dtSummaryModel = objDLUserLogin.MyDocument(objUserSummaryModel);

            if (dtSummaryModel != null && dtSummaryModel.Tables.Count > 0)
            {

                objUserSummaryModel = GetDataRowToEntity<UserMyDocument>(dtSummaryModel.Tables[0].Rows[0]);

                if (dtSummaryModel != null && dtSummaryModel.Tables.Count > 1)
                {
                    objUserSummaryModel.objListFolderModel = new List<FolderModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtSummaryModel.Tables[1].Rows)
                    {
                        objUserSummaryModel.objListFolderModel.Add(GetDataRowToEntity<FolderModel>(dr));
                    }
                }

                if (dtSummaryModel != null && dtSummaryModel.Tables.Count > 2)
                {
                    objUserSummaryModel.objListDocumentModel = new List<DocumentModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtSummaryModel.Tables[2].Rows)
                    {
                        objUserSummaryModel.objListDocumentModel.Add(GetDataRowToEntity<DocumentModel>(dr));
                    }
                }

                objUserSummaryModel.PageSize = PageSize;
                objUserSummaryModel.CurrentPage = CurrentPage;
                objUserSummaryModel.SortBy = SortBy;
                objUserSummaryModel.SortOrder = SortOrder;

                if (objUserSummaryModel != null && objUserSummaryModel.objListDocumentModel != null && objUserSummaryModel.objListDocumentModel.Count > 0)
                {
                    objUserSummaryModel.CurrentPage = objUserSummaryModel.objListDocumentModel[0].CurrentPage;
                    int totalRecord = objUserSummaryModel.objListDocumentModel[0].TotalCount;

                    if (decimal.Remainder(totalRecord, objUserSummaryModel.PageSize) > 0)
                        objUserSummaryModel.TotalPages = (totalRecord / objUserSummaryModel.PageSize + 1);
                    else
                        objUserSummaryModel.TotalPages = totalRecord / objUserSummaryModel.PageSize;
                }
                else
                {
                    objUserSummaryModel.TotalPages = 0;
                    objUserSummaryModel.CurrentPage = 1;
                }


            }

            
            return objUserSummaryModel;

        }

        public UserMyDocument MyTempletes(UserMyDocument objUserSummaryModel)
        {
            //UserMyDocument objUserSummaryModel = new UserMyDocument();

            int PageSize = objUserSummaryModel.PageSize;
            int CurrentPage = objUserSummaryModel.CurrentPage;
            string SortBy = objUserSummaryModel.SortBy;
            int SortOrder = objUserSummaryModel.SortOrder;

            DataSet dtSummaryModel = objDLUserLogin.MyTempletes(objUserSummaryModel);

            if (dtSummaryModel != null && dtSummaryModel.Tables.Count > 0)
            {

                objUserSummaryModel = GetDataRowToEntity<UserMyDocument>(dtSummaryModel.Tables[0].Rows[0]);

                if (dtSummaryModel != null && dtSummaryModel.Tables.Count > 1)
                {
                    objUserSummaryModel.objListFolderModel = new List<FolderModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtSummaryModel.Tables[1].Rows)
                    {
                        objUserSummaryModel.objListFolderModel.Add(GetDataRowToEntity<FolderModel>(dr));
                    }
                }

                if (dtSummaryModel != null && dtSummaryModel.Tables.Count > 2)
                {
                    objUserSummaryModel.objListDocumentModel = new List<DocumentModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtSummaryModel.Tables[2].Rows)
                    {
                        objUserSummaryModel.objListDocumentModel.Add(GetDataRowToEntity<DocumentModel>(dr));
                    }
                }

                objUserSummaryModel.PageSize = PageSize;
                objUserSummaryModel.CurrentPage = CurrentPage;
                objUserSummaryModel.SortBy = SortBy;
                objUserSummaryModel.SortOrder = SortOrder;

                if (objUserSummaryModel != null && objUserSummaryModel.objListDocumentModel != null && objUserSummaryModel.objListDocumentModel.Count > 0)
                {
                    objUserSummaryModel.CurrentPage = objUserSummaryModel.objListDocumentModel[0].CurrentPage;
                    int totalRecord = objUserSummaryModel.objListDocumentModel[0].TotalCount;

                    if (decimal.Remainder(totalRecord, objUserSummaryModel.PageSize) > 0)
                        objUserSummaryModel.TotalPages = (totalRecord / objUserSummaryModel.PageSize + 1);
                    else
                        objUserSummaryModel.TotalPages = totalRecord / objUserSummaryModel.PageSize;
                }
                else
                {
                    objUserSummaryModel.TotalPages = 0;
                    objUserSummaryModel.CurrentPage = 1;
                }


            }


            return objUserSummaryModel;

        }

        #region Get ,Insert, Update and delete Folder
        public List<FolderModel> GetFolderByUserId(int UserId)
        {
            //Call GetGroupBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLUserLogin.GetFolderByUserID(UserId);
            List<FolderModel> objFolderModel = new List<FolderModel>();
            // if datatable has row than set object parameters
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objFolderModel.Add(GetDataRowToEntity<FolderModel>(dt.Rows[i]));
                }
            }

            return objFolderModel;
        }


        public FolderModel InsertUpdateFolder(FolderModel objFolderModel)
        {
            //call InsertUpdateGroup Method of dataLayer and return GroupModel
            return objDLUserLogin.InsertUpdateFolder(objFolderModel);
        }

        public ViewFolderModel DeleteFolder(ViewFolderModel objViewFolderModel)
        {
            return objDLUserLogin.DeleteFolder(objViewFolderModel);
        }

        public DocumentModel RenameDocuemnt(DocumentModel objDocumentModel)
        {
            return objDLUserLogin.RenameDocument(objDocumentModel);
        }

        public ViewFolderModel DeleteDocument(ViewFolderModel objViewFolderModel)
        {
            return objDLUserLogin.DeleteDocument(objViewFolderModel);
        }

        #endregion

        #region  User DropDown
        /// <summary>
        /// Get  User DropDown Item
        /// </summary>
        /// <returns></returns>
        ///<remarks>
        /// Created By Rakesh Patidar , 11 Feb 2015
        ///</remarks>
        public List<DropdownModel> FillUserDropDown()
        {
            //Get All Category List 
            List<DropdownModel> lstUser = objDLUserLogin.GetAllUserList().ToList();
            return lstUser;
        }
 
        #endregion

    }
}
