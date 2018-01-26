using RepidShare.Business;
using RepidShare.Entities;
using RepidShare.Entities.DocumentResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RepidShare.API.Controllers
{
    public class DocumentResponseController : ApiController
    {
        BLDocumentResponse objBLDocumentResponse = new BLDocumentResponse();
        /// <summary>
        /// Get  Document Response For Save
        /// </summary>
        /// <param name="ObjDocumentApplicationMappingDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public DocumentResponseDetailModel GetDocumentResponseForSave(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            BLDocument objBLDocument = new BLDocument();
            BLStep objBLStep = new BLStep();
            //1. Fill Document detail
            objDocumentResponseDetailModel.objDocumentModel = new DocumentModel();
            objDocumentResponseDetailModel.objDocumentModel = objBLDocument.GetDocumentById(objDocumentResponseDetailModel.DocumentID);
            objDocumentResponseDetailModel.objDocumentModel.DocumentHTML = objBLDocumentResponse.GetDocumentPreviewTemp(objDocumentResponseDetailModel.DocumentID, objDocumentResponseDetailModel.UserId);
            //2. Fill Step detail
            objDocumentResponseDetailModel.objStepList = new List<StepModel>();
            objDocumentResponseDetailModel.objStepList = objBLStep.GetStepByDocumentIdAndUserId(objDocumentResponseDetailModel.DocumentID, objDocumentResponseDetailModel.UserId);
           

            return objBLDocumentResponse.GetDocumentResponseForSave(objDocumentResponseDetailModel);
        }

        /// <summary>
        /// Get  Document Response For view
        /// </summary>
        /// <param name="ObjDocumentApplicationMappingDetail"></param>
        /// <returns></returns>
        public DocumentResponseDetailModel GetDocumentResponseForView(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            return objBLDocumentResponse.GetDocumentResponseForView(objDocumentResponseDetailModel);
        }

        /// <summary>
        /// Get Document response detail
        /// </summary>
        /// <param name="ObjDocumentApplicationMappingDetail"></param>
        /// <returns></returns>
        public DocumentResponseDetailModel GetDocumentPreview(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            return objBLDocumentResponse.GetDocumentPreview(objDocumentResponseDetailModel);
        }

        /// <summary>
        /// Insert or Update Document Answer 
        /// </summary>
        /// <param name="objViewQuestionModel">Object of Model ViewQuestionModel</param>
        /// <returns></returns>
        public DocumentResponseDetailModel InsertUpdateDocumentResult(DocumentResponseDetailModel objDocumentResponseDetailModel)
        {
            return objBLDocumentResponse.InsertUpdateDocumentResult(objDocumentResponseDetailModel);
        }

        [HttpGet]
        public DocumentModel GetUserDocumentView(int DocumentId, int UserId)
        {
            DocumentModel objDocumentModel = new DocumentModel();
            BLDocument objBLDocument = new BLDocument();

            objDocumentModel = objBLDocument.GetDocumentById(DocumentId);
            objDocumentModel.DocumentHTML = objBLDocumentResponse.GetDocumentPreviewTemp(DocumentId, UserId);

            objDocumentModel.objListActivityModel = new List<ActivityModel>();
            objDocumentModel.objListActivityModel = objBLDocumentResponse.GetUserDocumentHistory(DocumentId, UserId);

            return objDocumentModel;
        }

        [HttpPost]
        public PriceQuestionModel InsertUpdatePriceQuestion(PriceQuestionModel objPriceQuestionModel)
        {
            return objBLDocumentResponse.InsertUpdatePriceQuestion(objPriceQuestionModel);
        }

        [HttpPost]
        public PriceQuestionModel GetPriceQuestion(PriceQuestionModel objPriceQuestionModel)
        {
            return objBLDocumentResponse.GetPriceQuestion(objPriceQuestionModel);
        }

        public void InsertStepSatus(UserDetailModel objUserDetailModel)
        {
          objBLDocumentResponse.InsertStepSatus(objUserDetailModel);
        }

    }
}
