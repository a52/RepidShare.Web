using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class ViewQuestionAnswerModel : QuestionDetailModel
    {
        public long DocumentResultDetailID { get; set; }
        public long DocumentResultID { get; set; }
        public string AnswerDetail { get; set; }
        //public List<DocumentResultDetailModel> Answers { get; set; }
        public List<QuestionPropertyModel> QuestionPropertyList { get; set; }
        public QuestionTypeDetailModel QuestionTypeDetail { get; set; }
      //  public bool IsView { get; set; }
        public string SelectedAnswers { get; set; }

    }
}
