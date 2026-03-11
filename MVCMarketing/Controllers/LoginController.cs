using MVCMarketing.Models;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace MVCMarketing.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Loginprocess(string Password, string UserId)
        {
            if (Password != null)
            {
                SqlCommand com = new SqlCommand("sp_Employee");
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Password", Password);
                com.Parameters.AddWithValue("@UserId", UserId);
                com.Parameters.AddWithValue("@Action", "LOGIN");
                DataTable dt = ConnectionClass.getDataTable(com);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        return Json(new { success = false, message = "Oops! User id or password does not match." });
                    }
                    var record = dt.AsEnumerable().Select(row => new { col1 = row[0], col2 = row[1], col3 = row[2], col4 = row[3], col5 = row[4] });
                    if (record != null)
                    {
                        if (record.FirstOrDefault().col5.ToString().ToUpper() != "TRUE")
                        {
                            return Json(new { success = false, message = "Account is locked" });
                        }

                        Session["UserId"] = record.FirstOrDefault().col1;
                        Session["UserSort"] = record.FirstOrDefault().col2;

                        string Designation = record.FirstOrDefault().col3.ToString();
                        Session["UserDesignation"] = Designation;
                        Session["User"] = record.FirstOrDefault().col4;
                        //  var Menu =  record.FirstOrDefault().col6.ToString();
                        // Session["Menu"] = Menu;


                        if (Designation.ToUpper().Equals("ADMIN"))
                        {
                            Session["Layout"] = "~/Views/Shared/_Layout.cshtml";
                            return Json(new { success = true, url = Url.Action("Index", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("ACCOUNT"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutAccount.cshtml";
                            return Json(new { success = true, url = Url.Action("AccountDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("OPERATION MANAGER"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutOperationManager.cshtml";
                            return Json(new { success = true, url = Url.Action("OperationDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("PROJECT COORDINATOR"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutProgrammer.cshtml";
                            return Json(new { success = true, url = Url.Action("CoordinatorDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("PROJECT DESIGNING"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutDesigning.cshtml";
                            return Json(new { success = true, url = Url.Action("DesigningDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("PROJECT MANAGER"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutProjectManager.cshtml";
                            return Json(new { success = true, url = Url.Action("ProjectManagerDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("SALES FIELD"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutSalesField.cshtml";
                            return Json(new { success = true, url = Url.Action("SalesFieldDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("SALES HEAD"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutSalesHead.cshtml";
                            return Json(new { success = true, url = Url.Action("SalesHeadDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("SALES OFFICE"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutSalesOffice.cshtml";
                            return Json(new { success = true, url = Url.Action("SalesOfficeDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("SITE SUPERVISIOR"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutSiteSupervisior.cshtml";
                            return Json(new { success = true, url = Url.Action("SiteSupervisiorDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("STORE"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutStore.cshtml";
                            return Json(new { success = true, url = Url.Action("StoreDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("BACK OFFICE"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutBackOffice.cshtml";
                            return Json(new { success = true, url = Url.Action("BackOfficeDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("DISTRIBUTOR"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutDistributor.cshtml";
                            return Json(new { success = true, url = Url.Action("DistributorDashboard", "Home") });
                        }
                        else if (Designation.ToUpper().Equals("TRANSPORT"))
                        {
                            Session["Layout"] = "~/Views/Shared/_LayoutTranspotation.cshtml";
                            return Json(new { success = true, url = Url.Action("TranspotationDashboard", "Home") });
                        }
                    }
                }
            }
            return Json(new { success = false, message = "Oops! User id or password does not match." });
        }
    }
}