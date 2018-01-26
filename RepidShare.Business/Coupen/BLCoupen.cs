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
    public class BLCoupen : BLBase
    {
        private DLCoupen objDLCoupen = new DLCoupen();
        #region Get ,Insert, Update and delete Coupen
        /// <summary>
        /// Get Coupen By Id
        /// </summary>
        /// <param name="CoupenId"></param>
        /// <returns>Coupen Model</returns>
        public CoupenModel GetCoupenById(int CoupenId)
        {
            //Call GetCoupenBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLCoupen.GetCoupenById(CoupenId);
            CoupenModel objCoupenModel = new CoupenModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objCoupenModel = GetDataRowToEntity<CoupenModel>(dt.Rows[0]);
            }

            return objCoupenModel;
        }

        /// <summary>
        /// Insert or Update  Coupen
        /// </summary>
        /// <param name="objCoupenModel">object of  Coupen Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public CoupenModel InsertUpdateCoupen(CoupenModel objCoupenModel)
        {
            //call InsertUpdateCoupen Method of dataLayer and return CoupenModel
            return objDLCoupen.InsertUpdateCoupen(objCoupenModel);
        }

        public ViewCoupenModel DeleteCoupen(ViewCoupenModel objViewCoupenModel)
        {
            return objDLCoupen.DeleteCoupen(objViewCoupenModel);
        }
        #endregion

        #region View  Coupen
        /// <summary>
        /// Get  Coupen List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewCoupenModel">object of Model ViewCoupenModel</param>
        /// <returns></returns>
        public ViewCoupenModel GetCoupenList(ViewCoupenModel objViewCoupenModel)
        {
            List<CoupenModel> lstCoupenModel = new List<CoupenModel>();
            //if FilterCoupenName is NULL than set to empty
            objViewCoupenModel.FilterSubCatName = objViewCoupenModel.FilterSubCatName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewCoupenModel.SortBy = objViewCoupenModel.SortBy ?? String.Empty;

            //call GetCoupenList Method which will retrun datatable of  Coupen
            DataTable dtCoupen = objDLCoupen.GetCoupenList(objViewCoupenModel);

            //fetch each row of datable
            foreach (DataRow dr in dtCoupen.Rows)
            {
                //Convert datarow into Model object and set Model object property
                CoupenModel itemCoupenModel = GetDataRowToEntity<CoupenModel>(dr);
                //Add  Coupen in List
                lstCoupenModel.Add(itemCoupenModel);
            }

            //set Coupen List of Model ViewCoupenModel
            objViewCoupenModel.CoupenList = lstCoupenModel;
            //if  Coupen List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewCoupenModel != null && objViewCoupenModel.CoupenList != null && objViewCoupenModel.CoupenList.Count > 0)
            {
                objViewCoupenModel.CurrentPage = objViewCoupenModel.CoupenList[0].CurrentPage;
                int totalRecord = objViewCoupenModel.CoupenList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewCoupenModel.PageSize) > 0)
                    objViewCoupenModel.TotalPages = (totalRecord / objViewCoupenModel.PageSize + 1);
                else
                    objViewCoupenModel.TotalPages = totalRecord / objViewCoupenModel.PageSize;


            }
            else
            {
                objViewCoupenModel.TotalPages = 0;
                objViewCoupenModel.CurrentPage = 1;
            }
            return objViewCoupenModel;
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
        public List<DropdownModel> FillCoupenDropDown()
        {
            //Get All Coupen List 
            List<DropdownModel> lstCaegory = objDLCoupen.GetAllCoupenList().ToList();
            return lstCaegory;
        }
        #endregion
    }
}
