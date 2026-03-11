using MVCMarketing.Models;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using MVCMarketing.Common;

namespace MVCMarketing.Controllers
{
    [SessionTimeout]
    public class BranchController : Controller
    {
        // GET: Branch
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
                SqlCommand com = new SqlCommand("sp_Branch");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@BranchId", id);
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
                SqlCommand com = new SqlCommand("sp_Branch");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@BranchId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@BranchName", formCollection["txtBranchName"]);
                com.Parameters.AddWithValue("@Address", formCollection["txtAddress"]);
                com.Parameters.AddWithValue("@PersonName", formCollection["txtPersonName"]);
                com.Parameters.AddWithValue("@Contact", formCollection["txtContact"]);
                com.Parameters.AddWithValue("@Email", formCollection["txtEmail"]);
                com.Parameters.AddWithValue("@GSTNumber", formCollection["txtGSTNumber"]);
                com.Parameters.AddWithValue("@PAN", formCollection["txtPAN"]);
                com.Parameters.AddWithValue("@BankDetail", formCollection["txtBankDetail"]);
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

        //public ActionResult Upload(string id)
        //{
        //    ViewBag.JsonWordUploadDataId = id;
        //    ViewBag.JsonWordUploadVideoId = id;
        //    ViewBag.JsonWordUploadAudioId = id;
        //    if (id != null)
        //    {
        //        SqlCommand com = new SqlCommand("sp_Branch");
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@BranchId", id);
        //        com.Parameters.AddWithValue("@Action", "SELECTIMAGE");
        //        DataTable dt = ConnectionClass.getDataTable(com);
        //        if (dt != null)
        //        {
        //            string JSONString = string.Empty;
        //            JSONString = JsonConvert.SerializeObject(dt);
        //            ViewBag.JsonDataImage = JSONString;
        //            ViewBag.JsonDataVideo = JSONString;
        //            ViewBag.JsonDataAudio = JSONString;
        //        }
        //    }
        //    return PartialView();
        //}

        [HttpPost]
        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var BranchName = pagination.data.columns[0].search.value;
            var PersonName = pagination.data.columns[1].search.value;
            var Contact = pagination.data.columns[2].search.value;
            var Status = pagination.data.columns[3].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Branch");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@BranchName", BranchName == "" ? null : BranchName);
            com.Parameters.AddWithValue("@PersonName", PersonName == "" ? null : PersonName);
            com.Parameters.AddWithValue("@Contact", Contact == "" ? null : Contact);
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

