using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MVCMarketing.Models;
using Newtonsoft.Json;

namespace MVCMarketing.Controllers
{
    //[SessionTimeOut]
    public class SchemeTargetController : Controller
    {
        // GET: SchemeTarget
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            ViewBag.JsonSchemeId = id;
            return PartialView();
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_SchemeTarget");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeTargetId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    ViewBag.JsonData = JSONString;
                }
            }
            //ViewBag.JsonSchemeId = id;
            return PartialView();
        }

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_SchemeTarget");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeTargetId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@SchemeId", formCollection["hdSchemeId"]);
                com.Parameters.AddWithValue("@SchemeAmount", formCollection["txtAmount"]);
                com.Parameters.AddWithValue("@TargetAmount", formCollection["txtTargetAmount"]);
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
            SqlCommand com = new SqlCommand("sp_SchemeTarget");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SchemeTargetId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
        }

        [HttpPost]
        public ActionResult LoadData(string id)
        {
            SqlCommand com = new SqlCommand("sp_SchemeTarget");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SchemeId", id);
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

        //[HttpPost]
        //public ActionResult LoadData()
        //{
        //    //jQuery DataTables Param
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    //Find paging info
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();

        //    //find search columns info
        //    var Scheme = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
        //    var Tag = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt16(start) : 0;
        //    int recordsTotal = 0;

        //    int page = (skip / pageSize) + 1;

        //    SqlCommand com = new SqlCommand("sp_SchemeTarget");
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@Label", Scheme == "" ? null : Scheme);
        //    com.Parameters.AddWithValue("@SchemeTag", Tag == "" ? null : Tag);
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