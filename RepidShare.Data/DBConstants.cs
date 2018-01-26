using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Data
{
    public static class DBConstants
    {
        #region Error Log
        public const string ADMIN_INSERTERRORLOG = "Admin_InsertErrorLog";
        #endregion

        #region Login
        public const string PROC_UserLogin = "UserLogin";
        public const string Web_RegisterUser = "Web_RegisterUser";
        public const string ADMIN_CHANGEPASSWORD = "Admin_ChangePassword";
        public const string ADMIN_FORGOTPASSWORD = "Admin_ForgotPassword";
        #endregion

        #region Category
        public const string Admin_DeleteCategory = "Admin_DeleteCategory";
        public const string Admin_GetCategoryByID = "Admin_GetCategoryByID";
        public const string Admin_GetCategoryList = "Admin_GetCategoryList";
        public const string Admin_GetCategoryListForDDL = "Admin_GetCategoryListForDDL";
        public const string Admin_InsertUpdateCategory = "Admin_InsertUpdateCategory";
        // Narayan
        public const string Admin_UpdateCategoryStatusByID = "Admin_UpdateCategoryStatusByID";

        #endregion

        #region Sub Category
        public const string Admin_DeleteSubCategory = "Admin_DeleteSubCategory";
        public const string Admin_GetSubCategoryByID = "Admin_GetSubCategoryByID";
        public const string Admin_GetSubCategoryList = "Admin_GetSubCategoryList";
        public const string Admin_GetSubCategoryListForDDL = "Admin_GetSubCategoryListForDDL";
        public const string Admin_InsertUpdateSubCategory = "Admin_InsertUpdateSubCategory";

        // Narayan
        public const string Admin_UpdateSubCategoryStatusByID = "Admin_UpdateSubCategoryStatusByID";
        #endregion

        #region Step
        public const string Admin_DeleteStep = "Admin_DeleteStep";
        public const string Admin_GetStepByID = "Admin_GetStepByID";
        public const string Admin_GetStepList = "Admin_GetStepList";
        public const string Admin_GetStepListForDDL = "Admin_GetStepListForDDL";
        public const string Admin_GetStepListForDDLByDocumentId = "Admin_GetStepListForDDLByDocumentId";
        public const string Admin_GetStepByDocumentIdANDUserID = "Admin_GetStepByDocumentIdANDUserID";
        public const string Admin_InsertUpdateStep = "Admin_InsertUpdateStep";
        #endregion

        #region Group
        public const string Admin_DeleteGroup = "Admin_DeleteGroup";
        public const string Admin_GetGroupByID = "Admin_GetGroupByID";
        public const string Admin_GetGroupList = "Admin_GetGroupList";
        public const string Admin_GetGroupListForDDL = "Admin_GetGroupListForDDL";
        public const string Admin_InsertUpdateGroup = "Admin_InsertUpdateGroup";
        #endregion

        #region Question
        public const string ADMIN_GETQUESTIONTYPELIST = "Admin_GetQuestionTypeList";
        public const string ADMIN_GETQUESTIONDETAILBYID = "Admin_GetQuestionDetailByID";
        public const string ADMIN_GETQUESTIONLIST = "Admin_GetQuestionList";
        public const string ADMIN_INSERTUPDATEQUESTION = "Admin_InsertUpdateQuestion";
        public const string ADMIN_DELETEQUESTION = "Admin_DeleteQuestion";
        public const string ADMIN_GETALLDISPLAYCHOICE = "Admin_GetAllDisplayChoice";
        #endregion

        #region Step Doc Mapped
        public const string ADMIN_GETSTEPDOCMAPPEDBYID = "Admin_GetStepDocMappedByID";
        public const string ADMIN_DELETESTEPDOCMAPPED = "Admin_DeleteStepDocMapped";
        public const string ADMIN_GETSTEPDOCMAPPEDBYDOCUMENTID = "Admin_GetStepDocMappedByDocumentID";
        public const string ADMIN_INSERTUPDATESTEPDOCMAPPED = "Admin_InsertUpdateStepDocMapped";
        #endregion

        #region Document
        public const string Admin_DeleteDocument = "Admin_DeleteDocument";
        public const string Admin_GetDocumentByID = "Admin_GetDocumentByID";
        public const string Admin_GetDocumentList = "Admin_GetDocumentList";
        public const string Admin_InsertUpdateDocument = "Admin_InsertUpdateDocument";

        public const string WEB_GetDocumentListForResponse = "WEB_GetDocumentListForResponse";
        public const string WEB_GetDocumentResponseUserList = "WEB_GetDocumentResponseUserList";
        public const string WEB_GetUserDocumentList = "WEB_GetUserDocumentList";
        #endregion

        #region DropDown
        public const string Admin_DeleteDropDown = "Admin_DeleteDropDown";
        public const string Admin_GetDropDownByID = "Admin_GetDropDownByID";
        public const string Admin_GetDropDownList = "Admin_GetDropDownList";
        public const string Admin_GetDropDownListForDDL = "Admin_GetDropDownListForDDL";
        public const string Admin_InsertUpdateDropDown = "Admin_InsertUpdateDropDown";
        #endregion

        #region Service
        public const string Admin_DeleteMaster = "Admin_DeleteMaster";
        public const string Admin_GetMasterByID = "Admin_GetMasterByID";
        public const string Admin_GetMasterList = "Admin_GetMasterList";
        public const string Admin_GetMasterListForDDL = "Admin_GetMasterListForDDL";
        public const string Admin_InsertUpdateMaster = "Admin_InsertUpdateMaster";
        #endregion

        #region DocumentResponse
        public const string Web_GETDocumentRESPONSE = "[dbo].[Web_GetDocumentResponse]";
        public const string Web_GETDocumentRESPONSEFORSAVE = "[dbo].[Web_GetDocumentResponseForSave]";
        public const string Web_GetDocumentResponseForSaveStep = "[dbo].[Web_GetDocumentResponseForSaveStep]";

        public const string Web_GETDocumentRESPONSEFORVIEW = "[dbo].[Web_GetDocumentResponseForView]";
        public const string Web_GETDocumentPREVIEW = "[dbo].[Web_GetDocumentPreview]";
        public const string Web_INSERTUPDATEDocumentRESPONSE = "[dbo].[Web_InsertUpdateDocumentResponse]";

        public const string Web_GetDocumentPreviewTemp = "Web_GetDocumentPreviewTemp";
        public const string Admin_GetDocumentHistory = "Admin_GetDocumentHistory";

        public const string Web_InsertUpdatePriceQuestion = "Web_InsertUpdatePriceQuestion";
        public const string Web_GetPriceQuestion = "Web_GetPriceQuestion";


        #endregion

        #region USER
        public const string Admin_GetUserList = "Admin_GetUserList";
        // aded by  Narayan
        public const string Admin_GetUserByID = "Admin_GetUserByID";
        public const string Admin_UpdateUserStatusByID = "Admin_UpdateUserStatusByID";
        public const string Admin_GetEmailSettingByEmailID = "Admin_GetEmailSettingByEmailID";
        public const string Admin_EmailTypeDrowdown = "Admin_EmailTypeDrowdown";
        public const string Admin_EmailTemplateList = "Admin_EmailTemplateList";
        public const string Admin_GetEmailTemplateByID = "Admin_GetEmailTemplateByID";
        public const string Admin_InsertUpdateEmailDetail = "Admin_InsertUpdateEmailDetail";
        public const string Admin_DeleteEmailTemplate = "Admin_DeleteEmailTemplate";
        #endregion

        #region Home
        public const string Web_CategoryView = "Web_CategoryView";
        public const string Web_SubCategoryView = "Web_SubCategoryView";
        public const string Web_LawGuideView = "Web_LawGuideView";
        public const string Web_DocumentView = "Web_DocumentView";
        #endregion

        #region Law Guide
        public const string Admin_DeleteLawGuide = "Admin_DeleteLawGuide";
        public const string Admin_GetLawGuideByID = "Admin_GetLawGuideByID";
        public const string Admin_GetLawGuideList = "Admin_GetLawGuideList";
        public const string Admin_GetLawGuideListForDDL = "Admin_GetLawGuideListForDDL";
        public const string Admin_InsertUpdateLawGuide = "Admin_InsertUpdateLawGuide";
        #endregion

        #region User Section
        public const string User_SummaryView = "User_SummaryView";
        public const string User_MyDocument = "User_MyDocument";
        public const string User_SharedMyDocument = "User_SharedMyDocument";
        public const string Admin_GetFolderByUserID = "Admin_GetFolderByUserID";
        public const string Admin_DeleteFolder = "Admin_DeleteFolder";
        public const string Admin_InsertUpdateFolder = "Admin_InsertUpdateFolder";
        public const string Admin_RenameDocument = "Admin_RenameDocument";
        public const string Admin_UserDeleteDocument = "Admin_UserDeleteDocument";

        public const string Admin_GetUserListForDDL = "Admin_GetUserListForDDL";
        #endregion

        #region Order
        public const string Admin_GetOrderList = "Admin_GetOrderList";
        public const string Admin_GetOrderDetailList = "Admin_GetOrderDetailList";
        public const string Admin_InsertUpdateOrder = "Admin_InsertUpdateOrder";
        #endregion

        #region MasterSetting
        public const string Admin_GetMasterSettingByID = "Admin_GetMasterSettingByID";
        public const string Admin_InsertUpdateMasterSetting = "Admin_InsertUpdateMasterSetting";
        #endregion

        #region Coupen
        public const string Admin_DeleteCoupen = "Admin_DeleteCoupen";
        public const string Admin_GetCoupenByID = "Admin_GetCoupenByID";
        public const string Admin_GetCoupenList = "Admin_GetCoupenList";
        public const string Admin_GetCoupenListForDDL = "Admin_GetCoupenListForDDL";
        public const string Admin_InsertUpdateCoupen = "Admin_InsertUpdateCoupen";
        #endregion

        #region Bulletin
        public const string Admin_DeleteBulletin = "Admin_DeleteBulletin";
        public const string Admin_GetBulletinByID = "Admin_GetBulletinByID";
        public const string Admin_GetBulletinList = "Admin_GetBulletinList";
        public const string Admin_GetBulletinListForDDL = "Admin_GetBulletinListForDDL";
        public const string Admin_InsertUpdateBulletin = "Admin_InsertUpdateBulletin";
        // Narayan
        public const string Admin_UpdateBulletinStatusByID = "Admin_UpdateBulletinStatusByID";

        #endregion

        #region How It Works
        public const string Admin_GetHowItWorkdsById = "Admin_GetHowItWorkdsById";
        #endregion

    }
}