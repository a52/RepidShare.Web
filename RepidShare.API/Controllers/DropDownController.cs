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
    public class DropDownController : ApiController
    {
        private BLDropDown objBLDropDown = new BLDropDown();

        #region Get ,Insert, Update and delete DropDown

        [HttpGet]
        public DropDownModel GetDropDownById(int DropDownId)
        {
            
            return objBLDropDown.GetDropDownById(DropDownId);
        }

        [HttpPost]
        public DropDownModel InsertUpdateDropDown(DropDownModel objDropDownModel)
        {
            return objBLDropDown.InsertUpdateDropDown(objDropDownModel);
        }

        [HttpPost]
        public ViewDropDownModel DeleteDropDown(ViewDropDownModel objViewDropDownModel)
        {
            return objBLDropDown.DeleteDropDown(objViewDropDownModel);
        }

        #endregion

        #region View  DropDown
        [HttpPost]
        public ViewDropDownModel GetDropDownList(ViewDropDownModel objViewDropDownModel)
        {
            return objBLDropDown.GetDropDownList(objViewDropDownModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillDropDown()
        {
            return objBLDropDown.FillCaegoryDropDown();
        }
        #endregion
    }
}
