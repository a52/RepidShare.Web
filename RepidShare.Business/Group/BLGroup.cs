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
    public class BLGroup : BLBase
    {
        private DLGroup objDLGroup = new DLGroup();
        #region Get ,Insert, Update and delete Group
        /// <summary>
        /// Get Group By Id
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns>Group Model</returns>
        public GroupModel GetGroupById(int GroupId)
        {
            //Call GetGroupBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLGroup.GetGroupById(GroupId);
            GroupModel objGroupModel = new GroupModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objGroupModel = GetDataRowToEntity<GroupModel>(dt.Rows[0]);
            }

            return objGroupModel;
        }

        /// <summary>
        /// Insert or Update  Group
        /// </summary>
        /// <param name="objGroupModel">object of  Group Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public GroupModel InsertUpdateGroup(GroupModel objGroupModel)
        {
            //call InsertUpdateGroup Method of dataLayer and return GroupModel
            return objDLGroup.InsertUpdateGroup(objGroupModel);
        }

        public ViewGroupModel DeleteGroup(ViewGroupModel objViewGroupModel)
        {
            return objDLGroup.DeleteGroup(objViewGroupModel);
        }
        #endregion

        #region View  Group
        /// <summary>
        /// Get  Group List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewGroupModel">object of Model ViewGroupModel</param>
        /// <returns></returns>
        public ViewGroupModel GetGroupList(ViewGroupModel objViewGroupModel)
        {
            List<GroupModel> lstGroupModel = new List<GroupModel>();
            //if FilterGroupName is NULL than set to empty
            objViewGroupModel.FilterSubCatName = objViewGroupModel.FilterSubCatName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewGroupModel.SortBy = objViewGroupModel.SortBy ?? String.Empty;

            //call GetGroupList Method which will retrun datatable of  Group
            DataTable dtGroup = objDLGroup.GetGroupList(objViewGroupModel);

            //fetch each row of datable
            foreach (DataRow dr in dtGroup.Rows)
            {
                //Convert datarow into Model object and set Model object property
                GroupModel itemGroupModel = GetDataRowToEntity<GroupModel>(dr);
                //Add  Group in List
                lstGroupModel.Add(itemGroupModel);
            }

            //set Group List of Model ViewGroupModel
            objViewGroupModel.GroupList = lstGroupModel;
            //if  Group List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewGroupModel != null && objViewGroupModel.GroupList != null && objViewGroupModel.GroupList.Count > 0)
            {
                objViewGroupModel.CurrentPage = objViewGroupModel.GroupList[0].CurrentPage;
                int totalRecord = objViewGroupModel.GroupList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewGroupModel.PageSize) > 0)
                    objViewGroupModel.TotalPages = (totalRecord / objViewGroupModel.PageSize + 1);
                else
                    objViewGroupModel.TotalPages = totalRecord / objViewGroupModel.PageSize;


            }
            else
            {
                objViewGroupModel.TotalPages = 0;
                objViewGroupModel.CurrentPage = 1;
            }
            return objViewGroupModel;
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
        public List<DropdownModel> FillGroupDropDown()
        {
            //Get All Group List 
            List<DropdownModel> lstCaegory = objDLGroup.GetAllGroupList().ToList();
            return lstCaegory;
        }
        #endregion
    }
}
