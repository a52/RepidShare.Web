using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class MasterModel : BaseModel
    {
        public int MasterID { get; set; }
        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Master Value  is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblMasterText")]
        public string MasterValue { get; set; }

        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Description is Required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblSubDescription")]
        public string Description { get; set; }

        public int Count { get; set; }
        public int CurrentPage { get; set; }
    }
    public class ViewMasterModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(MasterResource), "GroupName")]
        public List<MasterModel> MasterList { get; set; }
        public int DeletedMasterID { get; set; }
        public int DeletedBy { get; set; }
    }
}
