using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Entities
{
    public class HomeLayOutModel
    {
        public ViewCategoryModel objViewCategoryModel { get; set; }
        public ViewSubCategoryModel objSubViewCategoryModel { get; set; }
    }

    public class HomeCategoryViewModel
    {
        public CategoryModel objCategoryModel { get; set; }

        public List<SubCategoryModel> objListSubCategory { get; set; }

        public List<DocumentModel> objListDocumentModel { get; set; }

        public int SelectedSubCatId { get; set; }
    }

    public class HomeLawGuideViewModel
    {
        public CategoryModel objCategoryModel { get; set; }

        public List<SubCategoryModel> objListSubCategory { get; set; }

        public List<LawGuideModel> objListLawGuideModel { get; set; }

        public int SelectedLawGuideId { get; set; }
    }

    public class HomeDocumentViewModel
    {
        public CategoryModel objCategoryModel { get; set; }

        public List<SubCategoryModel> objListSubCategory { get; set; }

        public DocumentModel objDocumentModel { get; set; }

        public List<DocumentModel> objListDocumentService { get; set; }

        public int SelectedDocumentId { get; set; }
    }

    public class PackedDocumentsParent
    {
        public int SubCategoryID { get; set; }

        public string DocumentTitle { get; set; }

        public string DocumentDescription { get; set; }
    }

   
}
