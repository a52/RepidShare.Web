using RepidShare.Business;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RepidShare.API.Controllers
{
    public class UserLoginController : ApiController
    {
        BLUserLogin objBusUserLogin = new BLUserLogin();

        [HttpPost]
        public UserLogin UserLogin(UserLogin logon)
        {
            return objBusUserLogin.Login(logon);  
        }

        [HttpPost]
        public UserLogin Register(Register objRegister)
        {
            return objBusUserLogin.Register(objRegister);
        }

        [HttpPost]
        public UserLogin ChangePassword(UserLogin logon)
        {
            return objBusUserLogin.ChangePassword(logon);
        }

        [HttpPost]
        public UserLogin ForgotPassword(UserLogin logon)
        {
            return objBusUserLogin.ForgotPassword(logon);
        }

        [HttpPost]
        public ViewUserLoginModel GetUsernameList(ViewUserLoginModel objViewUserLoginModel)
        {
            return objBusUserLogin.GetUsernameList(objViewUserLoginModel);

        }

        [HttpGet]
        public Register GetUserListById(int UserId)
        {

            return objBusUserLogin.GetUserListById(UserId);
        }


        [HttpGet]
        public void UpdateUserStatusByID(int UserId, int status)
        {

            objBusUserLogin.UpdateUserStatusByID(UserId, status);
        }

        [HttpGet]
        public UserSummaryModel SummaryView(int UserId)
        {
            return objBusUserLogin.SummaryView(UserId);
        }

        [HttpPost]
        public UserMyDocument MyDocument(UserMyDocument objUserMyDocument)
        {
            return objBusUserLogin.MyDocument(objUserMyDocument);
        }

        [HttpPost]
        public UserMyDocument MyTempletes(UserMyDocument objUserMyDocument)
        {
            return objBusUserLogin.MyTempletes(objUserMyDocument);
        }

        #region Get ,Insert, Update and delete Group

        [HttpGet]
        public List<FolderModel> GetFolderById(int UserId)
        {
            return objBusUserLogin.GetFolderByUserId(UserId);
        }

        [HttpPost]
        public FolderModel InsertUpdateFolder(FolderModel objFolderModel)
        {
            return objBusUserLogin.InsertUpdateFolder(objFolderModel);
        }

        [HttpPost]
        public ViewFolderModel DeleteFolder(ViewFolderModel objViewFolderModel)
        {
            return objBusUserLogin.DeleteFolder(objViewFolderModel);
        }

        [HttpPost]
        public DocumentModel RenameDocuemnt(DocumentModel objDocumentModel)
        {
            return objBusUserLogin.RenameDocuemnt(objDocumentModel);
        }

        [HttpPost]
        public ViewFolderModel DeleteDocument(ViewFolderModel objViewFolderModel)
        {
            return objBusUserLogin.DeleteDocument(objViewFolderModel);
        }

        #endregion

        [HttpGet]
        public List<DropdownModel> FillUserDropDown()
        {
            return objBusUserLogin.FillUserDropDown();
        }
    }
}
