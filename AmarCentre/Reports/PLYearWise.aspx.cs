using System;
using System.Web.UI;
using System.Data;
using AmarCentre.BAL;
using Telerik.Web.UI;

namespace AmarCentre.Reports
{
    public partial class PLYearWise : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                fillYear();
                previlage_check();
            }
        }

        public void fillYear()
        {
            RadComboBoxItem CodeItem;
            int lastyear = DateTime.Now.Year;
            for (int date = lastyear; date >= 2018; date--)
            {
                CodeItem = new RadComboBoxItem();
                CodeItem.Text = date.ToString();
                CodeItem.Value = date.ToString();
                drpYear.Items.Add(CodeItem);
            }
        }

        public void grid_fill()
        {

            DataSet ds = obj_report.ProfitLossYearWise(Convert.ToInt32(drpYear.SelectedValue));
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();

            if (ds.Tables[1].Rows.Count > 0)
                lbltotal.Text = ds.Tables[1].Rows[0]["TotalProfit"].ToString();
            else
                lbltotal.Text = "";

            Upd_List_Panel.Update();
        }

        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill();
            pnl_filter.Visible = false;
            upd_nav_filter.Update();
        }

        protected void btnGeneratePdf_OnClick(object sender, EventArgs e)
        {
            string url = "../Reports/PLYearWisePdf.aspx?Year=" + Convert.ToInt32(drpYear.SelectedValue);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btn_filter_OnClick(object sender, EventArgs e)
        {
            if (pnl_filter.Visible == true)
            {
                pnl_filter.Visible = false;
            }
            else
            {
                pnl_filter.Visible = true;
            }
            upd_nav_filter.Update();
        }

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    int val = obj_common.Form_Previlage_Validation(109, Convert.ToInt32(hdn_user_id.Value));
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