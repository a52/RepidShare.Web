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
   public  class BLLawGuide : BLBase
    {
        private DLLawGuide objDLLawGuide = new DLLawGuide();
        #region Get ,Insert, Update and delete LawGuide
        /// <summary>
        /// Get LawGuide By Id
        /// </summary>
        /// <param name="LawGuideId"></param>
        /// <returns>LawGuide Model</returns>
        public LawGuideModel GetLawGuideById(int LawGuideId)
        {
            //Call GetLawGuideBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLLawGuide.GetLawGuideById(LawGuideId);
            LawGuideModel objLawGuideModel = new LawGuideModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objLawGuideModel = GetDataRowToEntity<LawGuideModel>(dt.Rows[0]);
            }

            return objLawGuideModel;
        }

        /// <summary>
        /// Insert or Update  LawGuide
        /// </summary>
        /// <param name="objLawGuideModel">object of  LawGuide Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public LawGuideModel InsertUpdateLawGuide(LawGuideModel objLawGuideModel)
        {
            //call InsertUpdateLawGuide Method of dataLayer and return LawGuideModel
            return objDLLawGuide.InsertUpdateLawGuide(objLawGuideModel);
        }

        /// <summary>
        /// Delete  LawGuide by  LawGuide ID
        /// </summary>
        /// <param name="objViewLawGuideModel"></param>
        /// <param name="createdBy"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public ViewLawGuideModel DeleteLawGuide(ViewLawGuideModel objViewLawGuideModel)
        {
            return objDLLawGuide.DeleteLawGuide(objViewLawGuideModel);
        }

        #endregion

        #region View  LawGuide
        /// <summary>
        /// Get  LawGuide List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewLawGuideModel">object of Model ViewLawGuideModel</param>
        /// <returns></returns>
        public ViewLawGuideModel GetLawGuideList(ViewLawGuideModel objViewLawGuideModel)
        {
            List<LawGuideModel> lstLawGuideModel = new List<LawGuideModel>();
            //if FilterLawGuideName is NULL than set to empty
            objViewLawGuideModel.FilterSubCatName = objViewLawGuideModel.FilterSubCatName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewLawGuideModel.SortBy = objViewLawGuideModel.SortBy ?? String.Empty;

            //call GetLawGuideList Method which will retrun datatable of  LawGuide
            DataTable dtLawGuide = objDLLawGuide.GetLawGuideList(objViewLawGuideModel);

            //fetch each row of datable
            foreach (DataRow dr in dtLawGuide.Rows)
            {
                //Convert datarow into Model object and set Model object property
                LawGuideModel itemLawGuideModel = GetDataRowToEntity<LawGuideModel>(dr);
                //Add  LawGuide in List
                lstLawGuideModel.Add(itemLawGuideModel);
            }

            //set LawGuide List of Model ViewLawGuideModel
            objViewLawGuideModel.LawGuideList = lstLawGuideModel;
            //if  LawGuide List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewLawGuideModel != null && objViewLawGuideModel.LawGuideList != null && objViewLawGuideModel.LawGuideList.Count > 0)
            {
                objViewLawGuideModel.CurrentPage = objViewLawGuideModel.LawGuideList[0].CurrentPage;
                int totalRecord = objViewLawGuideModel.LawGuideList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewLawGuideModel.PageSize) > 0)
                    objViewLawGuideModel.TotalPages = (totalRecord / objViewLawGuideModel.PageSize + 1);
                else
                    objViewLawGuideModel.TotalPages = totalRecord / objViewLawGuideModel.PageSize;


            }
            else
            {
                objViewLawGuideModel.TotalPages = 0;
                objViewLawGuideModel.CurrentPage = 1;
            }
            return objViewLawGuideModel;
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
        public List<DropdownModel> FillLawGuideDropDown(int? CategoryID, int? GroupID)
        {
            //Get All LawGuide List 
            List<DropdownModel> lstCaegory = objDLLawGuide.GetAllLawGuideList(CategoryID, GroupID).ToList();
            return lstCaegory;
        }
        #endregion
    }
}
