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
    public class MasterController : ApiController
    {
        private BLMaster objBLMaster = new BLMaster();

        #region Get ,Insert, Update and delete Master

        [HttpGet]
        public MasterModel GetMasterById(int MasterId)
        {
            return objBLMaster.GetMasterById(MasterId);
        }

        [HttpPost]
        public MasterModel InsertUpdateMaster(MasterModel objMasterModel)
        {
            return objBLMaster.InsertUpdateMaster(objMasterModel);
        }

        [HttpPost]
        public ViewMasterModel DeleteMaster(ViewMasterModel objViewMasterModel)
        {
            return objBLMaster.DeleteMaster(objViewMasterModel);
        }

        #endregion

        #region View  Master
        [HttpPost]
        public ViewMasterModel GetMasterList(ViewMasterModel objViewMasterModel)
        {
            return objBLMaster.GetMasterList(objViewMasterModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillMasterDropDown()
        {
            return objBLMaster.FillMasterDropDown();
        }
        #endregion
    }
}
