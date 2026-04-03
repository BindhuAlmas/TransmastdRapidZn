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
    public partial class Pendingservice : System.Web.UI.Page
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
                fill_Drp_down();
              
            }
        }

        public void fill_Drp_down()
        {
            drp_Service.Items.Clear();
            drp_Service.DataSource = obj_report.Drp_Service(0);
            drp_Service.DataTextField = "text";
            drp_Service.DataValueField = "value";
            drp_Service.DataBind();

            drp_Customer.Items.Clear();
            drp_Customer.DataSource = obj_report.Drp_Customer();
            drp_Customer.DataTextField = "text";
            drp_Customer.DataValueField = "value";
            drp_Customer.DataBind();

            drpDepartment.Items.Clear();
            drpDepartment.DataSource = obj_report.Drp_Department();
            drpDepartment.DataTextField = "text";
            drpDepartment.DataValueField = "value";
            drpDepartment.DataBind();

            drpagent.Items.Clear();
            drpagent.DataSource = obj_report.fill_drp_Agent();
            drpagent.DataTextField = "text";
            drpagent.DataValueField = "value";
            drpagent.DataBind();
            drpagent.Text = "";

            drpApplicant.Items.Clear();
            drpApplicant.DataSource = obj_report.DrpApplicantPending();
            drpApplicant.DataTextField = "Particulars";
            drpApplicant.DataValueField = "Particulars";
            drpApplicant.DataBind();
            drpApplicant.Text = "";

            DataTable dtgen = obj_master.Edit_GeneralSettings();
            pnlstatus.Visible = dtgen.Rows[0]["IsDisplaySCStatus"].ToString() == "1" ? true : false;

            drpServiceStatus.Items.Clear();
            drpServiceStatus.DataSource = obj_report.DrpServiceStatuslist();
            drpServiceStatus.DataTextField = "Name";
            drpServiceStatus.DataValueField = "ID";
            drpServiceStatus.DataBind();
        }

        public void grid_fill(int page_number, int page_size)
        {
          
            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drp_Service.CheckedItems)
            {
                DataRow dr = dtService.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtService.Rows.Add(dr);
            }
            DataTable dt_cust = fill_get_customer();


            DataSet ds = obj_report.PendingserviceList(txt_reg_Frm_date.SelectedDate, txt_reg_to_date.SelectedDate,
                dtService, dt_cust, drpDepartment.SelectedValue==""?(int?)null:Convert.ToInt32(drpDepartment.SelectedValue),
                drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue), 
                page_number, page_size,drpServiceStatus.SelectedValue==""?(int?)null:Convert.ToInt32(drpServiceStatus.SelectedValue),
                drpApplicant.SelectedValue);
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();
                lbltotal.Text = ds.Tables[1].Rows[0][0].ToString();
            }
            else
            {
                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";
                lbltotal.Text = "";
            }
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        public DataTable fill_get_customer()
        {
            DataTable dtstatus = new DataTable();
            dtstatus.Columns.Add("StatusId");
            if (dtstatus.Rows.Count > 0)
                dtstatus.Rows.Clear();
            foreach (RadComboBoxItem item in drp_Customer.CheckedItems)
            {
                DataRow dr = dtstatus.NewRow();
                dr["StatusId"] = Convert.ToString(item.Value);
                dtstatus.Rows.Add(dr);
            }
            return dtstatus;
        }

        protected void drp_Customer_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            DataTable dt_cust = fill_get_customer();

            drpApplicant.ClearSelection();
            drpApplicant.Text = "";
            drpApplicant.Items.Clear();
            drpApplicant.DataSource = obj_report.Drp_Applicantdt_cust(dt_cust);
            drpApplicant.DataTextField = "Particulars";
            drpApplicant.DataValueField = "Particulars";
            drpApplicant.DataBind();

            updApplicant.Update();
        }
        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
          
            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drp_Service.CheckedItems)
            {
                DataRow dr = dtService.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtService.Rows.Add(dr);
            }
            DataTable dt_cust = fill_get_customer();

            DataSet ds = obj_report.PendingserviceExcelPdf(txt_reg_Frm_date.SelectedDate, txt_reg_to_date.SelectedDate,
                dtService, dt_cust,drpDepartment.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpDepartment.SelectedValue),
                drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue)
                , drpServiceStatus.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpServiceStatus.SelectedValue),
                drpApplicant.SelectedValue);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Pendingservice.xls");
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
            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drp_Service.CheckedItems)
            {
                DataRow dr = dtService.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtService.Rows.Add(dr);
            }

            DataTable dtCustomer = new DataTable();
            dtCustomer.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drp_Customer.CheckedItems)
            {
                DataRow dr = dtCustomer.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtCustomer.Rows.Add(dr);
            }

            Session["dtService"] = dtService;
            Session["dt_cust"] = dtCustomer;

            string url = "";
            url = "../Reports/Pendingservicepdf.aspx?FromDate=" + txt_reg_Frm_date.SelectedDate + "&ToDate=" + txt_reg_to_date.SelectedDate +
               "&departmentId=" + drpDepartment.SelectedValue + "&agentId=" + drpagent.SelectedValue+"&ServiceStatusId="+ drpServiceStatus.SelectedValue+
               "&Applicant=" + drpApplicant.SelectedValue;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            pnl_filter.Visible = false;
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
        //Check Privilege
        public void previlage_check()
        {
            try
            {
                if (hdn_user_id.Value != null)
                {
                    int val = obj_common.Form_Previlage_Validation(116, Convert.ToInt32(hdn_user_id.Value));
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