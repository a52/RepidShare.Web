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
    public class SubCategoryController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit category
        /// <summary>
        /// Add or Edit Category for 
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult SaveSubCategory(string prm = "")
        {
            SubCategoryModel objSubCategoryModel = new SubCategoryModel();
            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int SubCategoryId;
                    //decrypt parameter and set in SubCategoryId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out SubCategoryId);
                    //Get SubCategory detail by  SubCategory Id
                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.SubCategory + "/GetSubCategoryById?SubCategoryId=" + SubCategoryId.ToString());
                    objSubCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<SubCategoryModel>().Result : null;

                }
                CaegoryDropDown(objSubCategoryModel.CategoryID);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SubCategory", "SaveSubCategory Get");
            }

            return View("SaveSubCategory", objSubCategoryModel);
        }



        /// <summary>
        /// Add or Edit  SubCategory based on SubCategoryID
        /// </summary>
        /// <param name="objSubCategoryModel"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        [Filters.Authorized]
        public ActionResult SaveSubCategory(SubCategoryModel objSubCategoryModel, HttpPostedFileBase fileUpload)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }

                if (fileUpload != null)
                {
                    string fileName = string.Empty;
                    string destinationPath = string.Empty;
                    fileName = Path.GetFileName(fileUpload.FileName);
                    string AttachmentType = Path.GetExtension(fileUpload.FileName);
                    Stream fs = fileUpload.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                   /// string ImageContent = Convert.ToBase64String(bytes);
                    objSubCategoryModel.AttachmentContent = bytes;
                    objSubCategoryModel.AttachmentName = fileName;
                    objSubCategoryModel.AttachmentSize = fs.Length;
                    objSubCategoryModel.AttachmentType = AttachmentType;
                }
                objSubCategoryModel.IsActive = true;
                objSubCategoryModel.CreatedBy = LoggedInUserID;

                //Insert or Update  SubCategory
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.SubCategory + "/InsertUpdateSubCategory", objSubCategoryModel);
                objSubCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<SubCategoryModel>().Result : null;

                //if error code is 0 means  SubCategory saved successfully
                if (Convert.ToInt32(objSubCategoryModel.ErrorCode) == 0)
                {
                    // Set success message
                    TempData["SucessMessage"] = "SubCategory Saved Successfully";
                    return RedirectToAction("ViewSubCategory", "SubCategory");
                }
                else if (Convert.ToInt32(objSubCategoryModel.ErrorCode) == 52)
                {
                    //If Errorcode is  52 means SubCategory Name is duplicate set duplicate SubCategory error message.
                    objSubCategoryModel.Message = "SubCategory Duplicate not allowed";
                    objSubCategoryModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                else
                {
                    //set Error Message if error code is greater than 0 but not 52 (duplicate)
                    objSubCategoryModel.Message = "Error while adding record";
                    objSubCategoryModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower();
                }
                CaegoryDropDown(objSubCategoryModel.CategoryID);
                
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SubCategory", "SaveSubCategory POST");
            }
            return View("SaveSubCategory", objSubCategoryModel);
        }

        #endregion

        #region View  SubCategory
        /// <summary>
        /// View  SubCategory List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewSubCategory()
        {
            ViewSubCategoryModel ObjViewSubCategoryModel = new ViewSubCategoryModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewSubCategoryModel.CurrentPage = 1;
                ObjViewSubCategoryModel.PageSize = CommonUtils.PageSize;
                ObjViewSubCategoryModel.TotalPages = 0;
                //Get  SubCategory List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.SubCategory + "/GetSubCategoryList", ObjViewSubCategoryModel);
                ObjViewSubCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewSubCategoryModel>().Result : null;
                //ObjViewSubCategoryModel = objBLSubCategory.GetSubCategoryList(ObjViewSubCategoryModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewSubCategoryModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewSubCategoryModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }

                CaegoryDropDown(ObjViewSubCategoryModel.FilterCategoryId);
               // EmailTyepDropDown(ObjViewSubCategoryModel.FilterCategoryId);
                GroupDropDown(ObjViewSubCategoryModel.FilterGroupID);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SubCategory", "View GET");
            }
            return View("ViewSubCategory", ObjViewSubCategoryModel);
        }

        /// <summary>
        /// View  SubCategory List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewSubCategoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewSubCategory(ViewSubCategoryModel objViewSubCategoryModel)
        {
            try
            {
                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewSubCategoryModel.Message = objViewSubCategoryModel.MessageType = String.Empty;

                if (objViewSubCategoryModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.SubCategory + "/DeleteSubCategory", objViewSubCategoryModel);
                    objViewSubCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewSubCategoryModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewSubCategoryModel.Message = "SubCategory Deleted Successfully";
                        objViewSubCategoryModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewSubCategoryModel.Message = "Error while deleting record";
                        objViewSubCategoryModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  SubCategory List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.SubCategory + "/GetSubCategoryList", objViewSubCategoryModel);
                objViewSubCategoryModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewSubCategoryModel>().Result : null;

                CaegoryDropDown(objViewSubCategoryModel.FilterCategoryId);
                GroupDropDown(objViewSubCategoryModel.FilterGroupID);
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "SubCategory", "View POST");
            }
            return PartialView("_SubCategoryList", objViewSubCategoryModel);
        }

        [Filters.Authorized]
        public ActionResult ActivateDeactivateSubCategory(string prm = "")
        {
            CategoryModel objCategoryModel = new CategoryModel();

            try
            {
                //if prm(Paramter) is empty means Add condition else edit condition
                if (!String.IsNullOrEmpty(prm))
                {
                    int SubCategoryId;
                    int Status;
                    //decrypt parameter and set in CategoryId variable
                    int.TryParse(CommonUtils.Decrypt(prm.Split('~')[0]), out SubCategoryId);
                    int.TryParse(prm.Split('~')[1], out Status);
                    //Get Category detail by  Category Id


                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.SubCategory + "/UpdateSubCategoryStatusByID?SubCategoryId=" + SubCategoryId.ToString() + "&status=" + Status);
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
                ErrorLog(ex, "ViewSubCategory", "ViewSubCategory");
            }

            return RedirectToAction("ViewSubCategory");
        }

        #endregion

    }
}
