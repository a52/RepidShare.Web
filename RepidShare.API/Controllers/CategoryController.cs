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
    public class CategoryController : ApiController
    {
        private BLCategory objBLCategory = new BLCategory();

        #region Get ,Insert, Update and delete Category

        [HttpGet]
        public CategoryModel GetCategoryById(int CategoryId)
        {
            
            return objBLCategory.GetCategoryById(CategoryId);
        }

        [HttpPost]
        public CategoryModel InsertUpdateCategory(CategoryModel objCategoryModel)
        {
            return objBLCategory.InsertUpdateCategory(objCategoryModel);
        }

        [HttpPost]
        public ViewCategoryModel DeleteCategory(ViewCategoryModel objViewCategoryModel)
        {
            return objBLCategory.DeleteCategory(objViewCategoryModel);
        }

        #endregion

        #region View  Category
        [HttpPost]
        public ViewCategoryModel GetCategoryList(ViewCategoryModel objViewCategoryModel)
        {
            return objBLCategory.GetCategoryList(objViewCategoryModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillCaegoryDropDown()
        {
            return objBLCategory.FillCaegoryDropDown();
        }


        [HttpGet]
        public void UpdateCategoryStatusByID(int CategoryId, int status)
        {

            objBLCategory.UpdateCategoryStatusByID(CategoryId, status);
        }
        #endregion
    }
}
