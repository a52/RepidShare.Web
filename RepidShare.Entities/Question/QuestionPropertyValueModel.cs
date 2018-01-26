using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IIMSEntities
{
    public class QuestionPropertyValueModel
    {
        public int QuestionPropertyValueID { get; set; }
        public int QuestionID { get; set; }
        public int QuestionPropertyID { get; set; }
        public string PropertyValue { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
