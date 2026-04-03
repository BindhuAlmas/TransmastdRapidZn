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
    public partial class DayBookNav : System.Web.UI.Page
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
            string url = "../Reports/DayBookPdf.aspx?&ToDate=" + txtToDate.SelectedDate+ "&FromDate=" + radfromdate.SelectedDate + "&userid=" + (drpEmployee.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmployee.SelectedValue));
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {

            DataSet ds = obj_report.DayBookdetail(radfromdate.SelectedDate,txtToDate.SelectedDate, drpEmployee.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmployee.SelectedValue));
            DataTable dt = ds.Tables[0];
            DataTable dt_sum = ds.Tables[1];

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DayBook.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.RenderControl(hw);

                if (dt_sum.Rows.Count > 0)
                {
                    GridView g2 = new GridView();
                    g2.AllowPaging = false;
                    g2.DataSource = dt_sum;
                    g2.DataBind();
                    g2.HeaderRow.Style.Add("background-color", "#ccc");
                    g2.RenderControl(hw);
                }

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

        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    int val = obj_common.Form_Previlage_Validation(118, Convert.ToInt32(hdn_user_id.Value));
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