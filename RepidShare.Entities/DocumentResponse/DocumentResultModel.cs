using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class DocumentResultModel
    {
        public long DocumentResultID { get; set; }
        public long DocumentUserMappingID { get; set; }
        public int NoOfAttempt { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long DocumentTargetAudienceID { get; set; }        
    }
}
