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
    public class MatrixGroupController : Controller
    {
        // GET: MatrixGroup
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_MatrixGroup");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MatrixGroupId", id);
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
                SqlCommand com = new SqlCommand("sp_MatrixGroup");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MatrixGroupId", formCollection["hdMatrixGroupId"]);
                com.Parameters.AddWithValue("@Groupname", formCollection["txtGroup"]);
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

        [HttpPost]
        public ActionResult LoadMatrixGroupData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var Group = pagination.data.columns[0].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_MatrixGroup");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@Groupname", Group == "" ? null : Group);
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
        //public ActionResult LoadMatrixGroupData()
        //{
        //    //jQuery DataTables Param
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    //Find paging info
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();


        //    //find search columns info
        //    var Group = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt16(start) : 0;
        //    int recordsTotal = 0;

        //    int page = (skip / pageSize) + 1;

        //    SqlCommand com = new SqlCommand("sp_MatrixGroup");
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@Groupname", Group == "" ? null : Group);
        //    com.Parameters.AddWithValue("@PageSize", pageSize);
        //    com.Parameters.AddWithValue("@PageNumber", page);
        //    com.Parameters.AddWithValue("@Action", "SELECT");
        //    DataSet dataSet = ConnectionClass.getDataSet(com);
        //    if (dataSet != null)
        //    {
        //        DataTable dt = dataSet.Tables[0];
        //        DataTable dtCount = dataSet.Tables[1];
        //        //convert datatable to list using LINQ. Input datatable is "dt", returning list of "name:value" tuples
        //        /*var data = dt.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
        //                    .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //                   ).ToDictionary(z => z.Key, z => z.Value)
        //            ).ToList();*/

        //        recordsTotal = Convert.ToInt32(dtCount.Rows[0][0]);
        //        var data = dt.AsEnumerable().Select(row => row.ItemArray).ToList();
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public JsonResult Delete(int id)
        {
            SqlCommand com = new SqlCommand("sp_MatrixGroup");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@MatrixGroupId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
        }
    }
}