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
    public class SchemeDealerController : Controller
    {
        // GET: SchemeDealer
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
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_SchemeDealer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeAttachmentId", id);
                com.Parameters.AddWithValue("@Action", "SELECT-DEALER");
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

        /* ViewBag.JsonDistributorId = id;
            return PartialView();
        public ActionResult FormScheme(string id)
        {
            string JSONString = "null";
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_SchemeDealer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DistributorId", id);
                com.Parameters.AddWithValue("@Action", "SELECT-ORDER-FROM");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    JSONString = JsonConvert.SerializeObject(dt);
                }
            }
            ViewBag.JsonData = JSONString;
            return View();
        }

*/
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_SchemeDealer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeAttachmentId", id);
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
                SqlCommand com = new SqlCommand("sp_SchemeDealer");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SchemeAttachmentId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@Date", formCollection["txtDate"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["txtDistributorId"]);
                com.Parameters.AddWithValue("@SchemeId", formCollection["ddScheme"]);
                com.Parameters.AddWithValue("@SchemeTargetId", formCollection["ddTargetAmount"]);
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

        public ActionResult LoadDataInfo(string id, string schemeId)
        {
            SqlCommand com = new SqlCommand("sp_SchemeTrack");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DistributorId", id);
            com.Parameters.AddWithValue("@SchemeId", schemeId);
            com.Parameters.AddWithValue("@Action", "SCHEME-INVOICE");
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

        [HttpPost]
        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var DistributorId = pagination.data.columns[0].search.value;
            var Date = pagination.data.columns[1].search.value;
            var Label = pagination.data.columns[2].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_SchemeDealer");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@DistributorId", DistributorId == "" ? null : DistributorId);
            com.Parameters.AddWithValue("@Date", Date == "" ? null : Date);
            com.Parameters.AddWithValue("@Label", Label == "" ? null : Label);
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

        public ActionResult Attachment(string id)
        {
            ViewBag.JsonDataId = id;
            return PartialView();
        }

        public JsonResult uploadDocumentAttachment(string id)//
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    int i = 0;
                    string newid = id;
                    string label = Request.Form["Label"];
                    string fname = "";
                    string extension = "";
                    string fnamepath = "";
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            extension = file.FileName.Split('.')[1];
                            var alias = (files.Count > 1) ? "" + i : "";
                            fname = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension;
                        }
                        fnamepath = System.Configuration.ConfigurationManager.AppSettings["URL_SCHEME-ATTACHMENT"] + "\\" + fname;
                        string temp = System.IO.Path.Combine(Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["URL_SCHEME-ATTACHMENT"]), fname);
                        file.SaveAs(temp);

                        SqlCommand com = new SqlCommand("sp_SchemeAttach");
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@SchemeAttachmentId", newid);
                        com.Parameters.AddWithValue("@Label", label);
                        com.Parameters.AddWithValue("@Path", fname);
                        com.Parameters.AddWithValue("@Action", "INSERT");
                        var output = ConnectionClass.DML(com);
                    }
                    if (i != files.Count)
                    {
                        return Json(new { status = "Error", errMsg = "Uncompleted upload please try again" });
                    }
                    return Json(new { status = "Success", errMsg = "Upload Successfully.", url = "/Item/List" });
                }
                else
                    return Json(new { status = "Error", errMsg = "No file uploaded." });

            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", errMsg = ex.Message });
            }
        }

        public JsonResult DeleteAttachment(int id)
        {
            SqlCommand com = new SqlCommand("sp_SchemeAttach");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SchemeAttachtId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            return ConnectionClass.DML(com);
        }

        [HttpPost]
        public ActionResult LoadDataAttachment(string id)
        {
            SqlCommand com = new SqlCommand("sp_SchemeAttach");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@SchemeAttachmentId", id);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet dataSet = ConnectionClass.getDataSet(com);
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0];
                /*start: Add Image*/
                var path = "../" + System.Configuration.ConfigurationManager.AppSettings["URL_SCHEME-ATTACHMENT"] + "\\";
                DataTable newTable = dt.Copy();
                foreach (DataRow row in newTable.Rows)
                {
                    var attachment = row[2].ToString();
                    row[2] = "<a href=\"" + (path + attachment) + "\" alt=\"" + attachment + "\" target=\"blank\"><i class=\"fas fa-file-archive attachment-record\" title=\"Attachment Documents\" data-id=\"\"></i></a>";
                }
                /*start: Image*/
                var data = newTable.AsEnumerable().Select(row => row.ItemArray).ToList();
                var recordsTotal = dt.Rows.Count;
                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsFiltered = "0", recordsTotal = "0", data = "" }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}