        public ActionResult Image(string id)
        {
            ViewBag.JsonId = id;
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Branch");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@BranchId", id);
                com.Parameters.AddWithValue("@Action", "SELECTIMAGE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    var fnamepath = "../"+System.Configuration.ConfigurationManager.AppSettings["URL_BRANCH"];
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    ViewBag.JsonData = JSONString;
                    ViewBag.Path = fnamepath;
                }
            }
            return PartialView();
        }

        public JsonResult uploadDocument(string id)//
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    int i = 0;
                    string newid = id;
                    string fname = "";
                    string tag = Request.Form["tag"];
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
                            //extension = file.FileName.Split('.')[1];
                            //var alias = (files.Count > 1) ? "" + i : "";
                            string[] ext = file.FileName.Split('.');
                            extension = ext[ext.Length - 1];
                            var alias = (files.Count > 1) ? "" + i : "";
                            fname = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension;
                        }
                        fnamepath = System.Configuration.ConfigurationManager.AppSettings["URL_BRANCH"] + "\\" + fname;
                        string temp = System.IO.Path.Combine(Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["URL_BRANCH"]), fname);
                        file.SaveAs(temp);

                        SqlCommand com = new SqlCommand("sp_Branch");
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@BranchId", newid);
                        if(tag == "Stamp")
                            com.Parameters.AddWithValue("@StampUrl", fname);
                        else if (tag == "Logo")
                            com.Parameters.AddWithValue("@LogoUrl", fname);
                        else if (tag == "QR")
                            com.Parameters.AddWithValue("@QRUrl", fname);
                        com.Parameters.AddWithValue("@Action", "IMAGES");
                        var output = ConnectionClass.DML(com);
                    }
                    if (i != files.Count)
                    {
                        return Json(new { status = "Error", fileName = "", errMsg = "Uncompleted upload please try again" });
                    }
                    return Json(new { status = "Success", fileName = fname, errMsg = "Upload Successfully.", url = "" });
                }
                else
                    return Json(new { status = "Error", errMsg = "No file uploaded." });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", errMsg = ex.Message });
            }
        }

        public JsonResult uploadStamp(string id)//
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    int i = 0;
                    string newid = id;
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
                            //extension = file.FileName.Split('.')[1];
                            //var alias = (files.Count > 1) ? "" + i : "";
                            string[] ext = file.FileName.Split('.');
                            extension = ext[ext.Length - 1];
                            var alias = (files.Count > 1) ? "" + i : "";
                            fname = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension;
                        }
                        fnamepath = System.Configuration.ConfigurationManager.AppSettings["URL_STAMP"] + "\\" + fname;
                        string temp = System.IO.Path.Combine(Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["URL_STAMP"]), fname);
                        file.SaveAs(temp);

                        SqlCommand com = new SqlCommand("sp_Branch");
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@BranchId", newid);
                        com.Parameters.AddWithValue("@Stamp_Image", fname);
                        com.Parameters.AddWithValue("@Action", "IMAGES");
                        var output = ConnectionClass.DML(com);
                    }
                    if (i != files.Count)
                    {
                        return Json(new { status = "Error", fileName = "", errMsg = "Uncompleted upload please try again" });
                    }
                    return Json(new { status = "Success", fileName = fname, errMsg = "Upload Successfully.", url = "/Employee/List" });
                }
                else
                    return Json(new { status = "Error", errMsg = "No file uploaded." });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", errMsg = ex.Message });
            }
        }

        public JsonResult uploadLogo(string id)//
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    int i = 0;
                    string newid = id;
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
                            //extension = file.FileName.Split('.')[1];
                            //var alias = (files.Count > 1) ? "" + i : "";
                            string[] ext = file.FileName.Split('.');
                            extension = ext[ext.Length - 1];
                            var alias = (files.Count > 1) ? "" + i : "";
                            fname = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension;
                        }
                        fnamepath = System.Configuration.ConfigurationManager.AppSettings["URL_LOGO"] + "\\" + fname;
                        string temp = System.IO.Path.Combine(Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["URL_LOGO"]), fname);
                        file.SaveAs(temp);

                        SqlCommand com = new SqlCommand("sp_Branch");
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@BranchId", newid);
                        com.Parameters.AddWithValue("@Logo", fname);
                        com.Parameters.AddWithValue("@Action", "LOGO");
                        var output = ConnectionClass.DML(com);
                    }
                    if (i != files.Count)
                    {
                        return Json(new { status = "Error", fileName = "", errMsg = "Uncompleted upload please try again" });
                    }
                    return Json(new { status = "Success", fileName = fname, errMsg = "Upload Successfully.", url = "/Employee/List" });
                }
                else
                    return Json(new { status = "Error", errMsg = "No file uploaded." });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", errMsg = ex.Message });
            }
        }

        public JsonResult uploadQR(string id)//
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    int i = 0;
                    string newid = id;
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
                            //extension = file.FileName.Split('.')[1];
                            //var alias = (files.Count > 1) ? "" + i : "";
                            string[] ext = file.FileName.Split('.');
                            extension = ext[ext.Length - 1];
                            var alias = (files.Count > 1) ? "" + i : "";
                            fname = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension;
                        }
                        fnamepath = System.Configuration.ConfigurationManager.AppSettings["URL_QR"] + "\\" + fname;
                        string temp = System.IO.Path.Combine(Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["URL_QR"]), fname);
                        file.SaveAs(temp);

                        SqlCommand com = new SqlCommand("sp_Branch");
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@BranchId", newid);
                        com.Parameters.AddWithValue("@QRUrl", fname);
                        com.Parameters.AddWithValue("@Action", "QRURL");
                        var output = ConnectionClass.DML(com);
                    }
                    if (i != files.Count)
                    {
                        return Json(new { status = "Error", fileName = "", errMsg = "Uncompleted upload please try again" });
                    }
                    return Json(new { status = "Success", fileName = fname, errMsg = "Upload Successfully.", url = "/Employee/List" });
                }
                else
                    return Json(new { status = "Error", errMsg = "No file uploaded." });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", errMsg = ex.Message });
            }
        }

    }
}