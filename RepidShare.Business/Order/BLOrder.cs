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
    public class BLOrder : BLBase
    {
        private DLOrder objDLOrder = new DLOrder();

        #region View  Order
        public ViewOrderModel GetOrderList(ViewOrderModel objViewOrderModel)
        {
            List<OdersModel> lstOdersModel = new List<OdersModel>();
            //if FilterDocumentName is NULL than set to empty
            objViewOrderModel.FilterSubCatName = objViewOrderModel.FilterSubCatName ?? String.Empty;

            //if SortBy is NULL than set to empty
            objViewOrderModel.SortBy = objViewOrderModel.SortBy ?? String.Empty;

            //call GetDocumentList Method which will retrun datatable of  Document
            DataTable dtDocument = objDLOrder.GetOrderList(objViewOrderModel);

            //fetch each row of datable
            foreach (DataRow dr in dtDocument.Rows)
            {
                //Convert datarow into Model object and set Model object property
                OdersModel itemOdersModel = GetDataRowToEntity<OdersModel>(dr);
                //Add  Document in List
                lstOdersModel.Add(itemOdersModel);
            }

            //set Document List of Model ViewDocumentModel
            objViewOrderModel.OdersList = lstOdersModel;
            //if  Document List count is not null and greater than 0 Than set Total Pages for Paging.
            if (objViewOrderModel != null && objViewOrderModel.OdersList != null && objViewOrderModel.OdersList.Count > 0)
            {
                objViewOrderModel.CurrentPage = objViewOrderModel.OdersList[0].CurrentPage;
                int totalRecord = objViewOrderModel.OdersList[0].TotalCount;

                if (decimal.Remainder(totalRecord, objViewOrderModel.PageSize) > 0)
                    objViewOrderModel.TotalPages = (totalRecord / objViewOrderModel.PageSize + 1);
                else
                    objViewOrderModel.TotalPages = totalRecord / objViewOrderModel.PageSize;


            }
            else
            {
                objViewOrderModel.TotalPages = 0;
                objViewOrderModel.CurrentPage = 1;
            }
            return objViewOrderModel;
        }

        public ViewOdersDetailModel GetOrderById(int OrderId)
        {
            ViewOdersDetailModel objViewOdersDetailModel = new ViewOdersDetailModel();

            objViewOdersDetailModel.OdersDetailList = new List<OdersDetailModel>();

            //call GetDocumentList Method which will retrun datatable of  Document
            DataTable dtDocument = objDLOrder.GetOrderDetailList(OrderId);

            //fetch each row of datable
            foreach (DataRow dr in dtDocument.Rows)
            {
                objViewOdersDetailModel.OdersDetailList.Add(GetDataRowToEntity<OdersDetailModel>(dr));
            }
            return objViewOdersDetailModel;
        }
        #endregion

        public OdersModel InsertUpdateOrder(OdersModel objOdersModel)
        {
            return objDLOrder.InsertUpdateOrder(objOdersModel);
        }

    }
}
