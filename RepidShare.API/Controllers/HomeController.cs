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
    public class HomeController : ApiController
    {
        private BLHome objBLHome = new BLHome();

        [HttpGet]
        public HomeLayOutModel GetLayOutData()
        {
            HomeLayOutModel objHomeLayOutModel = new HomeLayOutModel();

            BLCategory objBLCategory = new BLCategory();
            BLDocument objBLDocument = new BLDocument();
            BLSubCategory objBLSubCategory = new BLSubCategory();

            ViewCategoryModel objViewCategoryModel = new ViewCategoryModel();
            objViewCategoryModel.CurrentPage = 1;
            objViewCategoryModel.PageSize = int.MaxValue - 1;
            objHomeLayOutModel.objViewCategoryModel = new ViewCategoryModel();
            objHomeLayOutModel.objViewCategoryModel = objBLCategory.GetCategoryList(objViewCategoryModel);

            if (objHomeLayOutModel != null && objHomeLayOutModel.objViewCategoryModel != null && objHomeLayOutModel.objViewCategoryModel.CategoryList != null && objHomeLayOutModel.objViewCategoryModel.CategoryList.Count > 0)
            {
                for (int i = 0; i < objHomeLayOutModel.objViewCategoryModel.CategoryList.Count; i++)
                {

                    //string[] DocumentIds = objHomeLayOutModel.objViewCategoryModel.CategoryList[i].QuickLinks.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    //if (DocumentIds != null && DocumentIds.Length > 0)
                    //{
                    //    objHomeLayOutModel.objViewCategoryModel.CategoryList[i].objDocumentList = new List<DocumentModel>();
                    //    for (int j = 0; j < DocumentIds.Length; j++)
                    //    {
                    //        objHomeLayOutModel.objViewCategoryModel.CategoryList[i].objDocumentList.Add(objBLDocument.GetDocumentById(Convert.ToInt32(DocumentIds[j])));
                    //    }
                    //}
                }
            }
            ViewSubCategoryModel objViewSubCategoryModel = new ViewSubCategoryModel();
            objViewSubCategoryModel.CurrentPage = 1;
            objViewSubCategoryModel.PageSize = int.MaxValue - 1;

            objHomeLayOutModel.objSubViewCategoryModel = new ViewSubCategoryModel();
            objHomeLayOutModel.objSubViewCategoryModel = objBLSubCategory.GetSubCategoryList(objViewSubCategoryModel);

            return objHomeLayOutModel;
        }

        [HttpGet]
        public HomeCategoryViewModel GetCategoryList(int CategoryId)
        {
            return objBLHome.GetCategoryList(CategoryId);
        }

        [HttpGet]
        public HomeCategoryViewModel GetSubCategoryList(int SubCategoryId)
        {
            return objBLHome.GetSubCategoryList(SubCategoryId);
        }

        [HttpGet]
        public HomeLawGuideViewModel GetLawGuideList(int CategoryId)
        {
            return objBLHome.GetLawGuideList(CategoryId);
        }

        [HttpGet]
        public HomeDocumentViewModel GetDocumentList(int DocumentId)
        {
            return objBLHome.GetDocumentList(DocumentId);
        }
    }
}
