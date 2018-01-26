using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RepidShare.Entities
{
    public class QuestionPropertyModel:BaseModel
    {
        public int QuestionPropertyID { get; set; }
        public int QuestionTypeID { get; set; }
        public string PropertyText { get; set; }
        public string PropertyValue { get; set; }
        public DateTime? DateDefaultValue { get; set; }
    }
}
