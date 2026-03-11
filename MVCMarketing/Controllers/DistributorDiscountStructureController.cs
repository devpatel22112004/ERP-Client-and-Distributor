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
    public class DistributorDiscountStructureController : Controller
    {
        // GET: DistributorDiscountStructure

        public ActionResult Form(string did, string sid)
        {
            //ViewBag.JsonDistributorId1 = id1;
            if (did != null || sid != null)
            {
                SqlCommand com = new SqlCommand("sp_DistributorDiscountStructure");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorId", did);
                com.Parameters.AddWithValue("@DiscountStructureId", sid);
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

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_DistributorDiscountStructure");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorDiscountStructureId", id);
                com.Parameters.AddWithValue("@Action", "EDIT");
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
                SqlCommand com = new SqlCommand("sp_DistributorDiscountStructure");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorDiscountStructureId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["hdDistributorId"]);
                com.Parameters.AddWithValue("@DiscountStructureId", formCollection["hdDiscountStructureId"]);
                com.Parameters.AddWithValue("@EffectedDate", formCollection["txtEffectedDate"]);
                com.Parameters.AddWithValue("@OverDate", formCollection["txtOverDate"]);
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

        public JsonResult Delete(string id)
        {
            SqlCommand com = new SqlCommand("sp_DistributorDiscountStructure");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorDiscountStructureId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
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
            var DiscountStructureTitle = pagination.data.columns[1].search.value;
            var EffectedDate = pagination.data.columns[2].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_DistributorDiscountStructure");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@Name", Name == "" ? null : Name);
            com.Parameters.AddWithValue("@DiscountStructureTitle", DiscountStructureTitle == "" ? null : DiscountStructureTitle);
            com.Parameters.AddWithValue("@EffectedDate", EffectedDate == "" ? null : EffectedDate);
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