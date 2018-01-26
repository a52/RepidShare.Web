using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class QuestionDetailModel : QuestionModel
    {
        [LocalizedDisplayName(typeof(QuestionResource), "lblQuestionType")]
        [Required(ErrorMessageResourceType = typeof(QuestionResource), ErrorMessageResourceName = "valQuestionType")]
        public string QuestionType { get; set; }

        public string QuestionTypeName { get; set; }
        public List<QuestionOptionsModel> QuestionOptionsList { get; set; }
        public int ID { get; set; }
        public int DisplayChoiceID { get; set; }
        public bool IsPublish { get; set; }
        public String DropDownXML { get; set; }
    }
}
