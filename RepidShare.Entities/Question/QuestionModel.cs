using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class QuestionModel : BaseModel
    {
        public int QuestionID { get; set; }
        public int StepDocMappedID { get; set; }
        [RegularExpression(RegularExpressionResourceKeys.QuestionAnswerSpecialCharacterPattern)]
        [Required(ErrorMessageResourceType = typeof(QuestionResource), ErrorMessageResourceName = "valQuestionDescription")]
        [LocalizedDisplayName(typeof(QuestionResource), "QuestionDescription")]
        public string QuestionDescription { get; set; }

        [LocalizedDisplayName(typeof(QuestionResource), "lblParentQuestion")]
        public string ParentQuestion { get; set; }

        [LocalizedDisplayName(typeof(QuestionResource), "lblParentAnswer")]
        public string ParentAnswer { get; set; }

        [LocalizedDisplayName(typeof(QuestionResource), "lblQuestionHint")]
        public string QuestionHint { get; set; }

        [Required(ErrorMessageResourceType = typeof(QuestionResource), ErrorMessageResourceName = "lblExplanation")]
        [LocalizedDisplayName(typeof(QuestionResource), "lblExplanation")]
        public string Explanation { get; set; }

        public int QuestionTypeID { get; set; }

        [LocalizedDisplayName(typeof(QuestionResource), "RequireResponseToQuestion")]
        public bool IsRequireResponse { get; set; }

        [LocalizedDisplayName(typeof(QuestionResource), "lblDisplayOrder")]
        public int DisplayOrder { get; set; }

        public int DocumentID { get; set; }

        [Required(ErrorMessage = "Step is required.")]
        [LocalizedDisplayName(typeof(CommonResource), "lblStepName")]
        public int StepID { get; set; }

        public string ParentDDLText { get; set; }
    }
}
