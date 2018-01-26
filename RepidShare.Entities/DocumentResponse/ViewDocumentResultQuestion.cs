using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RepidShare.Entities
{
    public class ViewDocumentResultQuestion : ViewParameters
    {
        //[LocalizedDisplayName(typeof(CommonResource), "lblApplicationName")]
        public string ApplicationName { get; set; }
        //[LocalizedDisplayName(typeof(DocumentResource), "lblDocumentName")]
        public string Name { get; set; }
        //[LocalizedDisplayName(typeof(DocumentResource), "Description")]
        public string Description { get; set; }
        public int DocumentID { get; set; }

        public bool IsComplete { get; set; }
        public List<DocumentResultQuestionModel> DocumentResultQuestionList { get; set; }
        public int DocumentApplicationMappingId { get; set; }
        public string ActionType { get; set; }
        public string ParamValue { get; set; }
        public int CreatedBy { get; set; }
        public int AppID { get; set; }
        public bool? IsAdminResult { get; set; }
        public bool IsPublish { get; set; }
        public bool ShowDocumentResultToUser { get; set; }
        public bool IsAuthorized { get; set; }
        public string ReferralUrl { get; set; }
        public string UrlPageName { get; set; }
        public string UrlTitle { get; set; }
        public int ApplicationRoleID { get; set; }
    }
}
