using MVCMarketing.Common;
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
    [SessionTimeout]
    public class TransportationController : Controller
    {
        // GET: Transportation
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Info(string id)
        {
            ViewBag.JsonTransportationId = id;
            return PartialView();
        }

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Transportation");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@TransportationId", id);
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
        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_Transportation");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@TransportationId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@Transport", formCollection["txtTransportName"]);
                com.Parameters.AddWithValue("@PersonName1", formCollection["txtTransportName1"]);
                com.Parameters.AddWithValue("@PersonName2", formCollection["txtTransportName2"]);
                com.Parameters.AddWithValue("@RouteName", formCollection["txtRouteName"]);
                com.Parameters.AddWithValue("@CityId", formCollection["ddCity"]);
                com.Parameters.AddWithValue("@Address", formCollection["txtAddress"]);
                com.Parameters.AddWithValue("@Contact1", formCollection["txtContact1"]);
                com.Parameters.AddWithValue("@Contact2", formCollection["txtContact2"]);
                com.Parameters.AddWithValue("@VehicleNumber", formCollection["txtVehicleNumber"]);
                com.Parameters.AddWithValue("@TransportType", formCollection["ddType"]);
                com.Parameters.AddWithValue("@GSTNumber", formCollection["txtGSTNumber"]);
                com.Parameters.AddWithValue("@Remark", formCollection["txtRemark"]);
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
        public JsonResult InsertDealer(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_TransportationAssign");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@AssignTransportationId", formCollection["hdId"] == "null" ? null : formCollection["hdId"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["hdDealerId"]);
                com.Parameters.AddWithValue("@TransportationId", formCollection["hdTransportId"]);
                com.Parameters.AddWithValue("@Action", "INSERT-MULTIPUL");
                //return Json(ConnectionClass.DML(com));
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_TransportationAssign");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@AssignTransportationId", id);
                com.Parameters.AddWithValue("@Action", "DELETE");
                //return Json(ConnectionClass.DML(com));
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }


        [HttpPost]
        public ActionResult LoadDataAssignTransport(string TransportationId, string Name, string CityId)
        {
            SqlCommand com = new SqlCommand("sp_Transportation");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", Name == "" ? null : Name);
            com.Parameters.AddWithValue("@CityId", CityId == "" ? null : CityId);
            com.Parameters.AddWithValue("@TransportationId", TransportationId);
            com.Parameters.AddWithValue("@Action", "SELECT-DEALER");
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

            var PersonName1 = pagination.data.columns[0].search.value;
            var CityId = pagination.data.columns[1].search.value;
            var Contact1 = pagination.data.columns[2].search.value;
            var RouteName = pagination.data.columns[3].search.value;
            var Transport = pagination.data.columns[4].search.value;
            var TransportType = pagination.data.columns[5].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Transportation");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@PersonName1", PersonName1 == "" ? null : PersonName1);
            com.Parameters.AddWithValue("@CityId", CityId == "" ? null : CityId);
            com.Parameters.AddWithValue("@Contact1", Contact1 == "" ? null : Contact1);
            com.Parameters.AddWithValue("@RouteName", RouteName == "" ? null : RouteName);
            com.Parameters.AddWithValue("@Transport", Transport == "" ? null : Transport);
            com.Parameters.AddWithValue("@TransportType", TransportType == "" ? null : TransportType);
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