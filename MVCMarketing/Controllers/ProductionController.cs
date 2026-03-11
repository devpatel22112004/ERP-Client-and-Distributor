using MVCMarketing.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using MVCMarketing.Common;

namespace MVCMarketing.Controllers
{
    [SessionTimeout]
    public class ProductionController : Controller
    {
        // GET: Production
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Production");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProductionId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    ViewBag.JsonData = JSONString;
                }
            }
            return View();
        }

        public JsonResult EditItem(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_ProductionItem");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProductionItemId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    return Json(JSONString, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_Production");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProductionId", formCollection["hdProductionId"]);
                com.Parameters.AddWithValue("@EmployeeId", formCollection["ddEmployee"]);
                com.Parameters.AddWithValue("@ProductionDate", formCollection["txtProductionDate"]);
                com.Parameters.AddWithValue("@StartTime", formCollection["txtStartDate"]);
                com.Parameters.AddWithValue("@StopTime", formCollection["txtEndDate"]);
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

        [HttpPost]
        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var ProductionDate = pagination.data.columns[0].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Production");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@ProductionDate", ProductionDate == "" ? null : ProductionDate);
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
        public JsonResult InsertItem(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_ProductionItem");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProductionItemId", formCollection["hdProductionItemId"]);
                com.Parameters.AddWithValue("@ProductionId", formCollection["hdProductionId2"]);
                com.Parameters.AddWithValue("@ItemId", formCollection["hdDCItemsId"]);
                com.Parameters.AddWithValue("@Qty", formCollection["txtQuantity"]);
                com.Parameters.AddWithValue("@Type", formCollection["ddType"]);
                com.Parameters.AddWithValue("@Note", formCollection["txtNote"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                //return Json(ConnectionClass.DML(com));
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult LoadDataItem(string id)
        {
            SqlCommand com = new SqlCommand("sp_ProductionItem");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ProductionId", id);
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