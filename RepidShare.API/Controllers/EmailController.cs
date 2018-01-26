using RepidShare.Business;
using RepidShare.Business.Email;
using RepidShare.Entities;
using RepidShare.Entities.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RepidShare.API.Controllers
{
    public class EmailController : ApiController
    {
        //
        // GET: /Email/


        BLEmail objBusEmail = new BLEmail();
        //public ActionResult Index()
        //{
        //    return View();
        //}


        [HttpGet]
        public List<DropdownModel> FillEmailTyepDropDown()
        {
            return objBusEmail.FillEmailTyepDropDown();
        }


        [HttpPost]
        [ActionName("GetEmailTemplateList")]
        public ViewEmailTemplateModel GetEmailTemplateList(ViewEmailTemplateModel ViewEmailTemplateModel)
        {
            return objBusEmail.GetEmailTemplateList(ViewEmailTemplateModel);
        }

        [HttpGet]
        public EmailTemplate GetEmailTemplateById(int EmailDetailID)
        {
            return objBusEmail.GetEmailTemplateById(EmailDetailID);
        }


        [HttpPost]
        [ActionName("InsertUpdateEmailDetail")]
        public EmailTemplate InsertUpdateEmailDetail(EmailTemplate objEmailTemplate)
        {
            return objBusEmail.InsertUpdateEmailDetail(objEmailTemplate);
        }

        [HttpPost]
        [ActionName("DeleteEmailTemplate")]
        public ViewEmailTemplateModel DeleteEmailTemplate(ViewEmailTemplateModel objViewEmailTemplateModel)
        {
            return objBusEmail.DeleteEmailTemplate(objViewEmailTemplateModel);
            //return null;
        }
        [HttpGet]
        public EmailTemplate GetEmailSettingByEmailID(int EmailID)
        {
            return objBusEmail.GetEmailSettingByEmailID(EmailID);
        }

      //  GetEmailSettingByEmailID
    }
}
