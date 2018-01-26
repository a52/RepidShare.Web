using RepidShare.Business;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
 

namespace RepidShare.API.Controllers
{
    public class SubCategoryController : ApiController
    {
        private BLSubCategory objBLSubCategory = new BLSubCategory();

        #region Get ,Insert, Update and delete SubCategory

        [HttpGet]
        public SubCategoryModel GetSubCategoryById(int SubCategoryId)
        {
            return objBLSubCategory.GetSubCategoryById(SubCategoryId);
        }

        [HttpPost]
        [ActionName("InsertUpdateSubCategory")]
        public SubCategoryModel InsertUpdateSubCategory(SubCategoryModel objSubCategoryModel)
        {
            return objBLSubCategory.InsertUpdateSubCategory(objSubCategoryModel);
        }

        [HttpPost]
        [ActionName("DeleteSubCategory")]
        public ViewSubCategoryModel DeleteSubCategory(ViewSubCategoryModel objViewSubCategoryModel)
        {
            return objBLSubCategory.DeleteSubCategory(objViewSubCategoryModel);
        }

        #endregion

        #region View  SubCategory
        [HttpPost]
        [ActionName("GetSubCategoryList")]
        public ViewSubCategoryModel GetSubCategoryList(ViewSubCategoryModel objViewSubCategoryModel)
        {
            return objBLSubCategory.GetSubCategoryList(objViewSubCategoryModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillSubCategoryDropDown(int? CategoryID, int? GroupID)
        {
            return objBLSubCategory.FillSubCategoryDropDown(CategoryID,GroupID);
        }
        #endregion

         [HttpGet]
        public void UpdateSubCategoryStatusByID(int SubCategoryId, int status)
        {

            objBLSubCategory.UpdateSubCategoryStatusByID(SubCategoryId, status);
        }

        
    }
}
