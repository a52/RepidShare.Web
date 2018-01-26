using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RepidShare.Entities.Resource;

namespace RepidShare.Entities
{
    public class AttachmentModel : BaseModel
    {
        public int AttachmentID { get; set; }
        public string ModuleName { get; set; }
        [LocalizedDisplayName(typeof(CommonResource), "lblAttachmentName")]
        public string AttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public Decimal AttachmentSize { get; set; }
        public byte[] AttachmentContent { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
    }
}
