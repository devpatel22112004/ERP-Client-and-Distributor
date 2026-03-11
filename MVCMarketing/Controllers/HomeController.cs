using MVCMarketing.Common;
using MVCMarketing.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVCMarketing.Controllers
{
    [SessionTimeout]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SqlCommand com = new SqlCommand("sp_Home");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserBy", Session["UserId"].ToString());
            com.Parameters.AddWithValue("@Action", "DESHBOARD");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt);
                ViewBag.JsonData = JSONString;
            }
            return View();
        }

        public ActionResult SalesFieldDashboard()
        {
            SqlCommand com = new SqlCommand("sp_Home");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserBy", Session["UserId"].ToString());
            com.Parameters.AddWithValue("@Action", "DESHBOARD");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt);
                ViewBag.JsonData = JSONString;
            }
            return View();
        }

        public ActionResult SalesOfficeDashboard()
        {
            SqlCommand com = new SqlCommand("sp_Home");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserBy", Session["UserId"].ToString());
            com.Parameters.AddWithValue("@Action", "DESHBOARD");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt);
                ViewBag.JsonData = JSONString;
            }
            return View();
        }
        public ActionResult DistributorDashboard()
        {
            SqlCommand com = new SqlCommand("sp_Home");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserBy", Session["UserId"].ToString());
            com.Parameters.AddWithValue("@Action", "DESHBOARD");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt);
                ViewBag.JsonData = JSONString;
            }
            return View();
        }

        public ActionResult TranspotationDashboard()
        {
            SqlCommand com = new SqlCommand("sp_Home");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserBy", Session["UserId"].ToString());
            com.Parameters.AddWithValue("@Action", "DESHBOARD");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt);
                ViewBag.JsonData = JSONString;
            }
            return View();
        }
        public JsonResult LoadEvent(string start, string end, string emp, string ScheduleTag)
        {
            var Desingnation = Session["UserDesignation"].ToString().Equals("ADMIN") ? "True" : "False";
            string JSONString = string.Empty;
            SqlCommand com = new SqlCommand("sp_Schedule");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserBy", emp == "" ? Session["UserId"].ToString() : emp);
            //com.Parameters.AddWithValue("@UserBy", Session["UserId"].ToString());
            com.Parameters.AddWithValue("@ScheduleTag", ScheduleTag == "" ? null : ScheduleTag);
            com.Parameters.AddWithValue("@StartDate", start);
            com.Parameters.AddWithValue("@EndDate", end);
            com.Parameters.AddWithValue("@Action", "SELECT-CALENDAR");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                JSONString = JsonConvert.SerializeObject(dt);

            }
            return Json(new { data = JSONString, desingnation = Desingnation }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadDataItem()
        {
            SqlCommand com = new SqlCommand("sp_Home");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Action", "DESHBOARD-ITEM");
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