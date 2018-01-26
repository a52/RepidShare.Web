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
    public class GroupController : ApiController
    {
        private BLGroup objBLGroup = new BLGroup();

        #region Get ,Insert, Update and delete Group

        [HttpGet]
        public GroupModel GetGroupById(int GroupId)
        {
            return objBLGroup.GetGroupById(GroupId);
        }

        [HttpPost]
        public GroupModel InsertUpdateGroup(GroupModel objGroupModel)
        {
            return objBLGroup.InsertUpdateGroup(objGroupModel);
        }

        [HttpPost]
        public ViewGroupModel DeleteGroup(ViewGroupModel objViewGroupModel)
        {
            return objBLGroup.DeleteGroup(objViewGroupModel);
        }

        #endregion

        #region View  Group
        [HttpPost]
        public ViewGroupModel GetGroupList(ViewGroupModel objViewGroupModel)
        {
            return objBLGroup.GetGroupList(objViewGroupModel);
        }
        #endregion

        #region  Caegory DropDown
        [HttpGet]
        public List<DropdownModel> FillGroupDropDown()
        {
            return objBLGroup.FillGroupDropDown();
        }
        #endregion
    }
}
