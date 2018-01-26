
using RepidShare.Entities;
using RepidShare.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace RepidShare.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit category
        public void FillQuickLinks(string QuickLinks, int CategoryID)
        {
            ViewDocumentModel ObjViewDocumentModel = new ViewDocumentModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewDocumentModel.CurrentPage = 1;
                ObjViewDocumentModel.PageSize = int.MaxValue - 1;
                ObjViewDocumentModel.TotalPages = 0;
                //Get  Document List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Document + "/GetDocumentList", ObjViewDocumentModel);
                ObjViewDocumentModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewDocumentModel>().Result : null;
                //ObjViewDocumentModel = objBLDocument.GetDocumentList(ObjViewDocumentModel);
                if (ObjViewDocumentModel != null && ObjViewDocumentModel.DocumentList != null)
                {
                    //ViewBag.PackList = ObjViewDocumentModel.DocumentList.Where(x => x.PackIds == string.Empty && x.DocumentID != DocumentId).ToList();
                    ViewBag.QuickLinks = new MultiSelectList(ObjViewDocumentModel.DocumentList.Where(x=>x.CategoryID == CategoryID).ToList(), "DocumentID", "DocumentTitle", Convert.ToString(string.IsNullOrWhiteSpace(QuickLinks) ? "" : QuickLinks).Split(",".ToCharArray()));
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Document", "View GET");
            }

        }



        /// <summary>
        /// Add or Edit Category for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveCategory(string prm = "")
        {
            CategoryModel objCategoryModel = new CategoryModel();

            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int CategoryId;
                    //decrypt parameter and set in CategoryId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out CategoryId);
                    //Get Category detail by  Category Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Category + "/GetCategoryById?CategoryId=" + CategoryId.ToString());
                    objCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<CategoryModel>().Result : null;
                }

                FillQuickLinks(objCategoryModel.QuickLinks, objCategoryModel.CategoryID);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Category", "SaveCategory Get");
            }

            return View("SaveCategory", objCategoryModel);
        }



        /// <summary>
        /// Add or Edit  Category based on CategoryID
        /// </summary>
        /// <param name="objCategoryModel"></param>
        /// <returns></returns>


        [ValidateAntiForgeryToken()]
        [Filters.Authorized]
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveCategory(CategoryModel objCategoryModel, HttpPostedFileBase fileUpload)
        {
            try
            {
                if (fileUpload != null)
                {

                    string fileName = string.Empty;
                    string destinationPath = string.Empty;
                    fileName = Path.GetFileName(fileUpload.FileName);
                    string AttachmentType = Path.GetExtension(fileUpload.FileName);
                    Stream fs = fileUpload.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string ImageContent = Convert.ToBase64String(bytes);
                    objCategoryModel.AttachmentContent = bytes;
                    objCategoryModel.AttachmentName = fileName;
                    objCategoryModel.AttachmentSize = fs.Length;
                    objCategoryModel.AttachmentType = AttachmentType;
                }


                objCategoryModel.IsActive = true;
                objCategoryModel.CreatedBy = LoggedInUserID;
                ;
                //Insert or Update  Category
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Category + "/InsertUpdateCategory", objCategoryModel);
                objCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<CategoryModel>().Result : null;

                //if error code is 0 means  category saved successfully
                if (Convert.ToInt32(objCategoryModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "Category Saved Successfully";
                    return RedirectToAction("ViewCategory", "Category");
                }
                else if (Convert.ToInt32(objCategoryModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means Category Name is duplicate set duplicate Category error message.
                    objCategoryModel.Message = "Category Duplicate not allowed";
                    objCategoryModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objCategoryModel.Message = "Error while adding record";
                    objCategoryModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }

                FillQuickLinks(objCategoryModel.QuickLinks, objCategoryModel.CategoryID);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Category", "SaveCategory POST");
            }
            return View("SaveCategory", objCategoryModel);
        }

        #endregion

        #region View  Category
        /// <summary>
        /// View  Category List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewCategory()
        {
            ViewCategoryModel ObjViewCategoryModel = new ViewCategoryModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewCategoryModel.CurrentPage = 1;
                ObjViewCategoryModel.PageSize = CommonUtils.PageSize;
                ObjViewCategoryModel.TotalPages = 0;

                //Get  Category List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Category + "/GetCategoryList", ObjViewCategoryModel);

                ObjViewCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewCategoryModel>().Result : null;
                //ObjViewCategoryModel = objBLCategory.GetCategoryList(ObjViewCategoryModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewCategoryModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewCategoryModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Category", "View GET");
            }
            return View("ViewCategory", ObjViewCategoryModel);
        }

        /// <summary>
        /// View  Category List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewCategoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewCategory(ViewCategoryModel objViewCategoryModel)
        {
            try
            {

                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewCategoryModel.Message = objViewCategoryModel.MessageType = String.Empty;

                if (objViewCategoryModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Category + "/DeleteCategory", objViewCategoryModel);
                    objViewCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewCategoryModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewCategoryModel.Message = "Category Deleted Successfully";
                        objViewCategoryModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewCategoryModel.Message = "Error while deleting record";
                        objViewCategoryModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Category List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Category + "/GetCategoryList", objViewCategoryModel);
                objViewCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewCategoryModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Category", "View POST");
            }
            return PartialView("_CategoryList", objViewCategoryModel);
        }


        [Filters.Authorized]
        public ActionResult ActivateDeactivateCategory(string prm = "")
        {
            CategoryModel objCategoryModel = new CategoryModel();

            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int CategoryId;
                    int Status;
                    //decrypt parameter and set in CategoryId variable
                    int.TryParse(CommonUtils.Decrypt(prm.Split('~')[0]), out CategoryId);
                    int.TryParse(prm.Split('~')[1], out Status);
                    //Get Category detail by  Category Id


                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Category + "/UpdateCategoryStatusByID?CategoryId=" + CategoryId.ToString() + "&status=" + Status);
                    objCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<CategoryModel>().Result : null;

                    //serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/GetUserListById?UserId=" + UserId.ToString());
                    //objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;
                    //if (objUserLogin != null)
                    //{

                    //    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.UserLogin + "/UpdateUserStatusByID?UserId=" + CategoryId.ToString() + "&status=" + Status);
                    //    objUserLogin = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<UserLogin>().Result : null;


                    //    //Admin_UpdateUserStatusByID

                    //}


                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Category", "ViewCategory");
            }

            return RedirectToAction("ViewCategory");
        }

        #endregion
    }
}
