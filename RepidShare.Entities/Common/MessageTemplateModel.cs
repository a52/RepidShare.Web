using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RepidShare.Entities
{
    public class MessageTemplateModel
    {
        public int MessageTemplateID { get; set; }
        public string MessageName { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
