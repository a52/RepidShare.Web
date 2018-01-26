using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class StepModel : BaseModel
    {
        public int StepID { get; set; }
        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Step Name is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblStepName")]
        public string StepName { get; set; }
        [Required(ErrorMessage = "Step Discription is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblStepDiscription")]
        public string StepDiscription { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public bool IsComplete { get; set; }
        public int stepStatus { get; set; }
    }
    public class ViewStepModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(CategoryResource), "CategoryName")]
        public List<StepModel> StepList { get; set; }
        public int DeletedStepID { get; set; }
        public int DeletedBy { get; set; }
    }
}
