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
    public partial class ServiceCompletionRep_Nav : System.Web.UI.Page
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
                txt_reg_Frm_date.SelectedDate = DateTime.Now;
                txt_reg_to_date.SelectedDate = DateTime.Now;
            }
        }

        public void fill_Drp_down()
        {
            drpDepartment.Items.Clear();
            drpDepartment.DataSource = obj_report.Drp_Department();
            drpDepartment.DataTextField = "text";
            drpDepartment.DataValueField = "value";
            drpDepartment.DataBind();

            drp_Service.Items.Clear();
            drp_Service.DataSource = obj_report.Drp_ServiceFilterByDep(fill_get_department());
            drp_Service.DataTextField = "text";
            drp_Service.DataValueField = "value";
            drp_Service.DataBind();

            drp_Cust.Items.Clear();
            drp_Cust.DataSource = obj_report.Drp_Customer();
            drp_Cust.DataTextField = "text";
            drp_Cust.DataValueField = "value";
            drp_Cust.DataBind();
            drp_Cust.Text = "";

            drpagent.Items.Clear();
            drpagent.DataSource = obj_report.fill_drp_Agent();
            drpagent.DataTextField = "text";
            drpagent.DataValueField = "value";
            drpagent.DataBind();
            drpagent.Text = "";

            drpEmployee.Items.Clear();
            drpEmployee.DataSource = obj_report.Drp_Employee();
            drpEmployee.DataTextField = "text";
            drpEmployee.DataValueField = "value";
            drpEmployee.DataBind();

            DataTable dtgen = obj_master.Edit_GeneralSettings();
            pnlstatus.Visible = dtgen.Rows[0]["IsDisplaySCStatus"].ToString() == "1" ? true : false;

            drpServiceStatus.Items.Clear();
            drpServiceStatus.DataSource = obj_report.DrpServiceStatuslist();
            drpServiceStatus.DataTextField = "Name";
            drpServiceStatus.DataValueField = "ID";
            drpServiceStatus.DataBind();

        }

        protected void drpDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            drp_Service.ClearCheckedItems();
            drp_Service.Text = "";

            drp_Service.Items.Clear();
            drp_Service.DataSource = obj_report.Drp_ServiceFilterByDep(fill_get_department());
            drp_Service.DataTextField = "text";
            drp_Service.DataValueField = "value";
            drp_Service.DataBind();

            UpdServicePanel.Update();
        }

        public DataTable fill_get_Service()
        {
            DataTable dtstatus = new DataTable();
            dtstatus.Columns.Add("StatusId");
            if (dtstatus.Rows.Count > 0)
                dtstatus.Rows.Clear();
            foreach (RadComboBoxItem item in drp_Service.CheckedItems)
            {
                DataRow dr = dtstatus.NewRow();
                dr["StatusId"] = Convert.ToString(item.Value);
                dtstatus.Rows.Add(dr);
            }
            return dtstatus;
        }
        public DataTable fill_get_department()
        {
            DataTable dtstatus = new DataTable();
            dtstatus.Columns.Add("StatusId");
            if (dtstatus.Rows.Count > 0)
                dtstatus.Rows.Clear();
            foreach (RadComboBoxItem item in drpDepartment.CheckedItems)
            {
                DataRow dr = dtstatus.NewRow();
                dr["StatusId"] = Convert.ToString(item.Value);
                dtstatus.Rows.Add(dr);
            }
            return dtstatus;
        }
        public DataTable fill_get_customer()
        {
            DataTable dtstatus = new DataTable();
            dtstatus.Columns.Add("StatusId");
            if (dtstatus.Rows.Count > 0)
                dtstatus.Rows.Clear();
            foreach (RadComboBoxItem item in drp_Cust.CheckedItems)
            {
                DataRow dr = dtstatus.NewRow();
                dr["StatusId"] = Convert.ToString(item.Value);
                dtstatus.Rows.Add(dr);
            }
            return dtstatus;
        }
        public DataTable fill_get_bank()
        {
            DataTable dtstatus = new DataTable();
            dtstatus.Columns.Add("StatusId");
            if (dtstatus.Rows.Count > 0)
                dtstatus.Rows.Clear();
            foreach (RadComboBoxItem item in drp_Bank.CheckedItems)
            {
                DataRow dr = dtstatus.NewRow();
                dr["StatusId"] = Convert.ToString(item.Value);
                dtstatus.Rows.Add(dr);
            }
            return dtstatus;
        }

        public void grid_fill(int page_number, int page_size)
        {
            DataTable dt_service = fill_get_Service();
            DataTable dt_cust = fill_get_customer();
            DataTable dt_bank = fill_get_bank();
            DataTable dt_department= fill_get_department();

            DataTable dt = new DataTable();
                dt = obj_report.Get_SC_Report(txt_reg_Frm_date.SelectedDate,txt_reg_to_date.SelectedDate,
                  dt_service, dt_cust,dt_bank, dt_department,
                  drpEmployee.SelectedValue==""?(int?)null:Convert.ToInt32(drpEmployee.SelectedValue),
                  drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue),
                  drpServiceStatus.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpServiceStatus.SelectedValue),
                  txt_search.Text,   // ADD THIS
                  page_number, page_size);
          
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";
            }
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        // ✅ THIS IS THE MISSING METHOD - ADD THIS
        protected void txt_search_OnTextChanged(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            Upd_List_Panel.Update();
        }
        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataTable dt_service = fill_get_Service();
            DataTable dt_cust = fill_get_customer();
            DataTable dt_bank = fill_get_bank();
            DataTable dt_department = fill_get_department();
            DataTable dt = new DataTable();
            dt = obj_report.Get_SC_Excel(txt_reg_Frm_date.SelectedDate, txt_reg_to_date.SelectedDate,
              dt_service, dt_cust, dt_bank, dt_department,
              drpEmployee.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpEmployee.SelectedValue),
               drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue),
               drpServiceStatus.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpServiceStatus.SelectedValue),
               txt_search.Text);

            if (dt.Rows.Count > 0)
            {
                StringWriter sw = obj_common.ExportToExcel(dt, "ServiceCompletionDetail");

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                HttpContext.Current.Response.Write(style);
                Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void btnPdfOnClick(object sender, EventArgs e)
        {
           
            DataTable dt_service = fill_get_Service();
            DataTable dt_cust = fill_get_customer();
            DataTable dt_bank = fill_get_bank();
            DataTable dt_department = fill_get_department();

            Session["dt_service"] = dt_service;
            Session["dt_cust"] = dt_cust;
            Session["dt_bank"] = dt_bank;
            Session["dt_department"] = dt_department;

            string url = "";
            url = "../Reports/ServiceCompletionpdf.aspx?FromDate=" + txt_reg_Frm_date.SelectedDate + "&ToDate=" + txt_reg_to_date.SelectedDate +
               "&employeeId=" + drpEmployee.SelectedValue + "&agentId=" + drpagent.SelectedValue + "&ServiceStatusId=" + drpServiceStatus.SelectedValue;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btn_search_OnClick(object sender, EventArgs e)
        {
            grid_fill(1, Convert.ToInt32(drp_count.SelectedValue));
            pnl_filter.Visible = false;
            upd_nav_filter.Update();
            Upd_List_Panel.Update();
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

                    int val = obj_common.Form_Previlage_Validation(22, Convert.ToInt32(hdn_user_id.Value));
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

        public string CalDate(Telerik.Web.UI.RadDatePicker Dates)
        {
            string month = Dates.SelectedDate.Value.Month.ToString();
            if (month != "10" && month != "11" && month != "12")
                month = "0" + month;
            string day = Dates.SelectedDate.Value.Day.ToString();
            for (int i = 0; i < 10; i++)
            {
                if (Convert.ToInt32(day) == i)
                    day = "0" + day;
            }
            string year = Dates.SelectedDate.Value.Year.ToString();
            return day + '/' + month + '/' + year;
        }

    }
}