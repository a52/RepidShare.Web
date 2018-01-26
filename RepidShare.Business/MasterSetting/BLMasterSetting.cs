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
    public class BLMasterSetting : BLBase
    {
        private DLMasterSetting objDLMasterSetting = new DLMasterSetting();
        #region Get ,Insert, Update and delete MasterSetting
        /// <summary>
        /// Get MasterSetting By Id
        /// </summary>
        /// <param name="MasterSettingId"></param>
        /// <returns>MasterSetting Model</returns>
        public MasterSettingModel GetMasterSettingById(int MasterSettingId)
        {
            //Call GetMasterSettingBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLMasterSetting.GetMasterSettingById(MasterSettingId);
            MasterSettingModel objMasterSettingModel = new MasterSettingModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objMasterSettingModel = GetDataRowToEntity<MasterSettingModel>(dt.Rows[0]);
            }

            return objMasterSettingModel;
        }

        /// <summary>
        /// Insert or Update  MasterSetting
        /// </summary>
        /// <param name="objMasterSettingModel">object of  MasterSetting Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public MasterSettingModel InsertUpdateMasterSetting(MasterSettingModel objMasterSettingModel)
        {
            //call InsertUpdateMasterSetting Method of dataLayer and return MasterSettingModel
            return objDLMasterSetting.InsertUpdateMasterSetting(objMasterSettingModel);
        }
         
        #endregion
 
    }
}
