using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MVCMarketing.Models;
using Newtonsoft.Json;

namespace MVCMarketing.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Employee");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@EmployeeId", id);
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

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_Employee");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@EmployeeId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@EmployeeName", formCollection["txtService"]);
                com.Parameters.AddWithValue("@Mobile", formCollection["txtMobile"]);
                com.Parameters.AddWithValue("@OtherMobile", formCollection["txtOtherMobile"]);
                com.Parameters.AddWithValue("@PermanentAddress", formCollection["txtPermitAddress"]);
                com.Parameters.AddWithValue("@CurrentAddress", formCollection["txtCurrentAddress"]);
                com.Parameters.AddWithValue("@DesignationId", formCollection["ddDesignation"]);
                com.Parameters.AddWithValue("@Education", formCollection["ddEducation"]);
                com.Parameters.AddWithValue("@DOB", formCollection["txtDOB"]);
                com.Parameters.AddWithValue("@Email", formCollection["txtEmail"]);
                com.Parameters.AddWithValue("@CompanyMobile", formCollection["txtMobileCompany"]);
                com.Parameters.AddWithValue("@CompanyEmail", formCollection["txtEmailCompany"]);
                com.Parameters.AddWithValue("@JoiningDate", formCollection["txtJoiningDate"]);
                com.Parameters.AddWithValue("@UserId", formCollection["txtUserId"]);
                com.Parameters.AddWithValue("@Password", formCollection["txtPassword"]);
                com.Parameters.AddWithValue("@Status", formCollection["ddStatus"]);
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

        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var EmpServiceEng = pagination.data.columns[0].search.value;
            var EmpDesignation = pagination.data.columns[1].search.value;
            var Status = pagination.data.columns[2].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Employee");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@EmployeeName", EmpServiceEng == "" ? null : EmpServiceEng);
            com.Parameters.AddWithValue("@DesignationId", EmpDesignation == "" ? null : EmpDesignation);
            com.Parameters.AddWithValue("@Status", Status == "" ? null : Status);
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