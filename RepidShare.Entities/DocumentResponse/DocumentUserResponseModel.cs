
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class DocumentUserResponseModel : UserLogin
    {
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int NoOfAttempt { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
