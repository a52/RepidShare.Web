using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class DocumentResultQuestionModel : BaseModel
    {
        public int DocumentResultQuestionId { get; set; }
        public string QuestionDescription { get; set; }
        public string QuestionResult { get; set; }
    }
}
