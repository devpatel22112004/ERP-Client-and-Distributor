using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MVCMarketing.Models;

namespace MVCMarketing.Controllers
{
    public class EmployeeDistributorProfileController : Controller
    {
        // GET: EmployeeDistributorProfile
        public ActionResult List()
        {
            return View();
        }

        public ActionResult ViewInfo(string id)
        {
            ViewBag.JsonEmployeeId = id;
            return PartialView();
        }

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_EmployeeDistributorProfile");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@EmployeeDistributorProfileId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["txtDistributorId"]);
                com.Parameters.AddWithValue("@EmployeeId", formCollection["hdEmployeeId"]);
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
        public ActionResult LoadData()
        {
            //jQuery DataTables Param
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //find search columns info
            var EmpServiceEng = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var EmpDesignation = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Status = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize) + 1;

            SqlCommand com = new SqlCommand("sp_EmployeeDistributorProfile");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@EmployeeName", EmpServiceEng == "" ? null : EmpServiceEng);
            com.Parameters.AddWithValue("@DesignationId", EmpDesignation == "" ? null : EmpDesignation);
            com.Parameters.AddWithValue("@Status", Status == "" ? null : Status);
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet dataSet = ConnectionClass.getDataSet(com);
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0];
                DataTable dtCount = dataSet.Tables[1];
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
        public ActionResult LoadDataDistributor(string id)
        {
            SqlCommand com = new SqlCommand("sp_EmployeeDistributorProfile");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@EmployeeId", id);
            com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
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