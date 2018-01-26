using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class DocumentResultSummaryAnswerModel
    {
        public string AnswerDetail { get; set; }
        public int AnswerResponseCount { get; set; }
        public int DocumentQuestionID { get; set; }
        public double AnswerPercentage { get; set; }
    }
}
