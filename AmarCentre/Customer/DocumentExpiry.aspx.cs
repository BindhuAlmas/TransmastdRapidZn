using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;

namespace AmarCentre.Customer
{
    public partial class DocumentExpiry : System.Web.UI.Page
    {
        Report_Bal obj_report = new Report_Bal();
        System_Utilities obj_common = new System_Utilities();
        Master_Bal obj_master = new Master_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {

                hdn_user_id.Value = Session["User_Id"].ToString();
                lbl_User_name.Text = Session["User_Name"].ToString();
                hdn_user_id.Value = Session["User_Id"].ToString();
                txtFromdate.SelectedDate = DateTime.Now;
                txtTodate.SelectedDate = DateTime.Now.AddDays(30);
                fill_Document();
                previlage_check();
                grid_fill();

                //DataSet ds = obj_report.CustomerDocumentforC( (DateTime?)null,(DateTime?)null,Convert.ToInt32(hdn_user_id.Value));
                //DataTable dt = ds.Tables[0];

                //rpt_list.DataSource = dt;
                //rpt_list.DataBind();

            }
        }
        public void grid_fill()
        {

            DataSet ds = obj_report.CustomerDocumentforC(txtFromdate.SelectedDate, txtTodate.SelectedDate, Convert.ToInt32(hdn_user_id.Value), drp_doc.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_doc.SelectedValue));
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            Upd_List_Panel.Update();
        }

        public void fill_Document()
        {
            drp_doc.Items.Clear();
            DataTable dt = obj_master.fill_drp_DocType();
            drp_doc.DataSource = dt;
            drp_doc.DataTextField = "Text";
            drp_doc.DataValueField = "Value";
            drp_doc.DataBind();
            drp_doc.Text = "";
        }

        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill();
        }
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {

                    int val = obj_common.CustomerForm_Previlage_Validation(6, Convert.ToInt32(hdn_user_id.Value));
                    if (val == 0)
                    {
                        Response.Redirect("../Landing.aspx");
                    }

                }
                else
                {
                    Response.Redirect("../Landing.aspx");
                }
            }
            catch
            {
                Response.Redirect("../Landing.aspx");
            }
        }
    }
}