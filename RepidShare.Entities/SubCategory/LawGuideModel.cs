using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;

namespace RepidShare.Entities
{
    public class LawGuideModel : BaseModel
    {
        public int LawGuideID { get; set; }
        [Required(ErrorMessage = "Please select Category.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblCategoryName")]
        public int SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }

        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Law Guide Name is Required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblLawGuideName")]
        public string LawGuideName { get; set; }

        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Description is Required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblSubDescription")]
        public string Description { get; set; }

        public int Count { get; set; }
        public int CurrentPage { get; set; }

    }
    public class ViewLawGuideModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(CategoryResource), "CategoryName")]
        public List<LawGuideModel> LawGuideList { get; set; }
        public int DeletedLawGuideID { get; set; }
        public int DeletedBy { get; set; }
    }
}
