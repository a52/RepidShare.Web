using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class DropDownModel : BaseModel
    {
        public int QuestionDropDownID { get; set; }

        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Drop Down Text/Value is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblDropDownText")]
        public string DropDownText { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }


    }
    public class ViewDropDownModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(DropDownResource), "DropDownName")]
        public List<DropDownModel> DropDownList { get; set; }
        public int DeletedDropDownID { get; set; }
        public int DeletedBy { get; set; }
    }
}
