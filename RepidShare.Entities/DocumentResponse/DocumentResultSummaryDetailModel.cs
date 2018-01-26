using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class DocumentResultSummaryDetailModel :BaseModel
    {
        public int DocumentQuestionID { get; set; }
        public string QuestionDescription { get; set; }
        public int QuestionTypeID { get; set; }
        public string QuestionType { get; set; }
        public int QuestionDisplayOrder { get; set; }
        public int TotalAnswerCount { get; set; }
        public int TotalOptionCount { get; set; }
        public int TotalMultiOptionCount { get; set; }
        public List<DocumentResultSummaryAnswerModel> ResultAnswer { get; set; }
        public List<DocumentResultSummaryAnswerModel> ResultOption { get; set; }
    }

  
}
