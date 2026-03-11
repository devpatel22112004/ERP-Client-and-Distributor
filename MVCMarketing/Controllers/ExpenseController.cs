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
    public class ExpenseController : Controller
    {
        // GET: Expense
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Expense");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ExpenseId", id);
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
                SqlCommand com = new SqlCommand("sp_Expense");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ExpenseId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@ExpenseCategoryId", formCollection["ddCategory"]);
                com.Parameters.AddWithValue("@ExpenseSubId", formCollection["ddSubCategory"]);
                com.Parameters.AddWithValue("@EmployeeId", formCollection["ddEmployee"]);
                com.Parameters.AddWithValue("@Date", formCollection["txtDate"]);
                com.Parameters.AddWithValue("@PaymentMode", formCollection["ddPaymentMode"]);
                com.Parameters.AddWithValue("@Amount", formCollection["txtAmount"]);
                com.Parameters.AddWithValue("@ChequeNo", formCollection["txtChequeNo"]);
                com.Parameters.AddWithValue("@ChequeName", formCollection["txtChequeName"]);
                com.Parameters.AddWithValue("@Remark", formCollection["txtRemark"]);
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
            SqlCommand com = new SqlCommand("sp_Expense");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ExpenseId", id);
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

            //find search columns info
            var Date = pagination.data.columns[0].search.value;
            var ExpenseCategoryId = pagination.data.columns[1].search.value;
            var ExpenseSubCategoryId = pagination.data.columns[2].search.value;
            var PaymentMode = pagination.data.columns[3].search.value;
            var EmployeeId = pagination.data.columns[4].search.value;
            //var Status = pagination.data.columns[5].search.value;
            //Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Expense");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@Date", Date == "" ? null : Date);
            com.Parameters.AddWithValue("@ExpenseCategoryId", ExpenseCategoryId == "" ? null : ExpenseCategoryId);
            com.Parameters.AddWithValue("@ExpenseSubId", ExpenseSubCategoryId == "" ? null : ExpenseSubCategoryId);
            com.Parameters.AddWithValue("@PaymentMode", PaymentMode == "" ? null : PaymentMode);
            com.Parameters.AddWithValue("@EmployeeId", EmployeeId == "" ? null : EmployeeId);
            //com.Parameters.AddWithValue("@Status", Status == "" ? null : Status);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet dataSet = ConnectionClass.getDataSet(com);
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[1];
                DataTable dtCount = dataSet.Tables[0];
                recordsTotal = Convert.ToInt32(dtCount.Rows[0][0]);
                DataTable newTable = dt.Copy();
                var data = newTable.AsEnumerable().Select(row => row.ItemArray).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }
        }        
    }
}