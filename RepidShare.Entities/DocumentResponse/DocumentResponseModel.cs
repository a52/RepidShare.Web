
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class DocumentResponseModel : DocumentModel
    {
        public string ApplicationName { get; set; }
        public int InProgresUser { get; set; }
        public int CompletedUser { get; set; }
        public int NotAttemptUser { get; set; }
        public int TotalUser { get; set; }

        public DateTime? PublishDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int NoOfAttempt { get; set; }
        public bool? IsCompleted { get; set; }
        public bool IsExpire { get; set; }
        public bool IsPOPUP { get; set; }
    }
}
