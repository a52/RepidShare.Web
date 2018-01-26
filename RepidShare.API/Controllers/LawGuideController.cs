using RepidShare.Business;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;


namespace RepidShare.API.Controllers
{
    public class LawGuideController : ApiController
    {
        private BLLawGuide objBLLawGuide = new BLLawGuide();

        #region Get ,Insert, Update and delete LawGuide

        [HttpGet]
        public LawGuideModel GetLawGuideById(int LawGuideId)
        {
            return objBLLawGuide.GetLawGuideById(LawGuideId);
        }

        [HttpPost]
        [ActionName("InsertUpdateLawGuide")]
        public LawGuideModel InsertUpdateLawGuide(LawGuideModel objLawGuideModel)
        {
            return objBLLawGuide.InsertUpdateLawGuide(objLawGuideModel);
        }

        [HttpPost]
        [ActionName("DeleteLawGuide")]
        public ViewLawGuideModel DeleteLawGuide(ViewLawGuideModel objViewLawGuideModel)
        {
            return objBLLawGuide.DeleteLawGuide(objViewLawGuideModel);
        }

        #endregion

        #region View  LawGuide
        [HttpPost]
        [ActionName("GetLawGuideList")]
        public ViewLawGuideModel GetLawGuideList(ViewLawGuideModel objViewLawGuideModel)
        {
            return objBLLawGuide.GetLawGuideList(objViewLawGuideModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillLawGuideDropDown(int? CategoryID, int? GroupID)
        {
            return objBLLawGuide.FillLawGuideDropDown(CategoryID, GroupID);
        }
        #endregion
    }
}
