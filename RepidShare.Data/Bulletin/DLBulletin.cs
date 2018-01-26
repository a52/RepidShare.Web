using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLBulletin
    {
        #region Get And Insert Update  Bulletin
        /// <summary>
        /// Get Bulletin by BulletinId
        /// </summary>
        /// <param name="BulletinId"></param>
        /// <returns></returns>
        public DataTable GetBulletinById(int BulletinId)
        {

            SqlParameter[] param = {
                                          new SqlParameter("@BulletinID",BulletinId),
                                      };
            //Call SPGETBulletinBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetBulletinByID, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Insert or Update  Bulletin
        /// </summary>
        /// <param name="objBulletinModel"></param>
        /// <returns></returns>
        public BulletinModel InsertUpdateBulletin(BulletinModel objBulletinModel)
        {
            try
            {
                objBulletinModel.BulletinName = objBulletinModel.BulletinName.ToString().Trim();
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@BulletinID",objBulletinModel.BulletinID)
                                        ,new SqlParameter("@BulletinName",objBulletinModel.BulletinName)   
                                        ,new SqlParameter("@Description",objBulletinModel.Description)  
                                        ,new SqlParameter("@ClassName",objBulletinModel.ClassName)
                                        ,new SqlParameter("@AttachmentID",objBulletinModel.AttachmentID)   
                                        ,new SqlParameter("@AttachmentType",objBulletinModel.AttachmentType)  
                                         ,new SqlParameter("@AttachmentName",objBulletinModel.AttachmentName)  
                                        ,new SqlParameter("@AttachmentSize",objBulletinModel.AttachmentSize)   
                                        ,new SqlParameter("@AttachmentContent",objBulletinModel.AttachmentContent)   
                                        ,new SqlParameter("@IsActive", objBulletinModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objBulletinModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage

                                          };

                //If  BulletinId is 0 Than Insert  Bulletin else Update  Bulletin
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateBulletin, parmList);
                //set error code and message 
                objBulletinModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objBulletinModel.Message = Convert.ToString(pErrorMessage.Value);
                return objBulletinModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete Bulletin
        /// </summary>
        /// <param name="objViewBulletinModel"></param>
        /// <returns></returns>
        public ViewBulletinModel DeleteBulletin(ViewBulletinModel objViewBulletinModel)
        {
            try
            {
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode",objViewBulletinModel.ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", objViewBulletinModel.Message);
                pErrorMessage.Direction = ParameterDirection.Output;

                SqlParameter[] parmList = { 
                                     	  new SqlParameter("@BulletinID",objViewBulletinModel.DeletedBulletinID)
                                         ,new SqlParameter("@CreatedBy",objViewBulletinModel.DeletedBy)
                                         ,pErrorCode                
                                         ,pErrorMessage
                 
                                        };
                //Call delete stored procedure to delete  Bulletin
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_DeleteBulletin, parmList);
                //set output parameter error code and error message
                objViewBulletinModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objViewBulletinModel.Message = Convert.ToString(pErrorMessage.Value);
                return objViewBulletinModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region View  Bulletin
        /// <summary>
        /// Get  Bulletin List  with paging, sorting and searching parameter
        /// </summary>
        /// <param name="objViewBulletinModel">object of Model ViewBulletinModel </param>
        /// <returns></returns>
        public DataTable GetBulletinList(ViewBulletinModel objViewBulletinModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("BulletinName", objViewBulletinModel.FilterBulletinName)
                                     ,new SqlParameter("CurrentPage", objViewBulletinModel.CurrentPage)
                                     ,new SqlParameter("PageSize", objViewBulletinModel.PageSize)
                                     ,new SqlParameter("SortBy", objViewBulletinModel.SortBy)
                                     ,new SqlParameter("SortOrder", objViewBulletinModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetBulletinList, parmList);

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

        #region  Bulletin Drop Down
        /// <summary>
        /// Get All  Bulletin List
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllBulletinListForDDL()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetBulletinListForDDL, parmList);
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
        /// Get All  Bulletin List Result Return in DropdownModel
        /// </summary>
        /// <returns>DropdownModel</returns>
        public List<DropdownModel> GetAllBulletinList()
        {
            try
            {
                List<DropdownModel> lstBulletin = new List<DropdownModel>();
                //Get All  Bulletin list
                DataTable dtBulletin = GetAllBulletinListForDDL();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtBulletin.Rows)
                {
                    lstBulletin.Add
                        (new DropdownModel()
                            {
                                ID = Convert.ToInt32(dr["BulletinID"]),
                                Value = Convert.ToString(dr["BulletinName"])
                            }
                        );
                }
                return lstBulletin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String UpdateBulletinStatusByID(int BulletinId, int status)
        {
            try
            {
                //Set Param values by objViewParameters
                SqlParameter[] Param = {     

                                           new SqlParameter("@BulletinId", BulletinId),
                                           new SqlParameter("@status", status)
                                       };

                //Call spGetDocumentResponse Procedure for view 
                return SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_UpdateBulletinStatusByID, Param).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
