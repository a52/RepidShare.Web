
using RepidShare.Entities;
using RepidShare.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace RepidShare.Admin.Controllers
{
    public class OrderController : BaseController
    {
        HttpResponseMessage serviceResponse;
        private UtilityWeb objUtilityWeb = new UtilityWeb();



        #region View  Order
        /// <summary>
        /// View  Order List
        /// </summary>
        /// <returns></returns>
        [Filters.Authorized]
        public ActionResult ViewOrder()
        {
            ViewOrderModel ObjViewOrderModel = new ViewOrderModel();
            try
            {
                //initial set of current page, pageSize , Total pages
                ObjViewOrderModel.CurrentPage = 1;
                ObjViewOrderModel.PageSize = CommonUtils.PageSize;
                ObjViewOrderModel.TotalPages = 0;

                //Get  Order List 
                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Order + "/GetOrderList", ObjViewOrderModel);

                ObjViewOrderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewOrderModel>().Result : null;
                //ObjViewOrderModel = objBLOrder.GetOrderList(ObjViewOrderModel);

                //Set Success Message if comes from save  page after click on save button
                if (!String.IsNullOrEmpty(Convert.ToString(TempData["SucessMessage"])))
                {
                    ObjViewOrderModel.Message = Convert.ToString(TempData["SucessMessage"]);
                    ObjViewOrderModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    TempData["SucessMessage"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Order", "View GET");
            }
            return View("ViewOrder", ObjViewOrderModel);
        }

        /// <summary>
        /// View  Order List with Searching, sorting , paging Parameters
        /// </summary>
        /// <param name="objViewOrderModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Filters.Authorized]
        public ActionResult ViewOrder(ViewOrderModel objViewOrderModel)
        {
            try
            {

                int ErrorCode = 0;
                String ErrorMessage = "";
                objViewOrderModel.Message = objViewOrderModel.MessageType = String.Empty;

                if (objViewOrderModel.ActionType == "delete")
                {
                    //delete
                    serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Order + "/DeleteOrder", objViewOrderModel);
                    objViewOrderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewOrderModel>().Result : null;

                    if (Convert.ToInt32(ErrorCode).Equals(0))
                    {
                        //if error code 0 means delete successfully than set Delete success message.
                        objViewOrderModel.Message = "Order Deleted Successfully";
                        objViewOrderModel.MessageType = CommonUtils.MessageType.Success.ToString().ToLower();
                    }
                    else
                    {
                        //if error code is not 0 means delete error  than set Delete error message.
                        objViewOrderModel.Message = "Error while deleting record";
                        objViewOrderModel.MessageType = CommonUtils.MessageType.Error.ToString().ToLower(); ;

                    }
                }
                //Get  Order List based on searching , sorting and paging parameter.

                serviceResponse = objUtilityWeb.PostAsJsonAsync(WebApiURL.Order + "/GetOrderList", objViewOrderModel);
                objViewOrderModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewOrderModel>().Result : null;

            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Order", "View POST");
            }
            return PartialView("_OrderList", objViewOrderModel);
        }


        [HttpGet]
        [Filters.Authorized]
        public ActionResult ViewOrderDetail(string prm = "")
        {



            ViewOdersDetailModel objViewOdersDetailModel = new ViewOdersDetailModel();
            try
            {
                if (!String.IsNullOrEmpty(prm))
                {
                    int OrderId;
                    //decrypt parameter and set in CategoryId variable
                    int.TryParse(CommonUtils.Decrypt(prm), out OrderId);
                    //Get Category detail by  Category Id

                    serviceResponse = objUtilityWeb.GetAsync(WebApiURL.Order + "/GetOrderById?OrderId=" + OrderId.ToString());
                    objViewOdersDetailModel = serviceResponse.StatusCode == HttpStatusCode.OK ? serviceResponse.Content.ReadAsAsync<ViewOdersDetailModel>().Result : null;

                    return PartialView("_OrderDetailList", objViewOdersDetailModel);
                }


            }
            catch (Exception ex)
            {
                ErrorLog(ex, "Order", "View POST");
            }
            return PartialView("_OrderDetailList", objViewOdersDetailModel);
        }


        #endregion
    }
}
