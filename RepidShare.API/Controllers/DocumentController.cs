using RepidShare.Business;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RepidShare.API.Controllers
{
    public class DocumentController : ApiController
    {
        private BLDocument objBLDocument = new BLDocument();

        #region Get ,Insert, Update and delete Document

        [HttpGet]
        public DocumentModel GetDocumentById(int DocumentId)
        {
            return objBLDocument.GetDocumentById(DocumentId);
        }

        [HttpPost]
        public DocumentModel InsertUpdateDocument(DocumentModel objDocumentModel)
        {
            return objBLDocument.InsertUpdateDocument(objDocumentModel);
        }

        [HttpPost]
        public ViewDocumentModel DeleteDocument(ViewDocumentModel objViewDocumentModel)
        {
            return objBLDocument.DeleteDocument(objViewDocumentModel);
        }

        #endregion

        #region View  Document
        [HttpPost]
        public ViewDocumentModel GetDocumentList(ViewDocumentModel objViewDocumentModel)
        {
            return objBLDocument.GetDocumentList(objViewDocumentModel);
        }
        #endregion

        #region Document Response
        /// <summary>
        /// Get All Document Response
        /// </summary>
        /// <param name="ObjViewDocumentResponseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ViewDocumentResponseModel GetAllDocumentResponse(ViewDocumentResponseModel ObjViewDocumentResponseModel)
        {
            return objBLDocument.GetAllDocumentResponse(ObjViewDocumentResponseModel);
        }

        /// <summary>
        /// Get All Document Response User
        /// </summary>
        /// <param name="ObjViewDocumentUserResponseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ViewDocumentUserResponseModel GetAllDocumentResponseUser(ViewDocumentUserResponseModel ObjViewDocumentUserResponseModel)
        {
            return objBLDocument.GetAllDocumentResponseUser(ObjViewDocumentUserResponseModel);
        }
        #endregion

        #region My Document
        /// <summary>
        /// Get User Document List
        /// </summary>
        /// <param name="ObjViewDocumentResponseModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ViewDocumentResponseModel GetUserDocumentList(ViewDocumentResponseModel ObjViewDocumentResponseModel)
        {
            return objBLDocument.GetUserDocumentList(ObjViewDocumentResponseModel);
        }
        #endregion
    }
}
