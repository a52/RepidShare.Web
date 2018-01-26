using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RepidShare.Entities
{
    public class NotificationModel
    {
        public long NotificationID { get; set; }
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string FromModule { get; set; }
        public long FromID { get; set; }
        public int Priority { get; set; }
        public bool IsSent { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
