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
    public class CoupenController : ApiController
    {
        private BLCoupen objBLCoupen = new BLCoupen();

        #region Get ,Insert, Update and delete Coupen

        [HttpGet]
        public CoupenModel GetCoupenById(int CoupenId)
        {
            return objBLCoupen.GetCoupenById(CoupenId);
        }

        [HttpPost]
        public CoupenModel InsertUpdateCoupen(CoupenModel objCoupenModel)
        {
            return objBLCoupen.InsertUpdateCoupen(objCoupenModel);
        }

        [HttpPost]
        public ViewCoupenModel DeleteCoupen(ViewCoupenModel objViewCoupenModel)
        {
            return objBLCoupen.DeleteCoupen(objViewCoupenModel);
        }

        #endregion

        #region View  Coupen
        [HttpPost]
        public ViewCoupenModel GetCoupenList(ViewCoupenModel objViewCoupenModel)
        {
            return objBLCoupen.GetCoupenList(objViewCoupenModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillCoupenDropDown()
        {
            return objBLCoupen.FillCoupenDropDown();
        }
        #endregion
    }
}
