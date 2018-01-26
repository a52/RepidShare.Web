
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class ViewQuestionAnswerModel : QuestionDetailModel
    {
        public ViewQuestionAnswerModel()
        {
            objPriceQuestionModel = new PriceQuestionModel();
        }
        public long ResultDetailID { get; set; }
        public long ResultID { get; set; }
        public string AnswerDetail { get; set; }
        //public List<ResultDetailModel> Answers { get; set; }
        public List<QuestionPropertyModel> QuestionPropertyList { get; set; }
        public QuestionTypeDetailModel QuestionTypeDetail { get; set; }
        //  public bool IsView { get; set; }
        public string SelectedAnswers { get; set; }
        public PriceQuestionModel objPriceQuestionModel { get; set; }
        public string HTML { get; set; }
    }
}
