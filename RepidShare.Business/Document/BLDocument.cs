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
    public class BLDocument : BLBase
    {
        private DLDocument objDLDocument = new DLDocument();
        #region Get ,Insert, Update and delete Document
        /// <summary>
        /// Get Document By Id
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <returns>Document Model</returns>
        public DocumentModel GetDocumentById(int DocumentId)
        {
            //Call GetDocumentBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLDocument.GetDocumentById(DocumentId);
            DocumentModel objDocumentModel = new DocumentModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objDocumentModel = GetDataRowToEntity<DocumentModel>(dt.Rows[0]);
            }

            return objDocumentModel;
        }

        /// <summary>
        /// Insert or Update  Document
        /// </summary>
        /// <param name="objDocumentModel">object of  Document Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public DocumentModel InsertUpdateDocument(DocumentModel objDocumentModel)
        {
            //call InsertUpdateDocument Method of dataLayer and return DocumentModel
            return objDLDocument.InsertUpdateDocument(objDocumentModel);
        }

        public ViewDocumentModel DeleteDocument(ViewDocumentModel objViewDocumentModel)
        {
            return objDLDocument.DeleteDocument(objViewDocumentModel);
        }
        #endregion

        #region View  Document
        /// <summary>
        /// Get  Document List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewDocumentModel">object of Model ViewDocumentModel</param>
        /// <returns></returns>
        public ViewDocumentModel GetDocumentList(ViewDocumentModel objViewDocumentModel)
        {
            List<DocumentModel> lstDocumentModel = new List<DocumentModel>();
            //if FilterDocumentName is NULL than set to empty
            objViewDocumentModel.FilterSubCatName = objViewDocumentModel.FilterSubCatName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewDocumentModel.SortBy = objViewDocumentModel.SortBy ?? String.Empty;

            //call GetDocumentList Method which will retrun datatable of  Document
            DataTable dtDocument = objDLDocument.GetDocumentList(objViewDocumentModel);

            //fetch each row of datable
            foreach (DataRow dr in dtDocument.Rows)
            {
                //Convert datarow into Model object and set Model object property
                DocumentModel itemDocumentModel = GetDataRowToEntity<DocumentModel>(dr);
                //Add  Document in List
                lstDocumentModel.Add(itemDocumentModel);
            }

            //set Document List of Model ViewDocumentModel
            objViewDocumentModel.DocumentList = lstDocumentModel;
            //if  Document List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewDocumentModel != null && objViewDocumentModel.DocumentList != null && objViewDocumentModel.DocumentList.Count > 0)
            {
                objViewDocumentModel.CurrentPage = objViewDocumentModel.DocumentList[0].CurrentPage;
                int totalRecord = objViewDocumentModel.DocumentList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewDocumentModel.PageSize) > 0)
                    objViewDocumentModel.TotalPages = (totalRecord / objViewDocumentModel.PageSize + 1);
                else
                    objViewDocumentModel.TotalPages = totalRecord / objViewDocumentModel.PageSize;


            }
            else
            {
                objViewDocumentModel.TotalPages = 0;
                objViewDocumentModel.CurrentPage = 1;
            }
            return objViewDocumentModel;
        }
        #endregion

        #region Document Response
        /// <summary>
        /// Get All Document Response
        /// </summary>
        /// <param name="ObjViewDocumentResponseModel"></param>
        /// <returns></returns>
        public ViewDocumentResponseModel GetAllDocumentResponse(ViewDocumentResponseModel ObjViewDocumentResponseModel)
        {
            //initialize DocumentResponseModel lstDocumentResponseModel
            ObjViewDocumentResponseModel.lstDocumentResponseModel = new List<DocumentResponseModel>();
            //Call DLDocument for Get All Document Response using filters 
            DataTable dtDocument = objDLDocument.GetAllDocumentResponse(ObjViewDocumentResponseModel);
            //Check dtDocument not null and Rows count 
            if (dtDocument != null && dtDocument.Rows.Count > 0)
            {
                for (int i = 0; i < dtDocument.Rows.Count; i++)
                {
                    //Convert DataRow to DocumentResponseModel Row and adding in lstDocumentResponseModel
                    ObjViewDocumentResponseModel.lstDocumentResponseModel.Add(GetDataRowToEntity<DocumentResponseModel>(dtDocument.Rows[i]));
                }

                int totalRecord = Convert.ToInt32(dtDocument.Rows[0]["TotalCount"]);
                //calculating total paging 
                ObjViewDocumentResponseModel.TotalPages = TotalPage(totalRecord, ObjViewDocumentResponseModel.PageSize);
            }
            else
            {
                //if Return o rows then set defualt values for TotalPages and CurrentPage
                ObjViewDocumentResponseModel.TotalPages = 0;
                ObjViewDocumentResponseModel.CurrentPage = 1;
            }
            return ObjViewDocumentResponseModel;
        }

        /// <summary>
        /// Get All Document Response User
        /// </summary>
        /// <param name="ObjViewDocumentUserResponseModel"></param>
        /// <returns></returns>
        public ViewDocumentUserResponseModel GetAllDocumentResponseUser(ViewDocumentUserResponseModel ObjViewDocumentUserResponseModel)
        {
            //initialize ViewDocumentUserResponseModel lstDocumentUserResponseModel
            ObjViewDocumentUserResponseModel.lstDocumentUserResponseModel = new List<DocumentUserResponseModel>();
            //Call DLDocument for Get All Document Response User using filters 
            DataSet dtDocument = objDLDocument.GetAllDocumentResponseUser(ObjViewDocumentUserResponseModel);

            if (dtDocument != null && dtDocument.Tables[0].Rows.Count > 0)
            {
                ObjViewDocumentUserResponseModel.DocumentName = dtDocument.Tables[0].Rows[0]["DocumentTitle"].ToString();
                ObjViewDocumentUserResponseModel.DocumentDescription = dtDocument.Tables[0].Rows[0]["DocumentDescription"].ToString();
            }

            //Check dtDocument not null and Rows count 
            if (dtDocument != null && dtDocument.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < dtDocument.Tables[1].Rows.Count; i++)
                {
                    //Convert DataRow to DocumentUserResponseModel Row and adding in lstDocumentUserResponseModel
                    ObjViewDocumentUserResponseModel.lstDocumentUserResponseModel.Add(GetDataRowToEntity<DocumentUserResponseModel>(dtDocument.Tables[1].Rows[i]));
                }

                int totalRecord = Convert.ToInt32(dtDocument.Tables[1].Rows[0]["TotalCount"]);
                //calculating total paging 
                ObjViewDocumentUserResponseModel.TotalPages = TotalPage(totalRecord, ObjViewDocumentUserResponseModel.PageSize);
            }
            else
            {
                //if Return o rows then set defualt values for TotalPages and CurrentPage
                ObjViewDocumentUserResponseModel.TotalPages = 0;
                ObjViewDocumentUserResponseModel.CurrentPage = 1;
            }
            return ObjViewDocumentUserResponseModel;
        }
        #endregion

        #region My Document
        /// <summary>
        /// Get User Document List
        /// </summary>
        /// <param name="ObjViewDocumentResponseModel"></param>
        /// <returns></returns>
        public ViewDocumentResponseModel GetUserDocumentList(ViewDocumentResponseModel ObjViewDocumentResponseModel)
        {
            //initialize ObjViewDocumentResponseModel lstDocumentResponseModel
            ObjViewDocumentResponseModel.lstDocumentResponseModel = new List<DocumentResponseModel>();
            //Call DLDocument for Get All Document Response User using filters 
            DataTable dtDocument = objDLDocument.GetUserDocumentList(ObjViewDocumentResponseModel);
            //Check dtDocument not null and Rows count 
            if (dtDocument != null && dtDocument.Rows.Count > 0)
            {
                for (int i = 0; i < dtDocument.Rows.Count; i++)
                {
                    //Convert DataRow to DocumentUserResponseModel Row and adding in lstDocumentUserResponseModel
                    ObjViewDocumentResponseModel.lstDocumentResponseModel.Add(GetDataRowToEntity<DocumentResponseModel>(dtDocument.Rows[i]));
                }

                int totalRecord = Convert.ToInt32(dtDocument.Rows[0]["TotalCount"]);
                //calculating total paging 
                ObjViewDocumentResponseModel.TotalPages = TotalPage(totalRecord, ObjViewDocumentResponseModel.PageSize);
            }
            else
            {
                //if Return o rows then set defualt values for TotalPages and CurrentPage
                ObjViewDocumentResponseModel.TotalPages = 0;
                ObjViewDocumentResponseModel.CurrentPage = 1;
            }
            return ObjViewDocumentResponseModel;
        }
        #endregion
    }
}
