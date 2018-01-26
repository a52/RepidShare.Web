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
    public class StepController : ApiController
    {
        private BLStep objBLStep = new BLStep();

        #region Get ,Insert, Update and delete Step

        [HttpGet]
        public StepModel GetStepById(int StepId)
        {
            return objBLStep.GetStepById(StepId);
        }

        [HttpPost]
        public StepModel InsertUpdateStep(StepModel objStepModel)
        {
            return objBLStep.InsertUpdateStep(objStepModel);
        }

        [HttpPost]
        public ViewStepModel DeleteStep(ViewStepModel objViewStepModel)
        {
            return objBLStep.DeleteStep(objViewStepModel);
        }

        #endregion

        #region View  Step
        [HttpPost]
        public ViewStepModel GetStepList(ViewStepModel objViewStepModel)
        {
            return objBLStep.GetStepList(objViewStepModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillStepDropDown()
        {
            return objBLStep.FillStepDropDown();
        }

        [HttpGet]
        public List<DropdownModel> FillStepDropDownByDocumentId(int DocumentId)
        {
            return objBLStep.FillStepByDocumentId(DocumentId);
        }
        #endregion
    }
}
