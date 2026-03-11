using MVCMarketing.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCMarketing.Controllers
{
    public class DistributorReportSalesOrderController : Controller
    {
        // GET: DistributorReportSalesOrder
        public ActionResult Report()
        {
            return View();
        }

        public ActionResult Info(string id)
        {
            ViewBag.JsonOrderId = id;
            return PartialView();
        }

        public ActionResult LoadData()
        {
            //jQuery DataTables Param
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            var Date = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var OrderNumber = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DistributorId = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize) + 1;

            SqlCommand com = new SqlCommand("sp_ReportSalesOrder");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Date", Date == "" ? null : Date);
            com.Parameters.AddWithValue("@OrderNumber", OrderNumber == "" ? null : OrderNumber);
            com.Parameters.AddWithValue("@DistributorId", Session["UserId"].ToString());
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@Action", "REPORT");
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
        }

        public void ExportToExcel(string Date, string OrderNumber, string DistributorId)
        {
            SqlCommand com = new SqlCommand("sp_ReportSalesOrder");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Date", Date == "" ? null : Date);
            com.Parameters.AddWithValue("@OrderNumber", OrderNumber == "" ? null : OrderNumber);
            com.Parameters.AddWithValue("@DistributorId", DistributorId == "" ? null : DistributorId);
            com.Parameters.AddWithValue("@Action", "EXPORTREPORT");
            DataTable dt = ConnectionClass.getDataTable(com);
            if (dt != null)
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=ReportStoreOutward-" + DateTime.Now.ToString("MM/dd/yyyyHH:mm") + ".xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
                    {
                        GridView grid = new GridView();
                        grid.DataSource = dt;
                        grid.DataBind();
                        grid.RenderControl(htw);
                        Response.Write(sw.ToString());
                    }
                }
                Response.End();
            }
        }

        public ActionResult LoadDataInfo(string id)
        {
            SqlCommand com = new SqlCommand("sp_ReportSalesOrder");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@OrderId", id);
            com.Parameters.AddWithValue("@Action", "ORDER-LIST");
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

        public void ExportToExcel1(string OrderId)
        {
            SqlCommand com = new SqlCommand("sp_ReportSalesOrder");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@OrderId", OrderId);
            com.Parameters.AddWithValue("@Action", "EXPORTREPORT-ITEM");
            DataSet ds = ConnectionClass.getDataSet(com);

            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            //DataTable dt2 = ds.Tables[2];
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=ReportToolsIssueItem-" + DateTime.Now.ToString("MM/dd/yyyyHH:mm") + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            if (dt != null)
            {

                Response.Write("Client Information");
                Response.Write(makeExcelData(dt));
            }
            if (dt1 != null)
            {
                Response.Write("Issue Item Information");
                Response.Write(makeExcelData(dt1));
            }

            Response.End();
        }

        private String makeExcelData(DataTable dt)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    GridView grid = new GridView();
                    grid.DataSource = dt;
                    grid.DataBind();
                    grid.RenderControl(htw);
                    return sw.ToString();
                }
            }
        }
    }
}