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
    public class BLDropDown : BLBase
    {
        private DLDropDown objDLDropDown = new DLDropDown();

        #region Get ,Insert, Update and delete DropDown
        /// <summary>
        /// Get DropDown By Id
        /// </summary>
        /// <param name="DropDownId"></param>
        /// <returns>DropDown Model</returns>
        public DropDownModel GetDropDownById(int DropDownId)
        {
            //Call GetDropDownBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLDropDown.GetDropDownById(DropDownId);
            DropDownModel objDropDownModel = new DropDownModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objDropDownModel = GetDataRowToEntity<DropDownModel>(dt.Rows[0]);
            }

            return objDropDownModel;
        }

        /// <summary>
        /// Insert or Update  DropDown
        /// </summary>
        /// <param name="objDropDownModel">object of  DropDown Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public DropDownModel InsertUpdateDropDown(DropDownModel objDropDownModel)
        {
            //call InsertUpdateDropDown Method of dataLayer and return DropDownModel
            return objDLDropDown.InsertUpdateDropDown(objDropDownModel);
        }

        /// <summary>
        /// Delete  DropDown by  DropDown ID
        /// </summary>
        /// <param name="objViewDropDownModel"></param>
        /// <param name="createdBy"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public ViewDropDownModel DeleteDropDown(ViewDropDownModel objViewDropDownModel)
        {
           return objDLDropDown.DeleteDropDown(objViewDropDownModel);
        }

        #endregion

        #region View  DropDown
        /// <summary>
        /// Get  DropDown List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewDropDownModel">object of Model ViewDropDownModel</param>
        /// <returns></returns>
        public ViewDropDownModel GetDropDownList(ViewDropDownModel objViewDropDownModel)
        {
            List<DropDownModel> lstDropDownModel = new List<DropDownModel>();
            //if FilterDropDownName is NULL than set to empty
            objViewDropDownModel.FilterDropDownName = objViewDropDownModel.FilterDropDownName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewDropDownModel.SortBy = objViewDropDownModel.SortBy ?? String.Empty;

            //call GetDropDownList Method which will retrun datatable of  DropDown
            DataTable dtDropDown = objDLDropDown.GetDropDownList(objViewDropDownModel);

            //fetch each row of datable
            foreach (DataRow dr in dtDropDown.Rows)
            {
                //Convert datarow into Model object and set Model object property
                DropDownModel itemDropDownModel = GetDataRowToEntity<DropDownModel>(dr);
                //Add  DropDown in List
                lstDropDownModel.Add(itemDropDownModel);
            }

            //set DropDown List of Model ViewDropDownModel
            objViewDropDownModel.DropDownList = lstDropDownModel;
            //if  DropDown List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewDropDownModel != null && objViewDropDownModel.DropDownList != null && objViewDropDownModel.DropDownList.Count > 0)
            {
                objViewDropDownModel.CurrentPage = objViewDropDownModel.DropDownList[0].CurrentPage;
                int totalRecord = objViewDropDownModel.DropDownList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewDropDownModel.PageSize) > 0)
                    objViewDropDownModel.TotalPages = (totalRecord / objViewDropDownModel.PageSize + 1);
                else
                    objViewDropDownModel.TotalPages = totalRecord / objViewDropDownModel.PageSize;


            }
            else
            {
                objViewDropDownModel.TotalPages = 0;
                objViewDropDownModel.CurrentPage = 1;
            }
            return objViewDropDownModel;
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
            //Get All DropDown List 
            List<DropdownModel> lstCaegory = objDLDropDown.GetAllDropDownList().ToList();
            return lstCaegory;
        }
        #endregion
    }
}
