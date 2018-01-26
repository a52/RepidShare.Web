using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities;
using System.Data;
using System.Reflection;
using RepidShare.Data;

namespace RepidShare.Business
{
    public class BLBase
    {
        #region Generic Methods
        // Genric Function
        /// <summary>
        /// This methods is used to fill all property dynamically from data row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public T GetDataRowToEntity<T>(DataRow dr) where T : new()
        {
            try
            {
                var objT = new T();
                PropertyInfo[] objprop = objT.GetType().GetProperties();
                foreach (PropertyInfo pr in objprop)
                {
                    if (dr.Table.Columns[pr.Name] != null && dr[pr.Name] != null && dr[pr.Name] != DBNull.Value)
                    {
                        var obj = dr[pr.Name];
                        pr.SetValue(objT, obj, null);
                    }
                }
                return objT;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }


        #endregion

        #region ErrorLog
        /// <summary>
        /// Insert exception or error detail into the database.
        /// </summary>
        /// <param name="UserID">Id of Loggedin User</param>
        /// <param name="ErrorMsg">Detail of error</param>
        /// <param name="ErrorStack">Stack detail of exception</param>
        /// <param name="ControllerName">Name of controller</param>
        /// <param name="FunctionName">Name of action where exception or error occur.</param> 
        public void SaveErrorLog(int UserID, string ErrorMsg, string ErrorStack, string ControllerName, string FunctionName)
        {
            try
            {
                RepidShare.Data.DLErrorLog.InsertErrorLog(UserID, ErrorMsg, ErrorStack, ControllerName, FunctionName);
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region Total Page Calculating
        /// <summary>
        /// Total Page Calculating
        /// </summary>
        /// <param name="totalRecord"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        /// <Remarks>
        /// Created By Rakesh Patidar
        /// </Remarks>
        public int TotalPage(int totalRecord, int PageSize)
        {
            if (decimal.Remainder(totalRecord, PageSize) > 0)
                return (totalRecord / PageSize + 1);
            else
                return totalRecord / PageSize;
        }
        #endregion
    }
}
