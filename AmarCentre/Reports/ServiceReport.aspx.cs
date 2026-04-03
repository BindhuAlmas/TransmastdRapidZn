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
    public partial class ServiceReport : System.Web.UI.Page
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
            drp_employee.Items.Clear();
            drp_employee.DataSource = obj_report.Drp_Employee();
            drp_employee.DataTextField = "text";
            drp_employee.DataValueField = "value";
            drp_employee.DataBind();

            drp_Service.Items.Clear();
            drp_Service.DataSource = obj_report.Drp_Service(0);
            drp_Service.DataTextField = "text";
            drp_Service.DataValueField = "value";
            drp_Service.DataBind();

            drpCustomer.Items.Clear();
            drpCustomer.DataSource = obj_report.Drp_Customer();
            drpCustomer.DataTextField = "text";
            drpCustomer.DataValueField = "value";
            drpCustomer.DataBind();

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
        }

        protected void drpServiceTypeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtServiceType = new DataTable();
            dtServiceType.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpServiceType.CheckedItems)
            {
                DataRow dr = dtServiceType.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtServiceType.Rows.Add(dr);
            }

            drp_Service.Text = "";
            drp_Service.ClearSelection();
            drp_Service.Items.Clear();
            drp_Service.DataSource = obj_report.GetServiceListdt(dtServiceType);
            drp_Service.DataTextField = "text";
            drp_Service.DataValueField = "value";
            drp_Service.DataBind();
            UpdServicePanel.Update();
        }

        public void grid_fill(int page_number, int page_size)
        {
            DataTable dtServiceType = new DataTable();
            dtServiceType.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpServiceType.CheckedItems)
            {
                DataRow dr = dtServiceType.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtServiceType.Rows.Add(dr);
            }

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

            DataSet ds = obj_report.Get_userservice_Report(txt_reg_Frm_date.DbSelectedDate != null ? DateTime.ParseExact(CalDate(txt_reg_Frm_date), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                txt_reg_to_date.DbSelectedDate != null ? DateTime.ParseExact(CalDate(txt_reg_to_date), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                drp_employee.SelectedValue==""?0: Convert.ToInt32(drp_employee.SelectedValue), dtServiceType,dtService, dtCustomer, dtDepartment, dtInvoice,
                page_number, page_size);
            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = dt;
            rpt_list.DataBind();
            if (dt.Rows.Count > 0)
            {
                if (drp_employee.SelectedValue != "")
                {
                    lblTarget.Text = ds.Tables[1].Rows[0]["Name"].ToString();
                }
                lbl_page_info.Text = "Showing Results " + dt.Rows[0]["start_num"].ToString() + " - " + dt.Rows[dt.Rows.Count - 1]["Sl_No"].ToString() + " Out of " + dt.Rows[0]["current_count"].ToString() + " Records";
                hdn_last_page.Value = dt.Rows[0]["last_page"].ToString();
                lbl_page_number.Text = dt.Rows[0]["page_number"].ToString();
                hdn_total.Value = dt.Rows[0]["current_count"].ToString();
            }
            else
            {
                lblTarget.Text = "";

                lbl_page_info.Text = "Showing Results " + 0 + " - " + 0 + " Out of " + 0 + " Records";
                hdn_last_page.Value = "0";
                lbl_page_number.Text = "1";
                hdn_total.Value = "0";
            }
            Upd_Nav_Panel.Update();
            Upd_List_Panel.Update();
        }

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataTable dtServiceType = new DataTable();
            dtServiceType.Columns.Add("Id", typeof(string));
            foreach (RadComboBoxItem item in drpServiceType.CheckedItems)
            {
                DataRow dr = dtServiceType.NewRow();
                dr["Id"] = Convert.ToString(item.Value);
                dtServiceType.Rows.Add(dr);
            }

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

            DataSet ds = obj_report.Get_userservce_Excel(txt_reg_Frm_date.DbSelectedDate != null ? DateTime.ParseExact(CalDate(txt_reg_Frm_date), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                txt_reg_to_date.DbSelectedDate != null ? DateTime.ParseExact(CalDate(txt_reg_to_date), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                drp_employee.SelectedValue == "" ? 0 : Convert.ToInt32(drp_employee.SelectedValue),
                 dtServiceType, dtService, dtCustomer, dtDepartment, dtInvoice);
            DataTable dtEmp = ds.Tables[0];
            DataTable dt = ds.Tables[1];
            DataTable dt_sum = ds.Tables[2];

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=UserServiceReport.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                if (drp_employee.SelectedValue != "")
                {
                    if (dtEmp.Rows.Count > 0)
                    {
                        GridView g3 = new GridView();
                        g3.AllowPaging = false;
                        g3.DataSource = dtEmp;
                        g3.DataBind();
                        g3.HeaderRow.Style.Add("background-color", "#ccc");
                        for (int i = 0; i < g3.Rows.Count; i++)
                        {
                            //Apply text style to each Row
                            g3.Rows[i].Attributes.Add("class", "textmode");

                        }
                        g3.RenderControl(hw);

                    }
                }

                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                if (dt_sum.Rows.Count > 0)
                {
                    GridView g2 = new GridView();
                    g2.AllowPaging = false;
                    g2.DataSource = dt_sum;
                    g2.DataBind();
                    g2.HeaderRow.Style.Add("background-color", "#ccc");
                    for (int i = 0; i < g2.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        g2.Rows[i].Attributes.Add("class", "textmode");

                    }
                    g2.RenderControl(hw);

                }

                string style = @"<style> .textmode { mso-number-format:\@; word-wrap: break-word; } </style>";
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
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
                    int val = obj_common.Form_Previlage_Validation(90, Convert.ToInt32(hdn_user_id.Value));
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