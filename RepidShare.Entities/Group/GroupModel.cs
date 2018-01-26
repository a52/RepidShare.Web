using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class GroupModel : BaseModel
    {
        public int GroupID { get; set; }
        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Group Text  is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblGroupText")]
        public string GroupText { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
    }
    public class ViewGroupModel : ViewParameters
    {
     //   [LocalizedDisplayName(typeof(GroupResource), "GroupName")]
        public List<GroupModel> GroupList { get; set; }
        public int DeletedGroupID { get; set; }
        public int DeletedBy { get; set; }
    }

    public class MasterSettingModel : BaseModel
    {
        public int MasterSettingID { get; set; }
        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Master Setting is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblVatTax")]
        public string VatTax { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
    }
}
