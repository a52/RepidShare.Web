using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;
using System.Reflection;


namespace RepidShare.Data
{
    public class DLOrder
    {

        #region View  Order List
        /// <summary>
        /// Get  Order List  
        /// </summary>
        /// <param name="objViewOrderModel">object of Model ViewDocumentModel </param>
        /// <returns></returns>
        public DataTable GetOrderList(ViewOrderModel objViewOrderModel)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     new SqlParameter("@CurrentPage", objViewOrderModel.CurrentPage)
                                     ,new SqlParameter("@PageSize", objViewOrderModel.PageSize)
                                     ,new SqlParameter("@SortBy", objViewOrderModel.SortBy)
                                     ,new SqlParameter("@SortOrder", objViewOrderModel.SortOrder)
                                  
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetOrderList, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetOrderDetailList(int OrderId)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     new SqlParameter("@OrderId", OrderId)
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetOrderDetailList, parmList);

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

        public OdersModel InsertUpdateOrder(OdersModel objOdersModel)
        {
            try
            {
                int ErrorCode = 0;
                string ErrorMessage = "";
                SqlParameter pErrorCode = new SqlParameter("@ErrorCode", ErrorCode);
                pErrorCode.Direction = ParameterDirection.Output;
                SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", ErrorMessage);
                pErrorMessage.Direction = ParameterDirection.Output;
                pErrorMessage.Size = 8000;

                SqlParameter[] parmList = { 
                                     	 new SqlParameter("@OrderID",objOdersModel.OrderID)
                                        ,new SqlParameter("@UserId",objOdersModel.UserId)   
                                        ,new SqlParameter("@PaymentID",objOdersModel.PaymentID)
                                        ,new SqlParameter("@OrderDate",objOdersModel.OrderDate)
                                        ,new SqlParameter("@RefTimestamp",objOdersModel.RefTimestamp)
                                        ,new SqlParameter("@TransactStatus",objOdersModel.TransactStatus)
                                        ,new SqlParameter("@PaidTotal",objOdersModel.PaidTotal)
                                        ,new SqlParameter("@OdersDetailModelList",ToDataTable(objOdersModel.OdersDetailModelList))  
                                        ,new SqlParameter("@IsActive", objOdersModel.IsActive)
                                        ,new SqlParameter("@CreatedBy",objOdersModel.CreatedBy)
                                        ,pErrorCode                
                                       ,pErrorMessage
                                          };

                //If  DocumentId is 0 Than Insert  Document else Update  Document
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_InsertUpdateOrder, parmList);
                //set error code and message 
                objOdersModel.ErrorCode = Convert.ToInt32(pErrorCode.Value);
                objOdersModel.Message = Convert.ToString(pErrorMessage.Value);
                return objOdersModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            dataTable.Columns.Remove("RowNumber");
            dataTable.Columns.Remove("DocumentTitle");

            return dataTable;
        }

    }
}
