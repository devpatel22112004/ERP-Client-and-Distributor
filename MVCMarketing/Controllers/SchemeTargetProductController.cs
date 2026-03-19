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
    //[SessionTimeOut]
    public class SchemeTargetProductController : Controller
    {
        // GET: SchemeTargetProduct
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
                SqlCommand com = new SqlCommand("sp_SchemeTargetProduct");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeId", id);
                com.Parameters.AddWithValue("@Action", "SELECT-SCHEME");
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
        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_SchemeTargetProduct");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeTargetProductId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@SchemeId", formCollection["hdSchemeId"]);
                // com.Parameters.AddWithValue("@SchemeTargetId", formCollection["hdSchemeTargetId"]);
                com.Parameters.AddWithValue("@CategoryId", formCollection["ddCategory"]);
                com.Parameters.AddWithValue("@SubCategoryIds", formCollection["hdSubCategoryIds"]);
                com.Parameters.AddWithValue("@ItemId", formCollection["txtItemId"]);
                com.Parameters.AddWithValue("@Action", "INSERT-ITEM");
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
            SqlCommand com = new SqlCommand("sp_SchemeTargetProduct");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SchemeTargetProductId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            //return Json(ConnectionClass.DML(com));
            return ConnectionClass.DML(com);
        }

        /* [HttpPost]
         public ActionResult LoadData()
         {
             //jQuery DataTables Param
             var draw = Request.Form.GetValues("draw").FirstOrDefault();
             //Find paging info
             var start = Request.Form.GetValues("start").FirstOrDefault();
             var length = Request.Form.GetValues("length").FirstOrDefault();


             //find search columns info
             var Label = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
             var Item = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();

             int pageSize = length != null ? Convert.ToInt32(length) : 0;
             int skip = start != null ? Convert.ToInt16(start) : 0;
             int recordsTotal = 0;

             int page = (skip / pageSize) + 1;

             SqlCommand com = new SqlCommand("sp_SchemeTargetProduct");
             com.CommandType = CommandType.StoredProcedure;
             com.Parameters.AddWithValue("@CategoryName", Label == "" ? null : Label);
             com.Parameters.AddWithValue("@ItemId", Item == "" ? null : Item);
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
         }*/

        [HttpPost]
        public ActionResult LoadData(string id)
        {
            SqlCommand com = new SqlCommand("sp_SchemeTargetProduct");
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

    }
}