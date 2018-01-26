using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using PayPal.Api;
using RepidShare.Entities;
using RepidShare.Utility;
using RepidShare.Web.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RepidShare.Web.Controllers
{
    [LayOutData]
    public class UserController : BaseController
    {
        HttpResponseMessage serviceResponse;
        CommonUtils objCommonUtils = new CommonUtils();

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
                    //objUserLogin.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                    //ModelState.AddModelError("", "Please contact with Admin");
                    return View(objUserLogin);
                }

                if (objUserLogin.ErrorCode > 0)
                {
                    objUserLogin.Message = "Invalid UserName Or Password";
                    //objUserLogin.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                    return View(objUserLogin);
                }

                //Added By Rakesh
                Session["EmailId"] = objUserLogin.Email;
                Session["RoleID"] = objUserLogin.RoleID;
                Session["UserId"] = objUserLogin.UserID;
                Session["UserFirstLastName"] = objUserLogin.FName + " " + objUserLogin.LName;
                Session["MasterSetting"] = objUserLogin.objMasterSetting;              
                ModelState.Clear();

                UserLogin objRegisterNew = new UserLogin();
                objRegisterNew.Message = "User login successfully.";
                //return View("Welcome");
                return RedirectToAction("SummaryView", "User");
            }
            catch (Exception ex)
            {
                //  Session["ExceptionMsg"] = objCommonUtils.ErrorLog("0", "Account", "Login Post", ex);
            }
            return RedirectToAction("Login", "User");
        }

        #endregion

        #region New Login Functionality
        [AllowAnonymous]
        public ActionResult PartialLogin(string userName, string password)
        {
            UserLogin objUserLogin = new UserLogin();
            UtilityWeb objUtilityWeb = new UtilityWeb();

            objUserLogin.UserName = userName;
            objUserLogin.Password = password;

            object Response = null;
            try
            {

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/UserLogin", objUserLogin);
                objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;

                if (objUserLogin == null)
                {
                    Response = new { status = "Fail", message = "Please contact with Admin", returnUrl = "" };
                    return Json(Response, JsonRequestBehavior.AllowGet);
                }

                if (objUserLogin.ErrorCode > 0)
                {
                    Response = new { status = "Fail", message = "Invalid UserName Or Password", returnUrl = "" };
                    return Json(Response, JsonRequestBehavior.AllowGet);

                }
                //Added By Rahul
                Session["EmailId"] = objUserLogin.Email;
                Session["RoleID"] = objUserLogin.RoleID;
                Session["UserId"] = objUserLogin.UserID;
                Session["UserFirstLastName"] = objUserLogin.FName + " " + objUserLogin.LName;
                Session["MasterSetting"] = objUserLogin.objMasterSetting;               
               //Response = new { status = "Ok", message = "", returnUrl = "User/SummaryView" };
                 Response = new { status = "Ok", message = "", returnUrl = "/" };              
                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // If we got this far, something failed, redisplay form

            Response = new { status = "Fail", message = "", returnUrl = "" };
            return Json(Response, JsonRequestBehavior.AllowGet);

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

                // Code commented by Rahul P
                //serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Email + "/GetEmailSettingByEmailID?EmailID=" + Convert.ToInt32(RepidShare.Utility.CommonUtils.EmailType.ForgotPassword));
                //objEmailTemplate = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<EmailTemplate>().Result : null;
                //message.Subject = objEmailTemplate.EmailSubject;
                ////"Forgot Password By Papeleslegales.com";
                // message.Body = objEmailTemplate.Content + "<br/>Your Password According to our records is: , <h3>" + objUserLogin.Password + "</h3>"; ;

                message.Subject = "Papeleslegales Password Recovery";

                message.Body = "Hi, " + objUserLogin.UserName + " <br/><br/> As requested, here is your login details<br/><br/> Your Username: " + objUserLogin.UserName + " <br/><br/> Your Password: " + objUserLogin.Password + " <br/><br/> Thank you for trusting us!Hope to hear from you soon!<br/><br/> ";
                message.IsBodyHtml = true;

                if (objUserLogin.ErrorCode == 0)
                {
                    if (RepidShare.Utility.CommonUtils.Send(message))
                        objChangePasswordModel.Message = "<p style='color:green;'>Your Password  has been sent to your account. for Login click <a href='http://localhost:49208/User/Login'> Login <a></p>";
                    //Session["SuccessMessage"] = "E-Mail Sent Successfully!";
                }
                else
                {
                    //Session["ErrorMessage"] = "Oppps, Your Account is not valid!!!";
                    objChangePasswordModel.Message = "<p style='color:red;'>Oppps, Your Account is not valid!!!</p>";
                }

                //  return RedirectToAction("Login", "User");
                return View("ForgotPassword", objChangePasswordModel);
            }
            catch (Exception ex)
            {
                //Session["ExceptionMsg"] = objCommonUtils.ErrorLog(createdBy.ToString(), "User", "ForgotPassword Post", ex);
            }

            return View("ForgotPassword", objChangePasswordModel);
        }



        #endregion

        #region Forget UserName

        /// <summary>
        /// View Forget password.
        /// </summary>
        /// <param name="pd" and "ud"></param> 
        public ActionResult ForgotUserName()
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
        public ActionResult ForgotUserName(ChangePasswordModel objChangePasswordModel)
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

                // Code commented by Rahul P
                //serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Email + "/GetEmailSettingByEmailID?EmailID=" + Convert.ToInt32(RepidShare.Utility.CommonUtils.EmailType.ForgotUsername));
                //objEmailTemplate = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<EmailTemplate>().Result : null;
                //message.Subject = objEmailTemplate.EmailSubject;
                ////"Forgot Password By Papeleslegales.com";
                //message.Body = objEmailTemplate.Content + "<br/>Your UserName According to our records is: , <h3>" + objUserLogin.UserName + "</h3>";


                message.Subject = "Papeleslegales Username Recovery";
                message.Body = "Hi, " + objUserLogin.UserName + " <br/><br/> As requested, your username is<br/><br/> Your Username: " + objUserLogin.UserName + "<br/><br/> Thank you for trusting us!Hope to hear from you soon!<br/><br/> ";

                message.IsBodyHtml = true;

                if (objUserLogin.ErrorCode == 0)
                {
                    if (RepidShare.Utility.CommonUtils.Send(message))
                    {
                        //    Session["SuccessMessage"] = "E-Mail Sent Successfully!";
                        // ViewBag.UserNameSucessMessgage = "your user name has been sent to your accont. for login click <a href='http://localhost:49208/User/Login'>Click on link <a>";
                        objChangePasswordModel.Message = "<p style='color:green;'>Your UserName has been sent to your account. for login click <a href='http://localhost:49208/User/Login'> Login <a></p>";
                    }
                    // Session["SuccessMessage"] = "E-Mail Sent Successfully!";

                }
                else
                {
                    //  Session["ErrorMessage"] = "Oppps, Your Account is not valid!!!";
                    objChangePasswordModel.Message = "<p style='color:red;'>Oppps, Your Account is not valid!!!</p>";
                }

                /// return RedirectToAction("ForgotUserName", "User");

                return View("ForgotUserName", objChangePasswordModel);
            }
            catch (Exception ex)
            {
                //Session["ExceptionMsg"] = objCommonUtils.ErrorLog(createdBy.ToString(), "User", "ForgotPassword Post", ex);
            }

            return View("ForgotUserName", objChangePasswordModel);
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

        #region

        public ActionResult Register()
        {
            Register objRegister = new Register();
            return View(objRegister);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Register(Register objRegister)
        {
            UtilityWeb objUtilityWeb = new UtilityWeb();
            try
            {
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/Register", objRegister);
                UserLogin objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;

                if (objUserLogin == null)
                {
                    //Set Error msg 
                    objRegister.Message = "Please contact with Admin";
                    objRegister.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                    //ModelState.AddModelError("", "Please contact with Admin");
                    return View(objRegister);
                }
                else if (objUserLogin.ErrorCode > 0)
                {
                    //Set Error msg 

                    objRegister.Message = "user name already taken.";
                    objRegister.MessageType = RepidShare.Utility.CommonUtils.MessageType.Error.ToString().ToLower();
                    //ModelState.AddModelError("", "Please contact with Admin");
                    return View(objRegister);
                }

                //Added By Rakesh
                Session["EmailId"] = objUserLogin.Email;
                Session["RoleID"] = objUserLogin.RoleID;
                Session["UserId"] = objUserLogin.UserID;
                Session["UserFirstLastName"] = objUserLogin.FName + " " + objUserLogin.LName;

                //string EncryptUserID = RepidShare.Utility.CommonUtils.Encrypt(objUserLogin.UserID.ToString(),true);
                //string WebURL = ConfigurationManager.AppSettings["WebURL"];

                // string Link=  WebURL+"User/ActivateUser?UserID="+EncryptUserID;

                // string ActivationLink = "<a href=" + Link + ">" + Link + "</a>";
                //<p> Please Click on below link to Activevate Account " + ActivationLink + "</p></br>

                //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                //message.To.Add(objUserLogin.Email);
                //message.Subject = "User Registration  By Papeleslegales.com";
                //message.Body = "<p>Your Are Registered Successfully, <h3> " + objUserLogin.UserName + "</h3></p>  </br><p>If you have any questions or trouble logging on please contact a site administrator.</p><br/><br/><br/>Thank you!";
                //message.IsBodyHtml = true;

                //if (RepidShare.Utility.CommonUtils.Send(message))
                //    Session["SuccessMessage"] = "E-Mail Sent Successfully!";

                Session["Message"] = "User registration successfully.";
                ModelState.Clear();

                Register objRegisterNew = new Register();
                objRegisterNew.Message = "User registration successfully.";

                //  return RedirectToAction("Index", "Home");

                return RedirectToAction("SummaryView", "User");  // comment by rahul need to uncomment when page summary page done.
            }
            catch (Exception ex)
            {
                //  Session["ExceptionMsg"] = objCommonUtils.ErrorLog("0", "Account", "Login Post", ex);
            }
            return RedirectToAction("Login", "User");
        }
        #endregion

        public ActionResult LogOff()
        {
            Request.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1d);
            Request.Cookies["ASP.NET_SessionId"].Value = "";
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            //Disable back button In all browsers.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Session.Abandon();
            Session.RemoveAll();

            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            return RedirectToAction("Index", "Home");

        }

        public ActionResult ActivateUser(string UserID)
        {
            string UsrID = RepidShare.Utility.CommonUtils.Decrypt(UserID, true);

            return View();
        }

        private UtilityWeb objUtilityWeb = new UtilityWeb();

        [Authorized]
        public ActionResult SummaryView(string prm = "")
        {
            //SummaryAndMyDocument summaryAndMyDocument = new SummaryAndMyDocument();
            //serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/SummaryView?UserId=" + LoggedInUserID.ToString());
            //summaryAndMyDocument.userSummaryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserSummaryModel>().Result : null;


            //// UserMyDocument objUserMyDocument = new UserMyDocument();
            ////initial set of current page, pageSize , Total pages
            //summaryAndMyDocument.myDocument = new UserMyDocument();
            //summaryAndMyDocument.myDocument.CurrentPage = 1;
            //summaryAndMyDocument.myDocument.PageSize = 10;
            //summaryAndMyDocument.myDocument.TotalPages = 0;
            //summaryAndMyDocument.myDocument.UserId = LoggedInUserID;


            //serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            //summaryAndMyDocument.myDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            //summaryAndMyDocument.myDocument.SearchType = "freetrial";
            //serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            //summaryAndMyDocument.myTrailDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            return View("Welcome", null);
        }

        [Authorized]
        public ActionResult MyDocument(string prm = "")
        {
            UserMyDocument objUserMyDocument = new UserMyDocument();
            //initial set of current page, pageSize , Total pages
            objUserMyDocument.CurrentPage = 1;
            objUserMyDocument.PageSize = 10;
            objUserMyDocument.TotalPages = 0;
            objUserMyDocument.UserId = LoggedInUserID;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", objUserMyDocument);
            objUserMyDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            return View("MyDocument", objUserMyDocument);
        }

        [HttpPost]
        [Authorized]
        public ActionResult MyDocument(UserMyDocument objUserMyDocument)
        {
            objUserMyDocument.UserId = LoggedInUserID;
            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", objUserMyDocument);
            objUserMyDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            return PartialView("_MyDocumentList", objUserMyDocument);
        }


        public ActionResult PrintPreview(string prm = "")
        {
            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }
            DocumentModel objDocumentModel = new DocumentModel();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.DocumentResponse + "/GetUserDocumentView?DocumentId=" + DocumentId + "&UserId=" + LoggedInUserID.ToString());
            objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;

            return PartialView("_PrintPreview", objDocumentModel.DocumentHTML);
        }
        [HttpGet]
        public JsonResult GetFolerById(string prm = "")
        {
            int DocumentId = 0; int FolderId = 0;

            //string [] parameter = prm.Split(',');           
            //   DocumentId = Convert.ToInt32(parameter[1]);
            if (!String.IsNullOrEmpty(prm))
            {
                //Decrypt
                string inputParameter = CommonUtils.Decrypt(prm).ToString().Replace("?", "").ToLower();

                //get query string value
                if (inputParameter.IndexOf("folderid") > -1 || inputParameter.IndexOf("documentid") > -1)
                {
                    foreach (var item in inputParameter.Split('&'))
                    {
                        if (item.IndexOf("folderid") > -1)
                        {
                            int.TryParse(item.Replace("folderid=", ""), out FolderId);
                        }
                        if (item.IndexOf("documentid") > -1)
                        {
                            int.TryParse(item.Replace("documentid=", ""), out DocumentId);
                        }
                    }
                }
            }

            List<FolderModel> objFolderModel = new List<FolderModel>();
           // serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetFolderById?UserId=" + LoggedInUserID.ToString());
            // serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetFolderById?UserId=" + Convert.ToInt32(parameter[0]));
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetFolderById?UserId=" + prm);
            objFolderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<FolderModel>>().Result : null;
            return Json(objFolderModel,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FolderMove(string prm = "")
        {
            int DocumentId = 0; int FolderId = 0;

            //string [] parameter = prm.Split(',');           
            //   DocumentId = Convert.ToInt32(parameter[1]);
            if (!String.IsNullOrEmpty(prm))
            {
                //Decrypt
                string inputParameter = CommonUtils.Decrypt(prm).ToString().Replace("?", "").ToLower();

                //get query string value
                if (inputParameter.IndexOf("folderid") > -1 || inputParameter.IndexOf("documentid") > -1)
                {
                    foreach (var item in inputParameter.Split('&'))
                    {
                        if (item.IndexOf("folderid") > -1)
                        {
                            int.TryParse(item.Replace("folderid=", ""), out FolderId);
                        }
                        if (item.IndexOf("documentid") > -1)
                        {
                            int.TryParse(item.Replace("documentid=", ""), out DocumentId);
                        }
                    }
                }
            }

            List<FolderModel> objFolderModel = new List<FolderModel>();
            // serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetFolderById?UserId=" + LoggedInUserID.ToString());
            // serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetFolderById?UserId=" + Convert.ToInt32(parameter[0]));
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetFolderById?UserId=" + prm);
            objFolderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<List<FolderModel>>().Result : null;
            ViewBag.FolderDropDown = new SelectList(objFolderModel, "FolderId", "FolderName", FolderId);

            FolderModel objFolder = new FolderModel();
            objFolder.FolderId = FolderId;
            objFolder.DocumentId = DocumentId;
            return PartialView("_FolderMove", objFolder);
        }

        [HttpPost]
        public ActionResult FolderMove(FolderModel objFolderModel)
        {
            if (objFolderModel.FolderId == 0)
            {
                objFolderModel.FolderName = "Default";
            }
            objFolderModel.UserId = LoggedInUserID;
            objFolderModel.IsActive = true;
            objFolderModel.CreatedBy = LoggedInUserID;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/InsertUpdateFolder", objFolderModel);
            objFolderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<FolderModel>().Result : null;

            return RedirectToAction("MyDocument", "User");
        }

        [HttpGet]
        public ActionResult QuickAccess()
        {
            HomeLayOutModel objHomeLayOutModel = (HomeLayOutModel)Session["LayOutData"];

            HomeDocumentViewModel homeDocumentViewModel = new HomeDocumentViewModel();
            homeDocumentViewModel.objListSubCategory = new List<SubCategoryModel>();
            homeDocumentViewModel.objListDocumentService = new List<DocumentModel>();

            List<PackedDocumentsParent> lstPackedDocumentsParent = new List<PackedDocumentsParent>();

            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetCategoryList?CategoryId=" + objHomeLayOutModel.objViewCategoryModel.CategoryList[0].CategoryID.ToString());
            var objHomeCategoryViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeCategoryViewModel>().Result : null;

            foreach (var subcategory in objHomeCategoryViewModel.objListSubCategory)
            {
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetSubCategoryList?SubCategoryId=" + subcategory.SubCategoryID.ToString());
                objHomeCategoryViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeCategoryViewModel>().Result : null;

                homeDocumentViewModel.objListSubCategory.Add(subcategory);
                homeDocumentViewModel.objListDocumentService.AddRange(objHomeCategoryViewModel.objListDocumentModel);

                foreach (var packedDocument in objHomeCategoryViewModel.objListDocumentModel)
                {
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetDocumentList?DocumentId=" + packedDocument.DocumentID.ToString());
                    var objHomeDocumentViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeDocumentViewModel>().Result : null;

                    if (objHomeDocumentViewModel.objListDocumentService.Count > 1)
                    {
                        lstPackedDocumentsParent.Add(new PackedDocumentsParent { SubCategoryID = packedDocument.SubCategoryID, DocumentTitle = packedDocument.DocumentTitle, DocumentDescription = packedDocument.DocumentDescription });
                    }
                }
            }
            return PartialView("_QuickAccess", homeDocumentViewModel);
        }


        [HttpGet]
        public ActionResult FolderCreate()
        {
            FolderModel objFolderModel = new FolderModel();
            return PartialView("_FolderCreate", objFolderModel);
        }

        public ActionResult CreateFolder(string folderName)
        {
            UserMyDocument myFolders = new UserMyDocument();

            if (folderName != "GetList")
            {
                FolderModel objFolderModel = new FolderModel();
                objFolderModel.UserId = LoggedInUserID;
                objFolderModel.IsActive = true;
                objFolderModel.CreatedBy = LoggedInUserID;
                objFolderModel.FolderName = folderName;

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/InsertUpdateFolder", objFolderModel);
                objFolderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<FolderModel>().Result : null;
            }

            myFolders.CurrentPage = 1;
            myFolders.PageSize = 10;
            myFolders.TotalPages = 0;
            myFolders.UserId = LoggedInUserID;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", myFolders);
            myFolders = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            return PartialView("_MyFolders", myFolders);

        }

        public ActionResult DeleteFolder(string prm = "")
        {
            ViewFolderModel objViewFolderModel = new ViewFolderModel();

            int FolderId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out FolderId);
            }

            objViewFolderModel.UserId = LoggedInUserID;
            objViewFolderModel.DeletedFolderID = FolderId;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/DeleteFolder", objViewFolderModel);
            objViewFolderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewFolderModel>().Result : null;

            return Json(true, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("SummaryView", "User", new { prm = "deleteFolder" });
        }

        public ActionResult MyActivityDetails()
        {
            SummaryAndMyDocument summaryAndMyDocument = new SummaryAndMyDocument();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/SummaryView?UserId=" + LoggedInUserID.ToString());
            summaryAndMyDocument.userSummaryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserSummaryModel>().Result : null;
            return PartialView("_MyActivityDetails", summaryAndMyDocument);
        }
        public ActionResult MyDocumentDetails()
        {
            SummaryAndMyDocument summaryAndMyDocument = new SummaryAndMyDocument();
            summaryAndMyDocument.myDocument = new UserMyDocument();
            summaryAndMyDocument.myDocument.CurrentPage = 1;
            summaryAndMyDocument.myDocument.PageSize = 10;
            summaryAndMyDocument.myDocument.TotalPages = 0;
            summaryAndMyDocument.myDocument.UserId = LoggedInUserID;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            summaryAndMyDocument.myDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            summaryAndMyDocument.myDocument.SearchType = "freetrial";
            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            summaryAndMyDocument.myTrailDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            return PartialView("_MyDocumentDetails", summaryAndMyDocument);
        }

        public ActionResult MyFolderDocuments(string folderId)
        {
            if (folderId == "")
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            SummaryAndMyDocument summaryAndMyDocument = new SummaryAndMyDocument();
            summaryAndMyDocument.myDocument = new UserMyDocument();
            summaryAndMyDocument.myDocument.CurrentPage = 1;
            summaryAndMyDocument.myDocument.PageSize = 10;
            summaryAndMyDocument.myDocument.TotalPages = 0;
            summaryAndMyDocument.myDocument.UserId = LoggedInUserID;


            int Id = Convert.ToInt32(folderId);

            //  if (Id == 0)
            //  {
            //serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            //summaryAndMyDocument.myDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            //summaryAndMyDocument.myDocument.SearchType = "freetrial";
            //serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            //summaryAndMyDocument.myTrailDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            summaryAndMyDocument.myDocument.FolderId = Id;
            summaryAndMyDocument.myDocument.SearchType = "folder";
            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            summaryAndMyDocument.myDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            //  }
            //else
            //{

            //    summaryAndMyDocument.myDocument.FolderId = Id;
            //    summaryAndMyDocument.myDocument.SearchType = "folder";
            //    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            //    summaryAndMyDocument.myDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;
            //}

            return PartialView("_MyFolderDocuments", summaryAndMyDocument);
        }



        //[HttpPost]
        //public ActionResult FolderCreate(FolderModel objFolderModel)
        //{
        //    ViewBag.CreateFolder = true;
        //    objFolderModel.UserId = LoggedInUserID;
        //    objFolderModel.IsActive = true;
        //    objFolderModel.CreatedBy = LoggedInUserID;

        //    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/InsertUpdateFolder", objFolderModel);
        //    objFolderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<FolderModel>().Result : null;
        //    //return RedirectToAction("MyDocument", "User");
        //    return RedirectToAction("SummaryView", "User");
        //}

        public ActionResult GuidanceNotes(string prm = "")
        {
            return PartialView("_GuidanceNotes", prm);
        }

        public ActionResult HistoryView(string prm = "")
        {
            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }
            DocumentModel objDocumentModel = new DocumentModel();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.DocumentResponse + "/GetUserDocumentView?DocumentId=" + DocumentId + "&UserId=" + LoggedInUserID.ToString());
            objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;

            return PartialView("_HistoryView", objDocumentModel.objListActivityModel);
        }

        [Authorized]
        public ActionResult AddForTrial(string prm = "")
        {
            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }
            return RedirectToAction("SaveDocumentResponse", "DocumentResponse", new { prm = CommonUtils.Encrypt("userid=" + LoggedInUserID + "&Documentid=" + DocumentId.ToString()) });
        }

        // Changed by Narayan
        public ActionResult UpdateAccount(string prm = "")
        {
            UtilityWeb objUtilityWeb = new UtilityWeb();
            Register objRegister = new Register();
            if (!String.IsNullOrEmpty(prm))
            {
                int UserId;
                int Status;
                //decrypt parameter and set in CategoryId variable
                int.TryParse(CommonUtils.Decrypt(prm), out UserId);
                //int.TryParse(prm.Split('~')[1], out Status);
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetUserListById?UserId=" + UserId.ToString());
                objRegister = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<Register>().Result : null;
                // ObjViewUserLoginModel
            }
            return View("Register", objRegister);

        }




        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult UpdateAccount(Register objRegister)
        {
            return View();
        }


        //public ActionResult deleteFolder(string prm = "")
        //{
        //    ViewFolderModel objViewFolderModel = new ViewFolderModel();

        //    int FolderId = 0;
        //    if (!String.IsNullOrEmpty(prm))
        //    {
        //        //decrypt parameter and set in DocumentId variable
        //        int.TryParse(CommonUtils.Decrypt(prm), out FolderId);
        //    }

        //    objViewFolderModel.UserId = LoggedInUserID;
        //    objViewFolderModel.DeletedFolderID = FolderId;

        //    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/DeleteFolder", objViewFolderModel);
        //    objViewFolderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewFolderModel>().Result : null;

        //    return Json(true, JsonRequestBehavior.AllowGet);
        //    //return RedirectToAction("SummaryView", "User", new { prm = "deleteFolder" });
        //}

        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document
            doc.Open();
            htmlWorker.StartDocument();

            // 5: parse the html into the document
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }

        public void ExportView(string prm = "")
        {
            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }

            DocumentModel objDocumentModel = new DocumentModel();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.DocumentResponse + "/GetUserDocumentView?DocumentId=" + DocumentId + "&UserId=" + LoggedInUserID.ToString());
            objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(GetPDF(string.IsNullOrWhiteSpace(objDocumentModel.DocumentHTML) ? "Documnet is not available. Please try later.<br/>" : objDocumentModel.DocumentHTML));
            Response.End();
        }

        [HttpGet]
        public ActionResult RenameDocument(string prm = "")
        {
            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }
            DocumentModel objDocumentModel = new DocumentModel();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.DocumentResponse + "/GetUserDocumentView?DocumentId=" + DocumentId + "&UserId=" + LoggedInUserID.ToString());
            objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;

            return PartialView("_RenameDocument", objDocumentModel);
        }

        [HttpPost]
        public ActionResult RenameDocument(DocumentModel objDocumentModel)
        {
            objDocumentModel.IsActive = true;
            objDocumentModel.CreatedBy = LoggedInUserID;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/RenameDocuemnt", objDocumentModel);
            objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;
            return RedirectToAction("MyDocument", "User");
        }

        public ActionResult deleteDocument(string prm = "")
        {
            ViewFolderModel objViewFolderModel = new ViewFolderModel();

            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }

            objViewFolderModel.UserId = LoggedInUserID;
            objViewFolderModel.DeletedDocumentID = DocumentId;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/DeleteDocument", objViewFolderModel);
            objViewFolderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewFolderModel>().Result : null;

            return RedirectToAction("MyDocument", "User");
        }

        [HttpGet]
        public ActionResult ShareDocument(string prm = "")
        {
            DocumentModel objDocumentModel = new DocumentModel();
            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }
            objDocumentModel.DocumentID = DocumentId;
            return PartialView("_ShareDocument", objDocumentModel);
        }

        [HttpPost]
        public ActionResult ShareDocument(DocumentModel objDocumentModel)
        {
            objDocumentModel.IsActive = true;
            objDocumentModel.CreatedBy = LoggedInUserID;

            EmailTemplate objEmailTemplate = new EmailTemplate();
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(objDocumentModel.ShareEMail);
            message.Subject = "Document Share by " + Session["UserFirstLastName"].ToString();
            //"Forgot Password By Papeleslegales.com";
            message.Body = "Dear,<br/><br/>Document Shared by " + Session["UserFirstLastName"].ToString() + " below Url mention. <br/><br/> http://papeleslegales.com/Home/DocumentView?prm=" + CommonUtils.Encrypt(objDocumentModel.DocumentID.ToString());
            //message.Subject = "Forgot UserName By Papeleslegales.com";
            //message.Body = "<p>Your UserName has been reset, <h3> " + objUserLogin.UserName + "</h3></p>  </br><p> According to our records, you have requested that your UserName be reset. Your new UserName is:<h3>" + objUserLogin.UserName + "<h3/></p> </br></br><p>If you have any questions or trouble logging on please contact a site administrator.</p><br/><br/><br/>Thank you!";
            message.IsBodyHtml = true;

            RepidShare.Utility.CommonUtils.Send(message);
            return RedirectToAction("MyDocument", "User");
        }

        [Authorized]
        public ActionResult MyTempletes(string prm = "")
        {
            UserMyDocument objUserMyDocument = new UserMyDocument();
            //initial set of current page, pageSize , Total pages
            objUserMyDocument.CurrentPage = 1;
            objUserMyDocument.PageSize = 10;
            objUserMyDocument.TotalPages = 0;
            objUserMyDocument.UserId = LoggedInUserID;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyTempletes", objUserMyDocument);
            objUserMyDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            return View("MyTempletes", objUserMyDocument);
        }

        [HttpPost]
        [Authorized]
        public ActionResult MyTempletes(UserMyDocument objUserMyDocument)
        {
            objUserMyDocument.UserId = LoggedInUserID;
            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyTempletes", objUserMyDocument);
            objUserMyDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            return PartialView("_MyTempleteList", objUserMyDocument);
        }

        [Authorized]
        [HttpGet]
        public ActionResult Buy(string prm = "")
        {
            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }
            DocumentModel objDocumentModel = new DocumentModel();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.DocumentResponse + "/GetUserDocumentView?DocumentId=" + DocumentId + "&UserId=" + LoggedInUserID.ToString());
            objDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<DocumentModel>().Result : null;

            PayPalItemDetail objPayPalItemDetail;
            if (Session["PayPalItemDetail"] == null)
            {
                objPayPalItemDetail = new PayPalItemDetail();
                objPayPalItemDetail = new PayPalItemDetail();
                objPayPalItemDetail.itemList = new ItemList();
                objPayPalItemDetail.details = new Details();
                objPayPalItemDetail.amount = new Amount();
                objPayPalItemDetail.itemList.items = new List<Item>();
            }
            else
            {
                objPayPalItemDetail = (PayPalItemDetail)Session["PayPalItemDetail"];
            }

            double VatTax = Math.Round((objDocumentModel.Price * Convert.ToDouble(MasterSetting.VatTax)) / 100, 2);

            objPayPalItemDetail.itemList.items.Add(new Item()
            {
                url = objDocumentModel.DocumentID.ToString(),
                name = objDocumentModel.DocumentTitle,
                currency = "USD",
                price = Convert.ToString(objDocumentModel.Price - VatTax),
                quantity = "1",
                sku = "sku",
                tax = VatTax.ToString()
            });

            double sumAmt = 0;
            foreach (Item item in objPayPalItemDetail.itemList.items)
            {
                sumAmt += Convert.ToDouble(item.price);
            }

            double sumVat = 0;
            foreach (Item item in objPayPalItemDetail.itemList.items)
            {
                sumVat += Convert.ToDouble(item.tax);
            }

            objPayPalItemDetail.details = new Details()
            {
                tax = sumVat.ToString(),
                shipping = "0",
                subtotal = sumAmt.ToString()
            };

            //vat tax 
            sumAmt = sumAmt + sumVat;

            //shippiing
            sumAmt = sumAmt + 0;
            objPayPalItemDetail.amount = new Amount()
            {
                currency = "USD",
                total = sumAmt.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = objPayPalItemDetail.details
            };

            Session["PayPalItemDetail"] = objPayPalItemDetail;

            return RedirectToAction("basket", "User");
        }

        [Authorized]
        [HttpGet]
        public ActionResult BuyRemove(string prm = "")
        {
            int DocumentId = 0;
            if (!String.IsNullOrEmpty(prm))
            {
                //decrypt parameter and set in DocumentId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
            }

            PayPalItemDetail objPayPalItemDetail;
            if (Session["PayPalItemDetail"] == null)
            {
                objPayPalItemDetail = new PayPalItemDetail();
                objPayPalItemDetail = new PayPalItemDetail();
                objPayPalItemDetail.itemList = new ItemList();
                objPayPalItemDetail.details = new Details();
                objPayPalItemDetail.amount = new Amount();
            }
            else
            {
                objPayPalItemDetail = (PayPalItemDetail)Session["PayPalItemDetail"];
            }

            objPayPalItemDetail.itemList.items = objPayPalItemDetail.itemList.items.Where(x => x.url != DocumentId.ToString()).ToList();

            double sumAmt = 0;
            foreach (Item item in objPayPalItemDetail.itemList.items)
            {
                sumAmt += Convert.ToDouble(item.price);
            }

            double sumVat = 0;
            foreach (Item item in objPayPalItemDetail.itemList.items)
            {
                sumVat += Convert.ToDouble(item.tax);
            }

            objPayPalItemDetail.details = new Details()
            {
                tax = sumVat.ToString(),
                shipping = "0",
                subtotal = sumAmt.ToString()
            };

            //vat tax 
            sumAmt = sumAmt + sumVat;

            //shippiing
            sumAmt = sumAmt + 0;
            objPayPalItemDetail.amount = new Amount()
            {
                currency = "USD",
                total = sumAmt.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = objPayPalItemDetail.details
            };

            Session["PayPalItemDetail"] = objPayPalItemDetail;

            return RedirectToAction("Basket", "User");
        }

        [Authorized]
        public ActionResult Basket()
        {
            PayPalItemDetail objPayPalItemDetail;
            if (Session["PayPalItemDetail"] == null)
            {
                objPayPalItemDetail = new PayPalItemDetail();
                objPayPalItemDetail.itemList = new ItemList();
                objPayPalItemDetail.details = new Details();
                objPayPalItemDetail.amount = new Amount();
            }
            else
            {
                objPayPalItemDetail = (PayPalItemDetail)Session["PayPalItemDetail"];
            }

            //if (Session["CoupenCode"] != null)
            //{
            //    CoupenModel objViewCoupenModelResult = (CoupenModel)Session["CoupenCode"];
            //    objPayPalItemDetail.amount.total = (Convert.ToDouble(objPayPalItemDetail.amount.total) - (Convert.ToDouble(objPayPalItemDetail.amount.total) * Convert.ToDouble(objViewCoupenModelResult.OffValue)) / 100).ToString();
            //}

            return View(objPayPalItemDetail);
        }

        [Authorized]
        public ActionResult Checkout()
        {
            PayPalItemDetail objPayPalItemDetail;
            if (Session["PayPalItemDetail"] == null)
            {
                objPayPalItemDetail = new PayPalItemDetail();
                objPayPalItemDetail.itemList = new ItemList();
                objPayPalItemDetail.details = new Details();
                objPayPalItemDetail.amount = new Amount();
            }
            else
            {
                objPayPalItemDetail = (PayPalItemDetail)Session["PayPalItemDetail"];
            }
            return View(objPayPalItemDetail);
        }

        [HttpGet]
        public ActionResult SharedDocument()
        {
            SharedDocument objSharedDocument = new SharedDocument();
            return PartialView("_SharedDocument", objSharedDocument);
        }

        [HttpPost]
        public ActionResult SharedDocument(SharedDocument objSharedDocument)
        {
            objSharedDocument.UserId = LoggedInUserID;
            objSharedDocument.IsActive = true;
            objSharedDocument.CreatedBy = LoggedInUserID;

            UtilityWeb objUtilityWeb = new UtilityWeb();
            try
            {
                UserLogin objUserLogin = new RepidShare.Entities.UserLogin();
                EmailTemplate objEmailTemplate = new EmailTemplate();
                objUserLogin.Email = "vipinrathore0@gmail.com";
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/ForgotPassword", objUserLogin);
                objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add("vipinrathore0@gmail.com");

                message.Subject = "Please create document for me, Title : - " + objSharedDocument.SharedDocumentTitle;
                //"Forgot Password By Papeleslegales.com";
                message.Body = objEmailTemplate.Content + "<br/>Your UserName According to our records is: , <h3>" + objUserLogin.UserName + "</h3><br/><br/><br/><p>Shared Document Descrition : - </p><p>" + objSharedDocument.SharedDocumentDescrition + "</p>";
                //message.Subject = "Forgot UserName By Papeleslegales.com";
                //message.Body = "<p>Your UserName has been reset, <h3> " + objUserLogin.UserName + "</h3></p>  </br><p> According to our records, you have requested that your UserName be reset. Your new UserName is:<h3>" + objUserLogin.UserName + "<h3/></p> </br></br><p>If you have any questions or trouble logging on please contact a site administrator.</p><br/><br/><br/>Thank you!";
                message.IsBodyHtml = true;

                if (objUserLogin.ErrorCode == 0)
                {
                    if (RepidShare.Utility.CommonUtils.Send(message))
                    {
                        //    Session["SuccessMessage"] = "E-Mail Sent Successfully!";
                        // ViewBag.UserNameSucessMessgage = "your user name has been sent to your accont. for login click <a href='http://localhost:49208/User/Login'>Click on link <a>";
                        //objChangePasswordModel.Message = "<p style='color:green;'>Your UserName has been sent to your account. for login click <a href='http://localhost:49208/User/Login'> Login <a></p>";
                    }
                    // Session["SuccessMessage"] = "E-Mail Sent Successfully!";

                }

            }
            catch (Exception ex)
            {
                //Session["ExceptionMsg"] = objCommonUtils.ErrorLog(createdBy.ToString(), "User", "ForgotPassword Post", ex);
            }

            //serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/InsertUpdateFolder", objSharedDocument);
            //objSharedDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<SharedDocument>().Result : null;
            return RedirectToAction("SummaryView", "User");

        }

        [Authorized]
        [HttpGet]
        public ActionResult ApplyOffer(string prm = "")
        {

            PayPalItemDetail objPayPalItemDetail;
            if (Session["PayPalItemDetail"] == null)
            {
                objPayPalItemDetail = new PayPalItemDetail();
                objPayPalItemDetail = new PayPalItemDetail();
                objPayPalItemDetail.itemList = new ItemList();
                objPayPalItemDetail.details = new Details();
                objPayPalItemDetail.amount = new Amount();
            }
            else
            {
                objPayPalItemDetail = (PayPalItemDetail)Session["PayPalItemDetail"];
            }

            ViewCoupenModel objViewCoupenModel = new ViewCoupenModel();
            //initial set of current page, pageSize , Total pages
            objViewCoupenModel.CurrentPage = 1;
            objViewCoupenModel.PageSize = int.MaxValue - 1;
            objViewCoupenModel.TotalPages = 0;

            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Coupen + "/GetCoupenList", objViewCoupenModel);
            objViewCoupenModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewCoupenModel>().Result : null;

            double discount = 0;
            if (objViewCoupenModel != null && objViewCoupenModel.CoupenList != null && objViewCoupenModel.CoupenList.Count > 0)
            {
                var objViewCoupenModelResult = objViewCoupenModel.CoupenList.Where(x => x.CoupenCode == prm).FirstOrDefault();
                if (objViewCoupenModelResult != null)
                {
                    discount = Math.Round(Convert.ToDouble(objViewCoupenModelResult.OffValue) * Convert.ToDouble(objPayPalItemDetail.amount.total) / 100);
                    //Session["CoupenCode"] = objViewCoupenModelResult;
                }
            }

            //Session["PayPalItemDetail"] = objPayPalItemDetail;

            return Json(new { Discount = discount }, JsonRequestBehavior.AllowGet);

        }

        [Authorized]
        public ActionResult BulletinView(string prm = "")
        {
            ViewBulletinModel ObjViewBulletinModel = new ViewBulletinModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewBulletinModel.CurrentPage = 1;
                ObjViewBulletinModel.PageSize = int.MaxValue - 1;
                ObjViewBulletinModel.TotalPages = 0;


                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Bulletin + "/GetBulletinList", ObjViewBulletinModel);

                ObjViewBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewBulletinModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Bulletin", "View GET");
            }
            return View("Bulletin", ObjViewBulletinModel);

        }

        public ActionResult BulletinPreview(string id)
        {
            BulletinModel objBulletinModel = new BulletinModel();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Bulletin + "/GetBulletinById?BulletinId=" + id.ToString());
            objBulletinModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<BulletinModel>().Result : null;

            return PartialView("_BulletinPreview", objBulletinModel.Description);
        }

        public ActionResult RequestNewDocument()
        {
            return PartialView("_RequestNewDocument", null);
        }
        public ActionResult MyActivity()
        {
            SummaryAndMyDocument summaryAndMyDocument = new SummaryAndMyDocument();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/SummaryView?UserId=" + LoggedInUserID.ToString());
            summaryAndMyDocument.userSummaryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserSummaryModel>().Result : null;


            // UserMyDocument objUserMyDocument = new UserMyDocument();
            //initial set of current page, pageSize , Total pages
            summaryAndMyDocument.myDocument = new UserMyDocument();
            summaryAndMyDocument.myDocument.CurrentPage = 1;
            summaryAndMyDocument.myDocument.PageSize = 10;
            summaryAndMyDocument.myDocument.TotalPages = 0;
            summaryAndMyDocument.myDocument.UserId = LoggedInUserID;


            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            summaryAndMyDocument.myDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;

            summaryAndMyDocument.myDocument.SearchType = "freetrial";
            serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.UserLogin + "/MyDocument", summaryAndMyDocument.myDocument);
            summaryAndMyDocument.myTrailDocument = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserMyDocument>().Result : null;
            return PartialView("_MyActivity", summaryAndMyDocument);
        }

    }
}

