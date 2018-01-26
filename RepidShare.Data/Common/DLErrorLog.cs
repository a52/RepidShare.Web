using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace RepidShare.Data
{
    public class DLErrorLog
    {

        /// <summary>
        /// Insert exception or error detail into the database.
        /// </summary>
        /// <param name="UserID">Id of Loggedin User</param>
        /// <param name="ErrorMsg">Detail of error</param>
        /// <param name="ErrorStack">Stack detail of exception</param>
        /// <param name="ControllerName">Name of controller</param>
        /// <param name="FunctionName">Name of action where exception or error occur.</param>
        public static void InsertErrorLog(int UserID, string ErrorMsg, string ErrorStack, string ControllerName, string FunctionName)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@UserId",UserID),
                                       new SqlParameter("@ErrorMessage",ErrorMsg),
                                        new SqlParameter("@ErrorStack",ErrorStack),
                                         new SqlParameter("@ErrorPage",ControllerName),
                                         new SqlParameter("@ErrorFunction",FunctionName),
                                         new SqlParameter("@ErrorDate",DateTime.Now)
                                     
                                      };
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_INSERTERRORLOG, parmList);

            }
            catch (Exception ex)
            {

            }
        }

    }
}
