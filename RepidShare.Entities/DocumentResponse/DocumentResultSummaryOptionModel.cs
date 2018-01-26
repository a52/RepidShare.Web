using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class DocumentResultSummaryOptionModel
    {
        public int DocumentQuestionID { get; set; }
        public int DocumentQuestionOptionsID { get; set; }
        public string OptionText { get; set; }
        public int questionOptionOrder { get; set; }
        public int OptionResponseCount { get; set; }

        public double OptionPercentage { get; set; }

        public int MultiOptionResponseCount { get; set; }
        public string AnswerOptions { get; set; }
        public int MultiSelectOrder { get; set; }
    }
}
