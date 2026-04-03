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
    public partial class DebitReportDateWise : System.Web.UI.Page
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
                grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            }
        }

        public void grid_fill(int page_number, int page_size)
        {
            DataTable dtgen = obj_master.Edit_GeneralSettings();

            DataSet ds = new DataSet();
            if (dtgen.Rows[0]["DebitorsReportFormat"].ToString() == "2")
                ds = obj_report.Debitors_ReportFromat2DateWise(page_number, page_size, txt_search.Text,
                    drptype.SelectedValue == "" ? 1 : Convert.ToInt32(drptype.SelectedValue),
                    txt_reg_Frm_date.SelectedDate,txt_reg_to_date.SelectedDate);
            else
                ds = obj_report.Debitors_ReportDateWise(page_number, page_size, txt_search.Text, 
                    drptype.SelectedValue == "" ? 1 : Convert.ToInt32(drptype.SelectedValue),
                    txt_reg_Frm_date.SelectedDate, txt_reg_to_date.SelectedDate);

            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();

                lblTotalAmount.Text = ds.Tables[1].Rows[0]["TotalAmount"].ToString();
            }
            else
            {
                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";

                lblTotalAmount.Text = "";
            }
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataTable dtgen = obj_master.Edit_GeneralSettings();

            DataSet ds = new DataSet();
            if (dtgen.Rows[0]["DebitorsReportFormat"].ToString() == "2")
                ds = obj_report.Debitors_ReportFromat2DateWisePdfExcel(
                    drptype.SelectedValue == "" ? 1 : Convert.ToInt32(drptype.SelectedValue),
                    txt_reg_Frm_date.SelectedDate, txt_reg_to_date.SelectedDate);
            else
                ds = obj_report.Debitors_ReportDateWisePdfExcel(
                    drptype.SelectedValue == "" ? 1 : Convert.ToInt32(drptype.SelectedValue),
                    txt_reg_Frm_date.SelectedDate, txt_reg_to_date.SelectedDate);

            DataTable dt = ds.Tables[0];
            DataTable dt_sum = ds.Tables[1];
            dt.Columns["Sl_No"].ColumnName = "Sl No.";
            if (dt.Rows.Count > 0)
            {
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DebitorsReport.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

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

                string style = @"<style> .textmode {  word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btnPdfOnClick(object sender, EventArgs e)
        {
            string url = "../Reports/DebitReportDateWisePdf.aspx?Ctype=" + (drptype.SelectedValue == "" ? "1" : drptype.SelectedValue)+
                "&FromDate=" + txt_reg_Frm_date.SelectedDate + "&ToDate=" + txt_reg_to_date.SelectedDate;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void lnkcust_Click(object sender, EventArgs e)
        {
            Control sendercontrol = (Control)sender;
            RepeaterItem itemrp = (RepeaterItem)sendercontrol.NamingContainer;
            HiddenField hdnCustomerId = (HiddenField)itemrp.FindControl("hdnCustomerId");

            string url = "../Reports/DebitReportDetailDateWise.aspx?CustomerId=" + Convert.ToInt32(hdnCustomerId.Value) +
                "&FromDate=" + txt_reg_Frm_date.SelectedDate + "&ToDate=" + txt_reg_to_date.SelectedDate;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }
        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
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
        #region Navigation

        //First Page
        protected void btn_first_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            Upd_List_Panel.Update();
        }

        //Previous Page
        protected void btn_prev_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) > 1)
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) - 1, Convert.ToInt32(drp_count.SelectedValue));
                Upd_List_Panel.Update();
            }
        }

        //Next Page
        protected void btn_next_OnClick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbl_page_number.Text) < Convert.ToInt32(hdn_last_page.Value))
            {
                grid_fill(Convert.ToInt32(lbl_page_number.Text) + 1, Convert.ToInt32(drp_count.SelectedValue));
                Upd_List_Panel.Update();
            }
        }

        //Last Page
        protected void btn_last_OnClick(object sender, EventArgs e)
        {

            grid_fill(Convert.ToInt32(hdn_last_page.Value), Convert.ToInt32(drp_count.SelectedValue));
            Upd_List_Panel.Update();
        }

        //Page Data Count
        protected void drp_count_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            Upd_List_Panel.Update();
        }

        #endregion
        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    int val = obj_common.Form_Previlage_Validation(127, Convert.ToInt32(hdn_user_id.Value));
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