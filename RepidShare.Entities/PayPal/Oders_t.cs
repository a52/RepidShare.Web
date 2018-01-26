using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Entities
{
    public class OdersModel : BaseModel
    {
        public int CurrentPage { get; set; }

        public int OrderID { get; set; }

        public int UserId { get; set; }

        public int PaymentID { get; set; }

        public DateTime OrderDate { get; set; }

        public string RefTimestamp { get; set; }

        public string TransactStatus { get; set; }

        public bool Deleted { get; set; }

        public decimal PaidTotal { get; set; }

        public string ErrorMsg { get; set; }

        public List<OdersDetailModel> OdersDetailModelList { get; set; }
    }

    public class OdersDetailModel
    {
        public string DocumentTitle { get; set; }

        public Int64 RowNumber { get; set; }

        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int DocumentID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal VatTax { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }

    public class ViewOdersDetailModel : ViewParameters
    {
        public List<OdersDetailModel> OdersDetailList { get; set; }
        public int OdersDetailID { get; set; }
        public int DeletedBy { get; set; }
    }

    public class ViewOrderModel : ViewParameters
    {
        public List<OdersModel> OdersList { get; set; }
        public int OdersID { get; set; }
        public int DeletedBy { get; set; }
    }
}
