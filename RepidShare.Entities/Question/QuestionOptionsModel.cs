using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RepidShare.Entities.Resource;
using RepidShare.Entities;


namespace RepidShare.Entities
{
    public class QuestionOptionsModel
    {
        public int QuestionOptionsID { get; set; }
        public int QuestionID { get; set; }
        [RegularExpression(RegularExpressionResourceKeys.QuestionAnswerSpecialCharacterPattern)]
        [LocalizedDisplayName(typeof(QuestionResource), "lblOptionText")]
        public string OptionText { get; set; }
        public int DisplayChoiceID { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsSelected { get; set; }
        public long ResultDetailID { get; set; }
    }
}
