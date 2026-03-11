using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace MVCMarketing.Models
{
    public class ConnectionClass
    {
        private static SqlConnection con = null;

        private static void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
            con = new SqlConnection(constr);
        }

        /*
        public static String DML(SqlCommand com)
        {
            var status = "Success";
            var msg = "";
            try
            {
                connection();
                con.Open();
                com.Connection = con;
                com.Parameters.AddWithValue("@UserBy", HttpContext.Current.Session["UserId"]);
                com.Parameters.Add("@msg", SqlDbType.VarChar, 150);
                com.Parameters["@msg"].Direction = ParameterDirection.Output;
                com.ExecuteNonQuery();
                con.Close();
                status = com.Parameters["@msg"].Value.ToString();
            }
            catch (Exception ex)
            {
                status = "Error";
                msg = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return new JavaScriptSerializer().Serialize(new { status = status, errMsg = msg });//Json(new {id = id,status=status}, JsonRequestBehavior.AllowGet);
        }
        */

        public static JsonResult DML(SqlCommand com)
        {
            var status = "Success";
            var message = "";
            var rowId = "";
            try
            {
                connection();
                con.Open();
                com.Connection = con;
                com.Parameters.AddWithValue("@UserBy", HttpContext.Current.Session["UserId"]);
                com.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserId"]);
                com.Parameters.Add("@dmlStatus", SqlDbType.VarChar, 150);
                com.Parameters["@dmlStatus"].Direction = ParameterDirection.Output;
                com.Parameters.Add("@dmlMessage", SqlDbType.VarChar,-1);
                com.Parameters["@dmlMessage"].Direction = ParameterDirection.Output;
                com.Parameters.Add("@rowId", SqlDbType.VarChar, 30);
                com.Parameters["@rowId"].Direction = ParameterDirection.Output;
                com.ExecuteNonQuery();
                con.Close();
                status = com.Parameters["@dmlStatus"].Value.ToString();
                message = com.Parameters["@dmlMessage"].Value.ToString();                
                rowId = com.Parameters["@rowId"].Value.ToString();
            }
            catch (Exception ex)
            {
                status = "error";
                message = ex.ToString();
                rowId = "";
            }
            finally
            {
                con.Close();
            }
            var result = new { status = status, idValue = rowId, message = message };
            //return new JavaScriptSerializer().Serialize(result);//Json(new {id = id,status=status}, JsonRequestBehavior.AllowGet);
            return new JsonResult()
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        public static string GetJsonData(SqlCommand com)
        {
            JArray resultJson = new JArray();
            try
            {
                // Assuming 'connection' establishes your connection to the database
                connection();
                com.Connection = con;

                // Make sure the connection is open
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Execute the query
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new JObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                            row[columnName] = value == null ? null : value.ToString();
                        }
                        resultJson.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            // Return the JSON string
            return resultJson.ToString();
        }

        public static DataTable getDataTable(SqlCommand com)
        {
            DataTable dt = null;
            try
            {
                connection();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                con.Open();
                com.Connection = con;
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                dt = null;
                Console.Write(ex);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        /*public static DataSet getDataSet(SqlCommand com)
        {
            DataSet dt = null;
            try
            {
                connection();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                con.Open();
                com.Connection = con;
                da.Fill(ds);
                dt = ds;
            }
            catch (Exception ex)
            {
                dt = null;
                Console.Write(ex);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }*/

        public static DataSet getDataSet(SqlCommand com)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlDataAdapter da = new SqlDataAdapter(com);
                con.Open();
                com.Connection = con;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
                Console.Write(ex);
            }
            finally
            {
                con.Close();
            }
            return ds;
        }

        public static string fillDropDown(string sp,string action,string refId)
        {
            string json;
            try
            {
                connection();
                con.Open();
                SqlCommand com = new SqlCommand(sp);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@recordId", refId);
                com.Parameters.AddWithValue("@Action", action);
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                List<SelectListItem> obj = new List<SelectListItem>();
                while (reader.Read())
                {
                    obj.Add(new SelectListItem() { Text = reader[1].ToString(), Value = reader[0].ToString() });
                }
                json = JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                json = "";
                Console.Write(ex);
            }
            finally
            {
                con.Close();
            }
            return json;
        }

        public static string fillAutoComplete(string sp, string prefix, string refId)
        {
            string dt = null;
            try
            {
                connection();
                SqlCommand com = new SqlCommand(sp);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@RecordId", refId);
                com.Parameters.AddWithValue("@Prefix", prefix);
                com.Connection = con;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();                
                da.Fill(ds);
                dt=JsonConvert.SerializeObject(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                dt = null;
                Console.Write(ex);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        /*start: API*/

        public static DataTable getAppDataTable(SqlCommand com)
        {
            DataTable dt = null;
            try
            {
                connection();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                con.Open();
                com.Connection = con;
                da.Fill(ds);
                //con.Close();
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public static DataSet getAppDataSet(SqlCommand com)
        {
            DataSet ds = null;
            try
            {
                connection();
                SqlDataAdapter da = new SqlDataAdapter(com);
                ds = new DataSet();
                con.Open();
                com.Connection = con;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                con.Close();
            }
            return ds;
        }

        public static string AppDMLJson(SqlCommand com)
        {

            var status = "Success";
            var message = "";
            var rowId = "";
            try
            {
                connection();
                con.Open();
                com.Connection = con;
                //com.Parameters.AddWithValue("@UserBy", HttpContext.Current.Session["UserId"]);
                com.Parameters.Add("@msg", SqlDbType.VarChar, 150);
                com.Parameters["@msg"].Direction = ParameterDirection.Output;
                com.Parameters.Add("@rowId", SqlDbType.VarChar, 150);
                com.Parameters["@rowId"].Direction = ParameterDirection.Output;

                com.ExecuteNonQuery();
                con.Close();
                status = com.Parameters["@msg"].Value.ToString();
                rowId = com.Parameters["@rowId"].Value.ToString();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.ToString();
                rowId = "";
            }
            finally
            {
                con.Close();
            }
            return "{\"status\":\"" + status + "\",\"message\":\"" + message + "\",\"rowId\":\"" + rowId + "\"}";//new JavaScriptSerializer().Serialize(new { status = status, message = message, rowId= rowId });
        }
        /*stop: API*/
    }
}