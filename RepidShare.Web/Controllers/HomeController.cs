using RepidShare.Entities;
using RepidShare.Utility;
using RepidShare.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RepidShare.Web.Controllers
{
    [LayOutData]
    public class HomeController : Controller
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();


        public ActionResult Index()
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
            Session["PackedDocument"] = lstPackedDocumentsParent;
            //serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetSubCategoryList?SubCategoryId=" + SubCategoryId.ToString());
            //objHomeCategoryViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeCategoryViewModel>().Result : null;
            //objHomeCategoryViewModel.SelectedSubCatId = SubCategoryId;

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(homeDocumentViewModel);
        }

        public ActionResult Registration()
        {
            ViewBag.Message = "Your app description page.";

            return View("About");
        }

        public ActionResult Contact()
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

        public ActionResult CategoryView(string prm = "")
        {
            HomeCategoryViewModel objHomeCategoryViewModel = new HomeCategoryViewModel();
            //if prm(Paramter) is empty means Add condition else edit condition
            if (!String.IsNullOrEmpty(prm))
            {
                int CategoryId;
                //decrypt parameter and set in CategoryId variable
                int.TryParse(CommonUtils.Decrypt(prm), out CategoryId);
                //Get Category detail by  Category Id
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetCategoryList?CategoryId=" + CategoryId.ToString());
                objHomeCategoryViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeCategoryViewModel>().Result : null;
            }

            return View(objHomeCategoryViewModel);
        }

        public ActionResult SubCategoryView(string prm = "")
        {
            HomeCategoryViewModel objHomeCategoryViewModel = new HomeCategoryViewModel();
            //if prm(Paramter) is empty means Add condition else edit condition
            if (!String.IsNullOrEmpty(prm))
            {
                int SubCategoryId;
                //decrypt parameter and set in CategoryId variable
                int.TryParse(CommonUtils.Decrypt(prm), out SubCategoryId);
                //Get Category detail by  Category Id
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetSubCategoryList?SubCategoryId=" + SubCategoryId.ToString());
                objHomeCategoryViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeCategoryViewModel>().Result : null;
                objHomeCategoryViewModel.SelectedSubCatId = SubCategoryId;
            }

            return View(objHomeCategoryViewModel);
        }

        public ActionResult LawGuideView(string prm = "")
        {
            HomeLawGuideViewModel objHomeLawGuideViewModel = new HomeLawGuideViewModel();
            //if prm(Paramter) is empty means Add condition else edit condition
            if (!String.IsNullOrEmpty(prm))
            {
                int LawGuideId;
                //decrypt parameter and set in CategoryId variable
                int.TryParse(CommonUtils.Decrypt(prm), out LawGuideId);
                //Get Category detail by  Category Id
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetLawGuideList?CategoryId=" + LawGuideId.ToString());
                objHomeLawGuideViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeLawGuideViewModel>().Result : null;
                objHomeLawGuideViewModel.SelectedLawGuideId = LawGuideId;
            }

            return View(objHomeLawGuideViewModel);
        }

        public ActionResult DocumentView(string prm = "")
        {
            HomeDocumentViewModel objHomeDocumentViewModel = new HomeDocumentViewModel();
            //if prm(Paramter) is empty means Add condition else edit condition
            if (!String.IsNullOrEmpty(prm))
            {
                int DocumentId;
                //decrypt parameter and set in CategoryId variable
                int.TryParse(CommonUtils.Decrypt(prm), out DocumentId);
                //Get Category detail by  Category Id
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetDocumentList?DocumentId=" + DocumentId.ToString());
                objHomeDocumentViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeDocumentViewModel>().Result : null;
                objHomeDocumentViewModel.SelectedDocumentId = DocumentId;
            }

            return View(objHomeDocumentViewModel);
        }

        public PartialViewResult GetDocument(int documentId)
        {
            HomeDocumentViewModel objHomeDocumentViewModel = new HomeDocumentViewModel();
            serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Home + "/GetDocumentList?DocumentId=" + documentId.ToString());
            objHomeDocumentViewModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HomeDocumentViewModel>().Result : null;
            objHomeDocumentViewModel.SelectedDocumentId = documentId;
            return PartialView("_DocumentViewPartial", objHomeDocumentViewModel);
        }

        public PartialViewResult FreeNewsLatter()
        {
            return PartialView("_FreeNewsLetterPartial");
        }
        public PartialViewResult PackedDocuments()
        {
            List<PackedDocumentsParent> objHomePackedDocumentModel = (List<PackedDocumentsParent>)Session["PackedDocument"];
            return PartialView("_PackedDocumentsPartial", objHomePackedDocumentModel);
        }
        public PartialViewResult LegalTools()
        {
            return PartialView("_LegalToolsPartial");
        }
        public PartialViewResult JudicailNews()
        {
            return PartialView("_JudicailNewsPartial");
        }
        public PartialViewResult RightAdwords()
        {
            return PartialView("_RightAdwordsPartial");
        }

        //public PartialViewResult DocumentCategories()
        //{
        //    return PartialView("_DocumentCategoriesPartial");
        //}
        public ActionResult Test()
        {
            return View();
        }
    }
}
