using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RepidShare.Entities.Resource;


namespace RepidShare.Entities
{
   public class DateAndTimeModel
    {
       [Required(ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valAnswerOptions")]
       public bool? IsDateOnly { get; set; }
       [Required(ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valDefaultValue")]
       public string DefaultValueType { get; set; }
       [Required(ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valSpecificDate")]
       public DateTime? DateDefaultValue { get; set; }
    }
}
