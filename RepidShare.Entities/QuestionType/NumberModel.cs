using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;

/*------------------------------------------------------------------------
Created By: Vashistha Sharma
Date: 09 Feb 2015
Description: Initial Creation for Number Question Type
------------------------------------------------------------------------*/
namespace RepidShare.Entities
{
    public class NumberModel
    {
        [Required(ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valMinValue")]
        
        public int? MinValue { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valMaxValue")]
        
        public int? MaxValue { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valNoOfDecimal")]
        [Range(0, 3, ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valRanageNoOfDecimal")]
        
        public int? NoOfDecimal { get; set; }
    }
}
