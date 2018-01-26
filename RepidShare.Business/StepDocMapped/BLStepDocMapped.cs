using RepidShare.Data;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Business
{
    public class BLStepDocMapped : BLBase
    {
        private DLStepDocMapped objDLStepDocMapped = new DLStepDocMapped();
        #region Get ,Insert, Update and delete StepDocMapped
        /// <summary>
        /// Get StepDocMapped By Id
        /// </summary>
        /// <param name="StepDocMappedId"></param>
        /// <returns>StepDocMapped Model</returns>
        public StepDocMappedModel GetStepDocMappedById(int StepDocMappedId)
        {
            //Call GetStepDocMappedBYId method of dataLayer which will return Datatable.
            DataTable dt = objDLStepDocMapped.GetStepDocMappedById(StepDocMappedId);
            StepDocMappedModel objStepDocMappedModel = new StepDocMappedModel();
            // if datatable has row than set object parameters
            if (dt.Rows.Count > 0)
            {
                objStepDocMappedModel = GetDataRowToEntity<StepDocMappedModel>(dt.Rows[0]);
            }

            return objStepDocMappedModel;
        }

        /// <summary>
        /// Insert or Update  StepDocMapped
        /// </summary>
        /// <param name="objStepDocMappedModel">object of  StepDocMapped Model</param>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public StepDocMappedModel InsertUpdateStepDocMapped(StepDocMappedModel objStepDocMappedModel)
        {
            //call InsertUpdateStepDocMapped Method of dataLayer and return StepDocMappedModel
            return objDLStepDocMapped.InsertUpdateStepDocMapped(objStepDocMappedModel);
        }

        public ViewStepDocMappedModel DeleteStepDocMapped(ViewStepDocMappedModel objViewStepDocMappedModel)
        {
            return objDLStepDocMapped.DeleteStepDocMapped(objViewStepDocMappedModel);
        }
        #endregion

        #region View  StepDocMapped
        /// <summary>
        /// Get  StepDocMapped List based on paging, searching and sorting parameter
        /// </summary>
        /// <param name="objViewStepDocMappedModel">object of Model ViewStepDocMappedModel</param>
        /// <returns></returns>
        public ViewStepDocMappedModel GetStepDocMappedList(int StepID, int DocumentID)
        {
            ViewStepDocMappedModel objViewStepDocMappedModel = new ViewStepDocMappedModel();
            objViewStepDocMappedModel.StepDocMappedList = new List<StepDocMappedModel>();
           
            //call GetStepDocMappedList Method which will retrun datatable of  StepDocMapped
            DataTable dtStepDocMapped = objDLStepDocMapped.GetStepDocMappedByDocumentID(StepID, DocumentID);

            //fetch each row of datable
            foreach (DataRow dr in dtStepDocMapped.Rows)
            {
                //Convert datarow into Model object and set Model object property
                StepDocMappedModel itemStepDocMappedModel = GetDataRowToEntity<StepDocMappedModel>(dr);
                //Add  StepDocMapped in List
                objViewStepDocMappedModel.StepDocMappedList.Add(itemStepDocMappedModel);
            }
            return objViewStepDocMappedModel;
        }
        #endregion

        
    }
}
