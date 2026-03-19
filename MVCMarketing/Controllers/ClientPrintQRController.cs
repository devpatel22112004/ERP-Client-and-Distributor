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
    ///*[SessionTimeOut]*/
    public class ClientPrintQRController : Controller
    {
        // GET: ClientPrintQR
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var Name = pagination.data.columns[0].search.value;
            var City = pagination.data.columns[1].search.value;
            var StateId = pagination.data.columns[2].search.value;
            var Status = pagination.data.columns[3].search.value;
            var Area = pagination.data.columns[4].search.value;
            var Code = pagination.data.columns[5].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_ClientPrintQR");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@Name", Name == "" ? null : Name);
            com.Parameters.AddWithValue("@CityId", City == "" ? null : City);
            com.Parameters.AddWithValue("@StateId", StateId == "" ? null : StateId);
            com.Parameters.AddWithValue("@Status", Status == "" ? null : Status);
            com.Parameters.AddWithValue("@Area", Area == "" ? null : Area);
            com.Parameters.AddWithValue("@Code", Code == "" ? null : Code);
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

        public ActionResult Info(string id)
        {
            ViewBag.JsonDistributorId = id;
            return PartialView();
        }

        public ActionResult LoadDataClientQR(string id)
        {
            SqlCommand com = new SqlCommand("sp_ClientPrintQR");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorId", id);
            com.Parameters.AddWithValue("@Action", "SELECT-CLIENT");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                //var data = dt.AsEnumerable().Select(row => row.ItemArray).ToList();
                var data = JsonConvert.SerializeObject(dt);
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