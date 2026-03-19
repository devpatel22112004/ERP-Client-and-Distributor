using MVCMarketing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMarketing.Controllers
{
    public class ClientPriceTagController : Controller
    {
        // GET: ClientPriceTag
        public ActionResult ClientPriceForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadDataClientPrice(string id)
        {
            string jArrayData = "";
            SqlCommand com = new SqlCommand("sp_StoreOutward");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SalesId", id);
            com.Parameters.AddWithValue("@Action", "SELECT-LIST-ITEM");
            jArrayData = ConnectionClass.GetJsonData(com);
            // Return the data as a JsonResult
            return Json(new { data = jArrayData }, JsonRequestBehavior.AllowGet);
        }
    }
}