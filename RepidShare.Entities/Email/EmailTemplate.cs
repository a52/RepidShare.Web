using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Entities
{
   public  class EmailTemplate : BaseModel
    {
        public int EmailDetailID { get; set; }
        [Required]
        public int EmailID { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        public string Content { get; set; }
        [Required]
        public string EmailTitle { get; set; }
        public int CurrentPage { get; set; }
    }

    public class ViewEmailTemplateModel :ViewParameters
    {
        public List<EmailTemplate> EmailTemplateList { get; set; }
        public int DeletedEmailDetailID { get; set; }
        public int DeletedBy { get; set; }
    }
}
