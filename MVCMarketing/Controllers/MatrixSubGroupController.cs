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
    public class MatrixSubGroupController : Controller
    {
        // GET: MatrixSubGroup
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_MatrixSubGroup");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MatrixSubGroupId", id);
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
                SqlCommand com = new SqlCommand("sp_MatrixSubGroup");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MatrixSubGroupId", formCollection["hdMatrixSubGroupId"]);
                com.Parameters.AddWithValue("@MatrixGroupId", formCollection["ddMatrixGroup"]);
                com.Parameters.AddWithValue("@SubGroupname", formCollection["txtSubGroup"]);
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
        public ActionResult LoadMatrixSubGroupData()
        {
            //jQuery DataTables Param
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();


            //find search columns info
            var SubGroup = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize) + 1;

            SqlCommand com = new SqlCommand("sp_MatrixSubGroup");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SubGroupname", SubGroup == "" ? null : SubGroup);
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet dataSet = ConnectionClass.getDataSet(com);
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0];
                DataTable dtCount = dataSet.Tables[1];
                //convert datatable to list using LINQ. Input datatable is "dt", returning list of "name:value" tuples
                /*var data = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                            .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                           ).ToDictionary(z => z.Key, z => z.Value)
                    ).ToList();*/

                recordsTotal = Convert.ToInt32(dtCount.Rows[0][0]);
                var data = dt.AsEnumerable().Select(row => row.ItemArray).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int id)
        {
            SqlCommand com = new SqlCommand("sp_MatrixSubGroup");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@MatrixSubGroupId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
        }
    }
}