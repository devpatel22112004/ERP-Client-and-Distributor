using MVCMarketing.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MVCMarketing.Controllers
{
   // [SessionTimeOut]
    public class TakeOrderController : Controller
    {
        // GET: TakeOrder
        public ActionResult List()
        {
            return PartialView();
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Visit");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorId", id);
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

        [HttpPost]
        public ActionResult LoadData(string Name, string Area, string Pincode, string CityId)
        {
            SqlCommand com = new SqlCommand("sp_TakeOrder");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", Name == "" ? null : Name);
            com.Parameters.AddWithValue("@Area", Area == "" ? null : Area);
            com.Parameters.AddWithValue("@Pincode", Pincode == "" ? null : Pincode);
            com.Parameters.AddWithValue("@CityId", CityId == "" ? null : CityId);
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