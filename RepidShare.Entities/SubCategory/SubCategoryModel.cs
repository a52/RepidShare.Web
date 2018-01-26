using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class SubCategoryModel : BaseModel
    {
        public int SubCategoryID { get; set; }
        [Required(ErrorMessage = "Please select Category.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblCategoryName")]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        
        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Sub Category Name is Required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblSubCategoryName")]
        public string SubCatName { get; set; }

        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Description is Required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblSubDescription")]
        public string Description { get; set; }

        public int Count { get; set; }
        public int CurrentPage { get; set; }

        public int AttachmentID { get; set; }
        public string ModuleName { get; set; }
        [LocalizedDisplayName(typeof(CommonResource), "lblAttachmentName")]
        public string AttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public Decimal? AttachmentSize { get; set; }
        public byte[] AttachmentContent { get; set; }
        // Narayan
        [Required(ErrorMessage = "ClassName is required.")]
        public string ClassName { get; set; }
        public bool Activate { get; set; }

    }
    public class ViewSubCategoryModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(CategoryResource), "CategoryName")]
        public List<SubCategoryModel> SubCategoryList { get; set; }
        public int DeletedSubCategoryID { get; set; }
        public int DeletedBy { get; set; }
    }
}
