using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RepidShare.Data;
using RepidShare.Business;
using RepidShare.Entities;


namespace RepidShare.Business
{
    public class BLStep : BLBase
    {
        private DLStep objDLStep = new DLStep();
        #region Get ,Insert, Update and delete Step
        /// <summary>
        /// Get Step By Id
        /// </summary>
        /// <param name="StepId"></param>
        /// <returns>Step Model</returns>
        public StepModel GetStepById(int StepId)
        {
            //Call GetStepBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLStep.GetStepById(StepId);
            StepModel objStepModel = new StepModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objStepModel = GetDataRowToEntity<StepModel>(dt.Rows[0]);
            }

            return objStepModel;
        }

        /// <summary>
        /// Insert or Update  Step
        /// </summary>
        /// <param name="objStepModel">object of  Step Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public StepModel InsertUpdateStep(StepModel objStepModel)
        {
            //call InsertUpdateStep Method of dataLayer and return StepModel
            return objDLStep.InsertUpdateStep(objStepModel);
        }

        /// <summary>
        /// Delete  Step by  Step ID
        /// </summary>
        /// <param name="objViewStepModel"></param>
        /// <param name="createdBy"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public ViewStepModel DeleteStep(ViewStepModel objViewStepModel)
        {
            return objDLStep.DeleteStep(objViewStepModel);
        }

        #endregion

        #region View  Step
        /// <summary>
        /// Get  Step List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewStepModel">object of Model ViewStepModel</param>
        /// <returns></returns>
        public ViewStepModel GetStepList(ViewStepModel objViewStepModel)
        {
            List<StepModel> lstStepModel = new List<StepModel>();
            //if FilterStepName is NULL than set to empty
            objViewStepModel.FilterSubCatName = objViewStepModel.FilterSubCatName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewStepModel.SortBy = objViewStepModel.SortBy ?? String.Empty;

            //call GetStepList Method which will retrun datatable of  Step
            DataTable dtStep = objDLStep.GetStepList(objViewStepModel);

            //fetch each row of datable
            foreach (DataRow dr in dtStep.Rows)
            {
                //Convert datarow into Model object and set Model object property
                StepModel itemStepModel = GetDataRowToEntity<StepModel>(dr);
                //Add  Step in List
                lstStepModel.Add(itemStepModel);
            }

            //set Step List of Model ViewStepModel
            objViewStepModel.StepList = lstStepModel;
            //if  Step List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewStepModel != null && objViewStepModel.StepList != null && objViewStepModel.StepList.Count > 0)
            {
                objViewStepModel.CurrentPage = objViewStepModel.StepList[0].CurrentPage;
                int totalRecord = objViewStepModel.StepList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewStepModel.PageSize) > 0)
                    objViewStepModel.TotalPages = (totalRecord / objViewStepModel.PageSize + 1);
                else
                    objViewStepModel.TotalPages = totalRecord / objViewStepModel.PageSize;


            }
            else
            {
                objViewStepModel.TotalPages = 0;
                objViewStepModel.CurrentPage = 1;
            }
            return objViewStepModel;
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
        public List<DropdownModel> FillStepDropDown()
        {
            //Get All Step List 
            List<DropdownModel> lstCaegory = objDLStep.GetAllStepList().ToList();
            return lstCaegory;
        }

        /// <summary>
        /// FillStepByDropDownId
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public List<DropdownModel> FillStepByDocumentId(int DocumentId)
        {
            //Get All Step List 
            List<DropdownModel> lstCaegory = objDLStep.GetAllStepListByDocumentId(DocumentId).ToList();
            return lstCaegory;
        }

        public List<StepModel> GetStepByDocumentIdAndUserId(int DocumentId, int UserId)
        {
            //Get All Step List 
            return objDLStep.GetStepByDocumentIdAndUserId(DocumentId, UserId);
        }


        #endregion
    }
}
