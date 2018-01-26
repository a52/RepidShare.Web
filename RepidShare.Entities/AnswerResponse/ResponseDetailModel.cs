
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class ResponseDetailModel : ViewParameters
    {
        //public ApplicationMappingModel ApplicationDetail { get; set; }
        public List<ViewQuestionAnswerModel> Questions { get; set; }
        //public ResultModel Result { get; set; }
        public int AppID { get; set; }
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int UserId { get; set; }
        public string QuestionAnswerXml { get; set; }
        public int Index { get; set; }
        public string ReferralUrl { get; set; }
        public string UrlPageName { get; set; }
        public string UrlTitle { get; set; }
        public int ApplicationRoleID { get; set; }
        public int NoOfAttempt { get; set; }
        public int MaxNoOfAttempt { get; set; }

        
    }

}
