using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RepidShare.Entities
{
    public class ViewQuestionModel : ViewParameters
    {
        public QuestionDetailModel QuestionDetail { get; set; }
        public List<QuestionPropertyModel> QuestionPropertyList { get; set; }
        public QuestionTypeDetailModel QuestionTypeDetail { get; set; }
        public List<QuestionDetailModel> QuestionsList { get; set; }
        public string QuestionProperties { get; set; }
        public string QuestionOptions { get; set; }
        public int DeletedQuestionID { get; set; }
        public int DeletedBy { get; set; }
    }
}
