using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLGroup
    {
        #region Get And Insert Update  Group
        /// <summary>
        /// Get Group by GroupId
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public DataTable GetGroupById(int GroupId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@GroupID",GroupId),
                                      };
            //Call SPGETGroupBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetGroupByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  Group
        /// </summary>
        /// <param name="objGroupModel"></param>
        /// <returns></returns>
        public GroupModel InsertUpdateGroup(GroupModel objGroupModel)
        {
            try
            {
                objGroupModel.GroupText = objGroupModel.GroupText.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@GroupID",objGroupModel.GroupID)
                                        ,new SqlParameter("@GroupText",objGroupModel.GroupText)                                        
                                        ,new SqlParameter("@IsActive", objGroupModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objGroupModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage


                                          };

                //If  GroupId is 0 Than Insert  Group else Update  Group
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateGroup, parmList);
                //set error code and message 
                objGroupModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objGroupModel.Message = Convert.ToString(pErrorMessage.Value);
                return objGroupModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Delete Sub Category
        /// </summary>
        /// <param name="objViewSubCategoryModel"></param>
        /// <returns></returns>
        public ViewGroupModel DeleteGroup(ViewGroupModel objViewGroupModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", objViewGroupModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewGroupModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@GroupID",objViewGroupModel.DeletedGroupID)
                                         ,new SqlParameter("@CreatedBy",objViewGroupModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  category
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteGroup, parmList);
                //set output parameter error code and error message
                objViewGroupModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewGroupModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewGroupModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region View  Group
        /// <summary>
        /// Get  Group List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewGroupModel">object of Model ViewGroupModel </param>
        /// <returns></returns>
        public DataTable GetGroupList(ViewGroupModel objViewGroupModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@GroupText", objViewGroupModel.FilterGroupText)
                                     ,new SqlParameter("@CurrentPage", objViewGroupModel.CurrentPage)
                                     ,new SqlParameter("@PageSize", objViewGroupModel.PageSize)
                                     ,new SqlParameter("@SortBy", objViewGroupModel.SortBy)
                                     ,new SqlParameter("@SortOrder", objViewGroupModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetGroupList, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  Group Drop Down
        /// <summary>
        /// Get All  Group List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllGroupListForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetGroupListForDDL, parmList);
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get All  Group List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllGroupList()
        {
            try
            {
                List<DropdownModel> lstGroup = new List<DropdownModel>();
                //Get All  Group list
                DataTable dtGroup = GetAllGroupListForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtGroup.Rows)
                {
                    lstGroup.Add
                        (new DropdownModel()
                            {
                                ID = Convert.ToInt32(dr["GroupID"]),
                                Value = Convert.ToString(dr["GroupText"])
                            }
                        );
                }
                return lstGroup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
