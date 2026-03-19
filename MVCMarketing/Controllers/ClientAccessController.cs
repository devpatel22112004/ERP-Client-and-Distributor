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
    public class ClientAccessController : Controller
    {        
        // GET: ClientAccess
        public ActionResult Index(string token)
        {            
            if (!string.IsNullOrEmpty(token))
            {
                SqlCommand com = new SqlCommand("sp_Customer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Token", token);
                com.Parameters.AddWithValue("@Action", "LOGIN");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        return RedirectToAction("Dashboard", "Message", new { message = "Oops! The client QR code functionality is not working as expected." });
                    }
                    else
                    {
                        var record = dt.AsEnumerable().Select(row => new { col1 = row[0], col2 = row[1], col3 = row[2], col4 = row[3]});
                        Session["ClientId"] = record.FirstOrDefault().col1;
                        Session["ClientSort"] = record.FirstOrDefault().col2;
                        Session["Client"] = record.FirstOrDefault().col3;
                    }
                }
                return RedirectToAction("Dashboard", "ClientArea", new { id = token });
            }
            return View();
        }

        [HttpPost]
        public JsonResult Loginprocess(string Password, string UserId)
        {
            if (Password != null)
            {
                SqlCommand com = new SqlCommand("sp_Employee");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Password", Password);
                com.Parameters.AddWithValue("@UserId", UserId);
                com.Parameters.AddWithValue("@Action", "LOGIN");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        return Json(new { success = false, message = "Oops! User id or password does not match." });
                    }
                    var record = dt.AsEnumerable().Select(row => new { col1 = row[0], col2 = row[1], col3 = row[2], col4 = row[3], col5 = row[4] });
                    if (record != null)
                    {
                        if (record.FirstOrDefault().col5.ToString().ToUpper() != "TRUE")
                        {
                            return Json(new { success = false, message = "Account is locked" });
                        }

                        Session["UserId"] = record.FirstOrDefault().col1;
                        Session["UserSort"] = record.FirstOrDefault().col2;

                        string Designation = record.FirstOrDefault().col3.ToString();
                        Session["UserDesignation"] = Designation;
                        Session["User"] = record.FirstOrDefault().col4;
                        //  var Menu =  record.FirstOrDefault().col6.ToString();
                        // Session["Menu"] = Menu;

                        if (Designation.ToUpper().Equals("ADMIN"))
                        {
                            Session["Layout"] = "~/Views/Shared/_Layout.cshtml";
                            return Json(new { success = true, url = Url.Action("Index", "Home") });
                        }
                    }
                }
            }
            return Json(new { success = false, message = "Oops! User id or password does not match." });
        }

        public ActionResult Message(string message)
        {
            ViewBag.LoginMessage = message; // you can use this in your view
            return View();
        }
        /*
        
        public ActionResult Dashboard(string id)
        {
            //ViewBag.JsonToken = id; // you can use this in your view
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
        */
    }
}