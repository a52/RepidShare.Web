
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class ViewDocumentResponseModel : ViewParameters
    {
        public List<DocumentResponseModel> lstDocumentResponseModel { get; set; }
        public int AppID { get; set; }
        public int UserId { get; set; }
        public int ApplicationRoleId { get; set; }
    }
}
