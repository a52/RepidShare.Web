using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Entities
{
    public class UserSummaryModel
    {
        public int TotalCount { get; set; }
        public int InProgressCount { get; set; }
        public int FreeTrialCount { get; set; }
        public List<ActivityModel> objListActivityModel { get; set; }
    }

    public class ActivityModel : BaseModel
    {
        public long ActivityId { get; set; }
        public int UserId { get; set; }
        public string ActivityText { get; set; }
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
    }

    public class UserMyDocument : ViewParameters
    {
        public int UserId { get; set; }
        public int TotalCount { get; set; }
        public int InProgressCount { get; set; }
        public int FreeTrialCount { get; set; }
        public int BinDocumentCount { get; set; }
        public List<FolderModel> objListFolderModel { get; set; }
        public List<DocumentModel> objListDocumentModel { get; set; }

        public string SearchType { get; set; }
        public int FolderId { get; set; }
    }

    public class FolderModel : BaseModel
    {
        public int DocumentId { get; set; }
        public int UserId { get; set; }
        public int FolderId { get; set; }
        [Required]
        public string FolderName { get; set; }
        public int Count { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ViewFolderModel : ViewParameters
    {
        public int UserId { get; set; }
        public List<FolderModel> objListFolderModel { get; set; }
        public int DeletedFolderID { get; set; }
        public int DeletedDocumentID { get; set; }
    }

    public class SharedDocument : BaseModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage ="Please enter title")]
        
        public string SharedDocumentTitle { get; set; }

        public string SharedDocumentDescrition { get; set; }
    }
    public class SummaryAndMyDocument
    {
        public UserSummaryModel userSummaryModel { get; set; }
        public UserMyDocument myDocument { get; set; }
        public UserMyDocument myTrailDocument { get; set; }

    }

}
