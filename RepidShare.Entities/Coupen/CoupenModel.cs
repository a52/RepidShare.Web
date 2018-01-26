using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class CoupenModel : BaseModel
    {
        public int CoupenID { get; set; }
        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Coupen Value  is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblCoupenText")]
        public string CoupenCode { get; set; }

        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Description is Required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblSubDescription")]
        public string OffValue { get; set; }

        public int Count { get; set; }
        public int CurrentPage { get; set; }
    }
    public class ViewCoupenModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(CoupenResource), "GroupName")]
        public List<CoupenModel> CoupenList { get; set; }
        public int DeletedCoupenID { get; set; }
        public int DeletedBy { get; set; }
    }
}
