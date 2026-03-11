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
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Item");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ItemId", id);
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

        public ActionResult Form()
        {
            return PartialView();
        }

        public ActionResult Imag(string id)
        {
            ViewBag.JsonId = id;
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_Item");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ItemId", id);
                com.Parameters.AddWithValue("@Action", "IMAGES-SELECTSINGLE");
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

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_Item");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ItemId", formCollection["hdId"]);
                com.Parameters.AddWithValue("@ItemCode", formCollection["txtItemCode"]);
                com.Parameters.AddWithValue("@BrandId", formCollection["ddBrand"]);
                com.Parameters.AddWithValue("@CategoryId", formCollection["ddCategory"]);
                com.Parameters.AddWithValue("@SubCategoryId", formCollection["ddSubCategory"]);
                com.Parameters.AddWithValue("@Description", formCollection["txtDescription"].Trim());
                com.Parameters.AddWithValue("@HSN", formCollection["txtHSN"]);
                com.Parameters.AddWithValue("@Unit", formCollection["ddUnit"]);
                com.Parameters.AddWithValue("@Packing", formCollection["txtPacking"]);
                com.Parameters.AddWithValue("@GST", formCollection["txtGST"]);
                com.Parameters.AddWithValue("@Purchase_Rate", formCollection["txtPurchaseRate"]);
                com.Parameters.AddWithValue("@Sales_Rate", formCollection["txtSalesRate"]);
                com.Parameters.AddWithValue("@Discount", formCollection["txtPriceDiscount"]);
                com.Parameters.AddWithValue("@StockLimit", formCollection["txtStockLimit"]);
                com.Parameters.AddWithValue("@OpeningStock", formCollection["txtOpeningStock"]);
                com.Parameters.AddWithValue("@Status", formCollection["ddStatus"]);
                com.Parameters.AddWithValue("@Refilling", formCollection["ddRefilling"]);
                com.Parameters.AddWithValue("@Warranty", formCollection["txtWarranty"]);
                com.Parameters.AddWithValue("@Purchase_Discount1", formCollection["txtPurchaseDiscount1"]);
                com.Parameters.AddWithValue("@Purchase_Discount2", formCollection["txtPurchaseDiscount2"]);
                com.Parameters.AddWithValue("@Purchase_Discount3", formCollection["txtPurchaseDiscount3"]);
                com.Parameters.AddWithValue("@Sales_Discount1", formCollection["txtSalesDiscount1"]);
                com.Parameters.AddWithValue("@Sales_Discount2", formCollection["txtSalesDiscount2"]);
                com.Parameters.AddWithValue("@Sales_Discount3", formCollection["txtSalesDiscount3"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult Delete(int id)
        {
            SqlCommand com = new SqlCommand("sp_Item");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ItemId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            return ConnectionClass.DML(com);
        }

        [HttpPost]
        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            //find search columns info
            var ItemCode = pagination.data.columns[0].search.value;
            var CategoryId = pagination.data.columns[1].search.value;
            var SubCategoryId = pagination.data.columns[2].search.value;
            var BrandId = pagination.data.columns[3].search.value;
            //Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_Item");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@ItemCode", ItemCode == "" ? null : ItemCode);
            com.Parameters.AddWithValue("@CategoryId", CategoryId == "" ? null : CategoryId);
            com.Parameters.AddWithValue("@SubCategoryId", SubCategoryId == "" ? null : SubCategoryId);
            com.Parameters.AddWithValue("@BrandId", BrandId == "" ? null : BrandId);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet dataSet = ConnectionClass.getDataSet(com);
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[1];
                DataTable dtCount = dataSet.Tables[0];
                recordsTotal = Convert.ToInt32(dtCount.Rows[0][0]);
                /*start: Add Image*/
                var path = "../" + System.Configuration.ConfigurationManager.AppSettings["URL_IMAGES_ITEM"] + "\\";
                DataTable newTable = dt.Copy();
                foreach (DataRow row in newTable.Rows)
                {
                    var img = (row[0].ToString().Length > 0 ? row[0].ToString() : "no_img.png");
                    row[0] = "<img src=\"" + (path + img) + "\" style=\"width:30px;height:30px\" alt=\"Image\" class=\"ImageIcon\">";
                }
                /*start: Image*/
                var data = newTable.AsEnumerable().Select(row => row.ItemArray).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
            }

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
                            //extension = file.FileName.Split('.')[1];
                            string[] ext = file.FileName.Split('.');
                            extension = ext[ext.Length - 1];
                            var alias = (files.Count > 1) ? "" + i : "";
                            fname = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension;
                        }
                        fnamepath = System.Configuration.ConfigurationManager.AppSettings["URL_IMAGES_ITEM"] + "\\" + fname;
                        string temp = System.IO.Path.Combine(Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["URL_IMAGES_ITEM"]), fname);
                        file.SaveAs(temp);

                        SqlCommand com = new SqlCommand("sp_Item");
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ItemId", newid);
                        com.Parameters.AddWithValue("@Img", fname);
                        com.Parameters.AddWithValue("@Action", "IMAGES");
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
    }
}