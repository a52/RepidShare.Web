using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RepidShare.Entities;


namespace RepidShare.Data
{
    public class DLHome
    {

        public DataSet GetCategoryByID(int CategoryId)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@CategoryId", CategoryId)
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_CategoryView, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds;
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSubCategoryByID(int SubCategoryId)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@SubCategoryId", SubCategoryId)
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_SubCategoryView, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds;
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLawGuideByCategoryId(int CategoryId)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@CategoryId", CategoryId)
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_LawGuideView, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds;
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocumentByDocumentId(int DocumentId)
        {
            try
            {
                SqlParameter[] parmList = { 
                                     
                                      new SqlParameter("@DocumentId", DocumentId)
                                      };

                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.ConnectionStringLocalTransaction, CommandType.StoredProcedure, DBConstants.Web_DocumentView, parmList);

                if (ds != null && ds.Tables.Count > 0)
                    return ds;
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
