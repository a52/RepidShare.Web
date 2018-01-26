using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepidShare.Entities;
using System.Web.Configuration;
using System.Configuration;
using RepidShare.Utility;
using System.Net.Http;
using System.Net;
using RepidShare.Admin.Controllers;


namespace RepidShare.Admin.Controllers
{
    public class UserController : BaseController
    {
        HttpResponseMessage serviceResponse;
        CommonUtils objCommonUtils = new CommonUtils();
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        /// <summary>
        /// Set createdBy 
        /// </summary>
        public int createdBy
        {
            get
            {
                int _userID = 0;
                if (Session["UserId"] != null)
                    int.TryParse(Convert.ToString(Session["UserId"]), out _userID);
                return _userID;
            }
        }

        #region [Login]
        /// <summary>
        /// Load Login Page
        /// </summary>
        public ActionResult Login()
        {
            UserLogin objUserLogin = new UserLogin();
            try
            {
                //Check SuccessMessage != null
                if (Session["SuccessMessage"] != null && Convert.ToString(Session["SuccessMessage"]) != "")
                {
                    objUserLogin.Message = Session["SuccessMessage"].ToString();
                    objUserLogin.MessageType = RepidShare.Utility.CommonUtils.MessageType.Success.ToString().ToLower();
                    Session["SuccessMessage"] = null;
                }
                //Check ErrorMessage != null
                if (Session["ErrorMessage"] != null && Convert.ToString(Session["ErrorMessage"]) != "")
                {
                    objUserLogin.Message = Session["ErrorMessage"].ToString();
                    objUserLogin.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                    Session["ErrorMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                //ErrorLog(createdBy.ToString(), "User", "Login GET", ex);

            }
            return View(objUserLogin);
        }

        /// <summary>
        /// Check User Credentials 
        /// </summary>
        /// <param name="objUserLogin"></param>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login(UserLogin objUserLogin)
        {

            UtilityWeb objUtilityWeb = new UtilityWeb();
            try
            {
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/UserLogin", objUserLogin);
                objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;

                if (objUserLogin == null)
                {
                    //Set Error msg 
                    objUserLogin.Message = "Please contact with Admin";
                    objUserLogin.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                    //ModelState.AddModelError("", "Please contact with Admin");
                    return View(objUserLogin);
                }

                if (objUserLogin.ErrorCode > 0)
                {
                    objUserLogin.Message = "Invalid UserName Or Password";
                    objUserLogin.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                    return View(objUserLogin);
                }

                //Added By Rakesh
                Session["EmailId"] = objUserLogin.Email;
                Session["RoleID"] = objUserLogin.RoleID;
                Session["UserId"] = objUserLogin.UserID;
                Session["UserFirstLastName"] = objUserLogin.FName + " " + objUserLogin.LName;

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //  Session["ExceptionMsg"] = objCommonUtils.ErrorLog("0", "Account", "Login Post", ex);
            }
            return RedirectToAction("Login", "User");
        }

        #endregion

        #region Forget Password

        /// <summary>
        /// View Forget password.
        /// </summary>
        /// <param name="pd" and "ud"></param> 
        public ActionResult ForgotPassword()
        {
            ChangePasswordModel objChangePasswordModel = new ChangePasswordModel();
            //Call ChangePassword View
            return View(objChangePasswordModel);
        }

        /// <summary>
        /// Performs Forget password operation
        /// </summary>
        /// <param name="objChangePasswordModel"></param>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ForgotPassword(ChangePasswordModel objChangePasswordModel)
        {
            UtilityWeb objUtilityWeb = new UtilityWeb();
            try
            {
                UserLogin objUserLogin = new RepidShare.Entities.UserLogin();
                EmailTemplate objEmailTemplate = new EmailTemplate();
                objUserLogin.Email = objChangePasswordModel.EmailID;
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/ForgotPassword", objUserLogin);
                objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(objChangePasswordModel.EmailID);
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Email + "/GetEmailSettingByEmailID?EmailID="+ Convert.ToInt32(RepidShare.Utility.CommonUtils.EmailType.ForgotPassword));
                objEmailTemplate = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<EmailTemplate>().Result : null;
                message.Subject = objEmailTemplate.EmailSubject;
                    //"Forgot Password By Papeleslegales.com";
                message.Body = objEmailTemplate.Content+ "<br/>Your Password According to our records is: , <h3>" + objUserLogin.Password + "</h3>";;
                    //"<p>Your password has been reset, <h3> " + objUserLogin.UserName + "</h3></p>  </br><p> According to our records, you have requested that your password be reset. Your new password is:<h3>" + objUserLogin.Password + "<h3/></p> </br></br><p>If you have any questions or trouble logging on please contact a site administrator.</p><br/><br/><br/>Thank you!";
                message.IsBodyHtml = true;

                if (objUserLogin.ErrorCode == 0)
                {
               
                   if (RepidShare.Utility.CommonUtils.Send(message))
                       // Session["SuccessMessage"] = "E-Mail Sent Successfully!";
                    objChangePasswordModel.Message = "<p style='color:green;'>Your Passowrd has been sent to your account. for login click <a href='http://localhost:49339/User/Login'> Login <a></p>";
                }
                else
                {
                  //  Session["ErrorMessage"] = "Oppps, Your Account is not valid!!!";
                   objChangePasswordModel.Message = "<p style='color:red;'>Oppps, Your Account is not valid!!!</p>";
                }

               // return RedirectToAction("Login", "User");
                return View("ForgotPassword", objChangePasswordModel);
            }
            catch (Exception ex)
            {
                //Session["ExceptionMsg"] = objCommonUtils.ErrorLog(createdBy.ToString(), "User", "ForgotPassword Post", ex);
            }

            return View("ForgotPassword", objChangePasswordModel);
        }



        #endregion

        #region ChangePassword

        public ActionResult ChangePassword()
        {
            ChangePasswordModel objChangePasswordModel = new ChangePasswordModel();
            try
            {
                return View(objChangePasswordModel);
            }
            catch (Exception ex)
            {
                //Session["ExceptionMsg"] = objCommonUtils.ErrorLog(createdBy.ToString(), "User", "ChangePassword Get", ex);
            }
            return View(objChangePasswordModel);
        }

        /// <summary>
        /// Change password of user.
        /// </summary>
        /// <param name="objChangePasswordModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ChangePassword(ChangePasswordModel objChangePasswordModel)
        {
            try
            {
                UtilityWeb objUtilityWeb = new UtilityWeb();
                UserLogin objUserLogin = new RepidShare.Entities.UserLogin();
                objUserLogin.UserID = LoggedInUserID;
                objUserLogin.Password = objChangePasswordModel.ConfirmPassword;
                objUserLogin.RoleID = RoleID;

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/ChangePassword", objUserLogin);
                objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;

                if (objUserLogin == null)
                {
                    //Set Error msg 
                    objChangePasswordModel.Message = "Please contact with Admin";
                    objChangePasswordModel.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                }

                if (objUserLogin.ErrorCode > 0)
                {
                    objChangePasswordModel.Message = "Invalid UserName Or Password";
                    objChangePasswordModel.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                }

                if (objUserLogin.ErrorCode == 0)
                {
                    objChangePasswordModel.Message = "Password changed successfully.";
                    objChangePasswordModel.MessageType = RepidShare.Utility.CommonUtils.MessageType.Success.ToString().ToLower();
                }
                return View(objChangePasswordModel);

            }
            catch (Exception ex)
            {
                //  Session["ExceptionMsg"] = objCommonUtils.ErrorLog(createdBy.ToString(), "User", "ChangePassword Post", ex);
            }

            return View(objChangePasswordModel);
        }

        #endregion

        #region View  User Name
        /// <summary>
        /// View  SubCategory List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult List()
        {
            ViewUserLoginModel ObjViewUserLoginModel = new ViewUserLoginModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewUserLoginModel.CurrentPage = 1;
                ObjViewUserLoginModel.PageSize = CommonUtils.PageSize;
                ObjViewUserLoginModel.TotalPages = 0;
                //Get  SubCategory List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/GetUsernameList", ObjViewUserLoginModel);
                ObjViewUserLoginModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewUserLoginModel>().Result : null;
                //ObjViewSubCategoryModel = objBLSubCategory.GetSubCategoryList(ObjViewSubCategoryModel);

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "UserList", "View GET");
            }
            return View("ViewUserList", ObjViewUserLoginModel);
        }

