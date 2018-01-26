using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RepidShare.Entities.Resource;
using System.Configuration;

/*------------------------------------------------------------------------
Created By: Vashistha Sharma
Date: 09 Feb 2015
Description: Initial Creation for MultiLine Question Type
------------------------------------------------------------------------*/
namespace RepidShare.Entities
{
    public class MultiLineModel
    {
        [Required(ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valNoOfLines")]
        [Range(1, 20, ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valRanageNoOfLine")]
        //[DynamicRangeValidator("MultiLineNoOfLineMin", "MultiLineNoOfLineMax", ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valRanageNoOfLine")]
       
        public int? NoOfLines { get; set; }

        public int MultiLineNoOfLineMin
        {
            get
            {
                int _multiLineNoOfLineMin = 0;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MultiLineNoOfLineMin"]))
                    int.TryParse(ConfigurationManager.AppSettings["MultiLineNoOfLineMin"], out _multiLineNoOfLineMin);
                return _multiLineNoOfLineMin;
            }
        }
        public int MultiLineNoOfLineMax
        {
            get
            {
                int _multiLineNoOfLineMax = 20;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MultiLineNoOfLineMax"]))
                    int.TryParse(ConfigurationManager.AppSettings["MultiLineNoOfLineMax"], out _multiLineNoOfLineMax);
                return _multiLineNoOfLineMax;
            }
        }
    }
}
