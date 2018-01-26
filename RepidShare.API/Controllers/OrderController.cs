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
    public class OrderController : ApiController
    {
        private BLOrder objBLOrder = new BLOrder();

        #region Get ,Insert, Update and delete Order

        [HttpGet]
        public ViewOdersDetailModel GetOrderById(int OrderId)
        {
            return objBLOrder.GetOrderById(OrderId);
        }

        [HttpPost]
        public OdersModel InsertUpdateOrder(OdersModel objOrderModel)
        {
            return objBLOrder.InsertUpdateOrder(objOrderModel);
        }


        #endregion

        #region View  Order
        [HttpPost]
        public ViewOrderModel GetOrderList(ViewOrderModel objViewOrderModel)
        {
            return objBLOrder.GetOrderList(objViewOrderModel);
        }
        #endregion

         
    }
}
