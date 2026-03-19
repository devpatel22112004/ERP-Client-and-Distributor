using MVCMarketing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MVCMarketing.Controllers
{
    public class ClientRouteController : Controller
    {
        //[SessionTimeOut]
        // GET: ClientRoute
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult Info(string id)
        {
            ViewBag.JsonClientRouteId = id;
            return PartialView();
        }

        [HttpPost]
        public JsonResult InsertClient(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_AssignClient");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@AssignClientId", formCollection["hdId"] == "null" ? null : formCollection["hdId"]);
                com.Parameters.AddWithValue("@ClientRouteId", formCollection["hdClientRouteId"]);
                com.Parameters.AddWithValue("@CustomerId", formCollection["hdCustomerId"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        [HttpPost]
        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_ClientRoute");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ClientRouteId", formCollection["hdClientRouteId"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["ddDealer"]);
                com.Parameters.AddWithValue("@WeekDay", formCollection["ddWeekDay"]);
                com.Parameters.AddWithValue("@RouteName", formCollection["txtRoute"]);
                com.Parameters.AddWithValue("@RouteNumber", formCollection["txtRouteNum"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult LoadDataAssignClient(string RouteId, string Name)
        {
            SqlCommand com = new SqlCommand("sp_AssignClient");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CustomerName", Name == "" ? null : Name);
            com.Parameters.AddWithValue("@ClientRouteId", RouteId);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet ds = ConnectionClass.getDataSet(com);
            if (ds != null)
            {
                var data1 = JsonConvert.SerializeObject(ds.Tables[0]);
                var data2 = JsonConvert.SerializeObject(ds.Tables[1]);
                //var data = dt.AsEnumerable().Select(row => new { col1 = row[0].ToString(), col2 = row[1].ToString(), col3 = row[2].ToString(), col4 = row[3].ToString(), col5 = row[4].ToString(), col6 = row[5], col7 = row[6] });//, col5 = row[4]
                return Json(new { recordsFiltered = "", recordsTotal = "", data1 = data1, data2 = data2 },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { recordsFiltered = "", recordsTotal = "", data = "" },
                    JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]

        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var WeekDay = pagination.data.columns[0].search.value;
            var RouteName = pagination.data.columns[1].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_ClientRoute");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@WeekDay", WeekDay == "" ? null : WeekDay);
            com.Parameters.AddWithValue("@RouteName", RouteName == "" ? null : RouteName);
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
    }
}