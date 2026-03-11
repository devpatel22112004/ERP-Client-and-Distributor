using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace MVCMarketing.Models
{
    public static class JsonResultProcess
    {
        public static JsonProperty DataTable(string result, Object obj)
        {
            JObject resultObject = JObject.Parse(result);
            JsonProperty jsonProperty = new JsonProperty();
            jsonProperty.isStatus = (string)resultObject["status"];
            jsonProperty.message = (string)resultObject["message"];
            jsonProperty.recordsFiltered = (int)resultObject["recordsFiltered"];
            jsonProperty.recordsTotal = (int)resultObject["recordsTotal"];
            JArray jArray = JArray.Parse(Convert.ToString(resultObject["data"]));
            List<Array> list = new List<Array>();
            foreach (JObject jObject in jArray)
            {
                List<string> columnValue = new List<string>();
                string hiddenValue = "";
                foreach (var item in jObject)
                {
                    PropertyInfo pi = obj.GetType().GetProperty(item.Key);
                    if (pi != null)
                    {
                        int isDisplayColumn = GetPropertyDisplayColumn(pi);
                        if (isDisplayColumn == 0)
                        {
                            columnValue.Add(item.Value.ToString());
                        }
                        else if (isDisplayColumn == 1)
                        {
                            hiddenValue += " data-" + item.Key + "=\"" + item.Value.ToString() + "\"";
                        }
                    }

                }
                if (hiddenValue != "")
                {
                    columnValue.Add("<span " + hiddenValue + "></span>");
                }
                list.Add(columnValue.ToArray());
            }
            jsonProperty.list = list;
            return jsonProperty;
        }

        public static JsonProperty DataTableWithoutPage(string result, Object obj)
        {
            JObject resultObject = JObject.Parse(result);
            JsonProperty jsonProperty = new JsonProperty();
            jsonProperty.isStatus = (string)resultObject["status"];
            jsonProperty.message = (string)resultObject["message"];
            JArray jArray = JArray.Parse(Convert.ToString(resultObject["data"]));
            List<Array> list = new List<Array>();
            foreach (JObject jObject in jArray)
            {
                List<string> columnValue = new List<string>();
                string hiddenValue = "";
                foreach (var item in jObject)
                {
                    PropertyInfo pi = obj.GetType().GetProperty(item.Key);
                    if (pi != null)
                    {
                        int isDisplayColumn = GetPropertyDisplayColumn(pi);
                        if (isDisplayColumn == 0)
                        {
                            columnValue.Add(item.Value.ToString());
                        }
                        else if (isDisplayColumn == 1)
                        {
                            hiddenValue += " data-" + item.Key + "=\"" + item.Value.ToString() + "\"";
                        }
                    }

                }
                if (hiddenValue != "")
                {
                    columnValue.Add("<span " + hiddenValue + "></span>");
                }
                list.Add(columnValue.ToArray());
            }
            jsonProperty.list = list;
            return jsonProperty;
        }

        private static int GetPropertyDisplayColumn(PropertyInfo pi)
        {
            try
            {
                var dp1 = pi.GetCustomAttributes(typeof(ParameterDisplayAttribute), true).Cast<ParameterDisplayAttribute>().SingleOrDefault();
                if (dp1 != null)
                {
                    string v = dp1.Display;
                    if (v == "Hidden")
                    {
                        return 1;
                    }
                    return 0;
                }
                return -1;
            }
            catch
            {
                return -1;
            }

        }

        public static string DataTableHeader(Object obj, bool isAction)
        {
            string th = "<tr>";
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                th += GetPropertyColumn(propertyInfo);
            }
            if (isAction)
                th += "<th></th>";
            th += "</tr>";
            return th;
        }

        private static string GetPropertyColumn(PropertyInfo pi)
        {
            try
            {
                var dp1 = pi.GetCustomAttributes(typeof(ParameterDisplayAttribute), true).Cast<ParameterDisplayAttribute>().SingleOrDefault();
                if (dp1 != null)
                {
                    return dp1.Header != null ? "<th>" + dp1.Header + "</th>" : "";
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public static string TableHeader(Object obj)
        {
            string array = "";
            try
            {
                bool isFirst = false;
                Type type = obj.GetType();
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    if (isFirst)
                    {
                        array += ",";
                    }
                    isFirst = true;
                    var atts = propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    array += atts!=null ? (atts[0] as DisplayNameAttribute).DisplayName : "No Title";
                }
            }
            catch(Exception ex)
            {
                array += "Error : "+ex.Message;
            }
            return array;
        }

        public static string ModelHeader(Object obj)
        {
            string array = "";
            try
            {
                bool isFirst = false;
                Type type = obj.GetType();
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    if (isFirst)
                    {
                        array += ",";
                    }
                    isFirst = true;
                    array += propertyInfo.Name;
                }
            }
            catch (Exception ex)
            {
                array += "Error : " + ex.Message;
            }
            return array;
        }
    }
}