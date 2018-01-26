
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class ViewDocumentUserResponseModel : ViewParameters
    {
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public List<DocumentUserResponseModel> lstDocumentUserResponseModel { get; set; }
        public int AppID { get; set; }
    }
}
