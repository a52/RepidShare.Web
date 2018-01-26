using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RepidShare.Entities
{
    public class QuestionTypeDetailModel
    {
        public SingleLineModel SingleLineTextType { get; set; }
        public MultiLineModel MultiLineTextType { get; set; }
        public DateAndTimeModel DateAndTimeType { get; set; }
        public NumberModel NumberType { get; set; }
        public SingleSelectModel SingleSelect { get; set; }
        public MultiSelectModel MultiSelect { get; set; }
        public SingleLineModel DropDownType { get; set; }
        public SingleLineModel PriceQuestionType { get; set; }
    }
}
