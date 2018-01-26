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
    public class BLMaster : BLBase
    {
        private DLMaster objDLMaster = new DLMaster();
        #region Get ,Insert, Update and delete Master
        /// <summary>
        /// Get Master By Id
        /// </summary>
        /// <param name="MasterId"></param>
        /// <returns>Master Model</returns>
        public MasterModel GetMasterById(int MasterId)
        {
            //Call GetMasterBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLMaster.GetMasterById(MasterId);
            MasterModel objMasterModel = new MasterModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objMasterModel = GetDataRowToEntity<MasterModel>(dt.Rows[0]);
            }

            return objMasterModel;
        }

        /// <summary>
        /// Insert or Update  Master
        /// </summary>
        /// <param name="objMasterModel">object of  Master Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public MasterModel InsertUpdateMaster(MasterModel objMasterModel)
        {
            //call InsertUpdateMaster Method of dataLayer and return MasterModel
            return objDLMaster.InsertUpdateMaster(objMasterModel);
        }

        public ViewMasterModel DeleteMaster(ViewMasterModel objViewMasterModel)
        {
            return objDLMaster.DeleteMaster(objViewMasterModel);
        }
        #endregion

        #region View  Master
        /// <summary>
        /// Get  Master List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewMasterModel">object of Model ViewMasterModel</param>
        /// <returns></returns>
        public ViewMasterModel GetMasterList(ViewMasterModel objViewMasterModel)
        {
            List<MasterModel> lstMasterModel = new List<MasterModel>();
            //if FilterMasterName is NULL than set to empty
            objViewMasterModel.FilterSubCatName = objViewMasterModel.FilterSubCatName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewMasterModel.SortBy = objViewMasterModel.SortBy ?? String.Empty;

            //call GetMasterList Method which will retrun datatable of  Master
            DataTable dtMaster = objDLMaster.GetMasterList(objViewMasterModel);

            //fetch each row of datable
            foreach (DataRow dr in dtMaster.Rows)
            {
                //Convert datarow into Model object and set Model object property
                MasterModel itemMasterModel = GetDataRowToEntity<MasterModel>(dr);
                //Add  Master in List
                lstMasterModel.Add(itemMasterModel);
            }

            //set Master List of Model ViewMasterModel
            objViewMasterModel.MasterList = lstMasterModel;
            //if  Master List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewMasterModel != null && objViewMasterModel.MasterList != null && objViewMasterModel.MasterList.Count > 0)
            {
                objViewMasterModel.CurrentPage = objViewMasterModel.MasterList[0].CurrentPage;
                int totalRecord = objViewMasterModel.MasterList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewMasterModel.PageSize) > 0)
                    objViewMasterModel.TotalPages = (totalRecord / objViewMasterModel.PageSize + 1);
                else
                    objViewMasterModel.TotalPages = totalRecord / objViewMasterModel.PageSize;


            }
            else
            {
                objViewMasterModel.TotalPages = 0;
                objViewMasterModel.CurrentPage = 1;
            }
            return objViewMasterModel;
        }
        #endregion

        #region Caegory DropDown
        /// <summary>
        /// Get  Caegory DropDown Item
        /// </summary>
        /// <returns></returns>
        ///<remarks>
        /// Created By Rakesh Patidar , 11 Feb 2015
        ///</remarks>
        public List<DropdownModel> FillMasterDropDown()
        {
            //Get All Master List 
            List<DropdownModel> lstCaegory = objDLMaster.GetAllMasterList().ToList();
            return lstCaegory;
        }
        #endregion
    }
}
