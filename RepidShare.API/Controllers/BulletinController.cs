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
    public class BulletinController : ApiController
    {
        private BLBulletin objBLBulletin = new BLBulletin();

        #region Get ,Insert, Update and delete Bulletin

        [HttpGet]
        public BulletinModel GetBulletinById(int BulletinId)
        {
            
            return objBLBulletin.GetBulletinById(BulletinId);
        }

        [HttpPost]
        public BulletinModel InsertUpdateBulletin(BulletinModel objBulletinModel)
        {
            return objBLBulletin.InsertUpdateBulletin(objBulletinModel);
        }

        [HttpPost]
        public ViewBulletinModel DeleteBulletin(ViewBulletinModel objViewBulletinModel)
        {
            return objBLBulletin.DeleteBulletin(objViewBulletinModel);
        }

        #endregion

        #region View  Bulletin
        [HttpPost]
        public ViewBulletinModel GetBulletinList(ViewBulletinModel objViewBulletinModel)
        {
            return objBLBulletin.GetBulletinList(objViewBulletinModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillCaegoryDropDown()
        {
            return objBLBulletin.FillCaegoryDropDown();
        }


        [HttpGet]
        public void UpdateBulletinStatusByID(int BulletinId, int status)
        {

            objBLBulletin.UpdateBulletinStatusByID(BulletinId, status);
        }
        #endregion
    }
}
