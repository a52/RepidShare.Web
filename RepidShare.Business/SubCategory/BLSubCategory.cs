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
    public class BLSubCategory : BLBase
    {
        private DLSubCategory objDLSubCategory = new DLSubCategory();
        #region Get ,Insert, Update and delete SubCategory
        /// <summary>
        /// Get SubCategory By Id
        /// </summary>
        /// <param name="SubCategoryId"></param>
        /// <returns>SubCategory Model</returns>
        public SubCategoryModel GetSubCategoryById(int SubCategoryId)
        {
            //Call GetSubCategoryBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLSubCategory.GetSubCategoryById(SubCategoryId);
            SubCategoryModel objSubCategoryModel = new SubCategoryModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objSubCategoryModel = GetDataRowToEntity<SubCategoryModel>(dt.Rows[0]);
            }

            return objSubCategoryModel;
        }

        /// <summary>
        /// Insert or Update  SubCategory
        /// </summary>
        /// <param name="objSubCategoryModel">object of  SubCategory Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public SubCategoryModel InsertUpdateSubCategory(SubCategoryModel objSubCategoryModel)
        {
            //call InsertUpdateSubCategory Method of dataLayer and return SubCategoryModel
            return objDLSubCategory.InsertUpdateSubCategory(objSubCategoryModel);
        }

        /// <summary>
        /// Delete  SubCategory by  SubCategory ID
        /// </summary>
        /// <param name="objViewSubCategoryModel"></param>
        /// <param name="createdBy"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public ViewSubCategoryModel DeleteSubCategory(ViewSubCategoryModel objViewSubCategoryModel)
        {
            return objDLSubCategory.DeleteSubCategory(objViewSubCategoryModel);
        }

        #endregion

        #region View  SubCategory
        /// <summary>
        /// Get  SubCategory List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewSubCategoryModel">object of Model ViewSubCategoryModel</param>
        /// <returns></returns>
        public ViewSubCategoryModel GetSubCategoryList(ViewSubCategoryModel objViewSubCategoryModel)
        {
            List<SubCategoryModel> lstSubCategoryModel = new List<SubCategoryModel>();
            //if FilterSubCategoryName is NULL than set to empty
            objViewSubCategoryModel.FilterSubCatName = objViewSubCategoryModel.FilterSubCatName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewSubCategoryModel.SortBy = objViewSubCategoryModel.SortBy ?? String.Empty;

            //call GetSubCategoryList Method which will retrun datatable of  SubCategory
            DataTable dtSubCategory = objDLSubCategory.GetSubCategoryList(objViewSubCategoryModel);

            //fetch each row of datable
            foreach (DataRow dr in dtSubCategory.Rows)
            {
                //Convert datarow into Model object and set Model object property
                SubCategoryModel itemSubCategoryModel = GetDataRowToEntity<SubCategoryModel>(dr);
                //Add  SubCategory in List
                lstSubCategoryModel.Add(itemSubCategoryModel);
            }

            //set SubCategory List of Model ViewSubCategoryModel
            objViewSubCategoryModel.SubCategoryList = lstSubCategoryModel;
            //if  SubCategory List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewSubCategoryModel != null && objViewSubCategoryModel.SubCategoryList != null && objViewSubCategoryModel.SubCategoryList.Count > 0)
            {
                objViewSubCategoryModel.CurrentPage = objViewSubCategoryModel.SubCategoryList[0].CurrentPage;
                int totalRecord = objViewSubCategoryModel.SubCategoryList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewSubCategoryModel.PageSize) > 0)
                    objViewSubCategoryModel.TotalPages = (totalRecord / objViewSubCategoryModel.PageSize + 1);
                else
                    objViewSubCategoryModel.TotalPages = totalRecord / objViewSubCategoryModel.PageSize;


            }
            else
            {
                objViewSubCategoryModel.TotalPages = 0;
                objViewSubCategoryModel.CurrentPage = 1;
            }
            return objViewSubCategoryModel;
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
        public List<DropdownModel> FillSubCategoryDropDown(int? CategoryID, int? GroupID)
        {
            //Get All SubCategory List 
            List<DropdownModel> lstCaegory = objDLSubCategory.GetAllSubCategoryList(CategoryID,GroupID).ToList();
            return lstCaegory;
        }
        #endregion

        public void UpdateSubCategoryStatusByID(int SubCategoryId, int status)
        {
            //Call GetCategoryBYId method of dataLayer which will return Datatable.
            objDLSubCategory.UpdateSubCategoryStatusByID(SubCategoryId, status);


        }
    }
}
