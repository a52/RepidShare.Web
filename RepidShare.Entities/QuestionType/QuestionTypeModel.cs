using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RepidShare.Entities
{
    public class QuestionTypeModel:BaseModel
    {
        public int QuestionTypeID { get; set; }
        public string QuestionTypeName { get; set; }
        public string QuestionType { get; set; }
    }
}
