using MVCMarketing.Common;
using MVCMarketing.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MVCMarketing.Controllers
{
    [SessionTimeout]
    public class VendorController : Controller
    {
        // GET: Vendor
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Supplier");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SupplierId", id);
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

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult List()
        {
            return View();
        }

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_Supplier");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SupplierId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@Name", formCollection["txtVendorName"]);
                com.Parameters.AddWithValue("@VendorCode", formCollection["txtVendorCode"]);
                com.Parameters.AddWithValue("@Address", formCollection["txtAddress"]);
                com.Parameters.AddWithValue("@City", formCollection["txtCity"]);
                com.Parameters.AddWithValue("@State", formCollection["ddState"]);
                com.Parameters.AddWithValue("@Pincode", formCollection["txtPostCode"]);
                com.Parameters.AddWithValue("@PersonName1", formCollection["txtName1"]);
                com.Parameters.AddWithValue("@Contact1", formCollection["txtContact1"]);
                com.Parameters.AddWithValue("@Contact2", formCollection["txtContact2"]);
                com.Parameters.AddWithValue("@Email", formCollection["txtEmail1"]);
                com.Parameters.AddWithValue("@GST", formCollection["txtGstNo"]);
                com.Parameters.AddWithValue("@PAN", formCollection["txtPAN"]);
                com.Parameters.AddWithValue("@Area", formCollection["txtArea"]);
                com.Parameters.AddWithValue("@GSTClass", formCollection["ddGSTType"]);
                com.Parameters.AddWithValue("@BankACNo", formCollection["txtbankAc"]);
                com.Parameters.AddWithValue("@BankIFSC", formCollection["txtbankIFSC"]);
                com.Parameters.AddWithValue("@BankBranch", formCollection["txtbankbranchname"]);
                com.Parameters.AddWithValue("@DueDays", formCollection["txtDuedays"]);
                com.Parameters.AddWithValue("@AssignVendorId", formCollection["ddVendorAssignTo"]);
                com.Parameters.AddWithValue("@CreditLimitAmount", formCollection["txtCreditLimit"]);
                com.Parameters.AddWithValue("@OpeningBalance", formCollection["txtOpeningBalance"]);
                com.Parameters.AddWithValue("@Status", formCollection["ddStatus"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                //return Json(ConnectionClass.DML(com));
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult Delete(int id)
        {
            SqlCommand com = new SqlCommand("sp_Supplier");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SupplierId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
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
            var VendorCode = pagination.data.columns[1].search.value;
            var Contact1 = pagination.data.columns[2].search.value;
            var City = pagination.data.columns[3].search.value;
            //var State = pagination.data.columns[4].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Supplier");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@Name", Name == "" ? null : Name);
            com.Parameters.AddWithValue("@VendorCode", VendorCode == "" ? null : VendorCode);
            com.Parameters.AddWithValue("@Contact1", Contact1 == "" ? null : Contact1);
            com.Parameters.AddWithValue("@City", City == "" ? null : City);
            //com.Parameters.AddWithValue("@State", State == "" ? null : State);
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
        //[HttpPost]
        //public ActionResult LoadData()
        //{
        //    //jQuery DataTables Param
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    //Find paging info
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();

        //    //find search columns info
        //    var Name = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
        //    var Contact1 = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
        //    var City = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
        //    var State = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt16(start) : 0;
        //    int recordsTotal = 0;

        //    int page = (skip / pageSize) + 1;

        //    SqlCommand com = new SqlCommand("sp_Supplier");
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@Name", Name == "" ? null : Name);
        //    com.Parameters.AddWithValue("@Contact1", Contact1 == "" ? null : Contact1);
        //    com.Parameters.AddWithValue("@City", City == "" ? null : City);
        //    com.Parameters.AddWithValue("@State", State == "" ? null : State);
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