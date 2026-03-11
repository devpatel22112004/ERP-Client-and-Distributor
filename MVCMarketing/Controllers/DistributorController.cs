using MVCMarketing.Common;
using MVCMarketing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MVCMarketing.Controllers
{
    [SessionTimeout]
    public class DistributorController : Controller
    {
        // GET: Distributor
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult Info(string id)
        {
            ViewBag.JsonDistributorId = id;
            return PartialView();
        }

        /*public ActionResult Adjustment(string Did)
         {
             ViewBag.JsonDistributorId = Did;
             return PartialView();
         }*/
        public ActionResult Adjustment(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_DistributorOpeningAdjustment");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorId", id);
                com.Parameters.AddWithValue("@Action", "DEALER-AMOUNT");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    ViewBag.JSONData = JsonConvert.SerializeObject(dt);
                }
            }
            return PartialView();
        }

        public JsonResult EditItem(string id)
        {
            string JSONString = string.Empty;
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_DistributorOpeningAdjustment");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorOpeningAdjustmentId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    JSONString = JsonConvert.SerializeObject(dt);
                }
            }
            return Json(JSONString);
        }


        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Distributor");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorId", id);
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

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_Distributor");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@Name", formCollection["txtName"]);
                com.Parameters.AddWithValue("@Address", formCollection["txtAddress"]);
                com.Parameters.AddWithValue("@Shipping", formCollection["txtShippingAddress"]);
                com.Parameters.AddWithValue("@CityId", formCollection["ddCity"]);
                com.Parameters.AddWithValue("@StateId", formCollection["ddState"]);
                com.Parameters.AddWithValue("@Pincode", formCollection["txtPincode"]);
                com.Parameters.AddWithValue("@PersonName1", formCollection["txtPersonName"]);
                com.Parameters.AddWithValue("@Contact1", formCollection["txtContact1"]);
                com.Parameters.AddWithValue("@Contact2", formCollection["txtContact2"]);
                com.Parameters.AddWithValue("@Email", formCollection["txtEmail"]);
                com.Parameters.AddWithValue("@GST", formCollection["txtGST"]);
                com.Parameters.AddWithValue("@PAN", formCollection["txtPAN"]);
                com.Parameters.AddWithValue("@Status", formCollection["ddStatus"]);
                com.Parameters.AddWithValue("@Area", formCollection["txtArea"]);
                com.Parameters.AddWithValue("@TaxMode", formCollection["ddTax"]);
                com.Parameters.AddWithValue("@DueDays", formCollection["txtDuedays"]);
                com.Parameters.AddWithValue("@SalesEmployeeId", formCollection["ddSalesAssignTo"]);
                com.Parameters.AddWithValue("@CreditLimitAmount", formCollection["txtCreditLimit"]);
                com.Parameters.AddWithValue("@OpeningBalance", formCollection["txtOpeningBalance"]);
                com.Parameters.AddWithValue("@UserId", formCollection["txtUserId"]);
                com.Parameters.AddWithValue("@Password", formCollection["txtPassword"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                //return Json(ConnectionClass.DML(com));
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
                SqlCommand com = new SqlCommand("sp_DistributorOpeningAdjustment");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorId", formCollection["hdDistributorId"]);
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

        public JsonResult InsertCredit(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_DistributorCredit");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorCreditId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["hdDistributorId"]);
                com.Parameters.AddWithValue("@CreditDay", formCollection["txtCredit"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                //return Json(ConnectionClass.DML(com));
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult DeletePayment(int id)
        {
            SqlCommand com = new SqlCommand("sp_DistributorOpeningAdjustment");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorOpeningAdjustmentId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
        }

        [HttpPost]
        public ActionResult LoadDataAmount(string id)
        {
            var recordsTotal = 0;
            SqlCommand com = new SqlCommand("sp_DistributorOpeningAdjustment");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorId", id);
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
        public ActionResult LoadDataDiscount(string id)
        {
            SqlCommand com = new SqlCommand("sp_DiscountStructureItem");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorId", id);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataTable dt = ConnectionClass.getDataTable(com);
            var data = dt.AsEnumerable().Select(row => row.ItemArray).ToList();
            var recordsTotal = dt.Rows.Count;
            return Json(new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);


            // return Json(new { recordsFiltered = "0", recordsTotal = "0", data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var Name = pagination.data.columns[0].search.value;
            var City = pagination.data.columns[1].search.value;
            var StateId = pagination.data.columns[2].search.value;
            var Status = pagination.data.columns[3].search.value;
            var Area = pagination.data.columns[4].search.value;
            var Code = pagination.data.columns[5].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Distributor");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@Name", Name == "" ? null : Name);
            com.Parameters.AddWithValue("@CityId", City == "" ? null : City);
            com.Parameters.AddWithValue("@StateId", StateId == "" ? null : StateId);
            com.Parameters.AddWithValue("@Status", Status == "" ? null : Status);
            com.Parameters.AddWithValue("@Area", Area == "" ? null : Area);
            com.Parameters.AddWithValue("@Code", Code == "" ? null : Code);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet dataSet = ConnectionClass.getDataSet(com);
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[1];
                DataTable dtCount = dataSet.Tables[0];
                recordsTotal = Convert.ToInt32(dtCount.Rows[0][0]);

                var data = dt.AsEnumerable().Select(row => row.ItemArray).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult LoadDataDistributorDiscountStructure(string id)
        {
            SqlCommand com = new SqlCommand("sp_DistributorDiscountStructure");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorId", id);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                var data = dt.AsEnumerable().Select(row => row.ItemArray).ToList();
                var recordsTotal = dt.Rows.Count;
                return Json(new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsFiltered = "0", recordsTotal = "0", data = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}