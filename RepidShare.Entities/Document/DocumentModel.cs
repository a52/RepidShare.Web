using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities.Resource;
using System.ComponentModel.DataAnnotations;


namespace RepidShare.Entities
{
    public class DocumentModel : BaseModel
    {
        public int DocumentID { get; set; }
        [Required(ErrorMessage = "Please Select Category ")]
        [LocalizedDisplayName(typeof(CommonResource), "lblCategoryName")]
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Please Select Sub Category ")]
        [LocalizedDisplayName(typeof(CommonResource), "lblSubCategoryName")]
        public int SubCategoryID { get; set; }

        public string CategoryName { get; set; }
        public string SubCatName { get; set; }

        [Required(ErrorMessage = "Document Title is Required")]
        [LocalizedDisplayName(typeof(DocumentResource), "lblDocumentTitle")]
        public string DocumentTitle { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [LocalizedDisplayName(typeof(DocumentResource), "lblDocumentDescription")]
        public string DocumentDescription { get; set; }

        [LocalizedDisplayName(typeof(DocumentResource), "lblDocumentHTML")]
        public string DocumentHTML { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [LocalizedDisplayName(typeof(DocumentResource), "lblPrice")]
        public double Price { get; set; }

        [LocalizedDisplayName(typeof(DocumentResource), "lblIsPublished")]
        public bool IsPublished { get; set; }

        [LocalizedDisplayName(typeof(DocumentResource), "lblIsMostPopular")]
        public bool IsMostPopular { get; set; }

        //[Required(ErrorMessage = "Please Select Group Title")]
        [LocalizedDisplayName(typeof(CommonResource), "lblGroupText")]
        public int? GroupID { get; set; }

        [LocalizedDisplayName(typeof(DocumentResource), "lblIncludeService")]
        public string PackIds { get; set; }

        [LocalizedDisplayName(typeof(DocumentResource), "lblIncludeService")]
        public string UserIds { get; set; }

        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int[] AllSteps { get; set; }
        public int[] SelectedSteps { get; set; }
        public string SavedStep { get; set; }
        public string ActionType { get; set; }
        public string SelectedText { get; set; }
        public string GroupName { get; set; }

        public List<ActivityModel> objListActivityModel { get; set; }

        public int FolderId { get; set; }

        public int ComPercentage { get; set; }

        [Required(ErrorMessage = "Please enter email.")]
        public string ShareEMail { get; set; }
    }
    public class ViewDocumentModel : ViewParameters
    {
        public List<DocumentModel> DocumentList { get; set; }
        public int DeletedDocumentID { get; set; }
        public int DeletedBy { get; set; }
    }
}
