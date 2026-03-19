using MVCMarketing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMarketing.Controllers
{
    public class ClientAreaController : Controller
    {
        // GET: ClientArea
        public ActionResult Dashboard(string id)
        {
            ViewBag.TokenId = id;
            return View();
        }

        [HttpPost]
        public ActionResult LoadDataDeliverClient(string token)
        {
            var recordsTotal = 0;
            SqlCommand com = new SqlCommand("sp_ClientDashboard");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Token", token);
            com.Parameters.AddWithValue("@Action", "DESHBOARD-DELIVER-CLIENT");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                recordsTotal = dt.Rows.Count;
                var data = JsonConvert.SerializeObject(dt);
                return Json(new { recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LoadDataPendingAmount(string token)
        {
            var recordsTotal = 0;
            SqlCommand com = new SqlCommand("sp_ClientDashboard");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Token", token);
            com.Parameters.AddWithValue("@Action", "DESHBOARD-CLIENT-PAYMENT");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                recordsTotal = dt.Rows.Count;
                var data = JsonConvert.SerializeObject(dt);
                return Json(new { recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LoadDataClientAmount(string token)
        {
            var recordsTotal = 0;
            SqlCommand com = new SqlCommand("sp_ClientDashboard");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Token", token);
            com.Parameters.AddWithValue("@Action", "DESHBOARD-CLIENT-PAYMENT");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                recordsTotal = dt.Rows.Count;
                var data = JsonConvert.SerializeObject(dt);
                return Json(new { recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LoadDataDealerInformation(string token)
        {
            SqlCommand com = new SqlCommand("sp_ClientDashboard");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Token", token);
            com.Parameters.AddWithValue("@Action", "DESHBOARD-DISTRIBUTOR");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                var data = JsonConvert.SerializeObject(dt);
                return Json(data);
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public ActionResult LoadDataClientPayment(string token)
        {
            SqlCommand com = new SqlCommand("sp_ClientDashboard");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Token", token);
            com.Parameters.AddWithValue("@Action", "DESHBOARD-CLIENT-PAYMENT");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                var data = JsonConvert.SerializeObject(dt);
                return Json(data);
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public ActionResult LoadDataLedger(string token, string StartDate, string EndDate)
        {
            var recordsTotal = 0;
            var openingBalance = "0";
            SqlCommand com = new SqlCommand("sp_ClientDashboard");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Token", token);
            com.Parameters.AddWithValue("@StartDate", StartDate == "" ? null : StartDate);
            com.Parameters.AddWithValue("@EndDate", EndDate == "" ? null : EndDate);
            com.Parameters.AddWithValue("@Action", "DESHBOARD-PAYMENT");
            DataSet ds = ConnectionClass.getDataSet(com);
            if (ds != null && ds.Tables.Count > 0)
            {
                recordsTotal = ds.Tables[0].Rows.Count;
                var data = JsonConvert.SerializeObject(ds.Tables[0]);
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    openingBalance = ds.Tables[1].Rows[0][0].ToString();
                }
                return Json(new { recordsTotal = recordsTotal, data = data, openingBalance = openingBalance }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsTotal = recordsTotal, data = "", openingBalance = openingBalance }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}