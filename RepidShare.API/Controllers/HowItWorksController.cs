using RepidShare.Business.HowItWorks;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RepidShare.API.Controllers
{
    
    public class HowItWorksController : ApiController
    {
        private BLHowItWorks objBLHowItWorks = new BLHowItWorks();
        #region Get ,Insert, Update and delete Master

        [HttpGet]
        public HowItWorksModel GetById(int id)
        {
            return objBLHowItWorks.GetHowItWorksById(id);
        }
        #endregion
    }
}
