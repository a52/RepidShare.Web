using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;
using System.Configuration;



namespace RepidShare.Entities
{
    public class SingleLineModel
    {
        [Required(ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valMaxNumberChar")]
        [Range(1, 500, ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valRanageMaxNumberChar")]
        //[DynamicRangeValidator("SingleLineMaxCharMin", "SingleLineMaxCharMax",ErrorMessageResourceType = typeof(QuestionTypeResource), ErrorMessageResourceName = "valRanageMaxNumberChar")]

        public int? MaxChar { get; set; }

        public string DropDownText { get; set; }

        public string DropDownValue { get; set; }

        public int SingleLineMaxCharMin
        {
            get
            {
                int _singleLineMaxCharMin = 0;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SingleLineMaxCharMin"]))
                    int.TryParse(ConfigurationManager.AppSettings["SingleLineMaxCharMin"], out _singleLineMaxCharMin);
                return _singleLineMaxCharMin;
            }
        }
        public int SingleLineMaxCharMax
        {
            get
            {
                int _singleLineMaxCharMax = 500;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SingleLineMaxCharMax"]))
                    int.TryParse(ConfigurationManager.AppSettings["SingleLineMaxCharMax"], out _singleLineMaxCharMax);
                return _singleLineMaxCharMax;
            }
        }
    }


    
}