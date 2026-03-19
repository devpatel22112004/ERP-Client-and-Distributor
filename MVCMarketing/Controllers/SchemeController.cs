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
    //[SessionTimeOut]
    public class SchemeController : Controller
    {
        // GET: Scheme
        public ActionResult List()
        {
            return View();
        }

        public ActionResult InfoScheme()
        {
            return PartialView();
        }

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult Info(string id)
        {
            ViewBag.JsonSchemeId = id;
            return PartialView();
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Scheme");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeId", id);
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
                SqlCommand com = new SqlCommand("sp_Scheme");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@StartDate", formCollection["txtStartDate"]);
                com.Parameters.AddWithValue("@EndDate", formCollection["txtEndDate"]);
                com.Parameters.AddWithValue("@Label", formCollection["txtLabel"]);
                com.Parameters.AddWithValue("@GracePeriod", formCollection["txtGracePeriod"]);
                com.Parameters.AddWithValue("@Description", formCollection["txtDescription"]);
                com.Parameters.AddWithValue("@TermsAndCondition", formCollection["txtTOC"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                //return Json(ConnectionClass.DML(com));
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public ActionResult Print(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Scheme");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeId", id);
                com.Parameters.AddWithValue("@Action", "SCHEME-INVOICE");
                DataSet ds = ConnectionClass.getDataSet(com);
                if (ds != null)
                {
                    string JSONString = string.Empty;

                    JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    ViewBag.JsonDataHead = JSONString;


                    DataTable newItemsTable = ds.Tables[1].Copy();
                    JSONString = JsonConvert.SerializeObject(newItemsTable);
                    ViewBag.JsonDataBody = JSONString;

                    /*JSONString = JsonConvert.SerializeObject(ds.Tables[2]);
                    ViewBag.JsonDataCalculation = JSONString;*/
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoadDataScheme()
        {
            SqlCommand com = new SqlCommand("sp_Home");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Action", "SCHEME");
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

        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var StartDate = pagination.data.columns[0].search.value;
            var EndDate = pagination.data.columns[1].search.value;
            var Label = pagination.data.columns[2].search.value;
            var Description = pagination.data.columns[3].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Scheme");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@StartDate", StartDate == "" ? null : StartDate);
            com.Parameters.AddWithValue("@EndDate", EndDate == "" ? null : EndDate);
            com.Parameters.AddWithValue("@Label", Label == "" ? null : Label);
            com.Parameters.AddWithValue("@Description", Description == "" ? null : Description);
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
    }
}