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
    public class BLBulletin : BLBase
    {
        private DLBulletin objDLBulletin = new DLBulletin();
        #region Get ,Insert, Update and delete Bulletin
        /// <summary>
        /// Get Bulletin By Id
        /// </summary>
        /// <param name="BulletinId"></param>
        /// <returns>Bulletin Model</returns>
        public BulletinModel GetBulletinById(int BulletinId)
        {
            //Call GetBulletinBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLBulletin.GetBulletinById(BulletinId);
            BulletinModel objBulletinModel = new BulletinModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objBulletinModel = GetDataRowToEntity<BulletinModel>(dt.Rows[0]);
            }

            return objBulletinModel;
        }

        /// <summary>
        /// Insert or Update  Bulletin
        /// </summary>
        /// <param name="objBulletinModel">object of  Bulletin Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public BulletinModel InsertUpdateBulletin(BulletinModel objBulletinModel)
        {
            //call InsertUpdateBulletin Method of dataLayer and return BulletinModel
            return objDLBulletin.InsertUpdateBulletin(objBulletinModel);
        }

        /// <summary>
        /// Delete  Bulletin by  Bulletin ID
        /// </summary>
        /// <param name="objViewBulletinModel"></param>
        /// <param name="createdBy"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public ViewBulletinModel DeleteBulletin(ViewBulletinModel objViewBulletinModel)
        {
           return objDLBulletin.DeleteBulletin(objViewBulletinModel);
        }

        #endregion

        #region View  Bulletin
        /// <summary>
        /// Get  Bulletin List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewBulletinModel">object of Model ViewBulletinModel</param>
        /// <returns></returns>
        public ViewBulletinModel GetBulletinList(ViewBulletinModel objViewBulletinModel)
        {
            List<BulletinModel> lstBulletinModel = new List<BulletinModel>();
            //if FilterBulletinName is NULL than set to empty
            objViewBulletinModel.FilterBulletinName = objViewBulletinModel.FilterBulletinName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewBulletinModel.SortBy = objViewBulletinModel.SortBy ?? String.Empty;

            //call GetBulletinList Method which will retrun datatable of  Bulletin
            DataTable dtBulletin = objDLBulletin.GetBulletinList(objViewBulletinModel);

            //fetch each row of datable
            foreach (DataRow dr in dtBulletin.Rows)
            {
                //Convert datarow into Model object and set Model object property
                BulletinModel itemBulletinModel = GetDataRowToEntity<BulletinModel>(dr);
                //Add  Bulletin in List
                lstBulletinModel.Add(itemBulletinModel);
            }

            //set Bulletin List of Model ViewBulletinModel
            objViewBulletinModel.BulletinList = lstBulletinModel;
            //if  Bulletin List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewBulletinModel != null && objViewBulletinModel.BulletinList != null && objViewBulletinModel.BulletinList.Count > 0)
            {
                objViewBulletinModel.CurrentPage = objViewBulletinModel.BulletinList[0].CurrentPage;
                int totalRecord = objViewBulletinModel.BulletinList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewBulletinModel.PageSize) > 0)
                    objViewBulletinModel.TotalPages = (totalRecord / objViewBulletinModel.PageSize + 1);
                else
                    objViewBulletinModel.TotalPages = totalRecord / objViewBulletinModel.PageSize;


            }
            else
            {
                objViewBulletinModel.TotalPages = 0;
                objViewBulletinModel.CurrentPage = 1;
            }
            return objViewBulletinModel;
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
            //Get All Bulletin List 
            List<DropdownModel> lstCaegory = objDLBulletin.GetAllBulletinList().ToList();
            return lstCaegory;
        }

        public void UpdateBulletinStatusByID(int BulletinId, int status)
        {
            //Call GetBulletinBYId method of dataLayer which will return Datatable.
            objDLBulletin.UpdateBulletinStatusByID(BulletinId, status);

            
        }
      //  UpdateBulletinStatusByID
        #endregion
    }
}
