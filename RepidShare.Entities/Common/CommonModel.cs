using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class CommonModel
    {

    }
    public class EmailFormModel
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Message { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
    }
    public class BaseModel
    {
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public int TotalCount { get; set; }
        public Int64 RowNumber { get; set; }

    }
    public class ViewParameters
    {

        //Action
        public String ActionType { get; set; }
        //Sorting
        public String SortBy { get; set; }
        public int SortOrder { get; set; }
        //Paging
        public string PagingPrefix { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public Int64 RowNumber { get; set; }
        //Searching
        [Display(Name = "Application")]
        public string FilterAppID { get; set; }
        public Int32 CommonCreatedBy { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public int ErrorCode { get; set; }

        [Display(Name = "From Expiry Date")]
        public DateTime? ExpireFromDate { get; set; }
        [Display(Name = "To Expiry Date")]
        public DateTime? ExpireEndDate { get; set; }

        [Display(Name = "From Create Date")]
        public DateTime? CreateFromDate { get; set; }
        [Display(Name = "To Create Date")]
        public DateTime? CreateEndDate { get; set; }

        [Display(Name = "From Submitted Date")]
        public DateTime? SubmittedFromDate { get; set; }
        [Display(Name = "To Submitted Date")]
        public DateTime? SubmittedEndDate { get; set; }

        [Display(Name = "From Start Date")]
        public DateTime? StartFromDate { get; set; }
        [Display(Name = "To Start Date")]
        public DateTime? StartEndDate { get; set; }

        //Searching User 
        [Display(Name = "First Name:")]
        public String FilterFirstName { get; set; }
        [Display(Name = "Last Name:")]
        public String FilterLastName { get; set; }
        [Display(Name = "Domain Name:")]
        public String FilterDomainName { get; set; }
        public Boolean IsReadOnly { get; set; }
        [Display(Name = "Menu Name")]
        public String FilterMenuName { get; set; }
        [Display(Name = "Level Name:")]
        public String FilterLevelName { get; set; }
        [Display(Name = "Level Code:")]
        public String FilterLevelCode { get; set; }
        [Display(Name = "Team Name:")]
        public String FilterTeamName { get; set; }
        [Display(Name = "Role Name")]
        public String FilterRoleName { get; set; }
        public string PageName { get; set; }
        [Display(Name = "Status")]
        public String FilterStatus { get; set; }

        #region Filter
        [Display(Name = " Name")]
        public String FilterName { get; set; }
        [Display(Name = "Description :")]
        public String FilterDescription { get; set; }
        [Display(Name = "IsAllow Multiple Responses :")]
        public Boolean FilterIsAllowMultipleResponses { get; set; }
        [Display(Name = "Show  Result To User :")]
        public Boolean FilterShowResultToUser { get; set; }


        [Display(Name = "Category Name")]
        public String FilterCategoryName { get; set; }
        [Display(Name = "Sub Category Name")]
        public String FilterSubCatName { get; set; }
        [Display(Name = "Group Name")]
        public String FilterGroupText { get; set; }
        [Display(Name = "Step Name")]
        public String FilterStepName { get; set; }

        [Display(Name = "Category")]
        public int FilterCategoryId { get; set; }
        [Display(Name = "Sub Category")]
        public int FilterSubCatId { get; set; }
        [Display(Name = "Group")]
        public int FilterGroupID { get; set; }

        [Display(Name = "DropDown Text/Value")]
        public string FilterDropDownName { get; set; }

        [Display(Name = "Document Title")]
        public string FilterDocumentTitle { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? FilterExpireDate { get; set; }

        [Display(Name = "User Status")]
        public int CompletedStatusId { get; set; }

        [Display(Name = " Status")]
        public int StatusId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Master Value")]
        public string FilterMasterText { get; set; }

        [Display(Name = "User Name")]
        public String FilterUserName { get; set; }

        [Display(Name = "Filter Coupen Code")]
        public String FilterCoupenText { get; set; }

        [Display(Name = "Bulletin")]
        public String FilterBulletinName { get; set; }
        #endregion
    }

    public class DropdownModel
    {
        public int ID { get; set; }
        public string Value { get; set; }
    }

    public class DropdownStringModel
    {
        public string ID { get; set; }
        public string Value { get; set; }
    }


}
