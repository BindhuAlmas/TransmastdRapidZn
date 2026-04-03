using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using AmarCentre.Layout;

namespace AmarCentre
{
    public partial class Home : System.Web.UI.Page
    {
        System_Utilities obj_common = new System_Utilities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                lbluser.Text = Session["User_Name"].ToString();

                DataSet ds = obj_common.SupportValidity();

                pnlsup1.Visible = true;
                pnlsup2.Visible = false;
                lblsupportalert.Text = ds.Tables[0].Rows[0]["lbl"].ToString();
                if (ds.Tables[1].Rows[0]["CircleValue"].ToString() == "1")
                    pnl1.Visible = true;
                else if (ds.Tables[1].Rows[0]["CircleValue"].ToString() == "2")
                    pnl2.Visible = true;
                else if (ds.Tables[1].Rows[0]["CircleValue"].ToString() == "3")
                    pnl3.Visible = true;
                else if (ds.Tables[1].Rows[0]["CircleValue"].ToString() == "4")
                    pnl4.Visible = true;
                else if (ds.Tables[1].Rows[0]["CircleValue"].ToString() == "5")
                    pnl5.Visible = true;
                else if (ds.Tables[1].Rows[0]["CircleValue"].ToString() == "6")
                {
                    pnlsup1.Visible = false;
                    pnlsup2.Visible = true;
                }
            }
        }
       
    }
}