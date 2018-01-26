using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RepidShare.Entities
{
    public class DocumentResultSummaryModel : ViewParameters
    {
        public int DocumentID { get; set; }
        public int UserId { get; set; }
        public int ApplicationRoleID { get; set; }
        public string ReferralUrl { get; set; }
        public string UrlPageName { get; set; }
        public string UrlTitle { get; set; }
        public bool IsViewOnly { get; set; }
        public Boolean? IsAdminResult { get; set; }
        
        public List<DocumentResultSummaryDetailModel> ResultSummaryDetail { get; set; }
        public int Index { get; set; }
        public string ParamValue { get; set; }
      //  public int AppID { get; set; }
        
    }
}
