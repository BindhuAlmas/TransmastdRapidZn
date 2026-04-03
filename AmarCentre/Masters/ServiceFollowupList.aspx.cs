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

namespace AmarCentre.Masters
{
    public partial class ServiceFollowupList : System.Web.UI.Page
    {
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
                txtFromdate.SelectedDate = DateTime.Now;
                txtTodate.SelectedDate = DateTime.Now.AddDays(30);
                fill_Drp_down();
                grid_fill();
            }
        }

        public void fill_Drp_down()
        {
            drpCustomer.Items.Clear();
            drpCustomer.DataSource = obj_report.Drp_Customer();
            drpCustomer.DataTextField = "text";
            drpCustomer.DataValueField = "value";
            drpCustomer.DataBind();

            drpagent.Items.Clear();
            drpagent.DataSource = obj_report.fill_drp_Agent();
            drpagent.DataTextField = "text";
            drpagent.DataValueField = "value";
            drpagent.DataBind();
            drpagent.Text = "";

            drpService.Items.Clear();
            drpService.DataSource = obj_report.Drp_Service(0);
            drpService.DataTextField = "text";
            drpService.DataValueField = "value";
            drpService.DataBind();
           
        }



        public void grid_fill()
        {
            DataSet ds = obj_report.FollowupList(txtFromdate.SelectedDate, txtTodate.SelectedDate, 
                drpCustomer.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCustomer.SelectedValue),
                drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue),
                drpService.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpService.SelectedValue));
            rpt_list.DataSource = ds.Tables[0];
            rpt_list.DataBind();

            Upd_List_Panel.Update();
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataSet ds = obj_report.FollowupList(txtFromdate.SelectedDate, txtTodate.SelectedDate,
                 drpCustomer.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpCustomer.SelectedValue),
                 drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue),
                 drpService.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpService.SelectedValue));
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=FollowupExcel.xls");
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

        protected void btnPdfOnClick(object sender, EventArgs e)
        {

            string url = "";
            url = "../Reports/ServiceFollowupListpdf.aspx?FromDate=" + txtFromdate.SelectedDate + "&ToDate=" + txtTodate.SelectedDate +
               "&CustomerId=" + drpCustomer.SelectedValue + "&AgentId=" + drpagent.SelectedValue + "&ServiceId=" + drpService.SelectedValue;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }



        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill();
            pnl_filter.Visible = false;
            upd_nav_filter.Update();
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
                    DataTable dt = obj_common.Action_Previlage_Validation(67, Convert.ToInt32(hdn_user_id.Value));
                    if (dt.Rows[7][1].ToString() != "1")
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