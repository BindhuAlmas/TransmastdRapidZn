using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Text;
using Telerik.Web.UI;
using AmarCentre.BAL;

namespace AmarCentre.Masters
{
    public partial class DisplayMenuCustomer : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();

                ViewState["userid"] = Request.QueryString["userid"].ToString();
                CreateDynamicMenu();
            }
        }

        private void CreateDynamicMenu()
        {
            bool parentflage = true;
            bool subparentflage = true;

            string parent = string.Empty;
            string Subparent = string.Empty;

            DataSet dsMenu = obj_master.Get_MenuListCustomer(Convert.ToInt32(ViewState["userid"].ToString()));
            DataTable dt = dsMenu.Tables[0];
            StringBuilder sb = new StringBuilder();
            sb.Append("<Tree>");
            foreach (DataRow dr1 in dt.Rows)
            {
                go_up:
                if (parentflage == true)
                {
                    sb.Append("<Node Text='" + dr1["MainMenu"].ToString() + "'>");
                    parent = dr1["Menu_Id"].ToString();
                    parentflage = false;
                }
                if (parent != dr1["Menu_Id"].ToString())
                {
                    sb.Append("</Node>");
                    sb.Append("</Node>");
                    parentflage = true;
                    subparentflage = true;
                    goto go_up;
                }
                //sub
                go_upsub:
                if (subparentflage == true)
                {
                    sb.Append("<Node Text='" + dr1["Sub_Menu_Name"].ToString() + "' Value ='" + dr1["Id"].ToString() + "' checked='" + dr1["checked"].ToString() + "'>");
                    Subparent = dr1["Id"].ToString();
                    subparentflage = false;
                }
                if (Subparent != dr1["Id"].ToString())
                {
                    sb.Append("</Node>");
                    subparentflage = true;
                    goto go_upsub;
                }
               
                //end
            }
            sb.Append("</Node>");
            sb.Append("</Node>");
            sb.Append("</Tree>");

            string xmlString = sb.ToString();
            RadTreeView1.LoadXml(xmlString);

            var nodes = RadTreeView1.GetAllNodes();
            for (var i = 0; i < nodes.Count; i++)
            {
                nodes[i].ExpandParentNodes();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string user_id = ViewState["userid"].ToString();
            try
            {
                DataTable dt_detail = new DataTable();
                dt_detail.Columns.Add("Sub_Menu_Id", typeof(int));

                RadTreeNode SubN = new RadTreeNode();

                IList<RadTreeNode> listnode = RadTreeView1.CheckedNodes;
                foreach (RadTreeNode n in listnode)
                {
                    if (n.Value != "")
                    {
                        dt_detail.Rows.Add(n.Value);
                        SubN = n;
                    }
                }
                obj_master.Update_CustomerMenu(Convert.ToInt32(user_id),  dt_detail);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Saved Successfully.!');", true);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
