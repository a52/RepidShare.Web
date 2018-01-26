using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RepidShare.Entities
{
    public class UserLogin : BaseModel
    {
        public int UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public int RoleID { get; set; }
        public DateTime? LasstLoginDate { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int Age { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string BusinessType { get; set; }
        public int CurrentPage { get; set; }

        public MasterSettingModel objMasterSetting { get; set; }
    }

    public class ViewUserLoginModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(CategoryResource), "CategoryName")]
        public List<UserLogin> UserLoginList { get; set; }
    }

    public class ChangePasswordModel
    {

        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
        public Int64 UserID { get; set; }

        public string Message { get; set; }
        public string MessageType { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9-_]+((?!.*\.\.)[a-zA-Z0-9\.\-_]*)[a-zA-Z0-9]+@[a-zA-Z0-9]+(\.[a-zA-Z]{2,3})?(\.[a-zA-Z]{2,3})$", ErrorMessage = "Please Type Valid Email Id")]
        [Required]
        [Display(Name = "Email")]
        public string EmailID { get; set; }

        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }

    }

    public class Register : BaseModel
    {
      
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select account type.")]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "Please select your title.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please select your gender.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please enter your date of birth.")]
        public string Dob_day { get; set; }
        public string Dob_month { get; set; }
        public string Dob_year { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Region { get; set; }
        [Required(ErrorMessage = "Please select a location.")]
        public string Country { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string DaytimeContactNumber { get; set; }
        public string MobileTelephoneNumber { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter a password.")]
        [MinLength(6), MaxLength(15)]
        public string Password { get; set; }
        [Compare("Password")]
        [MinLength(6), MaxLength(15)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "* I have read and agree to the terms of use")]
        [Range(typeof(bool), "false", "false", ErrorMessage = "This field is required.")]
        public bool TermsAndConditions { get; set; }

    }

}

