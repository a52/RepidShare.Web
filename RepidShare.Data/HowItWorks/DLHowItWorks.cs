using System.Data;
using System.Data.SqlClient;

namespace RepidShare.Data
{
    public class DLHowItWorks
    {
        private DLHowItWorks howItWorks = new DLHowItWorks();
        #region  #region Get ,Insert and Update  How it works

        public DataTable GetById(int id)
        {
            SqlParameter[] param = {
                                          new SqlParameter("@Id",id),
                                      };
            //Call SPGETMasterBYID stored procedure which will return dataset
            DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Admin_GetHowItWorkdsById, param);

            //if dataset is not null and tables count is greater than 0 than return dataset else return null
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        } 
        #endregion
    }
}
