
using RepidShare.Entities.DocumentResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class DocumentResponseDetailModel : ViewParameters
    {
        public List<ViewQuestionAnswerModel> Questions { get; set; }
        public DocumentResultModel Result { get; set; }
        public int AppID { get; set; }
        public int DocumentID { get; set; }
        public int DocumentRoleID { get; set; }
        public int UserId { get; set; }
        public string QuestionAnswerXml { get; set; }
        public int Index { get; set; }
        public string ReferralUrl { get; set; }
        public string UrlPageName { get; set; }
        public string UrlTitle { get; set; }
        public int ApplicationRoleID { get; set; }
        public int NoOfAttempt { get; set; }
        public int MaxNoOfAttempt { get; set; }
        public DocumentModel objDocumentModel { get; set; }
        public List<StepModel> objStepList { get; set; }
        public int FolderId { get; set; }
        public int isSaved { get; set; }
        public int MaxPage { get; set; }
        public List<UserDetailModel> objUserDetailModel { get; set; }
    }

}
