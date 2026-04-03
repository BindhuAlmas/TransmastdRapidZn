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

namespace AmarCentre.CRM
{
    public partial class Tasks : System.Web.UI.Page
    {
        Master_Bal masterBAL = new Master_Bal();
        System_Utilities systemUtilities = new System_Utilities();
        Transaction_Bal TransBal = new Transaction_Bal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_Id"] == null)
            {
                Response.Redirect("~/Landing.aspx");
            }
            if (!IsPostBack)
            {
                hdnUserId.Value = Session["User_Id"].ToString();
                FillDropdown();
                CheckPrivilege();
            }
        }

        public void FillDropdown()
        {
            DataSet ds = TransBal.drpforLead();

            drpEmployee.DataSource = masterBAL.DrpLeadEmployee(Convert.ToInt32(hdnUserId.Value));
            drpEmployee.DataValueField = "Value";
            drpEmployee.DataTextField = "Text";
            drpEmployee.DataBind();

            drpSource.DataSource = masterBAL.DrpLeadSource();
            drpSource.DataValueField = "Value";
            drpSource.DataTextField = "Text";
            drpSource.DataBind();

            drpPriority.DataSource = masterBAL.DrpPriority();
            drpPriority.DataValueField = "Value";
            drpPriority.DataTextField = "Text";
            drpPriority.DataBind();

            drpJurisdiction.DataSource = ds.Tables[6]; //segment
            drpJurisdiction.DataValueField = "Value";
            drpJurisdiction.DataTextField = "Text";
            drpJurisdiction.DataBind();

            drpActivity.DataSource = masterBAL.DrpActivity();
            drpActivity.DataValueField = "Value";
            drpActivity.DataTextField = "Text";
            drpActivity.DataBind();

            drpStatus.DataSource = masterBAL.DrpStatus();
            drpStatus.DataValueField = "Value";
            drpStatus.DataTextField = "Text";
            drpStatus.DataBind();
        }

        public void grid_fill()
        {
            DataSet ds = masterBAL.getCRMDashboard(txtFromdate.SelectedDate, txtTodate.SelectedDate, drpSource.SelectedValue == "" ? (int?)null :
                Convert.ToInt32(drpSource.SelectedValue), drpEmployee.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmployee.SelectedValue),
                drpJurisdiction.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpJurisdiction.SelectedValue),
                drpPriority.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpPriority.SelectedValue),
                drpStatus.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpStatus.SelectedValue), drpActivity.SelectedValue,
                Convert.ToInt32(hdnUserId.Value));
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();

            Upd_List_Panel.Update();
        }

        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill();
            pnl_filter.Visible = false;
            upd_nav_filter.Update();
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataSet ds = masterBAL.getCRMDashboard(txtFromdate.SelectedDate, txtTodate.SelectedDate, drpSource.SelectedValue == "" ? (int?)null :
               Convert.ToInt32(drpSource.SelectedValue), drpEmployee.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmployee.SelectedValue),
               drpJurisdiction.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpJurisdiction.SelectedValue),
               drpPriority.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpPriority.SelectedValue),
               drpStatus.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpStatus.SelectedValue),drpActivity.SelectedValue,
               Convert.ToInt32(hdnUserId.Value));
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Tasklist.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.RenderControl(hw);

                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
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

        public void CheckPrivilege()
        {
            try
            {
                if (hdnUserId.Value != null)
                {

                    int val = systemUtilities.Form_Previlage_Validation(140, Convert.ToInt32(hdnUserId.Value));
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