        /// <summary>
        /// View  SubCategory List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewUserLoginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult List(ViewUserLoginModel objViewUserLoginModel)
        {
            try
            {
                objViewUserLoginModel.Message = objViewUserLoginModel.MessageType = String.Empty;
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/GetUsernameList", objViewUserLoginModel);
                objViewUserLoginModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewUserLoginModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "UserList", "View POST");
            }
            return PartialView("_UserList", objViewUserLoginModel);
        }


        [Filters.Authorized]
        public ActionResult ActivateDeactivateUser(string prm = "")
        {
            UserLogin objUserLogin = new UserLogin();

            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int UserId;
                    int Status;
                    //decrypt parameter and set in CategoryId variable
                    int.TryParse(CommonUtils.Decrypt(prm.Split('~')[0]), out UserId);
                    int.TryParse(prm.Split('~')[1], out Status);
                    //Get Category detail by  Category Id

                    //serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetUserListById?UserId=" + UserId.ToString());
                    //objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;
                    //if (objUserLogin != null)
                    //{

                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/UpdateUserStatusByID?UserId=" + UserId.ToString() + "&status=" + Status);
                    objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;


                    //Admin_UpdateUserStatusByID

                    //}


                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "User", "ActivateDeactivateUser");
            }

            return RedirectToAction("List");
        }
        #endregion
    }
}
