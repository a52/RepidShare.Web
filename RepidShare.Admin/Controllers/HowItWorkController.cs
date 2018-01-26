using RepidShare.Entities;
using RepidShare.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RepidShare.Admin.Controllers
{
    public class HowItWorkController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();

        #region Add Edit How It Work
        /// <summary>
        /// Add Edit How It Work
        /// </summary>
        /// <param name="prm"></param>
        /// <returns></returns>

        [Filters.Authorized]
        public ActionResult Index(string prm = "")
        {
            HowItWorksModel model = new HowItWorksModel();

            if (!string.IsNullOrEmpty(prm))
            {
                int id;
                int.TryParse(CommonUtils.Decrypt(prm), out id);
                serviceResponse = objUtilityWeb.GetAsync(WebApiURL.HowItWorks + "/GetById?id=" + id.ToString());
                model = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<HowItWorksModel>().Result : null;
            }
            return View();
        }
        #endregion
    }
}
