using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using AmarCentre.BAL;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Telerik.Web.UI;

namespace AmarCentre.Reports
{
    public partial class Worksummary : System.Web.UI.Page
    {
        Master_Bal obj_master = new Master_Bal();
        System_Utilities obj_common = new System_Utilities();
        Report_Bal obj_report = new Report_Bal();
        dtClass dtc = new dtClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdn_user_id.Value = Session["User_Id"].ToString();
                previlage_check();
                txtToDate.SelectedDate = DateTime.Now;
                radfromdate.SelectedDate = DateTime.Now;
                fill_Drp_down();
            }
        }

        public void fill_Drp_down()
        {
            drpEmployee.Items.Clear();
            drpEmployee.DataSource = obj_report.Drp_Employee();
            drpEmployee.DataTextField = "text";
            drpEmployee.DataValueField = "value";
            drpEmployee.DataBind();

        }
        protected void btnGeneratePdf_OnClick(object sender, EventArgs e)
        {
            DataTable dtEmply = new DataTable();
            dtEmply.Columns.Add("Id", typeof(string));
            if (drpEmployee.CheckedItems.Count == 0)
            {
                foreach (RadComboBoxItem item in drpEmployee.Items)
                {
                    dtEmply.Rows.Add(Convert.ToInt32(item.Value));
                }
            }
            else
            {
                foreach (RadComboBoxItem item in drpEmployee.CheckedItems)
                {
                    dtEmply.Rows.Add(Convert.ToInt32(item.Value));
                }
            }

            dtc.setdtmultiple(dtEmply);

            string url = "../Reports/WorkSummaryPdf.aspx?ToDate=" + txtToDate.SelectedDate + "&FromDate=" + radfromdate.SelectedDate ;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btnGenerateDetPdf_OnClick(object sender, EventArgs e)
        {
            DataTable dtEmply = new DataTable();
            dtEmply.Columns.Add("Id", typeof(string));
            if (drpEmployee.CheckedItems.Count == 0)
            {
                foreach (RadComboBoxItem item in drpEmployee.Items)
                {
                    dtEmply.Rows.Add(Convert.ToInt32(item.Value));
                }
            }
            else
            {
                foreach (RadComboBoxItem item in drpEmployee.CheckedItems)
                {
                    dtEmply.Rows.Add(Convert.ToInt32(item.Value));
                }
            }

            dtc.setdtmultiple(dtEmply);

            string url = "../Reports/WorkSummaryDetailPdf.aspx?ToDate=" + txtToDate.SelectedDate + "&FromDate=" + radfromdate.SelectedDate;
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
                    int val = obj_common.Form_Previlage_Validation(77, Convert.ToInt32(hdn_user_id.Value));
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