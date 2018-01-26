using RepidShare.Data;
using RepidShare.Data.Email;
using RepidShare.Entities;
using RepidShare.Entities.Email;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Business.Email
{
    public class BLEmail : BLBase
    {
        DLEmail objDLEmail = new DLEmail();


        public List<DropdownModel> FillEmailTyepDropDown()
        {
            List<DropdownModel> lstEmailTye = objDLEmail.GetEmailTyepDropDownList().ToList();
            return lstEmailTye;
        }


        public ViewEmailTemplateModel GetEmailTemplateList(ViewEmailTemplateModel ViewEmailTemplateModel)
        {
            List<EmailTemplate> lstUserLogin = new List<EmailTemplate>();
            //if FilterSubCategoryName is NULL than set to empty
            // ViewEmailTemplateModel.FilterUserName = ViewEmailTemplateModel.FilterUserName ?? String.Empty;

            //if SortBy is NULL than set to empty
            ViewEmailTemplateModel.SortBy = ViewEmailTemplateModel.SortBy ?? String.Empty;

            //call GetSubCategoryList Method which will retrun datatable of  SubCategory
            DataTable dtEmailTemplate = objDLEmail.GetEmailTemplateList(ViewEmailTemplateModel);

            //fetch each row of datable
            foreach (DataRow dr in dtEmailTemplate.Rows)
            {
                //Convert datarow into Model object and set Model object property
                EmailTemplate itemEmailTemplate = GetDataRowToEntity<EmailTemplate>(dr);
                //Add  SubCategory in List
                lstUserLogin.Add(itemEmailTemplate);
            }

            //set SubCategory List of Model ViewSubCategoryModel
            ViewEmailTemplateModel.EmailTemplateList = lstUserLogin;
            //if  SubCategory List count is not null and greater than 0 Than set Total Pages for Paging.
            if (ViewEmailTemplateModel != null && ViewEmailTemplateModel.EmailTemplateList != null && ViewEmailTemplateModel.EmailTemplateList.Count > 0)
            {
                ViewEmailTemplateModel.CurrentPage = ViewEmailTemplateModel.EmailTemplateList[0].CurrentPage;
                int totalRecord = ViewEmailTemplateModel.EmailTemplateList[0].TotalCount;

                if (decimal.Remainder(totalRecord, ViewEmailTemplateModel.PageSize) > 0)
                    ViewEmailTemplateModel.TotalPages = (totalRecord / ViewEmailTemplateModel.PageSize + 1);
                else
                    ViewEmailTemplateModel.TotalPages = totalRecord / ViewEmailTemplateModel.PageSize;
            }
            else
            {
                ViewEmailTemplateModel.TotalPages = 0;
                ViewEmailTemplateModel.CurrentPage = 1;
            }
            return ViewEmailTemplateModel;
        }



        public EmailTemplate GetEmailTemplateById(int EmailDetailID)
        {
            //Call GetSubCategoryBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLEmail.GetEmailTemplateById(EmailDetailID);
            EmailTemplate objEmailTemplate = new EmailTemplate();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objEmailTemplate = GetDataRowToEntity<EmailTemplate>(dt.Rows[0]);
            }

            return objEmailTemplate;
        }




        public EmailTemplate InsertUpdateEmailDetail(EmailTemplate objEmailTemplate)
        {
            //call InsertUpdateSubCategory Method of dataLayer and return SubCategoryModel
            return objDLEmail.InsertUpdateEmailDetail(objEmailTemplate);
        }

          public ViewEmailTemplateModel DeleteEmailTemplate(ViewEmailTemplateModel objEmailTemplate)
        {
            //call InsertUpdateSubCategory Method of dataLayer and return SubCategoryModel
            return objDLEmail.DeleteEmailTemplate(objEmailTemplate);
        }
         public EmailTemplate GetEmailSettingByEmailID(int EmailID)
        {
            //call InsertUpdateSubCategory Method of dataLayer and return SubCategoryModel
            DataTable dt = objDLEmail.GetEmailSettingByEmailID(EmailID);
            EmailTemplate objEmailTemplate = new EmailTemplate();
            if (dt.Rows.Count > 0)
            {
                objEmailTemplate = GetDataRowToEntity<EmailTemplate>(dt.Rows[0]);
            }
            return objEmailTemplate;
        }

        
    }
}
