using RepidShare.Business;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RepidShare.API.Controllers
{
    public class MasterSettingController : ApiController
    {
        private BLMasterSetting objBLMasterSetting = new BLMasterSetting();

        #region Get ,Insert, Update and delete MasterSetting

        [HttpGet]
        public MasterSettingModel GetMasterSettingById(int MasterSettingId)
        {
            return objBLMasterSetting.GetMasterSettingById(MasterSettingId);
        }

        [HttpPost]
        public MasterSettingModel InsertUpdateMasterSetting(MasterSettingModel objMasterSettingModel)
        {
            return objBLMasterSetting.InsertUpdateMasterSetting(objMasterSettingModel);
        }
        #endregion

         
    }
}
