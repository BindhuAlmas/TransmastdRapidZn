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
    public partial class AttendenceReport : System.Web.UI.Page
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
            drp_employee.Items.Clear();
            drp_employee.DataSource = obj_report.Drp_Employee();
            drp_employee.DataTextField = "text";
            drp_employee.DataValueField = "value";
            drp_employee.DataBind();

            //month
            RadComboBoxItem CodeItem;
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "January";
            CodeItem.Value = "1";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "February";
            CodeItem.Value = "2";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "March";
            CodeItem.Value = "3";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "April";
            CodeItem.Value = "4";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "May";
            CodeItem.Value = "5";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "June";
            CodeItem.Value = "6";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "July";
            CodeItem.Value = "7";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "August";
            CodeItem.Value = "8";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "September";
            CodeItem.Value = "9";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "October";
            CodeItem.Value = "10";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "November";
            CodeItem.Value = "11";
            drpMonth.Items.Add(CodeItem);
            CodeItem = new RadComboBoxItem();
            CodeItem.Text = "December";
            CodeItem.Value = "12";
            drpMonth.Items.Add(CodeItem);

            //year
            RadComboBoxItem CodeItem1;
            int lastyear = DateTime.Now.Year + 5;
            for (int date = 2018; date <= lastyear; date++)
            {
                CodeItem1 = new RadComboBoxItem();
                CodeItem1.Text = date.ToString();
                CodeItem1.Value = date.ToString();
                drpYear.Items.Add(CodeItem1);
            }
        }

        public void grid_fill(int page_number, int page_size)
        {
            DataSet ds = obj_report.Get_Attendence_Report(drpMonth.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpMonth.SelectedValue),
                drpYear.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpYear.SelectedValue),
                drp_employee.SelectedValue==""?(int?)null:Convert.ToInt32(drp_employee.SelectedValue), page_number, page_size);

            DataTable dt = ds.Tables[0];
            rpt_list.DataSource = ds.Tables[0];
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

        protected void btn_excel_OnClick(object sender, EventArgs e)
        {
            DataSet ds = obj_report.Get_Attendence_Excel(drpMonth.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpMonth.SelectedValue),
                drpYear.SelectedValue == "" ? (int?)null : Convert.ToInt32(drpYear.SelectedValue),
                drp_employee.SelectedValue == "" ? (int?)null : Convert.ToInt32(drp_employee.SelectedValue));
            DataTable dt = ds.Tables[0];

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
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=AttendenceReport.xls");
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

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
                    int val = obj_common.Form_Previlage_Validation(57, Convert.ToInt32(hdn_user_id.Value));
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