using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIMSEntities;
using System.Data;
using RepidShare.Data;
using RepidShare.Business;
using RepidShare.Entities;


namespace RepidShare.Business
{
    public class BLCategory : BLBase
    {
        private DLCategory objDLCategory = new DLCategory();
        #region Get ,Insert, Update and delete Category
        /// <summary>
        /// Get Category By Id
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns>Category Model</returns>
        public CategoryModel GetCategoryById(int CategoryId)
        {
            //Call GetCategoryBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLCategory.GetCategoryById(CategoryId);
            CategoryModel objCategoryModel = new CategoryModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objCategoryModel = GetDataRowToEntity<CategoryModel>(dt.Rows[0]);
            }

            return objCategoryModel;
        }

        /// <summary>
        /// Insert or Update  Category
        /// </summary>
        /// <param name="objCategoryModel">object of  Category Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public CategoryModel InsertUpdateCategory(CategoryModel objCategoryModel)
        {
            //call InsertUpdateCategory Method of dataLayer and return CategoryModel
            return objDLCategory.InsertUpdateCategory(objCategoryModel);
        }

        /// <summary>
        /// Delete  Category by  Category ID
        /// </summary>
        /// <param name="objViewCategoryModel"></param>
        /// <param name="createdBy"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public ViewCategoryModel DeleteCategory(ViewCategoryModel objViewCategoryModel)
        {
           return objDLCategory.DeleteCategory(objViewCategoryModel);
        }

        #endregion

        #region View  Category
        /// <summary>
        /// Get  Category List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewCategoryModel">object of Model ViewCategoryModel</param>
        /// <returns></returns>
        public ViewCategoryModel GetCategoryList(ViewCategoryModel objViewCategoryModel)
        {
            List<CategoryModel> lstCategoryModel = new List<CategoryModel>();
            //if FilterCategoryName is NULL than set to empty
            objViewCategoryModel.FilterCategoryName = objViewCategoryModel.FilterCategoryName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewCategoryModel.SortBy = objViewCategoryModel.SortBy ?? String.Empty;

            //call GetCategoryList Method which will retrun datatable of  Category
            DataTable dtCategory = objDLCategory.GetCategoryList(objViewCategoryModel);

            //fetch each row of datable
            foreach (DataRow dr in dtCategory.Rows)
            {
                //Convert datarow into Model object and set Model object property
                CategoryModel itemCategoryModel = GetDataRowToEntity<CategoryModel>(dr);
                //Add  Category in List
                lstCategoryModel.Add(itemCategoryModel);
            }

            //set Category List of Model ViewCategoryModel
            objViewCategoryModel.CategoryList = lstCategoryModel;
            //if  Category List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewCategoryModel != null && objViewCategoryModel.CategoryList != null && objViewCategoryModel.CategoryList.Count > 0)
            {
                objViewCategoryModel.CurrentPage = objViewCategoryModel.CategoryList[0].CurrentPage;
                int totalRecord = objViewCategoryModel.CategoryList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewCategoryModel.PageSize) > 0)
                    objViewCategoryModel.TotalPages = (totalRecord / objViewCategoryModel.PageSize + 1);
                else
                    objViewCategoryModel.TotalPages = totalRecord / objViewCategoryModel.PageSize;


            }
            else
            {
                objViewCategoryModel.TotalPages = 0;
                objViewCategoryModel.CurrentPage = 1;
            }
            return objViewCategoryModel;
        }
        #endregion

        #region  Caegory DropDown
        /// <summary>
        /// Get  Caegory DropDown Item
        /// </summary>
        /// <returns></returns>
        ///<remarks>
        /// Created By Rakesh Patidar , 11 Feb 2015
        ///</remarks>
        public List<DropdownModel> FillCaegoryDropDown()
        {
            //Get All Category List 
            List<DropdownModel> lstCaegory = objDLCategory.GetAllCategoryList().ToList();
            return lstCaegory;
        }

        public void UpdateCategoryStatusByID(int CategoryId, int status)
        {
            //Call GetCategoryBYId method of dataLayer which will return Datatable.
            objDLCategory.UpdateCategoryStatusByID(CategoryId, status);

            
        }
      //  UpdateCategoryStatusByID
        #endregion
    }
}
