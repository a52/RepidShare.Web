using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RepidShare.Entities
{
    public class DocumentResultDetailModel
    {
        public long DocumentResultDetailID { get; set; }
        public long DocumentResultID { get; set; }
        public int DocumentQuestionID { get; set; }
        public int DocumentQuestionOptionsID { get; set; }
        public string AnswerDetail { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
       
        public int ResultOptionID { get; set; }
    }
}
