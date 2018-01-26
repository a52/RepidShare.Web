using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIMSEntities;
using System.Data;
using RepidShare.Data;
using RepidShare.Business;
using RepidShare.Entities;

namespace RepidShare.Business
{


    public class BLHome : BLBase
    {
        private DLHome objHome = new DLHome();


        public HomeCategoryViewModel GetCategoryList(int CategoryId)
        {
            HomeCategoryViewModel objeHomeCategoryViewModel = new HomeCategoryViewModel();

            DataSet dtCategory = objHome.GetCategoryByID(CategoryId);

            if (dtCategory != null && dtCategory.Tables.Count > 0)
            {
                objeHomeCategoryViewModel.objCategoryModel = new CategoryModel();

                objeHomeCategoryViewModel.objCategoryModel = GetDataRowToEntity<CategoryModel>(dtCategory.Tables[0].Rows[0]);

                if (dtCategory != null && dtCategory.Tables.Count > 1)
                {
                    objeHomeCategoryViewModel.objListSubCategory = new List<SubCategoryModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtCategory.Tables[1].Rows)
                    {
                        objeHomeCategoryViewModel.objListSubCategory.Add(GetDataRowToEntity<SubCategoryModel>(dr));
                    }
                }

                if (dtCategory != null && dtCategory.Tables.Count > 2)
                {
                    objeHomeCategoryViewModel.objListDocumentModel = new List<DocumentModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtCategory.Tables[2].Rows)
                    {
                        objeHomeCategoryViewModel.objListDocumentModel.Add(GetDataRowToEntity<DocumentModel>(dr));
                    }
                }
            }

            return objeHomeCategoryViewModel;
        }

        public HomeCategoryViewModel GetSubCategoryList(int SubCategoryId)
        {
            HomeCategoryViewModel objeHomeCategoryViewModel = new HomeCategoryViewModel();

            DataSet dtCategory = objHome.GetSubCategoryByID(SubCategoryId);

            if (dtCategory != null && dtCategory.Tables.Count > 0)
            {
                objeHomeCategoryViewModel.objCategoryModel = new CategoryModel();

                objeHomeCategoryViewModel.objCategoryModel = GetDataRowToEntity<CategoryModel>(dtCategory.Tables[0].Rows[0]);

                if (dtCategory != null && dtCategory.Tables.Count > 1)
                {
                    objeHomeCategoryViewModel.objListSubCategory = new List<SubCategoryModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtCategory.Tables[1].Rows)
                    {
                        objeHomeCategoryViewModel.objListSubCategory.Add(GetDataRowToEntity<SubCategoryModel>(dr));
                    }
                }

                if (dtCategory != null && dtCategory.Tables.Count > 2)
                {
                    objeHomeCategoryViewModel.objListDocumentModel = new List<DocumentModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtCategory.Tables[2].Rows)
                    {
                        objeHomeCategoryViewModel.objListDocumentModel.Add(GetDataRowToEntity<DocumentModel>(dr));
                    }
                }
            }

            return objeHomeCategoryViewModel;
        }

        public HomeLawGuideViewModel GetLawGuideList(int CategoryId)
        {
            HomeLawGuideViewModel objeHomeLawGuideViewModel = new HomeLawGuideViewModel();

            DataSet dtCategory = objHome.GetLawGuideByCategoryId(CategoryId);

            if (dtCategory != null && dtCategory.Tables.Count > 0)
            {
                objeHomeLawGuideViewModel.objCategoryModel = new CategoryModel();

                objeHomeLawGuideViewModel.objCategoryModel = GetDataRowToEntity<CategoryModel>(dtCategory.Tables[0].Rows[0]);

                if (dtCategory != null && dtCategory.Tables.Count > 1)
                {
                    objeHomeLawGuideViewModel.objListSubCategory = new List<SubCategoryModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtCategory.Tables[1].Rows)
                    {
                        objeHomeLawGuideViewModel.objListSubCategory.Add(GetDataRowToEntity<SubCategoryModel>(dr));
                    }
                }

                if (dtCategory != null && dtCategory.Tables.Count > 2)
                {
                    objeHomeLawGuideViewModel.objListLawGuideModel = new List<LawGuideModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtCategory.Tables[2].Rows)
                    {
                        objeHomeLawGuideViewModel.objListLawGuideModel.Add(GetDataRowToEntity<LawGuideModel>(dr));
                    }
                }
            }

            return objeHomeLawGuideViewModel;
        }

        public HomeDocumentViewModel GetDocumentList(int DocumentId)
        {
            HomeDocumentViewModel objHomeDocumentViewModel = new HomeDocumentViewModel();

            DataSet dtCategory = objHome.GetDocumentByDocumentId(DocumentId);

            if (dtCategory != null && dtCategory.Tables.Count > 0)
            {
                objHomeDocumentViewModel.objCategoryModel = new CategoryModel();

                objHomeDocumentViewModel.objCategoryModel = GetDataRowToEntity<CategoryModel>(dtCategory.Tables[0].Rows[0]);

                if (dtCategory != null && dtCategory.Tables.Count > 1)
                {
                    objHomeDocumentViewModel.objListSubCategory = new List<SubCategoryModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtCategory.Tables[1].Rows)
                    {
                        objHomeDocumentViewModel.objListSubCategory.Add(GetDataRowToEntity<SubCategoryModel>(dr));
                    }
                }

                if (dtCategory != null && dtCategory.Tables.Count > 2)
                {
                    objHomeDocumentViewModel.objDocumentModel = new DocumentModel();
                    objHomeDocumentViewModel.objDocumentModel = GetDataRowToEntity<DocumentModel>(dtCategory.Tables[2].Rows[0]);
                }

                if (dtCategory != null && dtCategory.Tables.Count > 3)
                {
                    objHomeDocumentViewModel.objListDocumentService = new List<DocumentModel>();
                    //fetch each row of datable
                    foreach (DataRow dr in dtCategory.Tables[3].Rows)
                    {
                        objHomeDocumentViewModel.objListDocumentService.Add(GetDataRowToEntity<DocumentModel>(dr));
                    }
                }
            }

            return objHomeDocumentViewModel;
        }
    }
}
