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
    public class DistributorItemStockController : Controller
    {
        // GET: DistributorItemStock
        public ActionResult Form(string id)
        {
            ViewBag.JsonDistributorId = id;
            return PartialView();
        }

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_DistributorItemStock");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorItemStockId", formCollection["hdDistributorItemStockId"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["hdDistributorId"]);
                com.Parameters.AddWithValue("@ItemId", formCollection["hdDCItemsId"]);
                com.Parameters.AddWithValue("@EmptyQty", formCollection["txtEmptyQty"]);
                com.Parameters.AddWithValue("@RefillQty", formCollection["txtRefillQty"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult DeleteDistributorItemStock(int id)
        {
            SqlCommand com = new SqlCommand("sp_DistributorItemStock");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorItemStockId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
        }

        [HttpPost]
        public ActionResult LoadDataStock(string id)
        {
            var recordsTotal = 0;
            SqlCommand com = new SqlCommand("sp_DistributorItemStock");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorId", id);
            com.Parameters.AddWithValue("@Action", "SELECT");
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
    }
}