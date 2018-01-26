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
    public class StepDocMappedController : ApiController
    {
        private BLStepDocMapped objBLStepDocMapped = new BLStepDocMapped();

        #region Get ,Insert, Update and delete StepDocMapped

        [HttpGet]
        public StepDocMappedModel GetStepDocMappedById(int StepDocMappedId)
        {
            return objBLStepDocMapped.GetStepDocMappedById(StepDocMappedId);
        }

        [HttpPost]
        public StepDocMappedModel InsertUpdateStepDocMapped(StepDocMappedModel objStepDocMappedModel)
        {
            return objBLStepDocMapped.InsertUpdateStepDocMapped(objStepDocMappedModel);
        }

        [HttpPost]
        public ViewStepDocMappedModel DeleteStepDocMapped(ViewStepDocMappedModel objViewStepDocMappedModel)
        {
            return objBLStepDocMapped.DeleteStepDocMapped(objViewStepDocMappedModel);
        }

        #endregion

        #region View  StepDocMapped
        [HttpGet]
        public ViewStepDocMappedModel GetStepDocMappedList(int StepID, int DocumentID)
        {
            return objBLStepDocMapped.GetStepDocMappedList(StepID, DocumentID);
        }
        #endregion


    }
}
