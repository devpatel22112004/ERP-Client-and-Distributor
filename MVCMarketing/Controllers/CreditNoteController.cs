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
    public class CreditNoteController : Controller
    {
        // GET: CreditNote
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_CreditNote");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    ViewBag.JsonData = JSONString;
                }
            }
            return View();
        }

        public JsonResult EditItem(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_CreditNoteItem");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteItemId", id);
                com.Parameters.AddWithValue("@Action", "SELECTSINGLE");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dt);
                    return Json(JSONString, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Print(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_CreditNote");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteId", id);
                com.Parameters.AddWithValue("@Action", "PROFORMA-INVOICE");
                DataSet ds = ConnectionClass.getDataSet(com);
                if (ds != null)
                {
                    string JSONString = string.Empty;

                    JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    ViewBag.JsonDataHead = JSONString;

                    DataTable newItemsTable = ds.Tables[1].Copy();
                    /*start: Add Image*/
                    /* var path = "../" + System.Configuration.ConfigurationManager.AppSettings["URL_IMAGES_ITEM"] + "\\";
                     foreach (DataRow row in newItemsTable.Rows)
                     {
                         var img = (row[1].ToString().Length > 0 ? row[1].ToString() : "no_img.png");
                         row[1] = "<img src=\"" + (path + img) + "\" style=\"width:50px;height:50px\" alt=\"Image\" class=\"ImageIcon\">";
                     }
                     */
                    JSONString = JsonConvert.SerializeObject(newItemsTable);
                    ViewBag.JsonDataBody = JSONString;

                    JSONString = JsonConvert.SerializeObject(ds.Tables[2]);
                    ViewBag.JsonDataFooter = JSONString;

                    JSONString = JsonConvert.SerializeObject(ds.Tables[3]);
                    ViewBag.JsonTaxCalculation = JSONString;
                }
            }
            return View();
        }

        public ActionResult TermCondition(string id)
        {
            if (id != null)
            {
                SqlCommand com = new SqlCommand("sp_CreditNote");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteId", id);
                com.Parameters.AddWithValue("@Action", "PROFORMA-INVOICE");
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
        public JsonResult UpdateTermCondition(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_CreditNote");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteId", formCollection["hdTermOrderId"]);
                com.Parameters.AddWithValue("@TermCondition", formCollection["txtTermCondition"]);
                com.Parameters.AddWithValue("@Action", "UPDATE-TERM");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult UpdateTotalDiscount(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_CreditNote");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteId", formCollection["hdDiscountOrderId"]);
                com.Parameters.AddWithValue("@Discount", formCollection["txtTotalDiscount"]);
                com.Parameters.AddWithValue("@Action", "UPDATE-DISCOUNT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult UpdateTotalCredit(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_CreditNote");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteId", formCollection["hdDiscountOrderId"]);
                com.Parameters.AddWithValue("@Credit", formCollection["txtTotalCredit"]);
                com.Parameters.AddWithValue("@Action", "UPDATE-CREDIT");
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult Insert(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_CreditNote");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteId", formCollection["hdCreditNoteId"]);
                com.Parameters.AddWithValue("@InvoiceId", formCollection["hdInvoiceId"]);
                com.Parameters.AddWithValue("@Date", formCollection["txtDate"]);
                com.Parameters.AddWithValue("@DistributorId", formCollection["txtDistributorId"]);
                com.Parameters.AddWithValue("@Remark", formCollection["txtRemark"]);
                com.Parameters.AddWithValue("@Type", formCollection["ddType"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                //return Json(ConnectionClass.DML(com));
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult InsertItem(FormCollection formCollection)
        {
            try
            {
                SqlCommand com = new SqlCommand("sp_CreditNoteItem");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CreditNoteItemId", formCollection["hdCreditNoteItemId"]);
                com.Parameters.AddWithValue("@CreditNoteId", formCollection["hdCreditNoteId2"]);
                com.Parameters.AddWithValue("@ItemId", formCollection["hdItemsId"]);
                com.Parameters.AddWithValue("@Quantity", formCollection["txtQuantity"]);
                com.Parameters.AddWithValue("@Unit", formCollection["txtItemUnit"]);
                com.Parameters.AddWithValue("@GST", formCollection["txtItemGST"]);
                com.Parameters.AddWithValue("@Rate", formCollection["txtItemRate"]);
                com.Parameters.AddWithValue("@Discount1", formCollection["txtDiscount1"]);
                com.Parameters.AddWithValue("@Discount2", formCollection["txtDiscount2"]);
                com.Parameters.AddWithValue("@Discount3", formCollection["txtDiscount3"]);
                com.Parameters.AddWithValue("@Total", formCollection["txtAmount"]);
                com.Parameters.AddWithValue("@TotalAmount", formCollection["txtFinalAmount"]);
                com.Parameters.AddWithValue("@Remark", formCollection["txtRemarkItem"]);
                com.Parameters.AddWithValue("@Action", "INSERT");
                //return Json(ConnectionClass.DML(com));
                return ConnectionClass.DML(com);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { status = "Error", errMsg = ex.Message }));
            }
        }

        public JsonResult DeleteItem(string id)
        {
            SqlCommand com = new SqlCommand("sp_CreditNoteItem");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CreditNoteItemId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            return ConnectionClass.DML(com);
        }

        public JsonResult Delete(string id)
        {
            SqlCommand com = new SqlCommand("sp_CreditNote");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CreditNoteId", id);
            com.Parameters.AddWithValue("@Action", "DELETE");
            return ConnectionClass.DML(com);
        }

        [HttpPost]
        public ActionResult LoadDataItem(string id)
        {
            SqlCommand com = new SqlCommand("sp_CreditNoteItem");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CreditNoteId", id);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataSet ds = ConnectionClass.getDataSet(com);
            if (ds != null)
            {
                var body = JsonConvert.SerializeObject(ds.Tables[0]);
                var footer = JsonConvert.SerializeObject(ds.Tables[1]);
                var recordsTotal = ds.Tables[0].Rows.Count;
                return Json(new
                {
                    recordsTotal = recordsTotal,
                    body = body,
                    footer = footer
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { recordsTotal = "0", data = "", footer = "" }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult LoadData(Pagination pagination)
        {

            //jQuery DataTables Param
            var draw = pagination.data.draw; //Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = pagination.data.start; //Request.Form.GetValues("start").FirstOrDefault();
            var length = pagination.data.length;// Request.Form.GetValues("length").FirstOrDefault();

            var Date = pagination.data.columns[0].search.value;
            var CreditNoteNumber = pagination.data.columns[1].search.value;
            var DistributorId = pagination.data.columns[2].search.value;
            var Group = pagination.data.columns[3].search.value;
            var GroupRef = pagination.data.columns[4].search.value;
            var Type = pagination.data.columns[5].search.value;


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            int page = (skip / pageSize);

            SqlCommand com = new SqlCommand("sp_CreditNote");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sort", pagination.data.order[0].dir);
            com.Parameters.AddWithValue("@column", pagination.data.order[0].column);//.columns[pagination.data.order[0].column].name
            com.Parameters.AddWithValue("@PageSize", pageSize);
            com.Parameters.AddWithValue("@PageNumber", page);
            com.Parameters.AddWithValue("@totalrow", pagination.data.length);
            com.Parameters.AddWithValue("@Search", pagination.data.search.value);

            com.Parameters.AddWithValue("@Date", Date == "" ? null : Date);
            com.Parameters.AddWithValue("@CreditNoteNumber", CreditNoteNumber == "" ? null : CreditNoteNumber);
            com.Parameters.AddWithValue("@DistributorId", DistributorId == "" ? null : DistributorId);
            com.Parameters.AddWithValue("@Group", Group == "" ? null : Group);
            com.Parameters.AddWithValue("@GroupRef", GroupRef == "" ? null : GroupRef);
            com.Parameters.AddWithValue("@Type", Type == "" ? null : Type);
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