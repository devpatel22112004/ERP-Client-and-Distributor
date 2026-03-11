using MVCMarketing.Models;
using MVCMarketing.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static iTextSharp.tool.xml.html.HTML;

namespace MVCMarketing.Controllers
{
    [SessionTimeout]
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Customer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    ViewBag.JsonData = JSONString;
                }
            }
            return PartialView();
        }

        public ActionResult ItemInfo(string id)
        {
            ViewBag.JsonClientId = id;
            return PartialView();
        }

        public ActionResult AddClient(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Customer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SalesId", id);
                com.Parameters.AddWithValue("@Action", "ADDCLIENT");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    ViewBag.JsonData = JSONString;
                }
            }
            return PartialView();
        }

        public JsonResult EditItem(string id)
        {
            string JSONString = string.Empty;
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_CustomerOpeningAdjustment");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerOpeningAdjustmentId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    JSONString = JsonConvert.SerializeObject(dt);
                }
            }
            return Json(JSONString);
        }

        public ActionResult Adjustment(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_CustomerOpeningAdjustment");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", id);
                com.Parameters.AddWithValue("@Action", "CLIENT-AMOUNT");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    ViewBag.JSONData = JsonConvert.SerializeObject(dt);
                }
            }
            return PartialView();
        }

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult MakeQR(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Customer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    ViewBag.JsonData = JSONString;
                }
            }
            return PartialView();
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult InsertItem(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_CustomerItemPrice");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", formCollection["hdCustomerId"]);
                com.Parameters.AddWithValue("@CustomerItemPriceId", formCollection["hdCustomerItemPriceId"]);
                com.Parameters.AddWithValue("@ItemId", formCollection["hdItemId"]);
                com.Parameters.AddWithValue("@Price", formCollection["hdPrice"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_Customer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["ddDealer"]);
                com.Parameters.AddWithValue("@Company", formCollection["txtCompany"]);
                com.Parameters.AddWithValue("@CustomerName", formCollection["txtClientName"]);
                com.Parameters.AddWithValue("@CustomerAddress", formCollection["txtAddress"]);
                com.Parameters.AddWithValue("@CustomerLandMark", formCollection["txtLandMark"]);
                com.Parameters.AddWithValue("@CustomerArea", formCollection["txtArea"]);
                com.Parameters.AddWithValue("@CustomerCity", formCollection["txtCity"]);
                com.Parameters.AddWithValue("@CustomerState", formCollection["ddState"]);
                com.Parameters.AddWithValue("@CustomerPincode", formCollection["txtPostCode"]);
                com.Parameters.AddWithValue("@PersonName1", formCollection["txtName1"]);
                com.Parameters.AddWithValue("@CustomerContact1", formCollection["txtContact1"]);
                com.Parameters.AddWithValue("@PersonName2", formCollection["txtName2"]);
                com.Parameters.AddWithValue("@CustomerContact2", formCollection["txtContact2"]);
                com.Parameters.AddWithValue("@PersonName3", formCollection["txtName3"]);
                com.Parameters.AddWithValue("@CustomerContact3", formCollection["txtContact3"]);
                com.Parameters.AddWithValue("@CustomerEmail", formCollection["txtEmail1"]);
                com.Parameters.AddWithValue("@CustomerEmail2", formCollection["txtEmail2"]);
                com.Parameters.AddWithValue("@CustomerGST", formCollection["txtGstNo"]);
                com.Parameters.AddWithValue("@CustomerPAN", formCollection["txtPAN"]);
                com.Parameters.AddWithValue("@OpeningBalance", formCollection["txtOpeningBalance"]);
                com.Parameters.AddWithValue("@Status", formCollection["ddStatus"]);
                com.Parameters.AddWithValue("@BusinessProfile", formCollection["ddBusinessProfile"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult InsertAmount(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_CustomerOpeningAdjustment");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", formCollection["hdClientId"]);
                com.Parameters.AddWithValue("@Date", formCollection["txtDate"]);
                com.Parameters.AddWithValue("@Type", formCollection["ddType"]);
                com.Parameters.AddWithValue("@PaymentType", formCollection["ddPaymentType"]);
                com.Parameters.AddWithValue("@ReferenceNo", formCollection["txtRefNo"]);
                com.Parameters.AddWithValue("@Detail", formCollection["txtDetails"]);
                com.Parameters.AddWithValue("@Amount", formCollection["txtAmount"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult DeletePayment(int id)
        {
            SqlCommand com = new SqlCommand("sp_CustomerOpeningAdjustment");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CustomerOpeningAdjustmentId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
        }

        public JsonResult Delete(int id)
        {
            SqlCommand com = new SqlCommand("sp_Customer");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CustomerId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
        }

        [HttpPost]
        public ActionResult LoadDataAmount(string id)
        {
            var recordsTotal = 0;
            SqlCommand com = new SqlCommand("sp_CustomerOpeningAdjustment");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CustomerId", id);
            com.Parameters.AddWithValue("@Action", "SELECT-AMOUNT");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                recordsTotal = dt.Rows.Count;
                var data = JsonConvert.SerializeObject(dt);
                return Json(new { recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LoadDataItem(string ClientId, string CustomerItemPriceId, string ItemCode, string CategoryId, string SubcategoryId)
        {
            SqlCommand com = new SqlCommand("sp_CustomerItemPrice");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CustomerId", ClientId);
            com.Parameters.AddWithValue("@ItemCode", ItemCode == "" ? null : ItemCode);
            com.Parameters.AddWithValue("@CategoryId", CategoryId == "" ? null : CategoryId);
            com.Parameters.AddWithValue("@SubcategoryId", SubcategoryId == "" ? null : SubcategoryId);
            com.Parameters.AddWithValue("@Action", "SELECT-ITEM");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                var data = JsonConvert.SerializeObject(dt);// dt.AsEnumerable().Select(row => row.ItemArray).ToList();
                var recordsTotal = dt.Rows.Count;
                return Json(new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsFiltered = "0", recordsTotal = "0", data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        
        [HttpPost]
        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            //find search columns info
            var Company = pagination.data.columns[0].search.value;
            var CustomerName = pagination.data.columns[1].search.value;
            var CustomerContact1 = pagination.data.columns[2].search.value;
            var CustomerCity = pagination.data.columns[3].search.value;
            var CustomerState = pagination.data.columns[4].search.value;
            var CustomerArea = pagination.data.columns[5].search.value;
            var DistributorId = pagination.data.columns[6].search.value;
            var CustomerCode = pagination.data.columns[7].search.value;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Customer");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@Company", Company == "" ? null : Company);
            com.Parameters.AddWithValue("@CustomerName", CustomerName == "" ? null : CustomerName);
            com.Parameters.AddWithValue("@CustomerContact1", CustomerContact1 == "" ? null : CustomerContact1);
            com.Parameters.AddWithValue("@CustomerCity", CustomerCity == "" ? null : CustomerCity);
            com.Parameters.AddWithValue("@CustomerState", CustomerState == "" ? null : CustomerState);
            com.Parameters.AddWithValue("@CustomerArea", CustomerArea == "" ? null : CustomerArea);
            com.Parameters.AddWithValue("@DistributorId", DistributorId == "" ? null : DistributorId);
            com.Parameters.AddWithValue("@CustomerCode", CustomerCode == "" ? null : CustomerCode);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet dataSet = ConnectionClass.getDataSet(com);
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[1];
                DataTable dtCount = dataSet.Tables[0];
                recordsTotal = Convert.ToInt32(dtCount.Rows[0][0]);
                /*start: Add Image*/

                var data = dt.AsEnumerable().Select(row => row.ItemArray).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        //[HttpPost]
        //public ActionResult LoadData()
        //{
        //    //jQuery DataTables Param
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    //Find paging info
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();

        //    //find search columns info
        //    var Search = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt16(start) : 0;
        //    int recordsTotal = 0;

        //    int page = (skip / pageSize) + 1;

        //    SqlCommand com = new SqlCommand("sp_Customer");
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@Search", Search == "" ? null : Search);
        //    com.Parameters.AddWithValue("@PageSize", pageSize);
        //    com.Parameters.AddWithValue("@PageNumber", page);
        //    com.Parameters.AddWithValue("@Action", "SELECT");
        //    DataSet dataSet = ConnectionClass.getDataSet(com);
        //    if (dataSet != null)
        //    {
        //        DataTable dt = dataSet.Tables[0];
        //        DataTable dtCount = dataSet.Tables[1];
        //        recordsTotal = Convert.ToInt32(dtCount.Rows[0][0]);
        //        var data = dt.AsEnumerable().Select(row => row.ItemArray).ToList();
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
        //    }

        //}
    }
}