using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class MenuProperty
    {
        //public static string GetTreeData()
        public static Node GetMenusAll()
        {
            SqlCommand com = new SqlCommand("sp_Menu");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataTable dt = ConnectionClass.getDataTable(com);

            Node nodes = new Node { text = "Department" };
            DataView view = new DataView(dt);
            view.RowFilter = "ParentId=0";
            foreach (DataRowView kvp in view)
            {
                State state = new State();
                state.checkedd = false;
                state.disabled = false;
                state.expanded = true;
                state.selected = false;

                string parentId = kvp["Id"].ToString();
                Node node = new Node { id = kvp["id"].ToString(), icon = kvp["icon"].ToString(), text = kvp["text"].ToString(), selectable = true, state = state };
                nodes.nodes.Add(node);
                AddChildMenus(dt, node, parentId);
            }
            return nodes;
        }

        public static Node GetMenusAssign(string id)
        {
            SqlCommand com = new SqlCommand("sp_MenuAssing");
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@EmpId", id);
            com.Parameters.AddWithValue("@Action", "SELECT");
            DataTable dt = ConnectionClass.getDataTable(com);

            Node nodes = new Node { text = "Department" };
            DataView view = new DataView(dt);
            view.RowFilter = "ParentId=0";
            foreach (DataRowView kvp in view)
            {
                State state = new State();
                state.checkedd = false;
                state.disabled = false;
                state.expanded = true;
                state.selected = false;

                string parentId = kvp["Id"].ToString();
                Node node = new Node { id = kvp["id"].ToString(), icon = kvp["icon"].ToString(), text = kvp["text"].ToString(), selectable = true, state = state };
                nodes.nodes.Add(node);
                AddChildMenus(dt, node, parentId);
            }
            return nodes;
        }

        private static void AddChildMenus(DataTable dt, Node parentNode, string ParentId)
        {
            State state = new State();
            state.checkedd = false;
            state.disabled = false;
            state.expanded = false;
            state.selected = false;

            DataView viewItem = new DataView(dt);
            viewItem.RowFilter = "ParentId=" + ParentId;
            foreach (DataRowView childView in viewItem)
            {
                Node node = new Node { id = childView["id"].ToString(), parentid = childView["ParentId"].ToString(), icon = childView["icon"].ToString(), text = childView["text"].ToString(), selectable = true, state = state };
                parentNode.nodes.Add(node);
                string pId = childView["Id"].ToString();
                AddChildMenus(dt, node, pId);
            }
        }

        public class Node
        {
            public Node()
            {
                nodes = new List<Node>();
            }
            public string id { get; set; }
            public string parentid { get; set; }
            public string text { get; set; }
            public string icon { get; set; }
            public bool selectable { get; set; }
            public State state { get; set; }
            public List<Node> nodes { get; set; }
        }

        public class State
        {
            public bool checkedd { get; set; }
            public bool disabled { get; set; }
            public bool expanded { get; set; }
            public bool selected { get; set; }
        }
    }
}