using RepidShare.Data;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Business.HowItWorks
{
   public class BLHowItWorks :BLBase
    {
        private DLHowItWorks objDLHowItWorks = new DLHowItWorks();
        #region Get ,Insert, Update
        public HowItWorksModel GetHowItWorksById(int id)
        {
            DataTable dt = objDLHowItWorks.GetById(id);
            HowItWorksModel objHowItWorksModel = new HowItWorksModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objHowItWorksModel = GetDataRowToEntity<HowItWorksModel>(dt.Rows[0]);
            }

            return objHowItWorksModel;
        }
        #endregion
    }
}
