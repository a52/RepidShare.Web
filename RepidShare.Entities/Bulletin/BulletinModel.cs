using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class BulletinModel : BaseModel
    {
        public int BulletinID { get; set; }

        [RegularExpression(RegularExpressionResourceKeys.SpecialCharacterPattern, ErrorMessageResourceName = "valSpecialChar", ErrorMessageResourceType = typeof(CommonResource))]
        [Required(ErrorMessage = "Bulletin Name is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblBulletinName")]
        public string BulletinName { get; set; }

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
        //narayan
        // [Required(ErrorMessage = "ClassName is required.")]
        public string ClassName { get; set; }
        public bool Activate { get; set; }


    }
    public class ViewBulletinModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(BulletinResource), "BulletinName")]
        public List<BulletinModel> BulletinList { get; set; }
        public int DeletedBulletinID { get; set; }
        public int DeletedBy { get; set; }
    }
}
