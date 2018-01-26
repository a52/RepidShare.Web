using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Entities
{
    public class PriceQuestionModel
    {
        public PriceQuestionModel()
        {
            dtPriceDetails = new List<PriceDetailsQuestionModel>();
         
        }
        public long ResultDetailId { get; set; }
        public int QuestionId { get; set; }
        public decimal PrintAmt { get; set; }
        public string PriceAmtText { get; set; }
        public decimal TaxAmt { get; set; }
        public string TaxAmtText { get; set; }
        public decimal TotalAmt { get; set; }
        public string TotalText { get; set; }
        public string TaxTypeValue { get; set; }
        public string InstNoValue { get; set; }
       
        public string TableInnerHTML { get; set; }
        public string DivInnerHTML { get; set; }
        public string FixedAmt { get; set; }

        public List<PriceDetailsQuestionModel> dtPriceDetails { get; set; }
    }

    public class PriceDetailsQuestionModel
    {
        public long ResultDetailId { get; set; }
        public int QuestionId { get; set; }
        public decimal InstAmt { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal IneteAmt { get; set; }
        public decimal TotalAmt { get; set; }
        public string DateAmt { get; set; }
        public string PenaltyAmt { get; set; }
    }
}
