using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace RepidShare.Data
{
    public class DLQuestionType
    {
        /// <summary>
        /// Get Question Type List
        /// </summary>
        /// <returns></returns>
        public static DataTable GetQuesetionTypeList()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_GETQUESTIONTYPELIST);

                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetAllDisplayChoice()
        {
            try
            {
                SqlParameter[] parmList = { };
                DataTable dt = SQLHelper.ExecuteDataTable(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.ADMIN_GETALLDISPLAYCHOICE, parmList);
                if (dt != null)
                    return dt;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
