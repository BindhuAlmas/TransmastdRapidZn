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
using AmarCentre.Masters;
using Telerik.Web;
using AmarCentre.Transactions;

namespace AmarCentre.Reports
{
    public partial class ServiceProfitStatement : System.Web.UI.Page
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

                txtFromDate.SelectedDate = DateTime.Now;
                txtToDate.SelectedDate = DateTime.Now;
            }
        }

        public void fill_Drp_down()
        {
            drpCustomer.Items.Clear();
            drpCustomer.DataSource = obj_report.Drp_Customer();
            drpCustomer.DataTextField = "text";
            drpCustomer.DataValueField = "value";
            drpCustomer.DataBind();

            drpService.Items.Clear();
            drpService.DataSource = obj_report.Drp_Service(0);
            drpService.DataTextField = "text";
            drpService.DataValueField = "value";
            drpService.DataBind();

            drpInvoice.Items.Clear();
            drpInvoice.DataSource = obj_report.Drp_Invoice();
            drpInvoice.DataTextField = "text";
            drpInvoice.DataValueField = "value";
            drpInvoice.DataBind();

            drpDepartment.Items.Clear();
            drpDepartment.DataSource = obj_report.Drp_Department();
            drpDepartment.DataTextField = "text";
            drpDepartment.DataValueField = "value";
            drpDepartment.DataBind();

            RadComboBoxItem CodeItem = new RadComboBoxItem();
            CodeItem.Text = "OtherDepartment";
            CodeItem.Value = "0";
            drpDepartment.Items.Insert(0, CodeItem);

            drpEmployee.Items.Clear();
            drpEmployee.DataSource = obj_report.Drp_Employee();
            drpEmployee.DataTextField = "text";
            drpEmployee.DataValueField = "value";
            drpEmployee.DataBind();

            drpVendor.Items.Clear();
            drpVendor.DataSource = obj_report.Drp_Vendor();
            drpVendor.DataTextField = "text";
            drpVendor.DataValueField = "value";
            drpVendor.DataBind();

            drpagent.Items.Clear();
            drpagent.DataSource = obj_report.fill_drp_Agent();
            drpagent.DataTextField = "text";
            drpagent.DataValueField = "value";
            drpagent.DataBind();
        }

        public void grid_fill(int page_number, int page_size)
        {
            DataTable dtEmply = new DataTable();
            dtEmply.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpEmployee.CheckedItems)
            {
                DataRow dr = dtEmply.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtEmply.Rows.Add(dr);
            }

            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpService.CheckedItems)
            {
                DataRow dr = dtService.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtService.Rows.Add(dr);
            }

            DataTable dtCustomer = new DataTable();
            dtCustomer.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpCustomer.CheckedItems)
            {
                DataRow dr = dtCustomer.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtCustomer.Rows.Add(dr);
            }

            DataTable dtDepartment = new DataTable();
            dtDepartment.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpDepartment.CheckedItems)
            {
                DataRow dr = dtDepartment.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtDepartment.Rows.Add(dr);
            }

            DataTable dtInvoice = new DataTable();
            dtInvoice.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpInvoice.CheckedItems)
            {
                DataRow dr = dtInvoice.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtInvoice.Rows.Add(dr);
            }

            DataSet ds = obj_report.ServiceProfitStatementNew(txtFromDate.SelectedDate, txtToDate.SelectedDate,
               dtCustomer, dtDepartment, dtService, dtInvoice, dtEmply, page_number, page_size, drpVendor.SelectedValue == "" ? (int?)null :
               Convert.ToInt32(drpVendor.SelectedValue), drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue));
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();

                lblinvAmount.Text = ds.Tables[1].Rows[0]["InvoiceAmount"].ToString();
                lbl_expnse.Text = ds.Tables[1].Rows[0]["Expense"].ToString();
                lbl_rectax.Text = ds.Tables[1].Rows[0]["ReceivedTax"].ToString();
                lbl_paytax.Text = ds.Tables[1].Rows[0]["PaidTax"].ToString();
                lbl_payabletax.Text = ds.Tables[1].Rows[0]["TaxPayable"].ToString();
                lbl_pft.Text = ds.Tables[1].Rows[0]["Profit"].ToString();
                lbl_commssion.Text = ds.Tables[1].Rows[0]["Commission"].ToString();
                lblvendorcommission.Text = ds.Tables[1].Rows[0]["Vendorcommission"].ToString();
                lblincentive.Text = ds.Tables[1].Rows[0]["Incentive"].ToString();
                lblagentpft.Text = ds.Tables[1].Rows[0]["AgentProfit"].ToString();
                lblnetpft.Text = ds.Tables[1].Rows[0]["NetProfit"].ToString();

            }
            else
            {
                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";

                lblinvAmount.Text = "";
                lbl_expnse.Text = "";
                lbl_rectax.Text = "";
                lbl_paytax.Text = "";
                lbl_payabletax.Text = "";
                lbl_pft.Text =  "";
                lbl_commssion.Text = lblvendorcommission.Text = lblincentive.Text = lblagentpft.Text = lblnetpft.Text = "";

            }
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataTable dtEmply = new DataTable();
            dtEmply.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpEmployee.CheckedItems)
            {
                DataRow dr = dtEmply.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtEmply.Rows.Add(dr);
            }

            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpService.CheckedItems)
            {
                DataRow dr = dtService.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtService.Rows.Add(dr);
            }

            DataTable dtCustomer = new DataTable();
            dtCustomer.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpCustomer.CheckedItems)
            {
                DataRow dr = dtCustomer.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtCustomer.Rows.Add(dr);
            }

            DataTable dtDepartment = new DataTable();
            dtDepartment.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpDepartment.CheckedItems)
            {
                DataRow dr = dtDepartment.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtDepartment.Rows.Add(dr);
            }

            DataTable dtInvoice = new DataTable();
            dtInvoice.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpInvoice.CheckedItems)
            {
                DataRow dr = dtInvoice.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtInvoice.Rows.Add(dr);
            }

            DataSet ds = obj_report.ServiceProfitStatementExcelNew(txtFromDate.SelectedDate, txtToDate.SelectedDate,
               dtCustomer, dtDepartment, dtService, dtInvoice, dtEmply, drpVendor.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpVendor.SelectedValue),
               drpagent.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpagent.SelectedValue));
            DataTable dt = ds.Tables[0];
            DataTable dt_sum = ds.Tables[1];

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ServiceProfitStatement.xls");
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

        protected void btnPdfOnClick(object sender, EventArgs e)
        {

            DataTable dtEmply = new DataTable();
            dtEmply.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpEmployee.CheckedItems)
            {
                DataRow dr = dtEmply.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtEmply.Rows.Add(dr);
            }

            DataTable dtService = new DataTable();
            dtService.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpService.CheckedItems)
            {
                DataRow dr = dtService.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtService.Rows.Add(dr);
            }

            DataTable dtCustomer = new DataTable();
            dtCustomer.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpCustomer.CheckedItems)
            {
                DataRow dr = dtCustomer.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtCustomer.Rows.Add(dr);
            }

            DataTable dtDepartment = new DataTable();
            dtDepartment.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpDepartment.CheckedItems)
            {
                DataRow dr = dtDepartment.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtDepartment.Rows.Add(dr);
            }

            DataTable dtInvoice = new DataTable();
            dtInvoice.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpInvoice.CheckedItems)
            {
                DataRow dr = dtInvoice.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtInvoice.Rows.Add(dr);
            }
             

            Session["dt_Emply"] = dtEmply;
            Session["dt_Service"] = dtService;
            Session["dt_Customer"] = dtCustomer;
            Session["dt_Department"] = dtDepartment;
            Session["dt_Invoice"] = dtInvoice;
          
            string url = "";
            url = "../Reports/ServiceProfitStatementpdf.aspx?FromDate=" + txtFromDate.SelectedDate + "&ToDate=" + txtToDate.SelectedDate +
               "&vendorId=" + drpVendor.SelectedValue + "&agentId=" + drpagent.SelectedValue;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "NewWindow", "window.open('" + url + "','_blank','height=600,width=900,status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );", true);
        }

        protected void btnSummary_OnClick(object sender, EventArgs e)
        {
            string url = "../Reports/ServiceProfitSummary.aspx?FromDate=" + txtFromDate.SelectedDate
        + "&ToDate=" + txtToDate.SelectedDate;
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
                    int val = obj_common.Form_Previlage_Validation(36, Convert.ToInt32(hdn_user_id.Value));
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